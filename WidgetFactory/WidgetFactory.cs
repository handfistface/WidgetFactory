using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WidgetFactory
{
    /// <summary>
    /// public partial class WidgetFactory
    /// Author: John Kirschner
    /// Date: 07-28-2018
    /// Details:
    ///     The main GUI for the user to create the factories
    ///     For QOL the last loaded path will be stored for future use
    /// Assumptions and known issues:
    ///     Input files must be stored in UTF-8 encoding
    ///     Input files are a pre-determined format as defined in the example files provided
    ///     Object construction files must contain at least one component, otherwise the object will not be processed
    ///     Some assumptions about the file system are made, AppData needs to be accessible to store some data, the file system will assume that the C: drive is present
    /// </summary>
    public partial class WidgetFactory : Form
    {
        #region WidgetFactory Variables
        private string s_LastLoadedPath;        //holds the last loaded path for the files, based off the relevant settings file variable
        private List<FactoryObject> lfo_Components;     //factory object list which holds all of the components
        private Queue<FactoryObject> qfo_BuildQueue;        //factory object queue which contains the items that will be built, based off a file
        #endregion WidgetFactory Variables

        #region public WidgetFactory()
        /// <summary>
        /// public WidgetFactory()
        /// The constructor for the widgetfactory application
        /// </summary>
        public WidgetFactory()
        {
            InitializeComponent();
            s_LastLoadedPath = Properties.Settings.Default.s_LastLoadedPath;        //load up the settings path in the class variable
            Util.Init(rtxt_WidgFact);       //setup the util for printing to the local widget factory text box
            lfo_Components = new List<FactoryObject>();     //create a new list to hold the components
            qfo_BuildQueue = new Queue<FactoryObject>();        //create a new queue to hold the build order
            //rtxt_WidgFact.TextChanged += Rtxt_WidgFact_TextChanged;       //disable the scrolling, it makes the rich text box jumpy and kind of hard to tell what's happening
        }
        #endregion public WidgetFactory()

        #region private void Rtxt_WidgFact_TextChanged(object sender, EventArgs e)
        /// <summary>
        /// private void Rtxt_WidgFact_TextChanged(object sender, EventArgs e)
        /// Handles the text changing event of the rich text box
        /// Will scroll to the bottom automatically so the user does not have to interact with the text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rtxt_WidgFact_TextChanged(object sender, EventArgs e)
        {
            //determine if a cross thread access is necessary
            if(rtxt_WidgFact.InvokeRequired)
            {
                //then a cross thread access is going to happen, make sure its thread safe
                rtxt_WidgFact.Invoke((MethodInvoker)delegate
                {
                    //rtxt_WidgFact.SelectionStart = rtxt_WidgFact.Text.Length;       //set the cursor to the end of the text of this box
                    rtxt_WidgFact.ScrollToCaret();      //scroll to the end of the text box
                });
            }
            else
            {
                //otherwise a cross thread access is not necessary, just access the ui control normally
                //rtxt_WidgFact.SelectionStart = rtxt_WidgFact.Text.Length;       //set the cursor to the end of the text of this box
                rtxt_WidgFact.ScrollToCaret();      //scroll to the end of the text box
            }
        }
        #endregion private void Rtxt_WidgFact_TextChanged(object sender, EventArgs e)

        #region private void btn_ObjConst_Click(object sender, EventArgs e)
        /// <summary>
        /// private void btn_ObjConst_Click(object sender, EventArgs e)
        /// The method that gets called whenever the user clicks on the Widget Construction button
        /// I call it the object construction because it makes more sense to me
        /// Opens up a file dialog which lets the user select the file to load
        /// Once the user loads the file then the Ingestion class will consume the file and generate a stack for the method to process
        /// If there are duplicate components then they are not added
        /// If there are components changing to a buildable component then it will be removed from the component list view and added to the buildable component list view, this is applicable when loading multiple files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ObjConst_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd_ObjConst = new OpenFileDialog();     //open file dialog will display a file selection for the user
            //if the last loaded path was not 
            if (s_LastLoadedPath != "")
            {
                ofd_ObjConst.InitialDirectory = s_LastLoadedPath;       //setup the last loaded path to the class wide variable
            }
            else
            {
                //otherwise the last loaded path was not used, just use the base C drive to load the path
                ofd_ObjConst.InitialDirectory = "C:\\";
            }
            ofd_ObjConst.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";        //setup the filter to look for specific files
            ofd_ObjConst.FilterIndex = 0; ;     //look at the correct index for the filter
            ofd_ObjConst.RestoreDirectory = true;
            ofd_ObjConst.Multiselect = false;       //do not allow the user to select multiple files
            if(ofd_ObjConst.ShowDialog() == DialogResult.OK)
            {
                //then the file was obtained for the user
                string s_FileName = ofd_ObjConst.FileName;
                FileInfo fi_ObjConst = new FileInfo(s_FileName);        //get the file info
                //determine if the file exists
                if (fi_ObjConst.Exists)
                {
                    //then the file exists, it is safe to ingest it
                    s_LastLoadedPath = fi_ObjConst.Directory.FullName;      //get the last loaded path
                    Properties.Settings.Default.s_LastLoadedPath = s_LastLoadedPath;        //update the application wide persistant variable
                    Properties.Settings.Default.Save();     //update the global variable
                    txt_ObjConst.Text = s_LastLoadedPath + "\\" + fi_ObjConst.Name;       //setup the text box
                    Stack<FactoryObject> sfo_ObjectConstruction;        //a stack which holds the object construction for the user
                    //be safe while processing the file, no telling how the user has their file system setup...
                    try
                    {
                        sfo_ObjectConstruction = Ingestion.IngestObjectFile(fi_ObjConst);       //setup the stack so that the objects can be ingested 
                    }
                    catch(Exception)
                    {
                        //print out the exception
                        Util.rtxtWriteLine("Error encountered while processing the requested file. Aborting file import");
                        //usually for exceptions I try to log what has occured, this is a weird case in that there's no logging setup with this program
                        return;
                    }
                    //if we've made it this far then the stack was initialized
                    //look through the list of components and determine if we are adding duplicates
                    foreach(FactoryObject fo in sfo_ObjectConstruction)
                    {
                        bool b_AlreadyExists = false;
                        //loop through each factory object in the class list
                        foreach(FactoryObject fo_Cmp in lfo_Components)
                        {
                            //if the factory object in the new stack matches the names of any factory object in the class list...
                            if(fo_Cmp.s_Name == fo.s_Name)
                            {
                                //then this is the same item in the stack
                                b_AlreadyExists = true;
                                break;
                            }
                        }
                        //determine if the item was found previously in the class list of factory objects...
                        if(b_AlreadyExists)
                        {
                            //then the item was previously existing in the list
                            //determine if the item is a component or a buildable object
                            if(fo.GetComponents().Count > 1)
                            {
                                //then this is a buildable object
                                //determine if the item was previously in the components list
                                //have to loop through everything, the method Contains requires an actual object comparison, not just a text comparison
                                foreach(ListViewItem lvi in lstv_Components.Items)
                                {
                                    //determine if hte list view item text is the same as the factory object name
                                    if(lvi.Text == fo.s_Name)
                                    {
                                        //then this item was previously in the list
                                        lstv_Components.Items.Remove(lvi);      //remove the item from the list of components
                                        lstv_BuildableObjects.Items.Add(new ListViewItem(fo.s_Name));       //add the item to the list of buildable items
                                        break;      //break from the loop
                                    }
                                }
                            }
                            else
                            {
                                //otherwise the item that we are viewing is a component
                                //if it was a component then just leave it alone for the time being, unless I can think of something specific to do
                            }
                        }
                        else
                        {
                            //otherwise the item did not exist in the list previously
                            //determine if this is going to be a craftable object or a regular component, view the components for the factory object
                            if(fo.GetComponents().Count > 1)
                            {
                                //then this item will be craftable, add it to the buildable items list view
                                lstv_BuildableObjects.Items.Add(new ListViewItem(fo.s_Name));
                            }
                            else
                            {
                                //otherwise this is just a component, add it to the components list view
                                lstv_Components.Items.Add(new ListViewItem(fo.s_Name));
                            }
                            //add the item to the list of factory objects
                            lfo_Components.Add(fo);
                        }
                    }
                    Util.rtxtWriteLine("Successfully updated the build order components");
                }
                else
                {
                    //otherwise the user selected a file that does not exist
                    Util.rtxtWriteLine("Please select a file that exists for the widget construction definition file.");
                }
            }

        }
        #endregion private void btn_ObjConst_Click(object sender, EventArgs e)

        #region private void btn_BuildOrder_Click(object sender, EventArgs e)
        /// <summary>
        /// private void btn_BuildOrder_Click(object sender, EventArgs e)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BuildOrder_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd_BuildOrder = new OpenFileDialog();     //open file dialog will display a file selection for the user
            //if the last loaded path was not 
            if (s_LastLoadedPath != "")
            {
                ofd_BuildOrder.InitialDirectory = s_LastLoadedPath;       //setup the last loaded path to the class wide variable
            }
            else
            {
                //otherwise the last loaded path was not used, just use the base C drive to load the path
                ofd_BuildOrder.InitialDirectory = "C:\\";
            }
            ofd_BuildOrder.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";        //setup the filter to look for specific files
            ofd_BuildOrder.FilterIndex = 0; ;     //look at the correct index for the filter
            ofd_BuildOrder.RestoreDirectory = true;
            ofd_BuildOrder.Multiselect = false;       //do not allow the user to select multiple files
            if (ofd_BuildOrder.ShowDialog() == DialogResult.OK)
            {
                string s_FileName = ofd_BuildOrder.FileName;        //get the file name of the item that was returned
                FileInfo fi_BuildOrder = new FileInfo(s_FileName);      //get the file info which describes the file that will be accessed
                if(fi_BuildOrder.Exists)
                {
                    //then the file exists, it is safe to ingest it
                    s_LastLoadedPath = fi_BuildOrder.Directory.FullName;      //get the last loaded path
                    Properties.Settings.Default.s_LastLoadedPath = s_LastLoadedPath;        //update the application wide persistant variable
                    Properties.Settings.Default.Save();     //update the global variable
                    txt_BuildOrder.Text = s_LastLoadedPath + "\\" + fi_BuildOrder.Name;       //setup the text box
                    Queue<FactoryObject> qfo_NewBuildQueue;     //a queue which will hold the new ingested file contents
                    //now the file can be consumed...
                    //be safe and wrap it in a try/catch statement
                    try
                    {
                        qfo_NewBuildQueue = Ingestion.IngestBuildOrderFile(fi_BuildOrder);
                    }
                    catch(Exception)
                    {
                        //catch a generic exception.
                        Util.rtxtWriteLine("Error while processing build order file. Exception encountered while parsing file. Please provide a file in the correct format");
                        //usually I log the exception, however I am not providing logging for this method
                        return;     //return from the method since there was an exception
                    }
                    //if there wasn't an exception then the method is fine and will continue
                    //The build order will need pushed to the interface and to the class wide queue variable
                    //clear the interface before updating it
                    lstv_BuildOrder.Items.Clear();        //clear the build order list view
                    qfo_BuildQueue.Clear();     //clear the build order queue
                    //loop through each item in the queue that was returned from ingesting the file
                    foreach(FactoryObject fo_NewObj in qfo_NewBuildQueue)
                    {
                        FactoryObject fo_BuildingComponent = new FactoryObject("");     //indicates the component that can be built
                        //determine if the item exists in the list of items we've constructed
                        bool b_CanBuild = false;        //indicates if the item exists in the defined list
                        bool b_SpitOutErrMsg = false;       //indicates if there was a problem which needed identified while processing the following foreach loop
                        //loop through each item in the list of components that we can create
                        foreach(FactoryObject fo_Component in lfo_Components)
                        {
                            //the matching factor will be the name of the component, say someone wants to build a car, the name has to match car in our list
                            if(fo_Component.s_Name == fo_NewObj.s_Name)
                            {
                                //then the names match, verify that the item is craftable by examining the item's components...
                                if(fo_Component.GetComponents().Count <= 0)
                                {
                                    //then the item does not have any components, what's the use in crafting the item if its already crafted
                                    Util.rtxtWriteLine("Error encountered while processing build order file. Cannot construct component: [" + fo_Component.s_Name + "]. Item does not have any required components");
                                    b_SpitOutErrMsg = true;     //set the flag indicating the error message was displayed
                                    break;      //break from the first foreach loop
                                }
                                else
                                {
                                    //otherwise the item is craftable
                                    b_CanBuild = true;
                                    fo_BuildingComponent = fo_Component;
                                    break;      //break from the first foreach loop
                                }
                            }
                        }
                        //determine if the item is safe to add to the queue of buildable items
                        if(b_CanBuild)
                        {
                            //then the item is craftable and exists in the list
                            lstv_BuildOrder.Items.Add(new ListViewItem(fo_BuildingComponent.s_Name));      //add the item to the list of items in the interface
                            qfo_BuildQueue.Enqueue(fo_BuildingComponent);      //add the item to the queue of items to build
                        }
                        else
                        {
                            //otherwise the item is not craftable or does not exist in the list
                            //if the error message was not already displayed then the user will need to know that the item does not exist in the list
                            if (!b_SpitOutErrMsg)
                                Util.rtxtWriteLine("Error encountered while processing build order file. Cannot construct component: [" + fo_NewObj.s_Name + "]. Object not found in the list of craftable items. Please define the construction of the object");
                        }
                    }
                    Util.rtxtWriteLine("Successfully processed the build order file.");
                }
                else
                {
                    //otherwise the file does not exist, tell the user
                    Util.rtxtWriteLine("Error while processing build order file. File does not exist. Please provide a valid file.");
                }
            }
        }
        #endregion private void btn_BuildOrder_Click(object sender, EventArgs e)

        #region private void btn_BeginConstruction_Click(object sender, EventArgs e)
        /// <summary>
        /// private void btn_BeginConstruction_Click(object sender, EventArgs e)
        /// Begins constructing items, needs to be done in a thread so that this does not interrupt the gui thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BeginConstruction_Click(object sender, EventArgs e)
        {
            //determine if a cross thread access for the rich text box is necessary
            if(rtxt_WidgFact.InvokeRequired)
            {
                //then a cross thread access is required
                rtxt_WidgFact.Invoke((MethodInvoker)delegate
                {
                    rtxt_WidgFact.Text = "";        //clear the text of the rich text box
                });
            }
            else
            {
                //otherwise a cross thread acces is not required
                rtxt_WidgFact.Text = "";        //clear the text of the rich text box
            }
            Thread th_BuildThread = new Thread(BeginConstruction);
            th_BuildThread.Start();
        }
        #endregion private void btn_BeginConstruction_Click(object sender, EventArgs e)
        #region private void BeginConstruction()
        /// <summary>
        /// private void BeginConstruction()
        /// Thread that is run whenever the user wants to begin building
        /// Needs to be a thread because the interface doesn't update while sitting in the same thread
        /// </summary>
        private void BeginConstruction()
        {
            //verify that there are items in the queue which will contain the build order
            if (qfo_BuildQueue.Count > 0)
            {
                //then there are items to build in the queue
                foreach (FactoryObject fo_Build in qfo_BuildQueue)
                {
                    int i_Delay = 250;
                    double d_Eta = fo_Build.GetComponents().Count * i_Delay / 1000;
                    Util.rtxtWriteLine("Building object: " + fo_Build.s_Name + ". ETA: " + d_Eta.ToString("0.0") + " seconds");
                    Util.rtxtWrite("\t");
                    Queue<FactoryObject> qfo_BuildingComponents = fo_Build.GetComponents();
                    //we will have to iterate through the components of the item that is building
                    foreach (FactoryObject fo_BuildComponent in qfo_BuildingComponents)
                    {
                        //print the component that is being constructed
                        Util.rtxtWrite(" " + fo_BuildComponent.s_Name);
                        Thread.Sleep(i_Delay);
                    }
                    Util.rtxtWriteLine(Environment.NewLine + "Successfully built item: " + fo_Build.s_Name);
                }
                Util.rtxtWriteLine("Successfully completed build queue.");
            }
            else
            {
                //otherwise there are no items in the queue
                Util.rtxtWriteLine("There are no items to construct in the queue. Please load the correct files and try again.");
            }
        }
        #endregion private void BeginConstruction()
    }
}

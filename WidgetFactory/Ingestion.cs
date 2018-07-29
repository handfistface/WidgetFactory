using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WidgetFactory
{
    /// <summary>
    /// public static class Ingestion
    /// Author: John Kirschner
    /// Date: 07-28-2018
    /// Details: 
    ///     This class contains the methods which will ingest files and turn them into factory objects
    /// </summary>
    public static class Ingestion
    {
        #region public static Stack<FactoryObject> IngestObjectFile(FileInfo fi_FileToConsume)
        /// <summary>
        /// public static Stack<FactoryObject> IngestObjectFile(FileInfo fi_FileToConsume)
        /// Ingests a file and processes the information in it so that the strings can be turned into FactoryObjects
        /// The FactoryObjects are then added to a stack and returned
        /// The object file describes how objects are assembled, for example
        ///     Motorcycle: Wheel, Wheel, Motor, Handlebars
        ///     Entries are separated by a new line character so multiple assemblies can be defined in the object file
        ///     Conponents can be proceeded by a number which indicates how many of that component are used in the construction
        ///     Motorcycle: 2 Wheel, Motor, Handlebars
        /// </summary>
        /// <param name="fi_FileToConsume">The FileInfo object which will be used to generate the FactoryObject stack</param>
        /// <returns>A stack of factory objects which are read from the file</returns>
        public static Stack<FactoryObject> IngestObjectFile(FileInfo fi_FileToConsume)
        {
            Stack<FactoryObject> sfo_Ret = new Stack<FactoryObject>();      //stack which will hold the information
            //use a stream reader to read the contents of the file
            //the FullName will be the full path to the file and will have the file name as well
            using (StreamReader sr = new StreamReader(fi_FileToConsume.FullName))
            {
                //while the end of the file has not been reached
                while(!sr.EndOfStream)
                {
                    string s_Line = sr.ReadLine();      //read a line, this will be used to describe the object
                    s_Line = s_Line.ToLower();      //convert the string to lower case because we don't want our storage to get confused
                    char[] ca_Delims = { ',', ';', ':' };      //used to split up the line that was just read
                    string[] sa_Split = s_Line.Split(ca_Delims);        //split the string that was just read
                    //verify that there are components to add to the object construction
                    if (sa_Split.Length <= 1)
                    {
                        //then there will be a problem with constructing the object
                        //an object cannot be by itself unless it is a component
                        if (sa_Split.Length == 1)
                            Util.rtxtWriteLine("Error while parsing the object construction file. [" + sa_Split[0] + "] must have at least one component");
                        else
                            Util.rtxtWriteLine("Error while parsing the object construction file. Object must have at least one component");
                        continue;       //continue to the next object to process
                    }
                    //verify that the stack does not already contain a definition for this object
                    //the names will be used as a determiner for matching with the object
                    bool b_AlreadyProcessed = false;        //indicates if the object should be skipped
                    foreach(FactoryObject fo in sfo_Ret)
                    {
                        //if the names match then this is going to be the same object
                        if (fo.s_Name == sa_Split[0])
                            b_AlreadyProcessed = true;      //set the flag allowing 
                    }
                    //determine if the entry is already processed
                    if (b_AlreadyProcessed)
                        continue;       //continue onto the next entry since this entry is already processed
                    //if we are at this point then there is a valid line of information to be read
                    FactoryObject fo_Construct = new FactoryObject(sa_Split[0]);        //create the factory object based on the name of the object to be created
                    //iterate through the components and add each one to the list of components
                    for(int i=1; i < sa_Split.Length; i++)
                    {
                        //determine if this is going to be an empty string, sometimes the split adds an empty string
                        if(sa_Split[i] == "")
                        {
                            //then just continue onto the next item in the split string
                            continue;
                        }
                        //if this is an integer then skip this and examine the next object, an integer indicates multiple objects as components
                        int i_HowMany;      //indicates how many components will be processed
                        if(int.TryParse(sa_Split[i], out i_HowMany))
                        {
                            //then the parse suceeded, this is an indicator for how many components come next
                            //determine if there is enough room to increment the index variable
                            if((i + 1) >= sa_Split.Length)
                            {
                                //then there will not be enough room to process the count
                                Util.rtxtWriteLine("Line format incorrect. Skipping entry. Ended in integer for object definition: " + fo_Construct.s_Name);
                                continue;       //continue to the next line, don't bother adding this object to the list
                            }
                            i++;        //increment the indexer
                            while (i < sa_Split.Length && sa_Split[i] == "")
                                i++;
                        }
                        else
                        {
                            i_HowMany = 1;      //otherwise the parse failed, just add one item to the list of components
                        }
                        FactoryObject fo_Component = null;     //the factory object component, null indicates it was not previously in the stack
                        //get the item if it already exists
                        foreach(FactoryObject fo_Examine in sfo_Ret)
                        {
                            //if the names match then this is going to be the same factory object
                            if (fo_Examine.s_Name == sa_Split[i])
                            {
                                fo_Component = fo_Examine;      //set the component to the object that already exists in the list
                                break;      //short circuit the loop to prevent any further processing
                            }
                        }
                        //determine if the component already exists in the list
                        if(fo_Component == null)
                        {
                            //then a new component will need to be created because it does not exist in the list
                            fo_Component = new FactoryObject(sa_Split[i]);
                            sfo_Ret.Push(fo_Component);
                        }
                        //now add the factory object to the list of components for the main object for this line
                        //add 'how many' items to the list
                        for(int j=0; j < i_HowMany; j++)
                            fo_Construct.AddComponent(fo_Component);        //add the component to the list
                    }
                    //now that the source object has been declared then it is proper to add the source object to the stack
                    sfo_Ret.Push(fo_Construct);     //push the constructed item onto the end of the list
                }
            }
            return sfo_Ret;     //return the stack of factory objects
        }
        #endregion public static Stack<FactoryObject> IngestObjectFile(FileInfo fi_FileToConsume)

        #region public static Stack<FactoryObject> IngestBuildOrderFile(FileInfo fi_FileToConsume)
        /// <summary>
        /// public static Stack<FactoryObject> IngestBuildOrderFile(FileInfo fi_FileToConsume)
        /// This method ingests a build order and spits out a stack which indicates the proper build order
        /// The ingestion is going to be similar to the IngestObjectFile in that integers are allowed to define the amount of 
        /// objects to be assembled in a row, example file: 
        /// Motorcycle, Motorcycle, Car, Truck
        /// 2 Motorcycle, Car, 3 Truck
        /// </summary>
        /// <param name="fi_FileToConsume">The FileInfo object which describes the file that will be ingested</param>
        /// <returns>A queue of items to build</returns>
        public static Queue<FactoryObject> IngestBuildOrderFile(FileInfo fi_FileToConsume)
        {
            Queue<FactoryObject> qfo_BuildOrder = new Queue<FactoryObject>();       //stack which contains the build order
            //use a stream reader to read the contents of the file
            using (StreamReader sr = new StreamReader(fi_FileToConsume.FullName))
            {
                //read until the end of the file is reached
                while(!sr.EndOfStream)
                {
                    string s_Line = sr.ReadLine();      //read a line of text
                    char[] ca_Delims = { ' ', ',', ':', ';' };      //deliminators used to split up the read line
                    string[] sa_Split = s_Line.Split(ca_Delims);        //split up the line that was read so that it contains the build order
                    //loop through the array of strings that will be processed
                    for(int i=0; i < sa_Split.Length; i++)
                    {
                        //determine if we're looking at an empty string, sometimes splitting up the string can get messy and return empty strings
                        if (sa_Split[i] == "")
                            continue;       //just continue onto the next string
                        //determine if the currently viewing string is an integer
                        int i_HowMany;      //the count of how many objects to push into the stack
                        if (int.TryParse(sa_Split[i], out i_HowMany))
                        {
                            //then the parse succeeded, determine if there is enough room at the end of the array to adjust the indexer
                            if ((i + 1) >= sa_Split.Length)
                            {
                                //then the digit is at the end of the line, it should be ignored and the next line should be read
                                Util.rtxtWriteLine("Error while parsing line of build order file. Integer at end of line, skipping line.");
                                break;      //break out of the wrapping for loop which is iterating through the sa_Split
                            }
                            //otherwise the parse succeeded and there is enough room to adjust the index variable
                            i++;        //increment i before adjusting
                            //again with the dirty string splitting
                            while(i < sa_Split.Length && sa_Split[i] == "")
                                i++;
                        }
                        else
                        {
                            //otherwise the parse failed for an integer and there is just going to be one item
                            i_HowMany = 1;
                        }
                        //now that how many of the items is out of the way then we can take the sa_Split at the current index and add it to the list
                        FactoryObject fo_Build = new FactoryObject(sa_Split[i]);        //create a factory object based on the split
                        for(int j=0; j < i_HowMany; j++)
                            qfo_BuildOrder.Enqueue(fo_Build);       //enqueue the item at the end of the line
                    }
                    //continue the while loop
                }
                //return the queue that was created to represent the build order
                return qfo_BuildOrder;      //return the queue
            }
        }
        #endregion public static Stack<FactoryObject> IngestBuildOrderFile(FileInfo fi_FileToConsume)
    }
}

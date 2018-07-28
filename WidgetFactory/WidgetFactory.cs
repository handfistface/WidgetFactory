using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
    /// </summary>
    public partial class WidgetFactory : Form
    {
        #region WidgetFactory Variables
        private string s_LastLoadedPath;        //holds the last loaded path for the files, based off the relevant settings file variable
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
        }
        #endregion public WidgetFactory()
    }
}

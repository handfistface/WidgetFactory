using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WidgetFactory
{
    /// <summary>
    /// public static class Util
    /// Author: John Kirschner
    /// Date: 07-28-2018
    /// Details: 
    ///     This is a utility class
    ///     Holds miscellaneous functions
    /// </summary>
    public static class Util
    {
        #region Util Variables
        private static RichTextBox rtxt = null;        //the rich text box which information will be printed to
        #endregion Util Variables

        #region public static void Init(RichTextBox rtxt_Printer)
        /// <summary>
        /// public static void Init(RichTextBox rtxt_Printer)
        /// The init method of the static class, sets up the rtxt of this class so information can be printed
        /// </summary>
        /// <param name="rtxt_Printer">The rich text box which information will be printed to. Mainly used in rtxtWriteLine()</param>
        public static void Init(RichTextBox rtxt_Printer)
        {
            rtxt = rtxt_Printer;        //setup the rich text box reference
        }
        #endregion public static void Init(RichTextBox rtxt_Printer)

        #region public static void rtxtWriteLine(string s_Line)
        /// <summary>
        /// public static void rtxtWriteLine(string s_Line)
        /// Prints a string to the rich text box
        /// Uses the class wide rtxt
        /// If there is a null reference for the class wide rtxt then it will prevent anything from happening
        /// </summary>
        /// <param name="s_Line">The line to print</param>
        public static void rtxtWriteLine(string s_Line)
        {
            //verify that there is not a null reference
            if (rtxt == null)
                return;     //get out of here before anything happens
            //call the parent method with the class rich text box reference
            rtxtWriteLine(s_Line, rtxt);        //call the line
        }
        #endregion public static void rtxtWriteLine(string s_Line)
        #region public static void rtxtWriteLine(string s_Line, RichTextBox rtxt_Printer)
        /// <summary>
        /// public static void rtxtWriteLine(string s_Line, RichTextBox rtxt_Printer)
        /// Prints a string to a rich text box
        /// </summary>
        /// <param name="s_Line">The line to print</param>
        /// <param name="rtxt_Printer">The rich text box to print the information to</param>
        public static void rtxtWriteLine(string s_Line, RichTextBox rtxt_Printer)
        {
            rtxtWrite(s_Line + Environment.NewLine, rtxt_Printer);      //write a line, append the Environment.NewLine onto the end of the text
        }
        #endregion public static void rtxtWriteLine(string s_Line, RichTextBox rtxt_Printer)

        //
        //rtxtWrite(string s_Text, RichTextBox rtxt_Printer) contains the meat and potatoes of the rich text box writing functionality
        //
        #region public static void rtxtWrite(string s_Text, RichTextBox rtxt_Printer)
        /// <summary>
        /// public static void rtxtWrite(string s_Text, RichTextBox rtxt_Printer)
        /// Similar to rtxtWriteLine except that no environment.newline is added to the end of the text
        /// </summary>
        /// <param name="s_Text">The text which will get written to the rich text box</param>
        /// <param name="rtxt_Printer">The rich text box which will get writen to</param>
        public static void rtxtWrite(string s_Text, RichTextBox rtxt_Printer)
        {
            //wrap in a try/catch because there can be issues when closing the application which cause a thread not existant access
            //this will prevent an outright crash in the middle of writing to the rich text box
            try
            {
                //null sanity check
                if (s_Text == null || rtxt_Printer == null)
                    return;
                //make sure that a multi threaded access is required
                if (rtxt_Printer.InvokeRequired)
                {
                    //then invoking the thread is required, this is not the creating thread of the rich text box
                    rtxt_Printer.Invoke((MethodInvoker)delegate
                    {
                        rtxt_Printer.AppendText(s_Text);      //tack the new line onto the end of the text box
                    });
                }
                else
                {
                    //otherwise invoking is not required, this is the creating thread of the rich text box
                    rtxt_Printer.AppendText(s_Text);      //tack the new line onto the end of the text box
                }
            }
            catch (Exception)
            {
                //usually this is where I log and/or create a dump file of the program
            }
        }
        #endregion public static void rtxtWrite(string s_Text, RichTextBox rtxt_Printer)
        #region public static void rtxtWrite(string s_Text)
        /// <summary>
        /// public static void rtxtWrite(string s_Text)
        /// Calls rtxtWrite(s_Text, rtxt_Printer) with the local static reference to the rich text box
        /// </summary>
        /// <param name="s_Text">The text which will be printed to the rich text box</param>
        public static void rtxtWrite(string s_Text)
        {
            rtxtWrite(s_Text, rtxt);        //call the method whiich will handle printing text with the local class reference to the rich text box
        }
        #endregion public static void rtxtWrite(string s_Text)
    }
}

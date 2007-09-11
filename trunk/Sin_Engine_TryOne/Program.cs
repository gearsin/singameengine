//#define LOG_TEST 

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Sin_Engine_TryOne.Helper;
using System.Diagnostics;


namespace Sin_Engine_TryOne
{
    static class Program
    {

        public static GameLogHandler log = new GameLogHandler();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            StackFrame callStack = new StackFrame();
            
            //StackTrace callStack = new StackTrace(1, true);
            log.AddTagTree("Project");
            log.AddTag("Name", Application.ProductName);

            log.AddTag("Type", "Project");
            log.AddTag("FileName", Application.ProductName);
            log.AddTag("Compiled", DateTime.Now.ToString() );

            #region  XML Log test
#if LOG_TEST
            string sfileName = "Program.cs";//callStack.GetFileName().ToString();
            string sMethodName = callStack.GetMethod().ToString();
            int LineNumber = callStack.GetFileLineNumber();
            //log.CloseTagTree();

            log.AddTagTree("LogEntries");
            log.WriteLog("Main", sMethodName, "Info", "Starting up test ", sfileName, callStack.GetFileLineNumber());
            log.WriteLog("Main", sMethodName, "Info", "Second test", sfileName, callStack.GetFileLineNumber());
            log.WriteLog("Main", sMethodName, "Warning", "A fake warning", sfileName, callStack.GetFileLineNumber());
            log.WriteLog("CLogHandler", "Write", "Info", "Writing down some stuff...", sfileName, callStack.GetFileLineNumber());
            log.WriteLog("Main", sMethodName, "Error", "A fake error!", sfileName, callStack.GetFileLineNumber());
            log.WriteLog("Main", sMethodName, "Critical", "Terminating", sfileName, callStack.GetFileLineNumber());
            log.WriteLog("Main", sMethodName, "Info", "(I'm fine, don't worry!)", sfileName, callStack.GetFileLineNumber());
            log.WriteLog("Main", sMethodName, "Info", "Look below it's a integer!", sfileName, callStack.GetFileLineNumber());
            //log.WriteLog("Main", "main", "Info", new String(6,0,3), callStack.GetFileName(), callStack.GetFileLineNumber());
            log.CloseTagTree(); // LogEntries
#endif
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SinGameEngineForm.TestGraphicEngineForm();
            //Application.Run(new SinGameEngineForm());
        }
    }
}
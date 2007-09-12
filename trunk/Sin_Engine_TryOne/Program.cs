//#define LOG_TEST 

#region Directives
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Sin_Engine_TryOne.Helper;
using System.Diagnostics;
using NUnit.Framework;
#endregion

namespace Sin_Engine_TryOne
{
    static class Program
    {

        /// <summary>
        /// log handler to log all the game events
        /// </summary>
        public static GameLogHandler log = new GameLogHandler();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            
            //StackTrace callStack = new StackTrace(1, true);
            log.AddTagTree("Project");
            log.AddTag("Name", Application.ProductName);

            log.AddTag("Type", "Project");
            log.AddTag("FileName", Application.ProductName);
            log.AddTag("Compiled", DateTime.Now.ToString() );

        

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
            UnitTesting.StatTest(UnitTesting.eTestModule.eGameFormTest);
#else
            //Place holder
            //To be replace to exact game loop
            //
            SinGameEngineForm.TestGraphicEngineForm();
#endif
        }

        ///<summary>
        ///Do unit test here
        ///</summary>
        #region Unit Test
        [Test]
        public static void TestXMLLogSystem()
        {
            StackFrame callStack = new StackFrame();
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
        }
        #endregion
    }
}
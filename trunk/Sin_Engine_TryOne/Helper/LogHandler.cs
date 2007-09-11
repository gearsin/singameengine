
/// <summary>
/// Helper class for making log system
/// XML base logging system
/// </summary>
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Sin_Engine_TryOne.Helper
{
    public class LogFileHelper
    {
        /// <summary>
        /// Declare variables in this region
        /// </summary>
        #region Variables Decleration
        private static string m_sFileName = "DebugLog.xml"; // log file name
        private static StreamWriter m_StreamWriter = null;
        #endregion

        /// <summary>
        /// initialize and destroy object
        /// </summary>
        /// <param name="sFileName"></param>
        #region Constructor
        public LogFileHelper()
        {
            try
            {
                FileStream file = new FileStream(m_sFileName, FileMode.Create, FileAccess.Write,
                                                  FileShare.ReadWrite);

                // Associate writer with that, when writing to a new file,
                // make sure UTF-8 sign is written, else don't write it again!
                //////if (file.Length == 0)
                  //  m_StreamWriter = new StreamWriter(file, System.Text.Encoding.UTF8);
               // else
                    m_StreamWriter = new StreamWriter(file);

                // Go to end of file
                m_StreamWriter.BaseStream.Seek(0, SeekOrigin.End);

                // Enable auto flush (always being up to date when reading!)
                m_StreamWriter.AutoFlush = true;

#if DEBUG
                // Add some info about this session
                System.Console.WriteLine("\n-----------------------");
                System.Console.WriteLine("\nSession started at: " + DateTime.Now);

                System.Console.WriteLine("\nProduct Name \n" + Application.ProductName +
                                         " version \n" + Application.ProductVersion);

                System.Console.WriteLine("\n-----------------------");
#endif


            }catch 
            {
                // Ignore any file exceptions, if file is not
                // create able (e.g. on a CD-Rom) it doesn't matter
                MessageBox.Show("Unable to open log file!", "File Error");         
            }


            
        }

        LogFileHelper(string sFileName)
        {
            m_sFileName = sFileName;
            //TODO
        }

        ~LogFileHelper() { Console.WriteLine("Log File Handler destroyed");/*m_StreamWriter.Close(); */   }
        #endregion

        #region Method
        public void clearFile()
        {
           if(m_StreamWriter !=null)
            {
                try {
                        m_StreamWriter.Close();
                        FileStream file = new FileStream(m_sFileName, FileMode.Create, FileAccess.Write,
                                                          FileShare.ReadWrite);

                       // m_StreamWriter = new StreamWriter(file, System.Text.Encoding.UTF8);
                        m_StreamWriter = new StreamWriter(file);
                
                }catch
                {
                    MessageBox.Show("Unable to clear File!", "File Error");
                }
      
            }
        }

        public void Write(string logText)
        {
            try
            {
                /*DateTime ct = DateTime.Now;
                string s = "[" + ct.Hour.ToString("00") + ":" +
                                ct.Minute.ToString("00") + ":" +
                                ct.Second.ToString("00") + "] " +
                                logText;*/

                m_StreamWriter.Write(logText);

#if DEBUG
                // In debug mode write that message to the console as well!
                System.Console.WriteLine(logText);
#endif

            }
            catch(Exception ex)
            {
                MessageBox.Show( ex.ToString(),"Unable to write!");
                //m_StreamWriter.Dispose();
                //m_StreamWriter.Close();
            }
        }
        #endregion
    }

   public class LogHandler
    {
        #region Variables
        List<string> m_vsTags;
        int m_iIndent    = 0;
        protected LogFileHelper m_logFile;
        #endregion

        #region Constructor
        public LogHandler()
        {
            m_vsTags = new List<string>();
            m_logFile = new LogFileHelper();
         //   m_logFile.clearFile();
            m_logFile.Write("<?xml version=\"1.0\" encoding=\"iso-8859-1\" ?>\n");
        }
        
        ~LogHandler()
        {            
           /* if (m_vsTags != null)
            {
                while (m_vsTags.Count > 0)
                {
                    CloseTagTree();
                }
            }*/
        }
        #endregion

        #region Method

       public void CloseLog()
       {
           if (m_vsTags != null)
           {
               while (m_vsTags.Count > 0)
               {
                   CloseTagTree();
               }
           }
       
       }
       public string addTabs()
       {
           string stab = "";
           int tabCount;
           //m_iIndent.ToString().Format( )
           for ( tabCount = 0; tabCount < m_iIndent; tabCount++)
               stab += "\t";

           return stab;
       }
        public void AddTagTree(string TagName)
        {
            m_logFile.Write(new string('\t', m_iIndent) + "<" + TagName + ">\n");
            m_vsTags.Add(TagName);
            ++m_iIndent;
        }

        public void AddTag(string TagName, string TagValue)
        {
            m_logFile.Write(new string('\t', m_iIndent) + "<" + TagName + ">" + TagValue);
            m_logFile.Write("</" + TagName + ">\n");
        }

        public void CloseTagTree()
        {
            if (m_vsTags.Count > 0)
            {
                if(m_iIndent > 0)
                {
                    --m_iIndent;
                }

                //string te = new string('\t', m_iIndent);
                m_logFile.Write( new string('\t', m_iIndent) + "</" + m_vsTags[m_vsTags.Count - 1] + ">\n");
                m_vsTags.RemoveAt(m_vsTags.Count - 1);
            }
        }
        #endregion
    }

    public class GameLogHandler : LogHandler
    {
    
        public GameLogHandler()
        {
            this.m_logFile.Write("<?xml-stylesheet type=\"text/xsl\" href=\"stylesheet.xsl\" ?>\n\n");
            this.m_logFile.Write("<!-- Created by XMLLogger -->\n\n");
            //this.AddTagTree("Log Entry");
        }

        public void WriteLog( string sClassName, string sFuncName, 
                               string sDebugType, string sInfo, string sFile,
                                long lLine)
        {
            DateTime timeStamp = DateTime.Now;
            this.AddTagTree("LogEntry");
            this.AddTag("ClassName", sClassName);
            this.AddTag("FuncName", sFuncName);
            this.AddTag("DebugType", sDebugType);
#if USE_LONG_TIME_STAMP
            this.AddTag("Timestamp", System.String.Format("{0:R}", timeStamp));
#else
             this.AddTag("Timestamp", timeStamp.ToString());
#endif

            this.AddTag("Info", sInfo);
            this.AddTag("Line", lLine.ToString());
            this.AddTag("File", sFile);
            this.CloseTagTree();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Sin_Engine_TryOne
{
    class UnitTesting
    {
        #region Variables
        /// <summary>
        /// Define the enumerated data to call unit test on module
        /// easy to understand
        /// </summary>
       public enum eTestModule
        {
            eGameFormTest,
            eCameraTest,
            eXMLLogTest,
            //More to come...
        };
        #endregion
        
       
        ///<summary>
        /// Place of all method 
        /// 
        ///</summary>>

        #region Method
        /// <summary>
        /// Start unit test on  selected modules
        /// </summary>
        /// <param name="iwhichTest"></param>
        public static void StatTest(eTestModule ewhichModule)
        {
            switch(ewhichModule)
            {
                default:
                case eTestModule.eGameFormTest:
                    SinGameEngineForm.TestGraphicEngineForm();
                    break;
                case eTestModule.eCameraTest:
                      //TODO  
                    break;
                case eTestModule.eXMLLogTest:
                    Program.TestXMLLogSystem();
                    break;
                

            }
        }
        #endregion

    }
}

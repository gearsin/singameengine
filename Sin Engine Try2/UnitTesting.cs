#region Directives
using System;
using System.Collections.Generic;
using System.Text;
using Sin_Engine.EngineDevice;
//using Sin_EngineOne.Input;
#endregion

namespace Sin_Engine
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
            eGameTest,
            eFormTest,
            eCameraTest,
            eXMLLogTest,
            eKeyboardTest,
            eMouseTest
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

            switch (ewhichModule)
            {
                default:
                case eTestModule.eFormTest:
                    GameForm.TestGraphicEngineForm();
                    break;
                case eTestModule.eGameTest:
                    GameForm testGame = new GameForm();
                    testGame.StartGame();
                    break;
                case eTestModule.eCameraTest:
                    //TODO  
                    break;
                case eTestModule.eXMLLogTest:
                    //Program.TestXMLLogSystem();
                    break;
                case eTestModule.eKeyboardTest:
                    Keyboard.TestKeyboardInput();
                    break;
                case eTestModule.eMouseTest:
                    Mouse.TestMouseInput();
                    break;

            }
        }
        #endregion

    }
}

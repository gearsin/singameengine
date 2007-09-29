/// <summary>
/// This class handle all the Keyboard input
/// create the DirectX device for handling the key input
/// Creation date : 15-Sep-07
/// Last Modified date : 15-Sept-07
/// created by Sunil Singh 'Gear'
/// </summary>

#region Directives

using Microsoft.DirectX.DirectInput;
using System.Windows.Forms;
using NUnit.Framework;
using Sin_Engine_TryOne.Helper;
using System;

#endregion

namespace Sin_Engine_TryOne.Input
{
    public class KeyboardInput
    {
        #region Varaibles
        ///<summary>
        /// DirectInput keyboard device
        ///</summary>
        private Device m_Keyboard   = null;

        ///<summary>
        /// Key State
        ///</summary>
        private KeyboardState m_KeyState;
        #endregion
        
        #region Constructor
        /// <summary>
        /// Create the keyboard input for the game form
        /// </summary>
        /// <param name="gameForm">Form</param>
        public KeyboardInput(Form gameForm)
        {
            //create the keyboard device
            m_Keyboard = new Device(SystemGuid.Keyboard);

            //allow other apps to have keyboard input as well
            m_Keyboard.SetCooperativeLevel(gameForm, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);

            //acquire Keyboard
            m_Keyboard.Acquire();
        }
        #endregion

        #region Properties
        ///<summary>
        ///check if Left move key pressed
        ///</summary>
        public bool Left 
        {
            get
            {
                return m_KeyState != null && (m_KeyState[Key.LeftArrow] || m_KeyState[Key.A]);
            } // get
        } // Left

        /// <summary>
        /// check if right move key  pressed 
        /// </summary>
        public bool Right
        {
            get
            {
                return m_KeyState != null && (m_KeyState[Key.RightArrow] || m_KeyState[Key.D]);
            } // get
        }//Right

        /// <summary>
        /// check if Up move key  pressed 
        /// </summary>
        public bool Up
        {
            get
            {
                return m_KeyState != null && (m_KeyState[Key.UpArrow] || m_KeyState[Key.W]);
            } // get
        }//UP

        /// <summary>
        /// check if Down key  pressed 
        /// </summary>
        public bool Down
        {
            get
            {
                return m_KeyState != null && (m_KeyState[Key.DownArrow] || m_KeyState[Key.S]);
            } // get
        }//Down

        /// <summary>
        /// check if Escape key  pressed 
        /// </summary>
        public bool Escape
        {
            get
            {
                return m_KeyState != null && (m_KeyState[Key.Escape]);
            } // get
        }//Escape

        bool lastFrameEscapePressed = false;
        /// <summary>
        /// check if Escape  key just pressed 
        /// </summary>
        public bool EscapeJustPressed
        {
            get
            {
                return Escape && lastFrameEscapePressed == false;
            } // get
        }//EscapeJustPressed

        /// <summary>
        /// Keyboard state, use this to retrieve more key states.
        /// For example: keyboardInput.State[Key.F1] ...
        /// </summary>
        /// <returns>State</returns>
        public KeyboardState KeyState
        {
            get
            {
                return m_KeyState;
            } // get
        } // State

        #endregion

        #region Methods
        /// <summary>
        /// Update the Keyboard state
        /// </summary>
        #region Update Method
        public void Update()
        {
            try
            {
                //remember key state from last frame
                lastFrameEscapePressed = Escape;

                // check the key state
                m_KeyState = m_Keyboard.GetCurrentKeyboardState();
            }
            catch(Exception ex)
            {
                Program.log.WriteLog(this.ToString(), "Update", "Error", ex.ToString(), "Keyboard.cs", 152);
            }
        }//Update()
        #endregion
        #endregion

        #region Unit Testing
#if DEBUG
        /// <summary>
        /// Test keyboard input
        /// </summary>
        [Test]
        public static void TestKeyboardInput()
        {
            SinGameEngineForm testForm = new SinGameEngineForm();//("TestKeyboardInput");

            KeyboardInput keyboardInput = new KeyboardInput(testForm);
            // Run application
            //WindowsHelper.ForceForegroundWindow(testForm);
            testForm.Show();
            while (testForm.IsDisposed == false)
            {
                keyboardInput.Update();
                testForm.Text = "TestKeyboardInput: " +
                    "Left=" + keyboardInput.Left + ", " +
                    "Right=" + keyboardInput.Right + ", " +
                    "Up=" + keyboardInput.Up + ", " +
                    "Down=" + keyboardInput.Down;
                Application.DoEvents();
            } // while (testForm.IsDisposed)
        } // TestKeyboardInput()
#endif

        #endregion

    }
}

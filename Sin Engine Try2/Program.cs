#region Directives
using System;
using System.Collections.Generic;
using System.Windows.Forms;
#endregion

namespace Sin_Engine
{
    static class Program
    {
        #region Variables
        ///<summary>
        /// the main game Object
        /// </summary>
        //public static GameForm game = nu;// = new GameForm()    
        #endregion

        #region Method
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GameForm game = new GameForm();
            game.StartGame();
            /*using (GameForm game = new GameForm())
            {
                if (1 > 2) //some error 
                {
                    MessageBox.Show("Print Some Error");
                }
                game.StartGame();
            }*/

        }

        //public static GameForm GetGameObject()
        //{
        //    //return game;
        //}
        #endregion

    }
}
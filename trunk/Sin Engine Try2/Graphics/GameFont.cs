#region Using Directinves
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Sin_Engine.Properties;
using System.Drawing;
using System.Windows.Forms;
#endregion
namespace Sin_Engine.Graphics
{
    class GameFont
    {
        #region Variabels
        /// <summary>
        ///  font rendering 
        /// </summary>
        Microsoft.DirectX.Direct3D.Font font;
        
        /// <summary>
        /// use this sprite to render all the fonts
        /// </summary>
        static Sprite fontSprite;
        #endregion

        #region Constructor
        /// <summary>
        /// default font constructor: create the default font type from game settings
        /// </summary>
        public GameFont()
        {
            //Create the default font type
            try
            {
                font = new Microsoft.DirectX.Direct3D.Font(GameForm.DirectXDevice, new System.Drawing.Font(
                                                                  GameSettings.Default.fontName,
                                                                  GameSettings.Default.fontSize));

                fontSprite = new Sprite(GameForm.DirectXDevice);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        /// <summary>
        /// Initialize the font system with specific family name and font size
        /// </summary>
        /// <param name="familyName"></param>
        /// <param name="fontSize"></param>
        public GameFont( string familyName, float fontSize )
        {
            font = new Microsoft.DirectX.Direct3D.Font( GameForm.DirectXDevice, 
                                                           new System.Drawing.Font(familyName, fontSize) );

            fontSprite = new Sprite( GameForm.DirectXDevice );
        }
        #endregion

        #region Draw text
        /// <summary>
        /// display text on screen
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="xpos"></param>
        /// <param name="ypos"></param>
        /// <param name="color"></param>
        public void PrintText(string text, int xpos, int ypos, Color color )
        {
            fontSprite.Begin(SpriteFlags.AlphaBlend);
            font.DrawText(fontSprite, text, new Point(xpos, ypos), color);
            fontSprite.End();
        }
        #endregion
    }
}

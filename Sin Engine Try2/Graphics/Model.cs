///<summary>
///TODO: 
/// Right now this class load the models, file name provided while creating the objects
/// This class should  load the models from
/// Step1 : Load the all game object from file
/// Step2 : UNKNOWN
///</summary>


#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Windows.Forms;

#endregion


namespace Sin_Engine
{
    class Entity3D
    {
        #region Variable
        Mesh object3D   = null;
        Texture entityTexture = null;
        #endregion

        #region Constructor
        public Entity3D( string fileName)
        {
            try
            {
                object3D = Mesh.FromFile(fileName, MeshFlags.Managed, GameForm.DirectXDevice);
                entityTexture = TextureLoader.FromFile(GameForm.DirectXDevice, "game data\\textures\\models\\Rocket.dds");
            }catch(Direct3DXException ex)
            {
                //catch the DX exception here
                MessageBox.Show(ex.ToString());
            }
            catch(Exception ex)
            {
                //catch other exception here
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

        #region Render Entity
        public void Render()
        {
            GameForm.DirectXDevice.Transform.World = Matrix.Scaling(0.25f, 0.25f, 0.25f);
            GameForm.DirectXDevice.RenderState.Lighting = false;
            GameForm.DirectXDevice.SetTexture(0, entityTexture);
            object3D.DrawSubset(0);
        }
        #endregion
    }
}

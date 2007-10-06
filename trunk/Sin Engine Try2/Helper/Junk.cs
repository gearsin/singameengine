using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Sin_Engine.Properties;

namespace Sin_Engine.Helper
{
    class Junk
    {
        #region Variables
        //Load all required resources (textures, fonts, model etc)
        Texture skyTexture = TextureLoader.FromFile( GameForm.DirectXDevice, "game data\\textures\\SpaceSkyCubeMap.dds");
        #endregion

        #region Constructor
        public Junk()
        {
            skyTexture = TextureLoader.FromFile(GameForm.DirectXDevice, "game data\\textures\\SpaceSkyCubeMap.dds");
        }

        public Junk(string texturename)
        {
            skyTexture = TextureLoader.FromFile(GameForm.DirectXDevice, texturename);
        }

        #endregion
        public void RenderSkyBox( )
        {
            CustomVertex.TransformedTextured[] screenVerts = new 
                                                                   CustomVertex.TransformedTextured[4];

            screenVerts[0] = new CustomVertex.TransformedTextured(0, 0, 0, 1, 0, 0);
            screenVerts[1] = new CustomVertex.TransformedTextured( GameSettings.Default.ScreenWidth, 0, 0, 1, 1, 0);
            screenVerts[2] = new CustomVertex.TransformedTextured( GameSettings.Default.ScreenWidth, GameSettings.Default.ScreenHeight, 0, 1, 1, 1);
            screenVerts[3] = new CustomVertex.TransformedTextured(0,GameSettings.Default.ScreenHeight, 0, 1, 0, 1);

            GameForm.DirectXDevice.RenderState.ZBufferEnable = false;
            GameForm.DirectXDevice.SetTexture(0, skyTexture);
            GameForm.DirectXDevice.VertexFormat = CustomVertex.TransformedTextured.Format;
            GameForm.DirectXDevice.DrawUserPrimitives(PrimitiveType.TriangleFan, 2, screenVerts);
            GameForm.DirectXDevice.RenderState.ZBufferEnable = true;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Sin_Engine_TryOne
{
    class Model
    {
        #region Variable
        Mesh entity3D   = null;
        Texture entityTexture = null;
        #endregion

        #region Constructor
        public Model(Device device, string fileName)
        {
            entity3D = Mesh.FromFile(fileName, MeshFlags.Managed, device);
            entityTexture = TextureLoader.FromFile(device, "..\\game data\\textures\\models\\Rocket.dds");
        }

        #endregion

        #region Render
        public void Render(Device device )
        {
            device.Transform.World = Matrix.Scaling(0.25f, 0.25f, 0.25f);
            device.RenderState.Lighting = false;
            device.SetTexture(0, entityTexture);
            entity3D.DrawSubset(0);
        }
        #endregion
    }
}

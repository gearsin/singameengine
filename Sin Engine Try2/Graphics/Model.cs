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

        /// <summary>
        /// Precaled matrix for rendering the model
        /// </summary>
        protected Matrix objectMatrix = new Matrix();

        #endregion

        #region Constructor
        public Entity3D( string fileName)
        {
            try
            {
                object3D = Mesh.FromFile(fileName, MeshFlags.Managed, GameForm.DirectXDevice);
                entityTexture = TextureLoader.FromFile(GameForm.DirectXDevice, "game data\\textures\\models\\Rocket.dds");
                CalcObjectMatrix();
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

        #region Rendering
        /// <summary>
        /// Render model with specified matrix.
        /// Note: Only used for testing, etc.
        /// AsteroidManager handles own model rendering for asteroids.
        /// </summary>
        /// <param name="renderMatrix">Render matrix</param>
        public virtual void Render(Matrix renderMatrix)
        {

            // Just render all materials with or without shaders.
            // Note: It is much faster to render all models sorted by
            // materials and shaders (first shaders, then materials).
            // Since we only use 1 shader and only 1 material per object,
            // we just sort by models and just render them :)
                Matrix worldMatrix = objectMatrix * renderMatrix;
                GameForm.WorldMatrix = worldMatrix;

                GameForm.DirectXDevice.RenderState.Lighting = false;
                GameForm.DirectXDevice.SetTexture(0, entityTexture);

                 object3D.DrawSubset(0 );
        } // Render(renderMatrix)

        /// <summary>
        /// Render model at specified position.
        /// </summary>
        public void Render(Vector3 renderPos)
        {
            Render(CalcDefaultRenderMatrix(renderPos));
        } // Render(renderPos)

        /// <summary>
        /// Render model at specified position with specified scale.
        /// </summary>
        public void Render(Vector3 renderPos, float scale)
        {
            Render(CalcDefaultRenderMatrix(renderPos, scale));
        } // Render(renderPos, scale)

        /// <summary>
        /// Render model at specified position, scale and rotation.
        /// </summary>
        public void Render(Vector3 renderPos, float scale, float rotation)
        {
            Render(CalcDefaultRenderMatrix(renderPos, scale, rotation));
        } // Render(renderPos, scale, rotation)

        /// <summary>
        /// Render model at specified position, scale, rotation and pitch.
        /// </summary>
        public void Render(Vector3 renderPos, float scale, float rotation,
            float pitch)
        {
            Render(CalcDefaultRenderMatrix(renderPos, scale, rotation, pitch));
        } // Render(renderPos, scale, rotation)
        #endregion
        #region Calc render matrix (for calling Render)
        /// <summary>
        /// Calc default render matrix
        /// </summary>
        public static Matrix CalcDefaultRenderMatrix(Vector3 renderPos)
        {
            return Matrix.Translation(renderPos);
        } // CalcDefaultRenderMatrix(renderPos)

        /// <summary>
        /// Calc default render matrix
        /// </summary>
        public static Matrix CalcDefaultRenderMatrix(
            Vector3 renderPos, float scale)
        {
            return Matrix.Scaling(new Vector3(scale, scale, scale)) *
                Matrix.Translation(renderPos);
        } // CalcDefaultRenderMatrix(renderPos, scale)

        /// <summary>
        /// Calc default render matrix
        /// </summary>
        public static Matrix CalcDefaultRenderMatrix(
            Vector3 renderPos, float scale, float rotation)
        {
            return Matrix.Scaling(new Vector3(scale, scale, scale)) *
                // Use always a ccw rotation of 90 degrees to show model pointing up
                Matrix.RotationZ(rotation - (float)Math.PI / 2.0f) *
                Matrix.Translation(renderPos);
        } // CalcDefaultRenderMatrix(renderPos, scale, rotation)

        /// <summary>
        /// Calc default render matrix, 
        /// </summary>
        public static Matrix CalcDefaultRenderMatrix(
            Vector3 renderPos, float scale,
            float rotation, float pitch)
        {
            return
                Matrix.Scaling(new Vector3(scale, scale, scale)) *
                Matrix.RotationX(pitch) *
                Matrix.RotationZ(rotation - (float)Math.PI / 2.0f) *
                Matrix.Translation(renderPos);
        } // CalcDefaultRenderMatrix(renderPos, scale, rotation)

        /// Calc objectMatrix for rendering, will fix rotation values
        /// provided by the model and scale it to objectRadius to normalize it.
        /// The correctionPosition and the defaultSize is also applied!
        /// </summary>
        protected void CalcObjectMatrix()
        {
            // Only center x and y!
            //objectMatrix = Matrix.Translate(0, 0, 0);
            //don't move it anymore: new Vector3(-center.X, -center.Y, 0.0f));

            //float scaleDownFactor = 1.0f; 
            float scaleDownFactor =  0.25f;
            objectMatrix =  Matrix.RotationX(-(float)Math.PI / 2.0f) *
                                Matrix.Scaling(
                                new Vector3(scaleDownFactor, scaleDownFactor, scaleDownFactor));
        } // CalcObjectMatrix(objectRadius)

        #endregion
        #endregion


    }
}

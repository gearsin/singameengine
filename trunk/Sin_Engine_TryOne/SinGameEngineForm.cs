#region Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
#endregion

namespace Sin_Engine_TryOne
{
    public partial class SinGameEngineForm : Form
    {

        /// <summary>
        /// declare all variables here
        /// </summary>
        #region Variables
        bool isNotClosed = true;
        protected static Device m_dxdevice = null;
        public static Microsoft.DirectX.Direct3D.Font textFont;
        public static Sprite textFontSprite;
        /// <summary>
        /// Use a standard field of view of 90 degrees, and use 1 for near plane
        /// and 1000 for far plane. Field of view might be changed for speed items.
        /// </summary>
        private static float fieldOfView = (float)Math.PI / 2.0f,
            nearPlane = 1.0f,
            farPlane = 100.0f;

        /// <summary>
        /// World, view and projection martix, also used for the shaders!
        /// </summary>
        //private static Matrix worldMatrix, viewMatrix, projectionMatrix;

        #endregion

        /// <summary>
        /// all object initialization done here
        /// </summary>
        #region Contructor
        public SinGameEngineForm()
        {
            InitializeComponent();
            InitializeDirectX();
        }
        #endregion


        #region Event
            protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Program.log.CloseLog();
            isNotClosed = false;
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            BuildProjectionMatrix();
        }
        #endregion

        #region Method

        /// <summary>
        /// Intialize all DirectX component
        /// </summary>

        #region Intialize DirectX
        protected void InitializeDirectX()
        {
            // Setup DirectX device
            PresentParameters presentParams = new PresentParameters();

            // Always use windowed mode for debugging and fullscreen for.
            presentParams.Windowed = true;

            presentParams.PresentationInterval = PresentInterval.Default;
            presentParams.BackBufferFormat = Format.X8R8G8B8;
            presentParams.BackBufferWidth = this.ClientSize.Width;
            presentParams.BackBufferHeight = this.ClientSize.Height;

            // Default to triple buffering for performance gain,
            // if we are low on video memory and use multisampling, 1 is ok too.
            presentParams.BackBufferCount = 2;

            // Discard back buffer when swapping, its faster
            presentParams.SwapEffect = SwapEffect.Discard;

            // Use a Z-Buffer with 32 bit if possible
            presentParams.EnableAutoDepthStencil = true;
            presentParams.AutoDepthStencilFormat = DepthFormat.D24X8;//D32;

            // For windowed, default to PresentInterval.Immediate which will
            // wait not for the vertical retrace period to prevent tearing,
            // but may introduce tearing. For full screen, default to
            // PresentInterval.Default which will wait for the vertical retrace
            // period to prevent tearing.
            if (presentParams.Windowed)
                presentParams.PresentationInterval = PresentInterval.Immediate;
            /*
             else
                presentParams.PresentationInterval = Settings.Default.WaitForVSync ?
                                                    PresentInterval.Default : PresentInterval.Immediate;
             */

            // Create device and set some render states.
            // We require a pure hardware device. This can be done much nicer,
            // see the DirectX SDK and DXUtil classes, but this would be to
            // complex for this simple game IMO.
            try
            {
                m_dxdevice = new Device(0, DeviceType.Hardware,
                                         this,
                                         CreateFlags.HardwareVertexProcessing |
                                         CreateFlags.PureDevice, presentParams);
            } // try
            catch (Exception ex)
            {
                throw ex;
            }


            BuildProjectionMatrix();
            IntializeFontSystem();
        }
        #endregion

       
        #region Build projection matrix
        /// <summary>
        /// Build projection matrix
        /// </summary>
        private void BuildProjectionMatrix()
        {
            if (m_dxdevice != null)
            {
                // Leave world matrix to the standard identity
                m_dxdevice.Transform.World = Matrix.Identity;
                // Create view and projection matrices (default stuff)
                m_dxdevice.Transform.View = Matrix.LookAtLH(
                    new Vector3(-50.0f, -20.0f, -4.0f),
                    new Vector3(0.0f, 0.0f, 0.0f),
                    new Vector3(0.0f, 1.0f, 0.0f));

                float aspectRatio = 1.0f;
                if (this.Height != 0)
                    aspectRatio = (float)this.Width / (float)this.Height;
               
                 m_dxdevice.Transform.Projection = Matrix.PerspectiveFovLH( fieldOfView, aspectRatio, 
                                                                             nearPlane, farPlane);
            }
        } // BuildProjectionMatrix()
        #endregion

        /// <summary>
        /// Intialize FOnt system
        /// </summary>
        public static void IntializeFontSystem()
        {
            textFont    = new Microsoft.DirectX.Direct3D.Font(m_dxdevice, new System.Drawing.Font( "Arial", 12.0f));
            textFontSprite = new Sprite(m_dxdevice);
        }
        public delegate void RenderDelegate();
        public void Render(RenderDelegate renderDelegate)
        {
            if ( m_dxdevice == null || m_dxdevice.Disposed )
                return;

            try{
                m_dxdevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
                m_dxdevice.BeginScene();
                renderDelegate();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Fatal error occurred: " + ex.ToString());
            }
            finally
            {
                m_dxdevice.EndScene();
                m_dxdevice.Present();
            }
        
        }

        public void RenderTextOnScreen(String text)
        {
            textFontSprite.Begin(SpriteFlags.AlphaBlend);
            textFont.DrawText(textFontSprite, text, new Point(0, 0), Color.Bisque);
            textFontSprite.End();
        }
        public void RenderOnScreen(Texture texture)
        {
            CustomVertex.TransformedTextured[] screenVerts = new
                                                                   CustomVertex.TransformedTextured[4];

            screenVerts[0] = new CustomVertex.TransformedTextured(0, 0, 0, 1, 0, 0);
            screenVerts[1] = new CustomVertex.TransformedTextured(this.Width, 0, 0, 1, 1, 0);
            screenVerts[2] = new CustomVertex.TransformedTextured(this.Width, this.Height, 0, 1, 1, 1);
            screenVerts[3] = new CustomVertex.TransformedTextured(0, this.Height, 0, 1, 0, 1);

            m_dxdevice.RenderState.ZBufferEnable = false;
            m_dxdevice.SetTexture(0, texture);
            m_dxdevice.VertexFormat = CustomVertex.TransformedTextured.Format;
            m_dxdevice.DrawUserPrimitives(PrimitiveType.TriangleFan, 2, screenVerts);
            m_dxdevice.RenderState.ZBufferEnable = true;
        }

        #endregion
        /// <summary>
        // In this region all unit testing is done
        // testing engine rendering features
        /// </summary>
        /// 
        #region Unit Testing
        [Test]
        public static void TestGraphicEngineForm()
        {
            //create form and initialize  DirectX
            SinGameEngineForm testForm = new SinGameEngineForm();
            testForm.Show(); // show the window

            //test if device is initialize or not
            Assert.IsNotNull(m_dxdevice);


            //Load all required resources (textures, fonts, model etc)
            Texture skyTexture = TextureLoader.FromFile(m_dxdevice, "..\\game data\\textures\\SpaceSkyCubeMap.dds");


            //load models
            Model rocketModel = new Model(m_dxdevice, "..\\game data\\Models\\Rocket.x");

            //TODO: this part should be change to load all the necessary data 
            //and should be not hard coded.
            //Render Loop
            while(testForm.isNotClosed)
            {
                Application.DoEvents();

                testForm.Render(
                          delegate
                          {
                              //display skybox/background textures
                              testForm.RenderOnScreen(skyTexture);

                              //render models
                              rocketModel.Render(m_dxdevice);
                              //Render Text 
                              testForm.RenderTextOnScreen("~~~Test Font system!!!!");
                          });
            }



        }//TestGraphicEngineForm()
        #endregion
    }
}
///<summary> 
/// engine form display and process game 
/// main entry and exit of the game
///</summary>

#region Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sin_Engine.EngineDevice;
using Microsoft.DirectX.Direct3D;
using Sin_Engine.Properties;
using Microsoft.DirectX;
using Sin_Engine.Graphics;
using Sin_Engine.Helper;
using Sin_Engine.GameLogic;
using FormKeyPress = System.Windows.Forms.Keys;
using Sin_Engine.Shaders;
#if DEBUG
using NUnit.Framework;
#endif
#endregion


namespace Sin_Engine
{
    #region TODO
    ///<summary>
    /// Task to be done for this class
    /// task are listed in order
    ///</summary>
    /*
     1  Create the game object - √
     * 
     2  Error handling         -
     * 
     3  initialize the game objects -
        InitializeGlobalResources();     
     *      
     4  build the game events   -
     * 
     // show the setup form to gather device options
     5  Setup setup = new Setup( options );
        setup.ShowDialog();
     * 
     */
    #endregion
    public class GameForm : Form
    {
        /// <summary>
        /// Declare all the game related variables here
        /// </summary>
        #region Variables
            static Device graphics = null;
            bool isLostFocused = false;  //is game minimize/in background
            bool isGamePaused  = false;  // is game paused
            bool isClosed = false;    // quit game

            private static MultiSampleType multisamplingFormat;
            ///<summary>
            /// load model
            ///</summary>
           Entity3D rocket;
           Entity3D rocket2;

           /// <summary>
           /// Sky cube map class using the PreScreenSkyCubeMapping shader.
           /// </summary>
           protected static PreScreenSkyCubeMapping skyCubeMap = null;

            //create default font to display the text
           GameFont gameInfo = null;

           //place holder
         //  Junk skyBox  = null;

           GameCamera camera; 

           /// <summary>
           /// World, view and projection matrix, also used for the shaders!
           /// </summary>
           private static Matrix mWorldMatrix, mViewMatrix, mProjectionMatrix;
        	
            /// <summary>
        	/// Precalculated view projection matrix for the Convert 3D Point To 2D point ,
		    /// IsInFrontOfCamera and IsVisible methods. Calculated in ViewMatrix.
		    /// </summary>

           private static Matrix mViewProjMatrix;

           #region InputHandlerVariables
                //float prvMouseX = 0.0f;
                //float prvMouseY = 0.0f;
                static Keyboard keyIO = null;
                static Mouse mouseIO = null;   
           #endregion
        #endregion

               ///<summary>
        /// Initialize the object
        ///</summary>
        #region Constructor`
        public GameForm()
        {
            InitializeComponent();

            this.Size = new Size(GameSettings.Default.ScreenWidth,
                            GameSettings.Default.ScreenHeight);
            //this.Width =  GameSettings.Default.ScreenWidth;
            //this.Height = GameSettings.Default.ScreenHeight; 

            if (!GameSettings.Default.FullScreen)
                this.ClientSize = new System.Drawing.Size(this.Width - 8, this.Height - 27);
            else
                this.ClientSize = this.Size;
 
        }
        #endregion

        #region Properties
        /// <summary>
        /// Using multisampling
        /// </summary>
        /// <returns>Bool</returns>
        public static bool UsingMultisampling
        {
            get
            {
                return multisamplingFormat != MultiSampleType.None;
            } // get
        } // UsingMultisampling

        /// <summary>
        /// return the DirectX device
        /// </summary>
        public static Device DirectXDevice
        {
            get{
                return graphics;
            }
        }

        public static Mouse mouse{
            get{
                return mouseIO;
            }
        }
        /// <summary>
        /// set or get world matrix
        /// </summary>
        public static Matrix WorldMatrix
        {
            get{
                return mWorldMatrix;
            }
            set {
                if( mWorldMatrix != value)
                {
                    mWorldMatrix = value;
                    graphics.Transform.World = mWorldMatrix;
                }
            }
        }

        /// <summary>
        /// set or get view matrix
        /// </summary>
        public static Matrix ViewMatrix
        {
            get{
                return mViewMatrix;
            }
            set{
                mViewMatrix = value;
                graphics.Transform.View = mViewMatrix;
                
                //calculate the view projection matrix
                mViewProjMatrix = mViewMatrix * mProjectionMatrix;
            }
        }

        public static Matrix ProjectionMatrix
        {
            get{
                return mProjectionMatrix;
            }
            set {
                mProjectionMatrix = value;
                graphics.Transform.Projection = mProjectionMatrix;
            }
        }
       public static Matrix InverseViewMatrix
       {
           get{
               return Matrix.Invert(mViewMatrix);
           }
       }
       
        public static Vector3 CameraPos
        {
            get{
                Matrix invView = InverseViewMatrix;
                return new Vector3(invView.M41, invView.M42, invView.M43);
                //retunr new Vector3( mViewMatrix.M41, mViewMatrix.M42, mViewMatirx.43);
            }
        }

        #endregion
        #region Methods
        ///<summary>
        /// all directx methods
        ///</summary>
        #region DirectX 
        public void InitializeGraphicsDevice()
        {
            try
            {
                //Get The device capability
                Caps deviceCaps = Manager.GetDeviceCaps(0, DeviceType.Hardware);
                bool bCheckDevice = Manager.CheckDeviceType(0,   //default adapter
                                                        deviceCaps.DeviceType, // Check for hardware render
                                                       Format.X8R8G8B8,         // 32 bit color
                                                       Format.X8R8G8B8,         // same for back buffer
                                                       GameSettings.Default.FullScreen //check for mode
                                                        );
                // if (bCheckDevice)
                {
                    // get the current desktop setting
                    //Format currentDisplayFormat = Manager.Adapters[0].CurrentDisplayMode.Format;

                    PresentParameters presentParams = new PresentParameters();

                    // Always use windowed mode for debugging and fullscreen for.
                    presentParams.Windowed = !GameSettings.Default.FullScreen;

                    presentParams.PresentationInterval  = PresentInterval.Default;
                    presentParams.BackBufferFormat      = Format.X8R8G8B8;
                    presentParams.BackBufferWidth       = this.ClientSize.Width;
                    presentParams.BackBufferHeight      = this.ClientSize.Height;

                    // Default to triple buffering for performance gain,
                    // if we are low on video memory and use multisampling, 1 is ok too.
                    presentParams.BackBufferCount = 1;

                    // Discard back buffer when swapping, its faster
                    presentParams.SwapEffect = SwapEffect.Discard;
                    // No multisampling yet ...
                    multisamplingFormat =
                        presentParams.MultiSample =
                        MultiSampleType.None;

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

                    //create the device
                    graphics = new Device(0, DeviceType.Hardware, this, 
                                                            CreateFlags.HardwareVertexProcessing |
                                                            CreateFlags.PureDevice, presentParams);

                }

            }
            catch (Direct3DXException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }


        /// <summary>
        /// event handler directx device lost events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void InvalidateDeviceObjects(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// event handler when device is reset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void RestoreDeviceObjects(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// event handler disposed device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DeleteDeviceObjects(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// event handler to cancel any form resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void EnvironmentResizing(object sender, CancelEventArgs e)
        {
        }

        #endregion


        #region Build the projection matrix
        void BuildProjection()
        {
            if (graphics != null)
            {
                ///set the world @ origin
                mWorldMatrix = Matrix.Identity;
                graphics.Transform.World = mWorldMatrix;

                //setup the view matrix
                mViewMatrix = Matrix.LookAtLH( new Vector3( -50.0f, 0.0f, -4.0f),
                                               new Vector3(   0.0f, 0.0f,  0.0f),
                                               new Vector3(   0.0f, 1.0f,  0.0f)
                                              );

                graphics.Transform.View = mViewMatrix;

                //calculate the aspect ratio
                float aspectRatio = 1.0f;
                if (this.Height != 0)
                {
                    aspectRatio = (float)this.Width / (float)this.Height;
                }

                //calculate the fov in radian
                float fieldOfView = GameSettings.Default.FOV * ( (float) Math.PI / 180.0f);
                //float fieldOfView = (float)Math.PI / 2.0f;
                //build the projection matrix
                mProjectionMatrix = Matrix.PerspectiveFovLH(fieldOfView, aspectRatio,
                                                               GameSettings.Default.nearPlane,
                                                               GameSettings.Default.farPlane
                                                              );
                graphics.Transform.Projection = mProjectionMatrix;
            }
        }
        #endregion
        /// <summary>
        /// Render the current game frame
        /// </summary>
        void RenderGame()
        {
            if (graphics == null || graphics.Disposed)
                return;
            try
            {
                graphics.Clear( ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
                graphics.BeginScene();

                //render the sky box
              //  skyBox.RenderSkyBox();

                // Render sky cube map as our background.
                skyCubeMap.RenderSky(
                    // Slightly change background zooming when speeding up.
                    //suxx big time: 1.0f + (Player.speedItemTimeout > 0 ? 0.2f : 0.0f),
                    1.0f,
                    // Use sky background color instead of lens flare color
                    // or lighting color.
                    Color.White);

                //render something here
                rocket.Render();
                //rocket2.Render(new Vector3(0.8f, 0.0f, -0.8f));

                //print text 
                gameInfo.PrintText("Test Font system!!!", 0, 0, Color.AliceBlue);
            }
            catch (DeviceLostException ex)
            {
                //catch the device lost exception
                MessageBox.Show(ex.ToString());
                return;
            }
            catch (DeviceNotResetException ex)
            {
                //device has not been reset, bit it can be reacquired now
                MessageBox.Show(ex.ToString());
                graphics.Reset(graphics.PresentationParameters);
                //graphics lost is false
            }
            catch (Direct3DXException ex)
            {
                //Catch DirectX related exception here
                MessageBox.Show(ex.ToString());
            }
            catch (ExecutionEngineException ex)
            {
                //Catch other exception here
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //finally complete the rendering 
                graphics.EndScene();
                graphics.Present();
            }
        }

        void UpdateInput()
        {
            keyIO.Update();
            mouseIO.Update();

            if (keyIO.PauseKey)
               isGamePaused = !isGamePaused;
           if (!isGamePaused)
           {
               if (keyIO.LeftKey)
                   camera.Move(GameCamera.eMoveDirection.XDir, -GameSettings.Default.moveCamera);

               if (keyIO.RightKey)
                   camera.Move(GameCamera.eMoveDirection.XDir, GameSettings.Default.moveCamera);

               if (keyIO.UpKey)
                   camera.Move(GameCamera.eMoveDirection.ZDir, GameSettings.Default.moveCamera);

               if (keyIO.DownKey)
                   camera.Move(GameCamera.eMoveDirection.ZDir, -GameSettings.Default.moveCamera);

               if (mouseIO.MouseWheelMove != 0.0f)
               {
                   camera.Move(GameCamera.eMoveDirection.ZDir, mouseIO.MouseWheelMove);
               }

               if (mouseIO.RightButtonPressed)
               {
                   if (mouseIO.MouseXMove != 0.0f || mouseIO.MouseYMove != 0.0f)
                       camera.Rotate(GameCamera.eRotationAxis.Roll, (mouseIO.MouseXMove - mouseIO.MouseYMove) *
                                                  GameSettings.Default.mouseRoateFactor);

               }
               else 
                   if (mouseIO.MouseXMove != 0.0f || mouseIO.MouseYMove != 0.0f)
                   {
                       camera.Rotate(GameCamera.eRotationAxis.Yaw, -mouseIO.MouseXMove *
                                                           GameSettings.Default.mouseRoateFactor);
                       camera.Rotate(GameCamera.eRotationAxis.Pitch, -mouseIO.MouseYMove *
                                                           GameSettings.Default.mouseRoateFactor);
                   }

           }//end of pause
        }
        /// <summary>
        /// Update the game logic and input every frame
        /// </summary>
        void UpdateGame()
        {
            UpdateInput();
            camera.Update();
        }

        /// <summary>
        /// load all the game data
        /// </summary>
        /// <returns></returns>
        bool LoadGameResources()
        {
            rocket = new Entity3D("game data\\Models\\Rocket.x");
            rocket2 = new Entity3D("game data\\Models\\Rocket.x");
            gameInfo = new GameFont();
            skyCubeMap = new PreScreenSkyCubeMapping();

            return true;
        }
        ///<summary>
        ///the entry point of the game
        /// here all the game initialization and resource allocation done
        /// 
        ///</summary>
        public void StartGame()
        {
            InitializeGraphicsDevice();
            BuildProjection();

            camera = new GameCamera(new Vector3(0.0f, 80.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));
            keyIO = new Keyboard(this);
            mouseIO = new Mouse(this);

            LoadGameResources();
            this.Show();
            Run();
        }

        /// <summary>
        /// the game run loop 
        /// </summary>
        protected void Run()
        {
            while (!isClosed)
            {
                if (!this.isLostFocused )
                {
                    UpdateGame();

                    RenderGame();
                }
                else
                {
                    //lets the windows do what it want to do for other application
                    System.Threading.Thread.Sleep(1);
                }
                Application.DoEvents();    
            }
            
        }
        #endregion


        #region Events
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            isClosed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            isLostFocused = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            isLostFocused = false;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            BuildProjection();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            //this.res
        }
        //handle the key events
        /// <summary>
        /// place holder in future the input handling should be done with DirectInput
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if ((int)e.KeyChar == (int)System.Windows.Forms.Keys.Escape)
            {
                isClosed = true;
                //call to shutdown the engine
                this.Dispose();
            }

            //if ((int)e.KeyChar == (int)System.Windows.Forms.Keys.Pause)
              //  isGamePaused = !isGamePaused;
            
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
#if USE_FORM_KEY
                    int iKeyPressed = (int)e.KeyValue;
                    
                    float moveamount = GameSettings.Default.moveCamera;
                    float angle = GameSettings.Default.rotateCamera;

                    switch (iKeyPressed)
                    {
                        ///<summary>
                        /// Translate camera
                        ///</summary>
                        case (int)FormKeyPress.W:
                        case (int)FormKeyPress.Up:
                            camera.Move(GameCamera.eMoveDirection.YDir, -moveamount);
                            break;
                        case (int)FormKeyPress.S:
                        case (int)FormKeyPress.Down:
                            camera.Move(GameCamera.eMoveDirection.YDir, +moveamount);
                            break;
                        case (int)FormKeyPress.A:
                        case (int)FormKeyPress.Left:
                            camera.Move(GameCamera.eMoveDirection.XDir, +moveamount);
                            break;
                        case (int)FormKeyPress.D:
                        case (int)FormKeyPress.Right:
                            camera.Move(GameCamera.eMoveDirection.XDir, -moveamount);
                            break;
                        case (int)FormKeyPress.Z:
                            camera.Move(GameCamera.eMoveDirection.ZDir, +moveamount);
                            break;
                        case (int)FormKeyPress.X:
                            camera.Move(GameCamera.eMoveDirection.ZDir, -moveamount);
                            break;

                        ///<summary>
                        /// Rotate camera
                        ///</summary>
                        case (int)FormKeyPress.Q:
                            camera.Rotate(GameCamera.eRotationAxis.Yaw, +angle);
                            break;
                        case (int)FormKeyPress.E:
                            camera.Rotate(GameCamera.eRotationAxis.Yaw, -angle);
                            break;

                        case (int)FormKeyPress.N:
                            camera.Rotate(GameCamera.eRotationAxis.Pitch, +angle);
                            break;

                        case (int)FormKeyPress.M:
                            camera.Rotate(GameCamera.eRotationAxis.Pitch, -angle);
                            break;


                        case (int)FormKeyPress.O:
                            camera.Rotate(GameCamera.eRotationAxis.Roll, -angle);
                            break;
                        case (int)FormKeyPress.P:
                            camera.Rotate(GameCamera.eRotationAxis.Roll, +angle);
                            break;
            }
#endif

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }
        #endregion

        //// wizard generated code
        #region WiZardGenerated code
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.SuspendLayout();
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GameForm";
            this.Text = "Sin Game Engine V2";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private void GameForm_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region UnitTesting
#if DEBUG 
        [Test]
        public static void TestGraphicEngineForm()
        {
            GameForm testEngine = new GameForm();
            testEngine.InitializeGraphicsDevice();
            testEngine.BuildProjection();
            testEngine.Show();
            Entity3D rocket = new Entity3D("game data\\Models\\Rocket.x");
            GameFont gameInfo = new GameFont();
            PreScreenSkyCubeMapping skyCubeMap = new PreScreenSkyCubeMapping();

            while( !testEngine.IsDisposed )
            {
                try
                {
                    GameForm.DirectXDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
                    GameForm.DirectXDevice.BeginScene();

                    skyCubeMap.RenderSky(1.0f, Color.Wheat);
                    rocket.Render();
                    gameInfo.PrintText("Game Font Test", 10, 20, Color.Blue);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    GameForm.DirectXDevice.EndScene();
                    GameForm.DirectXDevice.Present();
                }
                Application.DoEvents();    

            }

        }
#endif
        #endregion
    }
}
///<summary>
/// File : Camera.cs
/// Handle all the game cameras e.g Menu, First person ect.
/// This is Quaternion based camera to avoid gimbal lock
/// to learn about Quaternion : http://www.cprogramming.com/tutorial/3d/quaternions.html
/// to konw what is gimbal lock check out this :
/// http://www.anticz.com/eularqua.htm
///  TOPDO : This is class is directx specific, as using some direct class, but in future it should be
/// genreic so it can be used with any graphics APIS's
///</summary>
#region Directives
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
#endregion

namespace Sin_Engine_TryOne.GameLogic
{
   public class Camera
    {
       ///<summary>
       /// Quaternion:  rotation axis and amount of rotation (X, Y, Z, W)
       /// Position  : current camera position
       /// 
       ///</summary>
        #region Variables

            private Quaternion m_CamOrientation = Quaternion.Identity; // store the cam orientation
            private Vector3 m_vCamePos = new Vector3( 0.0f, 0.0f, 0.0f);           // current cam pos

            //input keys ,must be place holder or used only for testing/Free cam
            private Key m_moveForwarKey, m_moveBackwardKey,
                        m_StrafeLeftKey, m_StrafeRightKey,
                        m_rollLeftKey, m_rollRightKey;

            /// <summary>
            /// Default camera up vector (used for Matrix.LookAt calls)
            /// </summary>
            private static readonly Vector3 DefaultCameraUp =
                new Vector3(0.0f, 0.0f, -1.0f);

            #region Enumerated Data Types Declearion
            /// <summary>
           /// decide in which direction camera to move
           /// </summary>
           private enum eMoveDirction
           {
               /// <summary>
               /// Move in X direction (Left/Right)
               /// </summary>
               XDir,
               /// <summary>
               /// Move in Y direction (Up/ Down)
               /// </summary>
               YDir,
               /// <summary>
               /// Move in Z direction (In/Out of screen)
               /// </summary>
               ZDir,
           }

            ///<summary>
            /// decide the rotation axis of camera
            ///</summary>
           private enum eRotationAxis
           {
               /// <summary>
               /// rotation around X axis ()
               /// </summary>
                ePich,
               /// <summary>
               /// rotation around Y axis ()
               /// </summary>
               eYaw,
               /// <summary>
               /// rotation around Z axis ()
               /// </summary>
                eRoll,

           }
            //TODO: 3rd person & 1st person
            //Atach to entity

           /// <summary>
           /// Camera Type e.g Menu Camera, 1st person, 3rd Person etc
           /// </summary>
           public enum eCameraMode
           {
               ///<summary>
               /// Default Camera Mode = MENU
               ///</summary>
               eMenuMode,
               eInGame, // in game camera
               eFreeCamera, //Free roam camera
           };
            #endregion

           ///<summary>
           /// set camera mode
           ///</summary>
           private eCameraMode m_eCameraMode = eCameraMode.eFreeCamera;
        #endregion

        ///<summary>
        /// Construct camera 
        ///</summary>
        #region Constructor
        /// <summary>
        /// Create default camera at origin
        /// 
        /// </summary>
        /// <param name="setCameraPos">Set camera pos</param>
       public Camera() {
            
        }

        /// <summary>
        /// Create Camera at position 
        /// 
        /// </summary>
        /// <param name="setCameraPos">Set camera pos</param>
        public Camera(Vector3 vsetCamPos)
        {
            m_vCamePos          = vsetCamPos;
            m_moveForwarKey     = Key.UpArrow;
            m_moveBackwardKey   = Key.DownArrow;
            m_StrafeLeftKey     = Key.LeftArrow;
            m_StrafeRightKey    = Key.RightArrow;
            m_rollLeftKey       = Key.Z;
            m_rollRightKey      = Key.X;
        }

        #endregion

        ///<summary>
        ///
        ///</summary>

        #region Properties
        public eCameraMode camMode
        {
            set
            {
                m_eCameraMode = value;
            }
            get
            {
                return m_eCameraMode;
            }
        }

       /// <summary>
       /// get the current X axis from view matrix
       /// </summary>
       /// <returns> Vector3</returns>
       public static Vector3 Xaxis
       { 
        get{
            return new Vector3(  SinGameEngineForm.ViewMatrix.M11,
                                 SinGameEngineForm.ViewMatrix.M21,
                                 SinGameEngineForm.ViewMatrix.M31
                            );
        }//get
       }//Xaxis

       /// <summary>
       /// get the current Y axis from view matrix
       /// </summary>
       /// <returns> Vector3</returns>

       public static Vector3 Yaxis
       {
           get
           {
               return new Vector3(  SinGameEngineForm.ViewMatrix.M12,
                                    SinGameEngineForm.ViewMatrix.M22,
                                    SinGameEngineForm.ViewMatrix.M32
                               );
           }//get
       }//Yaxis

       /// <summary>
       /// get the current Z axis from view matrix
       /// </summary>
       /// <returns> Vector3</returns>

       public static Vector3 Zaxis
       {
           get
           {
               return new Vector3(  SinGameEngineForm.ViewMatrix.M13,
                                    SinGameEngineForm.ViewMatrix.M23,
                                    SinGameEngineForm.ViewMatrix.M33
                               );
           }//get
       }//Zaxis

        #endregion

        ///<summary>
        /// 
        ///</summary>
        #region Methods  
        ///<summary>
        /// Set cam position
        ///</summary>>
        public void SetPosition(Vector3 vsetCamPos)
        {
            m_vCamePos = vsetCamPos;
        }

        /// <summary>
        /// update the camera 
        /// </summary>
        public void Update()
        {
            UpdateFreeCamera();
            UpdateViewMatrix();
        }

        #region Handle Input
        /// <summary>
        /// Translate the cam according to amount and direction
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="direction"></param>
        private void Translate( float amount, eMoveDirction eMoveDir)
        {
            Vector3 vdir    =  eMoveDir == eMoveDirction.XDir ? Xaxis : 
                               eMoveDir == eMoveDirction.YDir ? Yaxis : Zaxis;

            m_vCamePos += vdir * amount;
        }
        /// <summary>
        /// update the free camera according to player input
        /// </summary>
        private void UpdateFreeCamera()
        {
             float fMoveFactor = 0.5f;
             //float fStrafeFactor = 0.5f;

            if(SinGameEngineForm.Keyboard.Up)
            {
                Translate( fMoveFactor, eMoveDirction.ZDir);
            }

            if(SinGameEngineForm.Keyboard.Down)
            {
                Translate( -fMoveFactor, eMoveDirction.ZDir);
            }

        }

        #region Update View matrix method
       /// <summary>
       /// update the view matrix
       /// </summary>
        private void UpdateViewMatrix()
        {
            Matrix rotateMatirx;
            m_CamOrientation.Normalize();
            rotateMatirx = Matrix.RotationQuaternion(m_CamOrientation);


            SinGameEngineForm.ViewMatrix = Matrix.Translation ( -m_vCamePos ) * rotateMatirx;
            ///
        }
        #endregion


        #endregion

        #region Helper Methods
        #endregion
        #endregion


        ///<summary>
        ///
        ///</summary>
        #region Unit Testing
        #endregion

    }
}

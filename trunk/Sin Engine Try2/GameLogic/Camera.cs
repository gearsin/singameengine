///<summary>
/// File : Camera.cs
/// Handle all the game cameras e.g Menu, First person ect.
/// This is Quaternion based camera to avoid gimbal lock
/// to learn about Quaternion : http://www.cprogramming.com/tutorial/3d/quaternions.html
/// to konw what is gimbal lock check out this :
/// http://www.anticz.com/eularqua.htm
///</summary>

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
#endregion

namespace Sin_Engine.GameLogic
{
    class GameCamera
    {
        #region Variables
        Quaternion camQut = Quaternion.Identity;
        Vector3 vcamPos  = new Vector3( 0.0f, 0.0f, 0.0f);
      //  Vector3 vcamTarget;
        readonly Vector3 vDefaultCamUp = new Vector3( 0.0f, 1.0f, 0.0f);

        /// <summary>
        /// Rotation matrix, used in UpdateViewMatrix.
        /// </summary>
        private Matrix rotMatrix = Matrix.Identity;

        #region Enumerated data types
        /// <summary>
        /// camera move direction
        /// </summary>
        public enum eMoveDirection
        {
            //Move left/right direction
            Xdir,
            /// <summary>
            ///Move Up/Down direction
            /// </summary>
            YDir,

            /// <summary>
            /// Move in/out direction (ZoomIn/ZoomOut)
            /// </summary>
            ZDir
        }//MoveDirection

        public enum eRotationAxis
        {
            /// <summary>
            /// Rotate Around X-axis (Moving head Up/Down)
            /// </summary>
            Pitch,
            /// <summary>
            /// Rotate around Y-axis (Moving head Left/Right)
            /// </summary>
            Yaw,
            /// <summary>
            /// Rotate around Z-axi (Moving head around nose)
            /// </summary>
            Roll
        }//RotationAxis

        public enum eCameraModes
        {
        }
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// create camera at origin
        /// </summary>
        public  GameCamera()
        {
        }

        /// <summary>
        /// create camera at position
        /// </summary>
        /// <param name="pos"></param>
        public GameCamera(Vector3 pos)
        {
            vcamPos = pos;
        }


        /// <summary>
        /// set the camera position and target (where to look)
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="target"></param>
        public GameCamera(Vector3 pos, Vector3 target) : this(pos)
        {
            SetLookAt(pos, target, vDefaultCamUp);
        }

        #endregion

        #region Properties
        public Quaternion camQutRotation
        {
            get {
                return camQut;
            }
            set{
                camQut = value;
            }
        }//camRotation

        public static Vector3 XAxis{
            get{
                   return new Vector3( GameForm.ViewMatrix.M11, 
                                       GameForm.ViewMatrix.M21,
                                       GameForm.ViewMatrix.M31
                                    );

                }
        }//xAxis

        public static Vector3 YAxis{
            get{
                return new Vector3( GameForm.ViewMatrix.M12,
                                    GameForm.ViewMatrix.M22,
                                    GameForm.ViewMatrix.M32
                                   );
            }
        }//yAxis
        
        public static Vector3 ZAxis{
            get{
                return new Vector3( GameForm.ViewMatrix.M13,
                                    GameForm.ViewMatrix.M23,
                                    GameForm.ViewMatrix.M33
                                   );
            }
        }//zAxis

        public Matrix RotationMatrix
        {
            get{
                return rotMatrix;
            }
        }
        #endregion

        #region Methods
            #region Helper method to get quaternion from LookAt matrix (private)
            /// <summary>
            /// Helper method to get quaternion from LookAt matrix.
            /// </summary>
            /// <param name="setCamPos">Set cam pos</param>
            /// <param name="setLookPos">Set look pos</param>
            /// <param name="setUpVector">Set up vector</param>
            private void SetLookAt( Vector3 setCamPos, Vector3 setLookPos, Vector3 setUpVector)
            {
                vcamPos = setCamPos;

                // Build look at matrix and get the quaternion from that
                camQut = Quaternion.RotationMatrix( Matrix.LookAtLH( vcamPos, setLookPos, setUpVector) );
            } // SetLookAt(setCamPos, setLookPos, setUpVector)
            #endregion

            /// <summary>
            /// set the camera position
            /// </summary>
            /// <param name="pos"></param>
            public void setPosition(Vector3 pos)
            {
                vcamPos = pos;
            }

            #region Camera move and rotation Methods
            /// <summary>
            /// move camera to amount in movement direction
            /// </summary>
            /// <param name="?"></param>
            /// <param name="amount"></param>
            public void Move( eMoveDirection moveDir, float amount)
            {
                Vector3 dir = moveDir == eMoveDirection.Xdir ? XAxis :
                              moveDir == eMoveDirection.YDir ? YAxis : ZAxis;
                vcamPos += dir * amount;
            }

            public void Rotate( eRotationAxis rotAxis, float angle)
            {
                Vector3 axis = rotAxis == eRotationAxis.Pitch ? new Vector3( 1.0f, 0.0f, 0.0f) :
                               rotAxis == eRotationAxis.Roll  ? new Vector3( 0.0f, 1.0f, 0.0f) :        
                                           new Vector3( 0.0f, 0.0f, 1.0f);
                Rotate(axis, angle);
            }   

            private void Rotate( Vector3 axis, float angle)
            {
                camQut *= Quaternion.RotationAxis(axis, angle);
            }
            #endregion

          public void Update()
          {
              UpdateViewMatrix();
          }
          
          private void UpdateViewMatrix()
          {
              camQut.Normalize();
              rotMatrix = Matrix.RotationQuaternion( camQut );

              GameForm.ViewMatrix = Matrix.Translation(-vcamPos) *  rotMatrix;
          } 
        #endregion
        }
}

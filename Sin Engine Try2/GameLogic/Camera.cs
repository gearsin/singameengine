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
        Vector3 vcamTarget;
        readonly Vector3 vDefaultCamUp = new Vector3( 0.0f, 1.0f, 0.0f);        
        #endregion

        #region Constructor
        GameCamera()
        {
        }
        GameCamera(Vector3 pos)
        {
        }

        GameCamera(Vector3 pos, Vector3 UpDir)
        {
        }
        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion
    }
}

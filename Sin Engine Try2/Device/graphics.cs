/// <summary>
/// obsolut for now
/// </summary>


#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.ComponentModel;
using Sin_Engine.Properties;
using System.Windows.Forms;
#endregion

namespace Sin_Engine.EngineDevice
{
    /// <summary>
    /// 
    /// </summary>

    public class GraphicsDevice
    {

        #region Variable
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>


       public GraphicsDevice()
        {
            //InitializeGraphicsDevice();
        }
        #endregion

        #region Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>

        #region Obsolute
        /// <summary>
        /// initialize and create the directx device object
        /// </summary>
        //public void InitializeGraphicsDevice()
        //{
        //    try
        //    {
        //        //Get The device capability
        //        Caps deviceCaps = Manager.GetDeviceCaps(0, DeviceType.Hardware);
        //        bool bCheckDevice = Manager.CheckDeviceType(0,   //default adapter
        //                                                deviceCaps.DeviceType, // Check for hardware render
        //                                               Format.X8R8G8B8,         // 32 bit color
        //                                               Format.X8R8G8B8,         // same for back buffer
        //                                               GameSettings.Default.FullScreen //check for mode
        //                                                );
        //       // if (bCheckDevice)
        //        {
        //            // get the current desktop setting
        //            //Format currentDisplayFormat = Manager.Adapters[0].CurrentDisplayMode.Format;

        //            PresentParameters presentParams = new PresentParameters();
        //            //PresentParameters presentParams = new PresentParameters();
        //            presentParams.Windowed = GameSettings.Default.FullScreen;

        //            presentParams.PresentationInterval = deviceCaps.PresentationIntervals;
        //            presentParams.BackBufferFormat = Format.X8R8G8B8;
        //            presentParams.BackBufferWidth = GameSettings.Default.clientSizeWidth;
        //            presentParams.BackBufferHeight = GameSettings.Default.clientSizeHeight;

        //            presentParams.BackBufferCount = 1;

        //            //discard the back buffer when swapping , its faster
        //            presentParams.SwapEffect = SwapEffect.Discard;

        //            //set the Z- buffer
        //            presentParams.EnableAutoDepthStencil = true;
        //            presentParams.AutoDepthStencilFormat = DepthFormat.D24X8; //

        //            // For windowed, default to PresentInterval.Immediate which will
        //            // wait not for the vertical retrace period to prevent tearing,
        //            // but may introduce tearing. For full screen, default to
        //            // PresentInterval.Default which will wait for the vertical retrace
        //            // period to prevent tearing.
        //            if (presentParams.Windowed)
        //                presentParams.PresentationInterval = PresentInterval.Immediate;

        //            //create the device
        //            dxDevice = new Device(0, DeviceType.Hardware, Program.GetGameObject(), CreateFlags.HardwareVertexProcessing |
        //                                            CreateFlags.PureDevice, presentParams);

        //        }

        //    }
        //    catch (Direct3DXException ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }

        //}
        #endregion
        #endregion

        #region Events
        #endregion

        #region UnitTest
        #endregion
    }
}

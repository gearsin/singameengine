// Project: Rocket Commander, File: PreScreenSkyCubeMapping.cs
// Namespace: RocketCommander.Shaders, Class: PreScreenSkyCubeMapping
// Path: C:\code\Rocket Commander\Shaders, Author: Abi
// Code lines: 684, Size of file: 19,40 KB
// Creation date: 04.11.2005 01:02
// Last modified: 07.11.2005 02:10
// Generated with Commenter by abi.exDream.com

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
#if DEBUG
using NUnit.Framework;
#endif
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace Sin_Engine.Shaders
{
	/// <summary>
	/// Pre screen sky cube mapping, if shaders are not supported a special
	/// fallback to fixed function pipeline rendering is provided also!
	/// </summary>
	public class PreScreenSkyCubeMapping 
	{
		#region Variables
		/// <summary>
		/// Vb screen instance
		/// </summary>
        private VertexBuffer vbScreen;

		/// <summary>
		/// Just render a vertex buffer with the screen coordinates.
		/// No subTexelSize stuff is performed, do that in the fx file.
		/// </summary>

		/// <summary>
		/// Shader effect filename.
		/// </summary>
		const string Filename = "game data\\Shaders\\PreScreenSkyCubeMapping.fx";

		/// <summary>
		/// Sky cube map texture filename.
		/// </summary>
        const string SkyCubeMapFilename = "game data\\Shaders\\SpaceSkyCubeMap.dds";

		/// <summary>
		/// The Cube Map texture for the sky!
		/// </summary>
		private CubeTexture skyCubeMapTexture = null;

		/// <summary>
		/// The shader effect
		/// </summary>
		private Effect effect = null;

		/// <summary>
		/// Effect handles for this shader: viewInverse, ambientColor, scale
		/// and diffuseTexture for our enviroment cube map.
		/// </summary>
		protected EffectHandle viewInverse,
			ambientColor,
			scale,
			diffuseTexture;
		#endregion

		#region Properties
		/// <summary>
		/// Get Direct3D effect, can be null when effect is not valid.
		/// We have to render with normal methods then
		/// (fallback if shader is not supported!)
		/// </summary>
		public Effect D3DEffect
		{
			get
			{
				return effect;
			} // get
		} // D3DEffect

		/// <summary>
		/// Is shader valid? No error while loading?
		/// When this returns true the shader can be used.
		/// </summary>
		public bool Valid
		{
			get
			{
				return effect != null &&
					true;
			} // get
		} // Valid

		/// <summary>
		/// Last used inverse view matrix
		/// </summary>
		private Matrix lastUsedInverseViewMatrix = Matrix.Zero;
		/// <summary>
		/// Set view inverse matrix
		/// </summary>
		protected Matrix InverseViewMatrix
		{
			set
			{
				if (effect != null &&
					viewInverse != null &&
					lastUsedInverseViewMatrix != value)
				{
					lastUsedInverseViewMatrix = value;
					effect.SetValue(viewInverse, value);
				} // if (effect)
			} // set
		} // InverseViewMatrix

		/// <summary>
		/// Last used sky color (internally this is ambientColor)
		/// </summary>
		private Color lastUsedAmbientColor = Color.Empty;//.FromArgb(0, 0, 0, 0);
		/// <summary>
		/// Sky color
		/// </summary>
		public Color SkyColor
		{
			set
			{
				if (effect != null &&
					ambientColor != null &&
					lastUsedAmbientColor != value)
				{
					lastUsedAmbientColor = value;
					effect.SetValue(ambientColor, new Vector4(
	                                            value.R / 255.0f, value.G / 255.0f, 
                                                value.B / 255.0f, value.A / 255.0f) );
				} // if (effect.D3DEffect)
			} // set
		} // SkyColor

		/// <summary>
		/// Last used scale
		/// </summary>
		private float lastUsedScale = 0;
		/// <summary>
		/// Scale for rendering sky cube map, 1.0 is normal, lower is wrapped,
		/// bigger is scaled up (e.g. 2.0 shows only half the area as 1.0).
		/// </summary>
		public float ScaleFactor
		{
			set
			{
				if (effect != null &&
					scale != null &&
					lastUsedScale != value)
				{
					lastUsedScale = value;
					effect.SetValue(scale, value);
				} // if (effect.D3DEffect)
			} // set
		} // ScaleFactor

		/// <summary>
		/// Last used diffuse texture
		/// </summary>
		private CubeTexture lastUsedDiffuseTexture = null;
		/// <summary>
		/// Set diffuse texture
		/// </summary>
		internal CubeTexture DiffuseTexture
		{
			set
			{
				if (effect != null &&
					diffuseTexture != null &&
					value != null &&
					lastUsedDiffuseTexture != value)
				{
					lastUsedDiffuseTexture = value;
					effect.SetValue(diffuseTexture, value);
				} // if (effect.D3DEffect)
			} // set
		} // DiffuseTexture
		#endregion

		#region Constructor
		/// <summary>
		/// Create pre screen sky cube mapping
		/// </summary>
		public PreScreenSkyCubeMapping()
		{
			Reload();
            //create VB 
            vbScreen = new VertexBuffer(
                typeof(CustomVertex.PositionTextured),
                4,
                GameForm.DirectXDevice,
                Usage.WriteOnly,
                CustomVertex.PositionTextured.Format,
                Pool.Managed);

            GraphicsStream gfxStream = vbScreen.Lock(0, 0, 0);
            CustomVertex.PositionTextured[] vertices =
                new CustomVertex.PositionTextured[]
				{
					new CustomVertex.PositionTextured(
					new Vector3(-1.0f, -1.0f, 0.5f),
					0, 1),
					new CustomVertex.PositionTextured(
					new Vector3(-1.0f, 1.0f, 0.5f),
					0, 0),
					new CustomVertex.PositionTextured(
					new Vector3(1.0f, -1.0f, 0.5f),
					1, 1),
					new CustomVertex.PositionTextured(
					new Vector3(1.0f, 1.0f, 0.5f),
					1, 0),
				};

            gfxStream.Write(vertices);
            vbScreen.Unlock();

		} // PreScreenSkyCubeMapping()
		#endregion

		#region Dispose
		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			// Dispose old shader
			if (effect != null)
			{
				try
				{
					skyCubeMapTexture.Dispose();
					effect.Dispose();
				} // try
				catch (Exception ex)
				{
						MessageBox.Show(ex.ToString());
                    
				} // catch
			} // if (effect)
			skyCubeMapTexture = null;
			effect = null;
		} // Dispose()
		#endregion

		#region IGraphicObject Members
		/// <summary>
		/// On reset device
		/// </summary>
		public void OnResetDevice()
		{
		} // OnResetDevice()

		/// <summary>
		/// On lost device
		/// </summary>
		public void OnLostDevice()
		{
		} // OnLostDevice()
		#endregion

		#region Reload effect
		/// <summary>
		/// Reload
		/// </summary>
		public void Reload()
		{
			// Dispose old shader
			Dispose();

			string fullFilename =  Filename;
			if (File.Exists(fullFilename) == false)
				throw new FileNotFoundException(
					"Can't load shader without valid shader filename",
					fullFilename);

			// Load it
			string effectError = "";
			try
			{
				effect = Effect.FromFile(GameForm.DirectXDevice,
					// Just load, no special stuff used.
					fullFilename, null, null, ShaderFlags.None, null, out effectError);
			} // try
			catch (Exception ex)
			{
				// Only use exception text if no effectError was logged
				if (String.IsNullOrEmpty(effectError))
					effectError = ex.ToString();
			} // catch

			// Error happened and no exception was thrown yet?
			if (String.IsNullOrEmpty(effectError) == false)
				throw new Exception(
					"Failed to load shader " + this.ToString() + ": " + effectError);

			// Reset and get all avialable parameters.
			ResetParameters();
			GetParameters();

			//GameForm.AddGraphicObject(this);
		} // Reload()
		#endregion

		#region Reset parameters
		/// <summary>
		/// Reset parameters
		/// </summary>
		protected void ResetParameters()
		{
			lastUsedInverseViewMatrix = Matrix.Zero;
			lastUsedScale = 0.0f;
			lastUsedAmbientColor = Color.Empty;
			lastUsedDiffuseTexture = null;
		} // ResetParameters()
		#endregion

		#region Get parameters
		/// <summary>
		/// Reload
		/// </summary>
		protected void GetParameters()
		{
			// Can't get parameters if loading failed!
			if (effect == null)
				return;

			string fullCubeMapFilename =	SkyCubeMapFilename;
			if (File.Exists(fullCubeMapFilename) == false)
				throw new FileNotFoundException("File " + fullCubeMapFilename +
					" does not exists for PreScreenSkyCubeMapping shader.");

			try
			{
				viewInverse =
					effect.GetParameter(null, "viewInverse");
				scale =
					effect.GetParameter(null, "scale");
				ambientColor =
					effect.GetParameter(null, "ambientColor");
				diffuseTexture =
					effect.GetParameter(null, "diffuseTexture");

				// Load and set cube map texture
				skyCubeMapTexture = TextureLoader.FromCubeFile(
					GameForm.DirectXDevice, fullCubeMapFilename);
				DiffuseTexture = skyCubeMapTexture;

				// Set sky color to nearly white and scale to 1
				SkyColor = Color.FromArgb(255, 232, 232, 232);
				ScaleFactor = 1.0f;

				// Select SkyCubeMap technique
				effect.Technique = "SkyCubeMap";
			} // try
			catch (Direct3DXException ex)
			{
                MessageBox.Show(ex.ToString());

				// Kill effect
				effect.Dispose();
				effect = null;
			} // catch
		} // GetParameters()
		#endregion

		#region Render shader
		/// <summary>
		/// Render sky with help of shader.
		/// </summary>
		public void RenderSky(float setWrappingScale, Color setSkyColor)
		{
			// Can't render with shader if shader is not valid!
			if (this.Valid == false)
				return;

			try
			{
				// Don't use or write to the z buffer
				GameForm.DirectXDevice.RenderState.ZBufferEnable = false;
				GameForm.DirectXDevice.RenderState.ZBufferWriteEnable = false;

				// Also don't use any kind of blending.
				GameForm.DirectXDevice.RenderState.AlphaBlendEnable = false;

				ScaleFactor = setWrappingScale;
				SkyColor = setSkyColor;
				// Rotate view matrix by level number, this way we look to a different
				// direction depending on which level we are in.
				Matrix invViewMatrix = GameForm.InverseViewMatrix *
					Matrix.RotationY( -1.0f * (float)Math.PI / 2.0f);
				InverseViewMatrix = invViewMatrix;

				try
				{
					int passes = effect.Begin(0);
					for (int pass = 0; pass < passes; pass++)
					{
						effect.BeginPass(pass);
                        // Rendering is pretty straight forward
                        GameForm.DirectXDevice.VertexFormat =
                            CustomVertex.PositionTextured.Format;
                        GameForm.DirectXDevice.SetStreamSource(
                            0, vbScreen,
                            0, CustomVertex.PositionTextured.StrideSize);
                        GameForm.DirectXDevice.DrawPrimitives(
                            PrimitiveType.TriangleStrip, 0, 2);
                        effect.EndPass();
					} // for (pass, <, ++)
				} // try
				finally
				{
					effect.End();
				} // finally
			} // try
			catch (Exception ex)
			{
                MessageBox.Show(ex.ToString());				// Make shader invalid, so we don't call it anymore
                effect = null;
			} // catch
			finally
			{
				// Enable z buffer again
				GameForm.DirectXDevice.RenderState.ZBufferEnable = true;
				GameForm.DirectXDevice.RenderState.ZBufferWriteEnable = true;
			} // finally
		} // RenderSky(wrappingScale, setSkyColor)

		/// <summary>
		/// Render sky
		/// </summary>
		public void RenderSky()
		{   
			RenderSky(lastUsedScale, lastUsedAmbientColor);
		} // RenderSky()
		#endregion

	} // class PreScreenSkyCubeMapping
} // namespace RocketCommander.Shaders

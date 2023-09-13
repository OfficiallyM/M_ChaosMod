using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Modules.Utilities
{
	public static class GUI
	{
		/// <summary>
		/// Create a colored texture.
		/// </summary>
		/// <param name="width">Texture width</param>
		/// <param name="height">Texture height</param>
		/// <param name="color">Texture color</param>
		/// <returns>Texture</returns>
		public static Texture2D ColorTexture(int width, int height, Color color)
		{
			Color[] pixels = new Color[width * height];
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = color;
			}
			Texture2D texture = new Texture2D(width, height);
			texture.SetPixels(pixels);
			texture.Apply();
			return texture;
		}
	}
}

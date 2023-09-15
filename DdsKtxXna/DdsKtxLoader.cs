using System;
using System.IO;
using DdsKtxSharp;
using Microsoft.Xna.Framework.Graphics;
using static DdsKtxSharp.DdsKtx;

namespace DdsKtxXna
{
	public class DdsKtxLoader
	{
		private static SurfaceFormat ToSurfaceFormat(ddsktx_format src)
		{
			SurfaceFormat format = SurfaceFormat.Color;

			switch (src)
			{
				case ddsktx_format.DDSKTX_FORMAT_BC1:
					format = SurfaceFormat.Dxt1;
					break;

				case ddsktx_format.DDSKTX_FORMAT_BC2:
					format = SurfaceFormat.Dxt3;
					break;

				case ddsktx_format.DDSKTX_FORMAT_BC3:
					format = SurfaceFormat.Dxt5;
					break;

				default:
					throw new Exception("Format " + src.ToString() + "isn't supported.");

			}

			return format;
		}

		private static byte[] LoadFace(DdsKtxParser parser, int faceIndex)
		{
			ddsktx_texture_info info = parser.Info;

			ddsktx_sub_data sub_data;
			var imageData = parser.GetSubData(0, faceIndex, 0, out sub_data);

			switch (info.format)
			{
				case ddsktx_format.DDSKTX_FORMAT_BGRA8:
					// Switch B and R
					for (var i = 0; i < imageData.Length / 4; ++i)
					{
						var temp = imageData[i * 4];
						imageData[i * 4] = imageData[i * 4 + 2];
						imageData[i * 4 + 2] = temp;
						imageData[i * 4 + 3] = 255;
					}

					break;

				case ddsktx_format.DDSKTX_FORMAT_RGB8:
					// Add alpha channel
					var newImageData = new byte[info.width * info.height * 4];
					for (var i = 0; i < newImageData.Length / 4; ++i)
					{
						newImageData[i * 4] = imageData[i * 3 + 2];
						newImageData[i * 4 + 1] = imageData[i * 3 + 1];
						newImageData[i * 4 + 2] = imageData[i * 3];
						newImageData[i * 4 + 3] = 255;
					}

					imageData = newImageData;
					break;
			}

			return imageData;
		}

		public static Texture2D Texture2DFromStream(GraphicsDevice device, Stream stream)
		{
			DdsKtxParser parser = DdsKtxParser.FromMemory(stream.ToByteArray());
			ddsktx_texture_info info = parser.Info;

			var format = ToSurfaceFormat(info.format);
			var imageData = LoadFace(parser, 0);

			Texture2D texture = new Texture2D(device, info.width, info.height, false, format);
			texture.SetData(imageData);

			return texture;
		}

		public static TextureCube TextureCubeFromStream(GraphicsDevice device, Stream stream)
		{
			DdsKtxParser parser = DdsKtxParser.FromMemory(stream.ToByteArray());
			ddsktx_texture_info info = parser.Info;

			var format = ToSurfaceFormat(info.format);

			var texture = new TextureCube(device, info.width, false, format);
			texture.SetData(CubeMapFace.PositiveX, LoadFace(parser, 0));
			texture.SetData(CubeMapFace.NegativeX, LoadFace(parser, 1));
			texture.SetData(CubeMapFace.PositiveY, LoadFace(parser, 2));
			texture.SetData(CubeMapFace.NegativeY, LoadFace(parser, 3));
			texture.SetData(CubeMapFace.PositiveZ, LoadFace(parser, 4));
			texture.SetData(CubeMapFace.NegativeZ, LoadFace(parser, 5));

			return texture;
		}
	}
}

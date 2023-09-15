using System;
using System.IO;
using DdsKtxSharp;
using Microsoft.Xna.Framework.Graphics;
using static DdsKtxSharp.DdsKtx;

namespace DdsKtxXna
{
	public class DdsKtxLoader
	{
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

				default:
					throw new Exception("Format " + info.format.ToString() + "isn't supported.");
			}

			return imageData;
		}

		public static Texture FromStream(GraphicsDevice device, Stream stream)
		{
			DdsKtxParser parser = DdsKtxParser.FromMemory(stream.ToByteArray());
			ddsktx_texture_info info = parser.Info;

			var format = SurfaceFormat.Color;
			switch (info.format)
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

				case ddsktx_format.DDSKTX_FORMAT_BGRA8:
					break;

				case ddsktx_format.DDSKTX_FORMAT_RGB8:
					break;

				default:
					throw new Exception($"Format {info.format} is not supported.");
			}

			// Load data
			var isCubeMap = (info.flags & ddsktx_texture_flags.DDSKTX_TEXTURE_FLAG_CUBEMAP) != 0;

			Texture result;

			if (!isCubeMap)
			{
				Texture2D texture2D = new Texture2D(device, info.width, info.height, false, format);

				var imageData = LoadFace(parser, 0);
				texture2D.SetData(imageData);

				result = texture2D;
			} else
			{
				var textureCube = new TextureCube(device, info.width, false, format);
				textureCube.SetData(CubeMapFace.PositiveX, LoadFace(parser, 0));
				textureCube.SetData(CubeMapFace.NegativeX, LoadFace(parser, 1));
				textureCube.SetData(CubeMapFace.PositiveY, LoadFace(parser, 2));
				textureCube.SetData(CubeMapFace.NegativeY, LoadFace(parser, 3));
				textureCube.SetData(CubeMapFace.PositiveZ, LoadFace(parser, 4));
				textureCube.SetData(CubeMapFace.NegativeZ, LoadFace(parser, 5));

				result = textureCube;
			}

			return result;
		}
	}
}

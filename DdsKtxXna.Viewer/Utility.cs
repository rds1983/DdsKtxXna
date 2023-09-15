using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;

namespace DdsKtxXna
{
	internal static class Utility
	{
		public static Texture2D FaceToTexture(this TextureCube texture, CubeMapFace face)
		{
			var data = new Color[texture.Size * texture.Size];
			texture.GetData(face, data);

			var result = new Texture2D(MyraEnvironment.GraphicsDevice, texture.Size, texture.Size);
			result.SetData(data);

			return result;
		}
	}
}

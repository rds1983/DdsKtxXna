using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DdsKtxXna.Tests
{
	[TestFixture]
	public class DdsKtxXnaTests
	{
		private static readonly Assembly _assembly = typeof(DdsKtxXnaTests).Assembly;

		[Test]
		public void TestTexture2D()
		{
			string imageName = "background.dds";

			Texture2D texture;
			using (var stream = _assembly.OpenResourceStream(imageName))
			{
				texture = DdsKtxLoader.Texture2DFromStream(TestsEnvironment.GraphicsDevice, stream);
			}

			Assert.AreEqual(texture.LevelCount, 1);
			Assert.AreEqual(texture.Width, 1024);
			Assert.AreEqual(texture.Height, 1024);
			Assert.AreEqual(texture.LevelCount, 1);

			Assert.AreEqual(texture.Format, SurfaceFormat.Dxt1);
		}

		[Test]
		public void TestTextureCube()
		{
			string imageName = "SkyBox.dds";

			TextureCube texture;
			using(var stream = _assembly.OpenResourceStream(imageName))
			{
				texture = DdsKtxLoader.TextureCubeFromStream(TestsEnvironment.GraphicsDevice, stream);
			}

			Assert.AreEqual(texture.LevelCount, 1);
			Assert.AreEqual(texture.Size, 512);
			Assert.AreEqual(texture.Format, SurfaceFormat.Color);
		}
	}
}

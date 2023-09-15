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

		[TestCase("background.dds", 1, 1024, 1024, SurfaceFormat.Dxt1)]
		[TestCase("fire.dds", 1, 64, 64, SurfaceFormat.Dxt5)]
		public void TestTexture2D(string imageName, int levels, int width, int height, SurfaceFormat format)
		{
			Texture texture;
			using (var stream = _assembly.OpenResourceStream(imageName))
			{
				texture = DdsKtxLoader.FromStream(TestsEnvironment.GraphicsDevice, stream);
			}

			Assert.IsInstanceOf<Texture2D>(texture);

			var texture2D = (Texture2D)texture;
			Assert.AreEqual(levels, texture2D.LevelCount);
			Assert.AreEqual(width, texture2D.Width);
			Assert.AreEqual(height, texture2D.Height);
			Assert.AreEqual(format, texture2D.Format);
		}

		[Test]
		public void TestTextureCube()
		{
			string imageName = "SkyBox.dds";

			Texture result;
			using(var stream = _assembly.OpenResourceStream(imageName))
			{
				result = DdsKtxLoader.FromStream(TestsEnvironment.GraphicsDevice, stream);
			}

			Assert.IsInstanceOf<TextureCube>(result);

			var textureCube = (TextureCube)result;
			Assert.AreEqual(textureCube.LevelCount, 1);
			Assert.AreEqual(textureCube.Size, 512);
			Assert.AreEqual(textureCube.Format, SurfaceFormat.Color);
		}
	}
}

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

		[TestCase("background.dds", 11, 1024, 1024, SurfaceFormat.Dxt1)]
		[TestCase("fire.dds", 7, 64, 64, SurfaceFormat.Dxt5)]
		[TestCase("wave0.dds", 10, 512, 512, SurfaceFormat.Color)]
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

		[TestCase("SkyBox.dds", 1, 512, SurfaceFormat.Color)]
		[TestCase("Sky2.dds", 10, 512, SurfaceFormat.Dxt1)]
		public void TestTextureCube(string imageName, int levelCount, int size, SurfaceFormat format)
		{
			Texture result;
			using(var stream = _assembly.OpenResourceStream(imageName))
			{
				result = DdsKtxLoader.FromStream(TestsEnvironment.GraphicsDevice, stream);
			}

			Assert.IsInstanceOf<TextureCube>(result);

			var textureCube = (TextureCube)result;
			Assert.AreEqual(levelCount, textureCube.LevelCount);
			Assert.AreEqual(size, textureCube.Size);
			Assert.AreEqual(format, textureCube.Format);
		}
	}
}

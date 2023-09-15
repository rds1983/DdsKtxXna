using System.IO;

namespace DdsKtxXna
{
	internal static class Utility
	{
		public static byte[] ToByteArray(this Stream stream)
		{
			byte[] bytes;
			
			using (var ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				bytes = ms.ToArray();
			}

			return bytes;
		}
	}
}

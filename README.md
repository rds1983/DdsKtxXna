# DdsKtxXna
[![NuGet](https://img.shields.io/nuget/v/DdsKtxXna.Monogame.svg)](https://www.nuget.org/packages/DdsKtxXna.Monogame/) [![Chat](https://img.shields.io/discord/628186029488340992.svg)](https://discord.gg/ZeHxhCY)

MonoGame/FNA Library for loading DDS and KTX images.

# Adding Reference for MonoGame
https://www.nuget.org/packages/DdsKtxXna.MonoGame/

# Usage
```c#
Texture texture;
using(var stream = File.OpenRead("image.dds"))
{
  texture = DdsKtxLoader.FromStream(GraphicsDevice, stream);
}

Texture2D texture2d = texture as Texture2D;
if (texture2d != null)
{
  // Process Texture2D...
}

TextureCube textureCube = texture as TextureCube;
if (textureCube != null)
{
  // Process TextureCube...
}

```


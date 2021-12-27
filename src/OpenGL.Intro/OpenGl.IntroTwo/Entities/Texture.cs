using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Collections.Generic;

namespace OpenGl.IntroTwo.Entities
{
    internal class Texture
    {
        public int Id { get; set; } = -1;
        private readonly string _texturePath;
        public Texture(string textureName)
        {
            _texturePath = textureName;
        }

        public Texture Create()
        {
            string root = "./Textures";
            Id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Id);
            var image = Image.Load<Rgba32>(Path.Combine(root, _texturePath));
            var pixels = GetPixels(image);
            GL.TexImage2D(TextureTarget.Texture2D, 0, 
                PixelInternalFormat.Rgba, image.Width, 
                    image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return this;
        }

        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, Id);
        }

        private List<byte> GetPixels(Image<Rgba32> image)
        {
            var pixels = new List<byte>();
            for (int y = 0; y < image.Height; y++)
            {
                var row = image.GetPixelRowSpan(y);

                for (int x = 0; x < image.Width; x++)
                {
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].A);
                }
            }
            return pixels;
        }
    }
}

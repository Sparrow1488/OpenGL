using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;

namespace GraphicEngine.V1.Entities
{
    public class Texture
    {
        public int Id { get; set; } = -1;
        //private byte[] Pixels { get; set; } = new byte[0];
        public string ShaderName { get; set; } = "texture0";
        private readonly string _texturePath = string.Empty;

        public Texture(string textureName)
        {
            _texturePath = textureName;
        }

        public Texture CombineTexture(Texture texture, Shader usingShader)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture.Id);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, Id);

            GL.Uniform1(GL.GetUniformLocation(usingShader.Id, "texture1"), 0);
            GL.Uniform1(GL.GetUniformLocation(usingShader.Id, "texture2"), 1);
            return this;
        }

        public Texture SetShaderName(string shaderName)
        {
            ShaderName = shaderName;
            return this;
        }

        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, Id);
        }

        public Texture Create()
        {
            string root = "./Textures";
            Id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Id);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            var image = Image.Load<Rgba32>(Path.Combine(root, _texturePath));
            image.Mutate(x => x.Flip(FlipMode.Vertical)); // для корректного отображения по вертикали
            var pixels = GetPixels(image);
            //Pixels = pixels;

            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba, image.Width,
                    image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            
            return this;
        }

        private byte[] GetPixels(Image<Rgba32> image)
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
            return pixels.ToArray();
        }
    }
}

using OpenGl.SapperTK.Entities;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenGl.SapperTK.DrawTools
{
    internal static class UIElements
    {
        public static int CreateButton(float[] vertices)
        {
            return CreateElement(vertices, new uint[]
            {
                0, 1, 2,
                0, 3, 2
            });
        }

        public static void DrawElement(int elem, int shaderProg)
        {
            GL.UseProgram(shaderProg);
            GL.BindVertexArray(elem);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        public static int CreateCenterQuadre(float size)
        {
            float half = size / 2;
            var vertices = new float[]
            {
                -half, -half, 0f,
                -half, half, 0f,
                half, half, 0f,
                half, -half, 0f
            };
            var indices = new uint[]
            {
                0, 1, 2,
                0, 2, 3
            };
            return CreateElement(vertices, indices);
        }
        
        /// <summary>
        /// Используя тип uniform в шейдерной программе, плавно анимируем затухание, появление цвета
        /// </summary>
        public static void UniformColorAnimate(Shader shader, string uniformValue)
        {
            var time = GLFW.GetTime();
            var greenValue = Math.Sin(time) / 2f + 0.5f;
            var redValue = Math.Cos(time) / 2f + 0.5f;
            var vertexColorLocation = GL.GetUniformLocation(shader.UID, uniformValue);
            GL.Uniform4(vertexColorLocation, new Color4((float)redValue, (float)greenValue, 0f, 1.0f));
        }

        public static void UniformAnimateCircle(Shader shader, string uniformValue)
        {
            var time = GLFW.GetTime();
            var x = Math.Sin(time) / 2f + 0.2f;
            var y = Math.Cos(time) / 2f + 0.2f;
            var location = GL.GetUniformLocation(shader.UID, uniformValue);
            GL.Uniform4(location, new Vector4((float)x, (float)y, 0f, 0f));
        }

        private static float _xLineAnimValue = 0f;
        private static float _xLineAnimCoef= 0.05f;
        public static void UniformAnimatePingPong(Shader shader, string uniformValue)
        {
            _xLineAnimValue += _xLineAnimCoef / 2f;
            var location = GL.GetUniformLocation(shader.UID, uniformValue);
            GL.Uniform4(location, new Vector4(_xLineAnimValue, 0.0f, 0.0f, 0.0f));
            if (_xLineAnimValue >= 1 || _xLineAnimValue <= -1)
                _xLineAnimCoef = -_xLineAnimCoef;
        }

        /// <returns>VAO</returns>
        public static int CreateElement(float[] vertices, uint[] indices)
        {
            int verticesBufferObject;
            int elementsBufferObject;
            int vertexArrayObject;

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexArrayObject);

            verticesBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferObject);

            elementsBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementsBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementsBufferObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);

            return vertexArrayObject;
        }

        public static int CreateTextureElement(float[] vertices, uint[] indices)
        {
            int verticesBufferObject;
            int elementsBufferObject;
            int vertexArrayObject;

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexArrayObject);

            verticesBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferObject);

            elementsBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementsBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementsBufferObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.BindVertexArray(0);

            return vertexArrayObject;
        }

        public static int CreateColorElement(float[] vertices)
        {
            var vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vao);

            var vbo = GL.GenVertexArray();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            return vao;
        }

        /// <summary>
        /// Отрисовывать через GL.DrawArrays(Triangle, 0, 3)
        /// </summary>
        /// <returns>VAO</returns>
        public static int CreateRainbowElement(float[] vertAndColors)
        {
            var vertexShaderSource = "./Shaders/Custom/Static/vertex2.glsl";
            var fragmentShaderSource = "./Shaders/Custom/Static/fragment2.glsl";
            var shaderProgram = new Shader(vertexShaderSource, fragmentShaderSource).UID;
            GL.UseProgram(shaderProgram);
            return CreateColorElement(vertAndColors);
        }

        public static int CreateTexture(string textureName)
        {
            var img = CreateImage("./Textures/" + textureName);
            var texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(
                TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                    img.image.Width, img.image.Height, 0, PixelFormat.Rgba, 
                        PixelType.UnsignedByte, img.pixels.ToArray());
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return texture;
        }

        private static (SixLabors.ImageSharp.Image<Rgba32> image, List<byte> pixels) CreateImage(string path)
        {
            var image = SixLabors.ImageSharp.Image.Load<Rgba32>(path);
            var pixels = new List<byte>(4 * image.Width * image.Height);

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

            return (image, pixels);
        }

        public static int CreateTextureElement(float[] vertices)
        {
            var vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vao);

            var vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            return vao;

        }

        //public static int CreateTextureElement(float[] vertices)
        //{
        //    var vao = GL.GenVertexArray();
        //    GL.BindVertexArray(vao);
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, vao);

        //    var vbo = GL.GenBuffer();
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        //    GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

        //    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        //    GL.EnableVertexAttribArray(0);

        //    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
        //    GL.EnableVertexAttribArray(0);

        //    GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        //    GL.EnableVertexAttribArray(2);
        //    return vao;
        //}
    }
}

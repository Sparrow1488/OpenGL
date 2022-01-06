using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace GraphicEngine.V1.Entities
{
    public class GameObject
    {
        public int Id { get; set; }
        public Color4 UniColor { get; set; }
        public string Name { get; set; }
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public bool Colored { get; set; }
        public Shader Shader { get; set; }
        protected byte[] _colorId = new byte[3];
        public IList<Texture> Textures { get; set; }
        private Engine _engine;
        public event OnSelectedHandler OnSelected;
        public delegate void OnSelectedHandler();

        public GameObject()
        {
            Textures = new List<Texture>();
            Colored = false;

            _engine = new Engine();
            Indices = new uint[0];
            Vertices = new float[0];
            Name = $"game_object";
        }

        public virtual GameObject Create(float[] vertices, uint[] indices)
        {
            Vertices = vertices;
            Indices = indices;

            Id = _engine.CreateWithoutBinding(vertices, indices);
            UniColor = new Color4((byte)(Id + 100), (byte)(Id + 50), (byte)(Id + 50), 1);

            return this;
        }

        public virtual GameObject Create()
        {
            if(Vertices is null || Indices is null)
                throw new ArgumentNullException($"{nameof(Vertices)} or {nameof(Indices)} was null");
            return Create(Vertices, Indices);
        }

        public GameObject IsColored()
        {
            Colored = true;
            return this;
        }

        public GameObject SetName(string objectName)
        {
            if (objectName is null)
                throw new ArgumentNullException($"{nameof(objectName)} was null");
            Name = objectName;
            return this;
        }

        public GameObject Use(Shader shader)
        {
            Shader = shader;
            return this;
        }

        public GameObject Add(Texture texture)
        {
            if (texture is null)
                throw new ArgumentNullException($"Texture {nameof(texture)} was null");
            Textures.Add(texture);
            return this;
        }

        public GameObject ChangeShaderColor(Color4 color, string uniformName)
        {
            Shader.Use();
            var uniformLocation = GL.GetUniformLocation(Shader.Id, uniformName);
            if (uniformLocation == -1)
                Console.WriteLine("Не удалось найти атрибут uniform с названием " + uniformName);
            GL.Uniform4(uniformLocation, color);
            return this;
        }

        public virtual void Draw()
        {
            GL.BindVertexArray(Id);
            if (!(Shader is null)) {
                Shader.Use();
            }

            if (!(Textures is null) && Textures.Count > 0) {
                OnTexturesDraw();
            }
            else if (Colored) {
                OnColoredDraw();
            }
            else {
                OnSimpleDraw();
            }

            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        public bool IsSelected(float x, float y)
        {
            bool isSelected = false;
            if(Vertices[0] < x && Vertices[6] > x &&
               Vertices[1] < y && Vertices[4] > y)
            {
                isSelected = true;
                OnSelected?.Invoke();
            }
            return isSelected;
        }

        public bool IsSelected_Color(int xPixel, int yPixel)
        {
            var result = false;
            var shader = new Shader("vertex1.glsl", "fragment1.glsl", "Pick_Colored").Create();
            shader.SetVector4("ColorId", new Color4((byte)Id, (byte)Id, (byte)Id, 1)).Use();
            Draw();
            var pixels = new byte[3];
            GL.ReadPixels(xPixel, yPixel, 1, 1, PixelFormat.Rgb, PixelType.Byte, pixels);
            if(pixels[0] == Id && pixels[1] == Id && pixels[2] == Id)
            {
                result = true;
            }
            return result;
        }

        protected virtual void OnTexturesDraw()
        {
            EnableTextures();

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }

        protected virtual void OnColoredDraw()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        }

        protected virtual void OnSimpleDraw()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        private void EnableTextures()
        {
            Shader.Use();
            for (int i = 0; i < Textures.Count; i++)
            {
                var currentTexture = Textures[i];

                if (Enum.TryParse<TextureUnit>($"Texture{i}", out var textureUnit))
                {
                    GL.ActiveTexture(textureUnit);
                    GL.BindTexture(TextureTarget.Texture2D, currentTexture.Id);

                    var uniformLocation = GL.GetUniformLocation(Shader.Id, currentTexture.ShaderName);
                    if(uniformLocation == -1)
                    {
                        Console.WriteLine("Не удалось найти аттрибут Uniform в шейдере, shaderName: {0}", currentTexture.ShaderName);
                    }
                    else
                    {
                        GL.Uniform1(uniformLocation, i);
                    }
                }
            }
        }
    }
}

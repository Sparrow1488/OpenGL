using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace GraphicEngine.V1.Entities
{
    public class GameObject
    {
        public int Id { get; set; }
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public bool Colored { get; set; }
        public Shader Shader { get; set; }
        public IList<Texture> Textures { get; set; }
        private Engine _engine;

        public GameObject()
        {
            Textures = new List<Texture>();
            Colored = false;

            _engine = new Engine();
            Indices = new uint[0];
            Vertices = new float[0];
        }

        public GameObject Create(float[] vertices, uint[] indices)
        {
            Vertices = vertices;
            Indices = indices;

            Id = _engine.CreateWithoutBinding(vertices, indices);

            return this;
        }

        public GameObject IsColored()
        {
            Colored = true;
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

        public virtual void Draw()
        {
            GL.BindVertexArray(Id);
            if (!(Shader is null))
            {
                Shader.Use();
            }

            if (!(Textures is null) || Textures.Count > 0)
            {
                EnableTextures();

                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            }
            else if (Colored)
            {
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            }
            else
            {
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            }
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
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

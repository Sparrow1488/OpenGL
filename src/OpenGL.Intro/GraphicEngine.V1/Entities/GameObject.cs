using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace GraphicEngine.V1.Entities
{
    public class GameObject
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; }
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public bool Colored { get; set; }
        public Shader Shader { get; set; }
        public Shader SelectableShader { get; set; }
        private Engine _engine;
        public IList<Texture> Textures { get; set; }
        public Color4 UniColor { get; set; }
        public Color4 SelectColor { get; set; } = new Color4(5, 150, 5, 1);

        public event OnSelectedHandler OnSelected;
        public delegate void OnSelectedHandler();
        public event OnUnSelectedHandler OnUnSelected;
        public delegate void OnUnSelectedHandler();

        private Matrix4 _model;
        private Matrix4 _view;
        private Matrix4 _projection;

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
            SelectableShader = new Shader("ver.glsl", "fra.glsl", "Selectable").Create();

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

        public GameObject SetMatrixes(Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            _model = model;
            _view = view;
            _projection = projection;
            Shader?.SetMatrix4("model", model)?
                   .SetMatrix4("view", view)?
                   .SetMatrix4("projection", projection);
            SelectableShader?.SetMatrix4("model", model)?
                             .SetMatrix4("view", view)?
                             .SetMatrix4("projection", projection);
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

        public void Select()
        {
            Shader.SetVector4("SelectColor", SelectColor);
            OnSelected?.Invoke();
        }

        public void UnSelect()
        {
            Shader.SetVector4("SelectColor", new Color4(0, 0, 0, 0));
            OnUnSelected?.Invoke();
        }

        public GameObject ChangeShaderColor(Color4 color, string uniformName)
        {
            Shader.SetVector4(uniformName, color);
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
            OnDraw();
        }

        public virtual void DrawSelectable()
        {
            GL.BindVertexArray(Id);
            if (!(SelectableShader is null))
            {
                SelectableShader.SetVector4("UniColor", new Color4((byte)(Id + 100), 50, 50, 1));
                SelectableShader.Use();
            }
            OnSimpleDraw();
            OnDraw();
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

        protected virtual void OnDraw()
        {
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        private void EnableTextures()
        {
            if(Shader == null)
            {
                Console.WriteLine("Шейдеры не установлены");
                return;
            }

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

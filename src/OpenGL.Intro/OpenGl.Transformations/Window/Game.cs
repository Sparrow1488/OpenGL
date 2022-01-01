using GraphicEngine.V1;
using GraphicEngine.V1.Entities;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;

namespace OpenGl.Transformations.Window
{
    internal class Game : GameWindow
    {
        private List<int> _cells = new List<int>();
        private int _quadre = -1;
        private Shader _transformShader;
        private Shader _normalizeShader;
        private Texture _texture;
        private Engine _engine;

        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            CenterWindow(new Vector2i(500, 500));
            _engine = new Engine();
            Context.SwapInterval = 2;
        }

        protected override void OnLoad()
        {
            var vertices = new float[]
            {
                // position         texture
                -0.3f, -0.3f, 0.0f, 0.0f, 0.0f,
                -0.3f, 0.3f, 0.0f,  0.0f, 1.0f,
                0.3f, 0.3f, 0.0f,   1.0f, 1.0f,
                0.3f, -0.3f, 0.0f,   1.0f, 0.0f
            };
            var indices = new uint[]
            {
                0, 1, 2,
                0, 3, 2
            };
            _transformShader = new Shader("vertex.glsl", "fragment.glsl", "Transform").Create();
            _normalizeShader = new Shader("vertex.glsl", "fragment.glsl", "Normalize").Create();
            _texture = new Texture("Linus.jpg").Create();
            _quadre = _engine.CreateTextured(vertices, indices);

            GenerateCells();

            base.OnLoad();
        }

        private float _angleZ = 10f;
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            GL.ClearColor(new Color4(230, 230, 250, 1));

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            for (int i = 0; i < _cells.Count; i++)
            {
                _normalizeShader.Use();
                GL.BindVertexArray(_cells[i]);
                GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            }

            #region Че-то с текстурами
            // _texture.Use();
            //_transformShader.Use();
            //var location = GL.GetUniformLocation(_transformShader.Id, "transform");

            //var rotate = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_angleZ -= 5f));
            //if (location == -1) throw new InvalidOperationException("Не удалось обнаружить позицию uniform в шейдере");
            //GL.UniformMatrix4(location, true, ref rotate);

            //GL.BindVertexArray(_quadre);
            //GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            #endregion

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        private void GenerateCells()
        {
            int quadreSize = 10;
            float step = 0.2f;
            
            var indices = new uint[]
            {
                0, 1, 2,
                0, 3, 2
            };

            var vertices = new float[]
            {
                0.0f, 0.0f, 0.0f,
                0.0f, step, 0.0f,
                step, step, 0.0f,
                step, 0.0f, 0.0f
            };

            for (int column = 0; column < quadreSize; column++)
            {
                for (int row = 0; row < quadreSize; row++)
                {
                    vertices[0] = step * row;
                    vertices[3] = step * row;
                    vertices[6] = step * row + step;
                    vertices[9] = step * row + step;
                    _cells.Add(_engine.Create(vertices, indices));
                }

                vertices[1] += step;
                vertices[4] += step;
                vertices[7] += step;
                vertices[10] += step;
            }


        }
    }
}

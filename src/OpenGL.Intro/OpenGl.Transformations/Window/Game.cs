using GraphicEngine.V1;
using GraphicEngine.V1.Entities;
using OpenGl.Transformations.Tools;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

namespace OpenGl.Transformations.Window
{
    internal class Game : GameWindow
    {
        private List<int> _cells = new List<int>();
        private List<GameObject> _gameObjects = new List<GameObject>();
        private Engine _engine;
        private Logger _logger;

        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            CenterWindow(new Vector2i(500, 500));
            _engine = new Engine();
            _logger = new Logger();
            Context.SwapInterval = 2;
        }

        protected override void OnLoad()
        {
            var texturedShader = new Shader("vertex1.glsl", "fragment1.glsl", "Textured").Create();
            var quadre = new Quadre().Create(0.15f)
                                     .Use(texturedShader)
                                     .SetName("Testing_game_object");
            OnSelectedInit(quadre);

            _gameObjects.Add(quadre);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            GL.ClearColor(new Color4(53, 95, 115, 1));

            foreach (var gameObject in _gameObjects)
            {
                gameObject.Draw();
            }

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            int width = 1;
            int height = 1;
            unsafe {
                GLFW.GetWindowSize(WindowPtr, out width, out height);
            }
            float mouseX = (float)(-1.0 + 2.0 * MousePosition.X / width);
            float mouseY = (float)(1.0 - 2.0 * MousePosition.Y / height);
            _logger.Log($"Mouse Down → ({mouseX}; {mouseY})");

            foreach (var obj in _gameObjects)
            {
                if (obj.IsSelected(mouseX, mouseY))
                {
                    _logger.Log(obj.Name + " selected");
                }
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        private void OnSelectedInit(GameObject obj)
        {
            obj.OnSelected += () =>
            {
                var rnd = new Random();
                var r = (float)rnd.NextDouble();
                var g = (float)rnd.NextDouble();
                var b = (float)rnd.NextDouble();
                obj.ChangeShaderColor(new Color4(r, g, b, 1f), "UniColor");
            };
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

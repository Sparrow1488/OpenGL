using GraphicEngine.V1;
using GraphicEngine.V1.Entities;
using OpenGl.Coordinates.Core.Tools;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

namespace OpenGl.Coordinates.Core
{
    internal class Game : GameWindow
    {
        private readonly List<int> _cells = new List<int>();
        private readonly List<GameObject> _gameObjects = new List<GameObject>();
        private readonly Engine _engine = new Engine();
        private readonly Logger _logger = new Logger();

        private List<Vector3> _cubePositions = new List<Vector3>();
        private Shader _coordShader;

        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            CenterWindow(new Vector2i(650));
            Context.SwapInterval = 3;
        }

        protected override void OnLoad()
        {
            _coordShader = new Shader("vertex1.glsl", "fragment1.glsl", "Coordinates").Create();
            var texture = new Texture("awesomeface.png").Create().SetShaderName("texture0");
            var textureBox = new Texture("box.jpg").Create().SetShaderName("texture1");
            var quadre = new Quadre().CreateTextured(0.35f)
                                        .Use(_coordShader)
                                        .Add(texture)
                                        .Add(textureBox)
                                        .SetName("Testing_game_quadre");
            var cube = new Cube().Create()
                                 .Use(_coordShader)
                                 //.Add(texture)
                                 //.Add(textureBox)
                                 .SetName("Testing_game_qube");
            OnSelectedInit(quadre);

            var positions = new Vector3[]
            {
                new Vector3(0.8f, -0.3f, -2.0f),
                new Vector3(0.1f, 0.7f, -4.5f),
                new Vector3(-0.5f, 0.2f, -4.0f),
                new Vector3(0.4f, -0.2f, -1.2f),
                new Vector3(0.9f, 0.3f, -8.2f)
            };
            _cubePositions.AddRange(positions);

            //_gameObjects.Add(quadre);
            _gameObjects.Add(cube);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            GL.ClearColor(new Color4(53, 95, 115, 1));
            GL.Enable(EnableCap.DepthTest);

            GL.Color3(255, 0, 0);
            for (int i = 0; i < _cubePositions.Count; i++)
            {
                double time;
                unsafe
                {
                    time = GLFW.GetTime();
                }
                var model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians((float)time) / 0.2f * 5f) *
                                Matrix4.CreateRotationY(MathHelper.DegreesToRadians((float)time / 0.2f * 5f)) *
                                    Matrix4.CreateTranslation(_cubePositions[i].X, _cubePositions[i].Y, _cubePositions[i].Z);
                var view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
                var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 500 / 500, 0.1f, 100.0f); // вид в проекции
                _coordShader.SetMatrix4("model", model)
                            .SetMatrix4("view", view)
                            .SetMatrix4("projection", projection);
                _gameObjects[0].Draw();
            }

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            int width = 1;
            int height = 1;
            unsafe
            {
                GLFW.GetWindowSize(WindowPtr, out width, out height);
            }
            float mouseX = (float)(-1.0 + 2.0 * MousePosition.X / width);
            float mouseY = (float)(1.0 - 2.0 * MousePosition.Y / height);
            //_logger.Log($"R {pixels[0]}; G {pixels[1]}; B {pixels[2]}");
            _logger.Log($"Mouse Down → ({mouseX}; {mouseY})");

            foreach (var obj in _gameObjects)
            {
                var pixels = new byte[3];
                GL.ReadPixels((int)MousePosition.X, (int)MousePosition.Y, 1, 1, PixelFormat.Rgb, PixelType.Byte, pixels);
                if ((pixels[0] - 100) == obj.Id)
                {
                    _logger.Success($"[{obj.Id}]-{obj.Name} was selected!");
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
    }
}
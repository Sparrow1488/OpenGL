using GraphicEngine.V1.Entities;
using OOpenGl.MousePicking.Tools;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Collections.Generic;

namespace OpenGl.MousePicking
{
    internal class Game : GameWindow
    {
        private readonly Logger _logger = new Logger();
        private readonly List<GameObject> _gameObjects = new List<GameObject>();
        private Vector3[] _translations;
        private Shader _shader;

        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            Context.SwapInterval = 2;
            CenterWindow(new Vector2i(650));
        }

        protected override void OnLoad()
        {
            _translations = new Vector3[]
            {
                new Vector3(-0.2f, 0.3f, -12.0f),
                new Vector3(0.4f, 0.6f, -9.0f),
                new Vector3(-0.9f, 1.0f, -3.0f)
            };

            _shader = new Shader("ver.glsl", "fra.glsl", "Coordinates").Create();
            var cube = new Cube().Create()
                                   .SetName("Selectable_cube")
                                     .Use(_shader);
            var cube1 = new Cube().Create()
                                   .SetName("Selectable_cube")
                                     .Use(_shader);
            var cube2 = new Cube().Create()
                                   .SetName("Selectable_cube")
                                     .Use(_shader);
            _gameObjects.Add(cube);
            _gameObjects.Add(cube1);
            _gameObjects.Add(cube2);
            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            GL.ClearColor(new Color4(38, 37, 88, 0));
            GL.Enable(EnableCap.DepthTest);

            for (int i = 0; i < _translations.Length; i++)
            {
                var model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f)) *
                                Matrix4.CreateRotationY(MathHelper.DegreesToRadians(20.0f)) *
                                    Matrix4.CreateTranslation(_translations[i]);
                var view = Matrix4.CreateTranslation(0f, 0f, -3.0f);
                var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 500 / 500, 0.1f, 100.0f); // вид в проекции
                _shader.SetMatrix4("model", model)
                         .SetMatrix4("view", view)
                           .SetMatrix4("projection", projection)
                             .SetVector4("UniColor", _gameObjects[i].UniColor);
                _gameObjects[i].Draw();
            }

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            OnMouseDown();
        }

        private void OnMouseDown()
        {
            int width = 1;
            int height = 1;
            unsafe
            {
                GLFW.GetWindowSize(WindowPtr, out width, out height);
            }
            float mouseX = (float)(-1.0 + 2.0 * MousePosition.X / width);
            float mouseY = (float)(1.0 - 2.0 * MousePosition.Y / height);
            _logger.Log($"Mouse Down → ({mouseX}; {mouseY})");

            int mouseXi = (int)MousePosition.X;
            int mouseYi = (int)MousePosition.Y;
            var pixels = new byte[3];
            GL.ReadPixels(mouseXi, width - mouseYi, 1, 1, PixelFormat.Rgb, PixelType.UnsignedByte, pixels);
            foreach (var obj in _gameObjects)
            {
                _logger.Log($"Color4({pixels[0]};{pixels[1]};{pixels[2]})");
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
    }
}
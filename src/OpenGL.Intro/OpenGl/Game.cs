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

            var texture = new Texture("awesomeface.png").Create();
            _shader = new Shader("ver.glsl", "fra.glsl", "Textured").Create();
            var cube = new Cube().Create()
                                   .SetName("Selectable_cube_1")
                                     .Use(_shader)
                                       .Add(texture);
            var cube1 = new Cube().Create()
                                   .SetName("Selectable_cube_2")
                                     .Use(_shader)
                                       .Add(texture);
            var cube2 = new Cube().Create()
                                   .SetName("Selectable_cube_3")
                                     .Use(_shader)
                                       .Add(texture);
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

            if (_drawSelectable) {
                DrawSelectedObjects();
            } 
            else {
                for (int i = 0; i < _translations.Length; i++)
                {
                    var model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians((float)GLFW.GetTime() / 0.2f * 6f)) *
                                    Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-(float)GLFW.GetTime() / 0.3f * 5f)) *
                                        Matrix4.CreateTranslation(_translations[i]);
                    var view = Matrix4.CreateTranslation(0f, 0f, -3.0f);
                    var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 500 / 500, 0.1f, 100.0f); // вид в проекции
                    _gameObjects[i].SetMatrixes(model, view, projection);
                    _gameObjects[i].Draw();
                }
            }
            

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        private bool _drawSelectable = false;
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            
            if(e.Button == MouseButton.Left) {
                int mouseXi = (int)MousePosition.X;
                int mouseYi = (int)MousePosition.Y;
                var selectedObject = SelectObject(mouseXi, mouseYi);
                if (selectedObject.Id != -1)
                {
                    _logger.Success($"[{selectedObject.Id}]-{selectedObject.Name} was selected!");
                }
            }
            else {
                DrawSelectedObjects();
                _drawSelectable = !_drawSelectable;
            }
        }

        private void DrawSelectedObjects()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            GL.Enable(EnableCap.DepthTest);

            for (int i = 0; i < _translations.Length; i++)
            {
                _gameObjects[i].DrawSelectable();
            }
        }

        private GameObject SelectObject(int mouseX, int mouseY)
        {
            var result = new GameObject();

            DrawSelectedObjects();
            int width = 1;
            unsafe {
                GLFW.GetWindowSize(WindowPtr, out width, out var height);
            }
            var pixels = new byte[3];
            GL.ReadPixels(mouseX, width - mouseY, 1, 1, PixelFormat.Rgb, PixelType.UnsignedByte, pixels);
            _logger.Log($"Color4({pixels[0]};{pixels[1]};{pixels[2]})");
            foreach (var obj in _gameObjects) {
                if ((pixels[0] - 100) == obj.Id) {
                    result = obj;
                }
            }
            return result;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }
    }
}
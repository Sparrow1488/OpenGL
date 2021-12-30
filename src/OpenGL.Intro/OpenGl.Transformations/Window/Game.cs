using GraphicEngine.V1;
using GraphicEngine.V1.Entities;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace OpenGl.Transformations.Window
{
    internal class Game : GameWindow
    {
        private int _quadre = -1;
        private Shader _transformShader;
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
            _texture = new Texture("Linus.jpg").Create();
            _quadre = _engine.CreateTextured(vertices, indices);

            base.OnLoad();
        }

        private float _angleZ = 10f;
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            GL.ClearColor(new Color4(250, 250, 250, 1));

            _texture.Use();
            _transformShader.Use();
            var location = GL.GetUniformLocation(_transformShader.Id, "transform");

            var rotate = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_angleZ -= 5f));
            if (location == -1) throw new InvalidOperationException("Не удалось обнаружить позицию uniform в шейдере");
            GL.UniformMatrix4(location, true, ref rotate);

            GL.BindVertexArray(_quadre);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }
    }
}

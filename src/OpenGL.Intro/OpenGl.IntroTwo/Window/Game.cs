using OpenGl.IntroTwo.Entities;
using OpenGl.IntroTwo.Tools;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenGl.IntroTwo.Window
{
    internal class Game : GameWindow
    {
        private int _triangle = -1;
        private int _quadre = -1;
        private Shader _testShader;
        private readonly GraphicEngine _engine;

        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default) {
            Context.SwapInterval = 2;
            _engine = new GraphicEngine();
        }

        protected override void OnLoad()
        {
            var triangleVertices = new float[]
            {
                -0.3f, -0.3f, 0.0f,
                0.0f, 0.3f, 0.0f,
                0.3f, -0.3f, 0.0f
            };
            var quadreVertices = new float[]
            {
                -1.0f, 0.8f, 0.0f,
                -1.0f, 1.0f, 0.0f,
                -0.8f, 1.0f, 0.0f,
                -0.8f, 0.8f, 0.0f
            };
            var quadreIndices = new uint[] // indices => указатели
            {
                0, 1, 2,
                0, 3, 2
            };
            _triangle = _engine.Create(triangleVertices);
            _quadre = _engine.Create(quadreVertices, quadreIndices);
            _testShader = new Shader("vertex1.glsl", "fragment1.glsl", "Static").Create();

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.3f, 0.2f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            _testShader.Use();
            GL.BindVertexArray(_triangle);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

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

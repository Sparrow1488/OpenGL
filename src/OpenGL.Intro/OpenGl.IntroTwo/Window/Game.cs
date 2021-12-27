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
        private int _triangleTex = -1;
        private Shader _testShader;
        private Shader _dynamicShader;
        private Shader _textureShader;
        private Texture _texture;
        private readonly GraphicEngine _engine;

        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default) {
            Context.SwapInterval = 3;
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
            var quadreTexVertices = new float[]
            {
                -0.5f, -0.5f, 0.0f,     0.0f, 0.0f,
                0.0f, 0.5f, 0.0f,        0.0f, 0.5f,
                0.5f, -0.5f, 0.0f,      0.5f, 0.0f 
            };
            var quadreIndices = new uint[] // indices => указатели
            {
                0, 1, 2,
                0, 3, 2
            };
            _triangle = _engine.Create(triangleVertices);
            _quadre = _engine.Create(quadreVertices, quadreIndices);
            _testShader = new Shader("vertex1.glsl", "fragment1.glsl", "Static").Create();
            _dynamicShader = new Shader("vertex1.glsl", "fragment1.glsl", "Dynamic").Create();
            _textureShader = new Shader("verticeTex1.glsl", "fragTex1.glsl", "Static").Create();

            _texture = new Texture("tex1.jpg").Create();
            _triangleTex = _engine.CreateTextured(quadreTexVertices);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.3f, 0.2f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            _texture.Use();
            _textureShader.Use();
            GL.BindVertexArray(_triangleTex);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            _dynamicShader.UseColorAnimation("uniColor");
            GL.BindVertexArray(_triangle);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3); // если элемент состоит из VBO&VAO

            _testShader.Use();
            GL.BindVertexArray(_quadre);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0); // если элемент имеет указатели VAO&VBO&EBO

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

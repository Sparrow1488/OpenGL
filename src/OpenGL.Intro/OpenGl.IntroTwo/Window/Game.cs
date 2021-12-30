using OpenGl.IntroTwo.Entities;
using OpenGl.IntroTwo.Tools;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;
using System.IO;

namespace OpenGl.IntroTwo.Window
{
    internal class Game : GameWindow
    {
        private int _triangle = -1;
        private int _quadre = -1;
        private int _triangleTex = -1;
        private int _quadreTex = -1;
        private int _quadreColorTex = -1;
        private int _doubleTexQuadre = -1;
        private Shader _testShader;
        private Shader _dynamicShader;
        private Shader _textureShader;
        private Shader _colorTextureShader;
        private Texture _texture;
        private Texture _boxTexture;
        private Texture _faceTexture;
        private readonly GraphicEngine _engine;
        private string[] _files;
        private int _currentFileIndex = 0;

        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default) {
            Context.SwapInterval = 3;
            _engine = new GraphicEngine();
            CenterWindow(new Vector2i(640, 500));
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

            var triangleTexVertices = new float[]
            {
                -0.5f, -0.5f, 0.0f,     0.0f, 0.0f,
                0.0f, 0.5f, 0.0f,        0.0f, 0.5f,
                0.5f, -0.5f, 0.0f,      0.5f, 0.0f 
            };

            //var quadreTexVertices = new float[]
            //{
            //    -0.7f, -0.7f, 0.0f,     0.0f, 0.0f,
            //    -0.7f, 0.7f, 0.0f,      0.0f, 1.0f,
            //    0.7f, 0.7f, 0.0f,        1.0f, 1.0f,
            //    0.7f, -0.7f, 0.0f,      1.0f, 0.0f
            //};
            var quadreTexVertices = new float[]
            {
                -1.0f, -1.0f, 0.0f,     0.0f, 0.0f,
                -1.0f, 1.0f, 0.0f,      0.0f, 1.0f,
                1.0f, 1.0f, 0.0f,        1.0f, 1.0f,
                1.0f, -1.0f, 0.0f,      1.0f, 0.0f
            };
            var quadreTexIndices = new uint[]
            {
                0, 1, 2,
                0, 3, 2
            };

            var quadreColorTexVertices = new float[]
            {
                //pos                    //color                //texture
                -0.7f, -0.7f, 0.0f,   1.0f, 0.0f, 0.0f,  0.0f, 0.0f,
                -0.7f, 0.7f, 0.0f,    0.0f, 1.0f, 0.0f,  0.0f, 1.0f,
                0.7f, 0.7f, 0.0f,      0.0f, 0.0f, 1.0f,  1.0f, 1.0f,
                0.7f, -0.7f, 0.0f,    0.0f, 1.0f, 0.0f,  1.0f, 0.0f
            };
            
            _triangle = _engine.Create(triangleVertices);
            _quadre = _engine.Create(quadreVertices, quadreIndices);
            _testShader = new Shader("vertex1.glsl", "fragment1.glsl", "Static").Create();
            _dynamicShader = new Shader("vertex1.glsl", "fragment1.glsl", "Dynamic").Create();
            _textureShader = new Shader("verticeTex1.glsl", "fragTex1.glsl", "Static").Create();
            _colorTextureShader = new Shader("vertexColTex1.glsl", "fragColTex1.glsl", "Static").Create();

            _texture = new Texture("tex1.jpg").Create();
            _boxTexture = new Texture("fantasy.gif").Create();
            _faceTexture = new Texture("awesomeface.png").Create();

            _triangleTex = _engine.CreateTextured(triangleTexVertices);
            _quadreTex = _engine.CreateTextured(quadreTexVertices, quadreTexIndices);
            _quadreColorTex = _engine.CreateColoredTextured(quadreColorTexVertices, quadreTexIndices);
            _doubleTexQuadre = _engine.CreateTextured(quadreTexVertices, quadreTexIndices);

            _files = Directory.GetFiles("./Textures/Gif");

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.5f, 0.3f, 0.1f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            

            SwapBuffers();

            //GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0); // если элемент имеет указатели VAO&VBO&EBO
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3); // если элемент состоит из VBO&VAO
            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        private void PlayGif()
        {
            // не забываем, копируем кадры гифки в конечную сборку
            if (_currentFileIndex == _files.Length)
                _currentFileIndex = 0;
            var textureName = new FileInfo(_files[_currentFileIndex]).Name;
            var texture = new Texture("Gif/" + textureName).Create();
            _textureShader.Use();
            texture.Use();
            GL.BindVertexArray(_doubleTexQuadre);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            _currentFileIndex++;
        }
    }
}

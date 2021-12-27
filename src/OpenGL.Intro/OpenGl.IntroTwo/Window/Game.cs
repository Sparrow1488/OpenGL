﻿using OpenGl.IntroTwo.Entities;
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

            var quadreTexVertices = new float[]
            {
                -0.7f, -0.7f, 0.0f,     0.0f, 0.0f,
                -0.7f, 0.7f, 0.0f,      0.0f, 1.0f,
                0.7f, 0.7f, 0.0f,        1.0f, 1.0f,
                0.7f, -0.7f, 0.0f,      1.0f, 0.0f
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
            _boxTexture = new Texture("box.jpg").Create();
            _faceTexture = new Texture("awesomeface.png").Create();

            _triangleTex = _engine.CreateTextured(triangleTexVertices);
            _quadreTex = _engine.CreateTextured(quadreTexVertices, quadreTexIndices);
            _quadreColorTex = _engine.CreateColoredTextured(quadreColorTexVertices, quadreTexIndices);
            _doubleTexQuadre = _engine.CreateTextured(quadreTexVertices, quadreTexIndices);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.3f, 0.2f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            _boxTexture.CombineTexture(_faceTexture, _textureShader).Use();
            _textureShader.Use();
            GL.BindVertexArray(_doubleTexQuadre);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            //_textureNext.Use();
            //_colorTextureShader.Use();
            //GL.BindVertexArray(_quadreColorTex);
            //GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            //_textureNext.Use();
            //_textureShader.Use();
            //GL.BindVertexArray(_quadreTex);
            //GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            //_texture.Use();
            //_textureShader.Use();
            //GL.BindVertexArray(_triangleTex);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            //_dynamicShader.UseColorAnimation("uniColor");
            //GL.BindVertexArray(_triangle);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3); // если элемент состоит из VBO&VAO

            //_testShader.Use();
            //GL.BindVertexArray(_quadre);
            //GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0); // если элемент имеет указатели VAO&VBO&EBO

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

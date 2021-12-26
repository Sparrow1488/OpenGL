using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenGl.SapperTK.DrawTools;
using System.IO;
using System;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenGl.SapperTK.Entities;

namespace OpenGl.SapperTK.Windows
{
    internal class Game : GameWindow
    {
        private List<int> _vaos = new List<int>();
        private List<Shader> _shaders = new List<Shader>();

        private Shader _transformShader;
        private Shader _textureShader;

        public Game() : 
            base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            CenterWindow(new Vector2i(720, 500));
            Context.SwapInterval = 2; // еще нормис при 2-3
            //VSync = VSyncMode.On; // считается устаревшим

            KeyUp += Game_KeyUp;
        }

        private void Game_KeyUp(KeyboardKeyEventArgs obj)
        {
            if (obj.Key == Keys.W)
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            if (obj.Key == Keys.R)
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[21.12.21] Hello Triangles!");
            Console.WriteLine("[25.12.21] Shaders!");
            Console.ResetColor();

            string vertexShaderSource;
            string fragmentShaderSource;


            var btn = UIElements.CreateButton(new[] { 
                -0.99f, 0.99f, 0f,
                -0.7f, 0.99f, 0f,
                -0.7f, 0.9f, 0f,
                -0.99f, 0.9f, 0f,
            });
            _vaos.Add(btn);

            var vertices = new float[]
            {
                -0.3f, 0f, 0f,
                -0.1f, 0f, 0f,
                -0.2f, 0.5f, 0f,

                0.3f, 0f, 0f,
                0.1f, 0f, 0f,
                0.2f, 0.5f, 0f
            };
            var indices = new uint[]
            {
                0, 1, 2,
                3, 4, 5
            };

            _vaos.Add(UIElements.CreateElement(vertices, indices));

            _vaos.Add(UIElements.CreateCenterQuadre(0.2f));

            vertexShaderSource = "./Shaders/YellowVertexShaders.glsl";
            fragmentShaderSource = "./Shaders/YellowFragmentShader.glsl";
            _shaders.Add(new Shader(vertexShaderSource, fragmentShaderSource));

            vertexShaderSource = "./Shaders/Custom/Dynamic/vertex1.glsl";
            fragmentShaderSource = "./Shaders/Custom/Dynamic/fragment1.glsl";
            _shaders.Add(new Shader(vertexShaderSource, fragmentShaderSource));

            vertexShaderSource = "./Shaders/Custom/Transform/vertex1.glsl";
            fragmentShaderSource = "./Shaders/Custom/Transform/fragment1.glsl";
            _transformShader = new Shader(vertexShaderSource, fragmentShaderSource);

            vertexShaderSource = "./Shaders/Custom/Static/vertex3.glsl";
            fragmentShaderSource = "./Shaders/Custom/Static/fragment3.glsl";
            _textureShader = new Shader(vertexShaderSource, fragmentShaderSource);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(new Color4(0.3f, 0.5f, 0.4f, 1f));
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.LineWidth(2f);

            for (int i = 0; i < _vaos.Count; i++)
            {
                if(i != 2)
                    UIElements.DrawElement(_vaos[i], _shaders[0].UID);
            }

            _transformShader.Use();
            //UIElements.UniformColorAnimate(_transformShader, "uniColor");
            //UIElements.UniformAnimatePingPong(_transformShader, "coord");
            //UIElements.DrawElement(_vaos[2], _transformShader.UID);

            var vertices = new[]
            {
                -0.5f, -0.5f, 0f,  1.0f, 0f, 0f,
                0f, 0.5f, 0f,      0f, 1.0f, 0f,
                0.5f, -0.5f, 0f,   0f, 0f, 1,0f
            };
            var verticesTex = new[]
            {
                // vertices              //texture
                -0.5f, -0.5f, 0.0f,   0.0f, 0.0f,
                -0.5f, 0.5f, 0.0f,  -1.0f, 1.0f,
                0.5f, 0.5f, 0.0f,    1.0f, 1.0f,
            };
            //UIElements.CreateRainbowElement(vertices);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            _textureShader.Use();
            var texture = UIElements.CreateTexture("tex1.jpg");
            var elem = UIElements.CreateTextureElement(verticesTex);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        private void Game_KeyDown(KeyboardKeyEventArgs obj)
        {
            if (obj.Key == Keys.W)
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            if (obj.Key == Keys.R)
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        protected override void OnUnload()
        {
            // выгружаем все ресурсы
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //GL.DeleteBuffers(0, ref _verticesBufferObject);

            for (int i = 0; i < _shaders.Count; i++)
            {
                GL.UseProgram(_shaders[i].UID);
                GL.DeleteProgram(_shaders[i].UID);
                _shaders.RemoveAt(i);
            }
            

            base.OnUnload();
        }

        
    }
}

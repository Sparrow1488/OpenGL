using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenGl.SapperTK.DrawTools;
using System.IO;
using System;

namespace OpenGl.SapperTK.Windows
{
    internal class Game : GameWindow
    {
        private List<int> _vaos = new List<int>();
        private int _verticesBufferObject;

        private int _shaderProgram;
        private int _shaderProgramSecond;

        public Game() : 
            base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            CenterWindow(new Vector2i(720, 500));
            Context.SwapInterval = 5;
            //VSync = VSyncMode.On; // считается устаревшим
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Hello Triangles!");
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

            //vertexShaderSource = File.ReadAllText("./Shaders/OragneVertexShaders.glsl");
            //fragmentShaderSource = File.ReadAllText("./Shaders/OrangeFragmentShader.glsl");
            //_shaderProgram = UIElements.CreateShaderProgram(vertexShaderSource, fragmentShaderSource);

            vertexShaderSource = File.ReadAllText("./Shaders/YellowVertexShaders.glsl");
            fragmentShaderSource = File.ReadAllText("./Shaders/YellowFragmentShader.glsl");
            _shaderProgramSecond = UIElements.CreateShaderProgram(vertexShaderSource, fragmentShaderSource);

            vertexShaderSource = File.ReadAllText("./Shaders/Custom/Static/vertex1.glsl");
            fragmentShaderSource = File.ReadAllText("./Shaders/Custom/Static/fragment1.glsl");
            _shaderProgram = UIElements.CreateShaderProgram(vertexShaderSource, fragmentShaderSource);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(new Color4(0.3f, 0.5f, 0.4f, 1f));
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.LineWidth(2f);
            //GL.UseProgram(_shaderProgramSecond);
            for (int i = 0; i < _vaos.Count; i++)
            {
                if(i == 0 || i % 2 == 0)
                    UIElements.DrawElement(_vaos[i], _shaderProgram);
                else UIElements.DrawElement(_vaos[i], _shaderProgramSecond);
            }
                
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); // включение wireframe mode (каркасный режим)

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUnload()
        {
            // выгружаем все ресурсы
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffers(0, ref _verticesBufferObject);

            GL.UseProgram(_shaderProgram);
            GL.DeleteProgram(_shaderProgram);

            base.OnUnload();
        }

        
    }
}

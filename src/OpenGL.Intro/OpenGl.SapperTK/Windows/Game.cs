using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenGl.SapperTK.DrawTools;

namespace OpenGl.SapperTK.Windows
{
    internal class Game : GameWindow
    {
        private List<int> _vaos = new List<int>();
        private int _verticesBufferObject;

        private int _shaderProgram;

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

            string vertexShaderSource = "#version 330 core\n" +
                                                            "layout (location = 0) in vec3 aPos;\n" +
                                                            "void main()\n" +
                                                            "{\n" +
                                                            "   gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n" +
                                                            "}\0";
            string fragmentShaderSource = "#version 330 core\n" +
                                                                "out vec4 FragColor;\n" +
                                                                "void main()\n" +
                                                                "{\n" +
                                                                "   FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);\n" +
                                                                "}";
            _shaderProgram = UIElements.CreateShaderProgram(vertexShaderSource, fragmentShaderSource);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(new Color4(0.3f, 0.5f, 0.4f, 1f));
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.LineWidth(2f);
            GL.UseProgram(_shaderProgram);
            foreach (var vao in _vaos)
            {
                GL.BindVertexArray(vao);
                GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
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

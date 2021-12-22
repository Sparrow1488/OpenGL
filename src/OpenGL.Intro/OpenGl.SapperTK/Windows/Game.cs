using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace OpenGl.SapperTK.Windows
{
    internal class Game : GameWindow
    {
        private int _verticesBufferObject;
        private int _vertexArrayObject;

        private int _vertexShader;
        private int _fragmentShader;

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
            #region Generate net
            //var vertecesList = new List<float>();
            //for (int column = 0; column < 5; column++)
            //    for (int row = 0; row < 5; row++)
            //    {
            //        var _cellSize = 0.1f;
            //        float baseX = row * 0.1f;
            //        float baseY = column * 0.1f;
            //        vertecesList.AddRange(new float[] { baseX, baseY, 0f });
            //        vertecesList.AddRange(new float[] { baseX, baseY + _cellSize, 0f });

            //        vertecesList.AddRange(new float[] { baseX, baseY + _cellSize, 0f });
            //        vertecesList.AddRange(new float[] { baseX + _cellSize, baseY + _cellSize, 0f });

            //        vertecesList.AddRange(new float[] { baseX + _cellSize, baseY + _cellSize, 0f });
            //        vertecesList.AddRange(new float[] { baseX + _cellSize, baseY, 0f });

            //        vertecesList.AddRange(new float[] { baseX + _cellSize, baseY, 0f });
            //        vertecesList.AddRange(new float[] { baseX, baseY, 0f });
            //    }
            #endregion

            var vertices = new float[]
            {
                -0.5f, -0.5f, 0f,
                0f, 0.5f, 0f,
                0.5f, -0.5f, 0f
            };

            #region DrawQuads
            //var vertices = new float[]
            //{
            //    -0.5f, -0.5f, 0f,
            //    -0.5f, 0.5f, 0f,
            //    0.5f, 0.5f, 0f,
            //    0.5f, -0.5f, 0f
            //};
            #endregion

            _verticesBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _verticesBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _verticesBufferObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);

            string vertexShaderSource = "#version 330 core\n" +
            "layout (location = 0) in vec3 aPos;\n" + 
            "void main()\n" +
            "{\n" +
            "   gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n" +
            "}\0";
            _vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(_vertexShader, vertexShaderSource);
            GL.CompileShader(_vertexShader);

            string fragmentShaderSource = "#version 330 core\n" +
            "out vec4 FragColor;\n" +
            "void main()\n" +
            "{\n" +
            "   FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);\n" +
            "}";
            _fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(_fragmentShader, fragmentShaderSource);
            GL.CompileShader(_fragmentShader);

            _shaderProgram = GL.CreateProgram();
            GL.AttachShader(_shaderProgram, _vertexShader);
            GL.AttachShader(_shaderProgram, _fragmentShader);
            GL.LinkProgram(_shaderProgram);

            GL.DetachShader(_shaderProgram, _vertexShader);
            GL.DetachShader(_shaderProgram, _fragmentShader);

            GL.DeleteShader(_vertexShader);
            GL.DeleteShader(_fragmentShader);

            GL.UseProgram(_shaderProgram);

            base.OnLoad();
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

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(new Color4(0.3f, 0.5f, 0.4f, 1f));
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.LineWidth(5f);
            GL.UseProgram(_shaderProgram);
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 4);

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}

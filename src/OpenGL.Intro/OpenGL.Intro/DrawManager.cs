using SharpGL.SceneGraph.Assets;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace OpenGL.Intro
{
    // в OpenGL используется Однородные координаты : https://neerc.ifmo.ru/wiki/index.php?title=Однородные_координаты

    // LookAt - перемещает координату глаза (камеры)
    public static class DrawManager
    {
        private static SharpGL.OpenGL _gl;
        private static float _angleX;
        private static float _angleY = 0;

        private static float _rotateAngleX;
        private static bool _useRotate = false;

        public static void UseOpenGL(SharpGL.OpenGL openGl)
        {
            _gl = openGl;
        }

        public static void UseRotate(float angleX)
        {
            _rotateAngleX = angleX;
            _useRotate = true;
        }

        public static void DrawWordK()
        {
            float rad = 1f;
            _gl.ClearColor(0.7f, 1f, 0.7f, 0f);
            _gl.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT);
            _gl.LoadIdentity();
            _gl.Translate(0, 0, -6f);

            _gl.Enable(SharpGL.OpenGL.GL_LINE_STIPPLE);
            _gl.LineStipple(1, 0x00ff);
            _gl.LineWidth(5);

            _gl.Begin(SharpGL.OpenGL.GL_LINES);

            _gl.Vertex(0, rad);
            _gl.Vertex(0, -rad);

            _gl.Vertex(0, 0);
            _gl.Vertex(rad, rad);

            _gl.Vertex(0, 0);
            _gl.Vertex(rad, -rad);

            _gl.End();
            _gl.Flush();

        }

        public static void DrawCircle(float radius)
        {
            _gl.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT);
            _gl.LoadIdentity();
            _gl.Translate(0, 0, -6f);

            _gl.Begin(SharpGL.OpenGL.GL_LINE_LOOP);
            _gl.Color(255, 255, 255);

            int segments = 100;
            for (int ii = 0; ii < segments + 1; ii++)
            {
                float theta = 2.0f * 3.1415926f * ii / segments;
                float x = radius * (float)Math.Cos(theta);
                float y = radius * (float)Math.Sin(theta);
                _gl.Vertex(x + 0, y + 0);
            }

            _gl.End();
            _gl.Flush();
        }

        public static void DrawBufferTriangle(float size)
        {
            // Компиляция шейдера
            string vertexShaders = "#version 330 core\n" +
                                                "layout (location = 0) in vec3 aPos;\n" + 
                                                "void main()\n" + 
                                                "{\n" + 
                                                "   gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n" + 
                                                "}\0";
            var vertexShader = _gl.CreateShader(SharpGL.OpenGL.GL_VERTEX_SHADER);
            _gl.ShaderSource(vertexShader, vertexShaders);
            _gl.CompileShader(vertexShader);

            // Фрагментный шейдер
            string fragmentShaderSource = "#version 330 core\n" +
                                                                "out vec4 FragColor;\n" +
                                                                "void main()\n" +
                                                                "{\n" +
                                                                "   FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);\n" +
                                                                "}\0";
            uint fragmentShader = _gl.CreateShader(SharpGL.OpenGL.GL_FRAGMENT_SHADER);
            _gl.ShaderSource(fragmentShader, fragmentShaderSource);
            _gl.CompileShader(fragmentShader);

            // Создание шейдерной программы (подключение шейдеров)
            var shaderProgram = _gl.CreateProgram();
            _gl.AttachShader(shaderProgram, vertexShader);
            _gl.AttachShader(shaderProgram, fragmentShader);
            _gl.LinkProgram(shaderProgram); // привязываем шейдерную прогу к нашему OpenGL
            //_openGL.UseProgram(shaderProgram); // активируем шейдерную программу со всем ее содержимым

            _gl.DeleteShader(vertexShader); // они нам больше не нужны
            _gl.DeleteShader(fragmentShader);

            var vertices = new[]{
                -0.5f, -0.5f, 0.0f,
                 0.5f, -0.5f, 0.0f,
                 0.0f,  0.5f, 0.0f
            };

            var buffers = new uint[8];
            _gl.GenBuffers(1, buffers);
            _gl.BindBuffer(SharpGL.OpenGL.GL_ARRAY_BUFFER, 1);
            _gl.BufferData(SharpGL.OpenGL.GL_ARRAY_BUFFER, vertices, SharpGL.OpenGL.GL_STATIC_DRAW);
            // Связывание атрибутов вершин
            _gl.VertexAttribPointer(0, 3, SharpGL.OpenGL.GL_FLOAT, false, 3 * sizeof(float), new IntPtr(0));
            _gl.EnableVertexAttribArray(0);

            DrawEmptyCube(2f);

            _gl.UseProgram(shaderProgram);
            _gl.BindVertexArray(1);

            _gl.End();
            _gl.Flush();
        }

        public static void DrawTetrahedron(float size)
        {
            if (_gl == null)
                throw new InvalidOperationException("Call \"UseOpenGL\" to initialize OpenGL instance");

            _gl.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT); // очистка цветового буфера и буфера глубины для трехмерных фигур
            _gl.LoadIdentity(); // сброс системы координат в изначальное положение, тоесть в координату (0;0)
            _gl.Translate(0, -1.2f, -6f);  // движение системы координат по x,y,z. Z стоит -6, потому что изначальная координата 0;0;0, то есть мы находимся внутри самой фигуры
            //gl.Translate(-1.5f, 0f, -6f);

            if(_useRotate)
                _gl.Rotate(_angleX, 0, 1, 0); // устанавливаем вектор вращения, вокруг которого мы будем вращать 3Д фигуру

            _gl.Begin(SharpGL.OpenGL.GL_TRIANGLES); // начинаем рисование треугольника

            _gl.Color(68f, 0, 48f);
            _gl.Vertex(0, size, 0);
            _gl.Vertex(size, 0, 0);
            _gl.Vertex(0, 0, size);

            _gl.Color(126f, 0, 48f);
            _gl.Vertex(-size, 0, 0);
            _gl.Vertex(0, size, 0);
            _gl.Vertex(0, 0, size);

            _gl.Color(47f, 0, 112f);
            _gl.Vertex(-size, 0, 0);
            _gl.Vertex(0, 0, -size);
            _gl.Vertex(0, size, 0);

            _gl.Color(47f, 0, 112f);
            _gl.Vertex(0, 0, -size);
            _gl.Vertex(size, 0, 0);
            _gl.Vertex(0, size, 0);

            _gl.End();
            _gl.Flush();

            if (_useRotate)
            {
                _angleX += _rotateAngleX;
            }
        }

        public static void DrawTriangle(float size)
        {
            if (_gl == null)
                throw new InvalidOperationException("Call \"UseOpenGL\" to initialize OpenGL instance");

            _gl.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT);
            _gl.LoadIdentity();
            _gl.Translate(0, -1.2f, -6f);

            _gl.Begin(SharpGL.OpenGL.GL_TRIANGLES);

            _gl.Color(68f, 0, 48f);
            _gl.Vertex(0, size);
            _gl.Vertex(size, 0);
            _gl.Vertex(-size, 0);

            _gl.End();
            _gl.Flush();
        }

        /// <param name="vertex">4 строки на 3 столбца</param>
        private static void DrawQuadre(float[] vertex)
        {
            _gl.Vertex(vertex[0], vertex[1], vertex[2]);
            _gl.Vertex(vertex[3], vertex[4], vertex[5]);
            _gl.Vertex(vertex[6], vertex[7], vertex[8]);
            _gl.Vertex(vertex[9], vertex[10], vertex[11]);
        }

        public static void DrawEmptyCube(float size)
        {
            float half = size / 2;

            _gl.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT);
            _gl.LoadIdentity();
            _gl.Translate(0, -1.2f, -6f);
            _gl.Color(255f, 255f, 255f);

            if (_useRotate)
                _gl.Rotate(_angleX, 0, 1, 0);

            _gl.Begin(SharpGL.OpenGL.GL_QUADS);

            DrawQuadre(
                new [] { half, 0, half,
                            -half, 0, half,
                            -half, size, half,
                             half, size, half }
            );

            DrawQuadre(
                new[] { half, size, half,
                          half, 0, half,
                          half, 0, -half,
                           half, size, -half }
            );

            //_gl.End();
            //_gl.Flush();

            if (_useRotate)
            {
                _angleX += _rotateAngleX;
                _angleY += _rotateAngleX;
            }
        }

        public static void DrawCube(float size)
        {
            float half = size / 2;

            _gl.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT);
            _gl.LoadIdentity();
            _gl.Translate(0, -1.2f, -6f);

            if (_useRotate)
                _gl.Rotate(_angleX, 0, 1, 0);

            _gl.Enable(SharpGL.OpenGL.GL_TEXTURE_2D);
            var texture = new Texture();
            texture.Create(_gl, @"C:\Users\Александр\Desktop\Repsitories\OpenGL\src\OpenGL.Intro\OpenGL.Intro\Textures\Floppa.jpg");
            texture.Bind(_gl);

            _gl.Begin(SharpGL.OpenGL.GL_QUADS);

            _gl.Color(255f, 255f, 255f);
            // Top
            _gl.TexCoord(half, half); _gl.Vertex(half, 0, half);
            _gl.TexCoord(0f, half); _gl.Vertex(-half, 0, half);
            _gl.TexCoord(0f, 0f); _gl.Vertex(-half, size, half);
            _gl.TexCoord(half, 0f); _gl.Vertex(half, size, half);

            // Right
            _gl.TexCoord(half, 0f); _gl.Vertex(half, size, half);
            _gl.TexCoord(0f, 0f); _gl.Vertex(half, 0, half);
            _gl.TexCoord(0f, half); _gl.Vertex(half, 0, -half);
            _gl.TexCoord(half, half); _gl.Vertex(half, size, -half);

            // Behind
            _gl.TexCoord(half, 0f); _gl.Vertex(half, size, -half);
            _gl.TexCoord(half, half); _gl.Vertex(half, 0, -half);
            _gl.TexCoord(0f, half); _gl.Vertex(-half, 0, -half);
            _gl.TexCoord(0f, 0f); _gl.Vertex(-half, size, -half);

            //Left
            _gl.TexCoord(half, 0f); _gl.Vertex(-half, size, -half);
            _gl.TexCoord(0f, half); _gl.Vertex(-half, 0, -half);
            _gl.TexCoord(0f, half); _gl.Vertex(-half, 0, half);
            _gl.TexCoord(0f, 0f); _gl.Vertex(-half, size, half);

            // Head
            _gl.Vertex(-half, size, half);
            _gl.Vertex(-half, size, -half);
            _gl.Vertex(half, size, -half);
            _gl.Vertex(half, size, half);

            // Bottom
            _gl.Vertex(half, 0, half);
            _gl.Vertex(-half, 0, half);
            _gl.Vertex(-half, 0, -half);
            _gl.Vertex(half, 0, -half);


            _gl.End();
            _gl.Flush();

            if (_useRotate)
            {
                _angleX += _rotateAngleX;
                _angleY += _rotateAngleX;
            }
        }
    }
}

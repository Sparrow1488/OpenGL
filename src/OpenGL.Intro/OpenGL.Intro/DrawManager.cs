using SharpGL.SceneGraph.Assets;
using System;

namespace OpenGL.Intro
{
    // в OpenGL используется Однородные координаты : https://neerc.ifmo.ru/wiki/index.php?title=Однородные_координаты
    public static class DrawManager
    {
        private static SharpGL.OpenGL _openGL;
        private static float _angleX;
        private static float _angleY = 0;

        private static float _rotateAngleX;
        private static bool _useRotate = false;

        public static void UseOpenGL(SharpGL.OpenGL openGl)
        {
            _openGL = openGl;
        }

        public static void UseRotate(float angleX)
        {
            _rotateAngleX = angleX;
            _useRotate = true;
        }

        public static void DrawTetrahedron(float size)
        {
            if (_openGL == null)
                throw new InvalidOperationException("Call \"UseOpenGL\" to initialize OpenGL instance");

            _openGL.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT); // очистка цветового буфера и буфера глубины для трехмерных фигур
            _openGL.LoadIdentity(); // сброс системы координат в изначальное положение, тоесть в координату (0;0)
            _openGL.Translate(0, -1.2f, -6f);  // движение системы координат по x,y,z. Z стоит -6, потому что изначальная координата 0;0;0, то есть мы находимся внутри самой фигуры
            //gl.Translate(-1.5f, 0f, -6f);

            if(_useRotate)
                _openGL.Rotate(_angleX, 0, 1, 0); // устанавливаем вектор вращения, вокруг которого мы будем вращать 3Д фигуру

            _openGL.Begin(SharpGL.OpenGL.GL_TRIANGLES); // начинаем рисование треугольника

            _openGL.Color(68f, 0, 48f);
            _openGL.Vertex(0, size, 0);
            _openGL.Vertex(size, 0, 0);
            _openGL.Vertex(0, 0, size);

            _openGL.Color(126f, 0, 48f);
            _openGL.Vertex(-size, 0, 0);
            _openGL.Vertex(0, size, 0);
            _openGL.Vertex(0, 0, size);

            _openGL.Color(47f, 0, 112f);
            _openGL.Vertex(-size, 0, 0);
            _openGL.Vertex(0, 0, -size);
            _openGL.Vertex(0, size, 0);

            _openGL.Color(47f, 0, 112f);
            _openGL.Vertex(0, 0, -size);
            _openGL.Vertex(size, 0, 0);
            _openGL.Vertex(0, size, 0);

            _openGL.End();
            _openGL.Flush();

            if (_useRotate)
            {
                _angleX += _rotateAngleX;
            }
        }

        public static void DrawTriangle(float size)
        {
            if (_openGL == null)
                throw new InvalidOperationException("Call \"UseOpenGL\" to initialize OpenGL instance");

            _openGL.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT);
            _openGL.LoadIdentity();
            _openGL.Translate(0, -1.2f, -6f);

            _openGL.Begin(SharpGL.OpenGL.GL_TRIANGLES);

            _openGL.Color(68f, 0, 48f);
            _openGL.Vertex(0, size);
            _openGL.Vertex(size, 0);
            _openGL.Vertex(-size, 0);

            _openGL.End();
            _openGL.Flush();
        }

        public static void DrawCube(float size)
        {
            float half = size / 2;
            float doubleSize = size * 2;

            _openGL.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT);
            _openGL.LoadIdentity();
            _openGL.Translate(0, -1.2f, -6f);

            if (_useRotate)
                _openGL.Rotate(_angleX, 0, 1, 0);

            _openGL.Enable(SharpGL.OpenGL.GL_TEXTURE_2D);
            var texture = new Texture();
            texture.Create(_openGL, @"C:\Users\aleks\OneDrive\Desktop\Илья\Repositories\OpenGL\src\OpenGL.Intro\OpenGL.Intro\Textures\Floppa.jpg");
            texture.Bind(_openGL);

            _openGL.Begin(SharpGL.OpenGL.GL_QUADS);

            _openGL.Color(255f, 255f, 255f);
            // Top
            _openGL.TexCoord(half, half); _openGL.Vertex(half, 0, half);
            _openGL.TexCoord(0f, half); _openGL.Vertex(-half, 0, half);
            _openGL.TexCoord(0f, 0f); _openGL.Vertex(-half, size, half);
            _openGL.TexCoord(half, 0f); _openGL.Vertex(half, size, half);

            // Right
            _openGL.TexCoord(half, 0f); _openGL.Vertex(half, size, half);
            _openGL.TexCoord(0f, 0f); _openGL.Vertex(half, 0, half);
            _openGL.TexCoord(0f, half); _openGL.Vertex(half, 0, -half);
            _openGL.TexCoord(half, half); _openGL.Vertex(half, size, -half);

            // Behind
            _openGL.TexCoord(half, 0f); _openGL.Vertex(half, size, -half);
            _openGL.TexCoord(half, half); _openGL.Vertex(half, 0, -half);
            _openGL.TexCoord(0f, half); _openGL.Vertex(-half, 0, -half);
            _openGL.TexCoord(0f, 0f); _openGL.Vertex(-half, size, -half);

            //Left
            _openGL.TexCoord(half, 0f); _openGL.Vertex(-half, size, -half);
            _openGL.TexCoord(0f, half); _openGL.Vertex(-half, 0, -half);
            _openGL.TexCoord(0f, half); _openGL.Vertex(-half, 0, half);
            _openGL.TexCoord(0f, 0f); _openGL.Vertex(-half, size, half);

            // Head
            _openGL.Vertex(-half, size, half);
            _openGL.Vertex(-half, size, -half);
            _openGL.Vertex(half, size, -half);
            _openGL.Vertex(half, size, half);

            // Bottom
            _openGL.Vertex(half, 0, half);
            _openGL.Vertex(-half, 0, half);
            _openGL.Vertex(-half, 0, -half);
            _openGL.Vertex(half, 0, -half);


            _openGL.End();
            _openGL.Flush();

            if (_useRotate)
            {
                _angleX += _rotateAngleX;
                _angleY += _rotateAngleX;
            }
        }
    }
}

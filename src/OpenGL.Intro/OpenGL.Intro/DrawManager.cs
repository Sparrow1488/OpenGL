using System;

namespace OpenGL.Intro
{
    // в OpenGL используется Однородные координаты : https://neerc.ifmo.ru/wiki/index.php?title=Однородные_координаты
    public static class DrawManager
    {
        private static SharpGL.OpenGL _openGL;
        private static float _angleX;
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
    }
}

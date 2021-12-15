using SharpGL;
using System.Windows;

namespace OpenGL.Intro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // в OpenGL используется Однородные координаты : https://neerc.ifmo.ru/wiki/index.php?title=Однородные_координаты
        private float angleX = 0;
        private void gl_OpenGLDraw(object sender, SharpGL.WPF.OpenGLRoutedEventArgs args)
        {
            var gl = this.gl.OpenGL;
            gl.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT); // очистка цветового буфера и буфера глубины для трехмерных фигур
            gl.LoadIdentity(); // сброс системы координат в изначальное положение, тоесть в координату (0;0)
            gl.Translate(0, -1.2f, -6f);  // движение системы координат по x,y,z. Z стоит -6, потому что изначальная координата 0;0;0, то есть мы находимся внутри самой фигуры
            //gl.Translate(-1.5f, 0f, -6f);

            gl.Rotate(angleX, 0, 1, 0); // устанавливаем вектор вращения, вокруг которого мы будем вращать 3Д фигуру

            gl.Begin(SharpGL.OpenGL.GL_TRIANGLES); // начинаем рисование треугольника

            gl.Color(68f, 0, 48f);
            gl.Vertex(0, 2f, 0);
            gl.Vertex(2f, 0, 0);
            gl.Vertex(0, 0, 2f);

            gl.Color(126f, 0, 48f);
            gl.Vertex(-2f, 0, 0);
            gl.Vertex(0, 2f, 0);
            gl.Vertex(0, 0, 2f);

            gl.Color(47f, 0, 112f);
            gl.Vertex(-2f, 0, 0);
            gl.Vertex(0, 0, -2f);
            gl.Vertex(0, 2f, 0);

            gl.Color(47f, 0, 112f);
            gl.Vertex(0, 0, -2f);
            gl.Vertex(2f, 0, 0);
            gl.Vertex(0, 2f, 0);

            gl.End();
            gl.Flush();

            angleX += 1.5f;
        }
    }
}

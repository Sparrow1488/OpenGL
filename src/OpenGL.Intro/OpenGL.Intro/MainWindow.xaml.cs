using SharpGL.WPF;
using System.Windows;

namespace OpenGL.Intro
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void gl_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
        {
            var gl = this.gl.OpenGL;

            DrawManager.UseOpenGL(gl);
            DrawManager.UseRotate(5f);
            //DrawManager.DrawEmptyCube(2f);

            //DrawManager.DrawBufferTriangle(2f);

            DrawManager.DrawCircle(1f);
        }
    }
}

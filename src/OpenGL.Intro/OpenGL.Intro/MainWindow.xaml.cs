using SharpGL;
using SharpGL.WPF;
using System.Threading;
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
            var sen = (OpenGLControl)sender;

            DrawManager.UseOpenGL(gl);
            DrawManager.UseRotate(5f);
            DrawManager.DrawCube(2f);
        }
    }
}

using SharpGL;
using System.Windows;

namespace OpenGL.Intro
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void gl_OpenGLDraw(object sender, SharpGL.WPF.OpenGLRoutedEventArgs args)
        {
            var gl = this.gl.OpenGL;

            DrawManager.UseOpenGL(gl);
            DrawManager.UseRotate(1.5f);
            DrawManager.DrawTriangle(2f);
        }
    }
}

using System.Windows;

namespace OpenGl.Sapper
{
    public partial class MainWindow : Window
    {
        private SapperGame _game;
        public MainWindow()
        {
            InitializeComponent();

            Width = 500;
            Height = 500;

            _game = new SapperGame((float)Height, (float)Width);
        }

        private void GlControl_OpenGLInitialized(object sender, SharpGL.WPF.OpenGLRoutedEventArgs args)
        {
            var gl = GlControl.OpenGL;
            _game.CreateGame(gl, 1f, 2);
        }

        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.WPF.OpenGLRoutedEventArgs args)
        {
            DrawManager.DrowQuadLeftBottom(GlControl.OpenGL, 1f);
            //_game.DrawGameField();
        }

        
    }
}

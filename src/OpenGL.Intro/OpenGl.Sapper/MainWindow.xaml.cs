using System.Windows;

namespace OpenGl.Sapper
{
    public partial class MainWindow : Window
    {
        private SapperGame _game;
        public MainWindow()
        {
            InitializeComponent();

            _game = new SapperGame();
        }

        private void GlControl_OpenGLInitialized(object sender, SharpGL.WPF.OpenGLRoutedEventArgs args)
        {
            var gl = GlControl.OpenGL;
            _game.CreateGame(gl, 1f, 10);
        }

        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.WPF.OpenGLRoutedEventArgs args)
        {
            _game.DrawGameField();
        }

        
    }
}

using System.Windows;

namespace OpenGl.Sapper
{
    public partial class MainWindow : Window
    {
        private SapperGame _game;
        public MainWindow()
        {
            InitializeComponent();

            _game = new SapperGame(Height, Width);
        }

        private void GlControl_OpenGLInitialized(object sender, SharpGL.WPF.OpenGLRoutedEventArgs args)
        {
            _game.Initialize(GlControl.OpenGL);
            _game.CreateGame();
        }

        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.WPF.OpenGLRoutedEventArgs args)
        {
            _game.DrawCell();
        }

        
    }
}

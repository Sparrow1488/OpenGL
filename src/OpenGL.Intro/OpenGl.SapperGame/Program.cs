using OpenGl.SapperGame.Window;

namespace OpenGl.SapperGame
{
    internal class Program
    {
        public static void Main()
        {
            using var game = new Window.SapperGame();
            game.Run();
        }
    }
}

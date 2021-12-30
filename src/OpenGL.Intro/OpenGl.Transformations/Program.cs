using OpenGl.Transformations.Window;
using OpenTK.Mathematics;
using System;

namespace OpenGl.Transformations
{
    internal class Program
    {
        public static void Main()
        {
            //TestTransformations();
            Console.WriteLine("Стуртуем");
            using var game = new Game();
            game.Run();

            Console.WriteLine("The End");
        }

        private static void TestTransformations()
        {
            
        }
    }
}

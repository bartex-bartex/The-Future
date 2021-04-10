using System;

namespace The_Future
{
    public static class Program
    {
        public static GameMain Game;

        [STAThread]
        static void Main()
        {
            using (var game = new GameMain())
            {
                Game = game;
                game.Run();
            }
        }
    }
}

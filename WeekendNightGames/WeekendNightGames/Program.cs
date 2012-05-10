using System;

namespace WeekendNightGames
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (PongGame game = new PongGame())
            {
                game.Run();
            }
        }
    }
#endif
}


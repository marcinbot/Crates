using System;

namespace editor2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Editor1 game = new Editor1())
            {
                game.Run();
            }
        }
    }
}


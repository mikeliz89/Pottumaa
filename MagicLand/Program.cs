using System;

namespace MagicLand
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MagicGame game = new MagicGame())
            {
                game.Run();
            }
        }
    }
#endif
}


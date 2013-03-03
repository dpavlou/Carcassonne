using System;
using MultiplayerGame.Networking;

namespace Carcassonne
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1(new ServerNetworkManager(),"","","Kokos"))
            {
                game.Run();
            }
        }
    }
}


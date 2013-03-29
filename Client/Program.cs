// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Client
{
    using System.Threading;

    using System;
    using Carcassonne;
    using MultiplayerGame;
    using MultiplayerGame.Networking;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        #region Methods

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Server Name: ");
            string serverName = Console.ReadLine();
            Console.WriteLine("IP: ");
            string IP = Console.ReadLine();
            Console.WriteLine("SteamID: ");
            string playerName = Console.ReadLine();

            Thread.Sleep(1000);

            using (var game = new Game1(new ClientNetworkManager(),serverName,IP,playerName))
            {
                game.Run();
            }
        }

        #endregion
    }
}
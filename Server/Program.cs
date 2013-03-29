// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Server
{
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

            Console.WriteLine("SteamID: ");
            string playerName = Console.ReadLine();

            using (var game = new Game1(new ServerNetworkManager(), serverName, "", playerName))
            {
               game.Run();
            }
        }

        #endregion
    }
}
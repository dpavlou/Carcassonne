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

            Console.WriteLine("SteamID: ");
            string playerName = Console.ReadLine();

            using (var game = new Game1(new ServerNetworkManager(), "Carcassonne_Server", "", playerName))
            {
               game.Run();
            }
        }

        #endregion
    }
}
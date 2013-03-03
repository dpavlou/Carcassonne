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
            using (var game = new Game1(new ServerNetworkManager()))
            {
                game.Run();
            }
        }

        #endregion
    }
}
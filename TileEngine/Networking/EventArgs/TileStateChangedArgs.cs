namespace MultiplayerGame.Args
{
    using System;
    using TileEngine.Entity;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TileStateChangedArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TileStateChangedArgs"/> class.
        /// </summary>

        public TileStateChangedArgs(Tile tile)
        {
            this.tile = tile;
        }

        #endregion

        #region Public Properties


        public Tile tile { get; private set; }

        #endregion
    }
}
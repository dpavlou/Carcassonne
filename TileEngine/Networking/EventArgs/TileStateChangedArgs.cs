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

        public TileStateChangedArgs(Tile tile,string playerID,float scale)
        {
            this.tile = tile;
            this.scale = scale;
            this.playerID = playerID;
        }

        #endregion

        #region Public Properties


        public Tile tile { get; private set; }

        public float scale { get; private set; }

        public string playerID { get; private set; }

        #endregion
    }
}
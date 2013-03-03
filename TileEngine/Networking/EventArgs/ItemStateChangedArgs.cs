namespace MultiplayerGame.Args
{
    using System;
    using TileEngine.Entity;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ItemStateChangedArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TileStateChangedArgs"/> class.
        /// </summary>

        public ItemStateChangedArgs(Item item)
        {
            this.item = item;
        }

        #endregion

        #region Public Properties

        public Item item { get; private set; }

        #endregion
    }
}
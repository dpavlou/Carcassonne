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
        /// Initializes a new instance of the <see cref="ItemStateChangedArgs"/> class.
        /// </summary>

        public ItemStateChangedArgs(Item item,string playerID,float scale,int colorID)
        {
            this.item = item;
            this.scale = scale;
            this.playerID = playerID;
            this.colorID = colorID;
        }
  
        #endregion

        #region Public Properties

        public Item item { get; private set; }

        public float scale { get; private set; }

        public string playerID { get; private set; }

        public int colorID { get; private set; }

        #endregion
    }
}
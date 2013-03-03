namespace MultiplayerGame.Args
{
    using System;
    using TileEngine.Entity;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IdentificationArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TileStateChangedArgs"/> class.
        /// </summary>

        public IdentificationArgs(string codeValue,int id,int count)
        {
            this.codeValue = codeValue;
            this.ID = id;
            this.Count = count;
        }

        #endregion

        #region Public Properties


        public string codeValue { get; private set; }

        public int ID { get; private set; }

        public int Count { get; private set; }

        #endregion
    }
}
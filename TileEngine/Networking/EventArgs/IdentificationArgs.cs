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
        /// Initializes a new instance of the <see cref="IdentificationdArgs"/> class.
        /// </summary>

        public IdentificationArgs(string codeValue,int id,int count,int colorID)
        {
            this.codeValue = codeValue;
            this.ID = id;
            this.Count = count;
            this.ColorID = colorID;
        }

        #endregion

        #region Public Properties


        public string codeValue { get; private set; }

        public int ID { get; private set; }

        public int Count { get; private set; }

        public int ColorID { get; set; }

        #endregion
    }
}
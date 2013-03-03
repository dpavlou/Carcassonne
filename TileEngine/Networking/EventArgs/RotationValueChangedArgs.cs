namespace MultiplayerGame.Args
{
    using System;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RotationValueChangedArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RotationValueChangedArgs"/> class.
        /// </summary>

        public RotationValueChangedArgs(float rotationValue, int Id,string playerID, string type)
        {
            this.rotationValue = rotationValue;
            this.playerID = playerID;
            this.type = type;
            this.ID = Id;
        }

        #endregion

        #region Public Properties

        public float rotationValue { get; private set; }

        public string playerID { get; private set; }

        public string type { get; private set; }

        public int ID { get; private set; }
        #endregion
    }
}

namespace MultiplayerGame.Networking.Messages
{
    using Lidgren.Network;
    using Lidgren.Network.Xna;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using TileEngine.Entity;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RotationMessage : IGameMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnemySpawnedMessage"/> class.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public RotationMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }


        public RotationMessage(float rotationValue,int Id, string playerID, string type)
        {
            this.RotationValue = rotationValue;
            this.PlayerID = playerID;
            this.Type = type;
            this.ID = Id;
        }

        #endregion

        #region Public Properties

        public int ID { get; set; }
        /// <summary>
        /// Gets or sets CodeValue.
        /// </summary>
        public float RotationValue { get; set; }

        /// <summary>
        /// Gets or sets MessageTime.
        /// </summary>
        public string PlayerID { get; set; }

        /// <summary>
        /// Gets MessageType.
        /// </summary>

        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.RotationValueState; //TODO: update message type
            }
        }

        /// <summary>
        /// Gets or sets type
        /// </summary>
        public string Type { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The decode.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public void Decode(NetIncomingMessage im)
        {
            this.RotationValue = im.ReadFloat();
            this.ID = im.ReadInt32();
            this.PlayerID = im.ReadString();
            this.Type = im.ReadString();
        }

        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="om">
        /// The om.
        /// </param>
        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.RotationValue);
            om.Write(this.ID);
            om.Write(this.PlayerID);
            om.Write(this.Type);
        }

        #endregion
    }
}

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
    public class UpdateTileMessage : IGameMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnemySpawnedMessage"/> class.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public UpdateTileMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }


        public UpdateTileMessage(Tile tile)
        {
            this.ID = tile.ID;
            this.CodeValue =tile.CodeValue;
            this.Location = tile.Location;
            this.Rotation = tile.RotationValue; //not necessary 
            this.MessageTime = NetTime.Now;
            //TODO: add layer message
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets CodeValue.
        /// </summary>
        public string CodeValue { get; set; }

        /// <summary>
        /// Gets or sets MessageTime.
        /// </summary>
        public double MessageTime { get; set; }

        /// <summary>
        /// Gets MessageType.
        /// </summary>
        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.UpdateTileState; //TODO: update message type
            }
        }

        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets Location
        /// </summary>
        public Vector2 Location { get; set; }

        /// <summary>
        /// Gets or sets Rotation.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets Texture;
        /// </summary>
        public int Texture { get; set; }

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
            this.CodeValue = im.ReadString();
            this.MessageTime = im.ReadDouble();
            this.Location = im.ReadVector2();
            this.Rotation = im.ReadSingle();
            this.ID = im.ReadInt32();
            //an int representing the Texture
        }

        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="om">
        /// The om.
        /// </param>
        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.CodeValue);
            om.Write(this.MessageTime);
            om.Write(this.Location);
            om.Write(this.Rotation);
            om.Write(this.ID);

        }

        #endregion
    }
}
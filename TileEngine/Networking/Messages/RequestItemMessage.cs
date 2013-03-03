
namespace MultiplayerGame.Networking.Messages
{
    using Lidgren.Network;
    using Lidgren.Network.Xna;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RequestItemMessage : IGameMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnemySpawnedMessage"/> class.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public RequestItemMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }


        public RequestItemMessage(string codeValue, int id,int Count)
        {
            this.ID = id;
            this.CodeValue = codeValue;
            this.MessageTime = NetTime.Now;
            this.Count = Count;
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
        /// 
        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.RequestItemState;
            }
        }

        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public int ID { get; set; }

        public int Count { get; set; }

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
            this.ID = im.ReadInt32();
            this.Count = im.ReadInt32();
            //this.Texture = im.ReadInt32();
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
            om.Write(this.ID);
            om.Write(this.Count);
        }

        #endregion
    }
}
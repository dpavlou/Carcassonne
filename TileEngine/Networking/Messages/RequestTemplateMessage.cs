
namespace MultiplayerGame.Networking.Messages
{
    using Lidgren.Network;
    using Lidgren.Network.Xna;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RequestTemplateMessage : IGameMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTemplateMessage"/> class.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public RequestTemplateMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }


        public RequestTemplateMessage(string name,int pos, string sender)
        {
            this.Name = name;
            this.Pos = pos;
            this.Sender = sender;


        }

        #endregion

        #region Public Properties



        public string Name { get; private set; }

        public int Pos { get; private set; }

        public string Sender { get; private set; }

        /// <summary>
        /// Gets MessageType.
        /// </summary>
        /// 
        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.RequestTemplateState;
            }
        }


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
            this.Name = im.ReadString();
            this.Pos = im.ReadInt32();
            this.Sender = im.ReadString();
        }

        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="om">
        /// The om.
        /// </param>
        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.Name);
            om.Write(this.Pos);
            om.Write(this.Sender);
        }

        #endregion
    }
}
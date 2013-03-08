namespace MultiplayerGame.Args
{
    using System;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TemplateArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateArgs"/> class.
        /// </summary>

        public TemplateArgs(string name,int pos, string sender)
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


        #endregion
    }
}
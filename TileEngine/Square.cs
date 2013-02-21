using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine

{

    public class Square
    {
        #region Declarations

        private string codeValue = "";
        protected Vector2 location;
        public Texture2D texture;

        #endregion

        #region Constructor

        public Square(string CodeValue,Texture2D texture,Vector2 location)
        {
            this.codeValue = CodeValue;
            this.location = location;
            this.texture = texture;

        }

        #endregion

        #region Properties

        public bool Occupied
        {
            get { return codeValue == ""; }
        }

        public virtual Vector2 Location
        {
            get { return location; }
            set {
                location.X = MathHelper.Clamp(value.X,-TileGrid.TileWidth/2,TileGrid.TileWidth * TileGrid.MapWidth);
                location.Y = MathHelper.Clamp(value.Y, 0, TileGrid.TileHeight * TileGrid.MapHeight);
            }
        }

        public Vector2 WorldLocation
        {
            get { return Location + Camera.worldLocation; }
        }


        public String CodeValue
        {
            get { return codeValue; }
            set { codeValue = value; }
        }

        public virtual Rectangle TileRectangle
        {
            get
            {
                return new Rectangle((int)Location.X - TileGrid.TileWidth / 2, (int)Location.Y - TileGrid.TileHeight / 2,
                                TileGrid.TileWidth,TileGrid.TileHeight); }

        }



        public Rectangle TileWorldRectangle
        {
            get
            {
                return new Rectangle((int)WorldLocation.X - TileGrid.TileWidth / 2, (int)WorldLocation.Y - TileGrid.TileHeight / 2,
                              TileGrid.TileWidth, TileGrid.TileHeight);
            }

        }

        #endregion

        #region Helper Methods

        public bool checkValue(string value)
        {
            return (value == CodeValue);
        }

        #endregion

        #region Draw

        public virtual void Draw(SpriteBatch spriteBatch)
        {
                       
        }

        #endregion

    }
}

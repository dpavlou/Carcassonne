using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    public class Item : Entity
    {
        #region Declarations

        private bool idle;
        private Texture2D onGround;
        private bool lying;
        public float Width, Height;

        #endregion

        #region Constructor

        public Item(string CodeValue, Vector2 labelOffset, Texture2D texture, SpriteFont font, Vector2 location, int ID, float layer, Texture2D onground,float bounds)
            : base(CodeValue, labelOffset, texture, font, location, ID, layer)
        {
            Layer = layer;
            Width = Height = bounds;
            Lock = false;
            onGround = onground;
            Lying = false;
        }

        #endregion

        #region Properties

        public bool Lying
        {
            get { return lying; }
            set { lying = value; }
        }

        public float Transparency
        {
            get
            {
                if (MouseOver)
                    return 1.0f;
                else
                    return 0.4f;
            }
        }

        public override Color SquareColor
        {
            get
            {
                if (ActiveTile && !Lock)
                    return Color.Red;
                else
                    return Color.Blue;
            }
        }

        public float Layer
        {
            get
            {
                if (ActiveTile)
                    return layer - 0.1f;
                return layer;
            }
            set { layer = value; }
        }

        public Texture2D Texture
        {
            get
            {
                if (Lying)
                    return onGround;
                else
                    return texture;
            }
        }

        public bool Idle
        {
            get { return idle; }
            set { idle = value; }
        }

        public override Rectangle TileRectangle
        {
            get
            {
                return new Rectangle((int)Location.X - (int)Width / 2, (int)Location.Y - (int)Height / 2,
                             (int)Width, (int)Height);
            }

        }

        
        #endregion

        #region Helper Methods

        public override void senseClick()
        {
            if (OnMouseClick() && !Lock && !Moving)
            {
                Moving = true;
                Start = MouseLocation;
            }
        }  



        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Camera.ObjectIsVisible(TileRectangle))
            {
                spriteBatch.Draw(
                         Texture,
                         Camera.WorldToScreen(Location),
                         null,
                         Color.White,
                         0.0f,
                         TileGrid.TileSourceCenter(0),
                         Camera.Scale,
                         SpriteEffects.None,
                         Layer);

                if (!Lock)
                {
                    spriteBatch.DrawString(
                      font,
                      CodeValue,
                      Camera.WorldToScreen(LabelOffset),
                      Color.Black * Transparency);

             
                }
            }
        }

        #endregion


    }
}

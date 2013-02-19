using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    public class Button : Entity
    {

        #region Declarations 

        private MouseState prevMouseState;
        private string owner;

       #endregion

        #region Constructor

        public Button(string CodeValue,Vector2 labelOffset, Texture2D texture,SpriteFont font, Vector2 location,int ID,float layer,string owner)
            : base(CodeValue, labelOffset,texture, font, location, ID, layer)
        {

            Lock=true;
            layer = 0.3f;
            prevMouseState = Mouse.GetState();
            Owner = owner;
        }

        #endregion

        #region Properties

        public override Vector2 MouseLocation
        {

            get { return new Vector2(mouseState.X, mouseState.Y); }

        }

        public override Rectangle TileRectangle
        {
            get
            {
                return new Rectangle((int)Location.X - TileGrid.TileWidth / 2, (int)Location.Y - TileGrid.TileHeight / 2,
                              TileGrid.OriginalTileWidth, TileGrid.OriginalTileHeight);
            }

        }

        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public float Transparency
        {
            get
            {
                if (!Lock)
                    return 1.0f;
                else if (MouseOver)
                    return 0.8f;
                else
                    return 0.4f;
            }
        }

        #endregion

        #region Mouse Events

        public override bool MouseClick
        {

            get
            {
                return (mouseState.LeftButton == ButtonState.Pressed
                          && MouseOver);
            }

        }

        public override bool OnMouseClick()
        {
            if((MouseClick && (prevMouseState.LeftButton != ButtonState.Pressed) ) && Lock) 
            {
                prevMouseState=mouseState;
                return true;
            }

             prevMouseState=mouseState;
             return false;
        }

        public override void dragWithMouse()
        {
            if (Moving && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (Camera.ObjectOnScreenBounds(MouseRectangle))
                {
                    Location += MouseLocation - Start;
                    Location = Camera.AdjustInScreenBounds(Location);
                    Start = MouseLocation;
                }
                else
                {
                    Location = Camera.AdjustInScreenBounds(MouseLocation);
                    Start = Location;
                }
            }
            else
                Moving = false;

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
   
                spriteBatch.Draw(
                         texture,
                         location,
                         null,
                         SquareColor* Transparency,
                         0.0f,
                         TileGrid.TileSourceCenter(0),
                         1.0f,
                         SpriteEffects.None,
                         layer);
            if(Lock)
                    spriteBatch.DrawString(
                      font,
                      CodeValue,
                      LabelOffset,
                      Color.Red * Transparency);
            
        }

        #endregion

    }
}

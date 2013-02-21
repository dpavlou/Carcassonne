using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    public class Item : RotatingTile
    {
        #region Declarations

        private bool idle;
        private Texture2D onGround;
        private bool lying;

        #endregion

        #region Constructor

        public Item(string CodeValue, Vector2 labelOffset, Texture2D texture, SpriteFont font, Vector2 location, int ID, float layer, Texture2D onground,float bounds)
            : base(CodeValue, labelOffset, texture, font, location, ID, layer)
        {
            Layer = layer;
            Lock = false;
            onGround = onground;
            Lying = false;
            Width = bounds;
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
                    return Color.White;
            }
        }

        public float Layer
        {
            get
            {
                if (ActiveTile)
                    return layer - 0.05f;
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
                return new Rectangle((int)Location.X - ((int)Width/2) , (int)Location.Y - ((int)Width/2) ,
                             (int)Width, (int)Width);
            }

        }
        public Vector2 ItemSourceCenter
        {
            get { return new Vector2(Texture.Width / 2, Texture.Height / 2); }   
        }
        
        #endregion

        #region Helper Methods

        public void ToggleLying()
        {
            Lying = !Lying;
        }
        public override void senseClick()
        {
            if (OnMouseClick() && !Lock && !Moving)
            {
                Moving = true;
                Start = MouseLocation;
            }
        }

        public override void HandleRotation()
        {
            mouseState = Mouse.GetState();
            currKeyState = Keyboard.GetState();

            if (((currKeyState.IsKeyDown(Keys.A) && !prevKeyState.IsKeyDown(Keys.A))
                || (mouseState.XButton1 == ButtonState.Pressed && prevMouseState.XButton1 != ButtonState.Pressed))
                && !Active)
            {
                RotateTile(Lying);
                ToggleLying();
            }
            else if (((currKeyState.IsKeyDown(Keys.S) && !prevKeyState.IsKeyDown(Keys.S))
                || (mouseState.XButton2 == ButtonState.Pressed && prevMouseState.XButton2 != ButtonState.Pressed))
                  && !Active)
            {
                RotateTile(Lying);
                ToggleLying();
            }
            prevMouseState = mouseState;
            prevKeyState = currKeyState;
        }

        public void RotateThis()
        {
            if (!Lock)
                HandleRotation();
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
                         SquareColor,
                         RotationAmount,
                         ItemSourceCenter,
                         Camera.Scale,
                         SpriteEffects.None,
                         Layer);

                if (!Lock)
                {
                    spriteBatch.DrawString(
                      font,
                      CodeValue,
                      Camera.WorldToScreen(LabelOffset),
                      Color.DarkRed * Transparency);

             
                }
            }
        }

        #endregion


    }
}

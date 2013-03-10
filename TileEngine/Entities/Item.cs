using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileEngine.Entity
{
    using TileEngine.Camera;
    using TileEngine.Form;

    public class Item : RotatingTile
    {
        #region Declarations

        private bool idle;
        private Texture2D onGround;
        private bool lying;
        private Vector2 bounds;
        private Vector2 boundSize;
        private bool lockedInBounds;
        private Color itemColor;
        private bool mouseOutOfBounds;
        public float lastY;
        private float OriginalWidth;
        private bool large;

        #endregion

        #region Constructor

        public Item(string CodeValue, Vector2 labelOffset, Texture2D texture, SpriteFont font, Vector2 location, int ID, float layer, Texture2D onground,float bounds,Color ItemColor,bool snappedtoform,bool large)
            : base(CodeValue, labelOffset, texture, font, location, ID, layer)
        {
            this.large = large;
            Layer = layer;
            Lock = false;
            onGround = onground;
            Lying = false;
            if (!large)
                bounds -= 15f;
            width = bounds * Camera.Scale;
            OriginalWidth = bounds;
            itemColor = ItemColor;
            MouseOutOfBounds = false;
            lastY = 0f;
            if (snappedtoform)
            {
                SnappedToForm = true;
                OffSet = Location - (FormManager.privateSpace.Location + Camera.WorldLocation);
            }
            type = "item";

        }

        #endregion

        #region Properties


        public Vector2 Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        public override float Width
        {
            get
            {
                if (SnappedToForm)
                    return OriginalWidth;
                else if (LockedInBounds)
                    return OriginalWidth;
                else
                    return width;
            }
            set
            {
                width = value;
            }
        }



        public Vector2 BoundSize
        {
            get { return boundSize; }
            set
            {
                boundSize = value;
            }
        }

        public bool LockedInBounds
        {
            get { return lockedInBounds; }
            set { lockedInBounds = value; }
        }

        public Vector2 AdjustLocationInBounds
        {
            get
            {
                Vector2 newLocation;
                newLocation.X = MathHelper.Clamp(location.X, Bounds.X, Bounds.X + BoundSize.X);
                newLocation.Y = MathHelper.Clamp(location.Y, Bounds.Y, Bounds.Y + BoundSize.Y);
                return newLocation;
            }
        }

        public float ScaleOffset
        {
            get
            {
                if (!large)
                    return 0.2f;
                else
                    return 0.0f;
            }
        }
        
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
                    return itemColor;
            }
        }

        public float Layer
        {
            get
            {
                if (ActiveTile && LockedInBounds)
                    return 0.04f;
                if (SnappedToForm && ActiveTile)
                    return layer - 0.32f;
                if (SnappedToForm)
                    return layer -0.29f;
                if (ActiveTile)
                    return layer - 0.15f;
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

        public bool MouseOutOfBounds
        {
            get { return mouseOutOfBounds; }
            set { mouseOutOfBounds = value; }
        }

        public override Rectangle TileRectangle
        {
            get
            {
                if (SnappedToForm)
                    return new Rectangle((int)Location.X - ((int)OriginalWidth / 2), (int)Location.Y - ((int)OriginalWidth / 2),
                           (int)OriginalWidth, (int)OriginalWidth);
                else
                    return new Rectangle((int)Location.X - ((int)Width / 2), (int)Location.Y - ((int)Width / 2),
                                 (int)Width, (int)Width);
            }

        }

        public override Vector2 MouseLocation
        {
            get
            {
                Vector2 mCenter;
                float X = mouseState.X;
                float Y = mouseState.Y;
                if (LockedInBounds)
                {
                    if (mouseState.X < Bounds.X - Camera.WorldLocation.X)
                    {
                        location.X = Bounds.X - Camera.WorldLocation.X;
                        if (!MouseOutOfBounds)
                        {
                            Y = Location.Y - Camera.WorldLocation.Y;
                            lastY = Y;
                        }
                        else
                            Y = lastY;
                        MouseOutOfBounds = true;
                    }
                    else
                    { 
                            MouseOutOfBounds = false;
                    }
              
                }

                mCenter.X = MathHelper.Clamp((X + (Camera.WorldLocation.X)), 0, TileGrid.TileWidth * TileGrid.MapWidth);
                mCenter.Y = MathHelper.Clamp((Y + (Camera.WorldLocation.Y)), 0, TileGrid.TileHeight * TileGrid.MapHeight);
               
                return mCenter;

            }
        }

        public float Scale
        {
            get
            {
                if (SnappedToForm)
                    return 1.0f;
                else if (LockedInBounds)
                    return 1.0f;
                else
                {
                    return Camera.Scale;
                }
            }
        }

        public Vector2 ItemSourceCenter
        {
            get { return new Vector2(Texture.Width / 2, Texture.Height / 2); }   
        }
        
        #endregion

        #region Helper Methods

        public void CalculateLocation(Vector2 startingPoint)
        {
            Location = startingPoint + Camera.WorldLocation + OffSet;
           // location.X = MathHelper.Min(Location.X, (startingPoint.X + Camera.WorldLocation.X + FormManager.menu.FormSize.X - TileGrid.TileWidth / 2));
        }


        public void AdjustToMenu()
        {


            if (!Moving)
            {
                
                CalculateLocation(FormManager.menu.Location);
            }
            else
                CalculateMenuOffSet();
     
            
        }
        public void CalculateMenuOffSet()
        {
   
           offSet=   Location - (FormManager.menu.Location+Camera.WorldLocation);
           // offSet.X = MathHelper.Clamp(offSet.X, 0, FormManager.menu.FormSize.X + TileGrid.TileWidth / 2);
        }

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

        public void AdjustLocationToOrigin()
        {
            
                AdjustToMenu();
                Location = AdjustLocationInBounds;
                          
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            if (LockedInBounds)
            {
                AdjustLocationToOrigin();
            }
            else
                FormIntersection(FormManager.privateSpace.FormWorldRectangle);


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
                         RotationAmount,
                         ItemSourceCenter,
                         Scale-ScaleOffset,
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

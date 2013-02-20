using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    public class Entity : Square
    {

        #region Declarations

            private Vector2 mouseBounds = new Vector2(1,1);
            public MouseState mouseState;
            private bool locked;
            private bool moving;
            private int ID;
            private Vector2 start;
            public float layer;
            public SpriteFont font;
            protected Vector2 labelOffset;
            private bool activeTile;
            private MouseState previousMouseState;

         #endregion

        #region Constructor

        public Entity(string CodeValue,Vector2 labelOffset, Texture2D texture,SpriteFont font, Vector2 location,int ID,float layer)
                : base(CodeValue, texture, location)
        {
            this.ID = ID;
            setMouseState();
            locked = false;
            this.font = font;
            this.layer = layer;
            this.labelOffset = labelOffset;
            moving = false;
            start = Vector2.Zero;
            ActiveTile = false;
            previousMouseState = Mouse.GetState();
        }

        #endregion

        #region Properties

        public bool Moving
        {
            get { return moving; }
            set { moving = value; } 
        }

        public Vector2 LabelOffset
        {
            get { return Location +labelOffset; }
        }


        public virtual Color SquareColor
        {
            get
            {
                if (MouseClick)
                    return Color.Gray;
                else
                    return Color.White;
            } 
        }

        public bool Lock
        {
            get { return locked; }
            set { locked = value; }
        }

        public Vector2 Start
        {
            get { return start; }
            set { start = value; }
        }

        public bool ActiveTile
        {
            get { return activeTile; }
            set { activeTile = value; }
        }

        #endregion
        
        #region Mouse Events

        private void setMouseState()
        {
            mouseState = Mouse.GetState();
        }

        public Vector2 MouseCenter
        {
            get { return new Vector2(MouseLocation.X, MouseLocation.Y); }
        }



        public virtual Vector2 MouseLocation
        {
            get
            {
                Vector2 mCenter;
                mCenter.X = MathHelper.Clamp((mouseState.X + (Camera.WorldLocation.X)), 0, TileGrid.TileWidth * TileGrid.MapWidth);
                mCenter.Y = MathHelper.Clamp((mouseState.Y + (Camera.WorldLocation.Y)), 0, TileGrid.TileHeight * TileGrid.MapHeight);
                return mCenter;

            }
        }

        public virtual Vector2 MouseToTile
        {
            get
            {
                Vector2 mCenter = MouseLocation;
                mCenter.X = MathHelper.Clamp(MouseLocation.X, Camera.WorldLocation.X + TileGrid.TileWidth / 2, Camera.WorldLocation.X + Camera.ViewPortWidth - TileGrid.TileWidth / 2);
                mCenter.Y = MathHelper.Clamp(MouseLocation.Y, Camera.WorldLocation.Y + TileGrid.TileHeight / 2, Camera.WorldLocation.Y + Camera.ViewPortHeight - TileGrid.TileHeight / 2);
                return mCenter;
            }
        }
              
        
        public Rectangle MouseRectangle
        {
           get
           {
            
               return new Rectangle((int)MouseCenter.X, (int)MouseCenter.Y,
                                    (int)mouseBounds.X,(int)mouseBounds.Y);
           }
        }
        
        public bool MouseOver
        {

            get { return (TileRectangle.Intersects(MouseRectangle)); }
        }
        

        public virtual bool MouseClick
        {

            get { return (mouseState.LeftButton == ButtonState.Pressed 
                            && MouseOver
                            && Camera.ObjectIsVisible(MouseRectangle));
                }

        }

        public virtual bool OnMouseClick()
        {
            if ((MouseClick && (previousMouseState.LeftButton != ButtonState.Pressed)))
            {
                previousMouseState = mouseState;
                return true;
            }

            previousMouseState = mouseState;
            return false;
        }

        #endregion

        #region Public Methods

        public void Move(Vector2 amount)
        {
            Location += amount;
        }

        public void MoveAt(Vector2 newLocation)
        {
            Location = newLocation;
        }

        public virtual void senseClick()
        {
            if (MouseClick && !Lock && !Moving)
            {
                Moving = true;
                start = MouseLocation;
            }
        }

        public virtual void dragWithMouse()
        {
            if (Moving && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (Camera.ObjectIsVisible(MouseRectangle))
                {
                    Location += MouseLocation - start;
                    Location = Camera.AdjustInWorldBounds(Location);
                    start = MouseLocation;
                }
                else
                {
                    Location = Camera.AdjustInWorldBounds(MouseLocation);
                    start = Location;
                }
           }
            else
                Moving = false;

        }

        #endregion

        #region Update

        public virtual void Update(GameTime gameTime)
        {
            setMouseState();
            senseClick();
            dragWithMouse();
        }

        #endregion

    }
}

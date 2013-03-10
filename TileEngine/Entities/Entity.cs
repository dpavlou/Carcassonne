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

    public class Entity : Square
    {

        #region Declarations

            private Vector2 mouseBounds = new Vector2(1,1);
            public MouseState mouseState;
            private bool locked;
            private bool moving;
            private int Id;
            private Vector2 start;
            public float layer;
            public SpriteFont font;
            protected Vector2 labelOffset;
            private bool activeTile;
            private MouseState previousMouseState;
            protected float width;
            private bool snappedToForm;
            protected Vector2 offSet;

         #endregion

        #region Constructor

        public Entity(string CodeValue,Vector2 labelOffset, Texture2D texture,SpriteFont font, Vector2 location,int ID,float layer)
                : base(CodeValue, texture, location)
        {
            this.Id = ID;
            setMouseState();
            locked = false;
            this.font = font;
            this.layer = layer;
            this.labelOffset = labelOffset;
            moving = false;
            offSet=start = Vector2.Zero;
            ActiveTile = false;
            previousMouseState = Mouse.GetState();
            Width = TileGrid.TileWidth;
            snappedToForm = false;

        }

        #endregion

        #region Properties

        public Vector2 OffSet
        {
            get { return offSet; }
            set { offSet = value; }
        }

        public int ID
        {
            get { return Id; }
            set { Id = value; }
        }
        
        public bool SnappedToForm
        {
            get { return snappedToForm; }
            set { snappedToForm = value; }
        }

        public virtual float Width
        {
            get { return width; }
            set
            {
                width = value;
            }
        }

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
                mCenter.X = MathHelper.Clamp(MouseLocation.X, Camera.WorldLocation.X + Width / 2, Camera.WorldLocation.X + Camera.ViewPortWidth - Width / 2);
                mCenter.Y = MathHelper.Clamp(MouseLocation.Y, Camera.WorldLocation.Y + Width / 2, Camera.WorldLocation.Y + Camera.ViewPortHeight - Width / 2);
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

         public virtual void FormIntersection(Rectangle FormRectangle)
         {
             if (TileRectangle.Intersects(FormRectangle) && Moving)
             {
                 OffSet = Location - (FormManager.privateSpace.Location+Camera.WorldLocation);
                 offSet.X = MathHelper.Clamp(offSet.X,0, FormManager.privateSpace.FormSize.X-TileGrid.TileWidth/2);
                 //SnappedToForm = true;
             }
             else if (!TileRectangle.Intersects(FormRectangle))
             {
                 OffSet = Vector2.Zero;
                 SnappedToForm = false;
             }
         }

         public virtual void CalculateOffSet(Vector2 startingPoint)
         {
             Location = startingPoint+Camera.WorldLocation + OffSet;
             location.X = MathHelper.Min(Location.X,( startingPoint.X + Camera.WorldLocation.X+FormManager.privateSpace.FormSize.X-TileGrid.OriginalTileWidth/2));
         }

        public void Move(Vector2 amount)
        {
            Location += amount;
        }

        public void MoveAt(Vector2 newLocation)
        {
            Location = newLocation;
        }

        public void AdjustToForm()
        {
            if (SnappedToForm && !Moving)
            {
                CalculateOffSet(FormManager.privateSpace.Location);
            }
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
                    Location = Camera.AdjustInWorldBounds(Location,Width);
                    start = MouseLocation;
                }
                else
                {
                    Location = Camera.AdjustInWorldBounds(MouseLocation, Width);
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
            if (!FormManager.Dragging)
            {
                setMouseState();
                senseClick();
                dragWithMouse();
            }

        }

        #endregion

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileEngine.Entity
{
    using TileEngine.Camera;

    public class Button : Entity
    {

        #region Declarations 

        private MouseState prevMouseState;
        private Vector2 bounds;
        private Vector2 boundSize;
        private bool lockedInBounds;
        private Color color;

       #endregion

        #region Constructor

        public Button(string CodeValue,Vector2 labelOffset, Texture2D texture,SpriteFont font, Vector2 location,int ID,float layer,bool lockedinbounds,Color color)
            : base(CodeValue, labelOffset,texture, font, location, ID, layer)
        {

            Lock=true;
            layer = 0.3f;
            prevMouseState = Mouse.GetState();
            lockedInBounds = lockedinbounds;
            FontColor = color;
        }

        #endregion

        #region Properties

        public Color FontColor
        {
            get { return color; }
            set { color = value; }
        }

        public Vector2 Bounds
        {
            get { return bounds; }
            set { bounds = value; }
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

        public override Vector2 MouseLocation
        {

            get
            {
                if (LockedInBounds && Moving)
                {
                    if (mouseState.X < Bounds.X)
                        return new Vector2(Bounds.X, Location.Y);
                  /*  else if (mouseState.Y < 0)
                        return new Vector2(0, 0);
                    else if (mouseState.Y > Camera.ViewPortHeight)
                        return new Vector2(0, Camera.ViewPortHeight);*/
                }
                return new Vector2(mouseState.X, mouseState.Y);
            }

        }

        public override Rectangle TileRectangle
        {
            get
            {
                return new Rectangle((int)Location.X - texture.Width / 2, (int)Location.Y - texture.Height / 2,
                              texture.Width, texture.Height);
            }

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

        public override Vector2 Location
        {
            get { return location; }
            set
            {
                location.X = MathHelper.Clamp(value.X, -400, TileGrid.TileWidth * TileGrid.MapWidth);
                location.Y = MathHelper.Clamp(value.Y, 0, TileGrid.TileHeight * TileGrid.MapHeight);
            }
        }

        public Vector2 TileSourceCenter
        {
            get { return new Vector2(texture.Width / 2,texture.Height / 2); }
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

        public override void senseClick()
        {
            if (MouseClick && (prevMouseState.LeftButton != ButtonState.Pressed) && !Lock)
            {      
                Moving = true;
                Start = MouseLocation;
            }
        }

        public override bool OnMouseClick()
        {
            if(MouseClick && (prevMouseState.LeftButton != ButtonState.Pressed)  && Lock) 
            {
                prevMouseState = mouseState;
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
                    Location = Camera.AdjustInScreenBounds(Location,TileGrid.OriginalTileWidth);
                    Start = MouseLocation;
                }
                else
                {
                    Location = Camera.AdjustInScreenBounds(MouseLocation,TileGrid.OriginalTileWidth);
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

            if (LockedInBounds)
               Location = AdjustLocationInBounds;
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
                         TileSourceCenter,
                         1.0f,
                         SpriteEffects.None,
                         layer);
            if(Lock)
                    spriteBatch.DrawString(
                      font,
                      CodeValue,
                      LabelOffset,
                      FontColor * Transparency);
            
        }

        #endregion

    }
}

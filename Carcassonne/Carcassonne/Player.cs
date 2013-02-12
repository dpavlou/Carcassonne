using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using TileEngine;

namespace Carcassonne
{
    public class Player
    {
        private Vector2 worldLocation;
        private String ID;
        private bool isScrolling = false;
        private Vector2 mousePosition;
        public Vector2 velocity;
        private Vector2 moveAmount;
        private Vector2 newMousePosition;
        private int GameWidth;
        private int GameHeight;

        private float prevWheelValue;
        private float currWheelValue;
        public float scale;


        #region Properties

        public Vector2 WorldPosition
        {
            get { return new Vector2(worldLocation.X - GameWidth / 2,
                                    worldLocation.Y - GameHeight/ 2); }
                 
        }

        #endregion

        #region Constructor

        public Player(ContentManager content,string playerName, Vector2 position)
        {
            ID = playerName;
            mousePosition = Vector2.Zero;
            velocity = Vector2.Zero;
            GameWidth = 1600;
            GameHeight = 900;
            worldLocation = position;
            prevWheelValue = currWheelValue = 0;
            scale = TileGrid.OriginalTileHeight;


        }

        #endregion

        #region Public Methods
        public void Update(GameTime gameTime)
        {
           float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var mouseState = Mouse.GetState();

            /*
             * var mousePosition = new Point(mouseState.X, mouseState.Y
             * if (area.Contains(mousePosition))
            {
                backgroundTexture = hoverTexture;
            }
                else
            {
                  backgroundTexture = defaultTexture;
            }
             * */

            ScrollScalling(mouseState);

    
            

            if (mouseState.RightButton != ButtonState.Pressed)
            {
                isScrolling = false;

            }
            
            if (mouseState.RightButton == ButtonState.Pressed
                && !isScrolling)
            {
               var mousePos = new Vector2(mouseState.X, mouseState.Y);
               mousePosition = new Vector2(mousePos.X,mousePos.Y);
               newMousePosition = new Vector2(mouseState.X, mouseState.Y); 
                isScrolling=true;
            }

            if (isScrolling)
            {
                newMousePosition = new Vector2(mouseState.X, mouseState.Y);    

                if(newMousePosition != mousePosition) //NaN error handler
                    velocity = newMousePosition - mousePosition;
                else
                    velocity = (newMousePosition-new Vector2(1,0)) - mousePosition;

                velocity.Normalize();
                velocity = Vector2.Negate(velocity);

               if (Vector2.Distance(newMousePosition, mousePosition)!=0.0f)
                velocity = velocity * Vector2.Distance(newMousePosition, mousePosition);
              
            }


            reduceVelocity();

            moveAmount =  velocity * elapsed;
            moveAmount *= 2;

            adjustLocation();
            //worldLocation += moveAmount;
   
            repositionCamera();

            TileManager.Update(gameTime, WorldPosition, ID);
        }
         #endregion

        #region Helper Methods

        private void adjustLocation()
        {
            worldLocation.X = MathHelper.Clamp(worldLocation.X += moveAmount.X, GameWidth/2, 
                                        TileGrid.MapWidth*TileGrid.TileWidth-GameWidth/2);

            worldLocation.Y = MathHelper.Clamp(worldLocation.Y += moveAmount.Y, GameHeight/2, 
                                        TileGrid.MapHeight * TileGrid.TileHeight-GameHeight/2);
        }


        private void ScrollScalling(MouseState mouseState)
        {

            float newScale = MathHelper.Clamp(scale+(currWheelValue/120 - prevWheelValue/120), 40f, TileGrid.OriginalTileWidth);

            if (newScale != scale)
            {
                Vector2 screenCenter = TileGrid.GetCellByPixel(new Vector2(worldLocation.X,
                                                                            worldLocation.Y));
                TileManager.AdjustTileLocation(ID, newScale); 
                TileGrid.TileWidth = (int)newScale;
                TileGrid.TileHeight = (int)newScale;
                float scaleDifference = newScale - scale;
                scale = newScale;
                             
                worldLocation=new Vector2(screenCenter.X*(float)TileGrid.TileWidth,
                                        (float)screenCenter.Y*TileGrid.TileHeight);
                adjustLocation();
                
            }

            prevWheelValue = currWheelValue;
            currWheelValue = mouseState.ScrollWheelValue;

            

        }
        private void reduceVelocity()
        {
            float reduceAmount = 10.0f;
            float maxAcceleration=1000.0f;

               if (velocity.X > 0)
                        velocity.X = MathHelper.Clamp(velocity.X-reduceAmount,0,maxAcceleration);
                    else
                        velocity.X = MathHelper.Clamp(velocity.X+reduceAmount,-maxAcceleration,0);

               if (velocity.Y > 0)
                   velocity.Y = MathHelper.Clamp(velocity.Y - reduceAmount, 0, maxAcceleration);
               else
                   velocity.Y = MathHelper.Clamp(velocity.Y + reduceAmount, -maxAcceleration, 0);                

        }

        private void repositionCamera()
        {
            int screenLocX = (int)Camera.WorldToScreen(worldLocation).X;
            int screenLocY = (int)Camera.WorldToScreen(worldLocation).Y;

            
            if (screenLocY > GameHeight/2)
            {
                Camera.Move(new Vector2(0, screenLocY - GameHeight / 2));
            }
            if (screenLocY < GameHeight / 2)
            {
                Camera.Move(new Vector2(0, screenLocY - GameHeight / 2));
            }

            if (screenLocX > GameWidth/2) 
            {
                Camera.Move(new Vector2(screenLocX - GameWidth / 2, 0));
            }

            if (screenLocX < GameWidth / 2)
            {
                Camera.Move(new Vector2(screenLocX - GameWidth / 2, 0));
            }
        }
    }
        #endregion

    }


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
        public Vector2 worldLocation;
        private String ID;
        private bool isScrolling = false;
        private Vector2 mousePosition;
        public Vector2 velocity;
        private Vector2 moveAmount;
        private Vector2 newMousePosition;
        private Vector2 rightClickPosition;
        public Vector2 desiredCenter;
        public bool autoPilot;

        private int GameWidth;
        private int GameHeight;
        private bool active = false; //double click simulation
        private float prevWheelValue;
        private float currWheelValue;
        private float timeSinceAutoPilot = 5.0f;
        private float timeSinceLastClick = 2.0f;
        private float timeSinceLastRightClick = 0.0f;
        public float scale;
        public MouseState prevMouseState;
        private bool onLock = false;

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
            GameWidth = Camera.ViewPortWidth;
            GameHeight = Camera.ViewPortHeight;
            worldLocation = position;
            prevWheelValue = currWheelValue = 0;
            scale = TileGrid.OriginalTileHeight;
            autoPilot = false;
            var prevmouseState = Mouse.GetState();
            prevMouseState = Mouse.GetState();
        }

        #endregion

        #region Public Methods
        public void Update(GameTime gameTime)
        {
           float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

           timeSinceAutoPilot = MathHelper.Min(timeSinceAutoPilot + elapsed, 5.0f);
           timeSinceLastClick= MathHelper.Min(timeSinceLastClick + elapsed, 5.0f);
   
            var mouseState = Mouse.GetState();

            ScrollScalling(mouseState);

            if (Keyboard.GetState().IsKeyDown(Keys.A) || 
                ( mouseState.XButton1 == ButtonState.Pressed && prevMouseState.XButton1 !=ButtonState.Pressed))
            {
                TileManager.AddRotatingTile(ID, false);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) ||
                  (mouseState.XButton2 == ButtonState.Pressed && prevMouseState.XButton2 != ButtonState.Pressed))
            {
                TileManager.AddRotatingTile(ID, true);
            }
             

            
            if (mouseState.RightButton == ButtonState.Pressed
                && !isScrolling)
            {
                autoPilot = false;
               var mousePos = new Vector2(mouseState.X, mouseState.Y);
               mousePosition = new Vector2(mousePos.X,mousePos.Y);
               newMousePosition = new Vector2(mouseState.X, mouseState.Y);
               rightClickPosition = new Vector2(mouseState.X, mouseState.Y); 
                isScrolling=true;
               
            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                timeSinceLastRightClick = MathHelper.Min(timeSinceLastRightClick + elapsed, 2.0f);
                velocity = rightClickPosition - new Vector2(mouseState.X, mouseState.Y);
                rightClickPosition = new Vector2(mouseState.X, mouseState.Y);
                onLock = true;
                moveAmount = velocity;
                velocity *= 8;
            }

       /*     if (isScrolling && 
                mouseState.RightButton != ButtonState.Pressed 
                && timeSinceLastRightClick<0.4f)
            {
          /*      newMousePosition = new Vector2(mouseState.X, mouseState.Y);    

                if(newMousePosition != mousePosition) //NaN error handler
                    velocity = newMousePosition - mousePosition;
                else
                    velocity = (newMousePosition-new Vector2(1,0)) - mousePosition;

                velocity.Normalize();
                velocity = Vector2.Negate(velocity);

               if (Vector2.Distance(newMousePosition, mousePosition)!=0.0f)
                velocity = velocity * Vector2.Distance(newMousePosition, mousePosition);*/
              
            //}

            if (mouseState.RightButton != ButtonState.Pressed)
            {
                isScrolling = false;
                timeSinceLastRightClick = 0.0f;
                onLock = false;
            }


            if (simulateDoubleClick(mouseState))
            {
                desiredCenter = (new Vector2(mouseState.X, mouseState.Y)+ worldLocation);
                calculateVelocity();
                autoPilot=true;
                timeSinceAutoPilot = 0.0f;
            }

          

            if (!onLock)
            {
                reduceVelocity();
                moveAmount = velocity * elapsed;


                if (ReachedDesiredLocation())
                    calculateVelocity();

                moveAmount *= 2;
            }
              

            adjustLocation();
            //worldLocation += moveAmount;
   
            repositionCamera();

            prevMouseState = mouseState;

            TileManager.Update(gameTime, WorldPosition, ID);
        }
         #endregion

        #region Helper Methods

        private void calculateVelocity()
        {
            velocity = desiredCenter - Camera.WorldScreenCenter(worldLocation);
        }

        public float calculateStep()
        {
      
            return (Vector2.Distance(Camera.WorldScreenCenter(worldLocation), desiredCenter));
      
        }

        public bool simulateDoubleClick(MouseState mouseState)
        {

           
            if (mouseState.LeftButton == ButtonState.Pressed)
                timeSinceLastClick = 0.0f;

            if (mouseState.LeftButton == ButtonState.Pressed && active)
            {
                active = false;
                return true;
            }
            if(mouseState.LeftButton != ButtonState.Pressed 
                && timeSinceLastClick <0.1f )
                active=true;
            else
                active=false;

             return false;

        }

        private bool ReachedDesiredLocation()
        {
            if (autoPilot)
            {
                if (TileGrid.CellWorldRectangle(desiredCenter).Intersects
                   (TileGrid.CellWorldRectangle(Camera.WorldScreenCenter(worldLocation)))
                    || timeSinceAutoPilot > 2.5f)
                {
                    autoPilot = false;
                    return false;
                }
                else
                {
                   // if (timeSinceAutoPilot % 0.2f < 0.01)
                       // ScaleScreen(MathHelper.Min(scale + 1, TileGrid.OriginalTileHeight));

                    return true;
                }
            }

            autoPilot = false;
            return false;

        }

        private void adjustLocation()
        {
            worldLocation.X = MathHelper.Clamp(worldLocation.X += moveAmount.X, GameWidth/2, 
                                        TileGrid.MapWidth*TileGrid.TileWidth-GameWidth/2);

            worldLocation.Y = MathHelper.Clamp(worldLocation.Y += moveAmount.Y, GameHeight/2, 
                                        TileGrid.MapHeight * TileGrid.TileHeight-GameHeight/2);
        }

        private void ScaleScreen(float newScale)
        {
            if (newScale != scale)
            {
                autoPilot = false;
                Vector2 screenCenter = TileGrid.GetCellByPixel(new Vector2(worldLocation.X,
                                                                            worldLocation.Y));
                TileManager.AdjustTileLocation(ID, newScale);
                TileGrid.TileWidth = (int)newScale;
                TileGrid.TileHeight = (int)newScale;
                float scaleDifference = newScale - scale;
                scale = newScale;

                worldLocation = new Vector2(screenCenter.X * (float)TileGrid.TileWidth,
                                        (float)screenCenter.Y * TileGrid.TileHeight);
                adjustLocation();

            }


        }

        private void ScrollScalling(MouseState mouseState)
        {
    
            float newScale = MathHelper.Clamp(scale+(currWheelValue/120 - prevWheelValue/120), 40f, TileGrid.OriginalTileWidth);

            ScaleScreen(newScale);

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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileEngine;

namespace Carcassonne
{
    public class CameraHandler
    {

        #region Declarations

        public Vector2 worldLocation;
        private bool isScrolling = false;
        private Vector2 mousePosition;
        public Vector2 velocity;
        private Vector2 moveAmount;
        private Vector2 newMousePosition;
        private Vector2 rightClickPosition;
        public Vector2 desiredCenter;
        public Vector2 offSet;
        public bool autoPilot;

        private bool active = false; //double click simulation
        private float prevWheelValue;
        private float currWheelValue;
        private float timeSinceAutoPilot = 5.0f;
        private float timeSinceLastClick = 2.0f;
        private float timeSinceLastRightClick = 0.0f;
        public float scale;
        private MouseState mouseState;
        public MouseState prevMouseState;
        private KeyboardState currKeyState;
        private KeyboardState prevKeyState;
        private bool onLock = false;

        #endregion

        #region Constructor

        public CameraHandler(Vector2 position)
        {
            mousePosition = Vector2.Zero;
            offSet = velocity = Vector2.Zero;
            worldLocation = position;
            prevWheelValue = currWheelValue = 0;
            scale = TileGrid.OriginalTileHeight;
            autoPilot = false;
            prevMouseState = Mouse.GetState();
            prevKeyState = Keyboard.GetState();
        }

        #endregion

        #region Properties

        public Vector2 WorldPosition
        {
            get { return new Vector2(worldLocation.X - Camera.ViewPortWidth / 2,
                                    worldLocation.Y - Camera.ViewPortHeight / 2);
            }
                 
        }

        #endregion

        #region Helper Methods

        private void calculateVelocity()
        {
            velocity = desiredCenter - Camera.WorldScreenCenter(worldLocation);
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
 
                    return true;
                }
            }

            autoPilot = false;
            return false;

        }

        private void adjustLocation()
        {
            worldLocation.X = MathHelper.Clamp(worldLocation.X += moveAmount.X, Camera.ViewPortWidth / 2,
                                        TileGrid.MapWidth * TileGrid.TileWidth - Camera.ViewPortWidth / 2);

            worldLocation.Y = MathHelper.Clamp(worldLocation.Y += moveAmount.Y, Camera.ViewPortHeight / 2,
                                        TileGrid.MapHeight * TileGrid.TileHeight - Camera.ViewPortHeight / 2);
        }

        private void ScaleScreen(float newScale)
        {
            if (newScale != scale)
            {
                autoPilot = false;
                Vector2 screenCenter = TileGrid.GetCellByPixel(new Vector2(worldLocation.X,
                                                                            worldLocation.Y));
                CalculateOffset(screenCenter);
                TileManager.AdjustTileLocation(newScale,scale);
                TileManager.AdjustItemLocation(newScale, scale);
                TileGrid.TileWidth = (int)newScale;
                TileGrid.TileHeight = (int)newScale;
                float scaleDifference = newScale - scale;
                scale = newScale;

                worldLocation = new Vector2(screenCenter.X * (float)TileGrid.TileWidth,
                                        (float)screenCenter.Y * TileGrid.TileHeight);
                worldLocation += offSet;
                adjustLocation();

            }


        }

        private void CalculateOffset(Vector2 screenCenter)
        {
            offSet = worldLocation - screenCenter* new Vector2(TileGrid.TileWidth,TileGrid.TileHeight);
            offSet *= Camera.Scale;
        }

        private void ScrollScalling(MouseState mouseState)
        {
            float newValue=0;
            if ((currWheelValue / 120 - prevWheelValue / 120) > 0)
                newValue = 2;
            else if ((currWheelValue / 120 - prevWheelValue / 120) < 0)
                newValue = -2;

            if (newValue != 0)
            {

                float newScale = MathHelper.Clamp(scale + newValue, 40f, TileGrid.OriginalTileWidth);
                ScaleScreen(newScale);
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


            if (screenLocY > Camera.ViewPortHeight / 2)
            {
                Camera.Move(new Vector2(0, screenLocY - Camera.ViewPortHeight / 2));
            }
            if (screenLocY < Camera.ViewPortHeight / 2)
            {
                Camera.Move(new Vector2(0, screenLocY - Camera.ViewPortHeight / 2));
            }

            if (screenLocX > Camera.ViewPortWidth / 2) 
            {
                Camera.Move(new Vector2(screenLocX - Camera.ViewPortWidth / 2, 0));
            }

            if (screenLocX < Camera.ViewPortWidth / 2)
            {
                Camera.Move(new Vector2(screenLocX - Camera.ViewPortWidth / 2, 0));
            }
        }
    
        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
           float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

           timeSinceAutoPilot = MathHelper.Min(timeSinceAutoPilot + elapsed, 5.0f);
           timeSinceLastClick= MathHelper.Min(timeSinceLastClick + elapsed, 5.0f);
   
            mouseState = Mouse.GetState();
            currKeyState = Keyboard.GetState();

            ScrollScalling(mouseState);

            
            if (mouseState.RightButton == ButtonState.Pressed
                && !isScrolling)
            {
                autoPilot = false;
                mousePosition = new Vector2(mouseState.X,mouseState.Y);
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
   
            repositionCamera();

            Camera.WorldLocation = WorldPosition;

            prevMouseState = mouseState;
            prevKeyState = currKeyState;

      //      TileManager.Update(gameTime, WorldPosition, ID);
        }

         #endregion

    }

    }
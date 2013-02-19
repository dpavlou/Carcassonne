using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{

    public class RotatingTile : Entity
    {

        #region Declarations

        private bool clockwise;
        private bool active;
        private static float rotationRate;
        private float rotationAmount;
        private int rotationTicksRemaining;
        private MouseState prevMouseState;
        private KeyboardState currKeyState;
        private KeyboardState prevKeyState;

        #endregion

        #region Constructor

        public RotatingTile(string CodeValue, Vector2 labelOffset, Texture2D texture, SpriteFont font, Vector2 location, int ID, float layer)
            : base(CodeValue, labelOffset, texture, font, location, ID, layer)
        {
            rotationRate = (MathHelper.PiOver2 / 10);
            prevMouseState = Mouse.GetState();
            prevKeyState = Keyboard.GetState();
            rotationTicksRemaining = 10;
            rotationAmount = 0;
            active = false;
        }

        #endregion

        #region Properties

        public bool Active
        {
            get { return active; }
        }

        public float RotationAmount
        {
            get
            {
                if (Active)
                {
                    UpdateRotation();
                    if (clockwise)
                        return rotationAmount += rotationRate;
                    else

                        return rotationAmount -= rotationRate;
                }
                else
                    return rotationAmount;
            }
        }

        #endregion

        #region PublicMethods

        public void HandleRotation()
        {
            mouseState = Mouse.GetState();
            currKeyState = Keyboard.GetState();

            if (((currKeyState.IsKeyDown(Keys.A) && !prevKeyState.IsKeyDown(Keys.A))
                || (mouseState.XButton1 == ButtonState.Pressed && prevMouseState.XButton1 != ButtonState.Pressed))
                && !Active)
            {
                RotateTile(true);
            }
            else if (((currKeyState.IsKeyDown(Keys.S) && !prevKeyState.IsKeyDown(Keys.S))
                || (mouseState.XButton2 == ButtonState.Pressed && prevMouseState.XButton2 != ButtonState.Pressed))
                  && !Active)
            {
                RotateTile(false);
            }
            prevMouseState = mouseState;
            prevKeyState = currKeyState;
        }

        public void RotateTile(bool clockwise)
        {
            this.clockwise = clockwise;
            rotationTicksRemaining = 10;
            active = true;
        }


        public void UpdateRotation()
        {

            if (rotationTicksRemaining == 1)
                active = false;

            rotationTicksRemaining = (int)MathHelper.Max(
                0,
                rotationTicksRemaining - 1);

        }

        #endregion

    }
}
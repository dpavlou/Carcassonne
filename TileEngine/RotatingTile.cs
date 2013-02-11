using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TileEngine
{
    public class RotatingTile
    {
       public bool clockwise;
       public bool alive;
        public static float rotationRate = (MathHelper.PiOver2 / 10);
        private float rotationAmount = 0;
        public int rotationTicksRemaining = 10;

        public float RotationAmount
        {
            get
            {
                if (clockwise)
                    return rotationAmount;
                else
                    return (MathHelper.Pi * 2) - rotationAmount;
            }
        }

        public RotatingTile(bool clockwise,float rotationAmount)
        {
            this.clockwise = clockwise;
            this.rotationAmount = rotationAmount;
            rotationTicksRemaining = 10;
            alive = true;
        }

        public bool Alive
        {
            get { return alive; }
        }

        public void UpdateRotation()
        {
            rotationAmount += rotationRate;
            if (rotationTicksRemaining == 0)
                alive = false;

            rotationTicksRemaining = (int)MathHelper.Max(
                0,
                rotationTicksRemaining - 1);
      
        }
    }
}

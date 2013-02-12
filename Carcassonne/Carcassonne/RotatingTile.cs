using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine;

namespace TileEngine
{
    public class RotatingTile : Tile
    {
        public bool clockwise;
        private bool active;
        public static float rotationRate = (MathHelper.PiOver2 / 10);
        private float rotationAmount = 0;
        public int rotationTicksRemaining = 10;

        public float RotationAmount
        {
            get
            {
                if (clockwise)
                    return rotationAmount += rotationRate;
                else
                    return rotationAmount -= rotationRate;
            }
        }

        public RotatingTile(int background,
                            int interactive,
                            int foreground,
                            string code,
                            Vector2 Position,
                            bool clockwise,
                            float rotationAmount)
            : base(background, interactive, foreground, code, Position)
        {
            this.clockwise = clockwise;
            this.rotationAmount = rotationAmount;
            rotationTicksRemaining = 10;
            active = true;
           
        }

        public bool Active
        {
            get { return active; }
        }

        public void UpdateRotation(Vector2 location)
        {
            position = location;


            if (rotationTicksRemaining == 0)
                active = false;

            rotationTicksRemaining = (int)MathHelper.Max(
                0,
                rotationTicksRemaining - 1);

        }

     /*   public Vector2 RotateAround(Vector2 source, Vector2 target, float radians)
        {
            Vector2 fromAround = source - target;
            float postRotationX = (float)(fromAround.X * Math.Cos(radians) - fromAround.Y * Math.Sin(radians));
            float postRotationY = (float)(fromAround.X * Math.Sin(radians) - fromAround.Y * Math.Cos(radians));
            return target + new Vector2(postRotationX, postRotationY);
        }
        */

        public void Draw(SpriteBatch spriteBatch)
        {
                      
           spriteBatch.Draw(
                      TileGrid.tempTile,
                      Camera.WorldToScreen(position+new Vector2(TileGrid.TileWidth/2,TileGrid.TileHeight/2)),
                      null,
                      Color.CornflowerBlue,
                      RotationAmount,
                      TileGrid.TileSourceCenter(0),
                      ((float)TileGrid.TileWidth/(float)TileGrid.OriginalTileWidth),
                      SpriteEffects.None,
                      0.1f);
                /*             spriteBatch.Draw(
                              TileGrid.tileSheet,
                              TileGrid.ExactScreenRectangle(Camera.WorldToScreen(RotateAround(position , position + new Vector2(TileGrid.TileWidth, 0),
                                                                                              100.0f))),
                              TileGrid.TileSourceRectangle(LayerTiles[i]),
                              Color.White,
                              rotationAmount,
                              Vector2.Zero,
                              SpriteEffects.None,
                              0f);
                      */  
          
        }

    }
}

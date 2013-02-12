using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine

{
    [Serializable]
    public class Tile
    {
        #region Declarations
        public int[] LayerTiles = new int[3];
        public string CodeValue = "";
        public bool passable = true;
        public Vector2 currPos = Vector2.Zero;
        public float rotation = 0.0f;
        private int rotationState = 0;
        #endregion

        #region Constructor
        public Tile(
            int background,
            int interactive,
            int foreground,
            string code,
            Vector2 Position
            )
        {
            LayerTiles[0] = background;
            LayerTiles[1] = interactive;
            LayerTiles[2] = foreground;
            CodeValue = code;
            currPos = Position;
        }
        #endregion

        #region Public Methods
        public Vector2 OffSetPosition
        {
            get
            {
                Vector2 offSet = Vector2.Zero;
                if (RotationState==1)
                    offSet.X += (float)TileGrid.TileWidth;
                else if (RotationState == 2)
                {
                    offSet.X += (float)TileGrid.TileWidth;
                    offSet.Y += (float)TileGrid.TileHeight;
                }
                else if (RotationState == 3)
                    offSet.Y += (float)TileGrid.TileHeight;

                return currPos + offSet;
            }

            set { currPos = value; }
        }

        public Vector2 position
        {
            get
            {
                return currPos;
            }

            set
            {
                currPos = value;
            }

        }

        public int RotationState
        {
            get { return rotationState; }
            set { rotationState = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public bool checkID(string ID)
        {
            return (ID == CodeValue);                
        }

        public bool Passable
        {
            get { return passable; }
            set { passable = value; }
        }


        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    public class Tile : RotatingTile
    {
        #region Declarations

        private bool onGrid;
        private bool idle;

        #endregion

        #region Constructor

        public Tile(string CodeValue,Vector2 labelOffset, Texture2D texture,SpriteFont font, Vector2 location,int ID,float layer)
            :base(CodeValue,labelOffset,texture,font,location,ID,layer)
        {
            onGrid = true;
            Layer = layer;
            Lock = false;
            idle = true;
        }
        
        #endregion

        #region Properties

        public override Color SquareColor
        {
            get
            {
                if (ActiveTile && !Lock)
                    return Color.Gray;
                else
                    return Color.White;
            }
        }

        public float Layer
        {
            get {

                if (Active && OnGrid)
                    return layer - 0.1f;
                if(OnGrid)
                    return layer+0.1f;
                if (Active && !OnGrid)
                    return layer - 0.2f;

                if (!OnGrid)
                    return layer - 0.2f;
                if(ActiveTile)
                    return layer-0.3f;
                return layer;
            }
            set { layer = value; }
        }

        public bool OnGrid
        {
            get { return onGrid; }
            set { onGrid = value; }
        }


        public bool Idle
        {
            get { return idle; }
            set { idle = value; }
        }
        #endregion

        #region Helper Methods

        public override void senseClick()
        {
            if (OnMouseClick() && !Lock && !Moving)
            {
                Moving = true;
                Start = MouseLocation;
            }
        }

        public void SnapToGrid()
        {
            if (!MouseClick && !OnGrid && !Moving && !Idle)
            {
                if (TileGrid.mapCells[TileGrid.GetCellByPixelX((int)Location.X),
                        TileGrid.GetCellByPixelY((int)Location.Y)].Occupied)
                {
                    TileGrid.mapCells[TileGrid.GetCellByPixelX((int)Location.X),
                    TileGrid.GetCellByPixelY((int)Location.Y)].CodeValue = CodeValue;
                    Location = TileGrid.GetCellLocation(Location)+ new Vector2(TileGrid.TileWidth / 2, TileGrid.TileHeight / 2);
                    OnGrid = true;
                }
                else
                    OnGrid = false;
                Idle = true;        
            }

        }

        public void ReleaseSquare()
        {
            if (MouseClick && Moving)
            {
                Idle = false;
                if (OnGrid)
                {
                    TileGrid.mapCells[TileGrid.GetCellByPixelX((int)Location.X),
                    TileGrid.GetCellByPixelY((int)Location.Y)].CodeValue = "";
                    OnGrid = false;

                }
            }
        }



        #endregion
     
        #region Update

        public override void Update(GameTime gameTime)
        {

            if (ActiveTile)
            {
                HandleRotation();
            }

            SnapToGrid();
            ReleaseSquare();
            base.Update(gameTime);
        }

        #endregion
        
        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Camera.ObjectIsVisible(TileRectangle))
            {
                spriteBatch.Draw(
                         texture,
                         Camera.WorldToScreen(Location),
                         null,
                         SquareColor,
                         RotationAmount,
                         TileGrid.TileSourceCenter(0),
                         Camera.Scale,
                         SpriteEffects.None,
                         Layer);

                if (!Lock)
                    spriteBatch.DrawString(
                      font,
                      CodeValue,
                      Camera.WorldToScreen(LabelOffset),
                      Color.Red);
                                               
            }
        }   

        #endregion


    }
}

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
        private Texture2D frame1;
        private Texture2D frame2;

        #endregion

        #region Constructor

        public Tile(string CodeValue,Vector2 labelOffset, Texture2D texture,SpriteFont font, Vector2 location,int ID,float layer,Texture2D Frame1,Texture2D Frame2)
            :base(CodeValue,labelOffset,texture,font,location,ID,layer)
        {
            onGrid = true;
            Layer = layer;
            Lock = false;
            idle = true;
            frame1=Frame1;
            frame2=Frame2;
        }
        
        #endregion

        #region Properties

        public float Transparency
        {
            get
            {
                if (MouseOver)
                    return 1.0f;
                else
                    return 0.4f;
            }
        }

        public override Color SquareColor
        {
            get
            {
                if (ActiveTile && !Lock)
                    return Color.Red;
                else
                    return Color.Blue;
            }
        }

        public float Layer
        {
            get {
                if (ActiveTile)
                    return layer - 0.2f;
                if (Active && OnGrid)
                    return layer - 0.03f;
                if(OnGrid)
                    return layer+0.03f;
                if (Active && !OnGrid)
                    return layer - 0.06f;

                if (!OnGrid)
                    return layer - 0.06f;
            
                return layer;
            }
            set { layer = value; }
        }

        public bool OnGrid
        {
            get { return onGrid; }
            set { onGrid = value; }
        }

        public Texture2D FrameTexture
        {
            get
            {
                if (ActiveTile)
                    return frame1;
                else
                    return frame2;
            }
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

        public void RotateThis()
        {
            if (!Lock)
                HandleRotation();
        }

        #endregion
     
        #region Update

        public override void Update(GameTime gameTime)
        {
            
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
                         Color.White,
                         RotationAmount,
                         TileGrid.TileSourceCenter(0),
                         Camera.Scale,
                         SpriteEffects.None,
                         Layer);

                if (!Lock)
                {
                    spriteBatch.DrawString(
                      font,
                      CodeValue,
                      Camera.WorldToScreen(LabelOffset),
                      Color.White*Transparency);
                
                    spriteBatch.Draw(
                        FrameTexture,
                        Camera.WorldToScreen(Location),
                        null,
                        Color.White,
                        RotationValue,
                        TileGrid.TileSourceCenter(0),
                        Camera.Scale,
                        SpriteEffects.None,
                        Layer-0.05f);
                   
                }              
            }
        }   

        #endregion


    }
}

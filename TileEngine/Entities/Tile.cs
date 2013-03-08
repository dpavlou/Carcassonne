using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileEngine.Entity
{

    using TileEngine.Camera;
    using TileEngine.Form;
    using MultiplayerGame.Args;
    using MultiplayerGame.Networking.Messages;
    using MultiplayerGame.Networking;

    public class Tile : RotatingTile
    {
        
        #region Declarations


        private bool showText;
        private bool onGrid;
        private bool idle;
        private Texture2D frame1;
        private Color fontColor;
        private float timeSinceShowText;

        #endregion

        #region Constructor

        public Tile(string CodeValue,Vector2 labelOffset, Texture2D texture,SpriteFont font, Vector2 location,int ID,float layer,Texture2D Frame1,Color FontColor)
            :base(CodeValue,labelOffset,texture,font,location,ID,layer)
        {
            onGrid = true;
            Layer = layer;
            Lock = false;
            idle = true;
            frame1=Frame1;
            fontColor = FontColor;
            SnappedToForm = true;
            OffSet = Location - (FormManager.privateSpace.Location + Camera.WorldLocation);
            type = "tile";
            ShowText = false;
            timeSinceShowText = 3.0f;

        }
        
        #endregion


        #region Properties

        public bool ShowText
        {
            get
            {
                if (timeSinceShowText < 2.0f)
                {
                    timeSinceShowText += 0.4f; return showText;
                }
                else
                    return false;
            }
            set { timeSinceShowText = 0; showText = value; }
        }

        public float Transparency
        {
            get
            {
                if (MouseOver)
                    return 1.0f;
                else
                    return 0.0f;
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
                if (SnappedToForm && ActiveTile)
                    return layer - 0.4f;
                if (SnappedToForm)
                    return layer - 0.31f;
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

        public float Scale
        {
            get
            {
                if (SnappedToForm)
                    return 1.0f;
                else
                    return Camera.Scale;
            }
        }

        public Texture2D FrameTexture
        {
            get
            {
                    return frame1;
            }
        }

        public bool Idle
        {
            get { return idle; }
            set { idle = value; }
        }

        public override Rectangle TileRectangle
        {
            get
            {
                if(SnappedToForm)
                    return new Rectangle((int)Location.X - TileGrid.OriginalTileWidth / 2, (int)Location.Y - TileGrid.OriginalTileHeight / 2,
                          TileGrid.OriginalTileWidth, TileGrid.OriginalTileHeight);       
                else
                 return new Rectangle((int)Location.X - TileGrid.TileWidth / 2, (int)Location.Y - TileGrid.TileHeight / 2,
                                TileGrid.TileWidth, TileGrid.TileHeight);
            }

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
            if (IsReadyToSnap()) 
            {
                TileGrid.OnSnapToGrid(this, TileGrid.PlayerID);
                CheckCell();
            }

        }

        public bool IsReadyToSnap()
        {
            return (!MouseClick && !OnGrid && !Moving && !Idle && !SnappedToForm);
        }

        public void CheckCell()
        {
            if (TileGrid.mapCells[TileGrid.GetCellByPixelX((int)Location.X),
                TileGrid.GetCellByPixelY((int)Location.Y)].Occupied)
            {
                TileGrid.mapCells[TileGrid.GetCellByPixelX((int)Location.X),
                TileGrid.GetCellByPixelY((int)Location.Y)].CodeValue = CodeValue;
                Location = TileGrid.GetCellLocation(Location) + new Vector2(TileGrid.TileWidth / 2, TileGrid.TileHeight / 2);
                OnGrid = true;
                
            }
            else
                OnGrid = false;
            Idle = true;
        }

        public void ReleaseSquare()
        {
            if (Moving)
            {
                Idle = false;
                if (OnGrid)
                {
                 
                    ResetMapCell();
                    TileGrid.OnRemoveFromGrid(this, TileGrid.PlayerID);
                }
            }
        }

        public void ResetMapCell()
        {
            TileGrid.mapCells[TileGrid.GetCellByPixelX((int)Location.X),
            TileGrid.GetCellByPixelY((int)Location.Y)].CodeValue = "";
            OnGrid = false;
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

            base.Update(gameTime);
            SnapToGrid();
            ReleaseSquare();
            FormIntersection(FormManager.privateSpace.FormWorldRectangle);

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
                         Scale,
                         SpriteEffects.None,
                         Layer);

                if (!Lock)
                {

                    spriteBatch.Draw(
                       FrameTexture,
                       Camera.WorldToScreen(Location),
                       null,
                       Color.White * Transparency,
                       RotationValue,
                       TileGrid.TileSourceCenter(0),
                       Scale,
                       SpriteEffects.None,
                       Layer - 0.05f);

                }
                if (ShowText)
                {
                    spriteBatch.DrawString(
                    font,
                    CodeValue,
                    Camera.WorldToScreen(LabelOffset),
                    Color.Black);
                }

            }
        }

        #endregion


    }
}

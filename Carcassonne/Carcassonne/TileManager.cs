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
    public static class TileManager
    {
        #region Declarations

        public static List<Tile> tiles = new List<Tile>();
        public static SpriteFont font;
        public static Texture2D texture;
        public static int ID;

        #endregion

        #region Initialize

        public static void Initialize(Texture2D Texture, SpriteFont Font)
        {
            texture = Texture;
            font = Font;
            ID = 0;
        }

        #endregion

        #region Helper Methods

        public static void AddTile(Vector2 location,string owner)
        {
            //texture = randomTexture;
            ID++;
            tiles.Add(new Tile(owner, new Vector2(-10, -5), texture, font, location, ID, 0.4f));
        }

        public static void LockTiles(string owner)
        {

            bool lockEverything = false;

            foreach (Tile tile in tiles)
                if (tile.checkValue(owner))
                {
                    if (!tile.Lock)

                    {
                        lockEverything = true;
                        break;
                    }
                }

            if (lockEverything)
            {
                foreach (Tile tile in tiles)
                    if (tile.checkValue(owner))
                    {
                        if (!tile.Lock)
                        {
                            tile.Lock = true;
                        }
                    }
            }
            else
            {
                foreach (Tile tile in tiles)
                    if (tile.checkValue(owner))
                    {
                        tile.Lock = false;
                    }
            }
            
        }

        static public void LockTiles()
        {
            foreach (Tile tile in tiles)
                    tile.Lock = true;
        }

        static public void NewActiveTile(int x)
        {
            if (PlayerManager.ActiveTileType == "tile")
                tiles[PlayerManager.ActiveTileID].ActiveTile = false;
            PlayerManager.ActiveTileType = "tile";
            tiles[x].ActiveTile = true;
            PlayerManager.ActiveTileID = x;
            PlayerManager.ActiveTile = true;
        }

        static public void AdjustTileLocation(float newScale, float scale)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.OnGrid)
                {
                    tile.Location = new Vector2((float)TileGrid.GetCellByPixelX((int)tile.Location.X) * newScale
                                                , (float)TileGrid.GetCellByPixelX((int)tile.Location.Y) * newScale);
                    tile.Location += new Vector2(TileGrid.TileWidth / 2, TileGrid.TileHeight / 2);
                }
                else
                {
                    Vector2 tempPos = tile.Location - (new Vector2((float)TileGrid.GetCellByPixelX((int)tile.Location.X) * scale
                                        , (float)TileGrid.GetCellByPixelX((int)tile.Location.Y) * scale));
                    tile.Location = new Vector2((float)TileGrid.GetCellByPixelX((int)tile.Location.X) * newScale
                                                , (float)TileGrid.GetCellByPixelX((int)tile.Location.Y) * newScale) + tempPos * newScale / scale;
                }
              
            }
        }
        
        #endregion

        #region Update

        public static void Update(GameTime gameTime)
        {
            if (!PlayerManager.ActiveTile)
            {
                for (int x = tiles.Count - 1; x >= 0; x--)
                {
                    if (!tiles[x].OnGrid)
                        tiles[x].Update(gameTime);
                    if (tiles[x].Moving)
                    {
                        NewActiveTile(x);
                        break;

                    }
                }
            }

            if (PlayerManager.ActiveTile && PlayerManager.ActiveTileType=="tile")
            {
                tiles[PlayerManager.ActiveTileID].Update(gameTime);
                if (!tiles[PlayerManager.ActiveTileID].Moving)
                    PlayerManager.ActiveTile = false;
            }
            else
            {
                for (int x = tiles.Count - 1; x >= 0; x--)

                {
                    if (x == PlayerManager.ActiveTileID && PlayerManager.ActiveTileType == "tile")
                       tiles[x].ActiveTile = true;
                    else
                       tiles[x].ActiveTile = false;

                    tiles[x].Update(gameTime);
                    if (tiles[x].Moving)
                    {
                        NewActiveTile(x);
                        break;
                   
                    }

                }
            }
          

        }

        #endregion

        #region Draw

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
                tile.Draw(spriteBatch);
        }

        #endregion
    }

}

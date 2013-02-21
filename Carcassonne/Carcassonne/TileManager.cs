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
        public static List<Item> items = new List<Item>();
        public static SpriteFont font;
        public static Texture2D frame1,frame2;
        public static int ID;
        public static int itemID;

        #endregion

        #region Initialize

        public static void Initialize(Texture2D Frame1,Texture2D Frame2, SpriteFont Font)
        {
            frame1 = Frame1;
            frame2 = Frame2;
            font = Font;
            itemID=ID = 0;
        }

        #endregion

        #region Helper Methods

        public static void AddTile(Vector2 location,string owner)
        {
            //texture = randomTexture;
            ID++;
            tiles.Add(new Tile(owner, new Vector2(-23, -10), Deck.GetRandomTile(), font, location, ID, 0.5f-ID*0.001f,frame1,frame2));
        }

        public static void AddSoldier(Vector2 location, string owner)
        {
            itemID++;
            items.Add(new Item(owner, new Vector2(-23, -10), Deck.GetSoldier(), font, location, ID, 0.4f - itemID * 0.001f, Deck.GetSoldier(),55f));
        }

        public static void LockTiles()
        {

            bool lockEverything = false;

            foreach (Tile tile in tiles)

                    if (!tile.Lock)

                    {
                        lockEverything = true;
                        break;
                    }

            foreach (Item item in items)

                if (!item.Lock)
                {
                    lockEverything = true;
                    break;
                }

            if (lockEverything)
            {
                foreach (Tile tile in tiles)

                        if (!tile.Lock)
                        {
                            tile.Lock = true;
                        }

                foreach (Item item in items)

                    if (!item.Lock)
                    {
                        item.Lock = true;
                    }

            }
            else
            {
                foreach (Tile tile in tiles)

                        tile.Lock = false;

                foreach (Item item in items)

                        item.Lock = false;

            }
            
        }

        static public void LockAllTiles()
        {
            foreach (Tile tile in tiles)
                    tile.Lock = true;

            foreach (Item item in items)

                item.Lock = true;
        }

        static public void NewActiveTile(int x)
        {
            if (PlayerManager.ActiveTileType == "tile")
                tiles[PlayerManager.ActiveTileID].ActiveTile = false;
            PlayerManager.ActiveTileType = "tile";
            PlayerManager.RotatingType = "tile";
            tiles[x].ActiveTile = true;
            PlayerManager.ActiveTileID = x;
            PlayerManager.ActiveTile = true;
        }

        static public void NewActiveItem(int x)
        {
            if (PlayerManager.ActiveTileType == "item")
                items[PlayerManager.ActiveTileID].ActiveTile = false;
            PlayerManager.ActiveTileType = "item";
            PlayerManager.RotatingType = "item";
            items[x].ActiveTile = true;
            PlayerManager.ActiveTileID = x;
            PlayerManager.ActiveTile = true;
        }

        static public void AdjustTileLocation(float newScale, float scale)
        {
            foreach (Tile tile in tiles)
            {
                if(!tile.Moving)
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


        static public void AdjustItemLocation(float newScale, float scale)
        {
            foreach (Item item in items)
            {
                item.Width *= newScale / scale;
                if (!item.Moving)
                {
                    Vector2 tempPos = item.Location - (new Vector2((float)TileGrid.GetCellByPixelX((int)item.Location.X) * scale
                                        , (float)TileGrid.GetCellByPixelX((int)item.Location.Y) * scale));
                    item.Location = new Vector2((float)TileGrid.GetCellByPixelX((int)item.Location.X) * newScale
                                                , (float)TileGrid.GetCellByPixelX((int)item.Location.Y) * newScale) + tempPos * newScale / scale;
                }

            }
        }

        static public void RotateTileOrItem()
        {
            if (PlayerManager.RotatingType == "tile")
                tiles[PlayerManager.ActiveTileID].RotateThis();
            else if (PlayerManager.RotatingType == "item")
                items[PlayerManager.ActiveTileID].RotateThis();
        }

        static public void UnlockAnObject()
        {
            if (PlayerManager.UnlockObject)
            {
                for (int x = items.Count - 1; x >= 0; x--)
                    if (items[x].MouseClick && items[x].Lock)
                    {
                        items[x].Lock = false;
                        items[x].Moving = true;
                        PlayerManager.UnlockObject=false;
                        ButtonManager.buttons[4].CodeValue = "Unlock " + PlayerManager.UnlockObject;
                        items[x].Start = items[x].MouseLocation;
                        NewActiveItem(x);
                        break;
                    }
                for (int x = tiles.Count - 1; x >= 0; x--)
                    if (tiles[x].MouseClick && tiles[x].Lock)
                    {
                        tiles[x].Lock = false;
                        tiles[x].Moving = true;
                        PlayerManager.UnlockObject=false;
                        ButtonManager.buttons[4].CodeValue = "Unlock " + PlayerManager.UnlockObject;
                        tiles[x].Start = tiles[x].MouseLocation;
                        NewActiveTile(x);
                        break;
                    }

            }
        }

        #endregion

        #region Update

        public static void Update(GameTime gameTime)  
        {    
            PlayerManager.ResetActiveTile();
        
             if (PlayerManager.ActiveTileType != "tile")
                UpdateItems(gameTime); 

            if (PlayerManager.ActiveTileType != "item")
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

                if (PlayerManager.ActiveTile && PlayerManager.ActiveTileType == "tile")
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

        }

        static public void UpdateItems(GameTime gameTime)
        {
            if (!PlayerManager.ActiveTile)
            {
                for (int x = items.Count - 1; x >= 0; x--)
                {
                        items[x].Update(gameTime);
                    if (items[x].Moving)
                    {
                        NewActiveItem(x);
                        break;

                    }
                }
            }

            if (PlayerManager.ActiveTile && PlayerManager.ActiveTileType == "item")
            {
                items[PlayerManager.ActiveTileID].Update(gameTime);
                if (!items[PlayerManager.ActiveTileID].Moving)
                    PlayerManager.ActiveTile = false;
            }
            else
            {
                for (int x = items.Count - 1; x >= 0; x--)
                {
                    if (x == PlayerManager.ActiveTileID && PlayerManager.ActiveTileType == "item")
                        items[x].ActiveTile = true;
                    else
                        items[x].ActiveTile = false;

                    items[x].Update(gameTime);
                    if (items[x].Moving)
                    {
                        NewActiveItem(x);
                        break;

                    }

                }
            }
            RotateTileOrItem();
            UnlockAnObject();
            
        }

        #endregion

        #region Draw

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
                tile.Draw(spriteBatch);

            foreach (Item item in items)
                item.Draw(spriteBatch);
        }

        #endregion
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TileEngine
{
    public static class TileGrid
    {
        #region Declarations
        static public int TileWidth = 90;
        static public int TileHeight = 90;
        public const int OriginalTileWidth = 90;
        public const int OriginalTileHeight = 90;
        public const int MapWidth = 100;
        public const int MapHeight = 100;
        public const int MapLayers = 3;
        private const int skyTile = 0;
        static public Vector2 MapLocation = new Vector2(10, 10);

        public static bool ShowMap = true;
        static public Tile[,] mapCells =
            new Tile[MapWidth, MapHeight];

    

        public static bool EditorMode = false;

        public static SpriteFont spriteFont;
        static public Texture2D tileSheet;
        static public Texture2D tempTile;

        #endregion

        #region Initialization
        static public void Initialize(Texture2D tileTexture,Texture2D temptile)
        {

            Random rand = new Random();
            tileSheet = tileTexture;
            tempTile = temptile;
 


            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    for (int z = 0; z < MapLayers; z++)
                    {
                        if(x==0 || y==0 || x==MapWidth-1 || y==MapWidth-1)
                            mapCells[x, y] = new Tile(2, 0, 0, "", Vector2.Zero);
                        else
                            mapCells[x, y] = new Tile(1, 0, 0, "",Vector2.Zero);
                    }
                }
            }
        }
        #endregion

        #region Tile and Tile Sheet Handling
        public static int TilesPerRow
        {
            get { return tileSheet.Width / OriginalTileWidth; }
        }

        public static Rectangle TileSourceRectangle(int tileIndex)
        {
            return new Rectangle(
                (tileIndex % TilesPerRow) * OriginalTileWidth,
                (tileIndex / TilesPerRow) * OriginalTileHeight,
                OriginalTileWidth,
                OriginalTileHeight);
        }

        public static Vector2 TileSourceCenter(int tileIndex)
        {
            return new Vector2(
                (tileIndex % TilesPerRow) * OriginalTileWidth + OriginalTileWidth / 2,
                (tileIndex / TilesPerRow) * OriginalTileHeight + OriginalTileHeight / 2);   
        }

        #endregion

        #region Information about Map Cells
        static public int GetCellByPixelX(int pixelX)
        {
            return pixelX / TileWidth;
        }

        static public int GetCellByPixelY(int pixelY)
        {
            return pixelY / TileHeight;
        }

        static public Vector2 GetCellByPixel(Vector2 pixelLocation)
        {
            return new Vector2(
                GetCellByPixelX((int)pixelLocation.X),
                GetCellByPixelY((int)pixelLocation.Y));
        }

        static public Vector2 GetCellCenter(int cellX, int cellY)
        {
            return new Vector2(
                (cellX * TileWidth) + (TileWidth / 2),
                (cellY * TileHeight) + (TileHeight / 2));
        }

        static public Vector2 GetCellCenter(Vector2 cell)
        {
            return GetCellCenter(
                (int)cell.X,
                (int)cell.Y);
        }

        static public Rectangle CellWorldRectangle(int cellX, int cellY)
        {
            return new Rectangle(
                cellX * TileWidth,
                cellY * TileHeight,
                TileWidth,
                TileHeight);
        }

        static public Rectangle CellWorldOriginalRectangle(int cellX, int cellY)
        {
            return new Rectangle(
                cellX * OriginalTileWidth,
                cellY * OriginalTileHeight,
                OriginalTileWidth,
                OriginalTileHeight);
        }

        static public Rectangle MapToScreenRectangle(int cellX, int cellY)
        {
            return new Rectangle(
            cellX * TileWidth / 4 + (int)MapLocation.X,
            cellY * TileHeight / 4 + (int)MapLocation.Y,
            TileWidth / 4,
            TileHeight / 4);
        }

        static public Rectangle CellWorldRectangle(Vector2 cell)
        {
            return CellWorldRectangle(
                (int)cell.X,
                (int)cell.Y);
        }

        static public Rectangle CellScreenRectangle(int cellX, int cellY)
        {
            return Camera.WorldToScreen(CellWorldRectangle(cellX, cellY));
        }

        static public Rectangle CellScreenOriginalRectangle(int cellX, int cellY)
        {
            return Camera.WorldToScreen(CellWorldOriginalRectangle(cellX, cellY));
        }


        static public Rectangle CellSreenRectangle(Vector2 cell)
        {
            return CellScreenRectangle((int)cell.X, (int)cell.Y);
        }

        static public bool CellIsPassable(int cellX, int cellY)
        {
            Tile square = GetMapSquareAtCell(cellX, cellY);

            if (square == null)
                return false;
            else
                return square.Passable;
        }
        
        static public bool CellIsPassable(Vector2 cell)
        {
            return CellIsPassable((int)cell.X, (int)cell.Y);
        }

        static public bool CellIsPassableByPixel(Vector2 pixelLocation)
        {
            return CellIsPassable(
                GetCellByPixelX((int)pixelLocation.X),
                GetCellByPixelY((int)pixelLocation.Y));
        }

        static public string CellCodeValue(int cellX, int cellY)
        {
            Tile square = GetMapSquareAtCell(cellX, cellY);

            if (square == null)
                return "";
            else
                return square.CodeValue;
        }

        static public string CellCodeValue(Vector2 cell)
        {
            return CellCodeValue((int)cell.X, (int)cell.Y);
        }

        static public Vector2 GetCellLocation(Vector2 pixelLocation)
        {
            Vector2 location = GetCellByPixel(pixelLocation);

            return new Vector2(location.X * TileWidth, location.Y * TileHeight);
        }

        #endregion

        #region Information about MapSquare objects
        static public Tile GetMapSquareAtCell(int tileX, int tileY)
        {
            if ((tileX >= 0) && (tileX < MapWidth) &&
                (tileY >= 0) && (tileY < MapHeight))
            {
                return mapCells[tileX, tileY];
            }
            else
            {
                return null;
            }
        }

        static public void SetMapSquareAtCell(
            int tileX,
            int tileY,
            Tile tile)
        {
            if ((tileX >= 0) && (tileX < MapWidth) &&
                (tileY >= 0) && (tileY < MapHeight))
            {
                mapCells[tileX, tileY] = tile;
            }
        }

        static public void SetTileAtCell(
            int tileX,
            int tileY,
            int layer,
            int tileIndex)
        {
            if ((tileX >= 0) && (tileX < MapWidth) &&
                (tileY >= 0) && (tileY < MapHeight))
            {
                mapCells[tileX, tileY].LayerTiles[layer] = tileIndex;
            }
        }

        static public Tile GetMapSquareAtPixel(int pixelX, int pixelY)
        {
            return GetMapSquareAtCell(
                GetCellByPixelX(pixelX),
                GetCellByPixelY(pixelY));
        }

        static public Tile GetMapSquareAtPixel(Vector2 pixelLocation)
        {
            return GetMapSquareAtPixel(
                (int)pixelLocation.X,
                (int)pixelLocation.Y);
        }

        static public bool AvailableForPlacement(int posX,int posY)
        {
            return ((mapCells[posX, posY].LayerTiles[1] == 0)
                       && mapCells[posX, posY].Passable);
        }

        static public Vector2 MouseCenter(Vector2 mousePos)
        {

            return new Vector2(
                 (int)mousePos.X - (int)(TileWidth / 2),
                 (int)mousePos.Y - (int)(TileHeight / 2));

        }

        static public Vector2 PositionInWorldBounds(Vector2 mousePos)
        {

            if (mousePos.Y < (float)TileHeight)
                mousePos.Y = (float)TileHeight;
            else if (mousePos.Y > ((float)TileHeight * (MapHeight - 2)))
                mousePos.Y = (float)TileHeight * (MapHeight - 2);

            if (mousePos.X < (float)TileWidth)
                mousePos.X = (float)TileWidth;
            else if (mousePos.X > ((float)TileWidth * (MapWidth - 2)))
                mousePos.X = (float)TileWidth * (MapWidth - 2);

            return mousePos;
                
        }

        #endregion

        #region Information About Tile Box


        
        static public Rectangle ExactScreenRectangle(Vector2 boxPos)
        {
            return new Rectangle(
            (int)boxPos.X,
            (int)boxPos.Y,
            TileWidth,
            TileHeight);
        }

        static public Rectangle ExactScreenOriginalRectangle(Vector2 boxPos)
        {
            return new Rectangle(
            (int)boxPos.X,
            (int)boxPos.Y,
            OriginalTileWidth,
            OriginalTileHeight);
        }
        #endregion

        #region Loading and Saving Maps
        public static void SaveMap(FileStream fileStream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, mapCells);
            fileStream.Close();
        }

        public static void LoadMap(FileStream fileStream)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                mapCells = (Tile[,])formatter.Deserialize(fileStream);
                fileStream.Close();
            }
            catch
            {
                ClearMap();
            }
        }

        public static void ClearMap()
        {
            for (int x = 0; x < MapWidth; x++)
                for (int y = 0; y < MapHeight; y++)
                    for (int z = 0; z < MapLayers; z++)
                    {
                        mapCells[x, y] = new Tile(skyTile, 0, 0, "",Vector2.Zero);
                    }
        }
        #endregion

        #region Drawing
        static public void Draw(SpriteBatch spriteBatch)
        {
            int startX = GetCellByPixelX((int)Camera.Position.X);
            int endX = GetCellByPixelX((int)Camera.Position.X +
                  Camera.ViewPortWidth);

            int startY = GetCellByPixelY((int)Camera.Position.Y);
            int endY = GetCellByPixelY((int)Camera.Position.Y +
                      Camera.ViewPortHeight);


            for (int x = startX; x <= endX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    for (int z = 0; z < MapLayers; z++)
                    {
                        if ((x >= 0) && (y >= 0) &&
                            (x < MapWidth) && (y < MapHeight))
                        {
                            spriteBatch.Draw(
                              tileSheet,
                              CellScreenRectangle(x, y),
                              TileSourceRectangle(mapCells[x, y].LayerTiles[z]),
                              Color.White,
                              mapCells[x,y].Rotation,
                              Vector2.Zero,
                              SpriteEffects.None,
                              1f - ((float)z * 0.1f));
                        }
                    }

                 

                    if (EditorMode)
                    {
                        DrawEditModeItems(spriteBatch, x, y);
                    }

                    if (ShowMap)
                    {
                        //  DrawMapOnScreen(spriteBatch, MapLocation);
                    }

                }
        }



        public static void DrawMapOnScreen(SpriteBatch spriteBatch, Vector2 MapLocation)
        {

            for (int x = 0; x <= MapWidth; x++)
                for (int y = 0; y <= MapHeight; y++)
                {
                    for (int z = MapLayers - 1; z < MapLayers; z++)
                    {
                        if ((x >= 0) && (y >= 0) &&
                            (x < MapWidth) && (y < MapHeight))
                        {
                            spriteBatch.Draw(
                              tileSheet,
                              MapToScreenRectangle(x, y),
                              TileSourceRectangle(mapCells[x, y].LayerTiles[z]),
                              Color.White,
                              0.0f,
                              Vector2.Zero,
                              SpriteEffects.None,
                              0.0f);
                        }
                    }
                }
        }
        public static void DrawEditModeItems(
            SpriteBatch spriteBatch,
            int x,
            int y)
        {
            if ((x < 0) || (x >= MapWidth) ||
                (y < 0) || (y >= MapHeight))
                return;


            if (mapCells[x, y].CodeValue != "")
            {
                Rectangle screenRect = CellScreenRectangle(x, y);

                spriteBatch.DrawString(
                    spriteFont,
                    mapCells[x, y].CodeValue,
                    new Vector2(screenRect.X, screenRect.Y),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    0.0f);
            }
        }
        #endregion

    }
}

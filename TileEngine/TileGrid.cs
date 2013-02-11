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
        static public int TileWidth = 51;
        static public int TileHeight = 51;
        public const int OriginalTileWidth = 51;
        public const int OriginalTileHeight = 51;
        public const int MapWidth = 150;
        public const int MapHeight = 150;
        public const int MapLayers = 3;
        private const int skyTile = 0;
        static public Vector2 MapLocation = new Vector2(10, 10);

        public static bool ShowMap = true;
        static private Tile[,] mapCells =
            new Tile[MapWidth, MapHeight];

        public static bool showRandomTile = false;
        public static List<Tile> BoxTiles = new List<Tile>();
        public static Tile TileSpawner;
        public static Tile commitTile;
        public static int[] CommitPos=new int[2];

        public static bool mouseOverSpawner = false;
        public static RotatingTile rotation;
        public static bool rotating = false;
        public static bool readyToCommit = false;
        public static bool readyToDrag = false;

        public static bool EditorMode = false;

        public static SpriteFont spriteFont;
        static private Texture2D tileSheet;
        static private SpriteFont pericles10;
        #endregion

        #region Initialization
        static public void Initialize(Texture2D tileTexture, SpriteFont pericles)
        {

            Random rand = new Random();
            tileSheet = tileTexture;
            pericles10 = pericles;
 
            TileSpawner = new Tile(0,0,3,"",new Vector2(1400,30));
            commitTile = new Tile(0, 0, 2, "", new Vector2(1400, 90));

            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    for (int z = 0; z < MapLayers; z++)
                    {
                        if (rand.Next(0,30) ==0)
                            mapCells[x, y] = new Tile(1, 0, 0, "",Vector2.Zero);
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

        #endregion

        #region Information About Tile Box

        static public bool ShowRandomTile
        {
            get { return showRandomTile; }
            set { showRandomTile = value; }
        }

        static public Rectangle ExactScreenRectangle(Vector2 boxPos)
        {
            return new Rectangle(
            (int)boxPos.X,
            (int)boxPos.Y,
            TileWidth,
            TileHeight);
        }

        static public void AddBoxTile(int ID,string code,Vector2 mousePosition)
        {
            BoxTiles.Add(new Tile(0, 0, ID, code, mousePosition));
        }

        static public bool NewTileAvailable()
        {
            return showRandomTile;
        }

        static public bool CanGenerateTile(Vector2 mousePos)
        {
            return (ExactScreenRectangle(mousePos).Intersects
                 (ExactScreenRectangle(TileSpawner.position))
                 && !showRandomTile);
                
        }
        static public void rotatePiece(string ID)
        {
            if (!rotating)
            {
                foreach (Tile tile in BoxTiles)
                    if (tile.checkID(ID))
                    {
                        rotation = new RotatingTile(true, tile.rotation);
                        rotating = true;
                    }
            }
        }

        static public Vector2 MouseCenter(Vector2 mousePos)
        {
            
             return new Vector2(
                  (int)mousePos.X - (int)(TileWidth / 2),
                  (int)mousePos.Y - (int)(TileHeight / 2));
            
        }

        static public void updateTilePosition(Vector2 mousePos, string ID)
        {
            foreach(Tile tile in BoxTiles)
                if (tile.checkID(ID))
                {
                    tile.position = MouseCenter(mousePos);

                }
        }

        static public bool ReadyToCommit
        {
            get { return readyToCommit; }
            set { readyToCommit = value; }
        }

        static public bool ReadyToDrag
        {
            get { return readyToDrag; }
            set { readyToDrag = value; }
        }

        static public void MouseOverTileGenerator(Vector2 mousePos)
        {
            if (new Rectangle((int)mousePos.X,(int)mousePos.Y,1,1).Intersects
                  (ExactScreenRectangle(TileSpawner.position))
                  && !showRandomTile)
                mouseOverSpawner = true;
            else
                mouseOverSpawner = false;
        }

        static public void AdjustTileLocation(string ID)
        {
            foreach (Tile tile in BoxTiles)
                if (tile.checkID(ID))
                    tile.position = new Vector2(CommitPos[0] * TileWidth, CommitPos[1] * TileWidth);
        }
        static public void PlaceTile(Vector2 mousePos, string ID)
        {
            mousePos += Camera.Position;
            for (int x = BoxTiles.Count - 1; x >= 0; x--)

                if (BoxTiles[x].checkID(ID))
                {
                   
                
                     if ((mapCells[GetCellByPixelX((int)mousePos.X), GetCellByPixelY((int)mousePos.Y)].LayerTiles[1]) == 0)
                     {
                         BoxTiles[x].position = GetCellLocation(mousePos);
                         CommitPos[0] =GetCellByPixelX((int)mousePos.X);
                         CommitPos[1]=GetCellByPixelY((int)mousePos.Y);
                        // mapCells[CommitPos[0], CommitPos[1]].LayerTiles[1] = BoxTiles[x].LayerTiles[2];
                       //  mapCells[CommitPos[0], CommitPos[1]].rotation = BoxTiles[x].rotation;
                         ReadyToCommit = true;
                         
                     }
                     ReadyToDrag = true;
                  
                }
          //  ShowRandomTile = false;

        }

        static public void CommitTile(Vector2 mousePos,string ID)
        {
            for (int x = BoxTiles.Count - 1; x >= 0; x--)

                if (BoxTiles[x].checkID(ID))
                {
                    if (ReadyToCommit)
                    {
                       if (new Rectangle((int)mousePos.X,(int)mousePos.Y,1,1).Intersects
                         (ExactScreenRectangle(commitTile.position)))
                       {
                           mapCells[CommitPos[0], CommitPos[1]].LayerTiles[1] = BoxTiles[x].LayerTiles[2];
                           mapCells[CommitPos[0], CommitPos[1]].rotation = BoxTiles[x].rotation;
                         //mapCells[CommitPos[0], CommitPos[1]].CodeValue = ID;
                           mapCells[CommitPos[0], CommitPos[1]].Passable = false;
                        BoxTiles.RemoveAt(x);
                        ShowRandomTile = false;
                        ReadyToCommit = false;
                        ReadyToDrag = false;
                  
                       }
                    }
                }

        }


        static public bool CheckTileMouseIntersection(Vector2 mousePos, string ID)
        {
            for (int x = BoxTiles.Count - 1; x >= 0; x--)

                if (BoxTiles[x].checkID(ID))
                {
                    if (new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1).Intersects
                        (ExactScreenRectangle(BoxTiles[x].position)))
                        return true;
                }
            return false;

        }


        static public Vector2 GetCellLocation(Vector2 pixelLocation)
        {
            Vector2 location = GetCellByPixel(pixelLocation);

            return new Vector2(location.X * TileWidth, location.Y * TileHeight);
        }

        static public float SpawnerTransparency()
        {
            if (showRandomTile)
                return 0.1f;
            else if (!mouseOverSpawner)
                return 0.7f;
            else
                return 1f;
        }

        static public float CommitTransparency()
        {
            if (ReadyToCommit)
                return 1f;
            else
                return 0.1f;
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

            DrawRandomTile(spriteBatch);

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

        public static void DrawRandomTile(SpriteBatch spriteBatch)
        {
            if (ShowRandomTile)
            {
                foreach (Tile tile in BoxTiles)
                {
                    for (int i = 0; i < MapLayers; i++)
                    {
                  
                    spriteBatch.Draw(
                      tileSheet,
                      ExactScreenRectangle(Camera.WorldToScreen(tile.position)),
                      TileSourceRectangle(tile.LayerTiles[i]),
                      Color.White,
                      tile.Rotation,
                      Vector2.Zero,
                      SpriteEffects.None,
                      1f - ((float)i * 0.1f));
                    }
                    spriteBatch.DrawString(
                    pericles10,
                    tile.CodeValue,
                    Camera.WorldToScreen(tile.position),
                    Color.White);
                }


            }

            spriteBatch.Draw(
                tileSheet,
                ExactScreenRectangle(TileSpawner.position),
                TileSourceRectangle(TileSpawner.LayerTiles[2]),
                Color.White * SpawnerTransparency(),
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                1f - ((float)2 * 0.1f));

            spriteBatch.Draw(
            tileSheet,
            ExactScreenRectangle(commitTile.position),
            TileSourceRectangle(commitTile.LayerTiles[2]),
            Color.White * CommitTransparency(),
             0.0f,
             Vector2.Zero,
            SpriteEffects.None,
            1f - ((float)2 * 0.1f));

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

﻿using System;
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

namespace TileEngine.Entity
{
    using TileEngine.Camera;
    using MultiplayerGame.Networking;
    using MultiplayerGame.Args;
    using MultiplayerGame.Networking.Messages;


    public static class TileGrid
    {
        #region Declarations
        static public int TileWidth = 90;
        static public int TileHeight = 90;
        public const int OriginalTileWidth = 90;
        public const int OriginalTileHeight = 90;
        public const int MapWidth = 100;
        public const int MapHeight = 100;
        static public string PlayerID = "";

        static public INetworkManager networkManager;
        static public Square[,] mapCells =
            new Square[MapWidth, MapHeight];

        #endregion

        public static event EventHandler<TileStateChangedArgs> SnapTileToGrid;
        public static event EventHandler<TileStateChangedArgs> RemoveFromGrid;
        public static event EventHandler<RotationValueChangedArgs> RotateValue;
        public static event EventHandler<TemplateArgs> UpdateTemplate;

        #region Initialization
        static public void Initialize(ContentManager Content,string playerID)
        {
            Texture2D border = Content.Load<Texture2D>(@"Textures\Table");
            Texture2D square = Content.Load<Texture2D>(@"Textures\MapSquare");

            PlayerID = playerID;

            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    if (x == 0 || y == 0 || x == MapWidth - 1 || y == MapWidth - 1) 
                        mapCells[x, y] = new Square("border",border, new Vector2(x * TileWidth, y * TileHeight));
                    else
                        mapCells[x, y] = new Square("", square, new Vector2(x * TileWidth, y * TileHeight));

                }
            }
            SnapTileToGrid += (sender, e) => TileGrid.networkManager.SendMessage(new SnapToGridMessage(e.tile, e.playerID,e.scale));
            RemoveFromGrid += (sender, e) => TileGrid.networkManager.SendMessage(new RemoveFromGridMessage(e.tile,e.playerID, e.scale));
            RotateValue += (sender, e) => TileGrid.networkManager.SendMessage(new RotationMessage(e.rotationValue,e.ID,e.playerID,e.type));
            UpdateTemplate += (sender, e) => TileGrid.networkManager.SendMessage(new UpdateTemplateMessage(e.Name, e.Pos, e.Sender));
        }

        #endregion

        #region Static Events

        public static void OnUpdateTemplate(string id,int score,string player)
        {
            EventHandler<TemplateArgs> updateTemplate = UpdateTemplate;
            if (updateTemplate != null)
                updateTemplate(updateTemplate, new TemplateArgs(id,score,player));
        }

        public static void OnSnapToGrid(Tile tile,string playerID)
        {
            EventHandler<TileStateChangedArgs> snapToGrid = SnapTileToGrid;
            if (snapToGrid != null)
                snapToGrid(snapToGrid, new TileStateChangedArgs(tile,playerID, TileGrid.TileWidth));
        }

        public static void OnRemoveFromGrid(Tile tile,string playerID)
        {
            EventHandler<TileStateChangedArgs> removeFromGrid = RemoveFromGrid;
            if (removeFromGrid != null)
                removeFromGrid(removeFromGrid, new TileStateChangedArgs(tile, playerID, TileGrid.TileWidth));
        }

        public static void OnRotation(float rotationAmount,string playerID,int ID,string type)
        {
            EventHandler<RotationValueChangedArgs> rotateValue = RotateValue;
            if (rotateValue != null)
                rotateValue(rotateValue, new RotationValueChangedArgs(rotationAmount, ID, playerID, type));
        }

        #endregion

        #region Tile and Tile Sheet Handling

        public static Vector2 mapCenter
        {
            get { return new Vector2(MapWidth / 2 * TileWidth, MapHeight / 2 * TileHeight); }
        }

        public static Rectangle TileSourceRectangle(int tileIndex)
        {
            return new Rectangle(
                0,
                0,
                OriginalTileWidth,
                OriginalTileHeight);
        }

        public static Vector2 TileSourceCenter(int tileIndex)
        {
            return new Vector2(
                OriginalTileWidth / 2,
                OriginalTileHeight / 2);   
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
        

        static public Vector2 GetCellLocation(Vector2 pixelLocation)
        {
            Vector2 location = GetCellByPixel(pixelLocation);

            return new Vector2(location.X * TileWidth, location.Y * TileHeight);
        }

        #endregion

        #region Information about MapSquare objects

  
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

            }
        }

     
        #endregion

        #region Draw

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
                     {
                        if ((x >= 0) && (y >= 0) &&
                            (x < MapWidth) && (y < MapHeight))
                        {
                            Color color = Color.White;
                            if (mapCells[x, y].CodeValue != "")
                                color = Color.Red;
                            spriteBatch.Draw(
                                  mapCells[x,y].texture,
                                  CellScreenRectangle(x, y),
                                  TileSourceRectangle(0),
                                  color,
                                  0.0f,
                                  Vector2.Zero,
                                  SpriteEffects.None,
                                  1.0f);

                       }
                    }
                }
                 
           
        }
               
     
        #endregion

    }
}

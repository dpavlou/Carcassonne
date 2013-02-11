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
    static class TileManager
    {
        static private Random rand = new Random();
        static private bool RightReleased = true;
        static public void GenerateNewTile(Vector2 mousePos,string ID)
        {
            if (TileGrid.CanGenerateTile(mousePos))
            {
                TileGrid.AddBoxTile(rand.Next(2, TileGrid.TilesPerRow), ID, mousePos);
                TileGrid.ShowRandomTile = true;
            }
        }


        static public void Update(GameTime gametime, Vector2 worldLocation,string ID)
        {
            var mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed
                && TileGrid.ReadyToDrag)
            {
                if (TileGrid.CheckTileMouseIntersection(new Vector2(mouseState.X, mouseState.Y) + worldLocation, ID))
                {
                    TileGrid.updateTilePosition(new Vector2(mouseState.X, mouseState.Y) + worldLocation, ID);
                    TileGrid.ReadyToDrag = false;
                    TileGrid.ReadyToCommit = false;
                }
            }

            //Rotation Temp code
            /*
            if (mouseState.LeftButton == ButtonState.Pressed
                && TileGrid.ReadyToCommit)
            {
                if (TileGrid.CheckTileMouseIntersection(new Vector2(mouseState.X, mouseState.Y)
                                                        + worldLocation, ID))
                    //TileGrid.rotatePiece(ID);
            }
            */

            if (mouseState.LeftButton == ButtonState.Pressed
                && !TileGrid.ReadyToDrag)
            {
                TileManager.GenerateNewTile(new Vector2(mouseState.X, mouseState.Y), ID);
                TileGrid.updateTilePosition(new Vector2(mouseState.X, mouseState.Y)+worldLocation, ID);
                RightReleased = false;
            }
            if (mouseState.LeftButton != ButtonState.Pressed 
                && TileGrid.ShowRandomTile
                && !RightReleased)
            {
                RightReleased = true;
                TileGrid.PlaceTile(new Vector2(mouseState.X, mouseState.Y), ID);
               
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
                TileGrid.CommitTile(new Vector2(mouseState.X, mouseState.Y), ID);

            TileGrid.MouseOverTileGenerator(new Vector2(mouseState.X, mouseState.Y));
        }

    }
}

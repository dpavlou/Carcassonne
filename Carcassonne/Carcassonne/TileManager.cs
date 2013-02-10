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

        static public void GenerateNewTile(Vector2 mousePos,string ID)
        {
            if (TileGrid.CanGenerateTile(mousePos))
            {
                TileGrid.AddBoxTile(rand.Next(2, TileGrid.TilesPerRow), ID, mousePos);
                TileGrid.ShowRandomTile = true;
            }
        }


        static public void Update(GameTime gametime, string ID)
        {
            var mouseState = Mouse.GetState();

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                TileManager.GenerateNewTile(new Vector2(mouseState.X, mouseState.Y), ID);
                TileGrid.updateTilePosition(new Vector2(mouseState.X, mouseState.Y), ID);
            }
            if (mouseState.RightButton != ButtonState.Pressed 
                && TileGrid.ShowRandomTile)
            {
                TileGrid.PlaceTile(new Vector2(mouseState.X, mouseState.Y), ID);
               
            }
            TileGrid.MouseOverTileGenerator(new Vector2(mouseState.X, mouseState.Y));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileEngine;

namespace Carcassonne
{
    public static class ButtonManager
    {

        #region Declarations

        public static Button keylocker;
        public static List<Button> buttons = new List<Button>();
        private static bool keyAdjustment;
        private static Form menu;

        #endregion

        #region Initialization

        public static void Initialize(Texture2D button1,Texture2D button2,Texture2D scoreBoard,SpriteFont font,string owner)
        {
            buttons.Add(new Button("Generator", new Vector2(-45, -15), button1, font, new Vector2((float)Camera.ViewPortWidth / 2 - 100, 60), 1, 0.1f, false));
            buttons.Add(new Button("Lock", new Vector2(-20, -15), button1, font, new Vector2((float)Camera.ViewPortWidth / 2, 60), 1, 0.1f, false));
            buttons.Add(new Button("Add Player", new Vector2(-45, -15), button2, font, new Vector2(Camera.ViewPortWidth + 100, 210), 1, 0.05f, true));
            keylocker = new Button("KeyLocker", new Vector2(-45, -15), button2, font, new Vector2(Camera.ViewPortWidth + 100, 100), 1, 0.05f, true);
            menu = new Form(button1, scoreBoard,new Vector2(600, Camera.ViewPortHeight), font, new Vector2(Camera.ViewPortWidth, 0));
            keyAdjustment = false;

            keylocker.BoundSize = buttons[2].BoundSize = menu.FormSize;

            AdjustFormButtonLocation();
        }

        #endregion

        #region Helper Methods

        public static void AdjustFormButtonLocation()
        {
            
            keylocker.Bounds = buttons[2].Bounds = menu.Location+new Vector2(TileGrid.OriginalTileWidth/2,0);

            buttons[2].Move(menu.Step);
            keylocker.Move(menu.Step);
            menu.PreviousLocation = menu.Location;
          
        }
        
        public static void ToggleKeyAdjustment()
        {
            keyAdjustment = !keyAdjustment;
        }

        public static void HandleKeyLocker()
        {

            if (keylocker.OnMouseClick())
            {
                ToggleKeyAdjustment();
                TileManager.LockAllTiles();
            }
            foreach (Button button in buttons)
                 button.Lock = !keyAdjustment;
        }

        #endregion

        #region Update

        public static void Update(GameTime gameTime)
        {

            if (PlayerManager.ActiveTile && PlayerManager.ActiveTileType == "button")
            {
                buttons[PlayerManager.ActiveTileID].Update(gameTime);
                if (!buttons[PlayerManager.ActiveTileID].Moving)
                    PlayerManager.ActiveTile = false;
            }
            else if (PlayerManager.ActiveTile && PlayerManager.ActiveTileType == "tile")
            {
                ;
            }
            else
            {
                for (int x = buttons.Count - 1; x >= 0; x--)
                {
                    if (x == PlayerManager.ActiveTileID && PlayerManager.ActiveTileType == "button")
                        buttons[x].ActiveTile = true;
                    else
                        buttons[x].ActiveTile = false;


                    buttons[x].Update(gameTime);

                    if (buttons[x].Moving)
                    {
                        if (PlayerManager.ActiveTileType == "button")
                        buttons[PlayerManager.ActiveTileID].ActiveTile = false;
                        PlayerManager.ActiveTileType = "button";
                        buttons[x].ActiveTile = true;
                        PlayerManager.ActiveTileID = x;
                        PlayerManager.ActiveTile = true;
                    }

                }
            }

            keylocker.Update(gameTime);
            menu.Update(gameTime);
            AdjustFormButtonLocation();

            if (buttons[0].OnMouseClick())
                TileManager.AddSoldier(Camera.Position+(buttons[0].Location-new Vector2(TileGrid.OriginalTileWidth,0)),PlayerManager.PlayerTurn);

            if(buttons[1].OnMouseClick())
                TileManager.LockTiles();

            HandleKeyLocker();

        }

        #endregion

        #region Draw

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in buttons)
                button.Draw(spriteBatch);

            keylocker.Draw(spriteBatch);
            menu.Draw(spriteBatch);
        }

        #endregion

    }

}

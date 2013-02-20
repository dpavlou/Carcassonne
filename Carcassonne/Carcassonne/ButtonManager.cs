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

        #endregion

        #region Initialization

        public static void Initialize(Texture2D texture,SpriteFont font,string owner)
        {
            buttons.Add( new Button("Generator", new Vector2(-45, -15), texture, font, new Vector2((float)Camera.ViewPortWidth - 110, 70), 1,0.1f,owner));
            buttons.Add( new Button("Lock", new Vector2(-20, -15), texture, font, new Vector2((float)Camera.ViewPortWidth - 110, 190), 1, 0.1f, owner));
            keylocker = new Button("KeyLocker", new Vector2(-45, -15), texture, font, new Vector2(60,160), 1, 0.1f, owner);
            keyAdjustment = false;
        }

        #endregion

        #region Helper Methods

        
        public static void ToggleKeyAdjustment()
        {
            keyAdjustment = !keyAdjustment;
        }

        public static void HandleKeyLocker()
        {

            if (keylocker.OnMouseClick())
            {
                ToggleKeyAdjustment();
                TileManager.LockTiles();
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

            if (buttons[0].OnMouseClick())
                TileManager.AddTile(Camera.Position+(buttons[0].Location-new Vector2(TileGrid.OriginalTileWidth,0)),PlayerManager.PlayerTurn);

            if(buttons[1].OnMouseClick())
                TileManager.LockTiles(buttons[1].Owner);

            HandleKeyLocker();

        }

        #endregion

        #region Draw

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in buttons)
                button.Draw(spriteBatch);

            keylocker.Draw(spriteBatch);
        }

        #endregion

    }

}

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

        public static void Initialize(Texture2D button1,Texture2D button2,SpriteFont font,string owner)
        {
            buttons.Add(new Button("Tiles", new Vector2(-25, -15), button1, font, new Vector2((float)Camera.ViewPortWidth / 2 - 100, 60), 1, 0.1f, false));
            buttons.Add(new Button("Lock", new Vector2(-20, -15), button1, font, new Vector2((float)Camera.ViewPortWidth / 2 + 100, 60), 1, 0.1f, false));
            buttons.Add(new Button("Add Player", new Vector2(-45, -15), button2, font, new Vector2(Camera.ViewPortWidth + 100, 210), 1, 0.09f, true));
            buttons.Add(new Button("Soldier", new Vector2(-35, -15), button1, font, new Vector2((Camera.ViewPortWidth /2), 60), 1, 0.05f, false));
            buttons.Add(new Button("Unlock False", new Vector2(-50, -15), button1, font, new Vector2((Camera.ViewPortWidth / 2 + 200), 60), 1, 0.05f, false));
            keylocker = new Button("KeyLocker", new Vector2(-45, -15), button2, font, new Vector2(Camera.ViewPortWidth + 100, 100), 1, 0.09f, true);
            
            keyAdjustment = false;

            keylocker.BoundSize = buttons[2].BoundSize = FormManager.menu.FormSize;

            AdjustFormButtonLocation();
        }

        #endregion

        #region Helper Methods

        public static void AdjustFormButtonLocation()
        {

            keylocker.Bounds = buttons[2].Bounds = FormManager.menu.Location + new Vector2(TileGrid.OriginalTileWidth / 2, 0);

            buttons[2].Move(FormManager.menu.Step);
            keylocker.Move(FormManager.menu.Step);
            FormManager.menu.PreviousLocation = FormManager.menu.Location;
          
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
             //   TileManager.LockAllTiles();
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
                        PlayerManager.RotatingType = "";
                        buttons[x].ActiveTile = true;
                        PlayerManager.ActiveTileID = x;
                        PlayerManager.ActiveTile = true;
                    }

                }
            }

            keylocker.Update(gameTime);

            AdjustFormButtonLocation();

            if (buttons[0].OnMouseClick())
                TileManager.AddTile(Camera.Position+(buttons[0].Location-new Vector2(0,-TileGrid.OriginalTileHeight)),PlayerManager.PlayerTurn);

            if(buttons[1].OnMouseClick())
                TileManager.LockTiles();

            if (buttons[2].OnMouseClick())
            { ;//add player
            }

            if (buttons[3].OnMouseClick())
                TileManager.AddSoldier(Camera.Position + (buttons[3].Location-new Vector2(2,-TileGrid.OriginalTileHeight+10)), PlayerManager.PlayerTurn);

            if (buttons[4].OnMouseClick())
            {            
                PlayerManager.UnlockObject = !PlayerManager.UnlockObject;
                buttons[4].CodeValue = "Unlock " + PlayerManager.UnlockObject;
            }
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

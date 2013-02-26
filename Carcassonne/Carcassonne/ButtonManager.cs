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

       // public static Button keylocker;
        public static List<Button> buttons = new List<Button>();
        private static bool keyAdjustment;
        private static Texture2D buttonTexture;
        private static SpriteFont Font;
        private static MouseState prevMouse, currMouse;

        #endregion

        #region Initialization

        public static void Initialize(Texture2D button1,Texture2D button2,SpriteFont font,string owner)
        {
            buttons.Add(new Button("Tiles", new Vector2(-25, -15), button2, font, new Vector2(200, 100), 1, 0.1f, true,Color.Blue));
            buttons.Add(new Button("Soldier", new Vector2(-35, -15), button2, font, new Vector2(80, 100), 1, 0.05f, true, Color.Blue));
            buttons.Add(new Button("Lock", new Vector2(-20, -15), button2, font, new Vector2(Camera.ViewPortWidth + 110, 210), 1, 0.1f, true, Color.Blue));
            buttons.Add(new Button("Add Player", new Vector2(-48, -15), button2, font, new Vector2(Camera.ViewPortWidth + 470, 80), 1, 0.09f, true, Color.Blue));
            buttons.Add(new Button("Unlock False", new Vector2(-50, -15), button2, font, new Vector2((Camera.ViewPortWidth + 220), 210), 1, 0.05f, true, Color.Blue));
            //keylocker = new Button("KeyLocker", new Vector2(-45, -15), button2, font, new Vector2(Camera.ViewPortWidth + 100, 100), 1, 0.09f, true, Color.Black);
            Font = font;
            buttonTexture = button1;
            
            keyAdjustment = false;

            for(int i=2;i<5;i++)
                buttons[i].BoundSize = FormManager.menu.FormSize;

            for(int i=0;i<2;i++)
                buttons[i].BoundSize = FormManager.privateSpace.FormSize;
          //  keylocker.BoundSize = 
             

            AdjustFormButtonLocation();

            AddPlayer("Kokos");
            currMouse = Mouse.GetState();
        }

        #endregion

        #region Helper Methods

        public static void AddPlayer(string player)
        {

            buttons.Add(new Button(player, new Vector2(-30, -15), buttonTexture, Font, PlayerManager.NewPlayerLocation+new Vector2(10,0), 1, 0.095f, true, PlayerManager.PlayerColor(PlayerManager.activePlayers)));

            PlayerManager.AddPlayer(player);
            buttons[buttons.Count - 1].BoundSize = FormManager.menu.FormSize;
            AdjustFormButtonLocation();

        }

        public static void AdjustFormButtonLocation()
        {

           // keylocker.Bounds =           
           // keylocker.Move(FormManager.menu.Step);

            AdjustButtonLocation();
            TileManager.AdjustToMenu();
            FormManager.menu.PreviousLocation = FormManager.menu.Location;
            FormManager.privateSpace.PreviousLocation = FormManager.privateSpace.Location;   
        }

        public static void AdjustButtonLocation()
        {
            for (int x = buttons.Count - 1; x >= 2; x--)
            {

                buttons[x].Bounds = FormManager.menu.Location + new Vector2(TileGrid.OriginalTileWidth / 2, 0);
                buttons[x].Move(FormManager.menu.Step);
            }
            for (int x = 0; x < 2; x++)
            {
                buttons[x].Bounds = FormManager.privateSpace.Location + new Vector2(TileGrid.OriginalTileWidth / 2, 0);
                buttons[x].Move(FormManager.privateSpace.Step);
            }
        }

        public static void ToggleKeyAdjustment()
        {
            keyAdjustment = !keyAdjustment;
        }

      /*  public static void HandleKeyLocker()
        {

            if (keylocker.OnMouseClick())
            {
                ToggleKeyAdjustment();
             //   TileManager.LockAllTiles();
            }
            for (int x = 0; x <= 4; x++)
                 buttons[x].Lock = !keyAdjustment;
        }*/

        public static void PlayerButtons()
        {
            for (int x = buttons.Count - 1; x >= 5; x--)
            {
                if (buttons[x].OnMouseClick())
                {
                    PlayerManager.PlayerTurn = buttons[x].CodeValue;
                    PlayerManager.ActivePlayerID = x - 4;
                }
            }
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

            //keylocker.Update(gameTime);

            AdjustFormButtonLocation();

             currMouse  = Mouse.GetState();

                if (buttons[0].OnMouseClick())
                    TileManager.AddTile(Camera.Position + (buttons[0].Location - new Vector2(0, -TileGrid.OriginalTileHeight)), PlayerManager.PlayerTurn);

                if (buttons[1].OnMouseClick())
                    TileManager.AddSoldier(Camera.Position + (buttons[1].Location - new Vector2(2, -TileGrid.OriginalTileHeight + 10)), PlayerManager.PlayerTurn);

                if (buttons[2].OnMouseClick() || (currMouse.MiddleButton==ButtonState.Pressed && prevMouse.MiddleButton!=ButtonState.Pressed))
                    TileManager.LockTiles();

                if (buttons[3].OnMouseClick())
                {
                    if (PlayerManager.activePlayers < 7)
                    {
                        AddPlayer("Kokos" + PlayerManager.activePlayers);
                        TileManager.AddScoreBoardSoldier("Kokos" + (PlayerManager.activePlayers - 1));
                    }
                }

                if (buttons[4].OnMouseClick())
                {
                    PlayerManager.UnlockObject = !PlayerManager.UnlockObject;
                    buttons[4].CodeValue = "Unlock " + PlayerManager.UnlockObject;
                }
            
            PlayerButtons();
            prevMouse = currMouse;
           // HandleKeyLocker();

        }

        #endregion

        #region Draw

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in buttons)
                button.Draw(spriteBatch);

           // keylocker.Draw(spriteBatch);

        }

        #endregion

    }

}

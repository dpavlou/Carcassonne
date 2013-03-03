using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using TileEngine.Entity;

namespace Carcassonne
{
    using TileEngine.Camera;
    using TileEngine.Form;

    public class ButtonManager
    {

        #region Declarations
        
        public  List<Button> buttons = new List<Button>();
        private  bool keyAdjustment;
        private  Texture2D buttonTexture;
        private  SpriteFont Font;
        private  MouseState prevMouse, currMouse;
        private readonly PlayerInformation player;
        private readonly TileManager tileManager;
        #endregion

        #region Constructor

        public ButtonManager(PlayerInformation player,TileManager tileManager)
        {
            this.player = player;
            this.tileManager = tileManager;
        }

        #endregion

        #region Initialization

        public  void Initialize(ContentManager Content,string owner)
        {
            Texture2D button2= Content.Load<Texture2D>(@"Textures\Button2");
            Font = Content.Load<SpriteFont>(@"Fonts\pericles10");
            buttonTexture = Content.Load<Texture2D>(@"Textures\Button");

            buttons.Add(new Button("Tiles", new Vector2(-25, -15), button2, Font, new Vector2(200, 100), 1, 0.1f, true,Color.Blue));
            buttons.Add(new Button("Soldier", new Vector2(-35, -15), button2, Font, new Vector2(80, 100), 1, 0.05f, true, Color.Blue));
            buttons.Add(new Button("Lock", new Vector2(-20, -15), button2, Font, new Vector2(Camera.ViewPortWidth + 110, 210), 1, 0.1f, true, Color.Blue));
            buttons.Add(new Button("Add Player", new Vector2(-48, -15), button2, Font, new Vector2(Camera.ViewPortWidth + 470, 80), 1, 0.09f, true, Color.Blue));
            buttons.Add(new Button("Unlock False", new Vector2(-50, -15), button2, Font, new Vector2((Camera.ViewPortWidth + 220), 210), 1, 0.05f, true, Color.Blue));
                                    
            keyAdjustment = false;

            for(int i=2;i<5;i++)
                buttons[i].BoundSize = FormManager.menu.FormSize;

            for(int i=0;i<2;i++)
                buttons[i].BoundSize = FormManager.privateSpace.FormSize;
            
            AdjustFormButtonLocation();

            AddPlayer("Kokos",1);
            currMouse = Mouse.GetState();
        }

        #endregion

        #region Helper Methods

        public  void AddPlayer(string player,int colorID)
        {

            buttons.Add(new Button(player, new Vector2(-30, -15), buttonTexture, Font, this.player.NewPlayerLocation+new Vector2(10,0), 1, 0.095f, true, this.player.PlayerColor(colorID)));

            this.player.AddPlayer(player);
            buttons[buttons.Count - 1].BoundSize = FormManager.menu.FormSize;
            AdjustFormButtonLocation();

        }

        public  void AdjustFormButtonLocation()
        {

           // keylocker.Bounds =           
           // keylocker.Move(FormManager.menu.Step);

            AdjustButtonLocation();
            tileManager.AdjustToMenu();
            FormManager.menu.PreviousLocation = FormManager.menu.Location;
            FormManager.privateSpace.PreviousLocation = FormManager.privateSpace.Location;   
        }

        public  void AdjustButtonLocation()
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

        public  void ToggleKeyAdjustment()
        {
            keyAdjustment = !keyAdjustment;
        }

      /*  public  void HandleKeyLocker()
        {

            if (keylocker.OnMouseClick())
            {
                ToggleKeyAdjustment();
             //   tileManager.LockAllTiles();
            }
            for (int x = 0; x <= 4; x++)
                 buttons[x].Lock = !keyAdjustment;
        }*/

        public  void PlayerButtons()
        {
            for (int x = buttons.Count - 1; x >= 5; x--)
            {
                if (buttons[x].OnMouseClick())
                {
                  //  player.PlayerTurn = buttons[x].CodeValue;
                    player.ActivePlayerID = x - 4;
                }
            }
        }

        public Vector2 scoreboardItemLocation()
        {
            return (Camera.WorldLocation + buttons[3 + player.activePlayers].Location + new Vector2(0, +TileGrid.OriginalTileHeight));
        }

        #endregion

        #region Update

        public  void Update(GameTime gameTime)
        {


            if (player.ActiveTile && player.ActiveTileType == "button")
            {
                buttons[player.ActiveTileID].Update(gameTime);
                if (!buttons[player.ActiveTileID].Moving)
                    player.ActiveTile = false;
            }
            else if (player.ActiveTile && player.ActiveTileType == "tile")
            {
                ;
            }
            else
            {
                for (int x = buttons.Count - 1; x >= 0; x--)
                {
                    if (x == player.ActiveTileID && player.ActiveTileType == "button")
                        buttons[x].ActiveTile = true;
                    else
                        buttons[x].ActiveTile = false;


                    buttons[x].Update(gameTime);

                    if (buttons[x].Moving)
                    {
                        if (player.ActiveTileType == "button")
                        buttons[player.ActiveTileID].ActiveTile = false;
                        player.ActiveTileType = "button";
                        player.RotatingType = "";
                        buttons[x].ActiveTile = true;
                        player.ActiveTileID = x;
                        player.ActiveTile = true;
                    }

                }
            }

            //keylocker.Update(gameTime);

            AdjustFormButtonLocation();

             currMouse  = Mouse.GetState();

                 if (buttons[0].OnMouseClick())
                   tileManager.AddTile(Camera.Position + (buttons[0].Location - new Vector2(0, -TileGrid.OriginalTileHeight)), player.PlayerTurn);

                if (buttons[1].OnMouseClick())
                    tileManager.AddSoldier(Camera.Position + (buttons[1].Location - new Vector2(2, -TileGrid.OriginalTileHeight + 10)), player.PlayerTurn,player.ActivePlayerID);

                if (buttons[2].OnMouseClick() || (currMouse.MiddleButton==ButtonState.Pressed && prevMouse.MiddleButton!=ButtonState.Pressed))
                    tileManager.LockTiles();

                if (buttons[3].OnMouseClick())
                {
                    if (player.activePlayers < 7)
                    {
                        AddPlayer("Kokos" + player.activePlayers,player.activePlayers);
                      //  tileManager.AddScoreBoardSoldier( scoreboardItemLocation(),"Kokos" + (player.activePlayers - 1));
                    }
                }

                if (buttons[4].OnMouseClick())
                {
                    player.UnlockObject = !player.UnlockObject;
                    buttons[4].CodeValue = "Unlock " + player.UnlockObject;
                }
            
            PlayerButtons();
            prevMouse = currMouse;
           // HandleKeyLocker();

        }

        #endregion

        #region Draw

        public  void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in buttons)
                button.Draw(spriteBatch);

           // keylocker.Draw(spriteBatch);

        }

        #endregion

    }

}

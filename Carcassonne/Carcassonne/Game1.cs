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
using Lidgren.Network;
using MultiplayerGame.Networking;
using MultiplayerGame.Networking.Messages;

namespace Carcassonne
{
    using TileEngine.Camera;
    using TileEngine.Entity;
    using TileEngine.Form;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private readonly CameraManager cameraHandler;
        private readonly INetworkManager networkManager;
        private readonly ButtonManager buttonManager;
        private readonly MenuText menuText;
        private readonly TileManager tileManager;
        private readonly PlayerInformation playerInformation;
        private readonly DatabaseManager databaseManager;
        private readonly DeckManager deckManager;
        private TemplateManager templateManager;
        private string serverName;
        private string IP;

        public Game1(INetworkManager networkManager,string serverName,string IP,string playerName)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.networkManager = networkManager;
            this.serverName = serverName;
            this.IP = IP;
            TileGrid.networkManager = this.networkManager;
            playerInformation = new PlayerInformation(playerName);
            tileManager = new TileManager(playerInformation);
            buttonManager = new ButtonManager(playerInformation,tileManager);
            menuText = new MenuText(playerInformation);
            cameraHandler = new CameraManager(tileManager);
            deckManager = new DeckManager();
            databaseManager = new DatabaseManager(deckManager);

        }
        
        /// <summary>        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// 

        #region Properties

        /// <summary>
        /// Gets a value indicating whether IsHost.
        /// </summary>
        private bool IsHost
        {
            get
            {
                return this.networkManager is ServerNetworkManager;
            }
        }

        #endregion

        protected override void Initialize()
        {

            this.IsMouseVisible = true;

            this.Window.Title = "Carcassonne ALPHA";
            this.graphics.IsFullScreen = false;
            this.graphics.PreferredBackBufferWidth = 1600;
            this.graphics.PreferredBackBufferHeight = 900;
            this.graphics.ApplyChanges();

            this.networkManager.Connect(serverName, IP);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics.PreferMultiSampling = true;


            Camera.WorldRectangle = new Rectangle(0, 0, TileGrid.MapWidth * TileGrid.TileWidth, TileGrid.MapHeight * TileGrid.TileHeight);
            Camera.Position = Vector2.Zero;
            Camera.ViewPortWidth = this.GraphicsDevice.Viewport.Width;
            Camera.ViewPortHeight = this.GraphicsDevice.Viewport.Height;

            TileGrid.Initialize(Content, playerInformation.playerTurn);
            FormManager.Initialize(Content, "Kokos");
            buttonManager.Initialize(Content, "Kokos");
            databaseManager.loadFromDatabase();
            deckManager.Initialize(Content);
            tileManager.Initialize(Content, deckManager, this.IsHost);
            menuText.Initialize(Content, deckManager);
            templateManager = new TemplateManager(Content);

            if(this.IsHost)
                templateManager.addTemplate(playerInformation.playerTurn, 0, playerInformation.playerTurn);

            // tileManager.AddScoreBoardSoldier(buttonManager.scoreboardItemLocation(),"Kokos");

            tileManager.TileStateAdd += (sender, e) => networkManager.SendMessage(new AddTileMessage(e.codeValue, e.ID, e.Count));
            tileManager.TileStateRequest += (sender, e) => networkManager.SendMessage(new RequestTileMessage(e.codeValue, e.ID, e.Count));
            tileManager.TileStateUpdated += (sender, e) => networkManager.SendMessage(new UpdateTileMessage(e.tile, e.playerID, e.scale));
            tileManager.ItemStateRequest += (sender, e) => networkManager.SendMessage(new RequestItemMessage(e.codeValue, e.ID, e.Count, e.ColorID, e.Size));
            tileManager.ItemStateAdd += (sender, e) => networkManager.SendMessage(new AddItemMessage(e.codeValue, e.ID, e.Count, e.ColorID, e.Size));
            tileManager.ItemStateUpdated += (sender, e) => networkManager.SendMessage(new UpdateItemMessage(e.item, e.playerID, e.scale));
            templateManager.RequestTemplate += (sender, e) => networkManager.SendMessage(new RequestTemplateMessage(e.Name,e.Pos,e.Sender));
            templateManager.AddTemplate += (sender, e) => networkManager.SendMessage(new AddTemplateMessage(e.Name, e.Pos, e.Sender));
            //request template event



        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            this.networkManager.Disconnect();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            exitResize();

            cameraHandler.Update(gameTime);
            FormManager.Update(gameTime);
            buttonManager.Update(gameTime);
            tileManager.Update(gameTime);
            menuText.Update(gameTime);
            templateManager.Update(gameTime);
            //Camera.Update();

            ProcessNetworkMessages();

            base.Update(gameTime);
        }

        private void exitResize()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if ((Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Keyboard.GetState().IsKeyDown(Keys.LeftShift))
               || (Keyboard.GetState().IsKeyDown(Keys.RightAlt) && Keyboard.GetState().IsKeyDown(Keys.RightShift)))
            {
                graphics.ToggleFullScreen();
            }
        }

        #region Handle Incoming Messages

        private void HandleUpdateTemplateMessage(NetIncomingMessage im)
        {
            var message = new AddTemplateMessage(im);

            if (playerInformation.playerTurn != message.Sender)
            {
                templateManager.updateTemplate(message.Name, message.Pos, message.Sender);
                if(this.IsHost)
                    TileGrid.OnUpdateTemplate(message.Name, message.Pos, message.Sender);
            }
        }

        private void HandleAddTemplateMessage(NetIncomingMessage im)
        {
            var message = new AddTemplateMessage(im);

             templateManager.addNewTemplate(message.Name, message.Pos,message.Sender);
            
        }

        private void HandleRequestTemplateMessage(NetIncomingMessage im)
        {
            var message = new RequestTemplateMessage(im);

            if (this.IsHost)
            {
                templateManager.addTemplate(message.Name,message.Pos,message.Sender);
            }
        }

        private void HandleAddTileMessage(NetIncomingMessage im)
        {
            var message = new AddTileMessage(im);

            var timeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(message.MessageTime));

            if(!this.IsHost)
            tileManager.AddTile(Camera.Position + (buttonManager.buttons[0].Location - new Vector2(0, -TileGrid.OriginalTileHeight)), message.CodeValue, message.ID,message.Count);
        }

        private void HandleRequestTileMessage(NetIncomingMessage im)
        {
            var message = new RequestTileMessage(im);

            var timeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(message.MessageTime));

            if(this.IsHost)
                tileManager.AddTile(Camera.Position + (buttonManager.buttons[0].Location - new Vector2(0, -TileGrid.OriginalTileHeight)), message.CodeValue);
        }

        private void HandleAddItemMessage(NetIncomingMessage im)
        {
            var message = new AddItemMessage(im);

            var timeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(message.MessageTime));

            if (!this.IsHost)
                tileManager.AddItem(Camera.Position + (buttonManager.buttons[1].Location - new Vector2(0, -TileGrid.OriginalTileHeight)), message.CodeValue, message.ID, message.Count,message.ColorID,message.Size);
        }

        private void HandleRequestItemMessage(NetIncomingMessage im)
        {
            var message = new RequestItemMessage(im);

            var timeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(message.MessageTime));

            if (this.IsHost)
                tileManager.AddSoldier(Camera.Position + (buttonManager.buttons[1].Location - new Vector2(0, -TileGrid.OriginalTileHeight)), message.CodeValue,message.ColorID,message.Size);
        }

        private void HandleUpdateTileMessage(NetIncomingMessage im)
        {
            var message = new UpdateTileMessage(im);

            tileManager.UpdateTile(message.ID, message.PlayerID, cameraHandler.adjustLocationAt(message.Location, message.Scale));
            if (this.IsHost)
               tileManager.OnUpdateTile(tileManager.getTileFromList(message.ID),message.PlayerID);
        }

        private void HandleUpdateItemMessage(NetIncomingMessage im)
        {
            var message = new UpdateItemMessage(im);
            tileManager.UpdateItem(message.ID, message.PlayerID,cameraHandler.adjustLocationAt(message.Location, message.Scale));
            if (this.IsHost)
                tileManager.OnUpdateItem(tileManager.getItemFromList(message.ID), message.PlayerID);
        }

        private void HandleRemoveFromGridMessage(NetIncomingMessage im)
        {
            var message = new RemoveFromGridMessage(im);
            tileManager.removeFromGrid(message.ID,message.PlayerID, cameraHandler.adjustLocationAt(message.Location, message.Scale));

        }

        private void HandleSnapToGridMessage(NetIncomingMessage im)
        {
            var message = new SnapToGridMessage(im);
            tileManager.snapToGrid(message.ID, message.PlayerID, cameraHandler.adjustLocationAt(message.Location, message.Scale));

        }

        private void HandleRotationMessage(NetIncomingMessage im)
        {
            var message = new RotationMessage(im);

            tileManager.rotateManually(message.RotationValue,message.ID, message.PlayerID, message.Type);
            if (this.IsHost)
                TileGrid.OnRotation(message.RotationValue, message.PlayerID, message.ID, message.Type);
        }

        #endregion

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(
            SpriteSortMode.BackToFront,
            BlendState.AlphaBlend);
            //null,null,null,null,
           // Camera.Transform);

            TileGrid.Draw(spriteBatch);
            FormManager.Draw(spriteBatch);
            tileManager.Draw(spriteBatch);
            buttonManager.Draw(spriteBatch);
            menuText.Draw(spriteBatch);
            templateManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
            
        }

        /// <summary>
        /// The process network messages.
        /// </summary>
        /// 
        private void ProcessNetworkMessages()
        {
            NetIncomingMessage im;

            while ((im = this.networkManager.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(im.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)im.ReadByte())
                        {
                            case NetConnectionStatus.Connected:
                                if (!this.IsHost)
                                {
                                    Console.WriteLine("Connected to {0}");
                                    templateManager.OnRequestTemplate(playerInformation.playerTurn, playerInformation.playerTurn);
                                }
                                else
                                {
                                    Console.WriteLine("{0} Connected");
                                }

                                break;
                            case NetConnectionStatus.Disconnected:
                                Console.WriteLine(
                                    this.IsHost ? "{0} Disconnected" : "Disconnected from {0}");
                                break;
                            case NetConnectionStatus.RespondedAwaitingApproval:
                                NetOutgoingMessage hailMessage = this.networkManager.CreateMessage();                          
                                im.SenderConnection.Approve(hailMessage);
                                break;
                        }

                        break;
                    case NetIncomingMessageType.Data:
                        var gameMessageType = (GameMessageTypes)im.ReadByte();
                        switch (gameMessageType)
                        {
                            case GameMessageTypes.AddTileState:
                                this.HandleAddTileMessage(im);
                                break;
                            case GameMessageTypes.RequestTileState:
                                this.HandleRequestTileMessage(im);
                                break;
                            case GameMessageTypes.UpdateTileState:
                                this.HandleUpdateTileMessage(im);
                                break;
                            case GameMessageTypes.UpdateItemState:
                                this.HandleUpdateItemMessage(im);
                                break;
                            case GameMessageTypes.RemoveFromGridState:
                                this.HandleRemoveFromGridMessage(im);
                                break;
                            case GameMessageTypes.SnapToGridState:
                                this.HandleSnapToGridMessage(im);
                                break;
                            case GameMessageTypes.AddItemState:
                                this.HandleAddItemMessage(im);
                                break;
                            case GameMessageTypes.RequestItemState:
                                this.HandleRequestItemMessage(im);
                                break;
                            case GameMessageTypes.RotationValueState:
                                this.HandleRotationMessage(im);
                                break;
                            case GameMessageTypes.RequestTemplateState:
                                this.HandleRequestTemplateMessage(im);
                                break;
                            case GameMessageTypes.AddTemplateState:
                                this.HandleAddTemplateMessage(im);
                                break;
                            case GameMessageTypes.UpdateTemplateState:
                                this.HandleUpdateTemplateMessage(im);
                                break;
                        }

                        break;
                }

                this.networkManager.Recycle(im);
            }
        }


      
    }
}
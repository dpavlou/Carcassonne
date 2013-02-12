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
using TileEngine;

namespace Carcassonne
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont pericles10;
        Player player;
        FpsMonitor fps;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            fps = new FpsMonitor();

            this.IsMouseVisible = true;

            this.Window.Title = "Carcassonne ALPHA";
            this.graphics.IsFullScreen = false;
            this.graphics.PreferredBackBufferWidth = 1600;
            this.graphics.PreferredBackBufferHeight = 900;
            this.graphics.ApplyChanges();

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

            pericles10 = Content.Load<SpriteFont>(@"Fonts\Pericles10");

            TileGrid.Initialize(
              Content.Load<Texture2D>(@"Textures\MapSquare"), Content.Load<Texture2D>(@"Textures\BaseGame\city1"));

            TileManager.Initialize(new Vector2(this.GraphicsDevice.Viewport.Width - 200, 40),
                                   new Vector2(this.GraphicsDevice.Viewport.Width - 200, 140), pericles10);

            //Camera.newViewPort = GraphicsDevice.Viewport;

            Camera.WorldRectangle = new Rectangle(0, 0, TileGrid.MapWidth * TileGrid.TileWidth, TileGrid.MapHeight * TileGrid.TileHeight);
            Camera.Position = Vector2.Zero;
            Camera.ViewPortWidth = this.GraphicsDevice.Viewport.Width;
            Camera.ViewPortHeight = this.GraphicsDevice.Viewport.Height;

            player = new Player(Content,"Kokos",new Vector2(TileGrid.MapWidth/2*TileGrid.TileWidth,TileGrid.MapHeight/2*TileGrid.TileHeight));

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit       
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            fps.Update();

            player.Update(gameTime);
           // Camera.Update();

            base.Update(gameTime);

        }

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
            TileManager.Draw(spriteBatch);

            spriteBatch.DrawString(
            pericles10,
            "WorldLocation:"+
            player.worldLocation,
            new Vector2(10, 10),
            Color.Red);

            fps.Draw(spriteBatch, pericles10, new Vector2(10, 40), Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

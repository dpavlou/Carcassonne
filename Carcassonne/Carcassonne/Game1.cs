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
using Tile_Engine;

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
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;

            this.Window.Title = "Carcassonne ALPHA";
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
              Content.Load<Texture2D>(@"Textures\MapSquare"));
            //Camera.newViewPort = GraphicsDevice.Viewport;

            Camera.WorldRectangle = new Rectangle(0, 0, TileGrid.MapWidth * TileGrid.TileWidth, TileGrid.MapHeight * TileGrid.TileHeight);
            Camera.Position = Vector2.Zero;
            Camera.ViewPortWidth = 1600;
            Camera.ViewPortHeight = 900;

            player = new Player(Content,"skata");

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

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

            spriteBatch.DrawString(
            pericles10,
            "WorldLocation:"+
            player.WorldPosition,
             new Vector2(10, 10),
             Color.Red);

            spriteBatch.DrawString(
            pericles10,
            "Scale:" +
            player.scale,
            new Vector2(10, 40),
            Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

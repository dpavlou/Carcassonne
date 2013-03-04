using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Carcassonne
{
    using TileEngine.Camera;
    using TileEngine.Form;

    public class MenuText
    {

        #region Declarations

        private  Vector2 text1offSet;
        private  Vector2 text2offSet;
        private  Vector2 text3offSet;
        private  Vector2 text4offSet;
        private  readonly FpsMonitor fps;
        private readonly PlayerInformation player;
        private DeckManager deckManager;
        private  SpriteFont font;

        #endregion

        #region Constructor

        public MenuText(PlayerInformation player)
        {
            fps = new FpsMonitor();
            text1offSet = new Vector2(40, 20);
            text2offSet = new Vector2(40, 80);
            text3offSet = new Vector2(40, 110);
            text4offSet = new Vector2(40, 50);
            this.player = player;
        }
        #endregion

        #region Initialize

        public  void Initialize(ContentManager Content,DeckManager deckManager)
        {
            this.deckManager=deckManager;
            font = Content.Load<SpriteFont>(@"Fonts\Pescadero14");
        }

        #endregion

        #region Update

        public  void Update(GameTime gameTime)
        {
            fps.Update();
        }

        #endregion
        
        #region Draw

        public  void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
             font,
            "WorldLocation: " +
             Camera.WorldLocation,
              text1offSet+FormManager.menu.Location,
            Color.Black);
             
            spriteBatch.DrawString(
            font,
            "Tiles Left: " +
             deckManager.CountAll,
             text2offSet+FormManager.menu.Location,
            Color.Black);
            
            spriteBatch.DrawString(
            font,
            "Active Player: " +
            player.PlayerTurn,
            text3offSet+FormManager.menu.Location,
            Color.Black);

            fps.Draw(spriteBatch, font, text4offSet + FormManager.menu.Location, Color.Black);
        }

        #endregion

    }
}

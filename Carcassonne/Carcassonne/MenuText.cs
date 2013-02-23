using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine;

namespace Carcassonne
{
    public static class MenuText
    {

        #region Declarations

        private static Vector2 text1offSet;
        private static Vector2 text2offSet;
        private static Vector2 text3offSet;
        private static Vector2 text4offSet;
        private static FpsMonitor fps;
        private static SpriteFont font;

        #endregion

        #region Initialize

        public static void Initialize(SpriteFont Font)
        {
            text1offSet=new Vector2(40,20);
            text2offSet=new Vector2(40,80);
            text3offSet=new Vector2(40,110);
            text4offSet=new Vector2(40, 50);
            font = Font;
            fps = new FpsMonitor();
        }

        #endregion

        #region Update

        public static void Update(GameTime gameTime)
        {
            fps.Update();
        }

        #endregion
        
        #region Draw

        public static void Draw(SpriteBatch spriteBatch)
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
            Deck.deck.Count,
             text2offSet+FormManager.menu.Location,
            Color.Black);

            spriteBatch.DrawString(
            font,
            "Active Player: " +
            PlayerManager.PlayerTurn,
            text3offSet+FormManager.menu.Location,
            Color.Black);

            fps.Draw(spriteBatch, font, text4offSet + FormManager.menu.Location, Color.Black);
        }

        #endregion

    }
}

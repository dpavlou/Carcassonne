using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
    public static class FormManager
    {

        #region Declarations

        public static Form menu;
        public static Form privateSpace;
        private static Texture2D scoreboard;

        #endregion

        #region Initialize

        public static void Initialize(Texture2D button1, Texture2D scoreBoard, SpriteFont font, string owner)
        {
            menu = new Form(button1,"MENU",new Vector2(600, Camera.ViewPortHeight), font, new Vector2(Camera.ViewPortWidth, 0),true);
            privateSpace = new Form(button1,"PRIVATE",new Vector2(300, Camera.ViewPortHeight), font, new Vector2(0, 0),false);
            ScoreBoard = scoreBoard;
        }

        #endregion

        #region Properties

        public static Texture2D ScoreBoard
        {
            get { return scoreboard; }
            set { scoreboard = value; }
        }

       public static Rectangle ScoreBoardRectangle
       {
           get { return new Rectangle((int)menu.Location.X + 30, Camera.ViewPortHeight - 470, ScoreBoard.Width+50, ScoreBoard.Height+50); }
       }

        #endregion

        #region Update

       public static void Update(GameTime gameTime)
       {
           menu.Update(gameTime);
           privateSpace.Update(gameTime);
       }

       #endregion

        #region Draw

       public static void Draw(SpriteBatch spriteBatch)
       {
           menu.Draw(spriteBatch);
           privateSpace.Draw(spriteBatch);
           spriteBatch.Draw(ScoreBoard, ScoreBoardRectangle,null, Color.White,0.0f,Vector2.Zero,SpriteEffects.None,0.05f);
       }

       #endregion

    }
}

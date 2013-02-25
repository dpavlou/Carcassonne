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
        private static bool dragging;
        private static Texture2D scoreboard;

        #endregion

        #region Initialize

        public static void Initialize(Texture2D button1, Texture2D scoreBoard, SpriteFont font, string owner)
        {
            menu = new Form(button1,"MENU",new Vector2(600, Camera.ViewPortHeight), font, new Vector2(Camera.ViewPortWidth, 0),true);
            privateSpace = new Form(button1,"  PRIVATE",new Vector2(300, Camera.ViewPortHeight), font, new Vector2(0, 0),false);
            ScoreBoard = scoreBoard;
            dragging = false;
        }

        #endregion

        #region Properties

        public static Texture2D ScoreBoard
        {
            get { return scoreboard; }
            set { scoreboard = value; }
        }

        public static bool Dragging
        {
            get { return dragging; }
            set { dragging = value; }
        }

       public static Rectangle ScoreBoardRectangle
       { 
           get { return new Rectangle((int)menu.Location.X + 10, Camera.ViewPortHeight - 510, ScoreBoard.Width+80, ScoreBoard.Height+80); }
       }

      /* public static Rectangle ScoreBoardRectangle
        {
            get
            {
                int textureOffSet = Camera.ViewPortHeight - (Camera.ViewPortHeight / 2 + 30 + ScoreBoard.Height);
                return new Rectangle((int)menu.Location.X + 10, Camera.ViewPortHeight / 2 + 30, ScoreBoard.Width + textureOffSet - 10, ScoreBoard.Height + textureOffSet - 10);
            }
        }*/

        #endregion

        #region Update

       public static void Update(GameTime gameTime)
       {

           menu.Update(gameTime);
           privateSpace.Update(gameTime);

           if (menu.Dragging || privateSpace.Dragging)
               Dragging = true;
           else
               Dragging = false;

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

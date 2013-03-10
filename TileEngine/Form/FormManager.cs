using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Form
{
    using TileEngine.Camera;

    public static class FormManager
    {

        #region Declarations

        public static Form menu;
        public static Form privateSpace;
        private static bool dragging;
        private static Texture2D scoreboard;

        #endregion

        #region Initialize

        public static void Initialize(ContentManager Content, string owner)
        {
            Texture2D button1=Content.Load<Texture2D>(@"Textures\formButton");
            SpriteFont font = Content.Load<SpriteFont>(@"Fonts\pericles10");

            menu = new Form(button1,"    M",new Vector2(600, Camera.ViewPortHeight), font, new Vector2(Camera.ViewPortWidth, 0),true);
            privateSpace = new Form(button1,"    T",new Vector2(200, Camera.ViewPortHeight), font, new Vector2(0, 0),false);
            privateSpace.Close();
            ScoreBoard = Content.Load<Texture2D>(@"Textures\ScoreBoard");
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
            get
            {
                int textureOffSet = Camera.ViewPortHeight - (Camera.ViewPortHeight / 2 + 30 + ScoreBoard.Height);
                return new Rectangle((int)menu.Location.X + 20, Camera.ViewPortHeight / 2 + 30, ScoreBoard.Width + textureOffSet/2 - 10, ScoreBoard.Height + textureOffSet/2 - 10);
            }
        }

        #endregion

        #region Update

       public  static void Update(GameTime gameTime)
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

       public  static void Draw(SpriteBatch spriteBatch)
       {
           menu.Draw(spriteBatch);
           privateSpace.Draw(spriteBatch);
           spriteBatch.Draw(ScoreBoard, ScoreBoardRectangle,null, Color.White,0.0f,Vector2.Zero,SpriteEffects.None,0.05f);
       }

       #endregion

    }
}

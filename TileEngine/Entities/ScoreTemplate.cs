using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TileEngine;

namespace TileEngine.Entity
{
    using TileEngine.Camera;

    public class ScoreTemplate
    {
        #region Declarations
        
        public List<Button> buttons = new List<Button>();
        public int score;
        private SpriteFont font;
        private Texture2D template;
        private Vector2 location;
        private Texture2D button;

        #endregion

        #region Constructor

        public ScoreTemplate(SpriteFont font,Texture2D button,Texture2D template,string id, int pos)
        {

            float offSet=5.0f;
            float nameOffset = 180f;

            Score = 0;
            ID = id;

            this.template = template;
            this.font = font;
            this.button = button;

            location = new Vector2(Camera.ViewPortWidth - template.Width - offSet, pos * template.Height + offSet);

            buttons.Add(new Button("+1", new Vector2(-13, -10), button, font, new Vector2(location.X + nameOffset + button.Width / 2, location.Y + button.Height / 2), 1, 0.1f, false, Color.Black));
            buttons.Add(new Button("-1", new Vector2(-13, -10), button, font, new Vector2(location.X + nameOffset + button.Width / 2, location.Y + button.Height + button.Height / 2), 1, 0.1f, false, Color.Black));
            buttons.Add(new Button("+10", new Vector2(-13, -10), button, font, new Vector2(location.X + nameOffset + button.Width + button.Width / 2, location.Y + button.Height / 2), 1, 0.1f, false, Color.Black));
            buttons.Add(new Button("-10", new Vector2(-13, -10), button, font, new Vector2(location.X + nameOffset + button.Width + button.Width / 2, location.Y + button.Height + button.Height / 2), 1, 0.1f, false, Color.Black));
        }

        #endregion

        #region Properties

        public int Score
        {
            get { return score; }
            set
            {
                score = (int)MathHelper.Max(0, (float)value);
                TileGrid.OnUpdateTemplate(ID,score,TileGrid.PlayerID);
            }
        }

        public string ID
        {
            get;
            set;
        }

        #endregion

        #region Events



        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {

            foreach (Button button in buttons)
                button.Update(gameTime);

            if (buttons[0].OnMouseClick())
                Score += 1;
            if (buttons[1].OnMouseClick())
                Score -= 1;
            if (buttons[2].OnMouseClick())
                Score += 10;
            if (buttons[3].OnMouseClick())
                Score -= 10;

        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(
                     template,
                     location,
                     null,
                     Color.White*0.5f,
                     0.0f,
                     Vector2.Zero,
                     1.0f,
                     SpriteEffects.None,
                     0.19f);

            spriteBatch.DrawString(
                       font,
                       " "+ID,
                       location+new Vector2(10,20),
                       Color.Black);

            spriteBatch.DrawString(
                     font,
                     "| " + Score,
                    location + new Vector2(120, 20),
                     Color.Black);

            foreach (Button button in buttons)
                button.Draw(spriteBatch);


        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Entity
{
    public class Frame
    {
        #region Declarations

        public Texture2D dot;

        #endregion

        #region Initialize

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            dot = new Texture2D(graphicsDevice, 1, 1);
            dot.SetData(new[] { Color.White });
        }
        #endregion

        public void DrawLine(SpriteBatch spriteBatch,Vector2 start, Vector2 end, Color color)
        {
            float length = (end - start).Length();
            float rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            spriteBatch.Draw(dot, start, null, color, rotation, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0.0f);

        }

        public void DrawRectangle(SpriteBatch spriteBatch, Vector2 location, Vector2 size,Color color)
        {
            DrawLine(spriteBatch, location, location + new Vector2(size.X, 0), color);
            DrawLine(spriteBatch, location, location + new Vector2(0,size.Y), color);
            DrawLine(spriteBatch, location + new Vector2(0, size.Y),location + new Vector2(size.X, size.Y),color);
            DrawLine(spriteBatch, location + new Vector2(size.X, 0), location + new Vector2(size.X, size.Y), color);
        }
    }
}

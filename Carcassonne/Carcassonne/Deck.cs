using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Carcassonne
{
    public static class Deck
    {

        #region Declarations

        public static List<Texture2D> textures = new List<Texture2D>();
        public static int texturesNo;

        #endregion

        #region Initialize

        public static void Initialize(ContentManager content)
        {
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city1"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city11ne"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city11we"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city1rse"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city1rsw"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city1rswe"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city1rwe"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city2nw"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city2nws"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city2nwsr"));
            texturesNo = 10;
        }
        #endregion

        #region Public Methods

        public static Texture2D GetRandomTile()
        {
            Random rand = new Random();

            int x = rand.Next(0, texturesNo);

            return textures[x];
        }

        #endregion

    }
}

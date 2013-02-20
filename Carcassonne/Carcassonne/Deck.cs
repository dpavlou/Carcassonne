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
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city2we"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city2wes"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city3"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city3r"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city3s"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city3sr"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city4"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\cloister"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\cloisterr"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\road2ns"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\road2sw"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\road3"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\road4"));
            texturesNo = 23;
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

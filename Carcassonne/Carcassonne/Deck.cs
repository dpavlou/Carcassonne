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
        public static List<int> deck = new List<int>();
        public static Texture2D soldier;
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
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city2nw"));
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city2nwr"));
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
            textures.Add(content.Load<Texture2D>(@"Textures\BaseGame\city1rwe"));
            soldier = (content.Load<Texture2D>(@"Textures\Soldier"));
            texturesNo = 24;

            LoadDeck();
        }
        #endregion

        #region Public Methods

        public static void LoadDeck()
        {
            for (int i = 0; i < 5; i++)
                deck.Add(0);
            for (int i = 0; i < 2; i++)
                deck.Add(1);
            for (int i = 0; i < 3; i++)
                deck.Add(2);
            for (int i = 0; i < 3; i++)
                deck.Add(3);
            for (int i = 0; i < 3; i++)
                deck.Add(4);
            for (int i = 0; i < 3; i++)
                deck.Add(5);
            for (int i = 0; i < 3; i++)
                deck.Add(6);
            for (int i = 0; i < 3; i++)
                deck.Add(7);
            for (int i = 0; i < 3; i++)
                deck.Add(8);
            for (int i = 0; i < 2; i++)
                deck.Add(9);
            for (int i = 0; i < 2; i++)
                deck.Add(10);
            for (int i = 0; i < 1; i++)
                deck.Add(11);
            for (int i = 0; i < 2; i++)
                deck.Add(12);
            for (int i = 0; i < 3; i++)
                deck.Add(13);
            for (int i = 0; i < 1; i++)
                deck.Add(14);
            for (int i = 0; i < 1; i++)
                deck.Add(15);
            for (int i = 0; i < 2; i++)
                deck.Add(16);
            for (int i = 0; i < 1; i++)
                deck.Add(17);
            for (int i = 0; i < 4; i++)
                deck.Add(18);
            for (int i = 0; i < 2; i++)
                deck.Add(19);
            for (int i = 0; i < 8; i++)
                deck.Add(20);
            for (int i = 0; i < 9; i++)
                deck.Add(21);
            for (int i = 0; i < 4; i++)
                deck.Add(22);
            for (int i = 0; i < 1; i++)
                deck.Add(23);

          //  for (int i = 0; i < 1; i++)
            //    deck.Add(0);

        }
        public static Texture2D GetRandomTile()
        {
            Random rand = new Random();

            if (deck.Count == 0)
            {
                LoadDeck();
                TileManager.itemID = 0;
                TileManager.ID = 0;
            }


            int x = rand.Next(0, deck.Count);

            int textureID = deck[x];
            deck.RemoveAt(x);

            return textures[textureID];
        }

        public static Texture2D GetSoldier()
        {
            return soldier;
        }

        #endregion

    }
}

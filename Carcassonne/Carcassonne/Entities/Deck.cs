using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Carcassonne
{
    public class Deck
    {

        #region Declarations

        public List<Texture2D> textures = new List<Texture2D>();
        public List<int> deck = new List<int>();
        public List<string> textureName = new List<string>();
        public int reservedTile;
        public bool reserved;
        public int fullDeck;

        #endregion

        #region Initialize

        public void Initialize(ContentManager content)
        {    

            foreach (string texturename in textureName)
            {
                textures.Add(content.Load<Texture2D>(texturename));
            }
            fullDeck = deck.Count;
            reserved = false;
          
        }
        #endregion

        #region Properties

        public bool Reserve
        {
            get { return reserved; }
            set { reserved = value; }
        }

        public int Count
        {
            get { return deck.Count; }
        }

        #endregion

        #region Public Methods

        public void reserveTile()
        {
            reserved = true;
            reservedTile = textureName.Count - 1;
            deck.RemoveAt(deck.Count - 1);
        }
        public Texture2D getReservedTile()
        {
            Reserve = false;
            return textures[reservedTile];
        }


        public int GetRandomTile()
        {
            Random rand = new Random();

            if (deck.Count == 0)
            {
                if (reserved)
                {
                    return reservedTile;
                }
            }


            return (rand.Next(0, deck.Count));

        }

        public Texture2D GetTileTexture(int x)
        {
            int textureID = deck[x];
            deck.RemoveAt(x);

            return textures[textureID];
        }

        public Texture2D getEndingTexture(int x)
        {
            return textures[x];
        }

        public Texture2D GetRandomTileTexture()
        {
            return GetTileTexture(GetRandomTile());
        }

        #endregion

    }
}

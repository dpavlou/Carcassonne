using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Carcassonne
{
    public class DeckManager
    {

        #region Declarations

        public List<Deck> decks;
        private int activeDeck;

        #endregion

        #region Constructor

        public DeckManager()
        {
            decks = new List<Deck>();
            activeDeck=0;
        }

        #endregion

        #region Initialize

        public void Initialize(ContentManager Content)
        {
            foreach (Deck deck in decks)
            {
                deck.Initialize(Content);
            }
            decks[0].reserveTile();
        }

        #endregion

        #region Properties

        public int NextDeck
        {
            get { return (int)MathHelper.Min(activeDeck+1, decks.Count); }
        }

        public int Count
        {
            get { return decks[activeDeck].Count; }
        }

        #endregion

        #region Public Methods

        public void AddNewDeck()
        {
            decks.Add(new Deck());
        }

        public void AddTextureName(string name, int deckID)
        {
            decks[deckID].textureName.Add(name);
        }

        public void AddQuantities(int quantity, int deckID)
        {
            decks[deckID].deck.Add(quantity);
        }

        public Texture2D GetTileTexture(int x,int deck)
        {
            return decks[deck].GetTileTexture(x);
        }

        public Texture2D GetTileTexture(int x)
        {

                if (activeDeck == 0 && decks[activeDeck].Reserve && Count==0)
                {
                    activeDeck = NextDeck;
                    return decks[0].getReservedTile();
                }
                if (Count == decks[1].fullDeck && activeDeck == 1)
                    return decks[1].GetTileTexture(0);

                if (Count == 0 && activeDeck >= 1)
                    return decks[1].getEndingTexture(0);
                          
              
            return decks[activeDeck].GetTileTexture(x);
        }

        public int GetRandomTileNo()
        {
            int no = decks[activeDeck].GetRandomTile();

         //   if (decks[activeDeck].Count ==0)
            //    activeDeck = NextDeck;

            return no;
        }

        public Texture2D GetRandomTile(int pos)
        {
                        
            return decks[activeDeck].GetTileTexture(pos);
        }


        #endregion

    }
}

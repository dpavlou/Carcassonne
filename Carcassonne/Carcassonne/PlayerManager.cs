using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TileEngine;

namespace Carcassonne
{
    public static class PlayerManager
    {

        #region Declarations

        private static bool activeTile;
        private static int activeTileID;
        private static string activeTileType;
        private static string rotatingType;
        public static Dictionary<string, int> PlayerStatus = new Dictionary<string, int>();
        public static string playerTurn;
        public static int activePlayers;
        private static bool unlockObject;
        private static int activePlayerID;

        #endregion

        #region Initialize

        public static void Initialize(string player)
        {
            activeTileID = 0;
            activeTile = false;
            rotatingType=activeTileType = "";
            activePlayers = 1;
            playerTurn = player;
            UnlockObject = false;
            ActivePlayerID = 1;
           // AddPlayer(player);
        }

        #endregion

        #region Properties

        public static bool UnlockObject
        {
            get { return unlockObject; }
            set { unlockObject = value; }
        }

        public static int ActiveTileID
        {
            get { return activeTileID; }
            set { activeTileID = value; }
        }

        public static int ActivePlayerID
        {
            get { return activePlayerID; }
            set { activePlayerID = value; }
        }

        public static string ActiveTileType
        {
            get { return activeTileType; }
            set { activeTileType = value; }
        }

        public static string RotatingType
        {
            get { return rotatingType; }
            set { rotatingType = value; }
        }

        public static Vector2 NewPlayerLocation
        {
            get
            {
                return
                    new Vector2(FormManager.menu.Location.X - TileGrid.OriginalTileWidth/2 +((activePlayers ) * TileGrid.OriginalTileWidth) + 2 * activePlayers, 330);
            }
        }
        public static bool ActiveTile
        {
            get { return activeTile; }
            set { activeTile = value; }
        }

        public static string PlayerTurn
        {
            get { return playerTurn; }
            set { playerTurn = value; }
        }
        public static Color ActivePlayerColor
        {
            get { return PlayerColor(activePlayerID); }
        }

        public static Color PlayerColor(int index)
        {
        
                if (index == 1)
                    return Color.Black;
                if (index == 2)
                    return Color.Blue;
                else if (index == 3)
                    return Color.Green;
                else if (index == 4)
                      return Color.Brown;
                else if (index == 5)
                     return Color.Red;
                else
                    return Color.Yellow;
        }
        

        #endregion

        #region Public Methods

        public static void AddPlayer(string Player)
        {
            PlayerStatus.Add(Player, activePlayers);
            activePlayers++;
        }

        public static void ResetActiveTile()
        {
          MouseState mouse=Mouse.GetState();

          if (mouse.RightButton != ButtonState.Pressed)
          {
              ActiveTileType = "";
              ActiveTile = false;
          }

        }

        public static void IncrementTiles()
        {
           //increment tile for active player
        }

        #endregion

    }

}

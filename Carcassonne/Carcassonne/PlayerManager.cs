using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
        private static bool unlockObject;

        #endregion

        #region Initialize

        public static void Initialize(string player)
        {
            activeTileID = 0;
            activeTile = false;
            rotatingType=activeTileType = "";
            PlayerStatus.Add(player, 0);
            playerTurn = player;
            UnlockObject = false;
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
        
        #endregion

        #region Public Methods

        public static void ResetActiveTile()
        {
          MouseState mouse=Mouse.GetState();

          if (mouse.RightButton != ButtonState.Pressed)
          {
              ActiveTileType = "";
              ActiveTile = false;
          }

        }
        public static void AddPlayer(string player)
        {
            PlayerStatus.Add(player, 0);
            //add button with player name if playing localy
        }

        public static void IncrementTiles()
        {
           //increment tile for active player
        }

        #endregion

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Carcassonne
{
    using TileEngine.Entity;
    using TileEngine.Camera;
    using TileEngine.Form;

    public class PlayerInformation
    {

        #region Declarations

        private bool activeTile;
        private int activeTileID;
        private string activeTileType;
        private string rotatingType;
        public Dictionary<string, int> PlayerStatus = new Dictionary<string, int>();
        public string playerTurn;
        public int activePlayers;
        private bool unlockObject;
        private int activePlayerID;

        #endregion

        #region Constructor

        public PlayerInformation(string player)
        {
            activeTileID = 0;
            activeTile = false;
            rotatingType = activeTileType = "";
            activePlayers = 1;
            playerTurn = player;
            UnlockObject = false;
            ActivePlayerID = 1;
            // AddPlayer(player);
        }

        #endregion

        #region Properties

        public bool UnlockObject
        {
            get { return unlockObject; }
            set { unlockObject = value; }
        }

        public int ActiveTileID
        {
            get { return activeTileID; }
            set { activeTileID = value; }
        }

        public int ActivePlayerID
        {
            get { return activePlayerID; }
            set { activePlayerID = value; }
        }

        public string ActiveTileType
        {
            get { return activeTileType; }
            set { activeTileType = value; }
        }

        public string RotatingType
        {
            get { return rotatingType; }
            set { rotatingType = value; }
        }

        public Vector2 NewPlayerLocation
        {
            get
            {
                return
                    new Vector2(FormManager.menu.Location.X - TileGrid.OriginalTileWidth / 2 + ((activePlayers) * TileGrid.OriginalTileWidth) + 2 * activePlayers, 330);
            }
        }
        public bool ActiveTile
        {
            get { return activeTile; }
            set { activeTile = value; }
        }

        public string PlayerTurn
        {
            get { return playerTurn; }
            set { playerTurn = value; }
        }
        public Color ActivePlayerColor
        {
            get { return PlayerColor(activePlayerID); }
        }

        public Color PlayerColor(int index)
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

        public void AddPlayer(string Player)
        {
            PlayerStatus.Add(Player, activePlayers);
            activePlayers++;
        }

        public void ResetActiveTile()
        {
            MouseState mouse = Mouse.GetState();

            if (mouse.RightButton != ButtonState.Pressed)
            {
                ActiveTileType = "";
                ActiveTile = false;
            }

        }

        public void IncrementTiles()
        {
            //increment tile for active player
        }

        #endregion

    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine

{
    [Serializable]
    public class Tile
    {
        #region Declarations
        public int[] LayerTiles = new int[3];
        public string CodeValue = "";
        public bool Passable = true;

        #endregion

        #region Constructor
        public Tile(
            int background,
            int interactive,
            int foreground,
            string code
            )
        {
            LayerTiles[0] = background;
            LayerTiles[1] = interactive;
            LayerTiles[2] = foreground;
            CodeValue = code;
           
        }
        #endregion

        #region Public Methods
 
        
        #endregion

    }
}

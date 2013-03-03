using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Carcassonne
{
    using MultiplayerGame.Args;
    using TileEngine.Entity;
    using TileEngine.Form;
    using TileEngine.Camera;

    public class TileManager
    {
        #region Declarations

        #region Event Declarations

        public event EventHandler<IdentificationArgs> TileStateAdd;
        public event EventHandler<TileStateChangedArgs> TileStateUpdated;
        public event EventHandler<ItemStateChangedArgs> ItemStateUpdated;
        public event EventHandler<IdentificationArgs> TileStateRequest;
        public event EventHandler<IdentificationArgs> ItemStateRequest;
        public event EventHandler<IdentificationArgs> ItemStateAdd;

        #endregion

        private List<Tile> tiles;
        private List<Item> items;
        private SpriteFont font;
        private Texture2D frame1;
        private int Id;
        private int itemID;
        private Deck deck;
        private PlayerInformation player;
        private bool isHost;

        #endregion

        #region Constructor

        public TileManager(PlayerInformation player)
        {
            tiles = new List<Tile>();
            items = new List<Item>();
            itemID = Id = 0;
            this.player = player;
        }

        #endregion

        #region Initialize

        public void Initialize(ContentManager Content, Deck deck,bool isHost)
        {
            this.deck = deck;
            frame1 = Content.Load<Texture2D>(@"Textures\Frame1");
            font = Content.Load<SpriteFont>(@"Fonts\Pericles10");
            this.isHost = isHost;

            Id++;
            tiles.Add(new Tile("  C", new Vector2(-23, -10), deck.GetTileTexture(deck.Count-1), font,TileGrid.mapCenter, Id, 0.5f - Id * 0.001f, frame1, Color.Red));
            tiles[0].SnappedToForm = false;
            tiles[0].CheckCell();
        }

        #endregion

        #region Properties

        public bool IsHost
        {
            get { return isHost; }
        }

        #endregion

        #region Event Methods

        #region Tile Events

        public void UpdateTile(int ID,string playerID, Vector2 location)
        {
            foreach (Tile tile in tiles)
                if (tile.ID == ID && player.playerTurn!=playerID)
                {
                    tile.SnappedToForm = false;
                    tile.Location = location;
                   // tile.RotationValue = rotation;
                    break;
                }
        }

        public void OnUpdateTile(Tile tile,string playerID)
        {
            EventHandler<TileStateChangedArgs> tileStateUpdated = TileStateUpdated;
            if (tileStateUpdated != null)
                tileStateUpdated(tileStateUpdated, new TileStateChangedArgs(tile, playerID,TileGrid.TileWidth));
        }

        public void OnAddTile(string codeValue, int id,int count)
        {
            EventHandler<IdentificationArgs> tileStateAdd = TileStateAdd;
            if (tileStateAdd != null)
                tileStateAdd(tileStateAdd, new IdentificationArgs(codeValue,id,count,0));
        }

        public void OnRequestTile(string codeValue, int id,int count,int colorID)
        {
            EventHandler<IdentificationArgs> tileStateRequest = TileStateRequest;
            if (tileStateRequest != null)
                tileStateRequest(tileStateRequest, new IdentificationArgs(codeValue, id, count,colorID));
        }
        #endregion

        #region Item Events

        public void OnAddItem(string codeValue, int id, int count,int colorID)
        {
            EventHandler<IdentificationArgs> itemStateAdd = ItemStateAdd;
            if (itemStateAdd != null)
                itemStateAdd(itemStateAdd, new IdentificationArgs(codeValue, id, count,colorID));
        }

        public void OnRequestItem(string codeValue, int id, int count,int colorID)
        {
            EventHandler<IdentificationArgs> itemStateRequest = ItemStateRequest;
            if (itemStateRequest != null)
                itemStateRequest(itemStateRequest, new IdentificationArgs(codeValue, id, count,colorID));
        }

        public void OnUpdateItem(Item item,string playerID)
        {
            EventHandler<ItemStateChangedArgs> itemStateUpdated = ItemStateUpdated;
            if (itemStateUpdated != null)
                itemStateUpdated(itemStateUpdated, new ItemStateChangedArgs(item, playerID, TileGrid.TileWidth,0));
        }

        public void UpdateItem(int Id, string playerID,Vector2 location)
        {
            foreach (Item item in items)
                if (item.ID == Id && playerID != player.playerTurn)
                {
                    item.SnappedToForm = false;
                    item.Location = location;
                    //item.RotationValue = rotation;
                    break;
                }
        }

        #endregion

        public void rotateManually(float rotationValue,int ID, string playerID, string type)
        {
            if (player.playerTurn != playerID)
            {
                if (type == "item")
                    getItemFromList(ID).RotationValue = rotationValue;
                else if (type == "tile")
                    getTileFromList(ID).RotationValue = rotationValue;
            }
        }

        public void snapToGrid(int ID, string playerID,Vector2 location)
        {
            foreach (Tile tile in tiles)
                if (tile.ID == ID && player.playerTurn != playerID)
                {
                    if (this.IsHost)
                        TileGrid.OnSnapToGrid(tile,playerID);
                    tile.CheckCell();
                    break;
                }
        }

        public void removeFromGrid(int ID, string playerID,Vector2 location)
        {
            foreach (Tile tile in tiles)
                if (tile.ID == ID && player.playerTurn != playerID)
                {
                    if (this.IsHost)
                        TileGrid.OnRemoveFromGrid(tile, playerID);

                    TileGrid.mapCells[TileGrid.GetCellByPixelX((int)location.X),
                    TileGrid.GetCellByPixelY((int)location.Y)].CodeValue = "";
                    tile.OnGrid = false;

                    break;
                }
        }

        public void AddTile(Vector2 location, string owner, int ID,int tileCount)
        {
            location = AdjustNewTileLocation(location, 1);
            if (location != Vector2.Zero)
            {
                tiles.Add(new Tile(owner, new Vector2(-23, -10), deck.GetTileTexture(ID), font, location, tileCount, 0.5f - tileCount * 0.001f, frame1, Color.Black));

            }
        }

        public void AddItem(Vector2 location, string owner, int ID, int itemCount,int colorID)
        {
             location = AdjustNewItemLocation(location, 1);
            if (location != Vector2.Zero)
            {
                items.Add(new Item(owner, new Vector2(-23, -10), deck.GetSoldier(), font, location, itemCount, 0.4f - itemCount * 0.001f, deck.GetSoldier(), 55f * Camera.Scale, player.PlayerColor(colorID), true));
            }
        }

        #endregion

        #region Helper Methods

        public Tile getTileFromList(int x)
        {
            foreach (Tile tile in tiles)
                if (tile.ID == x)
                    return tile;

            return tiles[0]; //update later
        }

        public Item getItemFromList(int x)
        {
            foreach (Item item in items)
                if (item.ID == x)
                    return item;

            return items[0]; //update later
        }


        public void AddTile(Vector2 location, string owner)
        {
            
            location = AdjustNewTileLocation(location, 1);
            if (location != Vector2.Zero)
            {
                if (this.IsHost)
                {
                    int deckX = deck.GetRandomTile();
                    Id++;
                    tiles.Add(new Tile(owner, new Vector2(-23, -10), deck.GetTileTexture(deckX), font, location, Id, 0.5f - Id * 0.001f, frame1, Color.Black));
                    OnAddTile(owner,deckX,Id);
            
                }
                else
                    OnRequestTile(owner,0,0,0);
            }
        }

        public void AddSoldier(Vector2 location, string owner,int colorID)
        {

            location = AdjustNewItemLocation(location, 1);
            if (location != Vector2.Zero)
            {
                if (this.IsHost)
                {
                    itemID++;
                    items.Add(new Item(owner, new Vector2(-23, -10), deck.GetSoldier(), font, location, itemID, 0.4f - itemID * 0.001f, deck.GetSoldier(), 55f * Camera.Scale, player.PlayerColor(colorID), true));
                    OnAddItem(owner, 0, itemID,colorID);
                }
                else
                    OnRequestItem(owner, 0, 0, player.ActivePlayerID);
            }
        }

        public void AddScoreBoardSoldier(Vector2 location, string owner)
        {
            itemID++;
            items.Add(new Item(owner, new Vector2(-23, -10), deck.GetSoldier(), font, location,
                    itemID, 0.05f - itemID * 0.001f, deck.GetSoldier(), 55f, player.PlayerColor(player.activePlayers - 1), false));
            items[itemID - 1].LockedInBounds = true;
            items[itemID - 1].BoundSize = FormManager.menu.FormSize;
            items[itemID - 1].Bounds = Camera.WorldLocation + FormManager.menu.Location + new Vector2(TileGrid.OriginalTileWidth / 2, 0);
            items[itemID - 1].CalculateMenuOffSet();
            // item.Move(FormManager.menu.Step);
        }

        public Vector2 AdjustNewTileLocation(Vector2 location, int reps)
        {
            for (int x = tiles.Count - 1; x >= 0; x--)
                if (tiles[x].TileRectangle.Intersects(new Rectangle((int)location.X, (int)location.Y, TileGrid.OriginalTileWidth, TileGrid.OriginalTileHeight / 10))
                    && tiles[x].SnappedToForm)
                {
                    AdjustNewTileLocation(location += new Vector2(0, TileGrid.OriginalTileHeight), reps++);
                    x = tiles.Count;
                }
            if (reps >= 6)
                return Vector2.Zero;
            else
                return location;
        }

        public Vector2 AdjustNewItemLocation(Vector2 location, int reps)
        {
            for (int x = items.Count - 1; x >= 0; x--)
                if (items[x].TileRectangle.Intersects(new Rectangle((int)location.X, (int)location.Y, 15, 15))
                    && items[x].SnappedToForm)
                {
                    AdjustNewItemLocation(location += new Vector2(0, 65), reps++);
                    x = items.Count;
                }
            if (reps >= 8)
                return Vector2.Zero;
            else
                return location;
        }

        public void AdjustToForm()
        {
            foreach (Tile tile in tiles)
            {
                if (tile.SnappedToForm)
                    tile.AdjustToForm();
            }

            foreach (Item item in items)
            {
                if (item.SnappedToForm)
                    item.AdjustToForm();

            }

        }

        public void AdjustToMenu()
        {
            foreach (Item item in items)
                if (item.LockedInBounds)
                {
                    item.Bounds = Camera.WorldLocation + FormManager.menu.Location + new Vector2(TileGrid.OriginalTileWidth / 2, 0);
                    item.Move(FormManager.menu.Step);
                    item.AdjustLocationToOrigin();

                }
        }

        public void LockTiles()
        {

            bool lockEverything = false;

            foreach (Tile tile in tiles)

                if (!tile.Lock)
                {
                    lockEverything = true;
                    break;
                }

            foreach (Item item in items)

                if (!item.Lock)
                {
                    lockEverything = true;
                    break;
                }

            if (lockEverything)
            {
                foreach (Tile tile in tiles)

                    if (!tile.Lock)
                    {
                        tile.Lock = true;
                    }

                foreach (Item item in items)

                    if (!item.Lock)
                    {
                        item.Lock = true;
                    }

            }
            else
            {
                foreach (Tile tile in tiles)

                    tile.Lock = false;

                foreach (Item item in items)

                    item.Lock = false;

            }

        }

        public void LockAllTiles()
        {
            foreach (Tile tile in tiles)
                tile.Lock = true;

            foreach (Item item in items)

                item.Lock = true;
        }

        public void NewActiveTile(int x)
        {
            if (player.ActiveTileType == "tile")
                tiles[player.ActiveTileID].ActiveTile = false;
            player.ActiveTileType = "tile";
            player.RotatingType = "tile";
            tiles[x].ActiveTile = true;
            player.ActiveTileID = x;
            player.ActiveTile = true;
        }

        public void NewActiveItem(int x)
        {
            if (player.ActiveTileType == "item")
                items[player.ActiveTileID].ActiveTile = false;
            player.ActiveTileType = "item";
            player.RotatingType = "item";
            items[x].ActiveTile = true;
            player.ActiveTileID = x;
            player.ActiveTile = true;
        }

        public void AdjustTileLocation(float newScale, float scale)
        {
            foreach (Tile tile in tiles)
            {
                if (!tile.Moving)
                /*  if (tile.OnGrid)
                  {
                      tile.Location = new Vector2((float)TileGrid.GetCellByPixelX((int)tile.Location.X) * newScale
                                                  , (float)TileGrid.GetCellByPixelX((int)tile.Location.Y) * newScale);
                      tile.Location += new Vector2(TileGrid.TileWidth / 2, TileGrid.TileHeight / 2);
                  }
                  else*/
                {
                    Vector2 tempPos = tile.Location - (new Vector2((float)TileGrid.GetCellByPixelX((int)tile.Location.X) * scale
                                        , (float)TileGrid.GetCellByPixelX((int)tile.Location.Y) * scale));
                    tile.Location = new Vector2((float)TileGrid.GetCellByPixelX((int)tile.Location.X) * newScale
                                                , (float)TileGrid.GetCellByPixelX((int)tile.Location.Y) * newScale) + tempPos * newScale / scale;
                }

            }
        }


        public void AdjustItemLocation(float newScale, float scale)
        {
            foreach (Item item in items)
            {
                item.Width *= newScale / scale;
                if (!item.Moving)
                {
                    Vector2 tempPos = item.Location - (new Vector2((float)TileGrid.GetCellByPixelX((int)item.Location.X) * scale
                                        , (float)TileGrid.GetCellByPixelX((int)item.Location.Y) * scale));
                    item.Location = new Vector2((float)TileGrid.GetCellByPixelX((int)item.Location.X) * newScale
                                                , (float)TileGrid.GetCellByPixelX((int)item.Location.Y) * newScale) + tempPos * newScale / scale;
                }

            }
        }

        public void RotateTileOrItem()
        {
            if (player.RotatingType == "tile")
                tiles[player.ActiveTileID].RotateThis();
            else if (player.RotatingType == "item")
                items[player.ActiveTileID].RotateThis();
        }

        public void UnlockAnObject()
        {

            if (player.UnlockObject)
            {
                for (int x = items.Count - 1; x >= 0; x--)
                    if (items[x].MouseClick && items[x].Lock)
                    {
                        items[x].Lock = false;
                        items[x].Moving = true;
                        player.UnlockObject = false;
                        //buttonManager.buttons[4].CodeValue = "Unlock " + player.UnlockObject;
                        items[x].Start = items[x].MouseLocation;
                        NewActiveItem(x);
                        break;
                    }
            }
            if (player.UnlockObject)
            {
                for (int x = tiles.Count - 1; x >= 0; x--)
                    if (tiles[x].MouseClick && tiles[x].Lock)
                    {
                        tiles[x].Lock = false;
                        tiles[x].Moving = true;
                        player.UnlockObject = false;
                        //buttonManager.buttons[4].CodeValue = "Unlock " + player.UnlockObject;
                        tiles[x].Start = tiles[x].MouseLocation;
                        NewActiveTile(x);
                        break;
                    }

            }
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {

            AdjustToForm();
            UnlockAnObject();
            RotateTileOrItem();
            player.ResetActiveTile();

            if (player.ActiveTileType != "tile")
                UpdateItems(gameTime);

            if (player.ActiveTileType != "item")
            {
                if (!player.ActiveTile)
                {
                    for (int x = tiles.Count - 1; x >= 0; x--)
                    {
                        if (!tiles[x].OnGrid || tiles[x].SnappedToForm)
                        {
                            tiles[x].Update(gameTime);

                        }
                        if (tiles[x].Moving)
                        {
                           NewActiveTile(x);
                            
                            break;

                        }
                    }
                }

                if (player.ActiveTile && player.ActiveTileType == "tile")
                {
                    tiles[player.ActiveTileID].Update(gameTime);
                    if (!tiles[player.ActiveTileID].Moving)
                    {
                        player.ActiveTile = false;
                    }
                }
                else
                {
                    for (int x = tiles.Count - 1; x >= 0; x--)
                    {
                        if (x == player.ActiveTileID && player.ActiveTileType == "tile")
                            tiles[x].ActiveTile = true;
                        else
                        {
                            tiles[x].ActiveTile = false;
                        }
                        
                        tiles[x].Update(gameTime);
                        if (tiles[x].Moving)
                        {
                            NewActiveTile(x);
                            break;

                        }

                    }
                }
            }
            HandleUpdateEvents();
        }

        public void HandleUpdateEvents()
        {
            if (player.ActiveTileType == "tile")
                OnUpdateTile(tiles[player.ActiveTileID],player.playerTurn); //Event
            if (player.ActiveTileType == "item")
                OnUpdateItem(items[player.ActiveTileID],player.playerTurn); //Event
        }

        public void UpdateItems(GameTime gameTime)
        {
            if (!player.ActiveTile)
            {
                for (int x = items.Count - 1; x >= 0; x--)
                {
                    if (items[x].SnappedToForm || items[x].LockedInBounds)
                        items[x].Update(gameTime);

                    if (items[x].Moving)
                    {
                        NewActiveItem(x);
                        break;

                    }
                }
            }

            if (player.ActiveTile && player.ActiveTileType == "item")
            {
                items[player.ActiveTileID].Update(gameTime);
                if (!items[player.ActiveTileID].Moving)
                    player.ActiveTile = false;
            }
            else
            {
                for (int x = items.Count - 1; x >= 0; x--)
                {
                    if (x == player.ActiveTileID && player.ActiveTileType == "item")
                        items[x].ActiveTile = true;
                    else
                        items[x].ActiveTile = false;

                    items[x].Update(gameTime);
                    if (items[x].Moving)
                    {
                        NewActiveItem(x);
                        break;
                    }
                }
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
                tile.Draw(spriteBatch);

            foreach (Item item in items)
                item.Draw(spriteBatch);
        }

        #endregion
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using TileEngine;

namespace Carcassonne
{
    static class TileManager
    {
        static private Random rand = new Random();
        static private SpriteFont pericles10;

        public static bool showRandomTile = false;
        public static List<Actor> BoxTiles = new List<Actor>();
        public static Actor TileSpawner;
        public static Actor commitTile;
        public static Actor freezeTile;
        public static bool[,] AvailabilityMap =
           new bool[TileGrid.MapWidth, TileGrid.MapHeight];
        private static float timeSinceLastGeneration = 2.0f;
        private static float minTimeToGeneration = 0.7f;
        private static float timeSinceLastFreeze = 2.0f;
        private static float minTimeToFreeze = 0.2f;
        public static bool dragging = false;
        private static int dragID = 99999;
   //     public static RotatingTile rotation;

        static public void Initialize(Vector2 TileSpawnerLocation, Vector2 CommitLocation, Vector2 FreezeLocation,SpriteFont pericles)
        {
            TileSpawner = new Actor(0, 0, 2, "", TileSpawnerLocation, true);
            commitTile = new Actor(0, 0, 2, "", CommitLocation, true);
            freezeTile = new Actor(0, 0, 2, "", FreezeLocation, true);
            pericles10 = pericles;

            ResetAvailabilityMap();
        }

        static public void AddBoxTile(int ID, string code, Vector2 mousePosition)
        {
            BoxTiles.Add(new Actor(0, 0, ID, code, mousePosition, false));
        }

        static public void GenerateNewTile(Vector2 mousePos,Vector2 worldLocation, string ID)
        {
            if (TileSpawner.MouseOver(mousePos))
           {
                AddBoxTile(rand.Next(2, 3), ID, mousePos + worldLocation); //TileGrid.TilesPerRow
                timeSinceLastGeneration = 0.0f;
            }
        }


        /*  static public void MouseOverTileGenerator(Vector2 mousePos)
          {
              if (new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1).Intersects
                    (TileGrid.ExactScreenRectangle(TileSpawner.position))
                    && !showRandomTile)
                  mouseOverSpawner = true;
              else
                  mouseOverSpawner = false;
          }
          */

        /*     static public void updateTilePosition(Vector2 mousePos, string ID)
             {
                 foreach (Tile tile in BoxTiles)
                     if (tile.checkID(ID))
                     {
                         tile.position = MouseCenter(mousePos);

                     }
             }
         * */

        /*    static public bool ReadyToCommit
            {
                get { return readyToCommit; }
                set { readyToCommit = value; }
            }

            static public bool ReadyToDrag
            {
                get { return readyToDrag; }
                set { readyToDrag = value; }
            }
            */
        static public void AdjustTileLocation(string ID,float newScale)
        {
            foreach (Actor actor in BoxTiles)
                if (actor.checkID(ID))
                    actor.position = new Vector2((float)TileGrid.GetCellByPixelX((int)actor.position.X) * newScale
                                                , (float)TileGrid.GetCellByPixelX((int)actor.position.Y) * newScale);
        }


        static public void ResetAvailabilityMap()
        {
            for (int i = 0; i < TileGrid.MapWidth; i++)
            {
                for (int j = 0; j < TileGrid.MapHeight; j++)
                    AvailabilityMap[i, j] = true;
            }
        }

        /*  static public void CommitTile(Vector2 mousePos, string ID)
          {
              for (int x = BoxTiles.Count - 1; x >= 0; x--)

                  if (BoxTiles[x].checkID(ID))
                  {
                      if (ReadyToCommit)
                      {
                          if (new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1).Intersects
                            (ExactScreenRectangle(commitTile.position)))
                          {
                              mapCells[CommitPos[0], CommitPos[1]].LayerTiles[1] = BoxTiles[x].LayerTiles[2];
                              mapCells[CommitPos[0], CommitPos[1]].rotation = BoxTiles[x].rotation;
                              //mapCells[CommitPos[0], CommitPos[1]].CodeValue = ID;
                              mapCells[CommitPos[0], CommitPos[1]].Passable = false;
                              BoxTiles.RemoveAt(x);
                              ShowRandomTile = false;
                              ReadyToCommit = false;
                              ReadyToDrag = false;

                          }
                      }
                  }

          }
          */

        /*     static public bool CheckTileMouseIntersection(Vector2 mousePos, string ID)
             {
                 for (int x = BoxTiles.Count - 1; x >= 0; x--)

                     if (BoxTiles[x].checkID(ID))
                     {
                         if (new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1).Intersects
                             (ExactScreenRectangle(BoxTiles[x].position)))
                             return true;
                     }
                 return false;

             }
             */



        /*       static public float SpawnerTransparency()
               {
                   if (showRandomTile)
                       return 0.1f;
                   else if (!mouseOverSpawner)
                       return 0.7f;
                   else
                       return 1f;
               }

               static public float CommitTransparency()
               {
                   if (ReadyToCommit)
                       return 1f;
                   else
                       return 0.1f;
               }
       */

        /*   static public void PlaceTile(Vector2 mousePos, string ID)
           {
               mousePos += Camera.Position;
               for (int x = BoxTiles.Count - 1; x >= 0; x--)

                   if (BoxTiles[x].checkID(ID))
                   {


                       if ((mapCells[GetCellByPixelX((int)mousePos.X), GetCellByPixelY((int)mousePos.Y)].LayerTiles[1]) == 0)
                       {
                           BoxTiles[x].position = GetCellLocation(mousePos);
                           CommitPos[0] = GetCellByPixelX((int)mousePos.X);
                           CommitPos[1] = GetCellByPixelY((int)mousePos.Y);
                           // mapCells[CommitPos[0], CommitPos[1]].LayerTiles[1] = BoxTiles[x].LayerTiles[2];
                           //  mapCells[CommitPos[0], CommitPos[1]].rotation = BoxTiles[x].rotation;
                           ReadyToCommit = true;

                       }
                       ReadyToDrag = true;

                   }
               //  ShowRandomTile = false;

           }
           */



        /*   static public bool CanGenerateTile(Vector2 mousePos)
           {
               return (ExactScreenRectangle(mousePos).Intersects
                    (ExactScreenRectangle(TileSpawner.position))
                    && !showRandomTile);

           }
         * /
           static public void rotatePiece(string ID)
           {
               if (!rotating)
               {
                   foreach (Tile tile in BoxTiles)
                       if (tile.checkID(ID))
                       {
                           rotation = new RotatingTile(true, tile.rotation);
                           rotating = true;
                       }
               }
           }





           static public bool ShowRandomTile
           {
               get { return showRandomTile; }
               set { showRandomTile = value; }
           }

         * /
         * //**/
        //Rotation Temp code
        /*
        if (mouseState.LeftButton == ButtonState.Pressed
            && TileGrid.ReadyToCommit)
        {
            if (TileGrid.CheckTileMouseIntersection(new Vector2(mouseState.X, mouseState.Y)
                                                    + worldLocation, ID))
                //TileGrid.rotatePiece(ID);
        }
        */

        #region Update

        static public void Update(GameTime gameTime, Vector2 worldLocation, string ID)
        {
            var mouseState = Mouse.GetState();
            Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y) + worldLocation;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastGeneration = MathHelper.Min(timeSinceLastGeneration+elapsed,6.0f);
            timeSinceLastFreeze = MathHelper.Min(timeSinceLastFreeze+elapsed,6.0f);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if(timeSinceLastGeneration>minTimeToGeneration && !dragging)
                GenerateNewTile(mousePos-worldLocation,worldLocation, ID);

                if (commitTile.MouseOver(mousePos - worldLocation) && !dragging)
                {
                    foreach (Actor actor in BoxTiles)
                        if (actor.checkID(ID))
                        {
                            actor.commitTile();
                        }
                }

                if (freezeTile.MouseOver(mousePos - worldLocation) && !dragging
                    && timeSinceLastFreeze>minTimeToFreeze)
                {
                    timeSinceLastFreeze = 0.0f;
                    bool temp = false;
                    foreach (Actor actor in BoxTiles)
                        if (actor.checkID(ID))
                        {
                            if (!actor.Freeze)
                            {
                                temp = true;
                                break;
                            }
                        }

                    if (temp)
                    {
                        foreach (Actor actor in BoxTiles)
                            if (actor.checkID(ID))
                            {
                                if (!actor.Freeze)
                                {
                                    actor.ToggleFreeze();
                                }
                            }
                    }
                    else
                    {
                        foreach (Actor actor in BoxTiles)
                            if (actor.checkID(ID))
                            {
                                actor.ToggleFreeze();
                            }
                    }
                }


               for (int x = BoxTiles.Count - 1; x >= 0; x--)
                    if (BoxTiles[x].checkID(ID))
                    {
                        if (BoxTiles[x].MouseOver(mousePos) || dragging)
                            if (!dragging || dragID==x)
                            {
                                BoxTiles[x].updateTilePosition(mousePos);
                                dragging=true;
                                dragID = x;
                                break;
                            }
                    }
                                
            }

            if (mouseState.LeftButton != ButtonState.Pressed)
            {
                dragging = false;
               // dragID=9999;

                          

                for (int x = BoxTiles.Count - 1; x >= 0; x--)
                    if (BoxTiles[x].checkID(ID))
                    {
                        ResetAvailabilityMap();
                        for (int j = BoxTiles.Count - 1; j >= 0; j--)
                        {
                            if (x!=j)
                            AvailabilityMap[TileGrid.GetCellByPixelX((int)BoxTiles[j].position.X),
                                      TileGrid.GetCellByPixelY((int)BoxTiles[j].position.Y)] = false;
                        }

                        BoxTiles[x].tilePlacemenet(mousePos);
                    }
            }

            for (int x = BoxTiles.Count - 1; x >= 0; x--)
                if (BoxTiles[x].checkID(ID))
                    if (BoxTiles[x].Inactive)
                        BoxTiles.RemoveAt(x);


      

            TileSpawner.TransparencyHandler(mousePos - worldLocation);
            commitTile.TransparencyHandler(mousePos-worldLocation);
            freezeTile.TransparencyHandler(mousePos - worldLocation);
            
        }
        #endregion



        #region Drawing

        static public void Draw(SpriteBatch spriteBatch)
        {
          for (int x = BoxTiles.Count - 1; x >= 0; x--)
          {
             for (int i = 0; i < TileGrid.MapLayers; i++)
                {
                 if(x==dragID)
                 {
                    spriteBatch.Draw(
                              TileGrid.tileSheet,
                              TileGrid.ExactScreenRectangle(Camera.WorldToScreen(BoxTiles[x].position)),
                              TileGrid.TileSourceRectangle(BoxTiles[x].LayerTiles[i]),
                              Color.White,
                              BoxTiles[x].Rotation,
                              Vector2.Zero,
                              SpriteEffects.None,
                              0.1f );
                 }
                 else
                    spriteBatch.Draw(
                              TileGrid.tileSheet,
                              TileGrid.ExactScreenRectangle(Camera.WorldToScreen(BoxTiles[x].position)),
                              TileGrid.TileSourceRectangle(BoxTiles[x].LayerTiles[i]),
                              Color.White,
                              BoxTiles[x].Rotation,
                              Vector2.Zero,
                              SpriteEffects.None,
                              1f - ((float)i * 0.1f ));

                }
              if(!BoxTiles[x].Freeze)
                spriteBatch.DrawString(
                pericles10,
                BoxTiles[x].CodeValue,
                Camera.WorldToScreen(BoxTiles[x].position),
                Color.Red);
            }

            




            spriteBatch.Draw(
                TileGrid.tileSheet,
                TileGrid.ExactScreenRectangle(TileSpawner.position),
                TileGrid.TileSourceRectangle(TileSpawner.LayerTiles[2]),
                Color.White * TileSpawner.Transparency,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                1f - ((float)2 * 0.1f));


            spriteBatch.Draw(
                 TileGrid.tileSheet,
                 TileGrid.ExactScreenRectangle(commitTile.position),
                 TileGrid.TileSourceRectangle(commitTile.LayerTiles[2]),
                 Color.White * commitTile.Transparency,
                 0.0f,
                 Vector2.Zero,
                 SpriteEffects.None,
                 1f - ((float)2 * 0.1f));

            spriteBatch.Draw(
             TileGrid.tileSheet,
             TileGrid.ExactScreenRectangle(freezeTile.position),
             TileGrid.TileSourceRectangle(freezeTile.LayerTiles[2]),
             Color.White * freezeTile.Transparency,
             0.0f,
             Vector2.Zero,
             SpriteEffects.None,
             1f - ((float)2 * 0.1f));

        }
        #endregion

    }

}

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
        public static List<RotatingTile> RotatingTiles = new List<RotatingTile>();
        public static Actor TileSpawner;
       // public static Actor commitTile;
        public static Actor freezeTile;
        public static bool[,] AvailabilityMap =
           new bool[TileGrid.MapWidth, TileGrid.MapHeight];
        private static float timeSinceLastGeneration = 2.0f;
        private static float minTimeToGeneration = 0.7f;
        private static float timeSinceLastFreeze = 2.0f;
        private static float minTimeToFreeze = 0.2f;
        public static bool dragging = false;
        private static bool activeRotation;
        private static int dragID = 99999;
        private static int rotatingID = 99999;
   //     public static RotatingTile rotation;

        static public void Initialize(Vector2 TileSpawnerLocation, 
                                    // Vector2 CommitLocation,
                                     Vector2 FreezeLocation,SpriteFont pericles)
        {
            TileSpawner = new Actor(0, 0, 2, "", TileSpawnerLocation, true);
          //  commitTile = new Actor(0, 0, 2, "", CommitLocation, true);
            freezeTile = new Actor(0, 0, 2, "", FreezeLocation, true);
            pericles10 = pericles;
            activeRotation = false;
            ResetAvailabilityMap();
        }

        static public void AddBoxTile(int ID, string code, Vector2 mousePosition)
        {
            BoxTiles.Add(new Actor(0, 0, ID, code, mousePosition, false));
        }

        static public void AddRotatingTile(string code, bool clockWise)
        {
            if (!activeRotation)
                for (int x = BoxTiles.Count - 1; x >= 0; x--)
                    if (BoxTiles[x].checkID(code) && x == dragID && !BoxTiles[x].Freeze)
                    {
                        RotatingTiles.Add(new RotatingTile(BoxTiles[x].LayerTiles[0],
                                                              BoxTiles[x].LayerTiles[1],
                                                              BoxTiles[x].LayerTiles[2],
                                                              code, BoxTiles[x].position, clockWise,
                                                              BoxTiles[x].Rotation));
                        activeRotation = true;
                        BoxTiles[x].Visible = false;
                        rotatingID = dragID;
                    }
        }


        static public void GenerateNewTile(Vector2 mousePos,Vector2 worldLocation, string ID)
        {
            if (TileSpawner.MouseOverReal(mousePos))
           {
                AddBoxTile(rand.Next(3, 4), ID, mousePos + worldLocation); //TileGrid.TilesPerRow
                timeSinceLastGeneration = 0.0f;
            }
        }


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

               /* if (commitTile.MouseOverReal(mousePos - worldLocation) && !dragging)
                {
                    foreach (Actor actor in BoxTiles)
                        if (actor.checkID(ID))
                        {
                            actor.commitTile();
                        }
                }*/
                

                if (freezeTile.MouseOverReal(mousePos - worldLocation) && !dragging
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
                        if ((BoxTiles[x].MouseOver(mousePos) || dragging) && !BoxTiles[x].Available) //get the available tile first
                            if (!dragging || dragID==x)
                            {
                                BoxTiles[x].updateTilePosition(mousePos);
                                dragging=true;
                                dragID = x;
                                break;
                            }
                    }

               for (int x = BoxTiles.Count - 1; x >= 0; x--)
                   if (BoxTiles[x].checkID(ID))
                   {
                       if (BoxTiles[x].MouseOver(mousePos) || dragging) //get any other tile if available tile wasnt found
                           if (!dragging || dragID == x)
                           {
                               BoxTiles[x].updateTilePosition(mousePos);
                               dragging = true;
                               dragID = x;
                               break;
                           }
                   }

                                
            }
            if(!activeRotation)
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
                            AvailabilityMap[TileGrid.GetCellByPixelX((int)BoxTiles[j].position.X+TileGrid.TileWidth/2),
                                      TileGrid.GetCellByPixelY((int)BoxTiles[j].position.Y + TileGrid.TileHeight / 2)] = false;
                        }

                        BoxTiles[x].tilePlacemenet(mousePos);
                    }
            }



            for (int x = RotatingTiles.Count - 1; x >= 0; x--)
            {

                RotatingTiles[x].UpdateRotation(BoxTiles[dragID].position);
                if (!RotatingTiles[x].Active)
                {
                    BoxTiles[rotatingID].Visible = true;
                    if (RotatingTiles[x].clockwise)
                    {
                         BoxTiles[rotatingID].Rotation =
                         BoxTiles[rotatingID].Rotation + MathHelper.Pi * 2 + MathHelper.PiOver2;
                    }
                    else
                         BoxTiles[rotatingID].Rotation =
                         BoxTiles[rotatingID].Rotation - MathHelper.Pi * 2 - MathHelper.PiOver2;

                    BoxTiles[rotatingID].RotationState = (BoxTiles[rotatingID].RotationState + 1) % 4;
                    RotatingTiles.RemoveAt(x);
                    activeRotation = false;
                }

            }

            for (int x = BoxTiles.Count - 1; x >= 0; x--)
                if (BoxTiles[x].checkID(ID))
                {

                   BoxTiles[x].CheckVisibility();

                    if (BoxTiles[x].Inactive)
                    {
                        BoxTiles.RemoveAt(x);
                    }
                }

            TileSpawner.TransparencyHandler(mousePos - worldLocation);
          //  commitTile.TransparencyHandler(mousePos-worldLocation);
            freezeTile.TransparencyHandler(mousePos - worldLocation);
            
        }
        #endregion



        #region Drawing

        static public void Draw(SpriteBatch spriteBatch)
        {

          foreach (RotatingTile rotatingtile in RotatingTiles)
                rotatingtile.Draw(spriteBatch);

          for (int x = BoxTiles.Count - 1; x >= 0; x--)
          {
              for (int i = 0; i < TileGrid.MapLayers; i++)
              {
                  float layer = 0.4f;
                  if (x == dragID && !BoxTiles[x].Freeze)
                  {

                      if (BoxTiles[x].Locked)
                          layer = 0.3f;
                      else
                          layer = 0.1f;

                      spriteBatch.Draw(
                       TileGrid.tempTile,
                       Camera.WorldToScreen(BoxTiles[x].position + new Vector2(TileGrid.TileWidth / 2, TileGrid.TileHeight / 2)),
                       null,
                       Color.CornflowerBlue * BoxTiles[x].Transparency,
                       BoxTiles[x].Rotation,
                       TileGrid.TileSourceCenter(0),
                       ((float)TileGrid.TileWidth / (float)TileGrid.OriginalTileWidth),
                       SpriteEffects.None,
                       layer);
                      /*    spriteBatch.Draw(
                                    TileGrid.tileSheet,
                                    TileGrid.ExactScreenRectangle(Camera.WorldToScreen(BoxTiles[x].position)),
                                    TileGrid.TileSourceRectangle(BoxTiles[x].LayerTiles[i]),
                                    Color.CornflowerBlue * BoxTiles[x].Transparency,
                                    BoxTiles[x].Rotation,
                                    Vector2.Zero,
                                    SpriteEffects.None,
                                    0.1f );
                       * */
                  }
                  else
                  {

                      if (!BoxTiles[x].Available)
                          layer = 0.2f;
            

                          spriteBatch.Draw(
                            TileGrid.tempTile,
                            Camera.WorldToScreen(BoxTiles[x].position + new Vector2(TileGrid.TileWidth / 2, TileGrid.TileHeight / 2)),
                            null,
                            Color.White * BoxTiles[x].Transparency,
                            BoxTiles[x].Rotation,
                            TileGrid.TileSourceCenter(0),
                            ((float)TileGrid.TileWidth / (float)TileGrid.OriginalTileWidth),
                            SpriteEffects.None,
                            layer);
                      /*      spriteBatch.Draw(
                                      TileGrid.tileSheet,
                                      TileGrid.ExactScreenRectangle(Camera.WorldToScreen(BoxTiles[x].position)),
                                      TileGrid.TileSourceRectangle(BoxTiles[x].LayerTiles[i]),
                                      Color.White * BoxTiles[x].Transparency,
                                      BoxTiles[x].Rotation,
                                      Vector2.Zero,
                                      SpriteEffects.None,
                                      1f - ((float)i * 0.1f ));*/
                  }

              }
              if (!BoxTiles[x].Freeze)
                  spriteBatch.DrawString(
                  pericles10,
                  BoxTiles[x].CodeValue,
                  Camera.WorldToScreen(BoxTiles[x].position),
                  Color.Red);
          }

            




            spriteBatch.Draw(
                TileGrid.tileSheet,
                TileGrid.ExactScreenOriginalRectangle(TileSpawner.position),
                TileGrid.TileSourceRectangle(TileSpawner.LayerTiles[2]),
                Color.White * TileSpawner.Transparency,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.02f);


        /*    spriteBatch.Draw(
                 TileGrid.tileSheet,
                 TileGrid.ExactScreenOriginalRectangle(commitTile.position),
                 TileGrid.TileSourceRectangle(commitTile.LayerTiles[2]),
                 Color.White * commitTile.Transparency,
                 0.0f,
                 Vector2.Zero,
                 SpriteEffects.None,
                 1f - (2 * 0.1f));*/

            spriteBatch.Draw(
             TileGrid.tileSheet,
             TileGrid.ExactScreenOriginalRectangle(freezeTile.position),
             TileGrid.TileSourceRectangle(freezeTile.LayerTiles[2]),
             Color.White * freezeTile.Transparency,
             0.0f,
             Vector2.Zero,
             SpriteEffects.None,
              0.02f);

            spriteBatch.DrawString(
            pericles10,
            "Tiles" ,
            new Vector2(Camera.ViewPortWidth - 100, 70),
            Color.Red * TileSpawner.Transparency);


            spriteBatch.DrawString(
            pericles10,
            "Lock",
            new Vector2(Camera.ViewPortWidth - 100, 170),
            Color.Red * freezeTile.Transparency);

        }
        #endregion

    }

}

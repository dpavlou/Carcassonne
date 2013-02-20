using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileEngine;

namespace Carcassonne
{
    public class Form
    {

        #region Declarations

       private Texture2D formTexture;
       private Vector2 formSize;
       private Vector2 defaultLocation;
       private Vector2 currentLocation;
       private SpriteFont font;
       private Button handle;
       private Vector2 start;
       private Vector2 text1Offset = new Vector2(100, 100); //temp
       private Vector2 text2Offset = new Vector2(200, 300); //temp
       private Vector2 text3Offset = new Vector2(100, 600); //temp
       private MouseState mouseState;
       private bool dragging;

        #endregion

       #region Constructor

       public Form(Texture2D texture, Vector2 size,SpriteFont font, Vector2 location)
       {
           formTexture = texture;
           this.font=font;
           formSize = size;
           currentLocation = defaultLocation = location;
           dragging = false;
           handle = new Button("MENU", new Vector2(-45, -10), texture, font, new Vector2(location.X, Camera.ViewPortHeight / 2 - TileGrid.TileHeight / 2), 1, 0.05f, "Menu");
       }

        #endregion

        #region Properties

       public Vector2 Location
       {
           get { return currentLocation; }
           set 
           {
               currentLocation.X = MathHelper.Clamp(value.X, defaultLocation.X - formSize.X, defaultLocation.X);
               
           }
       }

       public Vector2 ButtonLocation
       {
           get { return Location - new Vector2(TileGrid.TileWidth / 2, 0); }
       }

       public Vector2 MouseLocation
       {

           get { return new Vector2(mouseState.X, mouseState.Y); }

       }

       public Rectangle FormRectangle
       {
           get { return new Rectangle((int)Location.X, (int)Location.Y, (int)formSize.X, (int)formSize.Y); }
       }

        #endregion

       #region Helper Methods

       public void MoveForm()
       {
           Location += MouseLocation-start;
           start = MouseLocation;
           handle.MoveAt(new Vector2(Location.X,handle.Location.Y));
       }

       #endregion

       #region Update

       public void Update(GameTime gameTime)
       {
           mouseState = Mouse.GetState();

           handle.Update(gameTime);

           if (handle.OnMouseClick() && dragging == false)
           {
               start = MouseLocation;
               dragging = true;
           }

           if (dragging && mouseState.LeftButton == ButtonState.Pressed)
           {
               MoveForm();
           }
           else
               dragging = false;

       }

        #endregion

        #region Draw

       public void Draw(SpriteBatch spriteBatch)
       {
           spriteBatch.Draw(formTexture, FormRectangle,null,Color.White*0.5f,0.0f,Vector2.Zero,SpriteEffects.None,0.05f);
           handle.Draw(spriteBatch);

            spriteBatch.DrawString(
            font,
            "SKATA NA FAME",
            Location+text1Offset,
            Color.Black);

            spriteBatch.DrawString(
             font,
            "SKATA NA PIOUME",
            Location + text2Offset,
            Color.Black);

            spriteBatch.DrawString(
             font,
            "MESTA SKATA NA KOIMITHOUME",
            Location + text3Offset,
            Color.Black);
       }
        #endregion

    }
}

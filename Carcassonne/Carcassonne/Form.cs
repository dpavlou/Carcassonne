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
       private Texture2D ScoreBoard;
       private Vector2 formSize;
       private Vector2 defaultLocation;
       private Vector2 currentLocation;
       private SpriteFont font;
       private Button handle;
       private Vector2 start;
       private MouseState mouseState;
       private Vector2 moveAmount;
       private Vector2 previousLocation;
       private bool dragging;

        #endregion

       #region Constructor

       public Form(Texture2D texture,Texture2D scoreBoard, Vector2 size,SpriteFont font, Vector2 location)
       {
           formTexture = texture;
           this.font=font;
           formSize = size;
           currentLocation = defaultLocation = previousLocation = location;
           dragging = false;
           ScoreBoard = scoreBoard;
           handle = new Button("MENU", new Vector2(-45, -10), texture, font, new Vector2(location.X, Camera.ViewPortHeight / 2 - TileGrid.TileHeight / 2), 1, 0.05f, false);
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

       public Rectangle ScoreBoardRectangle
       {
           get { return new Rectangle((int)Location.X + 30, Camera.ViewPortHeight - 470, ScoreBoard.Width+50, ScoreBoard.Height+50); }
       }

       public bool Dragging
       {
           get { return (dragging && mouseState.LeftButton == ButtonState.Pressed); }
       }

       public Vector2 MoveAmount
       {
           get { return new Vector2(moveAmount.X,0); }
           set { moveAmount = value; }
       }

       public Vector2 Step
       {
           get { return Location - previousLocation; }
       }

       public Vector2 PreviousLocation
       {
           set { previousLocation = value; }
       }

       public Vector2 FormSize
       {
           get { return formSize; }
       }

       public Rectangle FormRectangle
       {
           get { return new Rectangle((int)Location.X, (int)Location.Y, (int)formSize.X, (int)formSize.Y); }
       }

        #endregion

       #region Helper Methods

       public void MoveForm()
       {
           previousLocation = Location;
           MoveAmount = MouseLocation - start;
           Location += MoveAmount;
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

           if(Dragging)
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
           spriteBatch.Draw(ScoreBoard, ScoreBoardRectangle, Color.White);

           handle.Draw(spriteBatch);

       }
        #endregion

    }
}

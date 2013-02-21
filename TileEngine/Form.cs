using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileEngine;

namespace TileEngine
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
       private MouseState mouseState;
       private Vector2 moveAmount;
       private Vector2 previousLocation;
       private bool dragLeft;
       private bool dragging;

        #endregion

        #region Constructor

       public Form(Texture2D texture,string buttonTag, Vector2 size,SpriteFont font, Vector2 location,bool dragleft)
       {
           formTexture = texture;
           this.font=font;
           formSize = size;
           currentLocation = defaultLocation = previousLocation = location;
           dragging = false;
           DragLeft = dragleft;

           Vector2 buttonLocation;
           if(DragLeft)
               buttonLocation =  new Vector2(location.X, Camera.ViewPortHeight / 2 - TileGrid.TileHeight / 2);
           else
               buttonLocation =  new Vector2(location.X+formSize.X, Camera.ViewPortHeight / 2 - TileGrid.TileHeight / 2);

           handle = new Button(buttonTag, new Vector2(-45, -10), texture, font, buttonLocation, 1, 0.05f, false);
       }

        #endregion

        #region Properties

       public bool DragLeft
       {
           get { return dragLeft; }
           set { dragLeft = value; }
       }

       public Vector2 Location
       {
           get { return currentLocation; }
           set 
           {
               currentLocation.X = MathHelper.Clamp(value.X, defaultLocation.X - formSize.X, defaultLocation.X);
               
           }
       }

       public Vector2 DefaultLocation
       {
           get { return defaultLocation; }
       }

       public Vector2 ButtonLocation
       {
           get
           { 
              return Location - new Vector2(TileGrid.TileWidth / 2, 0);
           }
       }

       public Vector2 MouseLocation
       {

           get { return new Vector2(mouseState.X, mouseState.Y); }

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

       public Rectangle FormWorldRectangle
       {
           get { return new Rectangle((int)Location.X + (int)Camera.WorldLocation.X, (int)Location.Y + (int)Camera.WorldLocation.Y, (int)formSize.X, (int)formSize.Y); }
       }

        #endregion

       #region Helper Methods

       public void MoveForm()
       {
           previousLocation = Location;
           MoveAmount = MouseLocation - start;
           Location += MoveAmount;
           start = MouseLocation;
           if(DragLeft)
             handle.MoveAt(new Vector2(Location.X,handle.Location.Y));
           else
              handle.MoveAt(new Vector2(Location.X+formSize.X, handle.Location.Y));

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

           handle.Draw(spriteBatch);

       }
        #endregion

    }
}

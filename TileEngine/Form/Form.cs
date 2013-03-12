using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileEngine;

namespace TileEngine.Form
{
    using TileEngine.Entity;
    using TileEngine.Camera;

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
       private bool autopilot,launchautopilot;
       private Vector2 velocity,target;
       private float timeSinceAutopilot;

        #endregion

        #region Constructor

       public Form(Texture2D texture,string buttonTag, Vector2 size,SpriteFont font, Vector2 location,bool dragleft)
       {
           formTexture = texture;
           this.font=font;
           formSize = size;
           currentLocation = defaultLocation = previousLocation = location;
           launchautopilot= autopilot = dragging = false;
           DragLeft = dragleft;

           Vector2 buttonLocation;
           if(DragLeft)
               buttonLocation =  new Vector2(location.X - texture.Width/2, Camera.ViewPortHeight / 2 - TileGrid.TileHeight / 2);
           else
               buttonLocation = new Vector2(location.X + formSize.X + -texture.Width / 2, Camera.ViewPortHeight / 2 - TileGrid.TileHeight / 2);

           handle = new Button(buttonTag, new Vector2(-45, -10), texture, font, buttonLocation, 1, 0.05f, false,Color.Red);
           target=velocity = Vector2.Zero;
           timeSinceAutopilot = 0.0f;
       }

        #endregion

        #region Properties

       public Vector2 InitialLocation
       {
           set
           {
              defaultLocation = value;
           }
       }

       public bool DragLeft
       {
           get { return dragLeft; }
           set { dragLeft = value; }
       }

       public Vector2 Velocity
       {
           get { return velocity; }
           set { velocity= value; }
       }

       public Vector2 Location
       {
           get { return currentLocation; }
           set 
           {
               currentLocation.X = MathHelper.Clamp(value.X, defaultLocation.X - formSize.X, defaultLocation.X);
               
           }
       }

       public bool LaunchAutopilot
       {
           get { return launchautopilot; }
           set { launchautopilot = value; }
       }

       public Vector2 DefaultLocation
       {
           get { return defaultLocation; }
       }

       public Vector2 Target
       {
           get { return target; }
           set { target = value; }
       }

       public Vector2 ButtonLocation
       {
           get
           {
               return Location -new Vector2(TileGrid.TileWidth / 2, 0);
           }
       }

       public Vector2 MouseLocation
       {

           get { return new Vector2(mouseState.X, mouseState.Y); }

       }
       public bool AutoPilot
       {
           get { return autopilot; }
           set { autopilot = value; }
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
           set { formSize = value; }
       }

       public Rectangle FormRectangle
       {
           get { return new Rectangle((int)Location.X, (int)Location.Y, (int)formSize.X, (int)formSize.Y); }
       }

       public Rectangle FormWorldRectangle
       {
           get { return new Rectangle((int)Location.X + (int)Camera.WorldLocation.X, (int)Location.Y + (int)Camera.WorldLocation.Y, (int)formSize.X, (int)formSize.Y); }
       }

       public bool FormOnBoundaries
       {
           get { return ((Location.X <= DefaultLocation.X - FormSize.X) || (Location.X == DefaultLocation.X )); }
       }


        #endregion

       #region Helper Methods

       public void MoveForm()
       {
           previousLocation = Location;
           if (MouseLocation != start)
               autopilot = false;
           MoveAmount = MouseLocation - start;
           CheckFormLocation();
           //if (MouseLocation.X > DefaultLocation.X - FormSize.X)
               Location += MoveAmount;
              // MouseLocation.X = DefaultLocation.X + FormSize.X;

               

           start = MouseLocation;
           if(DragLeft)
             handle.MoveAt(new Vector2(Location.X- handle.texture.Width/2,handle.Location.Y));
           else
               handle.MoveAt(new Vector2(Location.X + formSize.X + handle.texture.Width / 2, handle.Location.Y));

       }

       public void Close()
       {
           if (dragLeft)
               MoveAmount = new Vector2(formSize.X, 0);
           else
               MoveAmount = new Vector2(-formSize.X, 0);

           Location += MoveAmount;

           if (DragLeft)
               handle.MoveAt(new Vector2(Location.X - handle.texture.Width / 2, handle.Location.Y));
           else
               handle.MoveAt(new Vector2(Location.X + formSize.X + handle.texture.Width / 2, handle.Location.Y));

           CheckFormLocation();
       }

       public void MoveHandle(Vector2 moveStep)
       {
           handle.MoveAt(new Vector2(handle.Location.X+moveStep.X, handle.Location.Y));
       }

       public void CheckFormLocation()
       {
           if (DragLeft)
           {
               if (Location.X == DefaultLocation.X - FormSize.X
                   && MouseLocation.X < DefaultLocation.X - FormSize.X)
               {
                   MoveAmount = Vector2.Zero;
               
               }
           }
           else if (!DragLeft)
               if (Location.X == 0
                    && MouseLocation.X > DefaultLocation.X + FormSize.X)
               {
      
                   MoveAmount = Vector2.Zero;

               }


       }
       public void CalculateStep()
       {
           if (DragLeft && Location.X < (DefaultLocation.X - formSize.X / 2))
           {
               Velocity = new Vector2(8, 0);
               Target = new Vector2(DefaultLocation.X, 0);
           }
           else if (DragLeft && Location.X >= (DefaultLocation.X - formSize.X / 2))
           {
               Velocity = new Vector2(-8, 0);
               Target = new Vector2(DefaultLocation.X - formSize.X, 0);
           }
           else if (!DragLeft && Location.X < (DefaultLocation.X - formSize.X / 2))
           {
               Velocity = new Vector2(8, 0);
               Target = new Vector2(DefaultLocation.X, 0);
           }
           else if (!DragLeft && Location.X >= (DefaultLocation.X - formSize.X / 2))
           {
               Velocity = new Vector2(-8, 0);
               Target = new Vector2(DefaultLocation.X - formSize.X, 0);
           }
       }

       public void AutomaticMovement(float elapsed)
       {
           timeSinceAutopilot += elapsed;
           MoveAmount =Velocity
               *Vector2.Distance(Location,Target)*elapsed;
         
           Location += MoveAmount;

           if (DragLeft)
               handle.MoveAt(new Vector2(Location.X - handle.texture.Width / 2, handle.Location.Y));
           else
               handle.MoveAt(new Vector2(Location.X + formSize.X + handle.texture.Width / 2, handle.Location.Y));

           CheckFormLocation();

           if (timeSinceAutopilot>=1.0f)
               LaunchAutopilot = false;
       }

       #endregion

       #region Update

       public void Update(GameTime gameTime)
       {
           float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

           mouseState = Mouse.GetState();

           handle.Update(gameTime);

           if (handle.OnMouseClick() && dragging == false)
           {
               start = MouseLocation;
               dragging = true;
               autopilot = true;
               LaunchAutopilot = false;
           }

           if (Dragging)
           {
               MoveForm();
           }
           else
               dragging = false;

           if (autopilot && mouseState.LeftButton == ButtonState.Released)
           {
               CalculateStep();
               autopilot =false;
               LaunchAutopilot = true;
               timeSinceAutopilot = 0.0f;
               dragging = false;
           }

           if (LaunchAutopilot)
               AutomaticMovement(elapsed);
           

       }

        #endregion

        #region Draw

       public void Draw(SpriteBatch spriteBatch)
       {
           spriteBatch.Draw(formTexture, FormRectangle,null,Color.White*0.5f,0.0f,Vector2.Zero,SpriteEffects.None,0.20f);
           handle.Draw(spriteBatch);

       }
        #endregion

    }
}

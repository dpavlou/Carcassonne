using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
   public static class Camera
    {
        #region Declarations
        private static Vector2 position = Vector2.Zero;
        private static Vector2 viewPortSize = Vector2.Zero;
        private static Rectangle worldRectangle = new Rectangle(0, 0, 0, 0);
        public static Matrix transform;
        private static Vector2 centre;
        public static Viewport viewport;
        private static float zoom = 1;
        private static float rotation = 0;

        #endregion

        #region Properties

        static public Matrix Transform
        {
            get { return transform; }
        }


       
       public static float Zoom
       {
           get { return zoom; }
           set
           {
               zoom = value;
               if (zoom < 0.1f)
                   zoom = 0.1f;
           }
       }

       public static float Rotation
       {
           get { return rotation; }
           set { rotation = value; }
       }

       static public Viewport newViewPort
       {
          set { viewport = value; }
       }

       public static void Update()
       {
           centre = new Vector2(position.X,position.Y);
           transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                                               Matrix.CreateRotationZ(Rotation) *
                                              Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) *
                                              Matrix.CreateTranslation(new Vector3(viewport.Width / 3, viewport.Height / 3, 0));
       }
       

       public static Vector2 Position
        {
            get { return position; }
            set
            {
                position = new Vector2(
                    MathHelper.Clamp(value.X,
                        worldRectangle.X, worldRectangle.Width -
                        ViewPortWidth),
                    MathHelper.Clamp(value.Y,
                        worldRectangle.Y, worldRectangle.Height -
                        ViewPortHeight));
            }
        }

        public static Rectangle WorldRectangle
        {
            get { return worldRectangle; }
            set { worldRectangle = value; }
        }

        public static int ViewPortWidth
        {
            get { return (int)viewPortSize.X; }
            set { viewPortSize.X = value; }
        }

        public static int ViewPortHeight
        {
            get { return (int)viewPortSize.Y; }
            set { viewPortSize.Y = value; }
        }

        public static Rectangle ViewPort
        {
            get
            {
                return new Rectangle(
                    (int)Position.X, (int)Position.Y,
                    ViewPortWidth, ViewPortHeight);
            }
        }
        #endregion

        #region Public Methods
        public static void Move(Vector2 offset)
        {
            Position += offset;
        }

        public static bool ObjectIsVisible(Rectangle bounds)
        {
            return (ViewPort.Intersects(bounds));
        }

        public static Vector2 WorldToScreen(Vector2 worldLocation)
        {
            return worldLocation - position;
        }

        public static Rectangle WorldToScreen(Rectangle worldRectangle)
        {
            return new Rectangle(
                worldRectangle.Left - (int)position.X,
                worldRectangle.Top - (int)position.Y,
                worldRectangle.Width,
                worldRectangle.Height);
        }

        public static Vector2 ScreenToWorld(Vector2 screenLocation)
        {
            return screenLocation + position;
        }

        public static Rectangle ScreenToWorld(Rectangle screenRectangle)
        {
            return new Rectangle(
                screenRectangle.Left + (int)position.X,
                screenRectangle.Top + (int)position.Y,
                screenRectangle.Width,
                screenRectangle.Height);
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.CameraStuff;

namespace SecretProject.Class.Controls
{
    public class MouseManager
    {
        public bool IsClicked { get; set; }
        public bool IsRightClicked { get; set; }

        public bool IsClickedAndHeld { get; set; }

        public bool IsReleased { get; set; }

        public bool WasJustPressed { get; set; }

        Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } }

        Vector2 uIPosition;
        public Vector2 UIPosition { get { return uIPosition; } set { uIPosition = value; } }

        Vector2 worldMousePosition;
        public Vector2 WorldMousePosition { get { return worldMousePosition; } set { worldMousePosition = value; } }

        public MouseState MyMouse { get; set; }
        public Rectangle MouseRectangle { get; set; }
        public float RelativeMouseX { get; set; }
        public float RelativeMouseY { get; set; }
        public int YOffSet1 { get; set; } = 367;
        public int XOffSet1 { get; set; } = 647;

        public int XTileOffSet { get; set; } = 360;
        public int YTileOffSet { get; set; } = 640;
        public Camera2D Camera1 { get; set; }

        Vector2 worldPosition;

        GraphicsDevice graphicsDevice;

        public MouseManager(MouseState myMouse, Camera2D camera, GraphicsDevice graphicsDevice)
        {
            this.MyMouse = myMouse;
            IsClicked = false;
            this.Camera1 = camera;
            this.graphicsDevice = graphicsDevice;

        }

        public void Update()
        {
            IsClicked = false;
            IsRightClicked = false;
            WasJustPressed = false;
             

            MouseState oldMouse = MyMouse;
            MyMouse = Mouse.GetState();
            worldPosition = Vector2.Transform(Position, Matrix.Invert(Camera1.GetViewMatrix(Vector2.One)));

            position.X = MyMouse.Position.X ;
            position.Y = MyMouse.Position.Y;

            uIPosition.X = MyMouse.Position.X - 20;
            uIPosition.Y = MyMouse.Position.Y - 20;



            WorldMousePosition = new Vector2((int)worldPosition.X - XOffSet1, (int)worldPosition.Y - YOffSet1);
            //relativeMouseX = position.X + Camera

            MouseRectangle = new Rectangle(MyMouse.X, MyMouse.Y, 1, 1);

            if(MyMouse.LeftButton == ButtonState.Pressed)
            {
                IsReleased = false;
            }

            if(MyMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                WasJustPressed = true;
            }

            if ((MyMouse.LeftButton == ButtonState.Released) && (oldMouse.LeftButton == ButtonState.Pressed))
            {
                IsClicked = true;
                IsReleased = true;
            }

            if ((MyMouse.RightButton == ButtonState.Released) && (oldMouse.RightButton == ButtonState.Pressed))
            {
                IsRightClicked = true;
            }

            if(!IsReleased)
            {
                IsClickedAndHeld = true;
            }
            else
            {
                IsClickedAndHeld = false;
            }
        }


        public bool IsHovering(Rectangle rectangle)
        {

            if (MouseRectangle.Intersects(rectangle))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool IsHoveringTile(Rectangle rectangle)
        {
            Rectangle offSetRectange = new Rectangle((int)WorldMousePosition.X + 8,(int)WorldMousePosition.Y + 8, 1, 1);
            if (offSetRectange.Intersects(rectangle))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
        
}

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
        public MouseState myMouse;
        public Rectangle mouseRectangle;
        public bool IsClicked { get; set; }
        Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } }

        float relativeMouseX;
        float relativeMouseY;

        public Camera2D Camera;

        Vector2 worldPosition;

        GraphicsDevice graphicsDevice;

        int XOffSet = 640;
        int YOffSet = 360;



        public MouseManager(MouseState myMouse, Camera2D camera, GraphicsDevice graphicsDevice)
        {
            this.myMouse = myMouse;
            IsClicked = false;
            this.Camera = camera;
            this.graphicsDevice = graphicsDevice;

        }

        public void Update()
        {
            IsClicked = false;

            MouseState oldMouse = myMouse;
            myMouse = Mouse.GetState();
            worldPosition = Vector2.Transform(Position, Matrix.Invert(Camera.GetViewMatrix(Vector2.One)));

            position.X = myMouse.Position.X;
            position.Y = myMouse.Position.Y;
            //relativeMouseX = position.X + Camera

            mouseRectangle = new Rectangle(myMouse.X, myMouse.Y, 1, 1);

            if ((myMouse.LeftButton == ButtonState.Released) && (oldMouse.LeftButton == ButtonState.Pressed))
            {
                IsClicked = true;
            }

        }

        public bool IsHovering(Rectangle rectangle)
        {


            if (mouseRectangle.Intersects(rectangle))
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
            Rectangle offSetRectange = new Rectangle((int)worldPosition.X - XOffSet,(int)worldPosition.Y - YOffSet, 1, 1);
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

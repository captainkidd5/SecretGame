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
        public Rectangle MouseSquareCoordinateRectangle { get; set; }

        public int MouseSquareCoordinateX { get; set; }
        public int MouseSquareCoordinateY { get; set; }

        public float RelativeMouseX { get; set; }
        public float RelativeMouseY { get; set; }
        public int YOffSet1 { get; set; } = 367;
        public int XOffSet1 { get; set; } = 647;

        public int XTileOffSet { get; set; } = 360;
        public int YTileOffSet { get; set; } = 640;
        public Camera2D Camera1 { get; set; }

        public Vector2 SquarePosition { get; set; }

        private int OldScrollWheelValue;
        private int NewScrollWheelValue;

        public bool HasScrollWheelValueIncreased = false;
        public bool HasScrollWheelValueDecreased = false;

        public bool ToggleGeneralInteraction { get; set; } = false;
        public bool TogglePlantInteraction { get; set; } = false;

        Vector2 worldPosition;

        GraphicsDevice graphicsDevice;

        public MouseManager( Camera2D camera, GraphicsDevice graphicsDevice)
        {
            IsClicked = false;
            this.Camera1 = camera;
            this.graphicsDevice = graphicsDevice;

        }

        public void Update()
        {
            IsClicked = false;
            IsRightClicked = false;
            WasJustPressed = false;
            HasScrollWheelValueIncreased = false;
            HasScrollWheelValueDecreased = false;
             

            MouseState oldMouse = MyMouse;
            MyMouse = Mouse.GetState();
            ///
            OldScrollWheelValue = oldMouse.ScrollWheelValue;
            NewScrollWheelValue = MyMouse.ScrollWheelValue;

            if(NewScrollWheelValue > OldScrollWheelValue)
            {
                HasScrollWheelValueIncreased = true;
            }
            else if(NewScrollWheelValue < OldScrollWheelValue)
            {
                HasScrollWheelValueDecreased = true;
            }
            ///
            worldPosition = Vector2.Transform(Position, Matrix.Invert(Camera1.GetViewMatrix(Vector2.One)));

            position.X = MyMouse.Position.X ;
            position.Y = MyMouse.Position.Y;

            uIPosition.X = MyMouse.Position.X - 20;
            uIPosition.Y = MyMouse.Position.Y - 20;

            

            WorldMousePosition = new Vector2((int)worldPosition.X - XOffSet1, (int)worldPosition.Y - YOffSet1);
            //relativeMouseX = position.X + Camera

            MouseRectangle = new Rectangle(MyMouse.X, MyMouse.Y, 1, 1);
            MouseSquareCoordinateRectangle = new Rectangle((int)SquarePosition.X, (int)SquarePosition.Y, 16, 16);
            SquarePosition = GetMouseSquarePosition(WorldMousePosition);

            MouseSquareCoordinateX = (int)(WorldMousePosition.X / 16);
            MouseSquareCoordinateY = (int)(WorldMousePosition.Y / 16);

            if (MyMouse.LeftButton == ButtonState.Pressed)
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

        public void Draw(SpriteBatch spriteBatch, float depth)
        {      
            if(ToggleGeneralInteraction)
            {
                spriteBatch.Draw(Game1.AllTextures.CursorWhiteHand, new Vector2(WorldMousePosition.X + 6, WorldMousePosition.Y + 6), null, Color.White, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 1f);
            }
            if (TogglePlantInteraction)
            {
                spriteBatch.Draw(Game1.AllTextures.CursorPlant, new Vector2(WorldMousePosition.X + 6, WorldMousePosition.Y + 6), null, Color.White, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 1f);
            }


            

        }

        public Vector2 GetMouseSquarePosition(Vector2 mousePosition)
        {
            return new Vector2((int)(mousePosition.X / 16), (int)(mousePosition.Y / 16));
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

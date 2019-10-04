using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.TileStuff;

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
        public int XOffSet1 { get; set; } = Game1.Utility.CenterScreenX + 8;
        public int YOffSet1 { get; set; } = Game1.Utility.CenterScreenY + 8;
        

        public int XTileOffSet { get; set; } = Game1.Utility.CenterScreenX;
        public int YTileOffSet { get; set; } = Game1.Utility.CenterScreenY;
        public Camera2D Camera1 { get; set; }

        public Vector2 SquarePosition { get; set; }

        private int OldScrollWheelValue;
        private int NewScrollWheelValue;

        public bool HasScrollWheelValueIncreased = false;
        public bool HasScrollWheelValueDecreased = false;

        public bool ToggleGeneralInteraction { get; set; } = false;

        Vector2 worldPosition;

        public Rectangle WorldMouseRectangle { get; set; }

        public Texture2D MouseTypeTexture { get; set; }
        public Rectangle CursorSourceRectangle { get; set; }

        public Rectangle NormalInteractionSourceRectangle { get; set; }
        public Rectangle PlantInteractionSourceRectangle { get; set; }
        public Rectangle MiningInteractionSourceRectangle { get; set; }
        public Rectangle ChoppingInteractionSourceRectangle { get; set; }
        public Rectangle DiggingInteractionSourceRectangle { get; set; }
        public Rectangle ChatInteractionSourceRectangle { get; set; }

        public float HoldTimer { get; set; }
        public float RequiredHoldTime { get; set; }

        GraphicsDevice graphicsDevice;

        private MouseManager()
        {

        }

        public MouseManager( Camera2D camera, GraphicsDevice graphicsDevice)
        {
            IsClicked = false;
            this.Camera1 = camera;
            this.graphicsDevice = graphicsDevice;
            this.NormalInteractionSourceRectangle = new Rectangle(48, 256, 32, 32);
            this.PlantInteractionSourceRectangle = new Rectangle(16, 256, 32, 32);
            this.MiningInteractionSourceRectangle = new Rectangle(112, 256, 32, 32);
            this.ChoppingInteractionSourceRectangle = new Rectangle(144, 256, 32, 32);
            this.DiggingInteractionSourceRectangle = new Rectangle(80, 256, 32, 32);
            this.ChatInteractionSourceRectangle = new Rectangle(176, 256, 32, 32);
            //this.MouseTypeTexture = Game1.AllTextures.CursorWhiteHand;
            this.RequiredHoldTime = .5f;
        }

        public void Update(GameTime gameTime)
        {
            //ChangeMouseTexture(-50);
            IsClicked = false;
            IsRightClicked = false;
            WasJustPressed = false;
            this.IsReleased = false;
            
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

            

            WorldMouseRectangle = new Rectangle((int)WorldMousePosition.X, (int)WorldMousePosition.Y, 1, 1);

            if (MyMouse.LeftButton == ButtonState.Pressed)
            {
                HoldTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (HoldTimer > RequiredHoldTime)
            {
                this.IsClickedAndHeld = true;
            }
            if(MyMouse.LeftButton == ButtonState.Released)
            {
                if(IsClickedAndHeld)
                {
                    HoldTimer = 0f;
                    IsClickedAndHeld = false;
                    this.IsReleased = true;

                }
                else if(oldMouse.LeftButton == ButtonState.Pressed)
                {
                    IsClicked = true;
                }
            }
            
        }

        public void Draw(SpriteBatch spriteBatch, float depth)
        {      
            if(ToggleGeneralInteraction)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(WorldMousePosition.X + 6, WorldMousePosition.Y + 6), this.CursorSourceRectangle, Color.White, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 1f);
            }
            

        }

        public void ChangeMouseTexture(int type)
        {
            switch (type)
            {
                case -50:
                    this.CursorSourceRectangle = NormalInteractionSourceRectangle;
                    break;
                case 0:
                    this.CursorSourceRectangle = ChoppingInteractionSourceRectangle;
                    break;
                case 1:
                    this.CursorSourceRectangle = MiningInteractionSourceRectangle;
                    break;
                case 2:
                    this.CursorSourceRectangle = PlantInteractionSourceRectangle;
                    break;
                case 3:
                    this.CursorSourceRectangle = DiggingInteractionSourceRectangle;
                    break;
                case 200:
                    this.CursorSourceRectangle = ChatInteractionSourceRectangle;
                    break;

                default:
                    this.CursorSourceRectangle = NormalInteractionSourceRectangle;
                    break;


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

        public Tile GetMouseOverTile(IInformationContainer container)
        {
            if (this.MouseSquareCoordinateX > 0 && this.MouseSquareCoordinateY > 0)
            {

                return container.AllTiles[0][MouseSquareCoordinateX, MouseSquareCoordinateY];

            }
            else
            {
                return container.AllTiles[0][0, 0];
            }

        }

        public Tile[] GetMouseOverTileArray(IInformationContainer container)
        {
            Tile[] tilesToReturn = new Tile[container.AllTiles.Count];
            if (this.MouseSquareCoordinateX > 0 && this.MouseSquareCoordinateY > 0)
            {
                for (int i = 0; i < container.AllTiles.Count; i++)
                {
                    tilesToReturn[i] = container.AllTiles[i][MouseSquareCoordinateX, MouseSquareCoordinateY];
                }
            }
            return tilesToReturn;
        }
    }
        
}

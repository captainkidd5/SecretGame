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
    public enum CursorType
    {
        Normal = -50,
        NormalHeld = -49,
        Chopping = 21,
        Mining = 22,
        Planting = 2,
        Digging = 3,
        Door = 4,
        Currency = 5,
        NextChatWindow = 6,
        Chat = 200,

    }
    public class MouseManager
    {
        public bool IsDown { get; set; }
        public bool IsClicked { get; set; }
        public bool IsRightClicked { get; set; }

        public bool IsClickedAndHeld { get; set; }

        public Vector2 Position { get; set; }


        public Vector2 UIPosition { get; set; }

        Vector2 worldMousePosition;
        public Vector2 WorldMousePosition { get { return worldMousePosition; } set { worldMousePosition = value; } }

        public MouseState MyMouse { get; set; }
        public Rectangle MouseRectangle { get; set; }
        public Rectangle MouseSquareCoordinateRectangle { get; set; }

        public float RelativeMouseX { get; set; }
        public float RelativeMouseY { get; set; }

        

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
        public Vector2 OldMouseInterfacePosition { get; set; }

        public Rectangle WorldMouseRectangle { get; set; }

        public Texture2D MouseTypeTexture { get; set; }

        #region MOUSE CURSORTYPE RECTANGLES
        public Rectangle CursorSourceRectangle { get; set; }
        public Rectangle HeldCursorSourceRectangle { get; set; }

        public Rectangle NormalInteractionSourceRectangle { get; set; }
        public Rectangle NormalInteractionPressedSourceRectangle { get; set; }
        public Rectangle PlantInteractionSourceRectangle { get; set; }
        public Rectangle MiningInteractionSourceRectangle { get; set; }
        public Rectangle ChoppingInteractionSourceRectangle { get; set; }
        public Rectangle DiggingInteractionSourceRectangle { get; set; }
        public Rectangle ChatInteractionSourceRectangle { get; set; }
        public Rectangle CurrencyInteractionSourceRectangle { get; set; }
        public Rectangle DoorInteractionSourceRectangle { get; set; }
        public Rectangle NextChatWindowInteractionSourceRectangle { get; set; }
        #endregion
        public float HoldTimer { get; set; }
        public float RequiredHoldTime { get; set; }

        public bool ButtonOccupied { get; set; }

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
            this.NormalInteractionPressedSourceRectangle = new Rectangle(48, 288, 32, 32);
            this.PlantInteractionSourceRectangle = new Rectangle(16, 256, 32, 32);
            this.MiningInteractionSourceRectangle = new Rectangle(112, 256, 32, 32);
            this.ChoppingInteractionSourceRectangle = new Rectangle(144, 256, 32, 32);
            this.DiggingInteractionSourceRectangle = new Rectangle(80, 256, 32, 32);
            this.ChatInteractionSourceRectangle = new Rectangle(176, 256, 32, 32);
            this.CurrencyInteractionSourceRectangle = new Rectangle(16, 288, 32, 32);
            this.DoorInteractionSourceRectangle = new Rectangle(112, 288, 32, 32);
            this.NextChatWindowInteractionSourceRectangle = new Rectangle(80, 288, 32, 32);



            //HELD
            this.HeldCursorSourceRectangle = NormalInteractionPressedSourceRectangle;
            this.RequiredHoldTime = .15f;

            OldMouseInterfacePosition = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {
            //ChangeMouseTexture(-50);
            IsClicked = false;
            IsRightClicked = false;
            IsDown = false;
            
            HasScrollWheelValueIncreased = false;
            HasScrollWheelValueDecreased = false;

            OldMouseInterfacePosition = UIPosition;
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


            Position = new Vector2(MyMouse.Position.X, MyMouse.Position.Y);

            UIPosition = new Vector2(MyMouse.Position.X - 20, MyMouse.Position.Y - 20);

            

                WorldMousePosition = new Vector2((int)worldPosition.X - XTileOffSet, (int)worldPosition.Y - YTileOffSet);


            MouseRectangle = new Rectangle(MyMouse.X, MyMouse.Y, 1, 1);
            SquarePosition = GetMouseSquarePosition(WorldMousePosition);
            MouseSquareCoordinateRectangle = new Rectangle((int)SquarePosition.X, (int)SquarePosition.Y, 16, 16);
           


            

            WorldMouseRectangle = new Rectangle((int)WorldMousePosition.X, (int)WorldMousePosition.Y, 1, 1);

            if (MyMouse.LeftButton == ButtonState.Pressed)
            {
                IsDown = true;
                HoldTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (HoldTimer > RequiredHoldTime)
            {

                this.IsClickedAndHeld = true;
                ChangeMouseTexture(CursorType.NormalHeld);
                Game1.isMyMouseVisible = false;
                this.ToggleGeneralInteraction = true;
            }
            if(MyMouse.LeftButton == ButtonState.Released)
            {
                if(IsClickedAndHeld)
                {
                    
                    IsClickedAndHeld = false;

                }
                else if(oldMouse.LeftButton == ButtonState.Pressed)
                {
                    IsClicked = true;
                }
                HoldTimer = 0f;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch, float depth)
        {      
            if(ToggleGeneralInteraction)
            {
                Game1.isMyMouseVisible = false;
                if (this.IsClickedAndHeld)
                {
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.HeldCursorSourceRectangle, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);
                }
                else
                {
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.CursorSourceRectangle, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);
                }
                
            }
            

        }

        public void ChangeMouseTexture(CursorType type)
        {
            this.ToggleGeneralInteraction = true;
            Game1.isMyMouseVisible = false;
            switch (type)
            {
                case CursorType.Normal:
                    this.CursorSourceRectangle = NormalInteractionSourceRectangle;
                    this.HeldCursorSourceRectangle = NormalInteractionPressedSourceRectangle;
                    break;
                case CursorType.Chopping:
                    this.CursorSourceRectangle = ChoppingInteractionSourceRectangle;
                    break;
                case CursorType.Mining:
                    this.CursorSourceRectangle = MiningInteractionSourceRectangle;
                    break;
                case CursorType.Planting:
                    this.CursorSourceRectangle = PlantInteractionSourceRectangle;
                    break;
                case CursorType.Digging:
                    this.CursorSourceRectangle = DiggingInteractionSourceRectangle;
                    break;
                case CursorType.Chat:
                    this.CursorSourceRectangle = ChatInteractionSourceRectangle;
                    break;
                case CursorType.Door:
                    this.CursorSourceRectangle = DoorInteractionSourceRectangle;
                    break;
                case CursorType.Currency:
                    this.CursorSourceRectangle = CurrencyInteractionSourceRectangle;
                    break;
                case CursorType.NextChatWindow:
                    this.CursorSourceRectangle = NextChatWindowInteractionSourceRectangle;
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
            if (this.SquarePosition.X > 0 && this.SquarePosition.Y > 0)
            {

                return container.AllTiles[0][(int)SquarePosition.X / (container.X + 1), (int)SquarePosition.Y / (container.Y + 1)];

            }
            else
            {
                return null;
            }

        }

        public Tile[] GetMouseOverTileArray(IInformationContainer container)
        {
            Tile[] tilesToReturn = new Tile[container.AllTiles.Count];
            if (this.SquarePosition.X > 0 && this.SquarePosition.Y > 0)
            {
                for (int i = 0; i < container.AllTiles.Count; i++)
                {
                    tilesToReturn[i] = container.AllTiles[i][(int)SquarePosition.X / (container.X + 1), (int)SquarePosition.Y / (container.Y + 1)];
                }
            }
            return tilesToReturn;
        }
    }
        
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Dynamics.Joints;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

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
    public class MouseManager : IEntity
    {
        public bool EnableBody { get; set; }
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

        public RectangleCollider MouseCollider { get; set; }

        public float MouseAngleInRelationToPlayer { get; set; }

        private MouseManager()
        {

        }

        public MouseManager(Camera2D camera, GraphicsDevice graphicsDevice)
        {
            this.IsClicked = false;
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

            this.MouseAngleInRelationToPlayer = 0f;

            //HELD
            this.HeldCursorSourceRectangle = this.NormalInteractionPressedSourceRectangle;
            this.RequiredHoldTime = .15f;

            this.OldMouseInterfacePosition = Vector2.Zero;
            this.MouseCollider = new RectangleCollider(graphicsDevice, new Rectangle(0,0,1,1), this, ColliderType.MouseCollider);
            AttachMouseBody();
        }

        public void Update(GameTime gameTime)
        {
            //ChangeMouseTexture(-50);
            ChangeMouseTexture(CursorType.Normal);
            this.IsClicked = false;
            this.IsRightClicked = false;
            this.IsDown = false;

            HasScrollWheelValueIncreased = false;
            HasScrollWheelValueDecreased = false;

            this.OldMouseInterfacePosition = this.UIPosition;
            MouseState oldMouse = this.MyMouse;
            this.MyMouse = Mouse.GetState();
            ///
            OldScrollWheelValue = oldMouse.ScrollWheelValue;
            NewScrollWheelValue = this.MyMouse.ScrollWheelValue;

            if (NewScrollWheelValue > OldScrollWheelValue)
            {
                HasScrollWheelValueIncreased = true;
            }
            else if (NewScrollWheelValue < OldScrollWheelValue)
            {
                HasScrollWheelValueDecreased = true;
            }
            ///
            worldPosition = Vector2.Transform(this.Position, Matrix.Invert(this.Camera1.GetViewMatrix(Vector2.One)));


            this.Position = new Vector2(this.MyMouse.Position.X, this.MyMouse.Position.Y);

            this.UIPosition = new Vector2(this.MyMouse.Position.X - 20, this.MyMouse.Position.Y - 20);



            this.WorldMousePosition = new Vector2((int)worldPosition.X - this.XTileOffSet, (int)worldPosition.Y - this.YTileOffSet);


            this.MouseRectangle = new Rectangle(this.MyMouse.X, this.MyMouse.Y, 1, 1);
            this.SquarePosition = GetMouseSquarePosition(this.WorldMousePosition);
            this.MouseSquareCoordinateRectangle = new Rectangle((int)this.SquarePosition.X, (int)this.SquarePosition.Y, 16, 16);





            this.WorldMouseRectangle = new Rectangle((int)this.WorldMousePosition.X, (int)this.WorldMousePosition.Y, 1, 1);

            if (this.MyMouse.LeftButton == ButtonState.Pressed)
            {
                this.IsDown = true;
                this.HoldTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (this.HoldTimer > this.RequiredHoldTime)
            {

                this.IsClickedAndHeld = true;
                ChangeMouseTexture(CursorType.NormalHeld);
                Game1.isMyMouseVisible = false;
                this.ToggleGeneralInteraction = true;
            }
            if (this.MyMouse.LeftButton == ButtonState.Released)
            {
                if (this.IsClickedAndHeld)
                {

                    this.IsClickedAndHeld = false;

                }
                else if (oldMouse.LeftButton == ButtonState.Pressed)
                {
                    this.IsClicked = true;
                }
                this.HoldTimer = 0f;
            }
            if(this.MyMouse.RightButton == ButtonState.Released)
            {
                if (oldMouse.RightButton == ButtonState.Pressed)
                {
                    this.IsRightClicked = true;
                }
            }

            if (Game1.CurrentStage != null && Game1.CurrentStage != Game1.mainMenu)
            {


                if (WorldMouseRectangle.Intersects(Game1.Player.BigCollider.Rectangle))
                {
                    this.MouseCollider.Rectangle = this.WorldMouseRectangle;
                    List<ICollidable> returnObjects = new List<ICollidable>();
                    Game1.CurrentStage.QuadTree.Retrieve(returnObjects, this.MouseCollider);
                    for (int i = 0; i < returnObjects.Count; i++)
                    {
                        if (returnObjects[i].ColliderType == ColliderType.inert)
                        {
                            if (this.MouseCollider.Rectangle.Intersects(returnObjects[i].Rectangle))
                            {
                                // Tile tile = (Tile)returnObjects[i].Entity;
                                if (returnObjects[i].Entity != null)
                                {
                                    returnObjects[i].Entity.MouseCollisionInteraction();
                                }

                            }
                        }

                    }
                }
            }
                MouseBody.Enabled = EnableBody;

            MouseJoint.WorldAnchorB = WorldMousePosition;

        }

        public void Draw(SpriteBatch spriteBatch, float depth)
        {
            if (this.ToggleGeneralInteraction)
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

        public void DrawDebug(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.redPixel, this.MouseBody.Position, new Rectangle(0, 0, 1, 1),
                Color.White, 1f, new Vector2(.5f, .5f), new Vector2(10f, 10f), SpriteEffects.None, .99f);
        }

        public void ChangeMouseTexture(CursorType type)
        {
            this.ToggleGeneralInteraction = true;
            Game1.isMyMouseVisible = false;
            switch (type)
            {
                case CursorType.Normal:
                    this.CursorSourceRectangle = this.NormalInteractionSourceRectangle;
                    this.HeldCursorSourceRectangle = this.NormalInteractionPressedSourceRectangle;
                    break;
                case CursorType.Chopping:
                    this.CursorSourceRectangle = this.ChoppingInteractionSourceRectangle;
                    break;
                case CursorType.Mining:
                    this.CursorSourceRectangle = this.MiningInteractionSourceRectangle;
                    break;
                case CursorType.Planting:
                    this.CursorSourceRectangle = this.PlantInteractionSourceRectangle;
                    break;
                case CursorType.Digging:
                    this.CursorSourceRectangle = this.DiggingInteractionSourceRectangle;
                    break;
                case CursorType.Chat:
                    this.CursorSourceRectangle = this.ChatInteractionSourceRectangle;
                    break;
                case CursorType.Door:
                    this.CursorSourceRectangle = this.DoorInteractionSourceRectangle;
                    break;
                case CursorType.Currency:
                    this.CursorSourceRectangle = this.CurrencyInteractionSourceRectangle;
                    break;
                case CursorType.NextChatWindow:
                    this.CursorSourceRectangle = this.NextChatWindowInteractionSourceRectangle;
                    break;
                default:
                    this.CursorSourceRectangle = this.NormalInteractionSourceRectangle;
                    break;


            }

        }

        public Vector2 GetMouseSquarePosition(Vector2 mousePosition)
        {
            return new Vector2((int)(mousePosition.X / 16), (int)(mousePosition.Y / 16));
        }




        public bool IsHovering(Rectangle rectangle)
        {

            if (this.MouseRectangle.Intersects(rectangle))
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
            Rectangle offSetRectange = new Rectangle((int)this.WorldMousePosition.X, (int)this.WorldMousePosition.Y, 1, 1);
            if (offSetRectange.Intersects(rectangle))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Tile GetMouseOverTile(TileManager TileManager)
        {
            if (this.SquarePosition.X > 0 && this.SquarePosition.Y > 0)
            {

                return TileManager.AllTiles[0][(int)this.SquarePosition.X / (TileManager.X + 1), (int)this.SquarePosition.Y / (TileManager.Y + 1)];

            }
            else
            {
                return null;
            }

        }

        public Tile[] GetMouseOverTileArray(TileManager TileManager)
        {
            Tile[] tilesToReturn = new Tile[TileManager.AllTiles.Count];
            if (this.SquarePosition.X > 0 && this.SquarePosition.Y > 0)
            {
                for (int i = 0; i < TileManager.AllTiles.Count; i++)
                {
                    tilesToReturn[i] = TileManager.AllTiles[i][(int)this.SquarePosition.X / (TileManager.X + 1), (int)this.SquarePosition.Y / (TileManager.Y + 1)];
                }
            }
            return tilesToReturn;
        }
        public Body MouseBody { get; set; }
        public FixedMouseJoint MouseJoint { get; set; }
        public void AttachMouseBody()
        {
            MouseBody = BodyFactory.CreateCircle(Game1.VelcroWorld,ConvertUnits.ToSimUnits( 8f), 1f);
            MouseBody.Position = ConvertUnits.ToSimUnits(WorldMousePosition);
            MouseBody.BodyType = BodyType.Dynamic;
            MouseBody.Mass = .4f;
            MouseBody.Restitution = .2f;
            MouseBody.Friction = .4f;
            MouseBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Mouse;
            MouseBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Grass;
            MouseBody.FixedRotation = true;

             MouseJoint = JointFactory.CreateFixedMouseJoint(Game1.VelcroWorld, MouseBody, MouseBody.Position);
            MouseJoint.MaxForce = 500f;

            //MouseBody.OnCollision += OnCollision;

        }
        private void OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            // fixtureA.
            Console.WriteLine("mouse hit grass");
            // SelfDestruct();
        }

        public void DamageCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public void MouseCollisionInteraction()
        {
            throw new System.NotImplementedException();
        }
    }

}

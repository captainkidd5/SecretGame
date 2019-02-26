using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;


using SecretProject.Class.SpriteFolder;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.ItemStuff;
using Microsoft.Xna.Framework.Content;
using SecretProject.Class.Controls;

namespace SecretProject.Class.Playable
{
    [Serializable()]
    public class Player : ISerializable
    {

        //TODO: Work on gametime
        public Vector2 position;
        public Vector2 Velocity;
        public bool Activate { get; set; }

        public AnimatedSprite[] animations;

        public string Name { get; set; }
        public Inventory Inventory { get; set; }
        ContentManager content;

        public PlayerControls controls;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public void SetX(float newX)
        {
            position.X = newX;
        }

        public void SetY(float newY)
        {
            position.Y = newY;
        }

        public int Health { get { return Health1; } set { value = Health1; } }

        public int Health1 { get; set; } = 3;
        public Dir Direction { get; set; } = Dir.Down;
        public bool IsMoving { get; set; } = false;
        public float Speed1 { get; set; } = 3f;
        public AnimatedSprite PlayerMovementAnimations { get; set; }
        public Texture2D Texture { get; set; }
        public int FrameNumber { get; set; }
        public Collider MyCollider { get; set; }

        public bool IsPerformingAction = false;

       // private bool PlayingSecondaryAnimation = false;

        public AnimatedSprite CurrentAction;

        public AnimatedSprite CutGrassDown { get; set; }
        public AnimatedSprite CutGrassRight { get; set; }
        public AnimatedSprite CutGrassLeft { get; set; }
        public AnimatedSprite CutGrassUp { get; set; }


        public Texture2D BigHitBoxRectangleTexture;


        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X + 2, (int)position.Y + 20, ((int)Texture.Width / FrameNumber) -2, (int)Texture.Height - 25);
            }

        }

        public Rectangle ClickRangeRectangle
        {
            get
            {
                return new Rectangle((int)position.X - 15, (int)position.Y - 15, ((int)Texture.Width / FrameNumber) + 30, (int)Texture.Height + 30);
            }
        }



        public Player(string name, Vector2 position, Texture2D texture, int frameNumber, ContentManager content, GraphicsDevice graphics, MouseManager mouse)
        {
            this.content = content;
            Name = name;
            Position = position;
            this.Texture = texture;
            this.FrameNumber = frameNumber;
            animations = new AnimatedSprite[frameNumber];

            MyCollider = new Collider(Velocity, Rectangle);

            Inventory = new Inventory(graphics, content, mouse);

            controls = new PlayerControls(0);



            CutGrassDown = new AnimatedSprite(graphics, Game1.AllTextures.CutGrassDown, 1, 3, 3);
            CutGrassRight = new AnimatedSprite(graphics, Game1.AllTextures.CutGrassRight, 1, 3, 3) { AdjustedLocationX = 1 };
            CutGrassLeft = new AnimatedSprite(graphics, Game1.AllTextures.CutGrassLeft, 1, 3, 3) { AdjustedLocationX = -18 };
            CutGrassUp = new AnimatedSprite(graphics, Game1.AllTextures.CutGrassUp, 1, 3, 3) { AdjustedLocationX = -1, AdjustedLocationY = -7 };

            CurrentAction = CutGrassDown;

            SetRectangleTexture(graphics, ClickRangeRectangle);

        }

        public Player()
        {

        }

        

        public void PlayAnimation(GameTime gameTime, string action)
        {
            switch (action)
            {
                case "CutGrassDown":
                    IsPerformingAction = true;
                    CutGrassDown.PlayOnce(gameTime);
                    CurrentAction = CutGrassDown;
                    CurrentAction.IsAnimating = true;
                    break;

                case "CutGrassRight":
                    IsPerformingAction = true;
                    CutGrassRight.PlayOnce(gameTime);
                    CurrentAction = CutGrassRight;
                    CurrentAction.IsAnimating = true;
                    break;

                case "CutGrassLeft":
                    IsPerformingAction = true;
                    CutGrassLeft.PlayOnce(gameTime);
                    CurrentAction = CutGrassLeft;
                    CurrentAction.IsAnimating = true;
                    break;

                case "CutGrassUp":
                    IsPerformingAction = true;
                    CutGrassUp.PlayOnce(gameTime);
                    CurrentAction = CutGrassUp;
                    CurrentAction.IsAnimating = true;
                    break;


            }
        }



        private void SetRectangleTexture(GraphicsDevice graphicsDevice, Rectangle rectangleToDraw)
        {
            var Colors = new List<Color>();
            for (int y = 0; y < rectangleToDraw.Height; y++)
            {
                for (int x = 0; x < rectangleToDraw.Width; x++)
                {
                    if (x == 0 || //left side
                        y == 0 || //top side
                        x == rectangleToDraw.Width - 1 || //right side
                        y == rectangleToDraw.Height - 1) //bottom side
                    {
                        Colors.Add(new Color(255, 255, 255, 255));
                    }
                    else
                    {
                        Colors.Add(new Color(0, 0, 0, 0));

                    }

                }
            }
            BigHitBoxRectangleTexture = new Texture2D(graphicsDevice, rectangleToDraw.Width, rectangleToDraw.Height);
            BigHitBoxRectangleTexture.SetData<Color>(Colors.ToArray());
        }


        public void Update(GameTime gameTime, List<WorldItem> items, List<ObjectFolder.ObjectBody> objects)
        {
            if (Activate)
            {
                KeyboardState kState = Keyboard.GetState();
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

                //animation set changes depending on which key is pressed, as shown in playercontrols
                PlayerMovementAnimations = animations[(int)controls.Direction];

                MyCollider.Rectangle = this.Rectangle;
                MyCollider.Velocity = this.Velocity;
                MyCollider.DidCollideMagnet(items);


                MyCollider.DidCollide(objects);
                this.Velocity = MyCollider.Velocity;

                //Velocity = Velocity * dt;

                Position += Velocity;

                Velocity = Vector2.Zero;


                if (controls.IsMoving && CurrentAction.IsAnimating == false)
                {
                    PlayerMovementAnimations.Update(gameTime);
                }
                else if (CurrentAction.IsAnimating == true)
                {
                    CurrentAction.PlayOnce(gameTime);

                }
                else if (CurrentAction.IsAnimating == false && controls.IsMoving == false)
                {
                    PlayerMovementAnimations.setFrame(0);
                } 


                IsMoving = false;

                if(!CurrentAction.IsAnimating)
                {
                    controls.Update();
                }
                

                if (controls.IsMoving)
                {
                    switch (controls.Direction)
                    {
                        case Dir.Right:
                            Velocity.X = Speed1 ;
                            break;

                        case Dir.Left:
                            Velocity.X = -Speed1;
                            break;

                        case Dir.Down:
                            Velocity.Y = Speed1;
                            break;

                        case Dir.Up:
                            Velocity.Y = -Speed1;
                            break;

                        default:
                            break;

                    }


                }

                if(position.X < 0)
                {
                    position.X = 0;
                }
                if(position.X > 1580)
                {
                    position.X = 1580;
                }
                if(position.Y < 0)
                {
                    position.Y = 0;
                }
                if(position.Y > 1560)
                {
                    position.Y = 1560;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Position", Position);
        }

        public Player(SerializationInfo info, StreamingContext context)
        {
            Position = (Vector2)info.GetValue("Position", typeof(Vector2));
        }
    }
}
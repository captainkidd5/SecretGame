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
        private Vector2 position = new Vector2(300, 300);
        private int _health = 3;


        private Dir direction = Dir.Down;
        private bool isMoving = false;
        public float Speed = 2f;
        public Vector2 Velocity;
        public bool Activate { get; set; }

        private Texture2D texture;

        public AnimatedSprite anim;
        public AnimatedSprite[] animations;

        public string Name { get; set; }

        public int frameNumber;

        public Collider myCollider;

        Inventory inventory;
        public Inventory Inventory { get { return inventory; } set { inventory = value; } }
        ContentManager content;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y + 5, (int)texture.Width, (int)texture.Height -5);
            }

        }

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

        public int Health { get { return _health; } set { value = _health; } }



        public Keys Up;
        public Keys Left;
        public Keys Right;
        public Keys Down;



        public Player(string name, Vector2 position, Texture2D texture, int frameNumber, ContentManager content, GraphicsDevice graphics, MouseManager mouse)
        {
            Name = name;
            Position = position;
            this.texture = texture;
            this.frameNumber = frameNumber;
            animations = new AnimatedSprite[frameNumber];

            myCollider = new Collider(Velocity, Rectangle);

            inventory = new Inventory(content, graphics, mouse);

            this.content = content;



        }

        public Player()
        {

        }

        public void Update(GameTime gameTime, List<Sprite> sprites, List<ObjectFolder.ObjectBody> objects)
        {
            if (Activate)
            {
                KeyboardState kState = Keyboard.GetState();
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

                anim = animations[(int)direction];

                myCollider.Rectangle = this.Rectangle;
                myCollider.Velocity = this.Velocity;

                myCollider.DidCollideMagnet(sprites);


                myCollider.DidCollide(objects);
                this.Velocity = myCollider.Velocity;
                
                Position += Velocity;

                Velocity = Vector2.Zero;


                if (isMoving)
                    anim.Update(gameTime);
                else anim.setFrame(0);

                isMoving = false;

                if (kState.IsKeyDown(Keys.D))
                {
                    direction = Dir.Right;
                    isMoving = true;

                }

                if (kState.IsKeyDown(Keys.A))
                {
                    direction = Dir.Left;
                    isMoving = true;
                }

                if (kState.IsKeyDown(Keys.W))
                {
                    direction = Dir.Up;
                    isMoving = true;
                }

                if (kState.IsKeyDown(Keys.S))
                {
                    direction = Dir.Down;
                    isMoving = true;
                }


                if (isMoving)
                {
                    switch (direction)
                    {
                        case Dir.Right:
                            Velocity.X = Speed;
                            break;

                        case Dir.Left:
                            Velocity.X = -Speed;
                            break;

                        case Dir.Down:
                            Velocity.Y = Speed;
                            break;

                        case Dir.Up:
                            Velocity.Y = -Speed;
                            break;

                        default:
                            break;



                    }


                }
            }
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

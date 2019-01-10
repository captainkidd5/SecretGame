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
        public Vector2 position = new Vector2(300, 300);
        public Vector2 Velocity;
        public bool Activate { get; set; }

        public AnimatedSprite[] animations;

        public string Name { get; set; }
        public Inventory Inventory { get; set; }
        ContentManager content;

        PlayerControls controls;

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
        public float Speed1 { get; set; } = 50f;
        public AnimatedSprite Anim { get; set; }
        public Texture2D Texture { get; set; }
        public int FrameNumber { get; set; }
        public Collider MyCollider { get; set; }


        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y + 5, (int)Texture.Width, (int)Texture.Height - 5);
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



        }

        public Player()
        {

        }

        public void Update(GameTime gameTime, List<WorldItem> items, List<ObjectFolder.ObjectBody> objects)
        {
            if (Activate)
            {
                KeyboardState kState = Keyboard.GetState();
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

                Anim = animations[(int)controls.Direction];

                MyCollider.Rectangle = this.Rectangle;
                MyCollider.Velocity = this.Velocity;
                MyCollider.DidCollideMagnet(items);

               // MyCollider.DidCollideMagnet(sprites);


                MyCollider.DidCollide(objects);
                this.Velocity = MyCollider.Velocity;

                Position += Velocity;

                Velocity = Vector2.Zero;


                if (controls.IsMoving)
                    Anim.Update(gameTime);
                else Anim.setFrame(0);

                IsMoving = false;

                controls.Update();

                if (controls.IsMoving)
                {
                    switch (controls.Direction)
                    {
                        case Dir.Right:
                            Velocity.X = Speed1 * dt;
                            break;

                        case Dir.Left:
                            Velocity.X = -Speed1 * dt;
                            break;

                        case Dir.Down:
                            Velocity.Y = Speed1 * dt;
                            break;

                        case Dir.Up:
                            Velocity.Y = -Speed1 * dt;
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

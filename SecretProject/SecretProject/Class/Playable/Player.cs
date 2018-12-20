using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


using SecretProject.Class.SpriteFolder;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.CollisionDetection;

namespace SecretProject.Class.Playable
{
    public class Player
    {
        private Vector2 _position = new Vector2(300, 300);
        private int _health = 3;


        private Dir _direction = Dir.Down;
        private bool _isMoving = false;
        public float Speed = 2f;
        public Vector2 Velocity;
        public bool Activate { get; set; }

        private Texture2D _texture;

        public AnimatedSprite anim;
        public AnimatedSprite[] animations;

        public string Name { get; set; }

        public int frameNumber;

        public Collider myCollider;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y + 5, (int)_texture.Width, (int)_texture.Height -5);
            }

        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public void SetX(float newX)
        {
            _position.X = newX;
        }

        public void SetY(float newY)
        {
            _position.Y = newY;
        }

        public int Health { get { return _health; } set { value = _health; } }



        public Keys Up;
        public Keys Left;
        public Keys Right;
        public Keys Down;



        public Player(string name, Vector2 position, Texture2D texture, int frameNumber)
        {
            Name = name;
            Position = position;
            this._texture = texture;
            this.frameNumber = frameNumber;
            animations = new AnimatedSprite[frameNumber];

            myCollider = new Collider(Velocity, Rectangle);



        }

        public void Update(GameTime gameTime, List<Sprite> sprites, List<ObjectFolder.ObjectBody> objects)
        {
            if (Activate)
            {
                KeyboardState kState = Keyboard.GetState();
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

                anim = animations[(int)_direction];

                myCollider.Rectangle = this.Rectangle;
                myCollider.Velocity = this.Velocity;

                myCollider.DidCollideMagnet(sprites);


                myCollider.DidCollide(objects);
                this.Velocity = myCollider.Velocity;
                
                Position += Velocity;

                Velocity = Vector2.Zero;


                if (_isMoving)
                    anim.Update(gameTime);
                else anim.setFrame(0);

                _isMoving = false;

                if (kState.IsKeyDown(Keys.D))
                {
                    _direction = Dir.Right;
                    _isMoving = true;

                }

                if (kState.IsKeyDown(Keys.A))
                {
                    _direction = Dir.Left;
                    _isMoving = true;
                }

                if (kState.IsKeyDown(Keys.W))
                {
                    _direction = Dir.Up;
                    _isMoving = true;
                }

                if (kState.IsKeyDown(Keys.S))
                {
                    _direction = Dir.Down;
                    _isMoving = true;
                }


                if (_isMoving)
                {
                    switch (_direction)
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
    }
}

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
using Object = SecretProject.Class.ObjectFolder.Object;

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

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, (int)_texture.Width, (int)_texture.Height);
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

        }

        
        

        public void Update(GameTime gameTime, List<Sprite> sprites, List<Object> objects)
        {
            if (Activate)
            {
                KeyboardState kState = Keyboard.GetState();
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

                anim = animations[(int)_direction];



                foreach (var sprite in sprites)
                {


                    if ((this.Velocity.X > 0 && this.IsTouchingLeft(sprite)) ||
                       (this.Velocity.X < 0 && this.IsTouchingRight(sprite)))
                        this.Velocity.X = 0;

                    if ((this.Velocity.Y > 0 && this.IsTouchingTop(sprite)) ||
                       (this.Velocity.Y < 0 && this.IsTouchingBottom(sprite)))
                        this.Velocity.Y = 0;

                }

                foreach (var obj in objects)
                {


                    if (this.Velocity.X > 0 && this.IsTouchingLeft(obj))
                        this.Velocity.X -= this.Velocity.X; //+ (float).25;

                    if (this.Velocity.X < 0 && this.IsTouchingRight(obj))
                        this.Velocity.X -= this.Velocity.X; //- (float).25;

                    if (this.Velocity.Y > 0 && this.IsTouchingTop(obj))
                        this.Velocity.Y -= this.Velocity.Y; //+ (float).25;
                    if (this.Velocity.Y < 0 && this.IsTouchingBottom(obj))
                        this.Velocity.Y -= this.Velocity.Y;// - (float).25;

                }

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

        protected bool IsTouchingLeft(Sprite sprite)
        {
            return this.Rectangle.Right + this.Velocity.X > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Left &&
                this.Rectangle.Bottom > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Bottom;
        }
        protected bool IsTouchingRight(Sprite sprite)
        {
            return this.Rectangle.Left + this.Velocity.X < sprite.Rectangle.Right &&
                this.Rectangle.Right > sprite.Rectangle.Right &&
                this.Rectangle.Bottom > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Bottom;
        }
        protected bool IsTouchingTop(Sprite sprite)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Top &&
                this.Rectangle.Right > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Right;
        }
        protected bool IsTouchingBottom(Sprite sprite)
        {
            return this.Rectangle.Top + this.Velocity.Y < sprite.Rectangle.Bottom &&
                this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
                this.Rectangle.Right > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsTouchingLeft(ObjectFolder.Object obj)
        {
            return this.Rectangle.Right + this.Velocity.X > obj.Rectangle.Left &&
                this.Rectangle.Left < obj.Rectangle.Left &&
                this.Rectangle.Bottom > obj.Rectangle.Top &&
                this.Rectangle.Top < obj.Rectangle.Bottom;
        }
        protected bool IsTouchingRight(ObjectFolder.Object obj)
        {
            return this.Rectangle.Left + this.Velocity.X < obj.Rectangle.Right &&
                this.Rectangle.Right > obj.Rectangle.Right &&
                this.Rectangle.Bottom > obj.Rectangle.Top &&
                this.Rectangle.Top < obj.Rectangle.Bottom;
        }
        protected bool IsTouchingTop(ObjectFolder.Object obj)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > obj.Rectangle.Top &&
                this.Rectangle.Top < obj.Rectangle.Top &&
                this.Rectangle.Right > obj.Rectangle.Left &&
                this.Rectangle.Left < obj.Rectangle.Right;
        }
        protected bool IsTouchingBottom(ObjectFolder.Object obj)
        {
            return this.Rectangle.Top + this.Velocity.Y < obj.Rectangle.Bottom &&
                this.Rectangle.Bottom > obj.Rectangle.Bottom &&
                this.Rectangle.Right > obj.Rectangle.Left &&
                this.Rectangle.Left < obj.Rectangle.Right;
        }

    }
}

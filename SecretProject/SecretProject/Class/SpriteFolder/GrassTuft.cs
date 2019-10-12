using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.SpriteFolder
{
    public class GrassTuft
    {
        public int GrassType { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle DestinationRectangle { get; set; }
        public float Rotation { get; set; }
        public float RotationCap { get; set; }
        public bool StartShuff { get; set; }
        public float ShuffSpeed { get; set; }
        public float YOffSet { get; set; }
        public Dir ShuffDirection { get; set; }
        public bool ShuffDirectionPicked { get; set; }

        public Rectangle Type1SourceRectangle { get; set; }
        public Rectangle Type2SourceRectangle { get; set; }
        public Rectangle Type3SourceRectangle { get; set; }

        public GrassTuft(int grassType,Vector2 position)
        {
            this.GrassType = grassType;
            this.Position = position;
            this.DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, 16, 32);
            this.Rotation = 0f;
            this.RotationCap = .25f;
            this.ShuffSpeed = 2f;
            this.StartShuff = false;
            this.YOffSet = Game1.Utility.RFloat(.0000001f, .000001f);
            this.ShuffDirection = Dir.Left;
            this.ShuffDirectionPicked = false;

            this.Type1SourceRectangle = new Rectangle(0, 0, 16, 32);
            this.Type2SourceRectangle = new Rectangle(16, 0, 16, 32);
            this.Type3SourceRectangle = new Rectangle(32, 0, 16, 32);
        }
        public void Update(GameTime gameTime)
        {
            
            
           
            if(!StartShuff && Game1.Player.ColliderRectangle.Intersects(new Rectangle((int)Position.X, (int)Position.Y, 16, 16)))
            {
                this.StartShuff = true;
                   
            }



            if (!this.StartShuff)
            {
                RotateBackToOrigin(gameTime);
                this.ShuffDirectionPicked = false;
            }
            if (this.StartShuff)
            {
                Shuff(gameTime, (int)Game1.Player.controls.Direction);
                ShuffDirectionPicked = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
         
            switch (GrassType)
            {
                case 1:
                    spriteBatch.Draw(Game1.AllTextures.TallGrass, DestinationRectangle, Type1SourceRectangle,
                        Color.White, Rotation, new Vector2(8, 24), SpriteEffects.None, .5f + (DestinationRectangle.Y ) * .0000001f + YOffSet);
                    break;
                case 2:
                    spriteBatch.Draw(Game1.AllTextures.TallGrass, DestinationRectangle, Type2SourceRectangle,
                        Color.White, Rotation, new Vector2(8, 24), SpriteEffects.None, .5f + (DestinationRectangle.Y ) * .0000001f + YOffSet);
                    break;
                case 3:
                    spriteBatch.Draw(Game1.AllTextures.TallGrass, DestinationRectangle, Type3SourceRectangle,
                        Color.White, Rotation, new Vector2(8, 24), SpriteEffects.None, .5f + (DestinationRectangle.Y  ) * .0000001f + YOffSet);
                    break;
            }
        }

        public void Shuff(GameTime gameTime, int direction)
        {
            if(!ShuffDirectionPicked)
            {
                if (direction == (int)Dir.Up || direction == (int)Dir.Down)
                {
                    this.ShuffDirection = (Dir)Game1.Utility.RGenerator.Next(2, 4);
                }
                if(direction == (int)Dir.Right)
                {
                    this.ShuffDirection = Dir.Right;
                }
                if (direction == (int)(Dir.Left))
                {
                    this.ShuffDirection = Dir.Left;
                }

            }
            else
            {
                if (ShuffDirection == Dir.Right)
                {
                    if (this.Rotation < RotationCap + .5)
                    {
                        this.Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds * ShuffSpeed;
                    }
                    else if (Game1.Player.Rectangle.Intersects(this.DestinationRectangle))
                    {

                    }
                    else
                    {
                        this.StartShuff = false;
                    }


                }
                else if (ShuffDirection == Dir.Left)
                {
                    if (this.Rotation > RotationCap - 1)
                    {
                        this.Rotation -= (float)gameTime.ElapsedGameTime.TotalSeconds * ShuffSpeed;
                    }
                    else if (Game1.Player.Rectangle.Intersects(this.DestinationRectangle))
                    {

                    }
                    else
                    {
                        this.StartShuff = false;
                    }
                }
            }

        }

        public void RotateBackToOrigin(GameTime gameTime)
        {
            if (this.Rotation > 0 )
            {
                this.Rotation -= (float)gameTime.ElapsedGameTime.TotalSeconds /2;
            }
            else if(Rotation < 0)
            {
                this.Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds / 2;
            }
            else
            {
                this.StartShuff = false;
            }
        }
    }
}

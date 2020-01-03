using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SecretProject.Class.SpriteFolder
{
    public class AnimatedSprite
    {
        [XmlIgnore]
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private double timer;
        public double speed;

        public float MyDepth { get; set; }

        [XmlIgnore]
        protected Texture2D rectangleTexture;

        public bool ShowRectangle { get; set; }

        public int HitBoxFrames { get; set; }

        public int DesiredColumnStart { get; set; }

        public bool IsAnimating { get; set; }

        public int AdjustedLocationX { get; set; } = 0;
        public int AdjustedLocationY { get; set; } = 0;

        public AnimatedSprite(GraphicsDevice graphicsDevice, Texture2D texture, int rows, int columns, int hitBoxFrames)
        {
            this.Texture = texture;
            this.Rows = rows;
            this.Columns = columns;
            currentFrame = 0;
            totalFrames = this.Rows * this.Columns;
            speed = 0.15D;
            timer = speed;
            this.HitBoxFrames = hitBoxFrames;
            rectangleTexture = texture;
            SetRectangleTexture(graphicsDevice, texture);


        }

        //constructor for more complicated sprite atlases
        public AnimatedSprite(GraphicsDevice graphicsDevice, Texture2D texture, int rows, int columns, int hitBoxFrames, int desiredColumnStart, int desiredRowStart, int desiredColumnFinish)
        {
            this.Texture = texture;
            this.Rows = rows;
            this.Columns = columns;
            currentFrame = 0;
            //totalFrames = Rows * Columns;
            totalFrames = desiredColumnFinish - desiredColumnStart * rows;
            speed = 0.10D;
            timer = speed;
            this.DesiredColumnStart = desiredColumnStart;
            this.HitBoxFrames = hitBoxFrames;
            rectangleTexture = texture;
            SetRectangleTexture(graphicsDevice, texture);


        }

        public AnimatedSprite()
        {

        }
        //rectangle constructor


        private void SetRectangleTexture(GraphicsDevice graphicsDevice, Texture2D texture)
        {

            var Colors = new List<Color>();
            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < (texture.Width / this.HitBoxFrames); x++)
                {
                    if (x == 0 || //left side
                        y == 0 || //top side
                        x == texture.Width / this.HitBoxFrames - 1 || //right side
                        y == texture.Height - 1) //bottom side
                    {
                        Colors.Add(new Color(255, 255, 255, 255));
                    }
                    else
                    {
                        Colors.Add(new Color(0, 0, 0, 0));

                    }

                }
            }
            rectangleTexture = new Texture2D(graphicsDevice, texture.Width / this.HitBoxFrames, texture.Height);
            rectangleTexture.SetData<Color>(Colors.ToArray());

        }

        public void Update(GameTime gameTime)
        {

            timer -= gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0)
            {
                currentFrame++;
                timer = speed;
            }
            if (currentFrame == totalFrames)
                currentFrame = 0;

        }

        public void PlayOnce(GameTime gameTime)
        {


            timer -= gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0)
            {
                currentFrame++;
                timer = speed;
            }
            if (currentFrame == totalFrames)
            {
                currentFrame = 0;
                this.IsAnimating = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {
            this.MyDepth = layerDepth;
            int width = this.Texture.Width / this.Columns;
            int height = this.Texture.Height / this.Rows;
            int row = (int)((float)currentFrame / (float)this.Columns);
            int column = (currentFrame % this.Columns) + this.DesiredColumnStart;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X + this.AdjustedLocationX, (int)location.Y + this.AdjustedLocationY, width, height);


            spriteBatch.Draw(this.Texture, destinationRectangle: destinationRectangle, sourceRectangle: sourceRectangle, color: Color.White, layerDepth: this.MyDepth);
            if (this.ShowRectangle)
            {
                if (rectangleTexture != null)
                {
                    spriteBatch.Draw(rectangleTexture, location, color: Color.White, layerDepth: this.MyDepth);
                }


            }
        }
        public void setFrame(int newFrame)
        {
            currentFrame = newFrame;
        }
    }
}
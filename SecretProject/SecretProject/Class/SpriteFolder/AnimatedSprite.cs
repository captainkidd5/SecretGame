﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SecretProject.Class.SpriteFolder
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private double timer;
        private double speed;

        public float MyDepth { get; set; }

        protected Texture2D rectangleTexture;

        public bool ShowRectangle { get; set; }

        public int HitBoxFrames { get; set; }

        public AnimatedSprite(GraphicsDevice graphicsDevice, Texture2D texture, int rows, int columns, int hitBoxFrames)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            speed = 0.15D;
            timer = speed;
            this.HitBoxFrames = hitBoxFrames;
            this.rectangleTexture = texture;
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
                for (int x = 0; x < (texture.Width / HitBoxFrames); x++)
                {
                    if (x == 0 || //left side
                        y == 0 || //top side
                        x == texture.Width / HitBoxFrames - 1 || //right side
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
            rectangleTexture = new Texture2D(graphicsDevice, texture.Width / 4, texture.Height);
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

        public void Draw(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {
            this.MyDepth = layerDepth;
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);


            spriteBatch.Draw(Texture, destinationRectangle: destinationRectangle, sourceRectangle: sourceRectangle, color: Color.White, layerDepth: MyDepth);
            if (ShowRectangle)
            {
                if (rectangleTexture != null)
                {
                    spriteBatch.Draw(rectangleTexture, location, color: Color.White, layerDepth: MyDepth);
                }


            }
        }


        public void setFrame(int newFrame)
        {
            currentFrame = newFrame;
        }
    }
}


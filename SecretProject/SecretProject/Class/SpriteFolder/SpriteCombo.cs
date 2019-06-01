﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.SpriteFolder
{
    class SpriteCombo
    {
        public GraphicsDevice Graphics { get; set; }
        public Texture2D AtlasTexture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Rectangle DestinationRectangle { get; set; }

        public float TextureScaleX { get; set; }
        public float TextureScaleY { get; set; }

        public float ColorMultiplier { get; set; } = 1f;

        public bool IsAnimated { get; set; } = false;


        //For Animation use only
        public int CurrentFrame { get; set; }
        public int TotalFrames { get; set; }
        public int TotalRows { get; set; }
        public int TotalColumns { get; set; }
        public int ColumnStart { get; set; }
        public int ColumnFinish { get; set; }
        public int RowStart { get; set; }
        public int RowFinish { get; set; }
        public float AnimationSpeed { get; set; }
        public float AnimationTimer { get; set; }

        //for non animated sprites
        public SpriteCombo(GraphicsDevice graphics, Texture2D atlasTexture, Rectangle sourceRectangle, Rectangle destinationRectangle)
        {
            this.Graphics = graphics;
            this.AtlasTexture = atlasTexture;
            this.SourceRectangle = sourceRectangle;
            this.DestinationRectangle = destinationRectangle;
        }

        //for animated sprites
        public SpriteCombo(GraphicsDevice graphics, Texture2D atlasTexture, int totalRows, int totalColumns, int columnStart, int columnFinish,
            int rowStart, int rowFinish)
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if(IsAnimated)
            {

            }
            else
            {
                spriteBatch.Draw(AtlasTexture, sourceRectangle: SourceRectangle, destinationRectangle: DestinationRectangle, color: Color.White * ColorMultiplier,
                scale: new Vector2(TextureScaleX, TextureScaleY));
            }
            
        }

        

        
    }
}

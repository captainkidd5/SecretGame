﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.Playable.WardrobeStuff.Pieces;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable.WardrobeStuff
{
    public class ToolPiece : ClothingPiece
    {
        public Item Item { get; set; }
        public Sprite ItemSprite { get; set; }

        public bool IsRotational { get; set; }

        public float Rotation { get; set; }
        public float RotationSpeed { get; set; }
        public int RotationDirection { get; set; }

        public Vector2 Origin { get; set; }

        public ToolPiece(Color defaultColor) : base(defaultColor)
        {
            this.Texture = Game1.AllTextures.ItemSpriteSheet;
            this.Color = Color.White;
            this.LayerDepth = .00000015f;
            this.SpriteEffects = SpriteEffects.None;
            this.BaseYOffSet = 12;
            this.Scale = 1f;
            this.Item = Game1.ItemVault.GenerateNewItem(0, null);
            this.ItemSprite = Item.ItemSprite;
            this.SourceRectangle = Item.SourceTextureRectangle;

            this.Color = defaultColor;

        }

        public void ChangeTool(int id, Dir direction)
        {
            this.Item = Game1.ItemVault.GenerateNewItem(id, null);
            this.ItemSprite = Item.ItemSprite;
            this.SourceRectangle = Item.SourceTextureRectangle;
            this.Rotation = -.25f;
            this.Origin = new Vector2(15, 15);

            switch(direction)
            {
                case Dir.Down:
                    IsRotational = false;
                    this.Texture = Game1.AllTextures.ToolAtlas;
                    break;
                case Dir.Up:
                    IsRotational = false;
                    this.Texture = Game1.AllTextures.ToolAtlas;
                    break;
                case Dir.Left:
                    IsRotational = true;
                    this.Texture = Game1.AllTextures.ItemSpriteSheet;
                    this.Rotation = .6f;
                    break;
                case Dir.Right:
                    IsRotational = true;
                    this.Texture = Game1.AllTextures.ItemSpriteSheet;
                    this.Rotation = -.5f;
                    break;
            }

        }

       


        #region ChoppingUpdates

        public override void UpdateChopping(GameTime gameTime, Vector2 position, int currentFrame, Dir direction)
        {


                switch (direction)
                {
                    case Dir.Down:
                        this.SpriteEffects = SpriteEffects.None;
                    this.Position = new Vector2(position.X + 4, position.Y);
                        SwingDown(gameTime, currentFrame);
                        break;
                    case Dir.Up:
                        this.SpriteEffects = SpriteEffects.None;
                    this.Position = new Vector2(position.X + 4, position.Y - 4);
                        SwingUp(gameTime, currentFrame);
                        break;
                    case Dir.Left:
                        this.SpriteEffects = SpriteEffects.None;
                    this.Position = new Vector2(position.X + 3, position.Y +21);
                    this.RotationDirection = -1;
                    SwingRight(gameTime);
                        break;
                    case Dir.Right:
                        this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    this.Position = new Vector2(position.X + 12, position.Y + 20);
                    this.Origin = new Vector2(0, 15);
                    this.RotationDirection = 1;
                    SwingRight(gameTime);
                        break;

                }




            this.OldFrame = currentFrame;

        }

 
        public void SwingRight(GameTime gameTime)
        {
            this.Rotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * .006f * this.RotationDirection;
        }
        private void SwingUp(GameTime gameTime, int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 5;
            this.Row = 0;

            switch (currentFrame)
            {
                case 0:
                    yAdjustment = 0;
                    break;
                case 1:
                    xAdjustment = 16;
                    break;

                case 2:
                    xAdjustment = 32;
                    break;
                case 3:
                    xAdjustment = 32;
                    break;
                case 4:
                    xAdjustment = 32;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        private void SwingDown(GameTime gameTime, int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 0;
            this.Row = 0;

            switch (currentFrame)
            {
                case 0:
                    yAdjustment = 7;
                    break;
                case 1:
                    xAdjustment = 16;
                    yAdjustment = 7;
                    break;

                case 2:
                    xAdjustment = 32;
                    yAdjustment = 7;
                    break;
                case 3:
                    xAdjustment = 48;
                    yAdjustment =7;
                    break;
                case 4:
                    xAdjustment = 64;
                    yAdjustment = 3;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }

        #endregion

        public override void Draw(SpriteBatch spriteBatch, float yLayerHeight)
        {  
            if(this.IsRotational)
            {
                this.ItemSprite.DrawRotationalSprite(spriteBatch, this.Position, this.Rotation, this.Origin, yLayerHeight + this.LayerDepth, this.SpriteEffects, 1f);
            }
            else
            {
                spriteBatch.Draw(this.Texture, new Vector2(this.Position.X, this.Position.Y + this.BaseYOffSet * this.Scale), this.SourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, this.SpriteEffects, .9f);
            }
            
        }


        public override void Load(BinaryReader reader)
        {

        }

        public override void Save(BinaryWriter writer)
        {


        }


    }
}

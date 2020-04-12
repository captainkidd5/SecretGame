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

            if(direction == Dir.Left)
            {
                this.Rotation =.6f;
            }
            else if(direction == Dir.Right)
            {
                this.Rotation = -.5f;
            }
        }

       


        #region ChoppingUpdates

        public override void UpdateChopping(GameTime gameTime, Vector2 position, int currentFrame, Dir direction)
        {


                switch (direction)
                {
                    case Dir.Down:
                        this.SpriteEffects = SpriteEffects.None;
                        SwingDown(gameTime);
                        break;
                    case Dir.Up:
                        this.SpriteEffects = SpriteEffects.None;
                        SwingUp(gameTime);
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
            this.Rotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * .004f * this.RotationDirection;
        }
        private void SwingUp(GameTime gameTime)
        {

        }
        private void SwingDown(GameTime gameTime)
        {

        }

        #endregion

        public override void Draw(SpriteBatch spriteBatch, float yLayerHeight)
        {  
            this.ItemSprite.DrawRotationalSprite(spriteBatch, this.Position, this.Rotation, this.Origin,.9f, this.SpriteEffects, 1f);
        }


        public override void Load(BinaryReader reader)
        {

        }

        public override void Save(BinaryWriter writer)
        {


        }


    }
}

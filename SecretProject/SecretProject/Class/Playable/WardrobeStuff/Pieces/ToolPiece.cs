using Microsoft.Xna.Framework;
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

            this.Color = defaultColor;

        }
      

        #region ChoppingUpdates
        protected override void UpdateChopDown(int currentFrame)
        {
            this.BaseYOffSet = 12;
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 0;
            this.Row = 1;

            switch (currentFrame)
            {
                case 0:
                    yAdjustment = 0;
                    break;
                case 1:
                    xAdjustment = 16;
                    yAdjustment = 1;
                    break;

                case 2:
                    xAdjustment = 32;
                    yAdjustment = 1;
                    break;
                case 3:
                    xAdjustment = 48;
                    yAdjustment = 0;
                    break;
                case 4:
                    xAdjustment = 64;
                    yAdjustment = -1;
                    break;

            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
            this.Row = 0;
        }

        protected override void UpdateChopUp(int currentFrame)
        {
            this.BaseYOffSet = 16;
        }

        protected override void UpdateChopRight(int currentFrame)
        {
            this.BaseYOffSet = 12;
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 5;
            this.Row = 1;
            switch (currentFrame)
            {
                case 0:
                    yAdjustment = 0;
                    break;
                case 1:
                    xAdjustment = 16;
                    yAdjustment = 1;
                    break;

                case 2:
                    xAdjustment = 32;
                    yAdjustment = 1;
                    break;
                case 3:
                    xAdjustment = 48;
                    yAdjustment = 0;
                    break;
                case 4:
                    xAdjustment = 64;
                    yAdjustment = 1;
                    break;
                case 5:
                    xAdjustment = 80;
                    yAdjustment = 1;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
            this.Row = 0;
        }
        #endregion

        public override void Draw(SpriteBatch spriteBatch, float yLayerHeight)
        {
            spriteBatch.Draw(this.Texture, new Vector2(this.Position.X, this.Position.Y + this.BaseYOffSet * this.Scale), this.SourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, this.SpriteEffects, yLayerHeight + this.LayerDepth);
        }

        public override void DrawForCreationWindow(SpriteBatch spriteBatch)
        {
            this.Scale = 6f;
            spriteBatch.Draw(this.Texture, new Vector2(this.Position.X, this.Position.Y + this.BaseYOffSet * this.Scale), this.SourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, this.SpriteEffects, .9f + this.LayerDepth);
        }



        public override void Load(BinaryReader reader)
        {
            this.Row = reader.ReadInt32();

        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(this.Row);


        }


    }
}

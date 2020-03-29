using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Playable.WardrobeStuff.Pieces;
using SecretProject.Class.SavingStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable.WardrobeStuff
{
    public class ShoesPiece : ClothingPiece
    {


        public ShoesPiece()
        {
            this.Texture = Game1.AllTextures.ShoesAtlas;
            this.Color = Color.Brown;
            this.LayerDepth = .00000008f;
            this.SpriteEffects = SpriteEffects.None;
            this.BaseYOffSet = 15;

        }

        #region DIRECTION UPDATES
        public override void UpdateDown(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 0;
            switch (currentFrame)
            {
                case 0:
                    break;
                case 1:
                    xAdjustment = 16;
                    break;

                case 2:
                    xAdjustment = 32;
                    break;
                case 3:
                    break;
                case 4:
                    xAdjustment = 16;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case 5:
                    xAdjustment = 32;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;

            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        public override void UpdateUp(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 6;

            switch (currentFrame)
            {
                case 0:
                    yAdjustment = 2;
                    break;
                case 1:
                    xAdjustment = 16;
                    yAdjustment = 2;
                    break;

                case 2:
                    xAdjustment = 32;
                    yAdjustment = 2;
                    break;
                case 3:
                    yAdjustment = 2;
                    break;
                case 4:
                    xAdjustment = 16;
                    yAdjustment = 2;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case 5:
                    xAdjustment = 32;
                    yAdjustment = 2;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }

        public override void UpdateRight(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 3;

            switch (currentFrame)
            {
                case 0:
                    break;
                case 1:
                    xAdjustment = 16;
                    yAdjustment = -3;
                    break;

                case 2:
                    xAdjustment = 32;
                    yAdjustment = -3;
                    break;
                case 3:
                    break;
                case 4:
                    xAdjustment = 16;
                    yAdjustment = -2;
                    break;
                case 5:
                    xAdjustment = 32;
                    yAdjustment = -2;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        #endregion


       
    }
}

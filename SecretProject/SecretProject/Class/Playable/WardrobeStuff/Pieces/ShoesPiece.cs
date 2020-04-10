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


        public ShoesPiece(Color defaultColor) : base(defaultColor)
        {
            this.Texture = Game1.AllTextures.ShoesAtlas;
            this.Color = Color.Brown;
            this.LayerDepth = .00000008f;
            this.SpriteEffects = SpriteEffects.None;
            this.BaseYOffSet = 18;
            this.Scale = 1f;


        }

        #region DIRECTION UPDATES
        protected override void UpdateWalkDown(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 0;
            switch (currentFrame)
            {
                case 0:
                    yAdjustment = 1;
                    break;
                case 1:
                    xAdjustment = 16;
                    yAdjustment = 1;
                    break;

                case 2:
                    xAdjustment = 32;
                    yAdjustment = 2;
                    break;
                case 3:
                    yAdjustment = 1;
                    break;
                case 4:
                    xAdjustment = 16;
                    yAdjustment = 1;

                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case 5:
                     yAdjustment = 2;
                    xAdjustment = 32;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;

            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        protected override void UpdateWalkUp(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column =9;

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
                    yAdjustment = 3;
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
                    yAdjustment = 3;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }

        protected override void UpdateWalkRight(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 3;

            switch (currentFrame)
            {
                case 0:
                    yAdjustment = -1;
                    break;
                case 1:
                    xAdjustment = 16;
                    yAdjustment = 0;
                    break;

                case 2:
                    xAdjustment = 32;
                    yAdjustment = 1;
                    break;
                case 3:
                    xAdjustment = 48;
                    yAdjustment = -1;
                    break;
                case 4:
                    xAdjustment = 64;
                    yAdjustment = 0;
                    break;
                case 5:
                    xAdjustment = 80;
                    yAdjustment = 1;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        #endregion
        #region ChoppingUpdates
        protected override void UpdateChopDown(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 0;
            this.Row = 1;
            switch (currentFrame)
            {
                case 0:
                    yAdjustment = 1;
                    break;
                case 1:
                    xAdjustment = 16;
                    yAdjustment = 1;
                    break;

                case 2:
                    xAdjustment = 16;
                    yAdjustment = 1;
                    break;
                case 3:
                    xAdjustment = 16;
                    yAdjustment = 1;
                    break;
                case 4:
                    xAdjustment = 0;
                    yAdjustment = 1;

                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case 5:
                    yAdjustment = 2;
                    xAdjustment = 32;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;

            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
            this.Row = 0;
        }
        #endregion


    }
}

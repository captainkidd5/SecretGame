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
    public class ArmsPiece : ClothingPiece
    {


        public ArmsPiece()
        {
            this.Texture = Game1.AllTextures.ArmsAtlas;
            this.Color = Color.Green;
            this.LayerDepth = .00000015f;
            this.SpriteEffects = SpriteEffects.None;
            this.BaseYOffSet = 12;
            this.Scale = 1f;

            this.RedValue = 255;
            this.GreenValue = 255;
            this.BlueValue = 255;
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
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
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
            int column = 9;

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
                    yAdjustment = -2;
                    break;

                case 2:
                    xAdjustment = 32;
                    yAdjustment = -2;
                    break;
                case 3:
                    xAdjustment = 48;
                    yAdjustment = -2;
                    break;
                case 4:
                    xAdjustment = 64;
                    yAdjustment = -2;
                    break;
                case 5:
                    xAdjustment = 80;
                    yAdjustment = -2;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        #endregion


    }
}

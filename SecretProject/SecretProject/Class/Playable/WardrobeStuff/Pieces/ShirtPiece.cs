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
    public class ShirtPiece : ClothingPiece
    {


        public ShirtPiece(Color defaultColor) : base(defaultColor)
        {
            this.Texture = Game1.AllTextures.ShirtAtlas;
            this.Color = Color.White;
            this.LayerDepth = .00000010f;
            this.SpriteEffects = SpriteEffects.None;
            this.Row = 0;
            this.BaseYOffSet = 12;
            this.Scale = 1f;

            this.Color = defaultColor;

        }

        #region DIRECTION UPDATES
        public override void UpdateWalkDown(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 0;
            switch (currentFrame)
            {
                case 0:
                    yAdjustment = -3;
                    break;
                case 1:
                    yAdjustment = -3;
                    break;

                case 2:
                    yAdjustment = -3;
                    break;
                case 3:

                    yAdjustment = -3;
                    break;
                case 4:

                    yAdjustment = -3;
                    break;
                case 5:

                    yAdjustment = -3;
                    break;

            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        public override void UpdateWalkUp(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 2;

            switch (currentFrame)
            {
                case 0:
                    yAdjustment = -1;
                    break;
                case 1:
                    yAdjustment = 0;
                    break;

                case 2:
                    yAdjustment = -1;
                    break;
                case 3:
                    yAdjustment = -1;
                    break;
                case 4:
                    yAdjustment = 0;
                    break;
                case 5:
                    yAdjustment = -1;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }

        public override void UpdateWalkRight(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 1;

            switch (currentFrame)
            {
                case 0:
                    yAdjustment = -1;
                    break;
                case 1:
                    yAdjustment = -1;
                    break;

                case 2:
                    yAdjustment = 0;
                    break;
                case 3:
                    yAdjustment = -1;
                    break;
                case 4:
                    yAdjustment = -1;
                    break;
                case 5:
                    yAdjustment = 0;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        #endregion



    }
}

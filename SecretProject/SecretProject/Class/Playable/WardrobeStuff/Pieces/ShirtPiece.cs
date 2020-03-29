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


        public ShirtPiece()
        {
            this.Texture = Game1.AllTextures.ShirtAtlas;
            this.Color = Color.White;
            this.LayerDepth = .00000010f;
            this.SpriteEffects = SpriteEffects.None;
            this.Row = 0;
            this.BaseYOffSet = 12;
            this.Scale = 1f;

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
                    yAdjustment = -3;
                    break;
                case 1:
                    yAdjustment = -3;
                    break;

                case 2:
                    yAdjustment = -4;
                    break;
                case 3:

                    yAdjustment = -3;
                    break;
                case 4:

                    yAdjustment = -3;
                    break;
                case 5:

                    yAdjustment = -4;
                    break;

            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        public override void UpdateUp(int currentFrame)
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
                    yAdjustment = -1;
                    break;

                case 2:
                    yAdjustment = -2;
                    break;
                case 3:
                    break;
                case 4:
                    yAdjustment = -1;
                    break;
                case 5:
                    yAdjustment = -2;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }

        public override void UpdateRight(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 1;

            switch (currentFrame)
            {
                case 0:
                    yAdjustment = -2;
                    break;
                case 1:
                    yAdjustment = -3;
                    break;

                case 2:
                    yAdjustment = -3;
                    break;
                case 3:
                    yAdjustment = -2;
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
        #endregion



    }
}

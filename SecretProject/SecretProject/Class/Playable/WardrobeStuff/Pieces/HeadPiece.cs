using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    public class HeadPiece : ClothingPiece
    {


        public HeadPiece(GraphicsDevice graphics, ContentManager content, Texture2D texture, Color defaultColor) : base(graphics, content,texture, defaultColor)
        {
            this.Color = Color.White;
            this.LayerDepth = .000000007f;
            this.SpriteEffects = SpriteEffects.None;
            this.BaseYOffSet = 2;
            this.Scale = 1f;




            this.Color = defaultColor;
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
                    break;
                case 1:
                    yAdjustment = 1;
                    break;

                case 2:
                    yAdjustment = 2;
                    break;
                case 3:
                    break;
                case 4:
                    yAdjustment = 1;
                    break;
                case 5:
                    yAdjustment = 2;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        protected override void UpdateWalkUp(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 2;

            switch (currentFrame)
            {
                case 0:
                    break;
                case 1:
                    yAdjustment = 1;
                    break;

                case 2:
                    yAdjustment = 2;
                    break;
                case 3:
                    yAdjustment = 0;
                    break;
                case 4:
                    yAdjustment = 1;
                    break;
                case 5:
                    yAdjustment = 2;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }

        protected override void UpdateWalkRight(int currentFrame)
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
                    yAdjustment = 0;
                    break;

                case 2:
                    yAdjustment =1;
                    break;
                case 3:
                    yAdjustment = -1;
                    break;
                case 4:
                    yAdjustment = 0;
                    break;
                case 5:
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
            switch (currentFrame)
            {
                case 0:
                    break;
                case 1:
                    yAdjustment = 1;
                    break;

                case 2:
                    yAdjustment = 1;
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

        protected override void UpdateChopUp(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 2;

            switch (currentFrame)
            {
                case 0:
                    break;
                case 1:
                    yAdjustment = 1;
                    break;

                case 2:
                    yAdjustment = 1;
                    break;
                case 3:
                    yAdjustment = 0;
                    break;
                case 4:
                    yAdjustment = -1;
                    break;

            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }

        protected override void UpdateChopRight(int currentFrame)
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
                    yAdjustment = 0;
                    break;

                case 2:
                    yAdjustment = 0;
                    break;
                case 3:
                    yAdjustment = -1;
                    break;
                case 4:
                    yAdjustment = -2;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        #endregion
        #region SwordSwipeUpdates
        protected override void UpdateSwordSwipeDown(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 0;
            switch (currentFrame)
            {
                case 0:
                    break;
                case 1:
                    yAdjustment = 0;
                    break;

                case 2:
                    yAdjustment = 1;
                    break;
                case 3:
                    break;
                case 4:
                    yAdjustment = 0;
                    break;
                case 5:
                    yAdjustment =0;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }

        protected override void UpdateSwordSwipeUp(int currentFrame)
        {
        }

        protected override void UpdateSwordSwipeRight(int currentFrame)
        {
        }
        #endregion
        #region Pick Up Item
        protected override void UpdatePickUpItemDown(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 0;
            switch (currentFrame)
            {
                case 0:
                    yAdjustment = -1;
                    break;
                case 1:
                    yAdjustment = -2;
                    break;

                case 2:
                    yAdjustment = -3;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }

        protected override void UpdatePickUpItemUp(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 2;
            switch (currentFrame)
            {
                case 0:
                    yAdjustment = -2;
                    break;
                case 1:
                    yAdjustment = -3;
                    break;

                case 2:
                    yAdjustment = -4;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }

        protected override void UpdatePickUpItemRight(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 1;
            switch (currentFrame)
            {
                case 0:
                    break;
                case 1:
                    yAdjustment = -1;
                    break;

                case 2:
                    yAdjustment = -2;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        #endregion


    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Playable.WardrobeStuff.Pieces;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.UI.MainMenuStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable.WardrobeStuff
{
    public class PantsPiece : ClothingPiece
    {
        public List<Color> Colors { get; set; }
        public int ColorIndex { get; set; }

        public PantsPiece(Color defaultColor) : base(defaultColor)
        {
            this.Texture = Game1.AllTextures.PantsAtlas;
            this.Color = Color.White;
            this.LayerDepth = .00000009f;
            this.SpriteEffects = SpriteEffects.None;
            this.BaseYOffSet = 18;
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
                    xAdjustment = 32;
                    yAdjustment = 2;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        protected override void UpdateWalkUp(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 9;

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
                    yAdjustment = 1;
                    break;
                case 4:
                    xAdjustment = 32;
                    yAdjustment = 1;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case 5:
                    xAdjustment = 32;
                    yAdjustment = 2;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
            this.Row = 0;
        }

        protected override void UpdateChopUp(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 6;
            this.Row = 1;
            switch (currentFrame)
            {
                case 0:
                    yAdjustment = 2;
                    break;
                case 1:
                    xAdjustment = 16;
                    yAdjustment = 3;
                    break;

                case 2:
                    xAdjustment = 16;
                    yAdjustment = 3;
                    break;
                case 3:
                    yAdjustment = 3;
                    break;
                case 4:
                    yAdjustment = 1;
                    xAdjustment = 32;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
            this.Row = 0;
        }
        protected override void UpdateChopRight(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 3;
            this.Row = 1;

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
                    xAdjustment = 16;
                    yAdjustment = 0;
                    break;
                case 3:
                    xAdjustment = 16;
                    yAdjustment = -1;
                    break;
                case 4:
                    xAdjustment = 32;
                    yAdjustment = -2;
                    break;

            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
            this.Row = 0;
        }
        #endregion


        #region SwordSwipeUpdates
        protected override void UpdateSwordSwipeDown(int currentFrame)
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
                    yAdjustment = 1;
                    break;
                case 4:
                    xAdjustment = 32;
                    yAdjustment = 1;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case 5:
                    xAdjustment = 32;
                    yAdjustment = 2;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
            this.Row = 0;
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
            this.Row = 2;
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
                    yAdjustment = 1;
                    break;
                case 3:
                    yAdjustment = 1;
                    break;
                case 4:
                    xAdjustment = 32;
                    yAdjustment = 1;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case 5:
                    xAdjustment = 32;
                    yAdjustment = 2;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
            this.Row = 0;
        }

        protected override void UpdatePickUpItemUp(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 6;
            this.Row = 2;
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
                    yAdjustment = 1;
                    break;
                case 3:
                    yAdjustment = 1;
                    break;
                case 4:
                    xAdjustment = 32;
                    yAdjustment = 1;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case 5:
                    xAdjustment = 32;
                    yAdjustment = 2;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
            this.Row = 0;
        }

        protected override void UpdatePickUpItemRight(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 3;
            this.Row = 2;
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
                    yAdjustment = 1;
                    break;
                case 3:
                    yAdjustment = 1;
                    break;
                case 4:
                    xAdjustment = 32;
                    yAdjustment = 1;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case 5:
                    xAdjustment = 32;
                    yAdjustment = 2;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
            this.Row = 0;
        }
        #endregion

        public void ChangePantsColor(CycleDirection direction)
        {
            this.ColorIndex += (int)direction;
            if (this.ColorIndex < this.Colors.Count)
            {
                // this.HairIndex++;
            }
            else
            {
                this.ColorIndex = 0;
            }

            if (this.ColorIndex < 0)
            {
                this.ColorIndex = this.Colors.Count - 1;
            }

            this.Color = this.Colors[this.ColorIndex];


        }


    }
}

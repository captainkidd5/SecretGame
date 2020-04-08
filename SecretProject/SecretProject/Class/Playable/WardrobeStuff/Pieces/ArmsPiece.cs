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
        public Color DarkestShirtReplaceColor { get; set; }
        public Color MediumShirtReplaceColor { get; set; }
        public Color LightShirtReplaceColor { get; set; }

        public List<Color> ShirtReplacementColors { get; set; }

        public List<Color> SkinReplacementColors { get; set; }

        public ArmsPiece(Color defaultColor) : base(defaultColor)
        {
            this.Texture = Game1.AllTextures.ArmsAtlas;
            this.Color = Color.Green;
            this.LayerDepth = .00000015f;
            this.SpriteEffects = SpriteEffects.None;
            this.BaseYOffSet = 12;
            this.Scale = 1f;

            this.Color = defaultColor;


            this.DarkestShirtReplaceColor = new Color(48, 26, 22);
            this.MediumShirtReplaceColor = new Color(172, 50, 50);
            this.LightShirtReplaceColor = new Color(203, 50, 50);
            this.ShirtReplacementColors = new List<Color>()
            { DarkestShirtReplaceColor, MediumShirtReplaceColor, LightShirtReplaceColor};
            

        }

        public void SetArmSleeves(Color color)
        {

            Color[] armData = new Color[this.Texture.Width * Texture.Height];
            Texture.GetData(armData);


            for(int i = 0; i < this.ShirtReplacementColors.Count; i++)
            {
                bool wasReplaced = false;
                Color newColor = Color.White;
                for (int j = 0; j < armData.Length; j++)
                {
                    if(armData[j] == this.ShirtReplacementColors[i])
                    {
                        wasReplaced = true;
                        
                        newColor = new Color(color.R, color.G, color.B);
                        switch(i)
                        {
                            case 0:
                                newColor = Game1.Player.Wardrobe.ChangeColorLevel(color, Brightness.Dark);
                                break;
                            case 1:
                                newColor = Game1.Player.Wardrobe.ChangeColorLevel(color, Brightness.Normal);
                                break;
                            case 2:
                                newColor = Game1.Player.Wardrobe.ChangeColorLevel(color, Brightness.Bright);
                                break;
                        }
                        armData[j] = newColor;
                    }
                }

                if(wasReplaced)
                {
                    ShirtReplacementColors[i] = newColor; //need to update new replacement colors with the new data so it can be changed again in the future.
                }
            }

            this.Texture.SetData(armData);
        }

        public void SetSkinTone(Color color)
        {

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
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    yAdjustment = 1;
                    break;
                case 4:
                    xAdjustment = 16;
                    yAdjustment = 1;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case 5:
                    xAdjustment = 32;
                    yAdjustment = 1;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        public override void UpdateWalkUp(int currentFrame)
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

        public override void UpdateWalkRight(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 3;

            switch (currentFrame)
            {
                case 0:
                    yAdjustment = 0;
                    break;
                case 1:
                    xAdjustment = 16;
                    yAdjustment = 0;
                    break;

                case 2:
                    xAdjustment = 32;
                    yAdjustment = 0;
                    break;
                case 3:
                    xAdjustment = 48;
                    yAdjustment = 0;
                    break;
                case 4:
                    xAdjustment = 64;
                    yAdjustment = 0;
                    break;
                case 5:
                    xAdjustment = 80;
                    yAdjustment = 0;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
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


    }
}

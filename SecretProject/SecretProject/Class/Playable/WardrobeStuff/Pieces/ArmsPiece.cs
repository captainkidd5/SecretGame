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

        

        public void SetSkinTone(Color color)
        {

        }

        #region DIRECTION UPDATES
        protected override void UpdateWalkDown(int currentFrame)
        {
            this.BaseYOffSet = 12;
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
                    yAdjustment = 2;
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        protected override void UpdateWalkUp(int currentFrame)
        {
            this.BaseYOffSet = 12;
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
            this.BaseYOffSet = 12;
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
        }
        #endregion

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
            this.Color = new Color(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());

            int shirtReplacementColorsCount = reader.ReadInt32();
            this.ShirtReplacementColors.Clear();
            Game1.Player.Wardrobe.LoadColorList(reader, this.ShirtReplacementColors);

        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(this.Row);
            Game1.Player.Wardrobe.SaveColor(writer, this.Color);

            Game1.Player.Wardrobe.SaveColorList(writer, this.ShirtReplacementColors);



        }


    }
}

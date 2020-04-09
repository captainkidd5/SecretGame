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
    public class EyePiece : ClothingPiece
    {
        public List<Color> EyeColors { get; set; }
        public int EyeColorIndex { get; set; }

        public Color CurrentEyeColor { get; set; }


        public EyePiece(Color defaultColor) : base(defaultColor)
        {
            this.Texture = Game1.AllTextures.EyesAtlas;
            this.Color = Color.White;
            this.LayerDepth = .00000014f;
            this.SpriteEffects = SpriteEffects.None;
            this.BaseYOffSet = 2;
            this.Scale = 1f;
            this.EyeColors = new List<Color>()
            {
                new Color(139,69,19),//saddle brown
                new Color(218,165,32) //goldenrod
            };



            this.Color = defaultColor;

            this.CurrentEyeColor = new Color(95, 205, 228);
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
                    yAdjustment = 1;
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

        public virtual void ChangeEyeColor(CycleDirection direction)
        {
            this.EyeColorIndex += (int)direction;
            if(EyeColorIndex > this.EyeColors.Count - 1)
            {
                this.EyeColorIndex = 0;
            }
            else if(EyeColorIndex < 0)
            {
                EyeColorIndex = this.EyeColors.Count - 1;
            }

            SetEyeData(EyeColorIndex);

        }

        public void SetEyeData( int colorIndex)
        {
            Color[] data = new Color[this.Texture.Width * Texture.Height];
            Texture.GetData(data);


            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == this.CurrentEyeColor)
                {
                    data[i] = this.EyeColors[colorIndex];
                }

            }

            this.CurrentEyeColor = this.EyeColors[colorIndex];


            this.Texture.SetData(data);
        }

        public override void Load(BinaryReader reader)
        {
            base.Load(reader);


            SetEyeData(reader.ReadInt32());
        }

        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);


            writer.Write(this.EyeColorIndex);

        }

    }
}

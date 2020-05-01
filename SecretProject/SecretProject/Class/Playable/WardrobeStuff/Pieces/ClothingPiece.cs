using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.UI.MainMenuStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable.WardrobeStuff.Pieces
{
    public class ClothingPiece : ISaveable
    {
        public Texture2D Texture { get; set; }

        public int Row { get; set; }
        public int Column { get; set; }

        public Vector2 Position { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public float Scale { get; set; }

        public int OldFrame { get; set; }
        
        public float LayerDepth { get; set; }

        public SpriteEffects SpriteEffects { get; set; }
        public int BaseYOffSet { get; set; }

        //COLORS
        public Color Color { get; set; }


        public ClothingPiece(Color defaultColor)
        {
            this.Color = defaultColor;
        }

        public virtual void Cycle(CycleDirection direction)
        {
            this.Row += (int)direction;
            if(Row >= this.Texture.Height / 16 )
            {
                this.Row = 0;
            }
            else if(this.Row <0)
            {
                this.Row = this.Texture.Height / 16 - 1;
            }
        }

        public void Update(GameTime gameTime, Vector2 position, int currentFrame, Dir direction)
        {

            if (this.OldFrame != currentFrame)
            {
                switch (direction)
                {
                    case Dir.Down:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateWalkDown(currentFrame);
                        break;
                    case Dir.Up:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateWalkUp(currentFrame);
                        break;
                    case Dir.Left:
                        this.SpriteEffects = SpriteEffects.FlipHorizontally;
                        UpdateWalkRight(currentFrame);
                        break;
                    case Dir.Right:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateWalkRight(currentFrame);
                        break;

                }


            }
            this.Position = position;

            this.OldFrame = currentFrame;

        }

        public virtual void UpdateChopping(GameTime gameTime, Vector2 position, int currentFrame, Dir direction)
        {

            if (this.OldFrame != currentFrame)
            {
                switch (direction)
                {
                    case Dir.Down:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateChopDown(currentFrame);
                        break;
                    case Dir.Up:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateChopUp(currentFrame);
                        break;
                    case Dir.Left:
                        this.SpriteEffects = SpriteEffects.FlipHorizontally;
                        UpdateChopRight(currentFrame);
                        break;
                    case Dir.Right:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateChopRight(currentFrame);
                        break;

                }


            }
            this.Position = position;

            this.OldFrame = currentFrame;

        }

        public virtual void UpdateSwordSwipe(GameTime gameTime, Vector2 position, int currentFrame, Dir direction)
        {

            if (this.OldFrame != currentFrame)
            {
                switch (direction)
                {
                    case Dir.Down:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateSwordSwipeDown(currentFrame);
                        break;
                    case Dir.Up:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateSwordSwipeUp(currentFrame);
                        break;
                    case Dir.Left:
                        this.SpriteEffects = SpriteEffects.FlipHorizontally;
                        UpdateSwordSwipeRight(currentFrame);
                        break;
                    case Dir.Right:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateSwordSwipeRight(currentFrame);
                        break;

                }


            }
            this.Position = position;

            this.OldFrame = currentFrame;

        }

        public virtual void UpdatePickUpItem(GameTime gameTime, Vector2 position, int currentFrame, Dir direction)
        {

            if (this.OldFrame != currentFrame)
            {
                switch (direction)
                {
                    case Dir.Down:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdatePickUpItemDown(currentFrame);
                        break;
                    case Dir.Up:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdatePickUpItemUp(currentFrame);
                        break;
                    case Dir.Left:
                        this.SpriteEffects = SpriteEffects.FlipHorizontally;
                        UpdatePickUpItemRight(currentFrame);
                        break;
                    case Dir.Right:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdatePickUpItemRight(currentFrame);
                        break;

                }


            }
            this.Position = position;

            this.OldFrame = currentFrame;

        }
        #region WALK
        protected virtual void UpdateWalkDown(int currentFrame)
        {
            
        }

        protected virtual void UpdateWalkUp(int currentFrame)
        {
        }

        protected virtual void UpdateWalkRight(int currentFrame)
        {
        }
        #endregion

        #region Chop
        protected virtual void UpdateChopDown(int currentFrame)
        {

        }

        protected virtual void UpdateChopUp(int currentFrame)
        {
        }

        protected virtual void UpdateChopRight(int currentFrame)
        {
        }
        #endregion


        #region Sword Swipe
        protected virtual void UpdateSwordSwipeDown(int currentFrame)
        {

        }

        protected virtual void UpdateSwordSwipeUp(int currentFrame)
        {
        }

        protected virtual void UpdateSwordSwipeRight(int currentFrame)
        {
        }
        #endregion

        #region Pick Up Item
        protected virtual void UpdatePickUpItemDown(int currentFrame)
        {

        }

        protected virtual void UpdatePickUpItemUp(int currentFrame)
        {
        }

        protected virtual void UpdatePickUpItemRight(int currentFrame)
        {
        }
        #endregion

        public void UpdateOnceForCreationMenu(int currentFrame)
        {
            UpdateWalkDown(currentFrame);
        }
        public virtual void Draw(SpriteBatch spriteBatch, float yLayerHeight)
        {
            spriteBatch.Draw(this.Texture, new Vector2(this.Position.X, this.Position.Y + this.BaseYOffSet * this.Scale), this.SourceRectangle, this.Color, 0f, Game1.Utility.Origin, this.Scale, this.SpriteEffects, yLayerHeight + this.LayerDepth);
        }

        public virtual void DrawForCreationWindow(SpriteBatch spriteBatch)
        {
            this.Scale = 6f;
            spriteBatch.Draw(this.Texture, new Vector2(this.Position.X, this.Position.Y + this.BaseYOffSet * this.Scale), this.SourceRectangle, this.Color, 0f, Game1.Utility.Origin, this.Scale, this.SpriteEffects, .9f + this.LayerDepth);
        }


        public void UpdateSourceRectangle(int column, int xAdjustment = 0, int yAdjustment = 0)
        {
            this.SourceRectangle = new Rectangle(column * 16 + xAdjustment, this.Row * 16 + yAdjustment, 16, 16);
        }

        public virtual void ChangeColor(int red, int green, int blue)
        {
            this.Color = new Color(red, green, blue);
        }

        public virtual void ChangePartOfTexture(Color color, List<Color> replaceMentColors)
        {

            Color[] textureData = new Color[this.Texture.Width * Texture.Height];
            Texture.GetData(textureData);


            for (int i = 0; i < replaceMentColors.Count; i++)
            {
                bool wasReplaced = false;
                Color newColor = Color.White;
                for (int j = 0; j < textureData.Length; j++)
                {
                    if (textureData[j] == replaceMentColors[i])
                    {
                        wasReplaced = true;

                        newColor = new Color(color.R, color.G, color.B);
                        newColor = Wardrobe.ChangeColorLevel(newColor, i);
                        textureData[j] = newColor;
                    }
                }

                if (wasReplaced)
                {
                    replaceMentColors[i] = newColor; //need to update new replacement colors with the new data so it can be changed again in the future.
                }
            }

            this.Texture.SetData(textureData);
        }


        public virtual void Load(BinaryReader reader)
        {
            this.Row = reader.ReadInt32();
            this.Color = new Color(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
        }

        public virtual void Save(BinaryWriter writer)
        {
            writer.Write(this.Row);
            writer.Write(this.Color.R);
            writer.Write(this.Color.G);
            writer.Write(this.Color.B);
            
        }

        

       
    }
}

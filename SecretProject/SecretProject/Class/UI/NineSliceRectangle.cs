using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public enum RectangleSize
    {
        Tiny = 1,
        Small =2,
        Medium = 5,
        Large =8,
        XL = 15,
        XXL = 20
    }
    public class NineSliceRectangle
    {

        public Rectangle TotalRectangle { get; set; }
        public Vector2 Position { get; set; }

        public List<Rectangle> CombinedRectangle { get; set; }
        public List<Vector2> RectanglePositions { get; set; }

        public int Width { get;private set; }
        public int Height { get; private set; }
        public float Scale { get; set; }

        private readonly Rectangle TopLeftCorner = new Rectangle(1056, 128, 16, 16);
        private readonly Rectangle TopEdge = new Rectangle(1072, 128, 16, 16);
        private readonly Rectangle TopRightCorner = new Rectangle(1088, 128, 16, 16);

        private readonly Rectangle LeftEdge = new Rectangle(1056, 144, 16, 16);
        private readonly Rectangle Center = new Rectangle(1072, 144, 16, 16);
        private readonly Rectangle RightEdge = new Rectangle(1088, 144, 16, 16);

        private readonly Rectangle BottomLeftCorner = new Rectangle(1056, 164, 16, 16);
        private readonly Rectangle BottomEdge = new Rectangle(1072, 164, 16, 16);
        private readonly Rectangle BottomRightCorner = new Rectangle(1088, 164, 16, 16);

       
        public NineSliceRectangle(Vector2 position, string text, float textScale)
        {
            CombinedRectangle = new List<Rectangle>();
            RectanglePositions = new List<Vector2>();
            this.Scale = 2f;


            int totalRequiredWidth = (int)TextBuilder.GetTextLength(text, textScale) + 48;
            int totalRequireHeight = (int)TextBuilder.GetTextHeight(text, textScale) + 32;


            int currentWidth = (int)(LeftEdge.Width * Scale);
            int currentHeight = 0;


            this.Width = AddRow(totalRequiredWidth, position,TopLeftCorner, TopEdge, TopRightCorner);
            currentHeight += (int)(16 * this.Scale);
            position = new Vector2(position.X, position.Y + 16 * this.Scale);

            while (currentHeight <= totalRequireHeight)
            {
                AddRow(totalRequiredWidth, position, LeftEdge, Center, RightEdge);
                currentHeight += (int)(16 * this.Scale);
                position = new Vector2(position.X, position.Y + 16 * this.Scale);
            }

            //AddRow(totalRequiredWidth, position, LeftEdge, Center, RightEdge);
            //currentHeight += (int)(16 * this.Scale * this.Scale);
            //position = new Vector2(position.X, position.Y + 16);


            AddRow(totalRequiredWidth, position, BottomLeftCorner, BottomEdge, BottomRightCorner);
            currentHeight += (int)(16 * this.Scale);
            //position = new Vector2(position.X, position.Y + currentHeight);
            this.Position = new Vector2((int)RectanglePositions[0].X, (int)RectanglePositions[0].Y);

            this.TotalRectangle = new Rectangle((int)Position.X, (int)Position.Y, this.Width, this.Height);
        }


        private void AddRectangle(Rectangle rectangle, Vector2 position)
        {
            this.CombinedRectangle.Add(rectangle);
            this.RectanglePositions.Add(position);
        }

        /// <summary>
        /// Returns the width of a single row. To be used once in the constructor set our total width!
        /// </summary>
        /// <param name="length"></param>
        /// <param name="position"></param>
        /// <param name="left"></param>
        /// <param name="middle"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private int AddRow(int length, Vector2 position, Rectangle left, Rectangle middle, Rectangle right)
        {
            int totalWidth = 0;
            int startingPositionX = (int)position.X;
            int numberNeeded = (int)(length / this.Scale / 16);
            AddRectangle(left, position);
            totalWidth += left.Width;
            startingPositionX += (int)(16 * this.Scale);

            numberNeeded--;

            while (numberNeeded > 1)
            {

                Vector2 newPosition = new Vector2(startingPositionX, position.Y);
                AddRectangle(middle, newPosition);
                totalWidth += middle.Width;
                numberNeeded--;
                startingPositionX += (int)(16 * this.Scale);
            }
            AddRectangle(right, new Vector2(startingPositionX, position.Y));
            totalWidth += right.Width;

            return totalWidth;
        }

        



        public void Draw(SpriteBatch spriteBatch, float layerDepth = .7f)
        {
            for (int i = 0; i < CombinedRectangle.Count; i++)
            {

                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, RectanglePositions[i], CombinedRectangle[i], Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, layerDepth);
            }
        }

        public Vector2 CenterTextHorizontal(string text, float scale)
        {
            float textWidth = TextBuilder.GetTextLength(text, scale);
            float width = (float)this.Width / 2f;
            Vector2 returnVector = new Vector2(this.Position.X + width, this.Position.Y);
            return returnVector;
        }

    }
}

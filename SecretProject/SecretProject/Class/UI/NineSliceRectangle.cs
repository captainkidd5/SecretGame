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
        public List<Rectangle> CombinedRectangle { get; set; }
        public List<Vector2> RectanglePositions { get; set; }

        public int Width { get;private set; }
        public int Height { get; private set; }

        public float Scale { get; set; }

        Rectangle TopLeftCorner = new Rectangle(1056, 128, 16, 16);
        Rectangle TopEdge = new Rectangle(1072, 128, 16, 16);
        Rectangle TopRightCorner = new Rectangle(1088, 128, 16, 16);

        Rectangle LeftEdge = new Rectangle(1056, 144, 16, 16);
        Rectangle Center = new Rectangle(1072, 144, 16, 16);
        Rectangle RightEdge = new Rectangle(1088, 144, 16, 16);

        Rectangle BottomLeftCorner = new Rectangle(1056, 164, 16, 16);
        Rectangle BottomEdge = new Rectangle(1072, 164, 16, 16);
        Rectangle BottomRightCorner = new Rectangle(1088, 164, 16, 16);

       
        public NineSliceRectangle(Vector2 position, string text)
        {
            CombinedRectangle = new List<Rectangle>();
            RectanglePositions = new List<Vector2>();
            this.Scale = 2f;


            int totalRequiredWidth = (int)TextBuilder.GetTextLength(text, this.Scale, 0);
            int totalRequireHeight = (int)TextBuilder.GetTextHeight(text, this.Scale);


            int currentWidth = (int)(LeftEdge.Width * Scale);
            int currentHeight = 0;


            AddRow(totalRequiredWidth, position,TopLeftCorner, TopEdge, TopRightCorner);
            currentHeight += (int)(16 * this.Scale);
            position = new Vector2(position.X, position.Y + currentHeight);
            while (currentHeight < totalRequireHeight)
            {
                AddRow(totalRequiredWidth, position, LeftEdge, Center, RightEdge);
                currentHeight += (int)(16 * this.Scale);
                position = new Vector2(position.X, position.Y + currentHeight);
            }
          //  position = new Vector2(position.X, position.Y + currentHeight);
            AddRow(totalRequiredWidth, position, BottomLeftCorner, BottomEdge, BottomRightCorner);

  
        }

        private void AddRectangle(Rectangle rectangle, Vector2 position)
        {
            this.CombinedRectangle.Add(rectangle);
            this.RectanglePositions.Add(position);
        }

        private void AddRow(int length, Vector2 position, Rectangle left, Rectangle middle, Rectangle right)
        {
            int startingPositionX = (int)position.X;
            int numberNeeded = (int)(length / this.Scale / 16); ;
            AddRectangle(left, position);
            startingPositionX += (int)(16 * this.Scale);

            numberNeeded--;

            while (numberNeeded > 1)
            {

                Vector2 newPosition = new Vector2(startingPositionX, position.Y);
                AddRectangle(middle, newPosition);
                numberNeeded--;
                startingPositionX += (int)(16 * this.Scale);
            }
            AddRectangle(right, new Vector2(startingPositionX, position.Y));
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < CombinedRectangle.Count; i++)
            {

                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, RectanglePositions[i], CombinedRectangle[i], Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            }
        }

    }
}

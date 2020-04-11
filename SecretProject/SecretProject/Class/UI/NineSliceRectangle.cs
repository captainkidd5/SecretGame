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

        Rectangle LeftRectangle = new Rectangle(1024, 64, 16, 48);
        Rectangle MiddleRectangle = new Rectangle(1040, 64, 16, 48);
        Rectangle RightRectangle = new Rectangle(1120, 64, 16, 48);
        public NineSliceRectangle(Vector2 position, RectangleSize size)
        {
            CombinedRectangle = new List<Rectangle>();
            int width = 0;
            CombinedRectangle.Add(LeftRectangle);
            this.Scale = 2f;
            width += LeftRectangle.Width;
            
            for(int i =0; i < (int)size; i++)
            {
                CombinedRectangle.Add(MiddleRectangle);
                width += MiddleRectangle.Width;
            }
            CombinedRectangle.Add(RightRectangle);
            width += RightRectangle.Width;

            this.Width = width * (int)Scale;
            this.Height = LeftRectangle.Height;

            RectanglePositions = new List<Vector2>();
            RectanglePositions.Add(position);
            for(int i =0; i < (int)size; i++)
            {

                RectanglePositions.Add(new Vector2(RectanglePositions[i].X + MiddleRectangle.Width * Scale, position.Y));
            }
            RectanglePositions.Add(new Vector2(RectanglePositions[RectanglePositions.Count - 1].X + CombinedRectangle[RectanglePositions.Count - 1].Width * Scale, position.Y));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i =0; i< CombinedRectangle.Count; i++)
            {

                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, RectanglePositions[i], CombinedRectangle[i], Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            }
        }

        public NineSliceRectangle(string text)
        {
            CombinedRectangle = new List<Rectangle>();
            CombinedRectangle.Add(LeftRectangle);
            this.Scale = 2f;


            int totalWidth = (int)TextBuilder.GetTextLength(text, this.Scale, 0);
            int totalHeight = (int)TextBuilder.GetTextHeight(text, this.Scale);




            //width += LeftRectangle.Width;

            //for (int i = 0; i < (int)size; i++)
            //{
            //    CombinedRectangle.Add(MiddleRectangle);
            //    width += MiddleRectangle.Width;
            //}
            //CombinedRectangle.Add(RightRectangle);
            //width += RightRectangle.Width;

            //this.Width = width * (int)Scale;
            //this.Height = LeftRectangle.Height;

            //RectanglePositions = new List<Vector2>();
            //RectanglePositions.Add(position);
            //for (int i = 0; i < (int)size; i++)
            //{

            //    RectanglePositions.Add(new Vector2(RectanglePositions[i].X + MiddleRectangle.Width * Scale, position.Y));
            //}
            //RectanglePositions.Add(new Vector2(RectanglePositions[RectanglePositions.Count - 1].X + CombinedRectangle[RectanglePositions.Count - 1].Width * Scale, position.Y));
        }

        //public  NineSliceRectangle GetNineSliceRectangle(float lineLimit, Vector2 position)
        //{
        //    int numberOfMiddleSlicesNeeded = (int)Math.Ceiling(lineLimit / this.MiddleRectangle.Width * this.Scale);
        //    CombinedRectangle = new List<Rectangle>();

        //    CombinedRectangle.Add(LeftRectangle);

        //    this.Width += (int)(numberOfMiddleSlicesNeeded * this.MiddleRectangle.Width * Scale);

        //    this.Width += (int)(LeftRectangle.Width * Scale);

        //    for (int i = 0; i < numberOfMiddleSlicesNeeded; i++)
        //    {
        //        CombinedRectangle.Add(MiddleRectangle);
        //    }
        //    CombinedRectangle.Add(RightRectangle);
        //    this.Width += (int)(RightRectangle.Width * Scale);


        //    this.Height = (int)(LeftRectangle.Height * Scale);

        //    RectanglePositions = new List<Vector2>();
        //    RectanglePositions.Add(position);
        //    for (int i = 0; i < (int)size; i++)
        //    {

        //        RectanglePositions.Add(new Vector2(RectanglePositions[i].X + MiddleRectangle.Width * Scale, position.Y));
        //    }
        //    RectanglePositions.Add(new Vector2(RectanglePositions[RectanglePositions.Count - 1].X + CombinedRectangle[RectanglePositions.Count - 1].Width * Scale, position.Y));
        //}
    }
}

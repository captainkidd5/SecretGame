using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        Small =1,
        Medium = 5,
        Large =20
    }
    public class NineSliceRectangle
    {
        public List<Rectangle> CombinedRectangle { get; set; }
        public List<Vector2> RectanglePositions { get; set; }

        public int Width { get;private set; }
        public int Height { get; private set; }

        public float Scale { get; set; }
        public NineSliceRectangle(Vector2 position, Rectangle leftSideRectangle, Rectangle midSliceRectangle, Rectangle rightSideRectangle, RectangleSize size)
        {
            CombinedRectangle = new List<Rectangle>();
            int width = 0;
            CombinedRectangle.Add(leftSideRectangle);
            this.Scale = 2f;
            width += leftSideRectangle.Width;
            
            for(int i =0; i < (int)size; i++)
            {
                CombinedRectangle.Add(midSliceRectangle);
                width += midSliceRectangle.Width;
            }
            CombinedRectangle.Add(rightSideRectangle);
            width += rightSideRectangle.Width;

            this.Width = width * (int)Scale;
            this.Height = leftSideRectangle.Height;

            RectanglePositions = new List<Vector2>();
            RectanglePositions.Add(position);
            for(int i =0; i < (int)size; i++)
            {

                RectanglePositions.Add(new Vector2(RectanglePositions[i].X + midSliceRectangle.Width * Scale, position.Y));
            }
            RectanglePositions.Add(new Vector2(RectanglePositions[RectanglePositions.Count - 1].X + CombinedRectangle[RectanglePositions.Count - 1].Width * Scale, position.Y));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i =0; i< CombinedRectangle.Count; i++)
            {

                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, RectanglePositions[i], CombinedRectangle[i], Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardButtonDepth);
            }
        }
    }
}

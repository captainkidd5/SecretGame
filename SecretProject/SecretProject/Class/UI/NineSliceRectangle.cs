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
        Large =10
    }
    public class NineSliceRectangle
    {
        public List<Rectangle> CombinedRectangle { get; set; }
        public List<Vector2> RectanglePositions { get; set; }

        public NineSliceRectangle(Vector2 position, Rectangle leftSideRectangle, Rectangle midSliceRectangle, Rectangle rightSideRectangle, RectangleSize size)
        {
            CombinedRectangle = new List<Rectangle>();
            CombinedRectangle.Add(leftSideRectangle);
            for(int i =0; i < (int)size; i++)
            {
                CombinedRectangle.Add(midSliceRectangle);
            }
            CombinedRectangle.Add(rightSideRectangle);

            RectanglePositions = new List<Vector2>();
            RectanglePositions.Add(position);
            for(int i =0; i < (int)size; i++)
            {
                //RectanglePositions.Add(new Vector2(leftSideRectangle.X + leftSideRectangle.Width + midSliceRectangle.Width * i));
                RectanglePositions.Add(new Vector2(RectanglePositions[i].X + midSliceRectangle.Width, position.Y));
            }
            RectanglePositions.Add(new Vector2(RectanglePositions[RectanglePositions.Count - 1].X, position.Y));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i =0; i< CombinedRectangle.Count; i++)
            {

                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, RectanglePositions[i], CombinedRectangle[i], Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Utility.StandardButtonDepth);
            }
        }
    }
}

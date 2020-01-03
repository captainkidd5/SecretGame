using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SecretProject.Class.Universal
{
    public class Line
    {
        public Vector2 Point1 { get; set; }
        public Vector2 Point2 { get; set; }
        public float Rotation { get; set; }

        public Line(Vector2 point1, Vector2 point2)
        {
            this.Point1 = point1;
            this.Point2 = point2;
        }

        public void DrawLine(Texture2D texture, SpriteBatch spriteBatch, Color color, float angle)
        {
            Vector2 edge = this.Point2 - this.Point1;

            spriteBatch.Draw(texture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)this.Point1.X,
                    (int)this.Point1.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line 
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None, 1f);

        }





        // a1 is line1 start, a2 is line1 end, b1 is line2 start, b2 is line2 end

        public bool IntersectsRectangle(Rectangle rectangle)
        {
            Vector2 pointIntersected = Vector2.Zero;
            //TOP, BOTTOM, LEFT, RIGHT
            if (IntersectsLine(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y), out pointIntersected)
                || IntersectsLine(new Vector2(rectangle.X, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), out pointIntersected)
                || IntersectsLine(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X, rectangle.Y + rectangle.Height), out pointIntersected)
                || IntersectsLine(new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), out pointIntersected))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IntersectsLine(Vector2 b1, Vector2 b2, out Vector2 intersection)
        {
            intersection = Vector2.Zero;

            Vector2 b = this.Point2 - this.Point1;
            b = Game1.Utility.RotateVector2(this.Point2, this.Point1, this.Rotation);
            Vector2 d = b2 - b1;
            float bDotDPerp = b.X * d.Y - b.Y * d.X;

            // if b dot d == 0, it means the lines are parallel so have infinite intersection points
            if (bDotDPerp == 0)
                return false;

            Vector2 c = b1 - this.Point1;
            float t = (c.X * d.Y - c.Y * d.X) / bDotDPerp;
            if (t < 0 || t > 1)
                return false;

            float u = (c.X * b.Y - c.Y * b.X) / bDotDPerp;
            if (u < 0 || u > 1)
                return false;

            intersection = this.Point1 + t * b;

            return true;
        }
    }
}

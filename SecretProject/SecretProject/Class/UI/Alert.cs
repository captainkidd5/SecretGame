using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public enum AlertSize
    {
        Small = 0,
        Medium = 1,
        Large = 2
    }
    public class Alert
    {
        public NineSliceRectangle NineSliceRectangle { get; set; }

        public Rectangle LeftRectangle { get; set; }
        public Rectangle MiddleRectangle { get; set; }
        public Rectangle RightRectangle { get; set; }

        public Alert(AlertSize size, Vector2 position)
        {
            LeftRectangle = new Rectangle(1024, 64, 16, 48);
            MiddleRectangle = new Rectangle(1040, 64, 16, 48);
            RightRectangle = new Rectangle(1120, 64, 16, 48);

            switch (size)
            {
                case AlertSize.Small:
                    this.NineSliceRectangle = new NineSliceRectangle(position, LeftRectangle, MiddleRectangle, RightRectangle, RectangleSize.Small);
                    break;
                case AlertSize.Medium:
                    this.NineSliceRectangle = new NineSliceRectangle(position, LeftRectangle, MiddleRectangle, RightRectangle, RectangleSize.Medium);
                    break;

                case AlertSize.Large:
                    this.NineSliceRectangle = new NineSliceRectangle(position, LeftRectangle, MiddleRectangle, RightRectangle, RectangleSize.Large);
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.NineSliceRectangle.Draw(spriteBatch);
        }
    }
}

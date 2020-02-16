using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.ButtonStuff
{
    public class RedEsc : Button
    {
        public RedEsc(Vector2 position, GraphicsDevice graphics)
        {

            this.Texture = Game1.AllTextures.UserInterfaceTileSet;
            Position = position;

            size = new Vector2((graphics.Viewport.Width / 10), (graphics.Viewport.Height / 11));



            this.HitBoxScale = 1f;

            this.BackGroundSourceRectangle = new Rectangle(0, 0, 32, 32);
            UpdateHitBoxRectanlge(this.BackGroundSourceRectangle);

            this.CursorType = Controls.CursorType.Normal;
  
        }

        //RedEsc.Draw(spriteBatch);
    }
}

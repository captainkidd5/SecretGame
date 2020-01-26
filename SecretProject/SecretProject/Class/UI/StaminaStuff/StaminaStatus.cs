using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.StaminaStuff
{
    public class StaminaStatus
    {
        public Rectangle DrainingSourceRectangle { get; set; }
        public Rectangle SafeSourceRectangle { get; set; }
        public Rectangle Rectangle { get; set; }
        public Vector2 Position { get; set; }
        public StaminaStatus(Vector2 position)
        {
            this.DrainingSourceRectangle = new Rectangle(192, 320, 32, 32);
            this.SafeSourceRectangle = new Rectangle(160, 320, 32, 32);
            this.Rectangle = SafeSourceRectangle;
            this.Position = position;
        }

        public void UpdateStaminaRectangle(bool isDrainActive)
        {
            if(isDrainActive)
            {
                this.Rectangle = DrainingSourceRectangle;
            }
            else
            {
                this.Rectangle = SafeSourceRectangle;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.Rectangle, Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);
        }
    }

}

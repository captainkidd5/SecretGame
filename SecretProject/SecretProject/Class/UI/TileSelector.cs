using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class TileSelector
    {
        public int IndexX { get; set; }
        public int IndexY { get; set; }

        public int WorldX { get; set; }
        public int WorldY { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public TileSelector()
        {
            IndexX = 0;
            IndexY = 0;
            WorldX = 0;
            WorldY = 0;

            this.SourceRectangle = new Rectangle(48, 0, 16, 16);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(WorldX, WorldY, 16, 16),
                        SourceRectangle, Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .15f);


            
        }
    }
}

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
        public int ChunkX { get; set; }
        public int ChunkY { get; set; }

        public int WorldX { get { return IndexX * 16; }}
        public int WorldY { get { return IndexY * 16; } }
        public Rectangle SourceRectangle { get; set; }
        public TileSelector()
        {
            IndexX = 0;
            IndexY = 0;
            ChunkX = -1;
            ChunkY = -1;
            this.SourceRectangle = new Rectangle(48, 0, 16, 16);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(ChunkX != -1 && ChunkY !=-1)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(WorldX, WorldY, 16, 16),
                        SourceRectangle, Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .15f);
            }
            else
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(WorldX * chunkX, WorldY * ChunkY, 16, 16),
                       SourceRectangle, Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .15f);
            }
            
        }
    }
}

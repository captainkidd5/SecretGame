using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            this.IndexX = 0;
            this.IndexY = 0;
            this.WorldX = 0;
            this.WorldY = 0;

            this.SourceRectangle = new Rectangle(48, 0, 16, 16);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(this.WorldX, this.WorldY, 16, 16),
                    this.SourceRectangle, Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .15f);



        }
    }
}

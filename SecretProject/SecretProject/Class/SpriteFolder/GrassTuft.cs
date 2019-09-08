using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.SpriteFolder
{
    public class GrassTuft
    {
        public int GrassType { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle DestinationRectangle { get; set; }
        public GrassTuft(int grassType,Vector2 position)
        {
            this.GrassType = grassType;
            this.Position = position;
            this.DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
        }
        public void Update(GameTime gameTime)
        {
            if(Game1.Player.Rectangle.Intersects(DestinationRectangle)
        }
        public void Draw(SpriteBatch spriteBatch)
        {
         
            switch (GrassType)
            {
                case 1:
                    spriteBatch.Draw(Game1.AllTextures.TallGrass, DestinationRectangle, new Rectangle(0, 0, 16, 16),
                        Color.White, 1f, Game1.Utility.Origin, SpriteEffects.None, .5f + (DestinationRectangle.Top + DestinationRectangle.Height) * .00001f);
                    break;
            }
        }
    }
}

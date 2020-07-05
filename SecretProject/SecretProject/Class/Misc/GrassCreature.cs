using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SpriteFolder;

namespace SecretProject.Class.Misc
{
    public class GrassCreature : FunItems
    {
        public List<FunItems> FunItems { get; set; }

        private Texture2D Texture { get; set; }
        private Vector2 Position { get; set; }
        private Sprite[] AnimatedSprite { get; set; }

        private Vector2 DestinationPosition { get; set; }

        private float Speed { get; set; } = 1.7f;


        public GrassCreature(GraphicsDevice graphics)
        {
            this.AnimatedSprite = new Sprite[3];

            this.AnimatedSprite[0] = new Sprite(graphics, Texture, 0, 64, 16, 16, 3, .25f, this.Position, changeFrames: false);
            this.AnimatedSprite[1] = new Sprite(graphics, Texture, 16, 64, 16, 16, 3, .25f, this.Position, changeFrames: false);
            this.AnimatedSprite[2] = new Sprite(graphics, Texture, 32, 64, 16, 16, 3, .25f, this.Position, changeFrames: false);
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class ProgressBook : IExclusiveInterfaceComponent
    {
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public GraphicsDevice GraphicsDevice { get; set; }
        public Rectangle BackDropSourceRectangle { get; set; }
        public Vector2 BackDropPosition { get; set; }
        public float BackDropScale { get; set; }

        public ProgressBook(ContentManager content, GraphicsDevice graphics)
        {
            this.GraphicsDevice = graphics;
            this.BackDropSourceRectangle = new Rectangle(432, 48, 192, 272);
            this.BackDropPosition = new Vector2(Game1.ScreenWidth / 4, -64);
            this.BackDropScale = 3f;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackDropPosition, this.BackDropSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
        }
    }
}

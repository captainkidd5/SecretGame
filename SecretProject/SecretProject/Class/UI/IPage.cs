using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SecretProject.Class.UI
{
    public interface IPage
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}

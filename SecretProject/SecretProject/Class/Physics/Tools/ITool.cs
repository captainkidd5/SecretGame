using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Physics.Tools
{
    public interface ITool
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, float layerDepth);
    }
}

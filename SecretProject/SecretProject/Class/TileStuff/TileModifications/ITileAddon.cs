using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Physics.CollisionDetection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretProject.Class.TileStuff.TileModifications
{
    public interface ITileAddon
    {
        public List<ITileAddon> TileAddons { get; }

        void Load(List<HullBody> hullBodies, List<ITileAddon> tileAddons);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);

        public void Remove();
    }
}

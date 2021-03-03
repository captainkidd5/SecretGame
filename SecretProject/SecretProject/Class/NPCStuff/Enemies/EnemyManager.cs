using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretProject.Class.NPCStuff.Enemies
{
    public class EnemyManager : Component
    {
        public List<Enemy> Enemies { get; set; }
        public EnemyManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            this.Enemies = new List<Enemy>();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.Enemies.Count; i++)
            {
                this.Enemies[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.Enemies.Count; i++)
            {
                this.Enemies[i].Draw(spriteBatch);
            }
        }
        public override void Load()
        {
            throw new NotImplementedException();
        }

        public override void Unload()
        {
            throw new NotImplementedException();
        }
    }
}

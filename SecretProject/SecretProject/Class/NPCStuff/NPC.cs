using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using SecretProject.Class.SpriteFolder;
using SecretProject.Class.CollisionDetection;

namespace SecretProject.Class.NPCStuff
{
    public class NPC
    {
        public string name;

       protected Vector2 position;
        protected Vector2 velocity;

       protected AnimatedSprite NPCAnimatedSprite;

        protected GraphicsDevice graphics;

        Collider NPCCollider;



        

        public NPC(string name, Vector2 position, GraphicsDevice graphics)
        {
            this.name = name;
            this.graphics = graphics;
            this.position = position;

            //NPCCollider = new Collider(velocity, Rectangle);
            

        }

        public virtual void Update(GameTime gameTime)
        {
            
            NPCAnimatedSprite.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            NPCAnimatedSprite.Draw(spriteBatch, position, .4f);
        }

    }
}

/*
public Rectangle Rectangle
{
    get
    {
        return new Rectangle((int)position.X, (int)position.Y + 5, (int)NPCAnimatedSprite.Texture.Width / NPCAnimatedSprite.Columns, (int)NPCAnimatedSprite.Texture.Height - 5 / NPCAnimatedSprite.Rows);
    }

}
*/

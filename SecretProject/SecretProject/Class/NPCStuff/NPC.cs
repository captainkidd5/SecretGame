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
using SecretProject.Class.Controls;

namespace SecretProject.Class.NPCStuff
{
    public class NPC : INPC
    {
        public string name;


        protected GraphicsDevice graphics;

        Collider NPCCollider;

        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public AnimatedSprite NPCAnimatedSprite { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle NPCRectangle {
            get
            {
                return new Rectangle((int)Position.X + 2, (int)Position.Y + 20, 16, 32);
            }
        }
        

        public NPC(string name, Vector2 position, GraphicsDevice graphics)
        {
            this.Name = name;
            this.Position = position;


            //NPCCollider = new Collider(velocity, Rectangle);
            

        }

        public virtual void Update(GameTime gameTime, MouseManager mouse)
        {
            
            NPCAnimatedSprite.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            NPCAnimatedSprite.Draw(spriteBatch, Position, .4f);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.Controls;

namespace SecretProject.Class
{
    public abstract class Component
    {
 
        #region FIELDS

        protected ContentManager content;
        protected GraphicsDevice graphicsDevice;

        protected Game1 game;

       // protected MouseState mouse;

        protected MouseManager customMouse;


        #endregion

        public Component(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse)
        {
            this.game = game;
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            this.customMouse = mouse;
        }
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);

    }
}
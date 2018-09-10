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

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;

namespace SecretProject.Class
{
    public abstract class Component
    {



        #region FIELDS

        protected ContentManager content;
        protected GraphicsDevice graphicsDevice;

        protected Game1 game;

        protected MouseState mouse;


        #endregion


        #region METHODS





        public Component(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseState mouse)
        {
            this.game = game;
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            this.mouse = mouse;
        }
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, TiledMapRenderer renderer);

        public abstract void Update(GameTime gameTime, MouseState mouse);
        #endregion

    }
}
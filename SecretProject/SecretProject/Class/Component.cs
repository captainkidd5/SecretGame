using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretProject.Class
{

        /// <summary>
        /// Have classes inherit from this to grant easy access to graphicsdevice, content, and required loading and unloading.
        /// </summary>
        public abstract class Component
        {
            protected readonly GraphicsDevice graphicsDevice;
            protected readonly ContentManager content;
            public Component(GraphicsDevice graphicsDevice, ContentManager content)
            {
                this.graphicsDevice = graphicsDevice;
                this.content = content;
            }

            public abstract void Load();

            public abstract void Unload();


        }

        //public Template(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        //{
        // }
    
}

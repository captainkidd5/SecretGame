using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.LightStuff
{
    public class LightSource
    {
        public Texture2D LightTexture { get; set; }
        public Vector2 Position;
        public bool IsActive { get; set; } = false;

        public LightSource(int id, Vector2 position)
        {
            
            this.LightTexture = GetTextureFromID(id);
            this.Position = new Vector2(position.X +10 - this.LightTexture.Width / 2, position.Y - this.LightTexture.Height / 2);
        }

        private Texture2D GetTextureFromID(int id)
        {
            switch (id)
            {
                case 1:
                    return Game1.AllTextures.lightMask;
                default:
                    return Game1.AllTextures.lightMask;
            }

        }
    }
}

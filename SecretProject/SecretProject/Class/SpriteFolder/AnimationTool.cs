using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.SpriteFolder
{
    public class AnimationTool : Sprite
    {
        public AnimationTool(GraphicsDevice graphics, Texture2D atlasTexture, Rectangle sourceRectangle, Vector2 position, int width, int height) : base(graphics, atlasTexture, sourceRectangle, position, width, height)  
        {

        }


    }
}

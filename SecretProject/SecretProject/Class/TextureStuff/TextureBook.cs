using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TextureStuff
{
   public class TextureBook
    {
        public Texture2D JoeSprite;
        public Texture2D joeDown;
        public Texture2D joeUp;
        public Texture2D joeRight;
        public Texture2D joeLeft;

        public Texture2D MainCharacterSpriteStrip;

        public Texture2D CutGrassDown;


        public TextureBook(ContentManager content, SpriteBatch spriteBatch)
        {
            JoeSprite = content.Load<Texture2D>("Player/Joe/joe");

            joeDown = content.Load<Texture2D>("Player/Joe/JoeWalkForwardNew");
            joeUp = content.Load<Texture2D>("Player/Joe/JoeWalkBackNew");
            joeRight = content.Load<Texture2D>("Player/Joe/JoeWalkRightNew");
            joeLeft = content.Load<Texture2D>("Player/Joe/JoeWalkLefttNew");

            MainCharacterSpriteStrip = content.Load<Texture2D>("Player/MainPlayer/newPlayer");

            CutGrassDown = content.Load<Texture2D>("Player/MainPlayer/ClippingForward");

        }

    }
}

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

namespace SecretProject.Class.MenuStuff
{
    class Button
    {
        Game1 game;
        Texture2D Texture;
        public Vector2 Position;
        public Rectangle Rectangle;

        Color Color;

        MouseManager myMouse;

        public Vector2 size;

        bool down;
        public bool isClicked;

        SpriteFont font;

        public Button(Texture2D newtexture, GraphicsDevice graphicsDevice, MouseManager myMouse, Vector2 position)
        {
            Texture = newtexture;
            Position = position;
            //128x64
            size = new Vector2((graphicsDevice.Viewport.Width / 10), (graphicsDevice.Viewport.Height / 11));

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            this.myMouse = myMouse;

        }



        public void Update()
        {
            myMouse.Update();

            if(myMouse.IsHovering(Rectangle))
            {
                Color = Color.White * .5f;
                if(myMouse.IsClicked)
                {
                    isClicked = true;
                }
            }
            else
            {
                Color = Color.White;

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 fontLocation, Color tint)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);
            spriteBatch.DrawString(font, text, fontLocation, tint);

        }

        public bool ClickRegister()
        {
            if(myMouse.IsClicked)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        

    }

}


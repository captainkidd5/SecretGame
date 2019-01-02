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
       // Texture2D Texture;

        public Texture2D Texture { get; set; }
        public Vector2 Position;
        public Rectangle Rectangle;

        Color Color;

        MouseManager myMouse;

        public Vector2 size;

        bool down;
        public bool isClicked;
        public bool isClickedAndHeld;

        public bool IsHovered { get; set; }

        SpriteFont font;

        public Vector2 FontLocation { get { return new Vector2(Position.X -35  + (Texture.Width / 2), Position.Y + (Texture.Height / 2)); } }

        public int ItemCounter { get; set; }

        public Button(Texture2D newtexture, GraphicsDevice graphicsDevice, MouseManager myMouse, Vector2 position)
        {
            Texture = newtexture;
            Position = position;
            this.myMouse = myMouse;
            //128x64
            size = new Vector2((graphicsDevice.Viewport.Width / 10), (graphicsDevice.Viewport.Height / 11));

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            

        }
        public void Update()
        {

            if (myMouse.IsHovering(Rectangle) && myMouse.IsClicked)
            {
                Color = Color.White * .5f;
                // if(myMouse.IsClicked)
                //{
                isClicked = true;
                //}
            }
            else if(myMouse.IsHovering(Rectangle) && myMouse.IsClickedAndHeld)
            {
                isClickedAndHeld = true;
                Color = Color.White * .5f;
            }
            else if (myMouse.IsHovering(Rectangle) && isClicked == false)
            {
                Color = Color.White * .5f;
            }
            else if(myMouse.IsHovering(Rectangle))
            {
                IsHovered = true;
            }
            else
            {
                Color = Color.White;
                isClicked = false;
                IsHovered = false;
                isClickedAndHeld = false;

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

    }

}


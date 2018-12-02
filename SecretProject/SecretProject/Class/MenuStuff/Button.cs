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

namespace SecretProject.Class.MenuStuff
{
    class Button
    {
        Game1 game;
        Texture2D Texture;
        public Vector2 Position;
        public Rectangle Rectangle;

        Color Color;

        MouseState myMouse;

        public Vector2 size;

        bool down;
        public bool isClicked;

        public Button(Texture2D newtexture, GraphicsDevice graphicsDevice, Vector2 position)
        {
            Texture = newtexture;
            Position = position;
            //128x64
            size = new Vector2((graphicsDevice.Viewport.Width / 10), (graphicsDevice.Viewport.Height / 11));

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        }


        public void Update(MouseState mouse)
        {
            MouseState oldMouse = myMouse;
            myMouse = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle(myMouse.X, myMouse.Y, 1, 1);

            if (mouseRectangle.Intersects(Rectangle))
            {
                Color = Color.White * .5f;


                if ((myMouse.LeftButton == ButtonState.Released) && (oldMouse.LeftButton == ButtonState.Pressed)) isClicked = true;
            }
            else Color = Color.White;


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);
        }


    }

}


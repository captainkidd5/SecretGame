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
    public class Button
    {
        Game1 game;
       // Texture2D Texture;

        public Texture2D Texture { get; set; }
        public Vector2 Position;
        public Rectangle Rectangle;

        Color Color;

        public Vector2 size;

        bool down;
        public bool isClicked;
        public bool isClickedAndHeld;
        public bool wasJustReleased { get; set; }


        public bool Added { get; set; } = false;

        public bool IsHovered { get; set; }

        SpriteFont font;

        public Vector2 FontLocation { get { return new Vector2(Position.X -35  + (Texture.Width / 2), Position.Y + (Texture.Height / 2)); } }

        public int Index { get; set; }

        public int ItemCounter { get; set; }
        public int Price { get; set; }

        public Rectangle ItemSourceRectangleToDraw { get; set; }


        public Button(Texture2D newtexture, GraphicsDevice graphicsDevice, Vector2 position)
        {
            Texture = newtexture;
            Position = position;
            //128x64
            size = new Vector2((graphicsDevice.Viewport.Width / 10), (graphicsDevice.Viewport.Height / 11));

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        }
        public void Update(MouseManager mouse)
        {
            isClicked = false;
            wasJustReleased = false;

            if (mouse.IsHovering(Rectangle) && mouse.IsClicked)
            {
                Color = Color.White * .5f;
                isClicked = true;

            }
            else if (mouse.IsHovering(Rectangle) && isClicked == false)
            {
                Color = Color.White * .5f;
                IsHovered = true;

            }
            else
            {
                Color = Color.White;
                isClicked = false;
                IsHovered = false;

            }

            if(mouse.IsHovering(Rectangle) && mouse.WasJustPressed == true && mouse.IsClickedAndHeld == true)
            {
                this.isClickedAndHeld = true;
            }
            if(this.isClickedAndHeld == true && mouse.IsHovering(Rectangle) == false)
            {
                if(mouse.IsReleased)
                {
                    this.isClickedAndHeld = false;
                    wasJustReleased = true;
                }
            }



            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepthCustom)
        {
            spriteBatch.Draw(Texture, destinationRectangle: Rectangle, color: Color, layerDepth: layerDepthCustom);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle sourceRectangle,SpriteFont font, string text, Vector2 fontLocation, Color tint, float layerDepthCustom = .69f)
        {
            
            spriteBatch.Draw(Texture, sourceRectangle: sourceRectangle,destinationRectangle: Rectangle, color: Color, layerDepth: layerDepthCustom);
            spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f,SpriteEffects.None, layerDepth: .73f);
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 fontLocation, Color tint, float layerDepthCustom = .69f)
        {

            spriteBatch.Draw(Texture, destinationRectangle: Rectangle, color: Color, layerDepth: layerDepthCustom);
            spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, layerDepth: .73f);
        }

        public void Draw(SpriteBatch spriteBatch,Rectangle sourceRectangle, SpriteFont font, string text, Vector2 fontLocation, Color tint, Vector2 priceLocation, string price, float layerDepthCustom = .69f)
        {

            spriteBatch.Draw(Texture,sourceRectangle: sourceRectangle, destinationRectangle: Rectangle, color: Color, layerDepth: .68f);
            spriteBatch.DrawString(font, text, fontLocation, tint,0f, Game1.Utility.Origin, 1f, SpriteEffects.None, layerDepth: layerDepthCustom);
            spriteBatch.DrawString(font, price, priceLocation, Color.OrangeRed, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, layerDepth: layerDepthCustom);
        }

    }

}


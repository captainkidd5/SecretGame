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
        public Texture2D Texture { get; set; }
        public Vector2 Position;
        public Rectangle HitBoxRectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, (int)(BackGroundSourceRectangle.Width * HitBoxScale), (int)(BackGroundSourceRectangle.Height * HitBoxScale)); } set { } }
        public Rectangle SelectableTextRectangle { get; set; }
        Color Color;

        public Vector2 size;

        public bool isClicked;
        public bool isClickedAndHeld;
        public bool wasJustReleased { get; set; }


        public bool Added { get; set; } = false;

        public bool IsHovered { get; set; }

        SpriteFont font;

        

        public int Index { get; set; }

        public int ItemCounter { get; set; }
        public int Price { get; set; }

        public Rectangle ItemSourceRectangleToDraw { get; set; }
        public Rectangle BackGroundSourceRectangle { get; set; }

        public float HitBoxScale { get; set; }




        public Vector2 FontLocation { get { return new Vector2(HitBoxRectangle.X + 5, HitBoxRectangle.Y + BackGroundSourceRectangle.Height / 2); } set { } }




        public Button(Texture2D newtexture, Rectangle sourceRectangle,GraphicsDevice graphicsDevice, Vector2 position)
        {
            Texture = newtexture;
            Position = position;
            //128x64
            size = new Vector2((graphicsDevice.Viewport.Width / 10), (graphicsDevice.Viewport.Height / 11));

            this.HitBoxScale = 1f;
            HitBoxRectangle = new Rectangle((int)Position.X, (int)Position.Y, sourceRectangle.Width, sourceRectangle.Height);

            this.BackGroundSourceRectangle = sourceRectangle;
            //this.ItemSourceRectangleToDraw = sou


        }

        //for clickable text
        public Button(Rectangle clickRangeRectangle)
        {
            this.HitBoxScale = 1f;
            this.SelectableTextRectangle = clickRangeRectangle;
        }

        
        public void Update(MouseManager mouse)
        {
            isClicked = false;
            wasJustReleased = false;

            if (mouse.IsHovering(HitBoxRectangle) && mouse.IsClicked)
            {
                Color = Color.White * .5f;
                isClicked = true;

            }
            else if (mouse.IsHovering(HitBoxRectangle) && isClicked == false)
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

            if(mouse.IsHovering(HitBoxRectangle)&& mouse.IsClickedAndHeld == true)
            {
                this.isClickedAndHeld = true;
            }
            if(this.isClickedAndHeld == true && !mouse.IsHovering(HitBoxRectangle))
            {
                if(mouse.IsReleased)
                {
                    this.isClickedAndHeld = false;
                    wasJustReleased = true;
                }
            }



            
        }
        //for selectableText
        public void UpdateSelectableText(MouseManager mouse)
        {
            isClicked = false;
            wasJustReleased = false;

            if (mouse.IsHovering(SelectableTextRectangle) && mouse.IsClicked)
            {
                Color = Color.White * .5f;
                isClicked = true;

            }
            else if (mouse.IsHovering(SelectableTextRectangle) && isClicked == false)
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

            if (mouse.IsHovering(SelectableTextRectangle) && mouse.IsClickedAndHeld == true)
            {
                this.isClickedAndHeld = true;
            }
            if (this.isClickedAndHeld == true && !mouse.IsHovering(SelectableTextRectangle))
            {
                if (mouse.IsReleased)
                {
                    this.isClickedAndHeld = false;
                    wasJustReleased = true;
                }
            }




        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position,BackGroundSourceRectangle,  Color, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
        }


        public void DrawSelectableTextBoxOption(SpriteBatch spriteBatch, string text)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, text, new Vector2(this.SelectableTextRectangle.X, this.SelectableTextRectangle.Y), this.Color, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, layerDepth: .73f);
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepthCustom)
        {
            spriteBatch.Draw(Texture, destinationRectangle: HitBoxRectangle, color: Color, layerDepth: layerDepthCustom);
        }

        public void Draw(SpriteBatch spriteBatch, Color tint, float layerDepthCustom)
        {
            spriteBatch.Draw(this.Texture, this.HitBoxRectangle, this.BackGroundSourceRectangle, tint, 0f, Game1.Utility.Origin, SpriteEffects.None, layerDepthCustom);
        }

        //for toolbar
        public void Draw(SpriteBatch spriteBatch, Rectangle sourceRectangle, Rectangle backgroundSourceRectangle,SpriteFont font, string text, Vector2 fontLocation, Color tint, float scale = 1f, float layerDepthCustom = .69f)
        {

            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(HitBoxRectangle.X, HitBoxRectangle.Y), backgroundSourceRectangle, this.Color, 0f, Game1.Utility.Origin, scale,SpriteEffects.None, layerDepthCustom);
            spriteBatch.Draw(this.Texture, new Vector2(HitBoxRectangle.X + HitBoxRectangle.Width/4, HitBoxRectangle.Y + HitBoxRectangle.Height/4), sourceRectangle, this.Color, 0f, Game1.Utility.Origin, scale, SpriteEffects.None, layerDepthCustom + .01f);

            //spriteBatch.Draw(Texture, sourceRectangle: sourceRectangle,destinationRectangle: Rectangle, color: Color, layerDepth: layerDepthCustom);
            spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f,SpriteEffects.None, layerDepth: .73f);
        }

        //for crafting menu
        public void DrawCraftingSlot(SpriteBatch spriteBatch, Rectangle itemSourceRectangle, Rectangle backgroundSourceRectangle, SpriteFont font, string text, Vector2 fontLocation, Color tint, float backGroundScale = 1f, float foreGroundScale = 1f, float layerDepthCustom = .69f)
        {
            this.HitBoxScale = foreGroundScale;
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(HitBoxRectangle.X, HitBoxRectangle.Y), backgroundSourceRectangle, this.Color, 0f, Game1.Utility.Origin, backGroundScale, SpriteEffects.None, layerDepthCustom);
            spriteBatch.Draw(this.Texture, new Vector2(HitBoxRectangle.X + HitBoxRectangle.Width / 4, HitBoxRectangle.Y + HitBoxRectangle.Height / 4), itemSourceRectangle, tint, 0f, Game1.Utility.Origin, foreGroundScale, SpriteEffects.None, layerDepthCustom + .01f);
            spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, layerDepth: .73f);

            //this.Rectangle = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width * foreGroundScale, Rectangle.Height * foreGroundScale)
        }

        public void DrawCraftingSlotRetrievable(SpriteBatch spriteBatch, Rectangle itemSourceRectangle, Rectangle backgroundSourceRectangle, Color tint, float backGroundScale = 1f, float foreGroundScale = 1f, float layerDepthCustom = .69f)
        {

            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(HitBoxRectangle.X, HitBoxRectangle.Y), backgroundSourceRectangle, this.Color, 0f, Game1.Utility.Origin, backGroundScale, SpriteEffects.None, layerDepthCustom);
            spriteBatch.Draw(this.Texture, new Vector2(HitBoxRectangle.X + HitBoxRectangle.Width / 4, HitBoxRectangle.Y + HitBoxRectangle.Height / 4), itemSourceRectangle, tint, 0f, Game1.Utility.Origin, foreGroundScale, SpriteEffects.None, layerDepthCustom + .01f);
           

            //this.Rectangle = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width * foreGroundScale, Rectangle.Height * foreGroundScale)
        }

        //for esc menu
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 fontLocation, Color tint, float layerDepthCustom = .69f)
        {

            spriteBatch.Draw(Texture, destinationRectangle: HitBoxRectangle, color: Color, layerDepth: layerDepthCustom);
            spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, layerDepth: .73f);
        }

        //for shopMenu
        public void Draw(SpriteBatch spriteBatch,Rectangle sourceRectangle, SpriteFont font, string text, Vector2 fontLocation, Color tint, Vector2 priceLocation, string price, float layerDepthCustom = .69f)
        {

           // spriteBatch.Draw(Texture,sourceRectangle: sourceRectangle, destinationRectangle: Rectangle, color: Color, layerDepth: .68f);

            spriteBatch.Draw(Texture, HitBoxRectangle, sourceRectangle, this.Color, 0f, Game1.Utility.Origin, SpriteEffects.None, Game1.Utility.StandardButtonDepth);

            spriteBatch.DrawString(font, text, fontLocation, tint,0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            spriteBatch.DrawString(font, price, priceLocation, Color.OrangeRed, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);
        }

        //rework
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 fontLocation,Color textTint, float textureDepth, float textDepth)
        {
            spriteBatch.Draw(this.Texture, HitBoxRectangle , this.BackGroundSourceRectangle, this.Color, 0f, Game1.Utility.Origin, SpriteEffects.None, textDepth);
            spriteBatch.DrawString(font, text, fontLocation, textTint, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, textDepth);
        }


        //for scrolltree
        public void Draw(SpriteBatch spriteBatch,Vector2 locationToDrawBackGround, Vector2 locationToDrawButtonImage, Rectangle buttonImageSourceRectangle, Rectangle backgroundSourceRectangle, SpriteFont font, string text, Vector2 fontLocation, Color tint, float scale = 1f, float layerDepthCustom = .69f)
        {

            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, locationToDrawBackGround, backgroundSourceRectangle, tint, 0f, Game1.Utility.Origin, scale, SpriteEffects.None, layerDepthCustom);
            spriteBatch.Draw(this.Texture, locationToDrawButtonImage, buttonImageSourceRectangle, tint, 0f, Game1.Utility.Origin, scale, SpriteEffects.None, layerDepthCustom + .01f);

            //spriteBatch.Draw(Texture, sourceRectangle: sourceRectangle,destinationRectangle: Rectangle, color: Color, layerDepth: layerDepthCustom);
            spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, layerDepth: .73f);
        }

    }

}


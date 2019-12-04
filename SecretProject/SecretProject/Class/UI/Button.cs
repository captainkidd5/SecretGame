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
        public Color Color;

        public Vector2 size;

        public bool isClicked;
        public bool isClickedAndHeld;
        public bool wasJustReleased { get; set; }


        public bool Added { get; set; } = false;

        public bool IsHovered { get; set; }

        public int Index { get; set; }

        public int ItemCounter { get; set; }
        public int Price { get; set; }

        public Rectangle ItemSourceRectangleToDraw { get; set; }
        public Rectangle BackGroundSourceRectangle { get; set; }

        public float HitBoxScale { get; set; }




        public Vector2 FontLocation { get { return new Vector2(HitBoxRectangle.X + 5, HitBoxRectangle.Y + BackGroundSourceRectangle.Height / 2); } set { } }

        public CursorType CursorType { get; set; }

        public Button()
        {

        }

        public Button(Texture2D newtexture, Rectangle sourceRectangle, GraphicsDevice graphicsDevice, Vector2 position, CursorType cursorType, float scale = 1f)
        {
            Texture = newtexture;
            Position = position;
            //128x64
            size = new Vector2((graphicsDevice.Viewport.Width / 10), (graphicsDevice.Viewport.Height / 11));

            if (scale == 0)
            {
                this.HitBoxScale = 1f;
            }
            else
            {
                this.HitBoxScale = scale;
            }
            // this.HitBoxScale = scale;


            this.BackGroundSourceRectangle = sourceRectangle;
            UpdateHitBoxRectanlge(BackGroundSourceRectangle);
            //this.ItemSourceRectangleToDraw = sou
            this.CursorType = cursorType;
            //this.Color = Color.White;

        }

        public void UpdateHitBoxRectanlge(Rectangle newSourceRectangle)
        {
            this.BackGroundSourceRectangle = newSourceRectangle;
            HitBoxRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(this.BackGroundSourceRectangle.Width * HitBoxScale), (int)(this.BackGroundSourceRectangle.Height * HitBoxScale));
        }
        //for clickable text
        public Button(Rectangle clickRangeRectangle, CursorType cursorType)
        {
            this.HitBoxScale = 1f;
            this.SelectableTextRectangle = clickRangeRectangle;
        }


        public void Update(MouseManager mouse)
        {
            wasJustReleased = false;
            if (!mouse.IsClickedAndHeld)
            {
                isClicked = false;
            }
            


            if (mouse.IsHovering(HitBoxRectangle))
            {
                Color = Color.White * .5f;
                IsHovered = true;
                if (this.CursorType != 0)
                {
                    mouse.ChangeMouseTexture(this.CursorType);
                    mouse.ToggleGeneralInteraction = true;
                    Game1.isMyMouseVisible = false;
                }
                if (mouse.IsClicked)
                {
                    isClicked = true;
                }


            }
            else
            {
                Color = Color.White * 1f;
                isClicked = false;
                IsHovered = false;

            }

            if (mouse.IsHovering(HitBoxRectangle) && mouse.IsClickedAndHeld && !mouse.ButtonOccupied)
            {
                this.isClickedAndHeld = true;
                mouse.ButtonOccupied = true;
            }
            if (this.isClickedAndHeld )
            {
                if (!mouse.IsDown)
                {
                    this.isClickedAndHeld = false;
                    wasJustReleased = true;
                    mouse.ButtonOccupied = false;
                }
            }




        }
        //for selectableText
        public void UpdateSelectableText(MouseManager mouse)
        {
            isClicked = false;
            wasJustReleased = false;


            if (mouse.IsHovering(SelectableTextRectangle))
            {
                Color = Color.White * .5f;
                IsHovered = true;
                mouse.ChangeMouseTexture(CursorType.Normal);
                if (mouse.IsClicked)
                {
                    isClicked = true;
                }


            }
            else
            {
                Color = Color.White * 1f;
                isClicked = false;
                IsHovered = false;

            }

            if (mouse.IsHovering(SelectableTextRectangle) && mouse.IsClickedAndHeld == true)
            {
                this.isClickedAndHeld = true;
            }
            if (this.isClickedAndHeld == true && !mouse.IsHovering(SelectableTextRectangle))
            {
                if (!mouse.IsDown)
                {
                    this.isClickedAndHeld = false;
                    wasJustReleased = true;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, BackGroundSourceRectangle, Color, 0f, Game1.Utility.Origin, this.HitBoxScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
        }



        public void DrawSelectableTextBoxOption(SpriteBatch spriteBatch, string text)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, text, new Vector2(this.SelectableTextRectangle.X, this.SelectableTextRectangle.Y), this.Color, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, layerDepth: .73f);
        }


        //for toolbar
        public void Draw(SpriteBatch spriteBatch, Rectangle sourceRectangle, Rectangle backgroundSourceRectangle, SpriteFont font, string text, Vector2 fontLocation, Color tint, float backGroundScale = 1f, float scale = 1f, float layerDepthCustom = .9f)
        {

            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(HitBoxRectangle.X, HitBoxRectangle.Y), backgroundSourceRectangle, tint, 0f, Game1.Utility.Origin, backGroundScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            spriteBatch.Draw(this.Texture, new Vector2(HitBoxRectangle.X + HitBoxRectangle.Width / 4, HitBoxRectangle.Y + HitBoxRectangle.Height / 4), sourceRectangle, tint, 0f, Game1.Utility.Origin, scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);

            //spriteBatch.Draw(Texture, sourceRectangle: sourceRectangle,destinationRectangle: Rectangle, color: Color, layerDepth: layerDepthCustom);
            spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, layerDepth: Game1.Utility.StandardButtonDepth + .02f);
        }

        //for crafting menu


        //for shopMenu
        public void Draw(SpriteBatch spriteBatch, Rectangle sourceRectangle, SpriteFont font, string text, Vector2 fontLocation, Color tint, Vector2 priceLocation, string price, float layerDepthCustom = .69f)
        {

            // spriteBatch.Draw(Texture,sourceRectangle: sourceRectangle, destinationRectangle: Rectangle, color: Color, layerDepth: .68f);

            spriteBatch.Draw(Texture, HitBoxRectangle, sourceRectangle, this.Color, 0f, Game1.Utility.Origin, SpriteEffects.None, Game1.Utility.StandardButtonDepth);

            spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            spriteBatch.DrawString(font, price, priceLocation, Color.OrangeRed, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);
        }

        
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 fontLocation, Color textTint, float textureDepth, float textDepth, float scale = 1f)
        {
            spriteBatch.Draw(this.Texture, HitBoxRectangle, this.BackGroundSourceRectangle, this.Color, 0f, Game1.Utility.Origin, SpriteEffects.None, textureDepth);
            spriteBatch.DrawString(font, text, fontLocation, textTint, 0f, Game1.Utility.Origin,scale, SpriteEffects.None, textDepth);
        }

        public void DrawNormal(SpriteBatch spriteBatch, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float depth)
        {
            spriteBatch.Draw(this.Texture, position, sourceRectangle, color,
                rotation, origin, scale, effects,
                depth);
        }

    }

}



using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.Universal;

namespace SecretProject.Class.MenuStuff
{
    public class Button
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position;
        public Rectangle HitBoxRectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, (int)(this.BackGroundSourceRectangle.Width * this.HitBoxScale), (int)(this.BackGroundSourceRectangle.Height * this.HitBoxScale)); } set { } }
        public Rectangle SelectableTextRectangle { get; set; }
        public Color Color;

        public Vector2 size;

        public bool isClicked;
        public bool isRightClicked;
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




        public Vector2 FontLocation { get { return new Vector2(this.HitBoxRectangle.X + 5, this.HitBoxRectangle.Y + this.BackGroundSourceRectangle.Height / 2); } set { } }

        public CursorType CursorType { get; set; }

        public Item Item { get; set; }
        public string Description { get; set; }

        public int GID { get; set; }

        public Button()
        {

        }

        public Button(Texture2D newtexture, Rectangle sourceRectangle, GraphicsDevice graphicsDevice, Vector2 position, CursorType cursorType, float scale = 1f, Item item = null)
        {
            this.Texture = newtexture;
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
            UpdateHitBoxRectanlge(this.BackGroundSourceRectangle);
            //this.ItemSourceRectangleToDraw = sou
            this.CursorType = cursorType;
            //this.Color = Color.White;
            if(item != null)
            {
                this.Item = item;
                this.ItemSourceRectangleToDraw = this.Item.SourceTextureRectangle;
            }
        }

        public void UpdateHitBoxRectanlge(Rectangle newSourceRectangle)
        {
            this.BackGroundSourceRectangle = newSourceRectangle;
            this.HitBoxRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(this.BackGroundSourceRectangle.Width * this.HitBoxScale), (int)(this.BackGroundSourceRectangle.Height * this.HitBoxScale));
        }
        //for clickable text
        public Button(Rectangle clickRangeRectangle, CursorType cursorType)
        {
            this.HitBoxScale = 1f;
            this.SelectableTextRectangle = clickRangeRectangle;
        }


        public void Update(MouseManager mouse)
        {
            this.wasJustReleased = false;
            this.isRightClicked = false;
            if (!mouse.IsClickedAndHeld)
            {
                isClicked = false;
            }



            if (mouse.IsHovering(this.HitBoxRectangle))
            {
                Color = Color.White * .5f;
                this.IsHovered = true;
                if (this.CursorType != 0)
                {
                    mouse.ChangeMouseTexture(this.CursorType);
                    mouse.ToggleGeneralInteraction = true;
                    Game1.isMyMouseVisible = false;
                }
                if (mouse.IsClicked)
                {
                    isClicked = true;
                    //    Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.UIClick, true);
                }
                else if(mouse.IsRightClicked)
                {
                    isRightClicked = true;
                }


            }
            else
            {
                Color = Color.White * 1f;
                isClicked = false;
                this.IsHovered = false;

            }

            if (mouse.IsHovering(this.HitBoxRectangle) && mouse.IsClickedAndHeld && !mouse.ButtonOccupied)
            {
                isClickedAndHeld = true;
                mouse.ButtonOccupied = true;
            }
            if (isClickedAndHeld)
            {
                if (!mouse.IsDown)
                {
                    isClickedAndHeld = false;
                    this.wasJustReleased = true;
                    mouse.ButtonOccupied = false;
                }
            }




        }
        //for selectableText
        public void UpdateSelectableText(MouseManager mouse)
        {
            isClicked = false;
            this.wasJustReleased = false;


            if (mouse.IsHovering(this.SelectableTextRectangle))
            {
                Color = Color.White * .5f;
                this.IsHovered = true;
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
                this.IsHovered = false;

            }

            if (mouse.IsHovering(this.SelectableTextRectangle) && mouse.IsClickedAndHeld == true)
            {
                isClickedAndHeld = true;
            }
            if (isClickedAndHeld == true && !mouse.IsHovering(this.SelectableTextRectangle))
            {
                if (!mouse.IsDown)
                {
                    isClickedAndHeld = false;
                    this.wasJustReleased = true;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, Position, this.BackGroundSourceRectangle, Color, 0f, Game1.Utility.Origin, this.HitBoxScale, SpriteEffects.None, Utility.StandardButtonDepth + .02f);
        }



        public void DrawSelectableTextBoxOption(SpriteBatch spriteBatch, string text)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, text, new Vector2(this.SelectableTextRectangle.X, this.SelectableTextRectangle.Y), Color, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, layerDepth: .8f);
        }


        //for toolbar
        public void Draw(SpriteBatch spriteBatch, Rectangle sourceRectangle, Rectangle backgroundSourceRectangle, SpriteFont font, string text, Vector2 fontLocation, Color tint, float backGroundScale = 1f, float scale = 1f, float layerDepthCustom = .9f, bool centerImage = false)
        {

            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(this.HitBoxRectangle.X, this.HitBoxRectangle.Y), backgroundSourceRectangle, tint, 0f, Game1.Utility.Origin, backGroundScale, SpriteEffects.None, Utility.StandardButtonDepth + .01f);
            if (centerImage)
            {
                spriteBatch.Draw(this.Texture, new Vector2(this.HitBoxRectangle.X, this.HitBoxRectangle.Y), sourceRectangle, tint, 0f, Game1.Utility.Origin, scale, SpriteEffects.None, Utility.StandardButtonDepth + .02f);
            }
            else
            {
                spriteBatch.Draw(this.Texture, new Vector2(this.HitBoxRectangle.X + this.HitBoxRectangle.Width / 4, this.HitBoxRectangle.Y + this.HitBoxRectangle.Height / 4), sourceRectangle, tint, 0f, Game1.Utility.Origin, scale, SpriteEffects.None, Utility.StandardButtonDepth + .02f);
            }


            //spriteBatch.Draw(Texture, sourceRectangle: sourceRectangle,destinationRectangle: Rectangle, color: Color, layerDepth: layerDepthCustom);
            spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, layerDepth: Utility.StandardButtonDepth + .02f);
        }

        //for crafting menu


        //for shopMenu
        public void Draw(SpriteBatch spriteBatch, Rectangle sourceRectangle, SpriteFont font, string text, Vector2 fontLocation, Color tint, Vector2 priceLocation, string price, float layerDepthCustom = .69f)
        {

            // spriteBatch.Draw(Texture,sourceRectangle: sourceRectangle, destinationRectangle: Rectangle, color: Color, layerDepth: .68f);

            spriteBatch.Draw(this.Texture, this.HitBoxRectangle, sourceRectangle, Color, 0f, Game1.Utility.Origin, SpriteEffects.None, Utility.StandardButtonDepth);

            spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            spriteBatch.DrawString(font, price, priceLocation, Color.OrangeRed, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);
        }


        public void Draw(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 fontLocation, Color textTint, float textureDepth, float textDepth, float scale = 1f)
        {
            spriteBatch.Draw(this.Texture, this.HitBoxRectangle, this.BackGroundSourceRectangle, Color, 0f, Game1.Utility.Origin, SpriteEffects.None, textureDepth);
            spriteBatch.DrawString(font, text, fontLocation, textTint, 0f, Game1.Utility.Origin, scale, SpriteEffects.None, textDepth);
        }

        public void DrawNormal(SpriteBatch spriteBatch, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float depth)
        {
            spriteBatch.Draw(this.Texture, position, sourceRectangle, color,
                rotation, origin, scale, effects,
                depth);
        }

    }

}


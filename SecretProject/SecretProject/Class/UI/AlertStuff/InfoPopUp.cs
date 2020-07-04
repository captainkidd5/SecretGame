using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Universal;
using System.Collections.Generic;
using XMLData.ItemStuff;

namespace SecretProject.Class.UI
{
    public enum LineLimits
    {
        ItemDescriptions = 200,

    }

    public class InfoPopUp
    {
       
        public GraphicsDevice Graphics { get; set; }
        public Vector2 WindowPosition { get; set; }
        public string StringToWrite { get; set; }

        public bool IsActive { get; set; }

        public bool TextFitted { get; set; }

        public Color Color { get; set; }

        public string TitleString { get; set; }
        public bool DisplayTitle { get; set; }

        public NineSliceRectangle FittedRectangle { get; set; }
        public float LineLimit { get; set; }

        public float TextScale { get; private set; }

        public Vector2 TitleTextPosition { get; private set; }

        public List<Sprite> Sprites { get; private set; }


        public InfoPopUp(string stringToWrite, Vector2 windowPosition)
        {

            this.StringToWrite = stringToWrite;
            this.WindowPosition = windowPosition;
            this.TitleString = string.Empty;
            this.TextFitted = false;
            this.Color = Color.White;

            this.TextScale = 1f;

            this.FittedRectangle = new NineSliceRectangle(this.WindowPosition, this.StringToWrite, this.TextScale);
        }

        public InfoPopUp(GraphicsDevice graphics, ItemData itemData, Vector2 windowPosition, bool enableShopData = false)
        {
            this.Graphics = graphics;
            this.Sprites = new List<Sprite>();
            this.WindowPosition = windowPosition;
            this.StringToWrite = GetItemDataString(itemData, enableShopData);
            
            this.TitleString = itemData.Name;
            this.TextFitted = false;
            this.Color = Color.White;

            this.TextScale = 1f;

            if(Game1.AllTextures.MenuText.MeasureString(TitleString).X * (this.TextScale + 1f) > Game1.AllTextures.MenuText.MeasureString(StringToWrite).X)
                this.FittedRectangle = new NineSliceRectangle(this.WindowPosition, this.TitleString, this.TextScale + 1f);
            else
                        this.FittedRectangle = new NineSliceRectangle(this.WindowPosition, this.StringToWrite, this.TextScale);


            this.TitleTextPosition = FittedRectangle.CenterTextHorizontal(itemData.Name, this.TextScale + 1f);
            this.TitleTextPosition = new Vector2(this.TitleTextPosition.X, this.TitleTextPosition.Y + 16 * (this.TextScale));
            this.DisplayTitle = true;

            
        }

        private string GetItemDataString(ItemData itemData, bool enableShopData)
        {
            string s = string.Empty;
            s += itemData.Description + "\n";
            if(itemData.StaminaRestoreAmount > 0)
            {
                s += itemData.StaminaRestoreAmount + "\n";
                this.Sprites.Add(new Sprite(this.Graphics, Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 288, 32, 32),this.WindowPosition +  Game1.AllTextures.MenuText.MeasureString(s), 32, 32)); //Energy symbol
            }
            if(itemData.Damage > 0)
            {
                s += itemData.Damage  + " damage \n";
            }
            if(enableShopData)
            {
                if(itemData.Price > 0)
                {
                    s += "Shop will buy this item for " + itemData.Price + " gold.";
                }
            }
            return s;
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {

            }
        }

        public string FitText(string text, float scale)
        {
            this.StringToWrite = TextBuilder.ParseText(text, this.FittedRectangle.Width * 2.2f, 1.5f);
            return StringToWrite;
        }

        public string FitTitleText(string text, float scale)
        {
            return StringToWrite;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsActive)
            {
                this.FittedRectangle.Draw(spriteBatch,Utility.StandardButtonDepth + .09f);

                spriteBatch.DrawString(Game1.AllTextures.MenuText, this.StringToWrite, new Vector2(this.WindowPosition.X + 16, this.WindowPosition.Y + 48), this.Color, 0f, Game1.Utility.Origin,this.TextScale, SpriteEffects.None,Utility.StandardButtonDepth + .1f);
                if(this.DisplayTitle)
                {
                
                    spriteBatch.DrawString(Game1.AllTextures.MenuText, this.TitleString, this.TitleTextPosition, this.Color, 0f, Game1.Utility.Origin, this.TextScale + .5f, SpriteEffects.None,Utility.StandardButtonDepth + .1f);
                }

                if(this.Sprites != null)
                {
                    foreach (Sprite sprite in this.Sprites)
                    {
                        sprite.Draw(spriteBatch, .99f);
                    }
                }
                
            }

        }
    }
}

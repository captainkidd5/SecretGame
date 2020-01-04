using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.SanctuaryStuff;

namespace SecretProject.Class.UI.SanctuaryStuff
{
    public class Rack
    {
        public int Index { get; set; }

        public GraphicsDevice Graphics { get; set; }
        public Vector2 RackPosition { get; set; }
        public CompletionRequirement Requirement { get; set; }
        public List<Button> RewardIcons { get; set; }

        public float Scale { get; set; }

        public int GoldAmount { get; set; }

        

        public Rack(int index, GraphicsDevice graphics, Vector2 rackPosition, CompletionRequirement requirement)
        {
            this.Index = index;
            this.Graphics = graphics;
            this.RackPosition = rackPosition;
            this.Requirement = requirement;
            RewardIcons = new List<Button>();
            this.GoldAmount = requirement.GoldAmount;
            int buttonIdex = 0;
            for (int i = 0; i < Requirement.SanctuaryRewards.Count; i++)
            {

                RewardIcons.Add(new Button(Game1.AllTextures.ItemSpriteSheet, new Rectangle(736, 32, 32, 32),
                 this.Graphics, new Vector2(RackPosition.X + 64 * buttonIdex, RackPosition.Y), Controls.CursorType.Normal, 2f, Game1.ItemVault.GenerateNewItem((int)this.Requirement.SanctuaryRewards[i], null)));
                buttonIdex++;


            }
            if (GoldAmount > 0)
            {
                RewardIcons.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(736, 32, 32, 32),
                    this.Graphics, new Vector2(RackPosition.X + 64 * buttonIdex, RackPosition.Y), Controls.CursorType.Normal)
                {
                    ItemSourceRectangleToDraw = new Rectangle(16, 288, 32, 32),
                    
                }); ;
            }
        }

        public void Update(GameTime gameTime, Vector2 position, float scale)
        {
            this.Scale = scale;

            for (int i = 0; i < this.RewardIcons.Count; i++)
            {
                this.RewardIcons[i].Position = new Vector2(position.X + 48 * Scale + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Width + i * 32 * Scale, position.Y + 128 + (32 * Index * scale));
                this.RewardIcons[i].Update(Game1.myMouseManager);
                if (this.RewardIcons[i].IsHovered)
                {
                    Game1.Player.UserInterface.InfoBox.IsActive = true;
                    if (RewardIcons[i].Item != null)
                    {
                        Game1.Player.UserInterface.InfoBox.FitText("Unlocks: " + this.RewardIcons[i].Item.Name, 1f);
                    }
                    else
                    {
                        Game1.Player.UserInterface.InfoBox.FitText(this.GoldAmount.ToString() + " Coins", 1f);
                    }

                    Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(Game1.myMouseManager.Position.X + 48, Game1.myMouseManager.Position.Y + 48);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Rectangle lineSeparationRectangle)
        {
            float baseY = position.Y + 128 + (32 * this.Index * this.Scale);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, Requirement.String, new Vector2(position.X, baseY + 16), Color.Black, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Utility.StandardButtonDepth + .03f);

            //Item Image
            spriteBatch.Draw(Game1.AllTextures.MasterTileSet, new Rectangle((int)(position.X + Requirement.ImageLocation.X * (this.Scale - 1)), (int)baseY, 48, 48),
                Requirement.SourceRectangle, Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, Utility.StandardButtonDepth + .03f);

            //separation line
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(position.X, baseY + 64), lineSeparationRectangle, Color.White, 0f, Game1.Utility.Origin, Scale, SpriteEffects.None, Utility.StandardButtonDepth + .03f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, Requirement.CurrentCount.ToString() + "/" + Requirement.CountRequired.ToString(), new Vector2(position.X + Requirement.ImageLocation.X * (this.Scale - 1) + 32 * this.Scale, baseY),
                Color.Black, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardButtonDepth + .03f);

            for (int i = 0; i < RewardIcons.Count; i++)
            {
                if(RewardIcons[i].Item != null)
                {
                    this.RewardIcons[i].Draw(spriteBatch, this.RewardIcons[i].ItemSourceRectangleToDraw,
                    this.RewardIcons[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, "", this.RewardIcons[i].Position,
                    Color.White, 2, 2, Utility.StandardButtonDepth + .03f);
                }
                else
                {
                    this.RewardIcons[i].Draw(spriteBatch, this.RewardIcons[i].ItemSourceRectangleToDraw,
                    this.RewardIcons[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, "", this.RewardIcons[i].Position,
                    Color.White, 2, 1, Utility.StandardButtonDepth + .03f);
                    this.RewardIcons[i].HitBoxScale = 2f;
                }
                
            }

        }
    }
}

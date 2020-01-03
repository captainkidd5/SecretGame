using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using System.Collections.Generic;
using XMLData.SanctuaryStuff;

namespace SecretProject.Class.UI.SanctuaryStuff
{
    public class CompletionPage : IPage
    {
        public GraphicsDevice Graphics { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CompletionRequirement> SanctuaryRequirements { get; set; }

        public List<Button> RewardIcons { get; set; }
        public Button FinalRewardButton { get; set; }

        public SanctuaryReward FinalReward { get; set; }

        public CompletionPage(GraphicsDevice graphics)
        {
            this.SanctuaryRequirements = new List<CompletionRequirement>();
            this.RewardIcons = new List<Button>();
            this.Graphics = graphics;
            this.Name = "";
            this.Description = "";

        }
        public void Load()
        {
            for (int i = 0; i < this.SanctuaryRequirements.Count; i++)
            {
                this.RewardIcons.Add(new Button(Game1.AllTextures.ItemSpriteSheet, new Rectangle(720, 128, 32, 32),
                     this.Graphics, new Vector2(1, 1), Controls.CursorType.Normal, 2f)
                {
                    ItemSourceRectangleToDraw = Game1.ItemVault.GenerateNewItem((int)this.SanctuaryRequirements[i].SanctuaryReward, null).SourceTextureRectangle
                });

            }
            FinalRewardButton = new Button(Game1.AllTextures.ItemSpriteSheet, new Rectangle(720, 128, 32, 32),
                      this.Graphics, new Vector2(1, 1), Controls.CursorType.Normal, 2f);
        }
        public void Update(GameTime gameTime)
        {

        }
        public float Scale { get; set; }
        public void Update(GameTime gameTime, Vector2 position, float scale)
        {
            this.Scale = scale;
            for (int i = 0; i < this.RewardIcons.Count; i++)
            {
                this.RewardIcons[i].Position = new Vector2(position.X + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Width * scale - this.RewardIcons[i].BackGroundSourceRectangle.Width * scale * 2, position.Y + 64 + (32 * i * scale));
                this.RewardIcons[i].Update(Game1.myMouseManager);
                if (this.RewardIcons[i].IsHovered)
                {
                    Item item = Game1.ItemVault.GenerateNewItem((int)this.SanctuaryRequirements[i].SanctuaryReward, null);
                    Game1.Player.UserInterface.InfoBox.IsActive = true;
                    Game1.Player.UserInterface.InfoBox.FitText("Unlocks: " + item.Name, 1f);
                    Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(Game1.myMouseManager.Position.X + 48, Game1.myMouseManager.Position.Y + 48);
                }
            }
            FinalRewardButton.Position = new Vector2(position.X + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Width * scale/2, position.Y + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Height * Scale);
            FinalRewardButton.Update(Game1.myMouseManager);
            if (FinalRewardButton.IsHovered)
            {
                Item item = Game1.ItemVault.GenerateNewItem((int)this.FinalReward, null);
                Game1.Player.UserInterface.InfoBox.IsActive = true;
                Game1.Player.UserInterface.InfoBox.FitText("Unlocks: " + item.Name, 1f);
                Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(Game1.myMouseManager.Position.X + 48, Game1.myMouseManager.Position.Y + 48);
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Name, new Vector2(position.X + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Width / 2, position.Y),
                    Color.Black, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Description, new Vector2(position.X, position.Y + 32),
                    Color.Black, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
            for (int i = 0; i < this.SanctuaryRequirements.Count; i++)
            {
                CompletionRequirement requirement = this.SanctuaryRequirements[i];


                spriteBatch.DrawString(Game1.AllTextures.MenuText, requirement.String, new Vector2(position.X, position.Y + 64 + (32 * i * this.Scale)), Color.Black, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
                spriteBatch.Draw(Game1.AllTextures.MasterTileSet, new Rectangle((int)(position.X + requirement.ImageLocation.X * (this.Scale - 1)), (int)(position.Y + 64 + (32 * i * this.Scale)), 48, 48),
                requirement.SourceRectangle, Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, requirement.CurrentCount.ToString() + "/" + requirement.CountRequired.ToString(), new Vector2(position.X + requirement.ImageLocation.X * (this.Scale - 1) + 32 * this.Scale, position.Y + 64 + (32 * i * this.Scale)),
                    Color.Black, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
                this.RewardIcons[i].Draw(spriteBatch, this.RewardIcons[i].ItemSourceRectangleToDraw, this.RewardIcons[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, "", this.RewardIcons[i].Position, Color.White, 2, 2, Game1.Utility.StandardButtonDepth + .03f);
            }
            this.FinalRewardButton.Draw(spriteBatch, FinalRewardButton.ItemSourceRectangleToDraw, FinalRewardButton.BackGroundSourceRectangle, Game1.AllTextures.MenuText, "", FinalRewardButton.Position, Color.White, 2, 2, Game1.Utility.StandardButtonDepth + .03f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }


    }
}

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

        public Button FinalRewardButton { get; set; }

        public SanctuaryReward FinalReward { get; set; }

        public Rectangle LineSeparationSourceRectangle { get; set; }

        public List<Rack> ReqRacks { get; set; }

        public CompletionPage(GraphicsDevice graphics)
        {
            this.SanctuaryRequirements = new List<CompletionRequirement>();
            this.Graphics = graphics;
            this.Name = "";
            this.Description = "";
            this.LineSeparationSourceRectangle = new Rectangle(453, 147, 262, 2);
            this.ReqRacks = new List<Rack>();
        }
        public void Load()
        {
    
            for (int i = 0; i < this.SanctuaryRequirements.Count; i++)
            {
                this.ReqRacks.Add(new Rack(i, this.Graphics, Vector2.One, this.SanctuaryRequirements[i]));

                //if(this.SanctuaryRequirements[i].GoldAmount > 0)
                //{
                //    this.ReqRacks[i].RewardIcons
                //    this.ReqRacks.Add(new Rack(requirementIndex, this.Graphics, Vector2.One, this.SanctuaryRequirements[i], this.SanctuaryRequirements[i].GoldAmount));
                //    requirementIndex++;
                //}
            }
            FinalRewardButton = new Button(Game1.AllTextures.ItemSpriteSheet, new Rectangle(720, 128, 32, 32),
                      this.Graphics, new Vector2(1, 1), Controls.CursorType.Normal, 2f, Game1.ItemVault.GenerateNewItem((int)FinalReward, null));

        }
        public void Update(GameTime gameTime)
        {

        }
        public float Scale { get; set; }
        public void Update(GameTime gameTime, Vector2 position, float scale)
        {
            this.Scale = scale;
            for(int i =0; i < ReqRacks.Count; i++)
            {
                ReqRacks[i].Update(gameTime, position, scale);
            }

            FinalRewardButton.Position = new Vector2(position.X + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Width /2, position.Y + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Height * scale /2);
            FinalRewardButton.Update(Game1.myMouseManager);
            if (FinalRewardButton.IsHovered)
            {
                Game1.Player.UserInterface.InfoBox.IsActive = true;
                Game1.Player.UserInterface.InfoBox.FitText("Unlocks: " + FinalRewardButton.Item.Name, 1f);
                Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(Game1.myMouseManager.Position.X + 48, Game1.myMouseManager.Position.Y + 48);
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Name, new Vector2(position.X + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Width / 2, position.Y - 16),
                    Color.Black, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Description, new Vector2(position.X, position.Y + 24),
                    Color.Black, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
            for (int i = 0; i < this.ReqRacks.Count; i++)
            {
                ReqRacks[i].Draw(spriteBatch, position, this.LineSeparationSourceRectangle);
            }
            this.FinalRewardButton.Draw(spriteBatch, FinalRewardButton.ItemSourceRectangleToDraw, FinalRewardButton.BackGroundSourceRectangle, Game1.AllTextures.MenuText, "", FinalRewardButton.Position, Color.White, 2, 2, Game1.Utility.StandardButtonDepth + .03f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }


    }
}

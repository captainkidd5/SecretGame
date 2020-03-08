using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Universal;
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
        public int GIDUnlock { get; set; }
        public string GIDUnlockDescription { get; set; }

        public Rectangle LineSeparationSourceRectangle { get; set; }

        public List<Rack> ReqRacks { get; set; }

        public int CurrentRackIndex { get; set; }
        public int MaxRacksPerPage { get; set; }

        public Button ScrollUpButton { get; set; }
        public Button ScrollDownButton { get; set; }

        public CompletionPage(GraphicsDevice graphics)
        {
            this.SanctuaryRequirements = new List<CompletionRequirement>();
            this.Graphics = graphics;
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.LineSeparationSourceRectangle = new Rectangle(464, 112, 262, 2);
            this.ReqRacks = new List<Rack>();
            this.GIDUnlock = 0;
            this.GIDUnlockDescription = string.Empty;
            this.CurrentRackIndex = 1;
            this.MaxRacksPerPage = 3;
            
        }
        public void Load(Vector2 backgroundPosition, Rectangle backGroundSourceRectangle)
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
            if(FinalReward == null)
            {
                FinalReward = new SanctuaryReward();
            }
            FinalRewardButton = new Button(Game1.AllTextures.ItemSpriteSheet, new Rectangle(128, 688, 80, 32),
                      this.Graphics, new Vector2(1, 1), Controls.CursorType.Normal, 2f, Game1.ItemVault.GenerateNewItem((int)FinalReward.ItemUnlock, null))
            { Description = TextBuilder.ParseText("Complete the page to unlock the " + Game1.ItemVault.GetItem((int)FinalReward.ItemUnlock).Name, 112 * 2.5f, 1.5f) };

            this.Description = TextBuilder.ParseText(this.Description, 400, 1f);
            this.ScrollDownButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(560, 655, 16, 32), this.Graphics, new Vector2(backgroundPosition.X + backGroundSourceRectangle.Width, backgroundPosition.Y + 64), Controls.CursorType.Normal, 2f, null);
            this.ScrollUpButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(544, 655, 16, 32), this.Graphics, new Vector2(backgroundPosition.X + backGroundSourceRectangle.Width, backgroundPosition.Y), Controls.CursorType.Normal, 2f, null);
        }
        public void Update(GameTime gameTime)
        {

        }
        public float Scale { get; set; }
        public void Update(GameTime gameTime, Vector2 position, float scale)
        {
            this.Scale = scale;
            int updateIndex = 0;
            for (int i = CurrentRackIndex; i < CurrentRackIndex + this.MaxRacksPerPage; i++)
            {
                if (CurrentRackIndex + this.MaxRacksPerPage <= ReqRacks.Count)
                {
                    ReqRacks[i].Update(gameTime, position, scale, updateIndex);
                    updateIndex++;
                }
            }

            FinalRewardButton.Position = new Vector2(position.X + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Width + 104, position.Y - 16);
            FinalRewardButton.Update(Game1.MouseManager);
            if (FinalRewardButton.IsHovered)
            {
                Game1.Player.UserInterface.InfoBox.IsActive = true;
                //Game1.Player.UserInterface.InfoBox.FitText("Complete the page to unlock the " + FinalRewardButton.Item.Name, 1f);
                Game1.Player.UserInterface.InfoBox.StringToWrite = FinalRewardButton.Description;
                Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(Game1.MouseManager.Position.X + 48, Game1.MouseManager.Position.Y + 48);
            }
            ScrollUpButton.Update(Game1.MouseManager);
            ScrollDownButton.Update(Game1.MouseManager);
            if (ScrollUpButton.isClicked)
            {
     
                    CurrentRackIndex--;
                

            }
            else if (ScrollDownButton.isClicked)
            {
               
                    CurrentRackIndex++;
                
            }
            if(Game1.MouseManager.HasScrollWheelValueIncreased)
            {
                CurrentRackIndex--;
            }
            else if(Game1.MouseManager.HasScrollWheelValueDecreased)
            {
                CurrentRackIndex++;
            }

            if (CurrentRackIndex > ReqRacks.Count - this.MaxRacksPerPage)
            {
                CurrentRackIndex = ReqRacks.Count - this.MaxRacksPerPage;
            }
            if (CurrentRackIndex < 0)
            {
                CurrentRackIndex = 0;
            }

        }
        public bool CheckFinalReward()
        {
            if (CanClaimFinalReward())
            {
                //Game1.Player.UserInterface.CraftingMenu.UnlockRecipe(FinalRewardButton.Item.ID);
                FinalRewardButton.Description = TextBuilder.ParseText(Game1.ItemVault.GetItem((int)FinalReward.ItemUnlock).Name + " recipe has been added to the crafting guide!", 112 * 2.5f, 1.5f);
                Game1.Player.UserInterface.AddAlert(AlertType.Normal,AlertSize.Large, Game1.Player.position, "You have earned a new reward, check the Sanctuary Log to claim it!");
                if (this.GIDUnlock != 0)
                {
                    Game1.OverWorldSpawnHolder.UnlockSpawnElement(this.GIDUnlock);
                    Game1.Player.UserInterface.AddAlert(AlertType.Normal,AlertSize.Large, Vector2.Zero, this.GIDUnlockDescription);
                }
                return true;
            }
            return false;
        }
        public bool CanClaimFinalReward()
        {
            for (int i = 0; i < this.SanctuaryRequirements.Count; i++)
            {
                if (!SanctuaryRequirements[i].Satisfied)
                {
                    return false;
                }
            }
            return true;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Name, new Vector2(position.X + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Width / 2 - 64, position.Y - 16),
                    Color.Black, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Rewards", new Vector2(position.X + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Width + 64 * Scale, position.Y + 64),
                   Color.Black, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Description, new Vector2(position.X, position.Y + 24),
                    Color.Black, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
            int drawIndex = 0;
            for (int i = CurrentRackIndex; i < CurrentRackIndex + this.MaxRacksPerPage; i++)
            {
                if (CurrentRackIndex + this.MaxRacksPerPage <= ReqRacks.Count)
                {
                    ReqRacks[i].Draw(spriteBatch, position, drawIndex, this.LineSeparationSourceRectangle);
                    drawIndex++;
                }

            }
            this.FinalRewardButton.Draw(spriteBatch, FinalRewardButton.ItemSourceRectangleToDraw, FinalRewardButton.BackGroundSourceRectangle, Game1.AllTextures.MenuText, "", FinalRewardButton.Position, Color.White, 2, 3, Game1.Utility.StandardButtonDepth + .03f);

            ScrollUpButton.Draw(spriteBatch);
            ScrollDownButton.Draw(spriteBatch);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }


    }
}

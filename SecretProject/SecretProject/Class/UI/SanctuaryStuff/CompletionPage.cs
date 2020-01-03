using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
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

        public CompletionPage(GraphicsDevice graphics)
        {
            this.SanctuaryRequirements = new List<CompletionRequirement>();
            this.RewardIcons = new List<Button>();
            this.Graphics = graphics;
            

        }
        public void Load()
        {
            for (int i = 0; i < SanctuaryRequirements.Count; i++)
            {
                RewardIcons.Add(new Button(Game1.AllTextures.ItemSpriteSheet, new Rectangle(720, 128, 32, 32),
                     Graphics, new Vector2(1, 1), Controls.CursorType.Normal, 2f)
                { 
                ItemSourceRectangleToDraw = Game1.ItemVault.GenerateNewItem((int)SanctuaryRequirements[i].SanctuaryReward, null).SourceTextureRectangle
                });

            }
        }
        public void Update(GameTime gameTime)
        {

        }
        public float Scale { get; set; }
        public void Update(GameTime gameTime, Vector2 position, float scale)
        {
            this.Scale = scale;
            for (int i = 0; i < RewardIcons.Count; i++)
            {
                RewardIcons[i].Position = new Vector2(position.X + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Width * scale - RewardIcons[i].BackGroundSourceRectangle.Width * scale * 2, position.Y + 64 + (32 * i * scale));
                RewardIcons[i].Update(Game1.myMouseManager);
                if(RewardIcons[i].IsHovered)
                {
                    Item item = Game1.ItemVault.GenerateNewItem((int)SanctuaryRequirements[i].SanctuaryReward, null);
                    Game1.Player.UserInterface.InfoBox.IsActive = true;
                    Game1.Player.UserInterface.InfoBox.FitText("Unlocks: " + item.Name, 1f);
                    Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(Game1.myMouseManager.Position.X + 48, Game1.myMouseManager.Position.Y + 48) ;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, Name, new Vector2(position.X + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Width / 2, position.Y),
                    Color.Black, 0f, Game1.Utility.Origin, Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, Description, new Vector2(position.X , position.Y + 32),
                    Color.Black, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
            for (int i = 0; i < SanctuaryRequirements.Count; i++)
            {
                spriteBatch.DrawString(Game1.AllTextures.MenuText, SanctuaryRequirements[i].String, new Vector2(position.X, position.Y + 64 + (32 * i * Scale)), Color.Black, 0f, Game1.Utility.Origin, Scale - 1, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
                spriteBatch.Draw(Game1.AllTextures.MasterTileSet, new Rectangle((int)(position.X + SanctuaryRequirements[i].ImageLocation.X * (Scale - 1)), (int)(position.Y + 64 + (32 * i * Scale)), 48, 48),
                SanctuaryRequirements[i].SourceRectangle, Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, SanctuaryRequirements[i].CurrentCount.ToString() + "/" + SanctuaryRequirements[i].CountRequired.ToString(), new Vector2(position.X + SanctuaryRequirements[i].ImageLocation.X * (Scale - 1) + 32 * Scale, position.Y + 64 + (32 * i * Scale)),
                    Color.Black, 0f, Game1.Utility.Origin, Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
                RewardIcons[i].Draw(spriteBatch, RewardIcons[i].ItemSourceRectangleToDraw, RewardIcons[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, "", RewardIcons[i].Position, Color.White, 2, 2, Game1.Utility.StandardButtonDepth + .03f);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }


    }
}

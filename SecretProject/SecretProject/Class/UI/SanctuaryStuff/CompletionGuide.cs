using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;
using System.Collections.Generic;
using XMLData.SanctuaryStuff;

namespace SecretProject.Class.UI.SanctuaryStuff
{
    public class CompletionGuide
    {
        public GraphicsDevice Graphics { get; set; }


        //Background Information
        public Rectangle BackGroundSourceRectangle { get; set; }
        public Vector2 BackGroundPosition { get; set; }
        public float BackGroundScale { get; set; }

        public List<CompletionCategory> CategoryTabs { get; set; }
        public SanctuaryHolder SanctuaryHolder { get; set; }
        public CompletionCategory ActiveTab { get; set; }

        public CompletionGuide(GraphicsDevice graphicsDevice, SanctuaryHolder sanctuaryHolder)
        {
            this.Graphics = graphicsDevice;
            this.SanctuaryHolder = sanctuaryHolder;
            this.BackGroundSourceRectangle = new Rectangle(448, 112, 336, 208);
            this.BackGroundPosition = new Vector2(Game1.Utility.CenterScreenX, Game1.Utility.CenterScreenY);
            this.BackGroundScale = 2f;
            this.CategoryTabs = new List<CompletionCategory>()
            {
                new CompletionCategory("Flora", this.Graphics,this.BackGroundPosition,new Rectangle(464,80, 48, 32), this.BackGroundScale),
                new CompletionCategory("Minerals", this.Graphics,this.BackGroundPosition,new Rectangle(528,80, 48, 32), this.BackGroundScale),
                new CompletionCategory("Fauna", this.Graphics,this.BackGroundPosition,new Rectangle(592,80, 48, 32), this.BackGroundScale),
                new CompletionCategory("Special", this.Graphics,this.BackGroundPosition,new Rectangle(656,80, 48, 32), this.BackGroundScale),

            };
            for (int i = 0; i < this.CategoryTabs.Count; i++)
            {
                this.CategoryTabs[i].Pages.Add(new CompletionPage(graphicsDevice));

            }

            for (int i = 0; i < sanctuaryHolder.AllPages.Count; i++)
            {
                for (int j = 0; j < this.CategoryTabs.Count; j++)
                {
                    if (sanctuaryHolder.AllPages[i].Tab == j)
                    {
                        this.CategoryTabs[j].Pages[0].FinalReward = sanctuaryHolder.AllPages[i].FinalReward;
                        this.CategoryTabs[j].Pages[0].Name = sanctuaryHolder.AllPages[i].Name;
                        this.CategoryTabs[j].Pages[0].Description = sanctuaryHolder.AllPages[i].Description;
                        this.CategoryTabs[j].Pages[0].GIDUnlock = (int)sanctuaryHolder.AllPages[i].GIDUnlock;
                        this.CategoryTabs[j].Pages[0].GIDUnlockDescription = sanctuaryHolder.AllPages[i].GIDUnlockDescription;
                        for (int p = 0; p < sanctuaryHolder.AllPages[i].AllRequirements.Count; p++)
                        {
                            
                            
                            SanctuaryRequirement requirement = sanctuaryHolder.AllPages[i].AllRequirements[p];

                            List<SanctuaryReward> rewards = new List<SanctuaryReward>();
                            for(int r = 0; r < requirement.Rewards.Count; r++)
                            {
                                rewards.Add(requirement.Rewards[r]);
                            }
                            this.CategoryTabs[j].Pages[0].SanctuaryRequirements.Add(new CompletionRequirement(Graphics, this.CategoryTabs[j].Pages[0],requirement.ItemID, requirement.GIDRequired,
                                requirement.NumberRequired, requirement.Description,
                            requirement.Rectangle, rewards, requirement.Rewards[0].ItemUnlock, requirement.Rewards[0].GIDUnlock, requirement.GIDUnlockDescription));
                        }


                    }
                }
            }
            for (int i = 0; i < this.CategoryTabs.Count; i++)
            {
                for (int j = 0; j < this.CategoryTabs[i].Pages.Count; j++)
                {
                    this.CategoryTabs[i].Pages[j].Load(BackGroundPosition, BackGroundSourceRectangle);
                }
            }
            this.ActiveTab = this.CategoryTabs[0];


        }

        public Vector2 DrawPosition { get; set; }
        public void Update(GameTime gameTime)
        {
            this.DrawPosition = new Vector2(this.BackGroundPosition.X - (this.BackGroundSourceRectangle.Width * this.BackGroundScale) / 2,
               this.BackGroundPosition.Y - (this.BackGroundSourceRectangle.Height * this.BackGroundScale) / 2);
            for (int tab = 0; tab < this.CategoryTabs.Count; tab++)
            {
                this.CategoryTabs[tab].Button.Position = new Vector2(this.DrawPosition.X + 64 * tab * this.BackGroundScale, this.DrawPosition.Y - 32 * this.BackGroundScale);
                this.CategoryTabs[tab].Button.Update();
                if (this.CategoryTabs[tab].Button.IsHovered)
                {
                    this.CategoryTabs[tab].ButtonColorMultiplier = .5f;
                }
                else
                {
                    this.CategoryTabs[tab].ButtonColorMultiplier = 1f;
                }
                if (this.CategoryTabs[tab].Button.isClicked)
                {
                    this.ActiveTab = this.CategoryTabs[tab];
                }
            }

            this.ActiveTab.Update(gameTime, new Vector2(this.DrawPosition.X + 16 * this.BackGroundScale, this.DrawPosition.Y + 16 * this.BackGroundScale), this.BackGroundScale);
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.DrawPosition, this.BackGroundSourceRectangle,
                   Color.White, 0f, Game1.Utility.Origin, this.BackGroundScale, SpriteEffects.None,Utility.StandardButtonDepth);
            this.ActiveTab.Draw(spriteBatch, new Vector2(this.DrawPosition.X + 16 * this.BackGroundScale, this.DrawPosition.Y + 16 * this.BackGroundScale));
            for (int tab = 0; tab < this.CategoryTabs.Count; tab++)
            {
                this.CategoryTabs[tab].Button.DrawNormal(spriteBatch, this.CategoryTabs[tab].Button.Position, this.CategoryTabs[tab].Button.BackGroundSourceRectangle, Color.White * this.CategoryTabs[tab].ButtonColorMultiplier, 0f, Game1.Utility.Origin, this.CategoryTabs[tab].Button.HitBoxScale, SpriteEffects.None,Utility.StandardButtonDepth);
            }
        }
    }
}

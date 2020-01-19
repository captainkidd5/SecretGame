using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.TileStuff;
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
        public int GIDUnlock { get; set; }
        public string GIDUnlockDescription { get; set; }


        public float[] ColorMultiplier { get; set; }

        public Rectangle LockedSourceRectangle { get; set; }

        public SimpleTimer ChainsFadeTimer { get; set; }
        public float ChainsColorMultiplier { get; set; }

        public Rack(int index, GraphicsDevice graphics, Vector2 rackPosition, CompletionRequirement requirement)
        {
            this.Index = index;
            this.Graphics = graphics;
            this.RackPosition = rackPosition;
            this.Requirement = requirement;
            RewardIcons = new List<Button>();
            int buttonIndex = 0;
            for (int i = 0; i < Requirement.SanctuaryRewards.Count; i++)
            {
                if(Requirement.SanctuaryRewards[i].GoldAmount > 0)
                {
                    RewardIcons.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(736, 32, 32, 32),
                    this.Graphics, new Vector2(RackPosition.X + 64 * buttonIndex, RackPosition.Y), Controls.CursorType.Normal)
                    {
                        ItemSourceRectangleToDraw = new Rectangle(16, 288, 32, 32),

                    }); ;
                    
                }
                else if(Requirement.SanctuaryRewards[i].ItemUnlock != 0)
                {
                    RewardIcons.Add(new Button(Game1.AllTextures.ItemSpriteSheet, new Rectangle(736, 32, 32, 32),
                 this.Graphics, new Vector2(RackPosition.X + 64 * buttonIndex, RackPosition.Y), Controls.CursorType.Normal, 2f, Game1.ItemVault.GenerateNewItem((int)this.Requirement.SanctuaryRewards[i].ItemUnlock, null)));
                }
                else if(Requirement.SanctuaryRewards[i].GIDUnlock != 0)
                {
                    RewardIcons.Add(new Button(Game1.AllTextures.MasterTileSet, new Rectangle(736, 32, 32, 32),
                 this.Graphics, new Vector2(RackPosition.X + 64 * buttonIndex, RackPosition.Y), Controls.CursorType.Normal, 2f)
                        {
                        ItemSourceRectangleToDraw = TileUtility.GetSourceRectangleWithoutTile(Requirement.SanctuaryRewards[i].GIDUnlock,100)
                    });
                }
                
                buttonIndex++;



            }
            this.ColorMultiplier = new float[buttonIndex];
            this.ChainsColorMultiplier = 1f;
            for (int i = 0; i < this.ColorMultiplier.Length; i++)
            {
                this.ColorMultiplier[i] = 1f;
            }
            this.LockedSourceRectangle = new Rectangle(768, 32, 32, 32);

            ChainsFadeTimer = new SimpleTimer(2f);
        }

        public void Update(GameTime gameTime, Vector2 position, float scale, int rackIndex)
        {
            this.Scale = scale;

            for (int i = 0; i < this.RewardIcons.Count; i++)
            {
                this.RewardIcons[i].Position = new Vector2(position.X + 48 * Scale + Game1.Player.UserInterface.CompletionHub.AllGuides[0].BackGroundSourceRectangle.Width + i * 32 * Scale, position.Y + 120 + (32 * rackIndex * scale * (float)1.25f));
                this.RewardIcons[i].Update(Game1.myMouseManager);
                if (this.RewardIcons[i].IsHovered)
                {
                    Game1.Player.UserInterface.InfoBox.IsActive = true;

                        if(!this.Requirement.IndividualRewards[i])
                        {
                            Game1.Player.UserInterface.InfoBox.FitText(Requirement.SanctuaryRewards[i].Description, 1f);
                        }
                        else
                        {
                            Game1.Player.UserInterface.InfoBox.FitText(Requirement.SanctuaryRewards[i].Description + " (Claimed)", 1f);
                        }
                        
                    
                    
                    

                    Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(Game1.myMouseManager.Position.X + 48, Game1.myMouseManager.Position.Y + 48);

                    if (this.Requirement.Satisfied && !this.Requirement.IndividualRewards[i])
                    {
                        if (RewardIcons[i].isClicked && this.Requirement.ChainsTransitionCompleted)
                        {
                            if(Requirement.SanctuaryRewards[i].ItemUnlock != 0)
                            {
                                this.Requirement.ClaimReward(i, RewardIcons[i].Position, RewardIcons[i].Item);
                                this.ColorMultiplier[i] = .25f;
                            }
                            else if(Requirement.SanctuaryRewards[i].GIDUnlock != 0)
                            {
                                this.Requirement.ClaimReward(i, RewardIcons[i].Position, gidUnlock: Requirement.SanctuaryRewards[i].GIDUnlock);
                                this.ColorMultiplier[i] = .25f;
                            }
                            else if(Requirement.SanctuaryRewards[i].GoldAmount != 0)
                            {
                                this.Requirement.ClaimReward(i, RewardIcons[i].Position, gold: Requirement.SanctuaryRewards[i].GoldAmount);
                                this.ColorMultiplier[i] = .25f;
                            }
                            
                        }
                    }
                }
                if(this.Requirement.Satisfied && !this.Requirement.ChainsTransitionCompleted)
                {
                    ChainsColorMultiplier-= .01f;
                    if(ChainsFadeTimer.Run(gameTime))
                    {
                        this.Requirement.ChainsTransitionCompleted = true;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, int rackIndex, Rectangle lineSeparationRectangle)
        {
            float baseY = position.Y + 128 + (32 * rackIndex * this.Scale * (float)1.25);
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
                if (!this.Requirement.ChainsTransitionCompleted)
                {
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.RewardIcons[i].Position, this.LockedSourceRectangle, Color.White * ChainsColorMultiplier, 0f, Game1.Utility.Origin, 2, SpriteEffects.None, Utility.StandardButtonDepth + .04f);
                }
                if (RewardIcons[i].Item != null)
                {
                    this.RewardIcons[i].Draw(spriteBatch, this.RewardIcons[i].ItemSourceRectangleToDraw,
                    this.RewardIcons[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, "", this.RewardIcons[i].Position,
                    Color.White * this.ColorMultiplier[i], 2, 2, Utility.StandardButtonDepth + .03f);
                }
                else
                {
                    this.RewardIcons[i].Draw(spriteBatch, this.RewardIcons[i].ItemSourceRectangleToDraw,
                    this.RewardIcons[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, "", this.RewardIcons[i].Position,
                    Color.White * this.ColorMultiplier[i], 2, 1, Utility.StandardButtonDepth + .03f);
                    this.RewardIcons[i].HitBoxScale = 2f;
                }

            }

        }
    }
}

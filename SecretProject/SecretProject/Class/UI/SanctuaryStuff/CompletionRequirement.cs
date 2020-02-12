using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using System.Collections.Generic;
using XMLData.SanctuaryStuff;

namespace SecretProject.Class.UI.SanctuaryStuff
{
    public class CompletionRequirement
    {
        public GraphicsDevice Graphics { get; set; }
        public CompletionPage CompletionPage { get; set; }
        public int ItemID { get; set; }
        public int GID { get; set; }
        public int CountRequired { get; set; }
        public int CurrentCount { get; set; }
        public bool Satisfied { get; set; }
        public Rectangle SourceRectangle { get; set; }

        public string String { get; set; }
        public Vector2 ImageLocation { get; set; }

        public List<SanctuaryReward> SanctuaryRewards { get; set; }


        public bool[] IndividualRewards { get; set; }
        public bool AllRewardsClaimed { get; set; }
        public bool ChainsTransitionCompleted { get; set; }

        public CompletionRequirement(GraphicsDevice graphics, CompletionPage completionPage, int itemID, int gid, int countRequired, string description, Rectangle sourceRectangle, List<SanctuaryReward> sanctuaryRewards, int goldAmount, int gidUnlock, string gidUnlockDescription)
        {
            this.Graphics = graphics;
            this.CompletionPage = completionPage;
            this.ItemID = itemID;
            this.GID = gid;
            this.CountRequired = countRequired;
            this.SourceRectangle = sourceRectangle;
            this.String = description;
            this.ImageLocation = Game1.AllTextures.MenuText.MeasureString(this.String);
            this.SanctuaryRewards = sanctuaryRewards;

            IndividualRewards = new bool[SanctuaryRewards.Count];
        }

        public void Increment()
        {

            this.CurrentCount++;

            if (this.CurrentCount >= this.CountRequired)
            {
                this.Satisfied = true;
                this.String = "Completed! ";
                this.ImageLocation = Game1.AllTextures.MenuText.MeasureString(this.String);

                Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.MiniReward, true, .25f);
                if (CompletionPage.CheckFinalReward())
                {
                    Game1.Player.UserInterface.AllRisingText.Add(new RisingText(Game1.Player.UserInterface.BottomBar.OpenSanctuaryMenu.Position,
               Game1.Player.UserInterface.BottomBar.OpenSanctuaryMenu.Position.Y - 200, this.CompletionPage.Name + " Completed!", 25f, Color.Blue, true, 2f));
                }
                else
                {
                    Game1.Player.UserInterface.AllRisingText.Add(new RisingText(Game1.Player.UserInterface.BottomBar.OpenSanctuaryMenu.Position,
                Game1.Player.UserInterface.BottomBar.OpenSanctuaryMenu.Position.Y - 200, this.CountRequired.ToString() + "/" + this.CountRequired.ToString() + " " + Game1.ItemVault.GetItem(this.ItemID).Name + " Completed!", 50f, Color.Blue, true, 1f));
                }


            }
            else
            {
                Game1.Player.UserInterface.AllRisingText.Add(new RisingText(Game1.Player.UserInterface.BottomBar.OpenSanctuaryMenu.Position,
                Game1.Player.UserInterface.BottomBar.OpenSanctuaryMenu.Position.Y - 200, "+ 1 " + Game1.ItemVault.GetItem(ItemID).Name + "!", 100f, Color.White, true, 1f));
            }
        }

        public void ClaimReward(int index, Vector2 rewardPosition, Item item = null, int gidUnlock = 0, int gold = 0, int lootUnlock = 0)
        {
            if (item != null)
            {
                Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(item.ID, null));
                IndividualRewards[index] = true;
                return;
            }

            if (gold > 0)
            {
                for (int i = 0; i < gold; i++)
                {
                    Game1.Player.UserInterface.AllUISprites.Add(new UISprite(UISpriteType.Coin, this.Graphics, rewardPosition, Game1.Player.UserInterface.BottomBar.GoldIconPosition, Game1.Player.UserInterface.AllUISprites));
                }
                //Game1.Player.Inventory.Money += this.GoldAmount;
                IndividualRewards[index] = true;
                return;
            }
            if (gidUnlock != 0)
            {
                if (lootUnlock != 0)
                {
                    Game1.LootBank.LootInfo[gidUnlock].LootPieces[lootUnlock].Unlocked = true;
                }
                else
                {
                    Game1.OverWorldSpawnHolder.UnlockSpawnElement(gidUnlock);
                    Game1.Player.UserInterface.AddAlert(AlertType.Normal,AlertSize.Large, Vector2.Zero, "hi");
                }
                
                

                IndividualRewards[index] = true;
                return;
            }

                if (lootUnlock != 0)
                {
                    Game1.LootBank.LootInfo[gidUnlock].LootPieces[lootUnlock].Unlocked = true;
                Game1.Player.UserInterface.AddAlert(AlertType.Normal,AlertSize.Large, Vector2.Zero, "You are now able to harvest " + Game1.ItemVault.GetItem(lootUnlock).Name);
            }
                IndividualRewards[index] = true;
            

            IndividualRewards[index] = true;
            this.AllRewardsClaimed = true;
        }
    }
}

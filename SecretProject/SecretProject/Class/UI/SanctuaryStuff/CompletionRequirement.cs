using Microsoft.Xna.Framework;
using System.Collections.Generic;
using XMLData.SanctuaryStuff;

namespace SecretProject.Class.UI.SanctuaryStuff
{
    public class CompletionRequirement
    {
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

        public int GoldAmount { get; set; }
        public bool[] IndividualRewards { get; set; }
        public bool AllRewardsClaimed { get; set; }
        public bool ChainsTransitionCompleted { get; set; }

        public CompletionRequirement(CompletionPage completionPage, int itemID, int gid, int countRequired, string description, Rectangle sourceRectangle, List<SanctuaryReward> sanctuaryRewards, int goldAmount)
        {
            this.CompletionPage = completionPage;
            this.ItemID = itemID;
            this.GID = gid;
            this.CountRequired = countRequired;
            this.SourceRectangle = sourceRectangle;
            this.String = description;
            this.ImageLocation = Game1.AllTextures.MenuText.MeasureString(this.String);
            this.SanctuaryRewards = sanctuaryRewards;
            this.GoldAmount = goldAmount;

            int boolCount = 0;
            if (GoldAmount > 0)
            {
                boolCount++;
            }
            boolCount += this.SanctuaryRewards.Count;

            IndividualRewards = new bool[boolCount];
        }

        public void Increment()
        {

            this.CurrentCount++;

            if (this.CurrentCount >= this.CountRequired)
            {
                this.Satisfied = true;
                this.String = "Completed! ";
                this.ImageLocation = Game1.AllTextures.MenuText.MeasureString(this.String);

                Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.MiniReward, true, .25f);
                if (CompletionPage.CheckFinalReward())
                {
                    Game1.Player.UserInterface.AllRisingText.Add(new RisingText(Game1.Player.UserInterface.BottomBar.OpenSanctuaryMenu.Position,
               Game1.Player.UserInterface.BottomBar.OpenSanctuaryMenu.Position.Y - 200, this.CompletionPage.Name + " Completed!", 25f, Color.Blue, true, 2f));
                }
                else
                {
                    Game1.Player.UserInterface.AllRisingText.Add(new RisingText(Game1.Player.UserInterface.BottomBar.OpenSanctuaryMenu.Position,
                Game1.Player.UserInterface.BottomBar.OpenSanctuaryMenu.Position.Y - 200, this.CountRequired.ToString() + "/" + this.CountRequired.ToString() + " " + Game1.ItemVault.GenerateNewItem(this.ItemID, null).Name + " Completed!", 50f, Color.Blue, true, 1f));
                }


            }
            else
            {
                Game1.Player.UserInterface.AllRisingText.Add(new RisingText(Game1.Player.UserInterface.BottomBar.OpenSanctuaryMenu.Position,
                Game1.Player.UserInterface.BottomBar.OpenSanctuaryMenu.Position.Y - 200, "+ 1 " + Game1.ItemVault.GenerateNewItem(this.ItemID, null).Name + "!", 100f, Color.White, true, 1f));
            }
        }

        public void ClaimReward(int index)
        {
            if (index < SanctuaryRewards.Count)
            {
                Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem((int)this.SanctuaryRewards[index], null));
                IndividualRewards[index] = true;
                return;
            }

            if (this.GoldAmount > 0)
            {
                Game1.Player.Inventory.Money += this.GoldAmount;
                Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.CoinGet, false, 1f);
            }
            IndividualRewards[index] = true;

            this.AllRewardsClaimed = true;
        }
    }
}

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using XMLData.SanctuaryStuff;

namespace SecretProject.Class.UI.SanctuaryStuff
{
    public class CompletionRequirement
    {
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

        public CompletionRequirement(int itemID, int gid, int countRequired, string description, Rectangle sourceRectangle, List<SanctuaryReward> sanctuaryRewards, int goldAmount)
        {

            this.ItemID = itemID;
            this.GID = gid;
            this.CountRequired = countRequired;
            this.SourceRectangle = sourceRectangle;
            this.String = description;
            this.ImageLocation = Game1.AllTextures.MenuText.MeasureString(this.String);
            this.SanctuaryRewards = sanctuaryRewards;
            this.GoldAmount = goldAmount;
        }

        public void Increment()
        {

            this.CurrentCount++;
            if (this.CurrentCount >= this.CountRequired)
            {
                this.Satisfied = true;
                this.String = "Completed! ";
                this.ImageLocation = Game1.AllTextures.MenuText.MeasureString(this.String);
                for(int i =0; i < SanctuaryRewards.Count; i++)
                {
                    Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem((int)this.SanctuaryRewards[i], null));
                }
                if(this.GoldAmount > 0)
                {
                    Game1.Player.Inventory.Money += this.GoldAmount;
                    Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.CoinGet, false, 1f);
                }
                Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.MiniReward, true, .25f);

            }
        }
    }
}

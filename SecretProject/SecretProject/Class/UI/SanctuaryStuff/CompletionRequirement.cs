using Microsoft.Xna.Framework;
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

        public SanctuaryReward SanctuaryReward { get; set; }

        public int GoldAmount { get; set; }

        public CompletionRequirement(int itemID, int gid, int countRequired, string description, Rectangle sourceRectangle, SanctuaryReward sanctuaryReward, int goldAmount)
        {

            this.ItemID = itemID;
            this.GID = gid;
            this.CountRequired = countRequired;
            this.SourceRectangle = sourceRectangle;
            this.String = description;
            this.ImageLocation = Game1.AllTextures.MenuText.MeasureString(this.String);
            this.SanctuaryReward = sanctuaryReward;
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
                Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem((int)this.SanctuaryReward, null));
            }
        }
    }
}

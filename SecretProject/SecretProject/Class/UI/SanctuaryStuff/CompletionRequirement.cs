using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CompletionRequirement(int itemID, int gid, int countRequired, Rectangle sourceRectangle)
        {
            this.ItemID = itemID;
            this.GID = gid;
            this.CountRequired = countRequired;
            this.SourceRectangle = sourceRectangle;
            this.String = "Gather " + CountRequired.ToString() + " " + Game1.ItemVault.GenerateNewItem(ItemID, null).Name;
            ImageLocation = Game1.AllTextures.MenuText.MeasureString(String);
        }

        public void Increment()
        {
            
            CurrentCount++;
            if(CurrentCount >= CountRequired)
            {
                Satisfied = true;
                this.String = "Completed! ";
                ImageLocation = Game1.AllTextures.MenuText.MeasureString(String);
            }
        }
    }
}

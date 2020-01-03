using SecretProject.Class.UI.SanctuaryStuff;

namespace SecretProject.Class.StageFolder
{
    public class SanctuaryTracker
    {
        public CompletionGuide CompletionGuide { get; set; }
        public SanctuaryTracker(CompletionGuide completionGuide)
        {
            this.CompletionGuide = completionGuide;
        }
        public bool UpdateCompletionGuide(int itemID)
        {
            for (int i = 0; i < this.CompletionGuide.CategoryTabs.Count; i++)
            {
                for (int j = 0; j < this.CompletionGuide.CategoryTabs[i].Pages.Count; j++)
                {
                    CompletionRequirement requirement = this.CompletionGuide.CategoryTabs[i].Pages[j].SanctuaryRequirements.Find(x => x.ItemID == itemID);
                    if (requirement != null)
                    {
                        if (requirement.CurrentCount < requirement.CountRequired)
                        {
                            requirement.Increment();
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}

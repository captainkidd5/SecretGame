using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.SanctuaryStuff
{
    public class SanctuaryPageHolder
    {
        public string Name { get; set; }
        public int Tab { get; set; }
        public string Description { get; set; }
        public List<SanctuaryRequirement> AllRequirements { get; set; }

        public SanctuaryReward FinalReward { get; set; }

        [ContentSerializer(Optional = true)]
        public GIDUnlock GIDUnlock { get; set; }

        [ContentSerializer(Optional = true)]
        public String GIDUnlockDescription { get; set; }
    }
}

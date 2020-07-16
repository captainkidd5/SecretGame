using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Universal
{
    public enum HouseLocation
    {
        Town = 0,
        Forest = 1,
        Desert = 2
    }
    public class Flags
    {
        public bool EnablePlayerInvincibility = false;
        public bool EnablePlayerCollisions = true;
        public bool EnableCutScenes = false;
        public bool EnableMusic = false;
        public bool InfiniteArrows = false;
        public bool GenerateChunkLandscape = true;
        public bool AllowNaturalNPCSpawning = true;
        public bool UpdateCharacters = true;
        public HouseLocation HouseLocation;
    }
}

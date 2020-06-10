using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.StageFolder.DungeonStuff
{
    public class DungeonPortal : Portal
    {
        public DungeonPortal(int from, int to, int safteyX, int safteyY, bool mustBeClicked) : base(from, to, safteyX, safteyY, mustBeClicked)
        {

        }
    }
}

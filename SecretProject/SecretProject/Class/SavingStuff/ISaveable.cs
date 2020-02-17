using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.SavingStuff
{
    public interface ISaveable
    {
        void Save(BinaryWriter writer);

        void Load(BinaryReader reader);

    }
}

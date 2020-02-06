using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.SavingStuff
{
    public class SaveFile
    {
        public int ID { get; set; }
        public String Path { get; set; }

        public SaveFile(int id, string path)
        {
            this.ID = id;
            this.Path = path;
        }
    }
}

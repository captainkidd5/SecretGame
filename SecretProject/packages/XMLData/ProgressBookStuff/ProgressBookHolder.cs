using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.ProgressBookStuff
{
    public class ProgressBookHolder
    {
        public int ID { get; set; }
        public List<ProgressBookPageHolder> Tabs { get; set; }
    }
}

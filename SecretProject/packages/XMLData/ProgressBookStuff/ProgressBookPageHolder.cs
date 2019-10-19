using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.ProgressBookStuff
{
    public class ProgressBookPageHolder
    {
        public string TabName { get; set; }
        public List<ProgressBookPageSkeleton> Pages { get; set; }
    }
}

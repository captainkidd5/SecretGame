using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.RouteStuff;

namespace XMLData.DialogueStuff
{
    public class DialogueDay
    {
        public Month Month { get; set; }
        public int Day { get; set; }
        public List<DialogueSkeleton> DialogueSkeletons { get; set; }


       
    }
}

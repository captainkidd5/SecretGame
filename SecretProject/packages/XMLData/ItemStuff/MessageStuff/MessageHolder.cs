using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.ItemStuff.MessageStuff
{
    public class MessageHolder
    {
        public List<Message> Messages { get; set; }

        /// <summary>
        /// Finds message which is located at that tiles position, if any.
        /// </summary>
        /// <param name="x">tile square X coord</param>
        /// <param name="y">tile square y coord</param>
        /// <returns></returns>
        public string GetMessage(int x, int y)
        {
            return Messages.Find(msg => (msg.TileX == x) && msg.TileY == y).Description;
        }
    }
}

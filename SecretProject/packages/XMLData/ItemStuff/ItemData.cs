using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace XMLData.ItemStuff
{
    public class ItemData
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int InvMaximum { get; set; }
        public int TextureColumn { get; set; }
        public int TextureRow { get; set; }
        public int Price { get; set; }

        [ContentSerializer(Optional = true)]
        public int SmeltedItem { get; set; }

        [ContentSerializer(Optional = true)]
        public bool Plantable { get; set; }

        [ContentSerializer(Optional = true)]
        public int Durability { get; set; }

        [ContentSerializer(Optional = true)]<
        public int PlaceID { get; set; }
    }
}

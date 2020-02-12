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
        public string Description { get; set; }
        public int InvMaximum { get; set; }
        public int Price { get; set; }


        [ContentSerializer(Optional = true)]
        public int SmeltedItem { get; set; }

        [ContentSerializer(Optional = true)]
        public int ProcessWoodItem { get; set; }

        [ContentSerializer(Optional = true)]
        public int FuelValue { get; set; }

        [ContentSerializer(Optional = true)]
        public bool Plantable { get; set; }

        [ContentSerializer(Optional = true)]
        public int Durability { get; set; }

        [ContentSerializer(Optional = true)]
        public byte Damage { get; set; }

        [ContentSerializer(Optional = true)]
        public int PlaceID { get; set; }

        [ContentSerializer(Optional = true)]
        public string TilingSet { get; set; }


        [ContentSerializer(Optional = true)]
        public int TilingLayer { get; set; }

        [ContentSerializer(Optional = true)]
        public ItemType Type { get; set; }

        [ContentSerializer(Optional = true)]
        public int AnimationColumn { get; set; }

        [ContentSerializer(Optional = true)]
        public bool Food { get; set; }

        [ContentSerializer(Optional = true)]
        public int StaminaRestoreAmount { get; set; }

        [ContentSerializer(Optional = true)]
        public int CrateType { get; set; }
       
        [ContentSerializer(Optional = true)]
        public byte MeatValue { get; set; }

        [ContentSerializer(Optional = true)]
        public byte VegetableValue { get; set; }

        [ContentSerializer(Optional = true)]
        public byte FruitValue { get; set; }

    }
}

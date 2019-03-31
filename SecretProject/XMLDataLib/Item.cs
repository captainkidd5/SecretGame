using System;

namespace XMLDataLib
{
    public class Item
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int Count { get; set; }
        public int WorldMaximum { get; set; }
        //public Texture2D Texture { get; set; }
        public bool IsDropped { get; set; }
       // public Sprite ItemSprite { get; set; }
        public bool IsFull { get; set; }

        public bool Ignored { get; set; }

        public bool IsMagnetized { get; set; }
        public bool IsMagnetizable { get; set; }

        public bool IsTossable { get; set; } = false;
    }
}

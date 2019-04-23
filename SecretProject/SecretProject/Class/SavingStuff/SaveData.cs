using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using SecretProject.Class.ItemStuff;

namespace SecretProject.Class.SavingStuff
{
    [Serializable]
    [XmlRoot("SaveData")]
    public class SaveData
    {
        #region player
        [XmlElement("Position")]
        public Vector2 Position
        {
            get { return Game1.Iliad.Player.Position; }
            set { Game1.Iliad.Player.Position = value; }

        }

        [XmlElement("PlayerHealth")]
        public int PlayerHealth
        {
            get { return Game1.Iliad.Player.Health; }
            set { Game1.Iliad.Player.Health = value; }
        }

        [XmlElement("WorldItems")]
        public List<SerializableKeyValue<int, Vector2>> WorldItems
        {
            get { return GetWorldItems(); }
                
        }

        public List<SerializableKeyValue<int, Vector2>> GetWorldItems()
        {
            List<SerializableKeyValue<int, Vector2>> WorldItems = new List<SerializableKeyValue<int, Vector2>>();
            foreach (WorldItem item in Game1.GetCurrentStage().allItems)
            {
                WorldItems.Add(new SerializableKeyValue<int, Vector2> { Key = item.ID, Value = item.WorldPosition });
            }
            return WorldItems;
        }

        #endregion



                //used so we can get around serializing dictionary items...
        public class SerializableKeyValue<T1, T2>
        {
            public T1 Key { get; set; }
            public T2 Value { get; set; }


        }


    }
}


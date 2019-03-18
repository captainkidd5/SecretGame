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

        #endregion

        #region playerInventory
        [XmlElement("PlayerInventory")]
        public List<InventorySlot> PlayerInventory
        {
            get { return Game1.Player.Inventory.currentInventory; }
            set { Game1.Player.Inventory.currentInventory = value; }
        }

        #endregion

        //[XmlElement("Position")]
        //public int MyProperty { get; set; }

    }
}


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;

namespace SecretProject.Class.SavingStuff
{
    [Serializable]
    [XmlRoot("SaveData")]
    public class SaveData
    {
        [XmlElement("Position")]
        public Vector2 Position
        {
            get { return Game1.iliad.Player.Position; }
            set { Game1.iliad.Player.Position = value; }

        }

        //[XmlElement("Position")]
       //public int MyProperty { get; set; }

    }
}


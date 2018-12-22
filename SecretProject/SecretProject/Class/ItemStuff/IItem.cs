using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
    interface IItem
    {
        string Name { get; set; }
        int ID { get; set; }
        Texture2D Texture { get; set; }
    }
}

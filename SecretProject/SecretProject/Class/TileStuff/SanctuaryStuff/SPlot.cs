using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// For use with the sanctuary tile manager. These are defined within the sanctuary tmx maps and are used to define areas in which certain crops may be planted.
/// </summary>
namespace SecretProject.Class.TileStuff.SanctuaryStuff
{
    public enum PlantedItemType
    {
        Crop = 1,
        Tree = 2
    }

    public class SPlot
    {
        public PlantedItemType PlantedItemType { get; set; }
        public int ItemIDAllowed { get; set; }
        public Rectangle Bounds { get; set; }

        public SPlot(PlantedItemType plantedItemType,int itemIDAllowed, int x, int y, int width, int height)
        {
            this.PlantedItemType = plantedItemType;
            this.ItemIDAllowed = itemIDAllowed;
            this.Bounds = new Rectangle(x, y, width, height);
        }
    }
}

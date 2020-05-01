using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.TileModifications
{
    public interface ITileModifiable
    {
         int TileX { get; set; }
         int TileY { get; set; }
         int TileLayer { get; set; }
         IInformationContainer Container { get; set; }
        Tile Tile { get; set; }
        bool Update(GameTime gameTime);
    }
}

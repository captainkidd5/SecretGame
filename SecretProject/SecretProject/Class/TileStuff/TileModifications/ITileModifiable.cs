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
        Tile Tile { get; set; }
        bool Update(GameTime gameTime);
    }
}

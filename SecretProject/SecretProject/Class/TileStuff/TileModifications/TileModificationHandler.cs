using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.TileModifications
{
    public class TileModificationHandler
    {
        public static float TreeFallSpeed = 2f;
        public static float TreeFallLeftRotationAmt = -2f;
        public static float TreeFallRightRotationAmt = 2f;


        internal List<ITileModifiable> Modifiers { get; set; }

        public TileModificationHandler()
        {
            this.Modifiers = new List<ITileModifiable>();
        }

        public void AddModification(Tile tile, ITileModifiable tileModifiable)
        {
            this.Modifiers.Add(tileModifiable);
        }

        public void Update(GameTime gameTime)
        {
            for(int i =0; i < this.Modifiers.Count; i++)
            {
                if(Modifiers[i].Update(gameTime))
                {
                    Modifiers.RemoveAt(i);
                }
            }
        }
    }
}

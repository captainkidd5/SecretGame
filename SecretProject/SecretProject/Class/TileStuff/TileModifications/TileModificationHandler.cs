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
        public static float TreeFallSpeed = .1f;
        public static float TreeFallLeftRotationAmt = -1.65f;
        public static float TreeFallRightRotationAmt = 1.65f;


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
                ITileModifiable modifier = Modifiers[i];
                if (modifier.Update(gameTime))
                {
                    TileUtility.FinalizeTile(modifier.TileLayer, gameTime, modifier.TileX, modifier.TileY, modifier.Container);
                    Modifiers.RemoveAt(i);
                }
            }
        }

        public static ITileModifiable GetTileModificationType(string info, IInformationContainer container, int layer, int x, int y, Tile tile, Dir dir)
        {
            switch(info)
            {
                case "tree":
                    return new TileRotator(tile,container, layer, x, y, dir);
                default:
                    throw new Exception("tile property type does not contain a definition for " + info);
            }
        }
    }
}

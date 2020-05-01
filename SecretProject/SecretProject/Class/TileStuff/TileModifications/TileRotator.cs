using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.TileModifications
{
    internal class TileRotator : ITileModifiable
    {
        public int TileX { get; set; }
        public int TileY { get; set; }
        public int TileLayer { get; set; }
        public IInformationContainer Container { get; set; }
        public Tile Tile { get; set; }
        public float DesiredRotation { get; private set; }
        public float RotationSpeed { get; private set; }

        public TileRotator(Tile tile, Dir sideOfTreePlayerOn)
        {
            this.Tile = tile;
            if (sideOfTreePlayerOn == Dir.Right)
            {
                this.DesiredRotation = TileModificationHandler.TreeFallRightRotationAmt;
                this.RotationSpeed = TileModificationHandler.TreeFallSpeed;
            }
            else if (sideOfTreePlayerOn == Dir.Left)
            {
                this.DesiredRotation = TileModificationHandler.TreeFallLeftRotationAmt;
                this.RotationSpeed = TileModificationHandler.TreeFallSpeed;
            }
        }

        public bool Update(GameTime gameTime)
        {
            Tile.Rotation +=(float)gameTime.ElapsedGameTime.TotalMilliseconds * RotationSpeed;
            if(Tile.Rotation > this.DesiredRotation)
            {
                return true;
            }
            return false;
        }
    }
}

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

        public TileRotator(Tile tile, IInformationContainer container, int layer, int tileX, int tileY, Dir sideOfTreePlayerOn)
        {
            this.Tile = tile;
            this.Container = container;
            this.TileLayer = layer;
            this.TileX = tileX;
            this.TileY = tileY;
            if (sideOfTreePlayerOn == Dir.Right)
            {
                this.DesiredRotation = TileModificationHandler.TreeFallRightRotationAmt;
                this.RotationSpeed = TileModificationHandler.TreeFallSpeed;
            }
            else if (sideOfTreePlayerOn == Dir.Left)
            {
                this.DesiredRotation = TileModificationHandler.TreeFallLeftRotationAmt;
                this.RotationSpeed = TileModificationHandler.TreeFallSpeed * -1;
            }
            else
            {
                if(Game1.Utility.RGenerator.Next(0, 2) > 0)
                {
                    this.DesiredRotation = TileModificationHandler.TreeFallRightRotationAmt;
                    this.RotationSpeed = TileModificationHandler.TreeFallSpeed;
                }
                else
                {
                    this.DesiredRotation = TileModificationHandler.TreeFallLeftRotationAmt;
                    this.RotationSpeed = TileModificationHandler.TreeFallSpeed * -1;
                }
            }
            this.Tile.Origin = new Vector2(Tile.SourceRectangle.Width / 2, Tile.SourceRectangle.Height);
            this.Tile.Position = new Vector2(this.Tile.Position.X + Tile.SourceRectangle.Width / 2, this.Tile.Position.Y + this.Tile.SourceRectangle.Height);
        }

        public bool Update(GameTime gameTime)
        {
            Tile.Rotation +=(float)gameTime.ElapsedGameTime.TotalSeconds * RotationSpeed;
            this.RotationSpeed = this.RotationSpeed * 1.017f;
            if (this.DesiredRotation < 0)
            {
                if (Tile.Rotation < this.DesiredRotation)
                {
                    return true;
                }
            }
            else
            {
                if (Tile.Rotation > this.DesiredRotation)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}

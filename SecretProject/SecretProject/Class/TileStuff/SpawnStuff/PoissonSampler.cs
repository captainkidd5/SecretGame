using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff
{
    public class PoissonSampler
    {
        private const int CellSize = 16; //standard tile size
        private const int MaxK  = 30; //Maximum number of tries for a new point.


        public int MinDistance { get; private set; } //Minimum distance points must be separated from one another
        public int MaxDistance { get; private set; } //Maximum distance a point can be from at least one other point


        public List<Point> ActiveSamples { get; set; }
        public int[,] Grid { get; set; }

        public PoissonSampler(int minDistance, int maxDistance, int[,] grid)
        {
            this.MinDistance = minDistance;
            this.MaxDistance = maxDistance;
            this.ActiveSamples = new List<Point>();
            this.Grid = grid;
        }

        public void Generate()
        {
            //generate first point randomly within grid
            ActiveSamples.Add(new Point(Game1.Utility.RGenerator.Next(0, Grid.GetLength(0) - 1),
                Game1.Utility.RGenerator.Next(0, Grid.GetLength(0) - 1)));

            while(ActiveSamples.Count > 0)
            {

            }

        }

        private void IsValid()
        {

        }
    }
}

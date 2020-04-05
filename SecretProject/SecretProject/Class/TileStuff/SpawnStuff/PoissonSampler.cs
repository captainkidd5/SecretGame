using Microsoft.Xna.Framework;
using SecretProject.Class.PathFinding;
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
        private const int MaxK  = 15; //Maximum number of tries for a new point.


        public int MinDistance { get; private set; } //Minimum distance points must be separated from one another
        public int MaxDistance { get; private set; } //Maximum distance a point can be from at least one other point


        public List<Point> ActiveSamples { get; set; }
        public ObstacleGrid Grid { get; set; }
        public Rectangle GridRectangle { get; set; }

        public PoissonSampler(int minDistance, int maxDistance, ObstacleGrid grid)
        {
            this.MinDistance = minDistance;
            this.MaxDistance = maxDistance;
            this.ActiveSamples = new List<Point>();
            this.Grid = grid; //dont forget to clear grid

        }

        public ObstacleGrid Generate()
        {
            //generate first point randomly within grid
            ActiveSamples.Add(new Point(Game1.Utility.RGenerator.Next(0, Grid.Weight.GetLength(0) - 1),
                Game1.Utility.RGenerator.Next(0, Grid.Weight.GetLength(0) - 1)));

            while(ActiveSamples.Count > 0)
            {
                int sampleIndex = Game1.Utility.RGenerator.Next(0, ActiveSamples.Count - 1); //pick random sample within activesample list
                Point sample = ActiveSamples[sampleIndex];


                bool found = false;

                for(int k =0; k < MaxK; k++) //try MaxK times to find a valid point
                {
                    
                    int newX = sample.X + MinDistance * Game1.Utility.GetMultiplier();
                    int newY = sample.Y + MinDistance * Game1.Utility.GetMultiplier();
                    Point newPoint = new Point(newX, newY);
                    if (GridContains(newPoint))
                    {
                        if(IsFarEnough(newPoint))
                        {
                            found = true;
                            ActiveSamples.Add(newPoint);
                            Grid.Weight[newPoint.X, newPoint.Y] = (int)GridStatus.Obstructed;
                            break;
                        }
                    }


                }

                if(!found)
                {
                    ActiveSamples[sampleIndex] = ActiveSamples[ActiveSamples.Count - 1];
                    ActiveSamples.RemoveAt(ActiveSamples.Count - 1);
                }
                

            }

            return Grid;

        }

        private bool GridContains(Point sample)
        {
            if(sample.X > 0 && sample.Y > 0 && sample.X < Grid.Weight.GetLength(0) && sample.Y < Grid.Weight.GetLength(1))
            {
                return true;
            }
            return false;
        }

        private bool IsFarEnough(Point sample)
        {
            int startingX = sample.X - this.MinDistance;
            int startingY = sample.Y - this.MinDistance;
            if (startingX < 0)
            {
                startingX = 0; //dont forget to check for carry over
            } 
            if (startingY < 0)
            {
                startingY = 0;
            }

            int endingIndexX = sample.X + this.MinDistance;
            int endingIndexY = sample.Y + this.MinDistance;

            if(endingIndexX >= Grid.Weight.GetLength(0))
            {
                endingIndexX = Grid.Weight.GetLength(0) - 1;
            }

            if(endingIndexY >= Grid.Weight.GetLength(1))
            {
                endingIndexY = Grid.Weight.GetLength(1) - 1;
            }

            for (int i = startingX; i < endingIndexX; i++)
            {
                for(int j = startingY; j < endingIndexY; j++)
                {
                    if(Grid.Weight[i,j] == (int)GridStatus.Obstructed)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

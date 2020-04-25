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
        private int MaxK; //Maximum number of tries for a new point.


        public int MinDistance { get; private set; } //Minimum distance points must be separated from one another
        public int MaxDistance { get; private set; } //Maximum distance a point can be from at least one other point


        public List<Point> ActiveSamples { get; set; }
        public byte[,] Grid { get; set; }
        public Rectangle GridRectangle { get; set; }

        public Rarity OddsOfAdditionalSpawn { get; set; }

        public PoissonSampler(int minDistance, int maxDistance, ObstacleGrid grid, int tries, Rarity oddsOfAdditionalSpawn)
        {
            this.MinDistance = minDistance;
            this.MaxDistance = maxDistance;
            this.ActiveSamples = new List<Point>();
            this.Grid = (byte[,])grid.Weight.Clone(); //dont forget to clear grid
            this.MaxK = tries;
            this.OddsOfAdditionalSpawn = oddsOfAdditionalSpawn;
        }

        public void Generate(int gid, Tile[,] tiles, int layer, IInformationContainer container, GenerationType generationType, Random random, bool isCrop)
        {
            //generate first point randomly within grid
            ActiveSamples.Add(new Point(Game1.Utility.RGenerator.Next(0, Grid.GetLength(0) - 1),
                Game1.Utility.RGenerator.Next(0, Grid.GetLength(0) - 1)));

            while (ActiveSamples.Count > 0)
            {
                int sampleIndex = Game1.Utility.RGenerator.Next(0, ActiveSamples.Count - 1); //pick random sample within activesample list
                Point sample = ActiveSamples[sampleIndex];


                bool found = false;
                if (random.Next(0, 101) > (int)OddsOfAdditionalSpawn)
                {
                    return;
                }

                for (int k = 0; k < MaxK; k++) //try MaxK times to find a valid point
                {

                    int newX = sample.X + MinDistance * Game1.Utility.GetMultiplier();
                    int newY = sample.Y + MinDistance * Game1.Utility.GetMultiplier();
                    Point newPoint = new Point(newX, newY);

                    if (GridContains(newPoint))
                    {
                        if (Grid[newPoint.X, newPoint.Y] == (int)GridStatus.Clear)
                        {
                            if (IsFarEnough(newPoint))
                            {
                                if (container.AllTiles[0][newPoint.X, newPoint.Y].GenerationType == generationType)
                                {
                                    found = true;
                                    ActiveSamples.Add(newPoint);
                                    Grid[newPoint.X, newPoint.Y] = (int)GridStatus.Obstructed;
                                    TileUtility.ReplaceTile(layer, newPoint.X, newPoint.Y, gid, container);
                                    if (isCrop)
                                    {
                                        TileUtility.AddCropToTile(container.AllTiles[3][newPoint.X, newPoint.Y], newPoint.X, newPoint.Y, 3, container, true);
                                    }
                                    break;
                                }

                            }
                        }
                    }




                }

                if (!found)
                {
                    this.ActiveSamples[sampleIndex] = ActiveSamples[ActiveSamples.Count - 1];
                    ActiveSamples.RemoveAt(ActiveSamples.Count - 1);
                }


            }

            return;

        }

        private bool GridContains(Point sample)
        {
            if (sample.X > 0 && sample.Y > 0 && sample.X < Grid.GetLength(0) && sample.Y < Grid.GetLength(1))
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

            if (endingIndexX >= Grid.GetLength(0))
            {
                endingIndexX = Grid.GetLength(0) - 1;
            }

            if (endingIndexY >= Grid.GetLength(1))
            {
                endingIndexY = Grid.GetLength(1) - 1;
            }

            for (int i = startingX; i < endingIndexX; i++)
            {
                for (int j = startingY; j < endingIndexY; j++)
                {
                    if (Grid[i, j] == (int)GridStatus.Obstructed)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

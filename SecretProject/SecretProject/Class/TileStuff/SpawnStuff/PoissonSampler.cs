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
        private int Tries; //Maximum number of tries for a new point.


        private int minDistance; //Minimum distance points must be separated from one another
        private int maxDistance;//Maximum distance a point can be from at least one other point


        private List<Point> activeSamples;
        public byte[,] Grid { get; set; }
        public Rectangle GridRectangle { get; set; }

        private Rarity oddsOfAdditionalSpawn;

        public PoissonSampler(int minDistance, int maxDistance, ObstacleGrid grid, int tries, Rarity oddsOfAdditionalSpawn)
        {
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
            this.activeSamples = new List<Point>();
            this.Grid = (byte[,])grid.Weight.Clone(); //dont forget to clear grid
            this.Tries = tries;
            this.oddsOfAdditionalSpawn = oddsOfAdditionalSpawn;
        }

        public void Generate(int gid, Tile[,] tiles, int layerToPlace, int layerToPlaceOn, int layerToCheckIfEmpty, IInformationContainer container, GenerationType generationType, Random random, bool isCrop)
        {
            //generate first point randomly within grid
            activeSamples.Add(new Point(random.Next(0, Grid.GetLength(0) - 1),
                random.Next(0, Grid.GetLength(0) - 1)));

            while (activeSamples.Count > 0)
            {
                int sampleIndex = random.Next(0, activeSamples.Count - 1); //pick random sample within activesample list
                Point sample = activeSamples[sampleIndex];


                bool found = false;


                for (int k = 0; k < Tries; k++) //try MaxK times to find a valid point
                {

                    int newX = sample.X + minDistance * Game1.Utility.GetMultiplier();
                    int newY = sample.Y + minDistance * Game1.Utility.GetMultiplier();
                    Point newPoint = new Point(newX, newY);

                    if (GridContains(newPoint))
                    {
                        if (Grid[newPoint.X, newPoint.Y] == (int)GridStatus.Clear)
                        {
                            if (IsFarEnough(newPoint))
                            {
                                if (container.AllTiles[layerToPlaceOn][newPoint.X, newPoint.Y].GenerationType == generationType)
                                {
                                    if(layerToCheckIfEmpty != 0)
                                    {
                                        if (container.AllTiles[layerToCheckIfEmpty][newPoint.X, newPoint.Y].GID == -1)
                                        {
                                            found = true;
                                            activeSamples.Add(newPoint);
                                            Grid[newPoint.X, newPoint.Y] = (int)GridStatus.Obstructed;
                                            TileUtility.ReplaceTile(layerToPlace, newPoint.X, newPoint.Y, gid, container);
                                            if (isCrop)
                                            {
                                                TileUtility.AddCropToTile(container.AllTiles[3][newPoint.X, newPoint.Y], newPoint.X, newPoint.Y, 3, container, true);
                                            }
                                            break;
                                        }
                                    }
                                    
                                    else
                                    {
                                        found = true;
                                        activeSamples.Add(newPoint);
                                        Grid[newPoint.X, newPoint.Y] = (int)GridStatus.Obstructed;
                                        TileUtility.ReplaceTile(layerToPlace, newPoint.X, newPoint.Y, gid, container);
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




                }

                if (!found)
                {
                    this.activeSamples[sampleIndex] = activeSamples[activeSamples.Count - 1];
                    activeSamples.RemoveAt(activeSamples.Count - 1);
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
            int startingX = sample.X - this.minDistance;
            int startingY = sample.Y - this.minDistance;
            Game1.Utility.EnsurePositive(ref startingX);
            Game1.Utility.EnsurePositive(ref startingY);

            int endingIndexX = sample.X + this.minDistance;
            int endingIndexY = sample.Y + this.minDistance;
            int max = Grid.GetLength(0) - 1;

            Game1.Utility.EnsureNoMoreThanMax(ref endingIndexX, max);
            Game1.Utility.EnsureNoMoreThanMax(ref endingIndexY, max);


            for (int i = startingX; i < endingIndexX; i++)
            {
                for (int j = startingY; j < endingIndexY; j++)
                {
                    if (Grid[i, j] == (int)GridStatus.Obstructed) //remember, this is a COPY of the original grid. Only counts objects created in this poisson sample
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

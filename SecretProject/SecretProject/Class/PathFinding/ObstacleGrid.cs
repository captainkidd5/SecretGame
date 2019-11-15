﻿using Microsoft.Xna.Framework;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.PathFinding
{
    public class ObstacleGrid
    {

        public Rectangle Size;

        public byte[,] Weight;

        //0 means no object, 1 means object


        public ObstacleGrid(int mapWidth, int mapHeight, List<Tile[,]> tiles, List<ICollidable> objects, byte defaultValue = 0)
        {
            Size = new Rectangle(0, 0, mapWidth, mapHeight);
            Weight = new byte[mapWidth, mapHeight];
            for(int i =0; i < mapWidth; i++)
            {
                for(int j =0; j < mapHeight; j++)
                {
                    Weight[i, j] = 1;
                }
            }

            for (int z = 0; z < 4; z++)
            {


                for (var i = 0; i < mapWidth; i++)
                {
                    for (var j = 0; j < mapHeight; j++)
                    {
                        string tileKey = tiles[z][i, j].GetTileKey(z);
                        if (objects.Exists(x => x.LocationKey == tileKey))
                        {
                            Weight[i, j] = 0;
                        }

                    }
                }
            }
        }

        //1 empty, 0 obstructed
        public void UpdateGrid(int indexI, int indexJ, int newValue)
        {
            Weight[indexI, indexI] = (byte)newValue;
        }

        

    }
}
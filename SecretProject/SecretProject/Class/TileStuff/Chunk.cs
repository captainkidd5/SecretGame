using Microsoft.Xna.Framework;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public class Chunk
    {
        public bool IsLoaded { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<Tile[,]> Tiles { get; set; }

        public Chunk( int x, int y)
        {
            this.IsLoaded = false;
            this.X = x;
            this.Y = y;
            Tiles = new List<Tile[,]>();
            for(int i =0; i <1; i++)
            {
                Tiles.Add(new Tile[TileUtility.ChunkX, TileUtility.ChunkX]);
            }
        }

        public Rectangle GetChunkRectangle()
        {
            return new Rectangle(this.X * TileUtility.ChunkX, this.Y * TileUtility.ChunkY, TileUtility.ChunkX * 16, TileUtility.ChunkY * 16);
        }


        public void Save()
        {
            string path = @"Content/SaveFiles/Chunks/Chunk" + this.X + this.Y + ".dat";
            FileStream fileStream = File.OpenWrite(path);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
            for(int z =0; z <1; z++)
            {
                for(int i =0; i < TileUtility.ChunkX; i++)
                {
                    for(int j =0; j< TileUtility.ChunkY; j++)
                    {
                        binaryWriter.Write(Tiles[z][i, j].GID + 1);
                        binaryWriter.Write(Tiles[z][i, j].X);
                        binaryWriter.Write(Tiles[z][i, j].Y);
                    }
                }
            }

            binaryWriter.Flush();
            binaryWriter.Close();
        }
        public void Load()
        {
            string path = @"Content/SaveFiles/Chunks/Chunk" + this.X + this.Y + ".dat";
            FileStream fileStream = File.OpenRead(path);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            for (int z = 0; z < 1; z++)
            {
                for (int i = 0; i < TileUtility.ChunkX; i++)
                {
                    for (int j = 0; j < TileUtility.ChunkY; j++)
                    {
                        int gid = binaryReader.ReadInt32();
                        int x = binaryReader.ReadInt32();
                        int y = binaryReader.ReadInt32();
                        Tiles[z][i, j] = new Tile(x, y, gid);

                    }
                }
            }
            this.IsLoaded = true;
            binaryReader.Close();

        }


    
        public void Generate()
        {
            float chanceToBeDirt = .45f;
            for (int z = 0; z < 1; z++)
            {
                for (int i = 0; i < TileUtility.ChunkX; i++)
                {
                    for (int j = 0; j < TileUtility.ChunkY; j++)
                    {

   
                            if (Game1.Utility.RFloat(0, 1) > chanceToBeDirt)
                            {
                                Tiles[z][i, j] = new Tile(this.X * TileUtility.ChunkX + i, this.Y * TileUtility.ChunkY + j, 1106);
                            }
                            else
                            {
                                Tiles[z][i, j] = new Tile(this.X * TileUtility.ChunkX + i, this.Y * TileUtility.ChunkY + j, 1116);

                            }

                        
                        
                        
                    }
                }
            }

            for (int i = 0; i < 5; i++)
            {
                Tiles[0] = TileUtility.DoSimulation(Tiles[0], 1000, 1000, TileUtility.ChunkX, TileUtility.ChunkY, this.X, this.Y, TileUtility.ChunkX);
            }

            for (int i = 0; i < TileUtility.ChunkX; i++)
            {
                for (int j = 0; j < TileUtility.ChunkY; j++)
                {
                    TileUtility.ReassignTileForTiling(Tiles, i, j, TileUtility.ChunkX, TileUtility.ChunkY);

                }
            }
        }
        
        public void Unload()
        {
            this.IsLoaded = false;
        }
    }
}

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
            for(int i =0; i < 5; i++)
            {
                Tiles.Add(new Tile[16, 16]);
            }
        }


        public void Save()
        {
            string path = @"Content/SaveFiles/Chunks/Chunk" + this.X + this.Y + ".dat";
            FileStream fileStream = File.OpenWrite(path);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
            for(int z =0; z < 5; z++)
            {
                for(int i =0; i < TileUtility.ChunkX; i++)
                {
                    for(int j =0; j< TileUtility.ChunkY; j++)
                    {
                        binaryWriter.Write(Tiles[z][i, j].GID);
                        binaryWriter.Write(Tiles[z][i, j].X);
                        binaryWriter.Write(Tiles[z][i, j].Y);
                    }
                }
            }
        }
        public void Load()
        {
            string path = @"Content/SaveFiles/Chunks/Chunk" + this.X + this.Y + ".dat";
            FileStream fileStream = File.OpenRead(path);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            for (int z = 0; z < 5; z++)
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

        }

        public void Generate()
        {
            for (int z = 0; z < 5; z++)
            {
                for (int i = 0; i < TileUtility.ChunkX; i++)
                {
                    for (int j = 0; j < TileUtility.ChunkY; j++)
                    {
                        Tiles[z][i, j] = new Tile(this.X + i * 16, this.Y + j * 16, 1107);
                    }
                }
            }
        }
        
        public void Unload()
        {
            this.IsLoaded = false;
        }
    }
}

using Microsoft.Xna.Framework;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.ObjectFolder;
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

        public Dictionary<string, List<GrassTuft>> Tufts { get; set; }
        public Dictionary<string, ObjectBody> CurrentObjects { get; set; }
        public Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        public Dictionary<string, int> TileHitPoints { get; set; }
        public Dictionary<string,Chest> Chests { get; set; }


        public Chunk(int x, int y)
        {
            this.IsLoaded = false;
            this.X = x;
            this.Y = y;
            
            Tufts = new Dictionary<string, List<GrassTuft>>();    
            CurrentObjects = new Dictionary<string, ObjectBody>();
            AnimationFrames = new Dictionary<string, EditableAnimationFrameHolder>();
            TileHitPoints = new Dictionary<string, int>();
            Chests = new Dictionary<string, Chest>();
            Tiles = new List<Tile[,]>();
            for(int i =0; i <5; i++)
            {
                Tiles.Add(new Tile[TileUtility.ChunkX, TileUtility.ChunkX]);
            }
        }

        public Rectangle GetChunkRectangle()
        {
            Rectangle RectangleToReturn = new Rectangle(this.X * TileUtility.ChunkX * TileUtility.ChunkX, this.Y * TileUtility.ChunkY * TileUtility.ChunkY, TileUtility.ChunkX * 16, TileUtility.ChunkY * 16);
            return RectangleToReturn;
        }


        public void Save()
        {
            string path = @"Content/SaveFiles/Chunks/Chunk" + this.X + this.Y + ".dat";
            FileStream fileStream = File.OpenWrite(path);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
            for(int z =0; z <5; z++)
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

            binaryWriter.Write(this.Tufts.Count);
            foreach(KeyValuePair<string, List<GrassTuft>> tufts in this.Tufts)
            {

            }

            binaryWriter.Flush();
            binaryWriter.Close();
        }
        public void Load()
        {
            string path = @"Content/SaveFiles/Chunks/Chunk" + this.X + this.Y + ".dat";
            FileStream fileStream = File.OpenRead(path);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            for (int z = 0; z <5; z++)
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


    
        public void Generate(ITileManager tileManager)
        {
            float chanceToBeDirt = .45f;
            for (int z = 0; z < 5; z++)
            {
                for (int i = 0; i < TileUtility.ChunkX; i++)
                {
                    for (int j = 0; j < TileUtility.ChunkY; j++)
                    {
                        if (z > 0)
                        {
                            Tiles[z][i, j] = new Tile(this.X * TileUtility.ChunkX + i, this.Y * TileUtility.ChunkY + j, 0);
                        }
                        else
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
            }

            for (int i = 0; i < 5; i++)
            {
                Tiles[0] = TileUtility.DoSimulation(Tiles[0], tileManager.tilesetTilesWide, tileManager.tilesetTilesHigh, TileUtility.ChunkX, TileUtility.ChunkY, this.X, this.Y, TileUtility.ChunkX);
            }

            for (int i = 0; i < TileUtility.ChunkX; i++)
            {
                for (int j = 0; j < TileUtility.ChunkY; j++)
                {
                    TileUtility.ReassignTileForTiling(Tiles, i, j, TileUtility.ChunkX, TileUtility.ChunkY);
                    if (Game1.Utility.RGenerator.Next(1, TileUtility.GrassSpawnRate) == 5)
                    {
                        if (Game1.Utility.GrassGeneratableTiles.Contains(Tiles[0][i, j].GID))
                        {

                            int numberOfGrassTuftsToSpawn = Game1.Utility.RGenerator.Next(1, 4);
                            List<GrassTuft> tufts = new List<GrassTuft>();
                            for (int g = 0; g < numberOfGrassTuftsToSpawn; g++)
                            {
                                int grassType = Game1.Utility.RGenerator.Next(1, 4);
                                tufts.Add(new GrassTuft(grassType, new Vector2(TileUtility.GetDestinationRectangle(Tiles[0][i, j]).X, TileUtility.GetDestinationRectangle(Tiles[0][i, j]).Y)));

                            }
                             this.Tufts[Tiles[0][i, j].GetTileKey(0)] = tufts;
                        }
                    }

                }
            }
            TileUtility.PlaceChests(Tiles,this.Chests, tileManager.tilesetTilesWide, tileManager.tilesetTilesHigh, TileUtility.ChunkX, TileUtility.ChunkY, tileManager.GraphicsDevice);


        }
        
        public void Unload()
        {
            this.IsLoaded = false;
        }
    }
}

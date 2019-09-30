using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using XMLData.ItemStuff;

namespace SecretProject.Class.TileStuff
{
    public class Chunk : IInformationContainer
    {
        public WorldTileManager TileManager { get; set; }
        public int Type { get; set; }
        
        public GraphicsDevice GraphicsDevice { get; set; }
        public TmxMap MapName { get; set; }
        public int TileSetDimension { get; set; }
        public int TileSetNumber { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }

        public bool IsLoaded { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<Tile[,]> AllTiles { get; set; }

        public Dictionary<string, List<GrassTuft>> Tufts { get; set; }
        public Dictionary<string, ObjectBody> Objects { get; set; }
        public Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        public Dictionary<string, int> TileHitPoints { get; set; }
        public Dictionary<string, Chest> Chests { get; set; }
        public List<LightSource> Lights { get; set; }
        public Dictionary<string, Crop> Crops { get; set; }

        public Chunk[] RelativeChunks{ get; set; }

        //GENERATION
        public TileSimulationType SimulationType { get; set; }
        public List<int> GeneratableTiles { get; set; }
        public Dictionary<int,int> TilingDictionary { get; set; }
        public int MainGid { get; set; }
        public int SecondaryGid { get; set; }
        public float MainGIDSpawnChance { get; set; }
        public Chunk(WorldTileManager tileManager, int x, int y)
           
        {
            this.TileManager = tileManager;
            
            this.Type = 1;
            this.GraphicsDevice = tileManager.GraphicsDevice;
            this.MapName = tileManager.MapName;
            this.TileSetDimension = tileManager.tilesetTilesWide;
            this.TileSetNumber = tileManager.TileSetNumber;
            this.MapWidth = TileUtility.ChunkX;
            this.MapHeight = TileUtility.ChunkY;
            this.IsLoaded = false;
            this.X = x;
            this.Y = y;

            Tufts = new Dictionary<string, List<GrassTuft>>();
            Objects = new Dictionary<string, ObjectBody>();
            AnimationFrames = new Dictionary<string, EditableAnimationFrameHolder>();
            TileHitPoints = new Dictionary<string, int>();
            Chests = new Dictionary<string, Chest>();
            AllTiles = new List<Tile[,]>();
            Lights = new List<LightSource>();
            Crops = new Dictionary<string, Crop>();
            for (int i = 0; i < 5; i++)
            {
                AllTiles.Add(new Tile[TileUtility.ChunkX, TileUtility.ChunkX]);
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
            for (int z = 0; z < 5; z++)
            {
                for (int i = 0; i < TileUtility.ChunkX; i++)
                {
                    for (int j = 0; j < TileUtility.ChunkY; j++)
                    {
                        binaryWriter.Write(AllTiles[z][i, j].GID + 1);
                        binaryWriter.Write(AllTiles[z][i, j].X);
                        binaryWriter.Write(AllTiles[z][i, j].Y);

                    }
                }
            }

            binaryWriter.Write(this.Tufts.Count);

            foreach (KeyValuePair<string, List<GrassTuft>> tufts in this.Tufts)
            {
                binaryWriter.Write(tufts.Key);
                binaryWriter.Write(tufts.Value.Count);
                for (int s = 0; s < tufts.Value.Count; s++)
                {
                    binaryWriter.Write(tufts.Value[s].GrassType);
                    binaryWriter.Write(tufts.Value[s].Position.X);
                    binaryWriter.Write(tufts.Value[s].Position.Y);
                }
            }

            binaryWriter.Write(Chests.Count);
            foreach (KeyValuePair<string, Chest> chest in this.Chests)
            {
                binaryWriter.Write(chest.Key);
                binaryWriter.Write(chest.Value.Size);
                binaryWriter.Write(chest.Value.Location.X);
                binaryWriter.Write(chest.Value.Location.Y);
                for (int s = 0; s < chest.Value.Size; s++)
                {
                    binaryWriter.Write(chest.Value.Inventory.currentInventory[s].SlotItems.Count);
                    if (chest.Value.Inventory.currentInventory[s].SlotItems.Count > 0)
                    {
                        binaryWriter.Write(chest.Value.Inventory.currentInventory[s].SlotItems[0].ID);
                    }
                    else
                    {
                        binaryWriter.Write(-1);
                    }

                }

            }

            binaryWriter.Write(Crops.Count);
            foreach (KeyValuePair<string, Crop> crop in this.Crops)
            {
                binaryWriter.Write(crop.Key);
                binaryWriter.Write(crop.Value.ItemID);
                binaryWriter.Write(crop.Value.Name);
                binaryWriter.Write(crop.Value.GID);
                binaryWriter.Write(crop.Value.DaysToGrow);
                binaryWriter.Write(crop.Value.CurrentGrowth);
                binaryWriter.Write(crop.Value.Harvestable);
                binaryWriter.Write(crop.Value.DayPlanted);

            }
            binaryWriter.Flush();
            binaryWriter.Close();
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
                        AllTiles[z][i, j] = new Tile(x, y, gid);
                        TileUtility.AssignProperties(AllTiles[z][i, j], z, i, j, this);
                    }
                }
            }

            int tuftCount = binaryReader.ReadInt32();
            for (int t = 0; t < tuftCount; t++)
            {
                string key = binaryReader.ReadString();
                List<GrassTuft> tileTufts = new List<GrassTuft>();
                int tileTuftCount = binaryReader.ReadInt32();
                for (int c = 0; c < tileTuftCount; c++)
                {
                    int grassType = binaryReader.ReadInt32();
                    float posX = binaryReader.ReadSingle();
                    float posY = binaryReader.ReadSingle();
                    tileTufts.Add(new GrassTuft(grassType, new Vector2(posX, posY)));
                }
                Tufts.Add(key, tileTufts);
            }

            this.Chests = new Dictionary<string, Chest>();
            int chestCount = binaryReader.ReadInt32();
            for (int c = 0; c < chestCount; c++)
            {
                string chestKey = binaryReader.ReadString();
                int chestSize = binaryReader.ReadInt32();
                float locationX = binaryReader.ReadSingle();
                float locationY = binaryReader.ReadSingle();
                Chest chestToAdd = new Chest(chestKey, chestSize, new Vector2(locationX, locationY), this.GraphicsDevice, false);
                for (int i = 0; i < chestSize; i++)
                {
                    int numberOfItemsInSlot = binaryReader.ReadInt32();
                    int itemID = binaryReader.ReadInt32();

                    for (int j = 0; j < numberOfItemsInSlot; j++)
                    {
                        chestToAdd.Inventory.currentInventory[i].AddItemToSlot(Game1.ItemVault.GenerateNewItem(itemID, null, false));
                    }
                }

                this.Chests.Add(chestKey, chestToAdd);
            }

            int cropCount = binaryReader.ReadInt32();
            for (int c = 0; c < cropCount; c++)
            {
                string cropKey = binaryReader.ReadString();
                int itemID = binaryReader.ReadInt32();
                string name = binaryReader.ReadString();
                int gid = binaryReader.ReadInt32();
                int daysToGrow = binaryReader.ReadInt32();
                int currentGrow = binaryReader.ReadInt32();
                bool harvestable = binaryReader.ReadBoolean();
                int dayPlanted = binaryReader.ReadInt32();
                Crop crop = new Crop()
                {
                    ItemID = itemID,
                    Name = name,
                    GID = gid,
                    DaysToGrow = daysToGrow,
                    CurrentGrowth = currentGrow,
                    Harvestable = harvestable,
                    DayPlanted = dayPlanted
                };
                this.Crops.Add(cropKey, crop);
            }
            AssignRelativeChunks();
            
            this.IsLoaded = true;
            binaryReader.Close();

            Game1.World.Boars.Add(new Boar("Boar", new Vector2(TileUtility.GetDestinationRectangle(AllTiles[0][10, 10], this.X, this.Y).X, TileUtility.GetDestinationRectangle(AllTiles[0][10, 10], this.X, this.Y).X), this.GraphicsDevice, Game1.AllTextures.EnemySpriteSheet));

        }

        public void AssignRelativeChunks()
        {
            //Down Up Left Right
            this.RelativeChunks = new Chunk[4]
            {
                this.TileManager.ActiveChunks.SingleOrDefault(x => x.X == this.X && x.Y == (this.Y + 1)),
                this.TileManager.ActiveChunks.SingleOrDefault(x => x.X == this.X && x.Y == (this.Y - 1)),
                this.TileManager.ActiveChunks.SingleOrDefault(x => x.X == this.X -1 && x.Y == (this.Y)),
                this.TileManager.ActiveChunks.SingleOrDefault(x => x.X == this.X +1 && x.Y == (this.Y))
            };
        }


        public void Generate()
        {

            this.SimulationType = (TileSimulationType)Game1.Utility.RGenerator.Next(1, 4);
            switch (SimulationType)
            {
                case TileSimulationType.dirt:
                    this.GeneratableTiles = Game1.Utility.DirtGeneratableTiles;
                    this.TilingDictionary = TileUtility.DirtTiling;
                    this.MainGid = 1106;
                    this.SecondaryGid = 1115;
                    this.MainGIDSpawnChance = .75f;
                    break;
                case TileSimulationType.sand:
                    this.GeneratableTiles = Game1.Utility.SandGeneratableTiles;
                    this.TilingDictionary = TileUtility.SandTiling;
                    this.MainGid = 1222;
                    this.SecondaryGid = 1115;
                    MainGIDSpawnChance = .85f;
                    break;
                case TileSimulationType.water:
                    this.GeneratableTiles = Game1.Utility.WaterGeneratableTiles;
                    this.TilingDictionary = TileUtility.WaterTiling;
                    this.MainGid = 427;
                    this.SecondaryGid = 1115;
                    MainGIDSpawnChance = .85f;
                    break;
            }


            for (int z = 0; z < 5; z++)
            {
                for (int i = 0; i < TileUtility.ChunkX; i++)
                {
                    for (int j = 0; j < TileUtility.ChunkY; j++)
                    {
                        if (z > 0)
                        {
                            AllTiles[z][i, j] = new Tile(i, j, 0);
                        }
                        else
                        {
                            if (Game1.Utility.RFloat(0, 1) > MainGIDSpawnChance)
                            {
                                AllTiles[z][i, j] = new Tile(this.X * TileUtility.ChunkX + i, this.Y * TileUtility.ChunkY + j, this.MainGid);
                            }
                            else
                            {
                                AllTiles[z][i, j] = new Tile(this.X * TileUtility.ChunkX + i, this.Y * TileUtility.ChunkY + j, this.SecondaryGid);

                            }
                        }

                    }
                }
            }

            for (int i = 0; i < 1; i++)
            {
                AllTiles[0] = TileUtility.DoSimulation(this,this.MainGid,this.SecondaryGid,this.GeneratableTiles,this.TilingDictionary, this.X, this.Y, TileUtility.ChunkX);
            }

            for (int i = 0; i < TileUtility.ChunkX; i++)
            {
                for (int j = 0; j < TileUtility.ChunkY; j++)
                {
                    TileUtility.ReassignTileForTiling(AllTiles, this.MainGid, this.GeneratableTiles, this.TilingDictionary, i, j, TileUtility.ChunkX, TileUtility.ChunkY);
                    if (this.SimulationType == TileSimulationType.dirt)
                    {

                        if (Game1.Utility.RGenerator.Next(1, TileUtility.GrassSpawnRate) == 5)
                        {
                            if (Game1.Utility.GrassGeneratableTiles.Contains(AllTiles[0][i, j].GID))
                            {

                                int numberOfGrassTuftsToSpawn = Game1.Utility.RGenerator.Next(1, 4);
                                List<GrassTuft> tufts = new List<GrassTuft>();
                                for (int g = 0; g < numberOfGrassTuftsToSpawn; g++)
                                {
                                    int grassType = Game1.Utility.RGenerator.Next(1, 4);
                                    tufts.Add(new GrassTuft(grassType, new Vector2(TileUtility.GetDestinationRectangle(AllTiles[0][i, j]).X + Game1.Utility.RGenerator.Next(-8, 8), TileUtility.GetDestinationRectangle(AllTiles[0][i, j]).Y + Game1.Utility.RGenerator.Next(-8, 8))));

                                }
                                this.Tufts[AllTiles[0][i, j].GetTileKey(0)] = tufts;
                            }
                        }
                    }

                }
            }
            TileUtility.PlaceChests(this, this.GeneratableTiles, this.GraphicsDevice, this.X, this.Y);

            switch(this.SimulationType)
            {
                case TileSimulationType.dirt:
                    TileUtility.GenerateTiles(1, 979, "grass", 10, 0, this);
                    TileUtility.GenerateTiles(1, 2264, "grass", 10, 0, this);
                    TileUtility.GenerateTiles(1, 1079, "dirt", 10, 0, this);
                    TileUtility.GenerateTiles(1, 1586, "dirt", 10, 0, this);
                    TileUtility.GenerateTiles(1, 1664, "dirt", 10, 0, this);
                    TileUtility.GenerateTiles(1, 1294, "dirt", 10, 0, this);
                    TileUtility.GenerateTiles(1, 1295, "dirt", 10, 0, this);
                    TileUtility.GenerateTiles(1, 1297, "dirt", 10, 0, this);
                    TileUtility.GenerateTiles(1, 1298, "dirt", 10, 0, this);

                    TileUtility.GenerateTiles(1, 1002, "dirt", 10, 0, this);
                    break;

                case TileSimulationType.sand:
                    TileUtility.GenerateTiles(1, 1286, "sand", 10, 0, this);
                    TileUtility.GenerateTiles(1, 664, "sand", 10, 0, this);
                    break;
                case TileSimulationType.water:
                    TileUtility.GenerateTiles(1, 2264, "grass", 10, 0, this);
                    TileUtility.GenerateTiles(1, 1079, "grass", 10, 0, this);
                    TileUtility.GenerateTiles(1, 1586, "grass", 10, 0, this);
                    break;
            }
            

            for (int z = 0; z < 5; z++)
            {
                for (int i = 0; i < TileUtility.ChunkX; i++)
                {
                    for (int j = 0; j < TileUtility.ChunkY; j++)
                    {
                        if (z > 0)
                        {
                            AllTiles[z][i, j].X = AllTiles[z][i, j].X + TileUtility.ChunkX * this.X;
                            AllTiles[z][i, j].Y = AllTiles[z][i, j].Y + TileUtility.ChunkY * this.Y;
                        }

                        TileUtility.AssignProperties(AllTiles[z][i, j], z, i, j, this);
                    }
                }
            }
            AssignRelativeChunks();

            this.IsLoaded = true;
        }

        public void Unload()
        {
            this.IsLoaded = false;
        }
    }
}

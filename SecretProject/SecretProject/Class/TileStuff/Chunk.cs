using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.PathFinding;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Transportation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TiledSharp;
using XMLData.ItemStuff;

namespace SecretProject.Class.TileStuff
{
    public class Chunk : IInformationContainer
    {
        public Texture2D RectangleTexture { get; set; }
        //public WorldTileManager TileManager { get; set; }
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

 
        public List<ICollidable> Objects { get; set; }
        public Dictionary<int, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        public Dictionary<int, int> TileHitPoints { get; set; }
        public Dictionary<int, IStorableItem> StoreableItems { get; set; }
        public List<LightSource> Lights { get; set; }
        public Dictionary<string, Crop> Crops { get; set; }
        public Dictionary<float, string> ForeGroundOffSetDictionary { get; set; }

        public int ArrayI { get; set; }
        public int ArrayJ { get; set; }

        //GENERATION
        public TileSimulationType SimulationType { get; set; }
        public List<int> GeneratableTiles { get; set; }
        public Dictionary<int, int> TilingDictionary { get; set; }
        public int MainGid { get; set; }
        public int SecondaryGid { get; set; }
        public float MainGIDSpawnChance { get; set; }

        //PROPERTY MANAGEMENT
        public bool Owned { get; set; }


        //NPCS
        public List<Enemy> Enemies { get; set; }

        //PATHFINDING
        public ObstacleGrid PathGrid { get; set; }

        public WorldTileManager TileManager { get; set; }
        public List<int[]> AdjacentNoise { get; set; }

        public Chunk(WorldTileManager tileManager, int x, int y, int arrayI, int arrayJ)

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
            this.ArrayI = arrayI;
            this.ArrayJ = arrayJ;
            Objects = new List<ICollidable>();
            AnimationFrames = new Dictionary<int, EditableAnimationFrameHolder>();
            TileHitPoints = new Dictionary<int, int>();
            StoreableItems = new Dictionary<int, IStorableItem>();
            AllTiles = new List<Tile[,]>();
            Lights = new List<LightSource>();
            Crops = new Dictionary<string, Crop>();
            ForeGroundOffSetDictionary = new Dictionary<float, string>();
            for (int i = 0; i < 4; i++)
            {
                AllTiles.Add(new Tile[TileUtility.ChunkX, TileUtility.ChunkX]);
            }

            Enemies = new List<Enemy>();

            SetRectangleTexture(this.GraphicsDevice);


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
            for (int z = 0; z < 4; z++)
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

            //binaryWriter.Write(Objects.Count);
            //foreach(ICollidable obj in Objects)
            //{
            //    binaryWriter.Write((int)obj.ColliderType);
            //    if(obj.ColliderType == ColliderType.grass)
            //    {
            //        binaryWriter.Write()
            //    }
            //    binaryWriter.Write(obj.Rectangle.X);
            //    binaryWriter.Write(obj.Rectangle.Y);
            //    binaryWriter.Write(obj.Rectangle.Width);
            //    binaryWriter.Write(obj.Rectangle.Height);
            //    binaryWriter.Write(obj.LocationKey);
            //}

            binaryWriter.Write(StoreableItems.Count);
            foreach (KeyValuePair<int, IStorableItem> storeableItem in this.StoreableItems)
            {
                
                binaryWriter.Write(storeableItem.Key);
                binaryWriter.Write((int)storeableItem.Value.StorableItemType);
                binaryWriter.Write(storeableItem.Value.Size);
                binaryWriter.Write(storeableItem.Value.Location.X);
                binaryWriter.Write(storeableItem.Value.Location.Y);
                for (int s = 0; s < storeableItem.Value.Size; s++)
                {
                    binaryWriter.Write(storeableItem.Value.Inventory.currentInventory[s].SlotItems.Count);
                    if (storeableItem.Value.Inventory.currentInventory[s].SlotItems.Count > 0)
                    {
                        binaryWriter.Write(storeableItem.Value.Inventory.currentInventory[s].SlotItems[0].ID);
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
            binaryWriter.Write(this.Owned);


                binaryWriter.Flush();
            binaryWriter.Close();
        }



        public void Load()
        {
            string path = @"Content/SaveFiles/Chunks/Chunk" + this.X + this.Y + ".dat";
            FileStream fileStream = File.OpenRead(path);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            for (int z = 0; z < 4; z++)
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


            this.StoreableItems = new Dictionary<int, IStorableItem>();
            int storableItemCount = binaryReader.ReadInt32();
            for (int c = 0; c < storableItemCount; c++)
            {
                int storageKey = binaryReader.ReadInt32();
                int storableItemType = binaryReader.ReadInt32();
                StorableItemType itemType = (StorableItemType)storableItemType;
                int inventorySize = binaryReader.ReadInt32();
                float locationX = binaryReader.ReadSingle();
                float locationY = binaryReader.ReadSingle();
   
                switch (itemType)
                {
                    case StorableItemType.Chest:
                        Chest storeageItemToAdd = new Chest(storageKey, inventorySize, new Vector2(locationX, locationY), this.GraphicsDevice, false);
                        for (int i = 0; i < inventorySize; i++)
                        {
                            int numberOfItemsInSlot = binaryReader.ReadInt32();
                            int itemID = binaryReader.ReadInt32();

                            for (int j = 0; j < numberOfItemsInSlot; j++)
                            {
                                storeageItemToAdd.Inventory.currentInventory[i].AddItemToSlot(Game1.ItemVault.GenerateNewItem(itemID, null, false));
                            }
                        }

                        this.StoreableItems.Add(storageKey, storeageItemToAdd);
                        break;

                    case StorableItemType.Cauldron:
                        Cauldron cauldronToAdd = new Cauldron(storageKey, inventorySize, new Vector2(locationX, locationY), this.GraphicsDevice);
                        for (int i = 0; i < inventorySize; i++)
                        {
                            int numberOfItemsInSlot = binaryReader.ReadInt32();
                            int itemID = binaryReader.ReadInt32();

                            for (int j = 0; j < numberOfItemsInSlot; j++)
                            {
                                cauldronToAdd.Inventory.currentInventory[i].AddItemToSlot(Game1.ItemVault.GenerateNewItem(itemID, null, false));
                            }
                        }

                        this.StoreableItems.Add(storageKey, cauldronToAdd);
                        break;
                }

                
                
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
            this.Owned = binaryReader.ReadBoolean();

            
            
            PathGrid = new ObstacleGrid(this.MapWidth, this.MapHeight, this.AllTiles, this.Objects);

            if (this.X != 0 && this.Y != 0)
            {
                Game1.World.Enemies.Add(new Enemy("crab", new Vector2(AllTiles[0][5, 5].DestinationRectangle.X, AllTiles[0][5, 5].DestinationRectangle.Y), this.GraphicsDevice, Game1.AllTextures.EnemySpriteSheet, this));
                Game1.World.Enemies.Add(new Enemy("boar", new Vector2(AllTiles[0][5, 5].DestinationRectangle.X, AllTiles[0][5, 5].DestinationRectangle.Y), this.GraphicsDevice, Game1.AllTextures.EnemySpriteSheet, this));
            }
            
            this.IsLoaded = true;
            binaryReader.Close();

            //Game1.World.Boars.Add(new Boar("Boar", new Vector2(TileUtility.GetDestinationRectangle(AllTiles[0][10, 10], this.X, this.Y).X, TileUtility.GetDestinationRectangle(AllTiles[0][10, 10], this.X, this.Y).X), this.GraphicsDevice, Game1.AllTextures.EnemySpriteSheet));

        }
       

        public void Generate(TileSimulationType seed)
        {
            if (seed == 0)
            {
                // this.SimulationType = (TileSimulationType)Game1.Utility.RGenerator.Next(1, 4);
                this.SimulationType = TileSimulationType.dirt;
            }
            else
            {
                this.SimulationType = seed;
            }

            float[,] noise = new float[16, 16];

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    noise[i, j] = Game1.Utility.FastNoise.GetNoise(this.X * 16 + i, this.Y * 16 + j);
                }
            }

            //get row of tiles on all sides of current chunk
            int[] topRowNoise = new int[16];
            int[] bottomRowNoise = new int[16];
            int[] leftColumnNoise = new int[16];
            int[] rightColumnNoise = new int[16];

           

            for (int i =0; i < 16; i++)
            {
                topRowNoise[i] = TileUtility.GetTileFromNoise(Game1.Utility.FastNoise.GetNoise(this.X * 16 + i, (this.Y - 1) * 16 + 15));
                bottomRowNoise[i] = TileUtility.GetTileFromNoise(Game1.Utility.FastNoise.GetNoise(this.X * 16 + i, (this.Y + 1) * 16 ));

                leftColumnNoise[i] = TileUtility.GetTileFromNoise(Game1.Utility.FastNoise.GetNoise((this.X - 1) * 16 + 15, this.Y * 16 + i));

                rightColumnNoise[i] = TileUtility.GetTileFromNoise(Game1.Utility.FastNoise.GetNoise((this.X + 1) * 16, this.Y * 16 + i));
            }

            AdjacentNoise = new List<int[]>()
            { topRowNoise,
            bottomRowNoise,
            leftColumnNoise,
            rightColumnNoise
            };

            for (int z = 0; z < 4; z++)
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
                            int newGID = TileUtility.GetTileFromNoise(noise[i, j]);

                            AllTiles[z][i, j] = new Tile(this.X * TileUtility.ChunkX + i, this.Y * TileUtility.ChunkY + j, newGID);
                            if(newGID == 1115)
                            {
                                if(Game1.Utility.RGenerator.Next(0,10) < 2)
                                {
                                    int numberOfGrassTuftsToSpawn = Game1.Utility.RGenerator.Next(1, 4);
                                    List<GrassTuft> tufts = new List<GrassTuft>();
                                    for (int g = 0; g < numberOfGrassTuftsToSpawn; g++)
                                    {
                                        int grassType = Game1.Utility.RGenerator.Next(1, 4);
                                        GrassTuft grassTuft = new GrassTuft(this.GraphicsDevice, grassType, new Vector2(TileUtility.GetDestinationRectangle(AllTiles[0][i, j]).X
                                            + Game1.Utility.RGenerator.Next(-8, 8), TileUtility.GetDestinationRectangle(AllTiles[0][i, j]).Y + Game1.Utility.RGenerator.Next(-8, 8)), this.Objects);
                                        tufts.Add(grassTuft);
                                        Objects.Add(grassTuft);
                                      

                                    }
                                }
                                
                            }
                            
                        }

                    }
                }
            }


            for (int i = 0; i < TileUtility.ChunkX; i++)
            {
                for (int j = 0; j < TileUtility.ChunkY; j++)
                {


                    switch (AllTiles[0][i, j].GID)
                    {
                        case 1114:
                            this.GeneratableTiles = Game1.Utility.DirtGeneratableTiles;
                            this.TilingDictionary = TileUtility.DirtTiling;
                            this.MainGid = 1115;
                            break;
                        case 1221:
                            this.GeneratableTiles = Game1.Utility.SandGeneratableTiles;
                            this.TilingDictionary = TileUtility.SandTiling;
                            this.MainGid = 1222;
                            break;
                        case 426:
                            this.GeneratableTiles = Game1.Utility.WaterGeneratableTiles;
                            this.TilingDictionary = TileUtility.WaterTiling;
                            this.MainGid = 427;
                            break;

                        case 929:
                            this.GeneratableTiles = Game1.Utility.StoneGeneratableTiles;
                            this.TilingDictionary = TileUtility.StoneTiling;
                            this.MainGid = 930;
                            break;


                        default:
                            this.GeneratableTiles = Game1.Utility.DirtGeneratableTiles;
                            this.TilingDictionary = TileUtility.DirtTiling;
                            this.MainGid = 1115;
                            break;

                    }
                    TileUtility.GenerationReassignForTiling(this.MainGid, this.GeneratableTiles, this.TilingDictionary, 0, i, j, TileUtility.ChunkX, TileUtility.ChunkY, this, this.AdjacentNoise);

                }
            }
           // TileUtility.PlaceChests(this, this.GeneratableTiles, this.GraphicsDevice, this.X, this.Y);

            if (this.X == 0 && this.Y == 0)
            {
                //STARTING CHUNK

                AllTiles[3][8, 5] = new Tile(8, 5, 9025);
                AllTiles[1][8, 5] = new Tile(8, 4, 9625); 

            }

            else
            {
                switch (this.SimulationType)
                {
                    case TileSimulationType.dirt:
                        TileUtility.GenerateTiles(1, 979, "grass", 10, 0, this); //STONE
                        TileUtility.GenerateTiles(1, 2264, "grass", 5, 0, this); //THUNDERBIRCH
                        TileUtility.GenerateTiles(1, 1079, "dirt", 15, 0, this); //GRASSTUFT
                        TileUtility.GenerateTiles(1, 1586, "dirt", 5, 0, this); //CLUEFRUIT
                        TileUtility.GenerateTiles(1, 1664, "grass", 5, 0, this); //OAKTREE
                        TileUtility.GenerateTiles(1, 1294, "grass", 5, 0, this); //SPROUTERA
                        TileUtility.GenerateTiles(1, 1164, "grass", 1, 0, this); //WILLOW
                        TileUtility.GenerateTiles(1, 1002, "stone", 5, 0, this); //FISSURE
                        TileUtility.GenerateTiles(3, 1476, "grass", 6, 0, this); //FallenOak
                        TileUtility.GenerateTiles(3, 1278, "stone", 5, 0, this); //Steel Vein
                        TileUtility.GenerateTiles(3, 1277, "stone", 5, 0, this); //Steel Vein
                        TileUtility.GenerateTiles(3, 1276, "stone", 5, 0, this); //Steel Vein
                        TileUtility.GenerateTiles(3, 1275, "stone", 5, 0, this); //Steel Vein
                        TileUtility.GenerateTiles(3, 1274, "stone", 5, 0, this); //Steel Vein
                        TileUtility.GenerateTiles(3, 1278, "stone", 5, 0, this); //Steel Vein
                        TileUtility.GenerateTiles(1, 1581, "dirt", 15, 0, this); //ROCK
                        TileUtility.GenerateTiles(1, 1582, "grass", 5, 0, this); //RED MUSHROOM
                        TileUtility.GenerateTiles(1, 1583, "grass", 5, 0, this); //BLUE MUSHROOM

                        TileUtility.GenerateTiles(1, 1573, "sand", 10, 0, this); //Reeds

                        // TileUtility.GenerateTiles(1, 2964, "grass", 5, 0, this); //PINE
                        TileUtility.GenerateTiles(1, 1286, "sand", 10, 0, this); //THORN
                        TileUtility.GenerateTiles(1, 664, "sand", 10, 0, this);
                        TileUtility.GenerateTiles(1, 4615, "water", 5, 0, this);
                        TileUtility.GenerateTiles(1, 4414, "water", 5, 0, this);

                        break;

                    case TileSimulationType.sand:

                        //TileUtility.GenerateTiles(1, 1164, "sand", 20, 0, this);
                        break;
                    case TileSimulationType.water:
                        //TileUtility.GenerateTiles(1, 2264, "grass", 10, 0, this);
                        //TileUtility.GenerateTiles(1, 1079, "grass", 10, 0, this);
                        //TileUtility.GenerateTiles(1, 1586, "grass", 10, 0, this);
                        //TileUtility.GenerateTiles(1, 1164, "grass", 5, 0, this);


                        break;
                }
            }
           


            for (int z = 0; z < 4; z++)
            {
                for (int i = 0; i < TileUtility.ChunkX; i++)
                {
                    for (int j = 0; j < TileUtility.ChunkY; j++)
                    {
                        //if (i > 1 && j > 1)
                        //{
                        //    if ((noise[i, j] >= .1f && noise[i, j] <= .2f) || (noise[i, j] >= .02f && noise[i, j] <= .08f))
                        //    {
                        //        TileUtility.GeneratePerlinTiles(1, i, j, 2964, Game1.Utility.GrassGeneratableTiles, 1, this, 0, 2);
                        //    }

                        //    if ((noise[i, j] >= 0f && noise[i, j] <= .1f) || (noise[i, j] >= .32f && noise[i, j] <= .36f))
                        //    {
                        //        TileUtility.GeneratePerlinTiles(1, i, j, 2264, Game1.Utility.GrassGeneratableTiles, 1, this, 0, 2);
                        //    }
                        //}

                        if (z > 0)
                        {
                            AllTiles[z][i, j].X = AllTiles[z][i, j].X + TileUtility.ChunkX * this.X;
                            AllTiles[z][i, j].Y = AllTiles[z][i, j].Y + TileUtility.ChunkY * this.Y;
                        }

                        TileUtility.AssignProperties(AllTiles[z][i, j], z, i, j, this);
                    }
                }
            }
            PathGrid = new ObstacleGrid(this.MapWidth, this.MapHeight, this.AllTiles, this.Objects);
            if(this.X != 0 && this.Y != 0)
            {
               // Enemies.Add(new Boar("boar", new Vector2(AllTiles[0][5, 5].DestinationRectangle.X, AllTiles[0][5, 5].DestinationRectangle.Y), this.GraphicsDevice, Game1.AllTextures.EnemySpriteSheet));
            }
            

            this.IsLoaded = true;
        }

        public void Unload()
        {
            this.IsLoaded = false;
        }

        //DEBUG
        private void SetRectangleTexture(GraphicsDevice graphicsDevice)
        {
            Rectangle chunkRectangle = GetChunkRectangle();
            var Colors = new List<Color>();
            for (int y = 0; y < chunkRectangle.Height; y++)
            {
                for (int x = 0; x < chunkRectangle.Width; x++)
                {
                    if (x == 0 || //left side
                        y == 0 || //top side
                        x == chunkRectangle.Width - 1 || //right side
                        y == chunkRectangle.Height - 1) //bottom side
                    {
                        Colors.Add(new Color(255, 255, 255, 255));
                    }
                    else
                    {
                        Colors.Add(new Color(0, 0, 0, 0));

                    }

                }
            }
            RectangleTexture = new Texture2D(graphicsDevice, chunkRectangle.Width, chunkRectangle.Height);
            RectangleTexture.SetData<Color>(Colors.ToArray());
        }
    }
}

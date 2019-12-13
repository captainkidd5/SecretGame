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
        public Point WorldPosition { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Point ChunkPosition { get; set; }
        public List<Tile[,]> AllTiles { get; set; }


        public Dictionary<string, List<ICollidable>> Objects { get; set; }
        public Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        public Dictionary<string, int> TileHitPoints { get; set; }
        public Dictionary<string, IStorableItemBuilding> StoreableItems { get; set; }
        public List<LightSource> Lights { get; set; }
        public Dictionary<string, Crop> Crops { get; set; }
        public Dictionary<float, string> ForeGroundOffSetDictionary { get; set; }

        public Dictionary<string, List<GrassTuft>> Tufts { get; set; }

        public int ArrayI { get; set; }
        public int ArrayJ { get; set; }

        //GENERATION
        public List<int> GeneratableTiles { get; set; }
        public Dictionary<int, int> TilingDictionary { get; set; }
        public int MainGid { get; set; }
        public int SecondaryGid { get; set; }
        public float MainGIDSpawnChance { get; set; }

        //NPCS
        public List<Enemy> Enemies { get; set; }
        public NPCGenerator NPCGenerator { get; set; }

        //PATHFINDING
        public ObstacleGrid PathGrid { get; set; }

        public WorldTileManager TileManager { get; set; }
        public List<int[,]> AdjacentNoise { get; set; }

        public Chunk(WorldTileManager tileManager, int x, int y, int arrayI, int arrayJ)

        {
            this.TileManager = tileManager;

            this.Type = 1;
            this.GraphicsDevice = tileManager.GraphicsDevice;
            this.MapName = tileManager.MapName;
            this.TileSetDimension = tileManager.tilesetTilesWide;
            this.TileSetNumber = tileManager.TileSetNumber;
            this.MapWidth = TileUtility.ChunkWidth;
            this.MapHeight = TileUtility.ChunkHeight;
            this.IsLoaded = false;
            this.X = x;
            this.Y = y;
            this.ChunkPosition = new Point(X, Y);
            this.ArrayI = arrayI;
            this.ArrayJ = arrayJ;
            Objects = new Dictionary<string, List<ICollidable>>();
            AnimationFrames = new Dictionary<string, EditableAnimationFrameHolder>();
            TileHitPoints = new Dictionary<string, int>();
            StoreableItems = new Dictionary<string, IStorableItemBuilding>();
            PathGrid = new ObstacleGrid(this.MapWidth, this.MapHeight);
            AllTiles = new List<Tile[,]>();
            Lights = new List<LightSource>();
            Crops = new Dictionary<string, Crop>();
            ForeGroundOffSetDictionary = new Dictionary<float, string>();
            Tufts = new Dictionary<string, List<GrassTuft>>();
            for (int i = 0; i < 4; i++)
            {
                AllTiles.Add(new Tile[TileUtility.ChunkWidth, TileUtility.ChunkWidth]);
            }

            Enemies = new List<Enemy>();
            NPCGenerator = new NPCGenerator(this, GraphicsDevice);

            SetRectangleTexture(this.GraphicsDevice);


        }

        public Rectangle GetChunkRectangle()
        {
            Rectangle RectangleToReturn = new Rectangle(this.X * TileUtility.ChunkWidth * TileUtility.ChunkWidth, this.Y * TileUtility.ChunkHeight * TileUtility.ChunkHeight, TileUtility.ChunkWidth * 16, TileUtility.ChunkHeight * 16);
            return RectangleToReturn;
        }


        public void Save()
        {
            string path = @"Content/SaveFiles/Chunks/Chunk" + this.X + this.Y + ".dat";
            using (FileStream fileStream = File.OpenWrite(path))
            {


                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {


                    for (int z = 0; z < 4; z++)
                    {
                        for (int i = 0; i < TileUtility.ChunkWidth; i++)
                        {
                            for (int j = 0; j < TileUtility.ChunkHeight; j++)
                            {
                                binaryWriter.Write(AllTiles[z][i, j].GID + 1);
                                binaryWriter.Write(AllTiles[z][i, j].X);
                                binaryWriter.Write(AllTiles[z][i, j].Y);

                            }
                        }
                    }


                    binaryWriter.Write(StoreableItems.Count);
                    foreach (KeyValuePair<string, IStorableItemBuilding> storeableItem in this.StoreableItems)
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

                    binaryWriter.Write(Tufts.Count);
                    foreach (KeyValuePair<string, List<GrassTuft>> tuft in this.Tufts)
                    {
                        binaryWriter.Write(tuft.Key);
                        binaryWriter.Write(tuft.Value.Count);

                        for (int i = 0; i < tuft.Value.Count; i++)
                        {
                            binaryWriter.Write(tuft.Value[i].GrassType);
                            binaryWriter.Write(tuft.Value[i].Position.X);
                            binaryWriter.Write(tuft.Value[i].Position.Y);
                        }
                    }

                    binaryWriter.Flush();
                    binaryWriter.Close();
                }
            }
        }



        public void Load()
        {
            string path = @"Content/SaveFiles/Chunks/Chunk" + this.X + this.Y + ".dat";
            using (FileStream fileStream = File.OpenRead(path))
            {


                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {


                    PathGrid = new ObstacleGrid(this.MapWidth, this.MapHeight);
                    for (int z = 0; z < 4; z++)
                    {
                        for (int i = 0; i < TileUtility.ChunkWidth; i++)
                        {
                            for (int j = 0; j < TileUtility.ChunkHeight; j++)
                            {
                                int gid = binaryReader.ReadInt32();
                                int x = binaryReader.ReadInt32();
                                int y = binaryReader.ReadInt32();
                                AllTiles[z][i, j] = new Tile(x, y, gid);
                                TileUtility.AssignProperties(AllTiles[z][i, j], z, i, j, this);
                            }
                        }
                    }


                    this.StoreableItems = new Dictionary<string, IStorableItemBuilding>();
                    int storableItemCount = binaryReader.ReadInt32();
                    for (int c = 0; c < storableItemCount; c++)
                    {
                        string storageKey = binaryReader.ReadString();
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

                    int tuftListCount = binaryReader.ReadInt32();

                    for (int i = 0; i < tuftListCount; i++)
                    {
                        string key = binaryReader.ReadString();
                        int smallListCount = binaryReader.ReadInt32();
                        List<GrassTuft> tufts = new List<GrassTuft>();
                        for (int j = 0; j < smallListCount; j++)
                        {
                            GrassTuft tuft = new GrassTuft(GraphicsDevice, binaryReader.ReadInt32(),
                                new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle()));
                            tuft.TuftsIsPartOf = tufts;
                            tufts.Add(tuft);
                        }
                        Tufts.Add(key, tufts);
                    }


                    if (this.X != 0 && this.Y != 0)
                    {
                            Tile tile = SearchForEmptyTile(3);
                            if(tile != null)
                            {
                                Game1.World.Enemies.AddRange(NPCGenerator.SpawnNpcPack(Game1.Procedural.GetTilingContainerFromGID(tile.GID).GenerationType, new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y )));
                            }
                        

                    }

                    this.IsLoaded = true;
                    binaryReader.Close();

                }
            }
        }
        /// <summary>
        /// Tries X times at random to find a tile which doesn't contain an obstacle
        /// </summary>
        /// <param name="timesToSearch">number of attempts</param>
        /// <returns></returns>
        public Tile SearchForEmptyTile(int timesToSearch)
        {
            for(int i = 0; i < timesToSearch;i++)
            {
                int randomSpawnX = Game1.Utility.RNumber(2, 14);
                int randomSpawnY = Game1.Utility.RNumber(2, 14);
                if (PathGrid.Weight[randomSpawnX, randomSpawnY] == 1)
                {
                    return AllTiles[0][randomSpawnX, randomSpawnY];
                }
            }

               return null;
            
        }

        public void Generate()
        {

            PathGrid = new ObstacleGrid(this.MapWidth, this.MapHeight);
            float[,] noise = new float[16, 16];

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    noise[i, j] = Game1.Procedural.FastNoise.GetNoise(this.X * 16 + i, this.Y * 16 + j);
                }
            }

            //get row of tiles on all sides of current chunk, for each layer
            int[,] topRowNoise = new int[4,16];
            int[,] bottomRowNoise = new int[4,16];
            int[,] leftColumnNoise = new int[4,16];
            int[,] rightColumnNoise = new int[4,16];


            for(int z =0; z < 4; z++)
            {
                for (int i = 0; i < 16; i++)
                {
                    topRowNoise[z,i] = Game1.Procedural.GetTileFromNoise(Game1.Procedural.FastNoise.GetNoise(this.X * 16 + i, (this.Y - 1) * 16 + 15), z);
                    bottomRowNoise[z,i] = Game1.Procedural.GetTileFromNoise(Game1.Procedural.FastNoise.GetNoise(this.X * 16 + i, (this.Y + 1) * 16), z);

                    leftColumnNoise[z,i] = Game1.Procedural.GetTileFromNoise(Game1.Procedural.FastNoise.GetNoise((this.X - 1) * 16 + 15, this.Y * 16 + i), z);

                    rightColumnNoise[z,i] = Game1.Procedural.GetTileFromNoise(Game1.Procedural.FastNoise.GetNoise((this.X + 1) * 16, this.Y * 16 + i), z);
                }
            }
            

            AdjacentNoise = new List<int[,]>()
            { topRowNoise,
            bottomRowNoise,
            leftColumnNoise,
            rightColumnNoise
            };

            for (int z = 0; z < 4; z++)
            {
                for (int i = 0; i < TileUtility.ChunkWidth; i++)
                {
                    for (int j = 0; j < TileUtility.ChunkHeight; j++)
                    {
                        if (z > 1)
                        {
                            AllTiles[z][i, j] = new Tile(i, j, 0);
                        }
                        else
                        {
     
                            int newGID = Game1.Procedural.GetTileFromNoise(noise[i, j], z);

                            AllTiles[z][i, j] = new Tile(this.X * TileUtility.ChunkWidth + i, this.Y * TileUtility.ChunkHeight + j, newGID);
                            if (Game1.Procedural.AllTilingContainers[0].GeneratableTiles.Contains(newGID))
                            {
                                if (Game1.Utility.RGenerator.Next(0, 10) < 2)
                                {
                                    int numberOfGrassTuftsToSpawn = Game1.Utility.RGenerator.Next(1, 4);
                                    List<GrassTuft> tufts = new List<GrassTuft>();
                                    for (int g = 0; g < numberOfGrassTuftsToSpawn; g++)
                                    {
                                        int grassType = Game1.Utility.RGenerator.Next(1, 5);
                                        GrassTuft grassTuft = new GrassTuft(this.GraphicsDevice, grassType, new Vector2(TileUtility.GetDestinationRectangle(AllTiles[0][i, j]).X
                                            + Game1.Utility.RGenerator.Next(-8, 8), TileUtility.GetDestinationRectangle(AllTiles[0][i, j]).Y + Game1.Utility.RGenerator.Next(-8, 8)));
                                        grassTuft.TuftsIsPartOf = tufts;
                                        tufts.Add(grassTuft);
                                        if (Tufts.ContainsKey(AllTiles[0][i, j].GetTileKeyStringNew(0, this)) || Objects.ContainsKey(AllTiles[0][i, j].GetTileKeyStringNew(0, this)))
                                        {

                                        }
                                        else
                                        {
                                            Tufts.Add(AllTiles[0][i, j].GetTileKeyStringNew(0, this), tufts);

                                        }



                                    }
                                }

                            }

                        }

                    }
                }
            }

            for (int z = 0; z < 2; z++)
            {


                for (int i = 0; i < TileUtility.ChunkWidth; i++)
                {
                    for (int j = 0; j < TileUtility.ChunkHeight; j++)
                    {

                        TilingContainer container = Game1.Procedural.GetTilingContainerFromGID(AllTiles[z][i, j].GID);
                        if(container != null)
                        {
                            this.GeneratableTiles = container.GeneratableTiles;
                            this.TilingDictionary = container.TilingDictionary;

                            this.MainGid = AllTiles[z][i, j].GID + 1;
                            Game1.Procedural.GenerationReassignForTiling(this.MainGid, this.GeneratableTiles, this.TilingDictionary, z, i, j, TileUtility.ChunkWidth, TileUtility.ChunkHeight, this, this.AdjacentNoise);
                        }
                        

                    }
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



                TileUtility.GenerateRandomlyDistributedTiles(2, 979, GenerationType.Grass, 50, 0, this); //tree
                TileUtility.GenerateRandomlyDistributedTiles(2, 979, GenerationType.Dirt, 50, 0, this); //tree
                TileUtility.GenerateRandomlyDistributedTiles(2, 2264, GenerationType.Grass, 5, 0, this); //THUNDERBIRCH
                TileUtility.GenerateRandomlyDistributedTiles(2, 1079, GenerationType.Dirt, 50, 0, this); //GRASSTUFT
                TileUtility.GenerateRandomlyDistributedTiles(2, 1079, GenerationType.Grass, 50, 0, this); //GRASSTUFT
                TileUtility.GenerateRandomlyDistributedTiles(2, 1586, GenerationType.Dirt, 5, 0, this); //CLUEFRUIT
                TileUtility.GenerateRandomlyDistributedTiles(2, 1664, GenerationType.Grass, 5, 0, this); //OAKTREE
                TileUtility.GenerateRandomlyDistributedTiles(2, 1294, GenerationType.Grass, 5, 0, this); //SPROUTERA
                TileUtility.GenerateRandomlyDistributedTiles(2, 1381, GenerationType.Grass, 2, 0, this); //pumpkin
                TileUtility.GenerateRandomlyDistributedTiles(2, 1164, GenerationType.Grass, 2, 0, this); //WILLOW
                TileUtility.GenerateRandomlyDistributedTiles(2, 1002, GenerationType.Stone, 5, 0, this); //FISSURE
                TileUtility.GenerateRandomlyDistributedTiles(3, 1476, GenerationType.Grass, 6, 0, this); //FallenOak
                TileUtility.GenerateRandomlyDistributedTiles(3, 1278, GenerationType.Stone, 5, 0, this); //Steel Vein
                TileUtility.GenerateRandomlyDistributedTiles(3, 1277, GenerationType.Stone, 5, 0, this); //Steel Vein
                TileUtility.GenerateRandomlyDistributedTiles(3, 1276, GenerationType.Stone, 5, 0, this); //Steel Vein
                TileUtility.GenerateRandomlyDistributedTiles(3, 1275, GenerationType.Stone, 5, 0, this); //Steel Vein
                TileUtility.GenerateRandomlyDistributedTiles(3, 1274, GenerationType.Stone, 5, 0, this); //Steel Vein
                TileUtility.GenerateRandomlyDistributedTiles(3, 1278, GenerationType.Stone, 5, 0, this); //Steel Vein
                TileUtility.GenerateRandomlyDistributedTiles(2, 1581, GenerationType.Dirt, 15, 0, this); //ROCK
                TileUtility.GenerateRandomlyDistributedTiles(2, 1581, GenerationType.Dirt, 15, 0, this); //ROCK
                TileUtility.GenerateRandomlyDistributedTiles(2, 1580, GenerationType.Dirt, 15, 0, this); //stick
                TileUtility.GenerateRandomlyDistributedTiles(2, 1580, GenerationType.Dirt, 15, 0, this); //stick
                TileUtility.GenerateRandomlyDistributedTiles(2, 1582, GenerationType.Grass, 5, 0, this); //RED MUSHROOM
                TileUtility.GenerateRandomlyDistributedTiles(2, 1583, GenerationType.Grass, 5, 0, this); //BLUE MUSHROOM

                //SANDRUINS
                TileUtility.GenerateRandomlyDistributedTiles(3, 1853, GenerationType.SandRuin, 5, 0, this); //Chest
                TileUtility.GenerateRandomlyDistributedTiles(3, 2548, GenerationType.SandRuin, 5, 0, this); //ancient pillar (tall)
                TileUtility.GenerateRandomlyDistributedTiles(3, 2549, GenerationType.SandRuin, 5, 0, this); //ancient pillar (short)


                TileUtility.GenerateRandomlyDistributedTiles(2, 1573, GenerationType.Sand, 10, 0, this); //Reeds

                // TileUtility.GenerateTiles(1, 2964, Game1.Utility.GrassGeneratableTiles,, 5, 0, this); //PINE
                TileUtility.GenerateRandomlyDistributedTiles(2, 1286, GenerationType.Sand, 10, 0, this); //THORN
                TileUtility.GenerateRandomlyDistributedTiles(2, 664, GenerationType.Sand, 10, 0, this);
                // TileUtility.GenerateTiles(1, 4615, "water", 5, 0, this);
                //TileUtility.GenerateTiles(1, 4414, "water", 5, 0, this);
                TileUtility.GenerateRandomlyDistributedTiles(2, 2964, GenerationType.Grass, 25, 0, this); //oak2
                TileUtility.GenerateRandomlyDistributedTiles(2, 3664, GenerationType.Grass, 25, 0, this); //oak3
                TileUtility.GenerateRandomlyDistributedTiles(2, 2964, GenerationType.Dirt, 25, 0, this); //oak2
                TileUtility.GenerateRandomlyDistributedTiles(2, 3664, GenerationType.Dirt, 25, 0, this); //oak3


            }



            for (int z = 0; z < 4; z++)
            {
                for (int i = 0; i < TileUtility.ChunkWidth; i++)
                {
                    for (int j = 0; j < TileUtility.ChunkHeight; j++)
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

                        if (z > 1)
                        {
                            AllTiles[z][i, j].X = AllTiles[z][i, j].X + TileUtility.ChunkWidth * this.X;
                            AllTiles[z][i, j].Y = AllTiles[z][i, j].Y + TileUtility.ChunkHeight * this.Y;
                        }

                        TileUtility.AssignProperties(AllTiles[z][i, j], z, i, j, this);
                    }
                }
            }

            if (this.X != 0 && this.Y != 0)
            {
                Tile tile = SearchForEmptyTile(3);
                if (tile != null)
                {
                    Game1.World.Enemies.AddRange(NPCGenerator.SpawnNpcPack(Game1.Procedural.GetTilingContainerFromGID(tile.GID).GenerationType, new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y)));
                }


            }


            this.IsLoaded = true;
            Save();
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

        #region STATIC METHODS
        public static bool CheckIfChunkExistsInMemory(int idX, int idY)
        {
            if (File.Exists(@"Content/SaveFiles/Chunks/Chunk" + idX + idY + ".dat"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}

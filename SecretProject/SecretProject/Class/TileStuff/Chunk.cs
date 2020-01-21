using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.PathFinding;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff.SpawnStuff;
using System;
using System.Collections.Generic;
using System.IO;
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
        public bool AreReadersAndWritersDone { get; set; }
        public Point WorldPosition { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Point ChunkPosition { get; set; }
        public List<Tile[,]> AllTiles { get; set; }


        public Dictionary<string, List<ICollidable>> Objects { get; set; }
        public Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        public Dictionary<string, int> TileHitPoints { get; set; }
        public Dictionary<string, IStorableItemBuilding> StoreableItems { get; set; }
        public List<LightSource> NightTimeLights { get; set; }
        public List<LightSource> DayTimeLights { get; set; }
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

        public Random Random { get; set; }

        //NPCS
        public List<Enemy> Enemies { get; set; }
        public NPCGenerator NPCGenerator { get; set; }

        //PATHFINDING
        public ObstacleGrid PathGrid { get; set; }

        public WorldTileManager TileManager { get; set; }
        public List<int[,]> AdjacentNoise { get; set; }

        public readonly Object Locker;

        public List<ObstacleGrid> AdjacentObstacleGrids { get; set; }

        public bool IsSaving { get; set; }
        public bool IsLoading { get; set; }
        public bool IsGenerating { get; set; }
        public bool IsDoneLoading { get; set; }


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
            this.ChunkPosition = new Point(this.X, this.Y);
            this.ArrayI = arrayI;
            this.ArrayJ = arrayJ;
            this.Objects = new Dictionary<string, List<ICollidable>>();
            this.AnimationFrames = new Dictionary<string, EditableAnimationFrameHolder>();
            this.TileHitPoints = new Dictionary<string, int>();
            this.StoreableItems = new Dictionary<string, IStorableItemBuilding>();
            this.PathGrid = new ObstacleGrid(this.MapWidth, this.MapHeight);
            this.AllTiles = new List<Tile[,]>();
            this.NightTimeLights = new List<LightSource>();
            this.DayTimeLights = new List<LightSource>();
            this.Crops = new Dictionary<string, Crop>();
            this.ForeGroundOffSetDictionary = new Dictionary<float, string>();
            this.Tufts = new Dictionary<string, List<GrassTuft>>();
            for (int i = 0; i < 4; i++)
            {
                this.AllTiles.Add(new Tile[TileUtility.ChunkWidth, TileUtility.ChunkWidth]);
            }

            this.Enemies = new List<Enemy>();
            this.NPCGenerator = new NPCGenerator(this, this.GraphicsDevice);

            SetRectangleTexture(this.GraphicsDevice);
            this.AdjacentObstacleGrids = new List<ObstacleGrid>();
            this.Random = new Random(Game1.Utility.RGenerator.Next(0, 1000));
            this.Locker = new object();
        }

        public Rectangle GetChunkRectangle()
        {
            Rectangle RectangleToReturn = new Rectangle(this.X * TileUtility.ChunkWidth * TileUtility.ChunkWidth, this.Y * TileUtility.ChunkHeight * TileUtility.ChunkHeight, TileUtility.ChunkWidth * 16, TileUtility.ChunkHeight * 16);
            return RectangleToReturn;
        }


        public void Save()
        {
            if (!IsSaving)
            {


                lock (Locker)
                {


                    this.IsSaving = true;
                    this.AreReadersAndWritersDone = false;
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
                                        binaryWriter.Write(this.AllTiles[z][i, j].GID + 1);
                                        binaryWriter.Write(this.AllTiles[z][i, j].X);
                                        binaryWriter.Write(this.AllTiles[z][i, j].Y);

                                    }
                                }
                            }


                            binaryWriter.Write(this.StoreableItems.Count);
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

                            binaryWriter.Write(this.Crops.Count);
                            foreach (KeyValuePair<string, Crop> crop in this.Crops)
                            {
                                binaryWriter.Write(crop.Key);
                                binaryWriter.Write(crop.Value.ItemID);
                                binaryWriter.Write(crop.Value.Name);
                                binaryWriter.Write(crop.Value.GID);
                                binaryWriter.Write(crop.Value.BaseGID);
                                binaryWriter.Write(crop.Value.DaysToGrow);
                                binaryWriter.Write(crop.Value.CurrentGrowth);
                                binaryWriter.Write(crop.Value.Harvestable);
                                binaryWriter.Write(crop.Value.DayPlanted);

                            }

                            binaryWriter.Write(this.Tufts.Count);
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
                    this.AreReadersAndWritersDone = true;
                    this.IsSaving = false;
                }
            }
        }



        public void Load()
        {
            if (!IsDoneLoading)
            {


                lock (Locker)
                {
                    IsDoneLoading = true;

                    this.IsLoading = true;
                    this.AreReadersAndWritersDone = false;
                    string path = @"Content/SaveFiles/Chunks/Chunk" + this.X + this.Y + ".dat";
                    using (FileStream fileStream = File.OpenRead(path))
                    {


                        using (BinaryReader binaryReader = new BinaryReader(fileStream))
                        {


                            this.PathGrid = new ObstacleGrid(this.MapWidth, this.MapHeight);
                            for (int z = 0; z < 4; z++)
                            {
                                for (int i = 0; i < TileUtility.ChunkWidth; i++)
                                {
                                    for (int j = 0; j < TileUtility.ChunkHeight; j++)
                                    {
                                        int gid = binaryReader.ReadInt32();
                                        int x = binaryReader.ReadInt32();
                                        int y = binaryReader.ReadInt32();
                                        this.AllTiles[z][i, j] = new Tile(x, y, gid);
                                        TileUtility.AssignProperties(this.AllTiles[z][i, j], z, i, j, this);
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
                                int baseGID = binaryReader.ReadInt32();
                                int daysToGrow = binaryReader.ReadInt32();
                                int currentGrow = binaryReader.ReadInt32();
                                bool harvestable = binaryReader.ReadBoolean();
                                int dayPlanted = binaryReader.ReadInt32();
                                int newCurrentGrowth = Game1.GlobalClock.TotalDays - dayPlanted;
                                if(newCurrentGrowth > daysToGrow)
                                {
                                    newCurrentGrowth = daysToGrow;
                                }
                                Crop crop = new Crop()
                                {
                                    ItemID = itemID,
                                    Name = name,
                                    
                                    BaseGID = baseGID,
                                    DaysToGrow = daysToGrow,

                                    Harvestable = harvestable,
                                    DayPlanted = dayPlanted,
                                    CurrentGrowth = newCurrentGrowth,
                                    GID = this.MapName.Tilesets[this.TileSetNumber].Tiles[baseGID].AnimationFrames[newCurrentGrowth].Id + 1,
                            };
                                this.Crops.Add(cropKey, crop);
                                TileUtility.ReplaceTile(3, crop.X, crop.Y, crop.GID, this);
                            }

                            int tuftListCount = binaryReader.ReadInt32();

                            for (int i = 0; i < tuftListCount; i++)
                            {
                                string key = binaryReader.ReadString();
                                int smallListCount = binaryReader.ReadInt32();
                                List<GrassTuft> tufts = new List<GrassTuft>();
                                for (int j = 0; j < smallListCount; j++)
                                {
                                    GrassTuft tuft = new GrassTuft(this.GraphicsDevice, binaryReader.ReadInt32(),
                                        new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle()));
                                    tuft.TuftsIsPartOf = tufts;
                                    tufts.Add(tuft);
                                }
                                this.Tufts.Add(key, tufts);
                            }


                            if (this.X != 0 && this.Y != 0)
                            {
                                if (Game1.OverWorld.Enemies.Count < 10)
                                {


                                    Tile tile = SearchForEmptyTile(3);
                                    if (tile != null)
                                    {
                                        TilingContainer tilingContainer = Game1.Procedural.GetTilingContainerFromGID(tile.GenerationType);
                                        if (tilingContainer != null)
                                        {
                                            Game1.OverWorld.Enemies.AddRange(this.NPCGenerator.SpawnNpcPack(tilingContainer.GenerationType, new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y)));
                                        }

                                    }
                                }


                            }

                            this.IsLoaded = true;
                            binaryReader.Close();

                        }
                    }

                    this.IsLoading = false;
                    this.AreReadersAndWritersDone = true;
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
            for (int i = 0; i < timesToSearch; i++)
            {
                int randomSpawnX = Game1.Utility.RNumber(2, 14);
                int randomSpawnY = Game1.Utility.RNumber(2, 14);
                if (this.PathGrid.Weight[randomSpawnX, randomSpawnY] == 1)
                {
                    return this.AllTiles[0][randomSpawnX, randomSpawnY];
                }
            }

            return null;
        }



        public void Generate()
        {
            if (!IsDoneLoading)
            {


                lock (Locker)
                {

                    IsDoneLoading = true;
                    this.IsGenerating = true;
                    this.PathGrid = new ObstacleGrid(this.MapWidth, this.MapHeight);
                    float[,] noise = new float[16, 16];

                    for (int i = 0; i < 16; i++)
                    {
                        for (int j = 0; j < 16; j++)
                        {
                            noise[i, j] = Game1.Procedural.FastNoise.GetNoise(this.X * 16 + i, this.Y * 16 + j);
                        }
                    }

                    #region AdjacentNoise

                    //Four chunks, four layers, 16 rows, 16 columns, phew!
                    int[,,] chunkAboveNoise = new int[4, 16, 16];
                    int[,,] ChunkBelowNoise = new int[4, 16, 16];
                    int[,,] ChunkLeftNoise = new int[4, 16, 16];
                    int[,,] ChunkRightNoise = new int[4, 16, 16];

                    for (int z = 0; z < 4; z++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            for (int j = 0; j < 16; j++)
                            {
                                chunkAboveNoise[z, i, j] = Game1.Procedural.GetTileFromNoise(Game1.Procedural.FastNoise.GetNoise(this.X * 16 + i, (this.Y - 1) * 16 + j), z);
                                ChunkBelowNoise[z, i, j] = Game1.Procedural.GetTileFromNoise(Game1.Procedural.FastNoise.GetNoise(this.X * 16 + i, (this.Y + 1) * 16 + j), z);
                                ChunkLeftNoise[z, i, j] = Game1.Procedural.GetTileFromNoise(Game1.Procedural.FastNoise.GetNoise((this.X - 1) * 16 + i, this.Y * 16 + j), z);
                                ChunkRightNoise[z, i, j] = Game1.Procedural.GetTileFromNoise(Game1.Procedural.FastNoise.GetNoise((this.X + 1) * 16 + i, this.Y * 16 + j), z);
                            }
                        }
                    }

                    List<int[,,]> AllAdjacentChunkNoise = new List<int[,,]>()
            {
                chunkAboveNoise,
                ChunkBelowNoise,
                ChunkLeftNoise,
                ChunkRightNoise
            };

                    #endregion


                    for (int z = 0; z < 4; z++)
                    {
                        for (int i = 0; i < TileUtility.ChunkWidth; i++)
                        {
                            for (int j = 0; j < TileUtility.ChunkHeight; j++)
                            {


                                int newGID = Game1.Procedural.GetTileFromNoise(noise[i, j], z);
                                this.AllTiles[z][i, j] = new Tile(i, j, newGID);



                            }
                        }
                    }

                    for (int z = 0; z < 4; z++) //This loop needs to happen separately from the previous one because all tiles need to be set first.
                    {
                        for (int i = 0; i < TileUtility.ChunkWidth; i++)
                        {
                            for (int j = 0; j < TileUtility.ChunkHeight; j++)
                            {
                                if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                {
                                    if (MapName.Tilesets[TileSetNumber].Tiles[this.AllTiles[z][i, j].GID].Properties.ContainsKey("generate"))
                                    {
                                        this.AllTiles[z][i, j].GenerationType = (GenerationType)Enum.Parse(typeof(GenerationType), MapName.Tilesets[TileSetNumber].Tiles[this.AllTiles[z][i, j].GID].Properties["generate"]);
                                        //grass = 1, stone = 2, wood = 3, sand = 4
                                    }

                                    TilingContainer container = Game1.Procedural.GetTilingContainerFromGID(this.AllTiles[z][i, j].GenerationType);
                                    if (container != null)
                                    {


                                        this.MainGid = this.AllTiles[z][i, j].GID + 1;
                                        Game1.Procedural.GenerationReassignForTiling(this.MainGid, container.GeneratableTiles, container.TilingDictionary, z, i, j, TileUtility.ChunkWidth, TileUtility.ChunkHeight, this, AllAdjacentChunkNoise);
                                    }
                                }


                            }
                        }
                    }

                    if (this.X == 0 && this.Y == 0)
                    {
                        //STARTING CHUNK

                        this.AllTiles[3][2, 2] = new Tile(2, 2, 979);
                        //  this.AllTiles[1][8, 5] = new Tile(8, 4, 9625);
                        int columnCounter = 100;
                        for (int sx = 0; sx < 5; sx++)
                        {
                            for (int sy = 0; sy < 5; sy++)
                            {
                                int whirlPoolID = 7946 + sy * columnCounter + sx;
                                this.AllTiles[2][4 + sx, 5 + sy] = new Tile(4 + sx, 5 + sy, whirlPoolID);
                            }

                        }

                    }
                    else
                    {
                        HandleCliffEdgeCases(AllAdjacentChunkNoise);
                        GenerateLandscape();
                    }

                    List<int> CliffBottomTiles = new List<int>()
        {
            3033, 3034, 3035, 2937, 3036, 2936
        };



                    for (int z = 0; z < 4; z++)
                    {
                        for (int i = 0; i < TileUtility.ChunkWidth; i++)
                        {
                            for (int j = 0; j < TileUtility.ChunkHeight; j++)
                            {
                                if (z == 2)
                                {
                                    if (CliffBottomTiles.Contains(this.AllTiles[3][i, j].GID))
                                    {

                                        int counter = 1;

                                        for (int c = j; c < j + 5; c++)
                                        {
                                            if (j + counter < 16)
                                            {


                                                this.AllTiles[2][i, j + counter].GID = this.AllTiles[3][i, j].GID + 1 + 100 * counter;

                                                this.AllTiles[3][i, j + counter].GID = 0;
                                                if (c < j + 4)
                                                {
                                                    this.AllTiles[0][i, j + counter].GID = 0;
                                                }

                                                counter++;
                                            }
                                        }

                                    }
                                }

                                this.AllTiles[z][i, j].X = this.AllTiles[z][i, j].X + TileUtility.ChunkWidth * this.X;
                                this.AllTiles[z][i, j].Y = this.AllTiles[z][i, j].Y + TileUtility.ChunkHeight * this.Y;

                                TileUtility.AssignProperties(this.AllTiles[z][i, j], z, i, j, this);
                                if (z == 3)
                                {
                                    AddGrassTufts(this.AllTiles[z][i, j], this.AllTiles[1][i, j]);
                                }

                            }
                        }
                    }

                    if (this.X != 0 && this.Y != 0)
                    {
                        if (Game1.OverWorld.Enemies.Count < 10)
                        {
                            Tile tile = SearchForEmptyTile(3);
                            if (tile != null)
                            {
                                TilingContainer container = Game1.Procedural.GetTilingContainerFromGID(tile.GenerationType);
                                if (container != null)
                                {
                                    Game1.OverWorld.Enemies.AddRange(this.NPCGenerator.SpawnNpcPack(container.GenerationType, new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y)));
                                }


                            }
                        }


                    }
                    this.IsLoaded = true;
                    this.IsGenerating = false;
                    Save();
                }
            }
        }

        public void Unload()
        {
            this.IsLoaded = false;
        }

        public void GenerateLandscape()
        {
            //Specify GID + 1
            for (int s = 0; s < Game1.OverWorldSpawnHolder.OverWorldSpawnElements.Count; s++)
            {
                OverworldSpawnElement element = Game1.OverWorldSpawnHolder.OverWorldSpawnElements[s];

                if (element.Unlocked)
                {
                    TileUtility.GenerateRandomlyDistributedTiles((int)element.MapLayerToPlace, element.GID, element.GenerationType, element.Frequency,
                        (int)element.MapLayerToCheckIfEmpty, this, element.ZeroLayerOnly, element.AssertLeftAndRight, element.Limit);
                }
            };



        }

        public void HandleCliffEdgeCases(List<int[,,]> AllAdjacentChunkNoise)
        {
            //these gids are all +1
            for (int i = 0; i < 16; i++)
            {
                for (int j = 15; j > 10; j--)
                {
                    if (AllAdjacentChunkNoise[0][3, i, j] == 2935)
                    {
                        int newJIndex = j;

                        int newCliffGID = 3035;
                        for (int remainAbove = newJIndex; remainAbove < 15; remainAbove++)
                        {
                            newCliffGID += 100;
                        }

                        for (int newY = 0; newCliffGID != 3535; newY++)
                        {

                            newCliffGID += 100;
                            this.AllTiles[2][i, newY].GID = newCliffGID;
                            if (newCliffGID != 3535)
                            {
                                this.AllTiles[0][i, newY].GID = 0;

                                this.AllTiles[1][i, newY].GID = 0;

                            }


                        }
                        break;


                    }
                }
            }
        }

        public void AddGrassTufts(Tile tile, Tile zeroTile)
        {
            if (tile.GID != -1)
            {

                if (zeroTile.GenerationType == GenerationType.Grass)
                {
                    if (this.Random.Next(0, 10) < 2)
                    {
                        if ((this.Tufts.ContainsKey(tile.TileKey)))
                        {
                        }
                        else
                        {

                            int numberOfGrassTuftsToSpawn = this.Random.Next(1, 4);
                            List<GrassTuft> tufts = new List<GrassTuft>();
                            for (int g = 0; g < numberOfGrassTuftsToSpawn; g++)
                            {
                                int grassType = this.Random.Next(1, 5);
                                GrassTuft grassTuft = new GrassTuft(this.GraphicsDevice, grassType, new Vector2(TileUtility.GetDestinationRectangle(tile).X
                                    + this.Random.Next(-8, 8), TileUtility.GetDestinationRectangle(tile).Y + this.Random.Next(-8, 8)));
                                grassTuft.TuftsIsPartOf = tufts;
                                tufts.Add(grassTuft);


                            }
                            this.Tufts.Add(tile.TileKey, tufts);

                        }
                    }
                }
            }
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
            this.RectangleTexture = new Texture2D(graphicsDevice, chunkRectangle.Width, chunkRectangle.Height);
            this.RectangleTexture.SetData<Color>(Colors.ToArray());
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

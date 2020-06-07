using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using XMLData.ItemStuff;

namespace SecretProject.Class.StageFolder.DungeonStuff
{
    public class DungeonRoom
    {
        public Dungeon Dungeon { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public TileManager TileManager { get; private set; }



        public DungeonRoom(Dungeon dungeon, int x, int y, ContentManager content)
        {
            this.Dungeon = dungeon;
            this.X = x;
            this.Y = y;
            this.TileManager = new TileManager(Dungeon.TileSet, Dungeon.Map, Dungeon.Graphics, content, (int)Dungeon.TileSetNumber, Dungeon);
        }

        public void Generate(string path)
        {
            
        }

        public void Save(string path)
        {
            //string path = this.ChunkPath + this.X + this.Y + ".dat";
            using (FileStream fileStream = File.OpenWrite(path))
            {


                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                }
            }
        }

        public void Load(string path)
        {
            using (FileStream fileStream = File.OpenRead(path))
            {


                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    this.TileManager.Load(binaryReader);
                   


                    Dungeon.AllTiles.StoreableItems = new Dictionary<string, IStorableItemBuilding>();
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
                                Chest storeageItemToAdd = new Chest(storageKey, inventorySize, new Vector2(locationX, locationY), Dungeon.Graphics, false);
                                for (int i = 0; i < inventorySize; i++)
                                {
                                    int numberOfItemsInSlot = binaryReader.ReadInt32();
                                    int itemID = binaryReader.ReadInt32();

                                    for (int j = 0; j < numberOfItemsInSlot; j++)
                                    {
                                        storeageItemToAdd.Inventory.currentInventory[i].AddItemToSlot(Game1.ItemVault.GenerateNewItem(itemID, null, false));
                                    }
                                }

                                Dungeon.AllTiles.StoreableItems.Add(storageKey, storeageItemToAdd);
                                break;

                            case StorableItemType.Cauldron:
                                Cauldron cauldronToAdd = new Cauldron(storageKey, inventorySize, new Vector2(locationX, locationY), Dungeon.Graphics);
                                for (int i = 0; i < inventorySize; i++)
                                {
                                    int numberOfItemsInSlot = binaryReader.ReadInt32();
                                    int itemID = binaryReader.ReadInt32();

                                    for (int j = 0; j < numberOfItemsInSlot; j++)
                                    {
                                        cauldronToAdd.Inventory.currentInventory[i].AddItemToSlot(Game1.ItemVault.GenerateNewItem(itemID, null, false));
                                    }
                                }

                                Dungeon.AllTiles.StoreableItems.Add(storageKey, cauldronToAdd);
                                break;
                        }
                    }
                    int itemCount = binaryReader.ReadInt32();
                    for (int item = 0; item < itemCount; item++)
                    {
                        Game1.ItemVault.GenerateNewItem(binaryReader.ReadInt32(), new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle()), true, Dungeon.AllTiles.AllItems);
                    }
                    int cropCount = binaryReader.ReadInt32();
                    for (int c = 0; c < cropCount; c++)
                    {
                        string cropKey = binaryReader.ReadString();
                        int cropX = binaryReader.ReadInt32();
                        int cropY = binaryReader.ReadInt32();
                        int itemID = binaryReader.ReadInt32();
                        string name = binaryReader.ReadString();
                        int gid = binaryReader.ReadInt32();
                        int baseGID = binaryReader.ReadInt32();
                        int daysToGrow = binaryReader.ReadInt32();
                        int currentGrow = binaryReader.ReadInt32();
                        bool harvestable = binaryReader.ReadBoolean();
                        int dayPlanted = binaryReader.ReadInt32();
                        //int newCurrentGrowth = Game1.GlobalClock.TotalDays - dayPlanted;
                        //if (newCurrentGrowth > daysToGrow)
                        //{
                        //    newCurrentGrowth = daysToGrow;
                        //}
                        if (currentGrow >= TileManager.TileSetDictionary[baseGID].AnimationFrames.Count)
                        {
                            currentGrow = TileManager.TileSetDictionary[baseGID].AnimationFrames.Count - 1;
                        }
                        Crop crop = new Crop()
                        {
                            ItemID = itemID,
                            Name = name,
                            X = cropX,
                            Y = cropY,
                            BaseGID = baseGID,
                            DaysToGrow = daysToGrow,

                            Harvestable = harvestable,
                            DayPlanted = dayPlanted,
                            CurrentGrowth = currentGrow,

                            GID = this.TileManager.TileSetDictionary[baseGID].AnimationFrames[currentGrow].Id + 1,
                        };
                        if (!Dungeon.AllTiles.Crops.ContainsKey(cropKey))
                        {
                            Dungeon.AllTiles.Crops.Add(cropKey, crop);
                        }

                        TileUtility.ReplaceTile(3, crop.X, crop.Y, crop.GID, (IInformationContainer)Dungeon.AllTiles);
                    }

                    int tuftListCount = binaryReader.ReadInt32();

                    for (int i = 0; i < tuftListCount; i++)
                    {
                        string key = binaryReader.ReadString();
                        int smallListCount = binaryReader.ReadInt32();
                        List<GrassTuft> tufts = new List<GrassTuft>();
                        for (int j = 0; j < smallListCount; j++)
                        {
                            GrassTuft tuft = new GrassTuft(Dungeon.Graphics, binaryReader.ReadInt32(),
                                new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle()));
                            tuft.TuftsIsPartOf = tufts;
                            tufts.Add(tuft);
                        }
                        Dungeon.AllTiles.Tufts.Add(key, tufts);
                    }

                    //if (this.X != 0 && this.Y != 0)
                    //{
                    //    if (Game1.AllowNaturalNPCSpawning)
                    //    {


                    //        if (world.Enemies.Count < Game1.NPCSpawnCountLimit)
                    //        {


                    //            Tile tile = SearchForEmptyTile(3);
                    //            if (tile != null)
                    //            {
                    //                TilingContainer tilingContainer = Game1.Procedural.GetTilingContainerFromGID(tile.GenerationType);
                    //                if (tilingContainer != null)
                    //                {

                    //                    world.Enemies.AddRange(this.NPCGenerator.SpawnNpcPack(tilingContainer.GenerationType, new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y)));
                    //                }

                    //            }
                    //        }
                    //    }


                    //}

                    binaryReader.Close();

                }
            }
        }
    }
}

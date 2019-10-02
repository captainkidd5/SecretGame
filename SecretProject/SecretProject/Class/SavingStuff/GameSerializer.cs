using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretProject.Class.Playable;
using Microsoft.Xna.Framework;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff;

using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;

namespace SecretProject.Class.SavingStuff
{
    public static class GameSerializer
    {
        /// <summary>
        /// Write: Uses a binary writer to generate bytes representative of properties we pass into the binary writer. 
        /// Read: Uses the binary writer to read the bytes back in the same order they were written. Reads them in a specific way
        /// depending on the datatype we ask it to read. Must be a basic datatype as far as I know.
        /// </summary>
        /// <param name="outPutMessage"></param>
        /// <param name="thingToAppend"></param>
        /// 
        //Can use this to check what values we are passing in.
        public static void AppendOutputMessage(string outPutMessage, string thingToAppend)
        {
            outPutMessage = outPutMessage + " " + thingToAppend;
        }

        //order really really matters
        public static void WritePlayer(Player player, BinaryWriter writer, string OutputMessage, float version)
        {
            writer.Write(Game1.Player.Position.X);
            AppendOutputMessage(OutputMessage, Game1.Player.Position.X.ToString());
            writer.Write(Game1.Player.Position.Y);
            AppendOutputMessage(OutputMessage, Game1.Player.Position.Y.ToString());

            writer.Write(Game1.Player.Name);
            AppendOutputMessage(OutputMessage, Game1.Player.Name);

          //  WriteInventory(Game1.Player.Inventory, writer, OutputMessage, version);

        }

        public static void ReadPlayer(Player player, BinaryReader reader, float version)
        {
            player.Position = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            player.Name = reader.ReadString();
           // player.Inventory = ReadInventory(player.Inventory, reader, version);
            
        }

        //public static void WriteInventory(Inventory inventory, BinaryWriter writer, string OutputMessage, float version)
        //{
        //    writer.Write(inventory.Capacity);
        //    writer.Write(inventory.Money);

        //    for(int i=0; i< inventory.Capacity; i++)
        //    {
        //        //use this to keep track of the number of items each inventory slot has.
        //        writer.Write(inventory.currentInventory[i].SlotItems.Count);
        //        if (inventory.currentInventory[i].SlotItems.Count > 0)
        //        {
        //            for (int j = 0; j < inventory.currentInventory[i].SlotItems.Count; j++)
        //            {
        //                WriteInventoryItem(inventory.currentInventory[i].SlotItems[0], writer, version);
        //            }
        //        }   
        //    } 
        //}

        //public static Inventory ReadInventory(Inventory inventory, BinaryReader reader, float version)
        //{
        //    //read capacity
        //    int capacity = reader.ReadInt32();
        //    int money = reader.ReadInt32();

        //    int[] slotItemsCounters = new int[capacity];
        //    Inventory newInventory = new Inventory(capacity);
        //    newInventory.Money = money;

        //    for(int i =0; i< capacity; i++)
        //    {
        //        int slotItemCounter = reader.ReadInt32();
        //        slotItemsCounters[i] = slotItemCounter;
        //        for(int j=0; j < slotItemsCounters[i]; j++)
        //        {
        //           // newInventory.currentInventory[i].AddItemToSlot(ReadInventoryItem(reader, version));
        //        }     
        //    }

            
        //    return newInventory;
        //}

        //public static void WriteInventoryItem(Item item, BinaryWriter writer, float version)
        //{
        //    writer.Write(item.Name);
        //    writer.Write(item.ID);
        //    writer.Write(item.Count);
        //    writer.Write(item.InvMaximum);
        //    writer.Write(item.WorldMaximum);
        //    writer.Write(item.Ignored);
        //    writer.Write(item.IsDropped);
        //    writer.Write(item.IsPlaceable);
        //    //writer.Write(item.id);
        //    writer.Write(item.Price);
        //}

       // public static Item ReadInventoryItem( BinaryReader reader, float version)
       // {
            //Item item = new Item();
            //item.Name = reader.ReadString();
            //item.ID = reader.ReadInt32();
            //item.Count = reader.ReadInt32();
            //item.InvMaximum = reader.ReadInt32();
            //item.WorldMaximum = reader.ReadInt32();
            //item.Ignored = reader.ReadBoolean();
            //item.IsDropped = reader.ReadBoolean();
            //item.IsPlaceable = reader.ReadBoolean();
            //item.Price = reader.ReadInt32();

            //return Game1.ItemVault.GenerateNewItem(item.ID, null, false);
       // }











        //just do items for now
        //public static void WriteWorld(World location, BinaryWriter writer, float version)
        //{
        //    writer.Write(location.WorldSize);
        //    writer.Write(location.AllTiles.AllTiles.Count);


        //    for (int z = 0; z < location.AllTiles.AllTiles.Count; z++)
        //    {
        //        for (int i = 0; i < location.AllTiles.MapWidth; i++)
        //        {
        //            for (int j = 0; j < location.AllTiles.mapHeight; j++)
        //            {
        //                WriteTile(location.AllTiles.AllTiles[z][i, j], writer, version);

        //            }
        //        }
        //    }
        //}

        //public static void ReadWorld(World location, GraphicsDevice graphics, BinaryReader reader, float version)
        //{
        //    //location.LoadPreliminaryContent();
        //    int worldSize = reader.ReadInt32();
        //    int allTilesCount = reader.ReadInt32();
        //    int tileSetTilesWide = reader.ReadInt32();
        //    int tileSetTilesHigh = reader.ReadInt32();

        //    for (int z = 0; z < location.AllTiles.AllTiles.Count; z++)
        //    {
        //        for (int i = 0; i < location.AllTiles.mapWidth; i++)
        //        {
        //            for (int j = 0; j < location.AllTiles.mapHeight; j++)
        //            {
        //               // location.AllTiles.AllTiles[z][i, j] = ReadTile(reader, graphics, version,location.AllTiles.tilesetTilesWide, location.AllTiles.tilesetTilesHigh);
        //            }
        //        }
        //    }

        //    location.AllTiles.LoadInitialTileObjects(location);

        //}
        public static void WriteTile(Tile tile, BinaryWriter writer, float version)
        {
            writer.Write(tile.GID + 1);
           
            writer.Write(tile.Y);
            writer.Write(tile.X);

            writer.Write(tile.LayerToDrawAt);
            writer.Write(tile.LayerToDrawAtZOffSet);


        }

        //public static Tile ReadTile(BinaryReader reader, GraphicsDevice graphics, float version, int tileSetTilesWide, int tileSetTilesHigh)
        //{
        //    Tile newTile;
        //    int gid = reader.ReadInt32();
        //    //float X = reader.ReadSingle();
        //   // float Y = reader.ReadSingle();
        //    float layer = reader.ReadSingle();
        //    float layerOffSet = reader.ReadSingle();
            
            

        //    //newTile = new Tile(X, Y, gid) { LayerToDrawAt = layer, LayerToDrawAtZOffSet = layerOffSet };


        //    return newTile;
            
        //}

        public static void WriteClock(Clock clock, BinaryWriter writer, float version)
        {
            writer.Write(clock.GlobalTime);
            writer.Write(clock.TotalDays);
        }

        public static void ReadClock(Clock clock, BinaryReader reader, float version)
        {
            int globalTime = reader.ReadInt32();
            int totalDays = reader.ReadInt32();

            clock.GlobalTime = globalTime;
            clock.TotalDays = totalDays;

        }


    }
}

﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretProject.Class.Playable;
using Microsoft.Xna.Framework;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.StageFolder;

namespace SecretProject.Class.Universal
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

            WriteInventory(Game1.Player.Inventory, writer, OutputMessage, version);

        }

        public static void ReadPlayer(Player player, BinaryReader reader, float version)
        {
            player.Position = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            player.Name = reader.ReadString();
            player.Inventory = ReadInventory(player.Inventory, reader, version);
            
        }

        public static void WriteInventory(Inventory inventory, BinaryWriter writer, string OutputMessage, float version)
        {
            writer.Write(inventory.Capacity);
            writer.Write(inventory.Money);

            for(int i=0; i< inventory.Capacity; i++)
            {
                //use this to keep track of the number of items each inventory slot has.
                writer.Write(inventory.currentInventory[i].SlotItems.Count);
                if (inventory.currentInventory[i].SlotItems.Count > 0)
                {
                    for (int j = 0; j < inventory.currentInventory[i].SlotItems.Count; j++)
                    {
                        WriteInventoryItem(inventory.currentInventory[i].SlotItems[0], writer, version);
                    }
                }   
            } 
        }

        public static Inventory ReadInventory(Inventory inventory, BinaryReader reader, float version)
        {
            //read capacity
            int capacity = reader.ReadInt32();
            int money = reader.ReadInt32();

            int[] slotItemsCounters = new int[capacity];
            Inventory newInventory = new Inventory(capacity);
            newInventory.Money = money;

            for(int i =0; i< capacity; i++)
            {
                int slotItemCounter = reader.ReadInt32();
                slotItemsCounters[i] = slotItemCounter;
                for(int j=0; j < slotItemsCounters[i]; j++)
                {
                    newInventory.currentInventory[i].AddItemToSlot(ReadInventoryItem(reader, version));
                }     
            }

            
            return newInventory;
        }

        public static void WriteInventoryItem(Item item, BinaryWriter writer, float version)
        {
            writer.Write(item.Name);
            writer.Write(item.ID);
            writer.Write(item.Count);
            writer.Write(item.InvMaximum);
            writer.Write(item.WorldMaximum);
            writer.Write(item.Ignored);
            writer.Write(item.IsDropped);
            writer.Write(item.IsPlaceable);
            //writer.Write(item.id);
            writer.Write(item.TextureString);
            writer.Write(item.Price);
        }

        public static Item ReadInventoryItem( BinaryReader reader, float version)
        {
            Item item = new Item();
            item.Name = reader.ReadString();
            item.ID = reader.ReadInt32();
            item.Count = reader.ReadInt32();
            item.InvMaximum = reader.ReadInt32();
            item.WorldMaximum = reader.ReadInt32();
            item.Ignored = reader.ReadBoolean();
            item.IsDropped = reader.ReadBoolean();
            item.IsPlaceable = reader.ReadBoolean();
            item.TextureString = reader.ReadString();
            item.Price = reader.ReadInt32();

            return Game1.ItemVault.GenerateNewItem(item.ID, null, false);
        }

        public static void WriteWorldItem(Item item, BinaryWriter writer, float version)
        {
            writer.Write(item.Name);
            writer.Write(item.ID);
            writer.Write(item.Count);
            writer.Write(item.InvMaximum);
            writer.Write(item.WorldMaximum);
            writer.Write(item.Ignored);
            writer.Write(item.IsDropped);
            writer.Write(item.IsPlaceable);
            //writer.Write(item.id);
            writer.Write(item.TextureString);
            writer.Write(item.Price);
            writer.Write(item.WorldPosition.X);
            writer.Write(item.WorldPosition.Y);
        }

        public static Item ReadWorldItem(BinaryReader reader, float version)
        {
            Item item = new Item();
            item.Name = reader.ReadString();
            item.ID = reader.ReadInt32();
            item.Count = reader.ReadInt32();
            item.InvMaximum = reader.ReadInt32();
            item.WorldMaximum = reader.ReadInt32();
            item.Ignored = reader.ReadBoolean();
            item.IsDropped = reader.ReadBoolean();
            item.IsPlaceable = reader.ReadBoolean();
            item.TextureString = reader.ReadString();
            item.Price = reader.ReadInt32();
            float itemPositionX = reader.ReadSingle();
            float itemPositionY = reader.ReadSingle();
            item.WorldPosition = new Vector2(itemPositionX, itemPositionY);

            return Game1.ItemVault.GenerateNewItem(item.ID, item.WorldPosition, true);
        }

        //just do items for now
        public static void WriteStage(Home home, BinaryWriter writer, float version)
        {
            writer.Write(home.AllItems.Count);
            for(int i=0; i< home.AllItems.Count; i++)
            {
                WriteWorldItem(home.AllItems[i], writer, version);
                //writer.Write(home.AllItems[i].WorldPosition.X);
                //writer.Write(home.AllItems[i].WorldPosition.Y);

            }
        }

        public static void ReadStage(Home home, BinaryReader reader, float version)
        {
            List<Item> AllItems = new List<Item>();
            int allItemsCount = reader.ReadInt32();
            for(int i=0; i < allItemsCount; i++)
            {
                AllItems.Add(ReadWorldItem(reader, version));
            }

            home.AllItems = AllItems;
        }

        public static void WriteClock(Clock clock, BinaryWriter writer, float version)
        {
            writer.Write(clock.GlobalTime);
            writer.Write(clock.TotalHours);
            writer.Write(clock.TotalDays);
        }

        public static void ReadClock(Clock clock, BinaryReader reader, float version)
        {
            int globalTime = reader.ReadInt32();
            int totalHours = reader.ReadInt32();
            int totalDays = reader.ReadInt32();

            clock.GlobalTime = globalTime;
            clock.TotalHours = totalHours;
            clock.TotalDays = totalDays;

        }






    }
}

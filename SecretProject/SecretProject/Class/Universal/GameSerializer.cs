using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretProject.Class.Playable;
using Microsoft.Xna.Framework;
using SecretProject.Class.ItemStuff;

namespace SecretProject.Class.Universal
{
    public static class GameSerializer
    {
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
                writer.Write(inventory.currentInventory[i].SlotItems.Count);
                if (inventory.currentInventory[i].SlotItems.Count > 0)
                {
                    for (int j = 0; j < inventory.currentInventory[i].SlotItems.Count; j++)
                    {
                        WriteItem(inventory.currentInventory[i].SlotItems[0], writer, version);
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
                    newInventory.currentInventory[i].AddItemToSlot(ReadItem(reader, version));
                }     
            }

            
            return newInventory;
        }

        public static void WriteItem(Item item, BinaryWriter writer, float version)
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

        public static Item ReadItem( BinaryReader reader, float version)
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

            return item;
        }




    }
}

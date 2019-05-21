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


        //order really really matters
        public static void SavePlayer(Player player, BinaryWriter writer, float version)
        {
            writer.Write(Game1.Player.Position.X);
            writer.Write(Game1.Player.Position.Y);
            writer.Write(Game1.Player.Name);
            WriteInventory(Game1.Player.Inventory, writer, version);
            

            
        }

        public static void LoadPlayer(Player player, BinaryReader reader, float version)
        {
            player.Position = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            player.Name = reader.ReadString();
            
        }

        public static void WriteInventory(Inventory inventory, BinaryWriter writer, float version)
        {
            writer.Write(inventory.Capacity);

            for(int i=0; i< inventory.Capacity; i++)
            {
                if (inventory.currentInventory[i].SlotItems.Count > 0)
                {
                    for (int j = 0; j < inventory.currentInventory[i].SlotItems.Count; j++)
                    {
                        WriteItem(inventory.currentInventory[i].SlotItems[0], writer, version);
                    }
                }   
            } 
        }

        public static void LoadInventory(Inventory inventory, BinaryReader reader, float version)
        {
            //read capacity
            int capacity = reader.ReadInt32();
            Inventory newInventory = new Inventory(capacity);
            for(int i =0; i< capacity; i++)
            {
                //need to figure out how to find each slot's capacity
                for(int j=0; j < 5; j++)
                {
                    newInventory.currentInventory[i].AddItemToSlot(LoadItem(reader, version));
                }
                newInventory.currentInventory
                 
            }


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
            writer.Write(item.id);
            writer.Write(item.TextureString);
            writer.Write(item.Price);
        }

        public static Item LoadItem( BinaryReader reader, float version)
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

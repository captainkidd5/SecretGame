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
using SecretProject.Class.ObjectFolder;
using Microsoft.Xna.Framework.Graphics;

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
            item.Price = reader.ReadInt32();
            float itemPositionX = reader.ReadSingle();
            float itemPositionY = reader.ReadSingle();
            item.WorldPosition = new Vector2(itemPositionX, itemPositionY);

            return Game1.ItemVault.GenerateNewItem(item.ID, item.WorldPosition, true);
        }

        public static void WriteObjectBody(ObjectBody obj, BinaryWriter writer, float version)
        {
            writer.Write(obj.ShowRectangle);
            writer.Write(obj.Rectangle.X);
            writer.Write(obj.Rectangle.Y);
            writer.Write(obj.Rectangle.Width);
            writer.Write(obj.Rectangle.Height);
            writer.Write(obj.Speed);
            writer.Write(obj.Identifier);
        }

        public static ObjectBody ReadObjectBody(BinaryReader reader, float version)
        {
            ObjectBody newObjectBody = new ObjectBody();
            return newObjectBody;
        }






        //just do items for now
        public static void WriteStage(IStage home, BinaryWriter writer, float version)
        {
            writer.Write(home.AllItems.Count);
            for (int i = 0; i < home.AllItems.Count; i++)
            {
                WriteWorldItem(home.AllItems[i], writer, version);
                //writer.Write(home.AllItems[i].WorldPosition.X);
                //writer.Write(home.AllItems[i].WorldPosition.Y);

            }
            ///works up until here for sure
            ///
            //writer.Write(home.allti)
            writer.Write(home.AllTiles.AllTiles.Count);
            writer.Write(home.AllTiles.tilesetTilesWide);
            writer.Write(home.AllTiles.tilesetTilesHigh);

            for (int z = 0; z < home.AllTiles.AllTiles.Count; z++)
            {
                for (int i = 0; i < home.AllTiles.tilesetTilesWide; i++)
                {
                    for (int j = 0; j < home.AllTiles.tilesetTilesHigh; j++)
                    {
                        WriteTile(home.AllTiles.AllTiles[z][i, j], writer, version);

                    }
                }
            }
        }

        public static void ReadStage(IStage home, GraphicsDevice graphics, BinaryReader reader, float version)
        {
            List<Item> AllItems = new List<Item>();
            int allItemsCount = reader.ReadInt32();
            for (int i = 0; i < allItemsCount; i++)
            {
                AllItems.Add(ReadWorldItem(reader, version));
            }

            home.AllItems = AllItems;

            int allTilesCount = reader.ReadInt32();
            int tileSetTilesWide = reader.ReadInt32();
            int tileSetTilesHigh = reader.ReadInt32();

            List<ObjectBody> newObjects = new List<ObjectBody>();

            for (int z = 0; z < home.AllTiles.AllTiles.Count; z++)
            {
                for (int i = 0; i < home.AllTiles.tilesetTilesWide; i++)
                {
                    for (int j = 0; j < home.AllTiles.tilesetTilesHigh; j++)
                    {
                        home.AllTiles.AllTiles[z][i, j] = ReadTile(reader, graphics, version);
                        //if(home.AllTiles.AllTiles[z][i, j].HasObject)
                        //{
                        //    newObjects.Add(home.AllTiles.AllTiles[z][i, j].TileObject);
                        //}
                    }
                }
            }

            home.AllObjects = newObjects;
            home.AllTiles.LoadInitialTileObjects();



            ///works up until here for sure
        }
        public static void WriteTile(Tile tile, BinaryWriter writer, float version)
        {
            writer.Write(tile.X);
            writer.Write(tile.Y);
            writer.Write(tile.GID + 1);
            writer.Write(tile.IsSelected);
            writer.Write(tile.TilesetTilesWide);
            writer.Write(tile.TilesetTilesHigh);
            writer.Write(tile.MapWidth);
            writer.Write(tile.MapHeight);
            writer.Write(tile.TileFrame);
            writer.Write(tile.TileHeight);
            writer.Write(tile.TileWidth);
            writer.Write(tile.Column);
            writer.Write(tile.Row);
            writer.Write(tile.TileNumber);
            writer.Write(tile.OldY);
            writer.Write(tile.OldY1);
            writer.Write(tile.OldX);
            writer.Write(tile.IsAnimated);
            writer.Write(tile.IsAnimating);
            writer.Write(tile.IsFinishedAnimating);
            writer.Write(tile.KillAnimation);
            writer.Write(tile.DelayTimer);
            writer.Write(tile.Plantable);
            writer.Write(tile.AssociatedItem);
            writer.Write(tile.Timer);
            writer.Write(tile.CurrentFrame);
            writer.Write(tile.TotalFramesX);
            writer.Write(tile.AddAmountX);
            writer.Write(tile.Speed);
            writer.Write(tile.Probability);
            writer.Write(tile.HasSound);
            //skipping color 
            writer.Write(tile.ColorMultiplier);
            writer.Write(tile.IsTemporary);
            writer.Write(tile.IsPortal);
            writer.Write(tile.portalDestination);
            writer.Write(tile.Diggable);

            if (tile.HasObject)
            {
                writer.Write(tile.HasObject);

                writer.Write(tile.TileObject.Rectangle.X);
                writer.Write(tile.TileObject.Rectangle.Y);
                writer.Write(tile.TileObject.Rectangle.Width);
                writer.Write(tile.TileObject.Rectangle.Height);

            }
            else
            {
                writer.Write(false);
            }
        }

        public static Tile ReadTile(BinaryReader reader, GraphicsDevice graphics, float version)
        {
            Tile newTile;

            float X = reader.ReadSingle();
            float Y = reader.ReadSingle();
            int gid = reader.ReadInt32();
            bool isSelected = reader.ReadBoolean();
            int tileSetTilesWide = reader.ReadInt32();
            int tileSetTilesHigh = reader.ReadInt32();
            int mapWidth = reader.ReadInt32();
            int mapHeight = reader.ReadInt32();
            int tileFrame = reader.ReadInt32();
            int tileHeight = reader.ReadInt32();
            int tileWidth = reader.ReadInt32();
            int column = reader.ReadInt32();
            int row = reader.ReadInt32();
            int tileNumber = reader.ReadInt32();
            float oldY = reader.ReadSingle();
            float oldY1 = reader.ReadSingle();
            float oldX = reader.ReadSingle();
            bool isAnimated = reader.ReadBoolean();
            bool isAnimating = reader.ReadBoolean();
            bool isFinishedAnimating = reader.ReadBoolean();
            bool killAnimation = reader.ReadBoolean();
            float delayTimer = reader.ReadSingle();
            bool plantable = reader.ReadBoolean();
            int associatedItem = reader.ReadInt32();
            double timer = reader.ReadDouble();
            int currentFrame = reader.ReadInt32();
            int totalFrames = reader.ReadInt32();
            int addAmount = reader.ReadInt32();
            double speed = reader.ReadDouble();
            int probability = reader.ReadInt32();
            bool hasSound = reader.ReadBoolean();
            float colorMultiplier = reader.ReadSingle();
            bool isTemporary = reader.ReadBoolean();
            bool isPortal = reader.ReadBoolean();
            string portalDestination = reader.ReadString();
            bool dirt = reader.ReadBoolean();
            bool grass = reader.ReadBoolean();
            bool stone = reader.ReadBoolean();
            bool diggable = reader.ReadBoolean();
            bool redRuneStone = reader.ReadBoolean();
            bool blueRuneStone = reader.ReadBoolean();
            bool hasObject = reader.ReadBoolean();

            newTile = new Tile(oldX, oldY, gid, tileSetTilesWide, tileSetTilesHigh, mapWidth, mapHeight);

            if(hasObject)
            {
                int rectangleX = reader.ReadInt32();
                int rectangleY = reader.ReadInt32();
                int rectangleWidth = reader.ReadInt32();
                int rectangleHeight = reader.ReadInt32();
                Rectangle tileRectangle = new Rectangle(rectangleX, rectangleY, rectangleWidth, rectangleHeight);
                ObjectBody body = new ObjectBody(graphics, tileRectangle, rectangleX);
                newTile.TileObject = body;
            }
            
            newTile.IsSelected = isSelected;
            newTile.TileFrame = tileFrame;
            newTile.TileHeight = tileHeight;
            newTile.TileWidth = tileWidth;
            newTile.TileNumber = tileNumber;
            newTile.IsAnimated = isAnimated;
            newTile.IsAnimating = isAnimating;
            newTile.IsFinishedAnimating = isFinishedAnimating;
            newTile.KillAnimation = killAnimation;
            newTile.DelayTimer = delayTimer;
            newTile.Plantable = plantable;
            newTile.AssociatedItem = associatedItem;
            newTile.Timer = timer;
            newTile.CurrentFrame = currentFrame;
            newTile.TotalFramesX = totalFrames;
            newTile.AddAmountX = addAmount;
            newTile.Speed = speed;
            newTile.Probability = probability;
            newTile.HasSound = hasSound;
            newTile.ColorMultiplier = colorMultiplier;
            newTile.IsTemporary = isTemporary;
            newTile.IsPortal = isPortal;
            newTile.portalDestination = portalDestination;
            newTile.Diggable = diggable;

            return newTile;
            
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

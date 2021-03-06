using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Playable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace SecretProject.Class.StageFolder.DungeonStuff.Desert
{
    public class DesertDungeon : Dungeon
    {
        public DesertDungeon(StageManager stageManager, string name, LocationType locationType, GraphicsDevice graphicsDevice, ContentManager content, Texture2D tileSet, TmxMap tmxMap,
              IServiceProvider service, PlayerManager playerManager, CharacterManager characterManager, bool isBasedOnPreloadedMap = true) : base(
                   stageManager, name, locationType, graphicsDevice, content, tileSet, tmxMap,
               service, playerManager, characterManager, isBasedOnPreloadedMap = true)
        { 
            this.Rooms = new ForestRoom[MaxDungeonRooms, MaxDungeonRooms];
        }

        protected override void InitializeRooms()
        {
            for (int i = 0; i < MaxDungeonRooms; i++)
            {
                for (int j = 0; j < MaxDungeonRooms; j++)
                {
                    Rooms[i, j] = new ForestRoom(this, i, j);
                }
            }
        }

        protected override void CreateFirstRoom()
        {
            DungeonRoom startingRoom = new DungeonRoom(this.AllTiles, this, 99, 0, this.Content); //starting room is top right
            Rooms[99, 0] = startingRoom;

            string startingRoomSavePath = this.RoomDirectory + "/" + startingRoom.X + "," + startingRoom.Y + ".dat";
            if (File.Exists(startingRoomSavePath))
            {

                startingRoom.Load(startingRoomSavePath);
            }
            else
            {
                startingRoom.Save(startingRoomSavePath);
            }
            this.CurrentRoom = startingRoom;

            this.CurrentRoom.DungeonPortals.Add(new DungeonPortal(DungeonGraph.GetNode(99, 1), 0, -64, Dir.Down));
            this.CurrentRoom.DungeonPortals[0].InteractionRectangle = new Rectangle(512, 1006, 80, 16);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace SecretProject.Class.StageFolder.DungeonStuff
{
    public class ForestDungeon : Dungeon
    {
        public ForestDungeon(string name, LocationType locationType, StageType stageType, GraphicsDevice graphics, ContentManager content, Texture2D tileSet, TmxMap tmxMap, int dialogueToRetrieve, int backDropNumber) : base(name, locationType, stageType, graphics, content, tileSet, tmxMap, dialogueToRetrieve, backDropNumber)
        {

            this.Rooms = new ForestRoom[MaxDungeonRooms, MaxDungeonRooms];
            InitializeRooms();
            this.Content = content;
            this.DungeonGraph = new DungeonGraph(this, 100);
            this.NPCGenerator = new NPCGenerator((IInformationContainer)this.AllTiles, graphics);
            this.AllPortals.Clear();
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
            DungeonRoom startingRoom = Rooms[99, 0];

            string startingRoomSavePath = this.RoomDirectory + "/" + startingRoom.X + "," + startingRoom.Y + ".dat";

            this.CurrentRoom = startingRoom;

            //this.CurrentRoom.DungeonPortals.Add(new DungeonPortal(DungeonGraph.GetNode(99, 1), 0, -64, Dir.Down));
            //this.CurrentRoom.DungeonPortals[0].InteractionRectangle = new Rectangle(512, 1006, 80, 16);
            GenerateRoomSavePath(startingRoom,startingRoom.X,startingRoom.Y);
        }



        protected override void AddFirstRoomPortal()
        {
            Portal portal = new Portal((int)this.StageIdentifier, (int)Game1.HomeStead.StageIdentifier, 0, 32, false);
            this.AllPortals.Add(portal);

            if (!Game1.PortalGraph.HasEdge((Stages)portal.From, (Stages)portal.To))
            {
                Game1.PortalGraph.AddEdge((Stages)portal.From, (Stages)portal.To);
            }
        }

    }
}

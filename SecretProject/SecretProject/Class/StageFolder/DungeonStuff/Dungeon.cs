using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Playable;
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
    public class Dungeon : TmxStageBase
    {
        public static int MaxDungeonRooms = 100;

        private readonly int startingRoomX = 99;
        private readonly int startingRoomY = 0;

        public DungeonRoom CurrentRoom { get; private set; }
        public DungeonRoom[,] Rooms { get;  set; }
        public ContentManager Content { get; private set; }

        public string RoomDirectory { get; set; }

        public DungeonGraph DungeonGraph { get; private set; }
        public NPCGenerator NPCGenerator{ get; private set; }

        public Dungeon(string name, LocationType locationType, StageType stageType, GraphicsDevice graphics, ContentManager content, Texture2D tileSet, TmxMap tmxMap, int dialogueToRetrieve, int backDropNumber) : base(name, locationType, stageType, graphics, content, tileSet, tmxMap, dialogueToRetrieve, backDropNumber)
        {
            this.Rooms = new DungeonRoom[MaxDungeonRooms, MaxDungeonRooms];
            InitializeRooms();
            this.Content = content;
            this.DungeonGraph = new DungeonGraph(this, 100);
            this.NPCGenerator = new NPCGenerator((IInformationContainer)this.AllTiles, graphics);
        }

        private bool IsStartingRoom()
        {
            return (this.CurrentRoom.X == startingRoomX && this.CurrentRoom.Y == startingRoomY);
        }

        private void InitializeRooms()
        {

            for(int i =0; i < MaxDungeonRooms; i++)
            {
                for(int j =0; j < MaxDungeonRooms; j++)
                {
                    Rooms[i, j] = new DungeonRoom(this, i, j);
                }
            }
        }

        public override void AssignPath(string startPath)
        {
            this.SavePath = startPath + "/Dungeons/" + this.StageName;
            Directory.CreateDirectory(this.SavePath);
            this.SavePath = this.SavePath + "/" + this.StageName + "data";
            if (File.Exists(this.SavePath))
            {
                
            }
            else
            {
                File.WriteAllText(this.SavePath, string.Empty);
            }
            this.RoomDirectory = startPath + "/Dungeons/" + this.StageName + "/Rooms";
            if (Directory.Exists(this.RoomDirectory))
            {

            }
            else
            {
                Directory.CreateDirectory(this.RoomDirectory);
            }
        }

        public override void TryLoadExistingStage()
        {
            if (new FileInfo(this.SavePath).Length > 0)
            {
                FileStream fileStream = File.OpenRead(this.SavePath);
                BinaryReader binaryReader = new BinaryReader(fileStream);

                Load(binaryReader);

                binaryReader.Close();
            }
            else
            {
                AllTiles.StartNew();
                this.DungeonGraph.GenerateLayout();
                CreateFirstRoom();
                this.DungeonGraph.GeneratePortalConnections();
            }
        }

        private void CreateFirstRoom()
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
            
            this.CurrentRoom.DungeonPortals.Add(new DungeonPortal( DungeonGraph.GetNode(99, 1),0, -64,Dir.Down));
            this.CurrentRoom.DungeonPortals[0].InteractionRectangle = new Rectangle(512, 1006, 80,16);
        }

        public void SwitchRooms(int x, int y, Dir directionToGoTo)
        {
            this.CurrentRoom.Save(this.RoomDirectory + "/" + this.CurrentRoom.X + "," + CurrentRoom.Y + ".dat");
            DungeonRoom room;
            ResetDebugString();
            room = this.Rooms[x, y];
            Game1.Player.UserInterface.LoadingScreen.BeginBlackTransition();
            this.Enemies = new List<NPCStuff.Enemies.Enemy>();
            GenerateRoomSavePath(room);
            this.Rooms[x, y] = room;
            this.CurrentRoom = room;
            this.AllTiles = this.CurrentRoom.TileManager;

            int newPlayerX = 0;
            int newPlayerY = 0;
            if (CurrentRoom.X == startingRoomX && CurrentRoom.Y == startingRoomY)
            {
                newPlayerX = 545;
                newPlayerY = 900;
                
            }
            else
            {
                switch (directionToGoTo) //player should spawn on the opposite side of the room where the next room's opening is
                {
                    case Dir.Down:
                        newPlayerX = this.CurrentRoom.DungeonPortals.Find(param => param.DirectionToSpawn == Dir.Up).InteractionRectangle.X;
                        newPlayerY = this.CurrentRoom.DungeonPortals.Find(param => param.DirectionToSpawn == Dir.Up).InteractionRectangle.Y + 32;

                        break;
                    case Dir.Up:
                        newPlayerX = this.CurrentRoom.DungeonPortals.Find(param => param.DirectionToSpawn == Dir.Down).InteractionRectangle.X;
                        newPlayerY = this.CurrentRoom.DungeonPortals.Find(param => param.DirectionToSpawn == Dir.Down).InteractionRectangle.Y - 32;

                        break;
                    case Dir.Left:
                        newPlayerX = this.CurrentRoom.DungeonPortals.Find(param => param.DirectionToSpawn == Dir.Right).InteractionRectangle.X - 32;
                        newPlayerY = this.CurrentRoom.DungeonPortals.Find(param => param.DirectionToSpawn == Dir.Right).InteractionRectangle.Y;
                        break;
                    case Dir.Right:
                        newPlayerX = this.CurrentRoom.DungeonPortals.Find(param => param.DirectionToSpawn == Dir.Left).InteractionRectangle.X + 32;
                        newPlayerY = this.CurrentRoom.DungeonPortals.Find(param => param.DirectionToSpawn == Dir.Left).InteractionRectangle.Y;
                        break;
                }
            }
            
            Game1.Player.position = new Vector2(newPlayerX, newPlayerY);
        }

        public void GenerateRoomSavePath(DungeonRoom room)
        {
            string path = this.RoomDirectory + "/" + room.X + "," + room.Y + ".dat";
            if (File.Exists(path))
            {

                room.Load(path);
            }
            else
            {
                room.Generate(path);
            }
        }

        public override void UpdatePortals(Player player, MouseManager mouse)
        {
            if(IsStartingRoom())
            {
                base.UpdatePortals(player, mouse);
            }
            
            List<DungeonPortal> portals = this.CurrentRoom.DungeonPortals;

            for (int p = 0; p < portals.Count; p++)
            {

                if (player.Rectangle.Intersects(portals[p].InteractionRectangle))
                {
                    //portalgraph logic here

                    SwitchRooms(portals[p].DestinationRoom.X, portals[p].DestinationRoom.Y, portals[p].DirectionToSpawn);
                    return;
                }

            }
        }

        public override void Update(GameTime gameTime, MouseManager mouse, Player player)
        {
            base.Update(gameTime, mouse, player);
        }

        public override void Save(BinaryWriter writer)
        {

            this.AllTiles.Save(writer);
            writer.Write(this.SavePath);
            writer.Write(this.RoomDirectory);
        }

        public override void Load(BinaryReader reader)
        {
            this.AllTiles.Load(reader);
            this.SavePath = reader.ReadString();
            this.RoomDirectory = reader.ReadString();

        }
        public bool CurrentRoomDebugDataGotten { get; private set; }
        public int NorthPortal { get; private set; }
        public int EastPortal { get; private set; }
        public int SouthPortal { get; private set; }
        public int WestPortal { get; private set; }
        public string RoomString { get; private set; }
        public override string GetDebugString()
        {
            if (Game1.CurrentStage == Game1.ForestDungeon)
            {


                if (!CurrentRoomDebugDataGotten)
                {



                    if (CurrentRoom.DungeonPortals.Find(x => x.DirectionToSpawn == Dir.Up) != null)
                    {
                        NorthPortal = 1;
                    }
                    if (CurrentRoom.DungeonPortals.Find(x => x.DirectionToSpawn == Dir.Right) != null)
                    {
                        EastPortal = 1;

                    }
                    if (CurrentRoom.DungeonPortals.Find(x => x.DirectionToSpawn == Dir.Down) != null)
                    {
                        SouthPortal = 1;
                    }
                    if (CurrentRoom.DungeonPortals.Find(x => x.DirectionToSpawn == Dir.Left) != null)
                    {
                        WestPortal = 1;
                    }
                    CurrentRoomDebugDataGotten = true;
                    this.RoomString = this.CurrentRoom.X.ToString() + "," + this.CurrentRoom.Y.ToString() + "\n\n " + NorthPortal.ToString()
                        + "\n" + WestPortal.ToString() + "  " + EastPortal.ToString() + "\n  " + SouthPortal.ToString();
                }
                return this.RoomString;
            }
            else
            {
                return "Not in forest dungeon";
            }
        }

        private void ResetDebugString()
        {
            NorthPortal = 0;
            EastPortal = 0;
            SouthPortal = 0;
            WestPortal = 0;
            CurrentRoomDebugDataGotten = false;
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
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

        public DungeonRoom CurrentRoom { get; private set; }
        public DungeonRoom[,] Rooms { get; private set; }
        public ContentManager Content { get; private set; }

        public string RoomDirectory { get; set; }

        public Dungeon(string name, LocationType locationType, StageType stageType, GraphicsDevice graphics, ContentManager content, Texture2D tileSet, TmxMap tmxMap, int dialogueToRetrieve, int backDropNumber) : base( name,  locationType,  stageType,  graphics,  content,  tileSet,  tmxMap,  dialogueToRetrieve,  backDropNumber)
        {
            this.Rooms = new DungeonRoom[MaxDungeonRooms, MaxDungeonRooms];
            this.Content = content;
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
                CreateFirstRoom();
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
        }

        public virtual void SwitchRooms(int x, int y)
        {
            DungeonRoom room;
            
            room = new DungeonRoom(this, x, y, this.Content);

            GenerateRoomSavePath(room);
            this.Rooms[x, y] = room;
            this.CurrentRoom = room;
        }

        public void GenerateRoomSavePath(DungeonRoom room)
        {
            string path = this.RoomDirectory + "/"+ room.X + "," + room.Y + ".dat";
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

            List<DungeonPortal> portals = this.CurrentRoom.DungeonPortals;

            for (int p = 0; p < portals.Count; p++)
            {
                if (player.ClickRangeRectangle.Intersects(portals[p].PortalStart))
                {
                    if (portals[p].MustBeClicked)
                    {
                        if (mouse.WorldMouseRectangle.Intersects(portals[p].PortalStart) && mouse.IsClicked)
                        {
                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpen);
                            Game1.SwitchStage((Stages)portals[p].From, (Stages)portals[p].To, portals[p]);
                            OnSceneChanged();
                            SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
                            return;
                        }
                    }
                    else
                    {
                        if (player.Rectangle.Intersects(portals[p].PortalStart))
                        {
                            Game1.SwitchStage((Stages)portals[p].From, (Stages)portals[p].To, portals[p]);
                            OnSceneChanged();
                            SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
                            return;
                        }

                    }
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


    }
}

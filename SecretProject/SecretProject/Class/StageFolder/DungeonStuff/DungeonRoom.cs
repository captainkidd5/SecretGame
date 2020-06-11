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
        public bool IsStartingRoom { get; set; }
        public Dungeon Dungeon { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ITileManager TileManager { get; private set; }

        public bool HasGenerated { get; set; }

        public List<DungeonPortal> DungeonPortals { get; set; }



        public DungeonRoom(Dungeon dungeon, int x, int y)
        {
            this.Dungeon = dungeon;
            this.X = x;
            this.Y = y;
            

            this.DungeonPortals = new List<DungeonPortal>();

        }
        //for starting room
        public DungeonRoom(ITileManager tileManager, Dungeon dungeon, int x, int y, ContentManager content)
        {
            this.Dungeon = dungeon;
            this.X = x;
            this.Y = y;
            this.TileManager = tileManager;

            this.DungeonPortals = new List<DungeonPortal>();
        }

        public void Generate( string path)
        {
            int roomDimensions = 64;
            this.TileManager = new TileManager(Dungeon.AllTiles.TileSet, Dungeon.AllTiles.MapName, Dungeon.Graphics, Dungeon.Content, (int)Dungeon.TileSetNumber, Dungeon, roomDimensions);
            for (int z = 0; z < this.TileManager.AllTiles.Count; z++)
            {
                for (int i = 0; i < this.TileManager.AllTiles[z].GetLength(0); i++)
                {
                    for (int j = 0; j < this.TileManager.AllTiles[z].GetLength(1); j++)
                    {
                        Tile tempTile;
                        int gid = 0;
                        if (z == 0)
                        {
                            gid = 402;
                             
                        }
                        else if(z == 3)
                        {
                            if (j == 5)
                            {
                                gid = 1221;
                            }
                            else if (j == 63)
                            {
                                gid = 523;
                            }
                            else
                            {
                                if(Game1.Utility.RNumber(0, 10 ) < 3)
                                {
                                    gid = 34;
                                }
                            }
                        }
                        else if (z == 4)
                        {
                            if (i == 63)
                            {
                                gid = 719;
                            }
                            else if (i == 0)
                            {
                                gid = 722;
                            }
                        }

                        tempTile = new Tile(i, j, gid) { LayerToDrawAt = i };
                        this.TileManager.AllTiles[z][i, j] = tempTile;

                        TileUtility.AssignProperties(this.TileManager.AllTiles[z][i, j], z, i, j, (IInformationContainer)this.TileManager);



                    }
                }
            }
            Save(path);
        }

        public void Save(string path)
        {
            string newPath = path + this.X + this.Y;
            using (FileStream fileStream = File.OpenWrite(path))
            {


                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    this.TileManager.Save(binaryWriter);
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
                   


                    

                    binaryReader.Close();

                }
            }
        }
    }
}

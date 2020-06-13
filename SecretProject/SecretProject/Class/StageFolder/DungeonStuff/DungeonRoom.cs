using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.PathFinding;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.TileStuff.SpawnStuff;
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
        public bool ContainsDoorDown { get; private set; }
        public bool ContainsDoorUp { get; private set; }
        public bool ContainsDoorLeft { get; private set; }
        public bool ContainsDoorRight { get; private set; }
        private void GetDoorwaysBasedOnPortals()
        {
            if(this.DungeonPortals.Find(x => x.DirectionToSpawn == Dir.Down) != null)
            {
                this.ContainsDoorDown = true;
            }
            if (this.DungeonPortals.Find(x => x.DirectionToSpawn == Dir.Up) != null)
            {
                this.ContainsDoorUp = true;
            }
            if (this.DungeonPortals.Find(x => x.DirectionToSpawn == Dir.Left) != null)
            {
                this.ContainsDoorLeft = true;
            }
            if (this.DungeonPortals.Find(x => x.DirectionToSpawn == Dir.Right) != null)
            {
                this.ContainsDoorRight = true;
            }
        }

        /// <summary>
        /// Creates four walls around the room, and will create holes if there exists a portal at that spot.
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void GenerateSorroundingWalls(ref int gid, int i, int j)
        {
            if (j == 3 || j == 63)
            {
                gid = 720;

            }
            if (i == 0 || i == 63)
            {
                gid = 720;
            }

            if (ContainsDoorDown)
            {
                if (j == 63)
                {
                    if (i > 30 && i < 33)
                    {
                        gid = 0;
                    }
                }
            }
            if (ContainsDoorUp)
            {
                if (j == 3)
                {
                    if (i > 30 && i < 33)
                    {
                        gid = 0;
                    }
                }
            }
            if (ContainsDoorLeft)
            {
                if (i == 0)
                {
                    if (j > 30 && j < 33)
                    {
                        gid = 0;
                    }
                }
            }
            if (ContainsDoorRight)
            {
                if (i == 63)
                {
                    if (j > 30 && j < 33)
                    {
                        gid = 0;
                    }
                }
            }
        }

        public void Generate(string path)
        {
            GetDoorwaysBasedOnPortals();
            int roomDimensions = 64;
            this.TileManager = new TileManager(Dungeon.AllTiles.TileSet, Dungeon.AllTiles.MapName, Dungeon.Graphics, Dungeon.Content, (int)Dungeon.TileSetNumber, Dungeon, roomDimensions);

            float[,] bottomNoise = new float[64, 64];
            for (int i = 0; i < this.TileManager.AllTiles[0].GetLength(0); i++)
            {
                for (int j = 0; j < this.TileManager.AllTiles[0].GetLength(1); j++)
                {
                    bottomNoise[i, j] = Game1.Procedural.OverworldBackNoise.GetNoise(this.X * 16 * 64 + i, this.Y * 16 * 64 + j);

                }
            }

            for (int z = 0; z < this.TileManager.AllTiles.Count; z++)
            {
                for (int i = 0; i < this.TileManager.AllTiles[z].GetLength(0); i++)
                {
                    for (int j = 0; j < this.TileManager.AllTiles[z].GetLength(1); j++)
                    {
                        Tile tempTile;
                        int gid = 0;
                        if (z != 4)
                        {
                            gid = Game1.Procedural.NoiseConverter.ConvertNoiseToGID(ChunkType.Rai, bottomNoise[i, j], z);
                        }


                        if (z == 3)
                        {



                            GenerateSorroundingWalls(ref gid, i, j);
                            
                        }

  
                        tempTile = new Tile(i, j, gid) { LayerToDrawAt = z };
                        this.TileManager.AllTiles[z][i, j] = tempTile;

                        



                    }
                }
            }
            
            Dictionary<int, TmxTilesetTile> tileDictionary = this.TileManager.MapName.Tilesets[0].Tiles;
            for (int z = 0; z < this.TileManager.AllTiles.Count; z++)
            {
                for (int i = 0; i < this.TileManager.AllTiles[z].GetLength(0); i++)
                {
                    for (int j = 0; j < this.TileManager.AllTiles[z].GetLength(1); j++)
                    {
                        if (tileDictionary.ContainsKey(TileManager.AllTiles[z][i, j].GID))
                        {
                            if (tileDictionary[TileManager.AllTiles[z][i, j].GID].Properties.ContainsKey("generate"))
                            {
                                this.TileManager.AllTiles[z][i, j].GenerationType = (GenerationType)Enum.Parse(typeof(GenerationType),
                                    tileDictionary[TileManager.AllTiles[z][i, j].GID].Properties["generate"]);
                                //grass = 1, stone = 2, wood = 3, sand = 4


                                TilingContainer container = Game1.Procedural.GetTilingContainerFromGID(this.TileManager.AllTiles[z][i, j].GenerationType);
                                if (container != null)
                                {

                                    int newGID = this.TileManager.AllTiles[z][i, j].GID;
                                    Game1.Procedural.GenerationReassignForTiling(newGID, container.GeneratableTiles, container.TilingDictionary,
                                        z, i, j, this.TileManager.AllTiles[z].GetLength(0), this.TileManager.AllTiles[z].GetLength(0), (IInformationContainer)this.TileManager, null);
                                }
                            }
                        }
                        
                    }
                }
            }
           
            for (int z = 0; z < this.TileManager.AllTiles.Count; z++)
            {
                for (int i = 0; i < this.TileManager.AllTiles[z].GetLength(0); i++)
                {
                    for (int j = 0; j < this.TileManager.AllTiles[z].GetLength(1); j++)
                    {
                        
                        TileUtility.AssignProperties(this.TileManager.AllTiles[z][i, j], z, i, j, (IInformationContainer)this.TileManager);
                        if (z == 3)
                        {
                            if (this.TileManager.PathGrid.Weight[i, j] == (int)GridStatus.Clear)
                            {
                                if (this.TileManager.AllTiles[z][i, j].TileKey != null)
                                {
                                    SpawnHolder.AddGrassTufts((IInformationContainer)TileManager, this.TileManager.AllTiles[z][i, j], this.TileManager.AllTiles[1][i, j]);
                                }

                            }

                        }
                    }
                }
            }
            SpawnHolder.SpawnOverWorld(Game1.OverWorldSpawnHolder, (IInformationContainer)this.TileManager, Game1.Utility.RGenerator);
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
                    this.TileManager = this.Dungeon.AllTiles;
                    this.TileManager.Load(binaryReader);





                    binaryReader.Close();

                }
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
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

        public void Generate(string path)
        {
            int roomDimensions = 64;
            this.TileManager = new TileManager(Dungeon.AllTiles.TileSet, Dungeon.AllTiles.MapName, Dungeon.Graphics, Dungeon.Content, (int)Dungeon.TileSetNumber, Dungeon, roomDimensions);

            float[,] bottomNoise = new float[64, 64];
            for (int i = 0; i < this.TileManager.AllTiles[0].GetLength(0); i++)
            {
                for (int j = 0; j < this.TileManager.AllTiles[0].GetLength(1); j++)
                {
                    bottomNoise[i, j] = Game1.Procedural.OverworldBackNoise.GetNoise(this.X * 16 + i, this.Y * 16 + j);

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

                            if (j == 4 || j == 63)
                            {
                                if (i < 30 || i > 34)
                                {
                                    gid = 720;
                                }

                            }

                        }
                        else if (z == 4)
                        {
                            if (j < 28 || j > 34)
                            {
                                if (i == 63)
                                {
                                    gid = 720;
                                }
                                else if (i == 0)
                                {
                                    gid = 720;
                                }
                            }

                        }

                        tempTile = new Tile(i, j, gid) { LayerToDrawAt = i };
                        this.TileManager.AllTiles[z][i, j] = tempTile;

                        



                    }
                }
            }
            SpawnHolder.SpawnOverWorld(Game1.OverWorldSpawnHolder, (IInformationContainer)this.TileManager, Game1.Utility.RGenerator);
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
                                this.TileManager.AllTiles[z][i, j].GenerationType = (GenerationType)Enum.Parse(typeof(GenerationType), tileDictionary[TileManager.AllTiles[z][i, j].GID].Properties["generate"]);
                                //grass = 1, stone = 2, wood = 3, sand = 4
                            }

                            TilingContainer container = Game1.Procedural.GetTilingContainerFromGID(this.TileManager.AllTiles[z][i, j].GenerationType);
                            if (container != null)
                            {

                                int newGID = this.TileManager.AllTiles[z][i, j].GID ;
                                Game1.Procedural.GenerationReassignForTiling(newGID, container.GeneratableTiles, container.TilingDictionary, z, i, j, TileUtility.ChunkWidth, TileUtility.ChunkHeight,(IInformationContainer)this.TileManager, null);
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
                    this.TileManager = this.Dungeon.AllTiles;
                    this.TileManager.Load(binaryReader);





                    binaryReader.Close();

                }
            }
        }
    }
}

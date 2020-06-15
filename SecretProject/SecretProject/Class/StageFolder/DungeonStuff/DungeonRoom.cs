﻿using Microsoft.Xna.Framework;
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
        public int Width { get; set; }
        public bool IsStartingRoom { get; set; }
        public Dungeon Dungeon { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ITileManager TileManager { get; private set; }

        public bool HasGenerated { get; set; }

        public List<DungeonPortal> DungeonPortals { get; set; }

        private int leftWallTop;
        private int rightWallTop;
        private int topWallLeft;
        private int bottomWallLeft;


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
            if (j == 3 || j == this.Width - 1)
            {
                gid = 720;

            }
            if (i == 0 || i == this.Width - 1)
            {
                gid = 720;
            }

            if (ContainsDoorDown)
            {
                if (j == this.Width - 1)
                {
                    
                    int rightSide = bottomWallLeft + 5;
                    if (i > bottomWallLeft && i < rightSide)
                    {
                        gid = 0;
                    }
                }
            }
            if (ContainsDoorUp)
            {
                if (j == 3)
                {
                    int rightSide = topWallLeft + 5;
                    if (i > topWallLeft && i < rightSide)
                    {
                        gid = 0;
                    }
                }
            }
            if (ContainsDoorLeft)
            {
                if (i == 0)
                {

                    int bottomSide = leftWallTop - 5;
                    if (j < leftWallTop && j > bottomSide)
                    {
                        gid = 0;
                    }
                }
            }
            if (ContainsDoorRight)
            {
                if (i == Width -1)
                {

                    int bottomSide = rightWallTop - 5;
                    if (j < rightWallTop && j > bottomSide)
                    {
                        gid = 0;
                    }
                }
            }
        }

        public void Generate(string path)
        {
            GetDoorwaysBasedOnPortals();
            int roomDimensions = 128;
            this.Width = roomDimensions;
            this.TileManager = new TileManager(Dungeon.AllTiles.TileSet, Dungeon.AllTiles.MapName, Dungeon.Graphics, Dungeon.Content, (int)Dungeon.TileSetNumber, Dungeon, roomDimensions);
            topWallLeft = Game1.Utility.RGenerator.Next(0, this.Width +5);
            bottomWallLeft = Game1.Utility.RGenerator.Next(0, this.Width + 5);
            leftWallTop = Game1.Utility.RGenerator.Next(0, this.Width + 5);
            rightWallTop = Game1.Utility.RGenerator.Next(0, this.Width + 5);
            float[,] bottomNoise = new float[Width, Width];
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
            Dungeon.Enemies = new List<NPCStuff.Enemies.Enemy>();
            if (Game1.AllowNaturalNPCSpawning)
            {


                if (Dungeon.Enemies.Count < Game1.NPCSpawnCountLimit)
                {


                    Tile tile = SearchForEmptyTile(3);
                    if (tile != null)
                    {
                        TilingContainer tilingContainer = Game1.Procedural.GetTilingContainerFromGID(tile.GenerationType);
                        if (tilingContainer != null)
                        {

                            Dungeon.Enemies.AddRange(Dungeon.NPCGenerator.SpawnNpcPack(tilingContainer.GenerationType, new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y)));
                        }

                    }
                }
            }
            Dungeon.MapRectangle = new Rectangle(0, 0, 16 * this.Width, 16 * this.Width);
            Save(path);
        }

        /// <summary>
        /// Tries X times at random to find a tile which doesn't contain an obstacle
        /// </summary>
        /// <param name="timesToSearch">number of attempts</param>
        /// <returns></returns>
        private Tile SearchForEmptyTile(int timesToSearch)
        {
            for (int i = 0; i < timesToSearch; i++)
            {
                int randomSpawnX = Game1.Utility.RNumber(2, TileManager.MapWidth - 1);
                int randomSpawnY = Game1.Utility.RNumber(2, TileManager.MapWidth - 1);
                if (this.TileManager.PathGrid.Weight[randomSpawnX, randomSpawnY] == 1)
                {
                    return this.TileManager.AllTiles[0][randomSpawnX, randomSpawnY];
                }
            }

            return null;
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



                    Dungeon.MapRectangle = new Rectangle(0, 0, 16 * this.Width, 16 * this.Width);

                    binaryReader.Close();

                }
            }
        }
    }
}

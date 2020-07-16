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
        public int Width { get; set; }
        public bool IsStartingRoom { get; set; }
        protected Dungeon Dungeon { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public TileManager TileManager { get; set; }

        public bool HasGenerated { get; set; }

        public List<DungeonPortal> DungeonPortals { get; set; }

        protected int leftWallTop;
        protected int rightWallTop;
        protected int topWallLeft;
        protected int bottomWallLeft;

        protected bool ContainsDoorDown;
        protected bool ContainsDoorUp;
        protected bool ContainsDoorLeft;
        protected bool ContainsDoorRight;


        public DungeonRoom(Dungeon dungeon, int x, int y)
        {
            this.Dungeon = dungeon;
            this.X = x;
            this.Y = y;


            this.DungeonPortals = new List<DungeonPortal>();

            this.Width = 128;

        }
        //for starting room
        public DungeonRoom(TileManager tileManager, Dungeon dungeon, int x, int y, ContentManager content)
        {
            this.Dungeon = dungeon;
            this.X = x;
            this.Y = y;
            this.TileManager = tileManager;

            this.DungeonPortals = new List<DungeonPortal>();
        }

        private void GetDoorwaysBasedOnPortals()
        {
            if (this.DungeonPortals.Find(x => x.DirectionToSpawn == Dir.Down) != null)
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
        protected virtual void GenerateSorroundingWalls(ref int gid, int i, int j)
        {
            if (j <= 3 || j >= this.Width - 3) //top and bottom walls
            {
                gid = 3031;

            }
            if (i <= 3 || i >= this.Width - 3) // left and right
            {
                gid = 3031;
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
                if (i == Width - 1)
                {

                    int bottomSide = rightWallTop - 5;
                    if (j < rightWallTop && j > bottomSide)
                    {
                        gid = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Checks to ensure a specified indexes below do not equal a gid
        /// </summary>
        /// <param name="gid">gid you wish to test for</param>
        /// <param name="layer">layer we are testing for</param>
        /// <param name="i">current index X</param>
        /// <param name="j">current index Y</param>
        /// <param name="yTilesBelow">how many down to test for</param>
        /// <returns></returns>
        private bool YTilesBelowDoNotMatchGID(int gid, int layer, int i, int j, int yTilesBelow)
        {
            if(j + yTilesBelow >= this.Width)
            {
                return true;
            }
            for(int z  = j; z < j+ yTilesBelow; z++)
            {
                if(this.TileManager.AllTiles[layer][i,z].GID == gid)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// places front tiles with cellular automata. Good for forest!
        /// </summary>
        /// <param name="positiveGID"></param>
        /// <param name="negativeGID"></param>
        protected virtual void PlaceFront(int positiveGID, int negativeGID)
        {
            CellularAutomata cellularAutomata = new CellularAutomata();
            bool[,] boolMap = cellularAutomata.generateMap(this.Width);

            for(int i =0; i < this.Width; i++)
            {
                for(int j = 0; j < this.Width; j++)
                {

                    int gid;
                    if (i <=1 || i >= this.Width - 2 || j  <= 1 || j >= this.Width - 2) //edges are always solid, unless they are a portal.
                    {
                        gid = 3032;
                    }
                    else
                    {


                        if (boolMap[i, j] && YTilesBelowDoNotMatchGID(118, (int)MapLayer.BackGround, i, j, (int)MapLayer.ForeGround))//not above water!
                        {
                            gid = positiveGID;

                        }
                        else
                        {
                            gid = negativeGID;
                        }
                    }
                    TileManager.AllTiles[(int)MapLayer.Front][i, j] = new Tile(i, j, gid) { LayerToDrawAt = (int)MapLayer.Front };
                }
            }
        }

        public virtual void Generate(string path)
        {
            GetDoorwaysBasedOnPortals();
            int roomDimensions = this.Width;
            this.Width = roomDimensions;
            this.TileManager = new TileManager(Dungeon.AllTiles.TileSet, Dungeon.AllTiles.MapName, Dungeon.Graphics, Dungeon.Content, (int)Dungeon.LocationType, Dungeon, false,roomDimensions);
            topWallLeft = Game1.Utility.RGenerator.Next(0, this.Width - 5);
            bottomWallLeft = Game1.Utility.RGenerator.Next(0, this.Width - 5);
            leftWallTop = Game1.Utility.RGenerator.Next(0, this.Width - 5);
            rightWallTop = Game1.Utility.RGenerator.Next(0, this.Width - 5);
            if (this.ContainsDoorDown)
            {
                this.DungeonPortals.Find(x => x.DirectionToSpawn == Dir.Down).InteractionRectangle = DungeonGraph.GetRectangleFromDirection(new Vector2(bottomWallLeft * 16, Width * 16), 5, Dir.Down);
            }
            if (this.ContainsDoorUp)
            {
                this.DungeonPortals.Find(x => x.DirectionToSpawn == Dir.Up).InteractionRectangle = DungeonGraph.GetRectangleFromDirection(new Vector2(topWallLeft * 16, 0), 5, Dir.Up);
            }
            if (this.ContainsDoorLeft)
            {
                this.DungeonPortals.Find(x => x.DirectionToSpawn == Dir.Left).InteractionRectangle = DungeonGraph.GetRectangleFromDirection(new Vector2(0, leftWallTop * 16), 5, Dir.Left);
            }
            if (this.ContainsDoorRight)
            {
                this.DungeonPortals.Find(x => x.DirectionToSpawn == Dir.Right).InteractionRectangle = DungeonGraph.GetRectangleFromDirection(new Vector2(Width * 16, rightWallTop * 16), 5, Dir.Right);
            }
            float[,] bottomNoise = new float[Width, Width];
            for (int i = 0; i < this.TileManager.AllTiles[0].GetLength(0); i++)
            {
                for (int j = 0; j < this.TileManager.AllTiles[0].GetLength(1); j++)
                {
                    bottomNoise[i, j] = Game1.Procedural.OverworldBackNoise.GetNoise(this.X * 16 * this.Width + i, this.Y * 16 * this.Width + j);

                }
            }
            

            for (int z = 0; z < this.TileManager.AllTiles.Count - 1; z++)
            {
                for (int i = 0; i < this.TileManager.AllTiles[z].GetLength(0); i++)
                {
                    for (int j = 0; j < this.TileManager.AllTiles[z].GetLength(1); j++)
                    {
                        Tile tempTile;
                        int gid = 0;
                       // if (z != 4)
                       // {
                            gid = Game1.Procedural.NoiseConverter.ConvertNoiseToGID(bottomNoise[i, j], z);
                        // }

                       
                        //if (z == 4)
                        //{



                        //    GenerateSorroundingWalls(ref gid, i, j);

                        //}


                        tempTile = new Tile(i, j, gid) { LayerToDrawAt = z };
                        this.TileManager.AllTiles[z][i, j] = tempTile;





                    }
                }
            }

            PlaceFront(3032, 0);

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
                                    tileDictionary[this.TileManager.AllTiles[z][i, j].GID].Properties["generate"]);
                                //grass = 1, stone = 2, wood = 3, sand = 4


                                TilingTileManager TileManager = Game1.Procedural.GetTilingTileManagerFromGID(this.TileManager.AllTiles[z][i, j].GenerationType);
                                if (TileManager != null)
                                {

                                    int newGID = this.TileManager.AllTiles[z][i, j].GID;

                                    if(i > 0 && i < this.Width - 1 && j > 0 && j < this.Width -1) //dont reassign outer tiles for retiling, otherwise its obvious where the map ends.
                                    {
                                        Game1.Procedural.GenerationReassignForTiling(newGID, TileManager.GeneratableTiles, TileManager.TilingDictionary,
                                        z, i, j, this.TileManager.AllTiles[z].GetLength(0), this.TileManager.AllTiles[z].GetLength(0), (TileManager)this.TileManager, null);
                                    }

                                    
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
                        if (z == (int)MapLayer.ForeGround)
                        {
                            if (j >= 4)
                            {

                                int trunkTestForestGid = this.TileManager.AllTiles[(int)MapLayer.Front][i, j - 3].GID;
                                if (trunkTestForestGid == 3031 || trunkTestForestGid == 2935)
                                {
                                   Tile tempTile = new Tile(i, j, 3332) { LayerToDrawAt = z };
                                    this.TileManager.AllTiles[(int)MapLayer.ForeGround][i, j] = tempTile;
                                }
                            }
                        }

                        TileUtility.AssignProperties(this.TileManager.AllTiles[z][i, j], z, i, j, (TileManager)this.TileManager);
                        if (z == (int)MapLayer.ForeGround)
                        {
                            if (this.TileManager.PathGrid.Weight[i, j] == (int)GridStatus.Clear)
                            {
                                if (this.TileManager.AllTiles[z][i, j].TileKey != null)
                                {
                                    SpawnHolder.AddGrassTufts((TileManager)TileManager, this.TileManager.AllTiles[z][i, j], this.TileManager.AllTiles[(int)MapLayer.MidGround][i, j]);
                                }

                            }

                        }
                    }
                }
            }
            SpawnHolder.SpawnOverWorld(Game1.OverWorldSpawnHolder, (TileManager)this.TileManager, Game1.Utility.RGenerator);
            Dungeon.AllTiles = this.TileManager;
            Dungeon.Enemies = new List<NPCStuff.Enemies.Enemy>();
            if (Game1.Flags.AllowNaturalNPCSpawning)
            {


                if (Dungeon.Enemies.Count < Game1.NPCSpawnCountLimit)
                {

                    for (int e = 0; e < 5; e++)
                    {

                        Tile tile = SearchForEmptyTile(3);
                        if (tile != null)
                        {
                            TilingTileManager tilingTileManager = Game1.Procedural.GetTilingTileManagerFromGID(tile.GenerationType);
                            if (tilingTileManager != null)
                            {

                                Dungeon.Enemies.AddRange(Dungeon.NPCGenerator.SpawnNpcPack(tilingTileManager.GenerationType, new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y), (TileManager)this.TileManager));
                            }

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



                    Dungeon.MapRectangle = new Rectangle(0, 0, 16 * TileManager.MapWidth, 16 * TileManager.MapWidth);

                    binaryReader.Close();

                }
            }
        }
    }
}

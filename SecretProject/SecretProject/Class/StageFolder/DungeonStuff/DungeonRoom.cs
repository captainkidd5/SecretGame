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

        private bool ContainsDoorDown;
        private bool ContainsDoorUp;
        private bool ContainsDoorLeft;
        private bool ContainsDoorRight;


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
        private void GenerateSorroundingWalls(ref int gid, int i, int j)
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
                return false;
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
        private void PlaceFront(int positiveGID, int negativeGID)
        {
            CellularAutomata cellularAutomata = new CellularAutomata();
            bool[,] boolMap = cellularAutomata.generateMap(this.Width);

            for(int i =0; i < this.Width; i++)
            {
                for(int j = 0; j < this.Width; j++)
                {
                    int gid;
                    if(boolMap[i,j] && YTilesBelowDoNotMatchGID(118, 0, i, j, 3))//not above water!
                    {
                        gid = positiveGID;
                        
                    }
                    else
                    {
                        gid = negativeGID;
                    }
                    TileManager.AllTiles[4][i, j] = new Tile(i, j, gid) { LayerToDrawAt = 4 };
                }
            }
        }

        public void Generate(string path)
        {
            GetDoorwaysBasedOnPortals();
            int roomDimensions = 64;
            this.Width = roomDimensions;
            this.TileManager = new TileManager(Dungeon.AllTiles.TileSet, Dungeon.AllTiles.MapName, Dungeon.Graphics, Dungeon.Content, (int)Dungeon.TileSetNumber, Dungeon, roomDimensions);
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
                            gid = Game1.Procedural.NoiseConverter.ConvertNoiseToGID(ChunkType.Rai, bottomNoise[i, j], z);
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
                        if (z == 3)
                        {
                            if (j >= 3)
                            {


                                if (this.TileManager.AllTiles[4][i, j - 3].GID == 3031)
                                {
                                   Tile tempTile = new Tile(i, j, 3332) { LayerToDrawAt = z };
                                    this.TileManager.AllTiles[z][i, j] = tempTile;
                                }
                            }
                        }

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
            Dungeon.AllTiles = this.TileManager;
            Dungeon.Enemies = new List<NPCStuff.Enemies.Enemy>();
            if (Game1.AllowNaturalNPCSpawning)
            {


                if (Dungeon.Enemies.Count < Game1.NPCSpawnCountLimit)
                {

                    for (int e = 0; e < 5; e++)
                    {

                        Tile tile = SearchForEmptyTile(3);
                        if (tile != null)
                        {
                            TilingContainer tilingContainer = Game1.Procedural.GetTilingContainerFromGID(tile.GenerationType);
                            if (tilingContainer != null)
                            {

                                Dungeon.Enemies.AddRange(Dungeon.NPCGenerator.SpawnNpcPack(tilingContainer.GenerationType, new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y), (IInformationContainer)this.TileManager));
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff
{

    public enum WallDirection
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
    public class MazeMaker
    {
        public MazeCell[,] Cells { get; set; }


        public MazeMaker(int dimension)
        {
            this.Cells = new MazeCell[dimension, dimension];
        }

        public void Generate()
        {
            int totalCells = Cells.GetLength(0);
            int visitedCells = 0;

            while (visitedCells < totalCells)
            {

            }
        }
    }

    public class MazeCell
    {
        int x;
        int y;
        bool visited;
        public int[] Walls;
        private MazeCell[,] Cells;

        public MazeCell(int x, int y, MazeCell[,] cells)
        {
            this.x = x;
            this.y = y;
            this.Walls = new int[4];
            this.Cells = cells;


        }

        public MazeCell GetUnvisitednNeighbor(MazeCell previousCell)
        {
            List<MazeCell> neighborsWithAllFourWalls = new List<MazeCell>();

            for(int i = 0; i < 4; i++)
            {
                MazeCell newCell = GetCellFromDirection((WallDirection)i);
                if(newCell.HasAllWalls())
                {
                    neighborsWithAllFourWalls.Add(newCell);
                }
            }


            int index = Game1.Utility.RNumber(0, neighborsWithAllFourWalls.Count);
            

            if(newCell != null)
            {
                if(!newCell.visited)
                {
                    return newCell;
                }
            }


        }

        public bool HasAllWalls()
        {
            for (int i = 0; i < Walls.Length; i++)
            {
                if (Walls[i] == 1)
                {
                    return false;
                }
            }
            return true;
        }

        private MazeCell GetCellFromDirection(WallDirection wall)
        {
            switch (wall)
            {
                case WallDirection.Up:
                    return GetCell(x, y - 1);
                case WallDirection.Right:
                    return GetCell(x + 1, y);
                case WallDirection.Down:
                    return GetCell(x, y + 1);
                case WallDirection.Left:
                    return GetCell(x - 1, y);
                default:
                    throw new Exception("WallDirection " + wall.ToString() + " does not exist.");
            }
        }

        private MazeCell GetCell(int x, int y)
        {
            if(x < 0 || y < 0 || x > Cells.GetLength(0) || y > Cells.GetLength(0))
            {
                return null;
            }
            else
            {
                return Cells[x, y];
            }
        }

        private WallDirection GetOppositeWall(WallDirection initialWall)
        {
            switch (initialWall)
            {
                case WallDirection.Up:
                    return WallDirection.Down;
                case WallDirection.Right:
                    return WallDirection.Left;

                case WallDirection.Down:
                    return WallDirection.Up;
                case WallDirection.Left:
                    return WallDirection.Right;
                default:
                    throw new Exception("WallDirection " + initialWall.ToString() + " does not exist.");
            }
        }

        public void KnockDownWall(WallDirection wall)
        {
            Walls[(int)wall] = 0; // 0 means no wall
            GetCellFromDirection(wall).KnockDownWall(GetOppositeWall(wall));


        }
    }
}

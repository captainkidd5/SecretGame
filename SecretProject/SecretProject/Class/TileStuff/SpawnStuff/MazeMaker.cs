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
        private MazeCell[,] Cells { get; set; }

        private MazeCell CurrentCell { get; set; }

        private Stack<MazeCell> CellStack { get; set; }

        public MazeMaker(int dimension)
        {
            this.Cells = new MazeCell[dimension, dimension];
            for(int i =0; i < dimension; i++)
            {
                for(int j =0; j < dimension;j++)
                {
                    Cells[i, j] = new MazeCell(i, j, Cells);
                }
            }
            CurrentCell = Cells[0, 0];
            CellStack = new Stack<MazeCell>();
        }

        public MazeCell[,] Generate()
        {
            int totalCells = Cells.GetLength(0);
            int visitedCells = 0;

            while (visitedCells < totalCells)
            {
                GetUnvisitednNeighbor(ref visitedCells, CurrentCell);
            }
            return this.Cells;
        }

        public void GetUnvisitednNeighbor(ref int visistedCells, MazeCell previousCell)
        {
            List<MazeCell> neighborsWithAllFourWalls = new List<MazeCell>();
            List<WallDirection> correspondingDirections = new List<WallDirection>();

            for (int i = 0; i < 4; i++)
            {
                MazeCell newCell = previousCell.GetCellFromDirection((WallDirection)i);
                if (!newCell.isDummy)
                {
                    if (newCell.HasAllWalls())
                    {
                        neighborsWithAllFourWalls.Add(newCell);
                        correspondingDirections.Add((WallDirection)i);
                    }
                }

            }
            if(neighborsWithAllFourWalls.Count > 0)
            {
                int index = Game1.Utility.RNumber(0, neighborsWithAllFourWalls.Count);
                MazeCell newCell = neighborsWithAllFourWalls[index];
                newCell.KnockDownWall(correspondingDirections[index]);
                CellStack.Push(CurrentCell);
                CurrentCell = newCell;
                visistedCells++;
            }
            else
            {
                CurrentCell = CellStack.Pop();
            }
           

        }
    }

    public struct MazeCell
    {
        public bool isDummy;
        int x;
        int y;
        public int[] Walls;
        private MazeCell[,] Cells;

        public MazeCell(int x, int y, MazeCell[,] cells)
        {
            this.x = x;
            this.y = y;
            this.Walls = new int[4];
            this.Cells = cells;
            isDummy = false;


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

        public MazeCell GetCellFromDirection(WallDirection wall)
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
                MazeCell dummyCell = new MazeCell();
                dummyCell.isDummy = true;
                return dummyCell;

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

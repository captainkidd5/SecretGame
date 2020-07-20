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
            //dimension = dimension;
            this.Cells = new MazeCell[dimension, dimension];
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    Cells[i, j] = new MazeCell(i, j, Cells);
                }
            }
            CurrentCell = Cells[0, 0];
            CellStack = new Stack<MazeCell>();
        }

        public MazeCell[,] Generate()
        {
            int totalCells = Cells.GetLength(0) * Cells.GetLength(0);
            int visitedCells = 0;

            while (visitedCells <= (totalCells - 2))
            {
                GetUnvisitednNeighbor(ref visitedCells, CurrentCell);
            }
            return this.Cells;
        }

        public bool[,] TransformMap()
        {
            int dimension = this.Cells.GetLength(0);

            bool[,] newMap = new bool[dimension, dimension];


            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    MazeCell cell = Cells[i, j];
                    

                    if (j > 0)
                    {
                        if (Cells[i, j].Walls[0] == 1) //wall above is intact.
                        {
                            newMap[i, j - 1] = true;
                        }
                    }
                    if (j < dimension - 2)
                    {
                        if (Cells[i, j].Walls[2] == 1) //wall below is intact.
                        {
                            newMap[i, j + 1] = true;
                        }
                    }
                    if (i > 0)
                    {
                        if (Cells[i, j].Walls[3] == 1) //wall left is intact.
                        {
                            newMap[i - 1, j] = true;
                        }
                    }
                    if (i < dimension- 2)
                    {
                        if (Cells[i, j].Walls[1] == 1) //wall right is intact.
                        {
                            newMap[i + 1, j] = true;
                        }
                    }
                  

                }
               
            }

            return newMap;
        }




        public void GetUnvisitednNeighbor(ref int visistedCells, MazeCell previousCell)
        {
            List<MazeCell> neighborsWithAllFourWalls = new List<MazeCell>();
            List<WallDirection> correspondingDirections = new List<WallDirection>();
            if (CurrentCell.x > 2 && CurrentCell.y > 2 && CurrentCell.x < 120 && CurrentCell.y < 120)
            {
                Console.WriteLine("test");
            }
            for (int i = 0; i < 4; i++)
            {
                bool gotValidCell = true;
                MazeCell newCell = previousCell.GetCellFromDirection(ref gotValidCell,(WallDirection)i);
                if (gotValidCell)
                {
                    if (newCell.HasAllWalls())
                    {
                        neighborsWithAllFourWalls.Add(newCell);
                        correspondingDirections.Add((WallDirection)i);
                    }
                }

            }
            if (neighborsWithAllFourWalls.Count > 0)
            {
                int index = Game1.Utility.RNumber(0, neighborsWithAllFourWalls.Count);
                MazeCell newCell = neighborsWithAllFourWalls[index];
                newCell.KnockDownWall(newCell.GetOppositeWall(correspondingDirections[index]));
                CurrentCell.KnockDownWall(correspondingDirections[index]);
                CellStack.Push(CurrentCell);

                
                CurrentCell = newCell;
                
                visistedCells++;
            }
            else if(CellStack.Count > 0)
            {
                CurrentCell = CellStack.Pop();
            }


        }
    }

    public struct MazeCell
    {
        public int x;
        public int y;
        public int[] Walls;
        private MazeCell[,] Cells;

        public MazeCell(int x, int y, MazeCell[,] cells)
        {
            this.x = x;
            this.y = y;
            this.Walls = new int[4];
            this.Cells = cells;



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

        public MazeCell GetCellFromDirection(ref bool gotValidCell, WallDirection wall)
        {
            switch (wall)
            {
                case WallDirection.Up:
                    return GetCell(ref gotValidCell, x, y - 1);
                case WallDirection.Right:
                    return GetCell(ref gotValidCell, x + 1, y);
                case WallDirection.Down:
                    return GetCell(ref gotValidCell, x, y + 1);
                case WallDirection.Left:
                    return GetCell(ref gotValidCell, x - 1, y);
                default:
                    throw new Exception("WallDirection " + wall.ToString() + " does not exist.");
            }
        }

        private MazeCell GetCell(ref bool gotValidCell, int x, int y)
        {
            if (x < 0 || y < 0 || x >= Cells.GetLength(0) || y >= Cells.GetLength(0))
            {
                gotValidCell = false;
                return Cells[0,0]; 


            }
            else
            {
                gotValidCell = true;
                return Cells[x, y];
            }
        }

        public WallDirection GetOppositeWall(WallDirection initialWall)
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
            Walls[(int)wall] = 1; // 1 means no wall



        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff.MazeStuff
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
        public int Rows { get; }
        public int Columns { get; }
        public int Size => Rows * Columns;

        private List<List<Cell>> _grid;

        public virtual Cell this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= Rows)
                {
                    return null;
                }
                if (column < 0 || column >= Columns)
                {
                    return null;
                }
                return _grid[row][column];
            }
        }

        public Cell RandomCell()
        {
            var rand = new Random();
            var row = rand.Next(Rows);
            var col = rand.Next(Columns);
            var randomCell = this[row, col];
            if (randomCell == null)
            {
                throw new InvalidOperationException("Random cell is null");
            }
            return randomCell;
        }
        // Row iterator
        public IEnumerable<List<Cell>> Row
        {
            get
            {
                foreach (var row in _grid)
                {
                    yield return row;
                }
            }
        }
        // Cell iterator
        public IEnumerable<Cell> Cells
        {
            get
            {
                foreach (var row in Row)
                {
                    foreach (var cell in row)
                    {
                        yield return cell;
                    }
                }
            }
        }

        public MazeMaker(int rows, int cols)
        {
            Rows = rows;
            Columns = cols;

            PrepareGrid();
            ConfigureCells();
        }

        private void PrepareGrid()
        {
            _grid = new List<List<Cell>>();
            for (var r = 0; r < Rows; r++)
            {
                var row = new List<Cell>();
                for (var c = 0; c < Columns; c++)
                {
                    row.Add(new Cell(r, c));
                }
                _grid.Add(row);
            }
        }

        private void ConfigureCells()
        {
            foreach (var cell in Cells)
            {
                var row = cell.Row;
                var col = cell.Column;

                cell.North = this[row - 1, col];
                cell.South = this[row + 1, col];
                cell.West = this[row, col - 1];
                cell.East = this[row, col + 1];
            }
        }

        public override string ToString()
        {
            var output = new StringBuilder("+");
            for (var i = 0; i < Columns; i++)
            {
                output.Append("---+");
            }
            output.AppendLine();

            foreach (var row in Row)
            {
                var top = "|";
                var bottom = "+";
                foreach (var cell in row)
                {
                    const string body = "   ";
                    var east = cell.IsLinked(cell.East) ? " " : "|";

                    top += body + east;

                    var south = cell.IsLinked(cell.South) ? "   " : "---";
                    const string corner = "+";
                    bottom += south + corner;
                }
                output.AppendLine(top);
                output.AppendLine(bottom);
            }

            return output.ToString();
        }
        // ...
    }

    public class Cell
    {
        // Position in the maze
        public int Row { get; }
        public int Column { get; }

        // Neighboring cells

        public Cell North { get; set; }

        public Cell South { get; set; }

        public Cell East { get; set; }

        public Cell West { get; set; }

        public List<Cell> Neighbors
        {
            get { return new[] { North, South, East, West }.Where(c => c != null).ToList(); }
        }

        // Cells that are linked to this cell
        private readonly Dictionary<Cell, bool> _links;
        public List<Cell> Links => _links.Keys.ToList();

        public Cell(int row, int col)
        {
            Row = row;
            Column = col;
            _links = new Dictionary<Cell, bool>();
        }

        public void Link(Cell cell, bool bidirectional = true)
        {
            _links[cell] = true;
            if (bidirectional)
            {
                cell.Link(this, false);
            }
        }

        public void Unlink(Cell cell, bool bidirectional = true)
        {
            _links.Remove(cell);
            if (bidirectional)
            {
                cell.Unlink(this, false);
            }
        }

        public bool IsLinked(Cell cell)
        {
            if (cell == null)
            {
                return false;
            }
            return _links.ContainsKey(cell);
        }
        // ...
    }

    public static class ListExtensions
    {
        public static T Sample<T>(this List<T> list, Random rand = null)
        {
            if (rand == null)
            {
                rand = new Random();
            }
            return list[rand.Next(list.Count)];
        }
    }


}

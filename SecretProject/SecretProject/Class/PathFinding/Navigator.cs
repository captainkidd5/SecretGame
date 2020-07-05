using Microsoft.Xna.Framework;
using SecretProject.Class.PathFinding.PathFinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.PathFinding
{
    public class Navigator
    {
        public string EntityName { get; set; }
        public PathFinderFast PathFinderFast { get; set; }
        private List<PathFinderNode> CurrentPath { get; set; }
        private byte[,] Grid { get; set; }



        public Navigator(string entityName, byte[,] grid)
        {
            this.EntityName = entityName;
            this.Grid = grid;
            this.PathFinderFast = new PathFinderFast(Grid);
        }
        public bool TryFindNewPath( Vector2 currentEntityPosition, Point endPoint)
        {
            PathFinderFast finder = new PathFinderFast(this.Grid);


            Point start = new Point(Math.Abs((int)currentEntityPosition.X / 16),
             (Math.Abs((int)currentEntityPosition.Y / 16)));

            this.CurrentPath = finder.FindPath(start, endPoint, this.EntityName);
            if (this.CurrentPath == null)
            {
                this.CurrentPath = new List<PathFinderNode>();
                return false;
                throw new Exception(this.EntityName + " was unable to find a path between " + start + " and " + endPoint);
            }
            return true;
        }
    }
}

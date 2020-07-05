using Microsoft.Xna.Framework;
using SecretProject.Class.PathFinding.PathFinder;
using SecretProject.Class.Universal;
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
        public bool IsMoving { get; set; }
        public Vector2 DirectionVector { get; set; }

        private PathFinderFast PathFinderFast { get; set; }
        public List<PathFinderNode> CurrentPath { get; set; }
        private byte[,] Grid { get; set; }

        private float WanderTimer { get; set; }
        public bool HasReachedNextPoint { get; set; }


        public Navigator(string entityName, byte[,] grid)
        {
            this.EntityName = entityName;
            this.Grid = grid;
            this.PathFinderFast = new PathFinderFast(Grid);
            this.CurrentPath = new List<PathFinderNode>();
        }

        

        /// <summary>
        /// If on current path, moves towards next point. Else if allowed to wander, finds a new path.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Wander(GameTime gameTime, ref Vector2 entityPosition)
        {
            WanderTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.CurrentPath.Count > 0)
            {
                if (!MoveTowardsPointAndDecrementPath(gameTime,ref entityPosition))
                {
                    this.CurrentPath = new List<PathFinderNode>();
                    this.WanderTimer = 0;
                }

            }
            else if (WanderTimer > 0)
            {
                this.IsMoving = false;
            }
            else if (WanderTimer <= 0)
            {

                int currentTileX = Utility.GetSquareTile(entityPosition.X);
                int currentTileY = Utility.GetSquareTile(entityPosition.Y);
                Point newWanderPoint = GetNewWanderPoint(currentTileX, currentTileY);

                if (newWanderPoint.X < Game1.CurrentStage.MapRectangle.Width / 16 - 2 && newWanderPoint.X > 0 && //if within map bounds.
                    newWanderPoint.Y < Game1.CurrentStage.MapRectangle.Width / 16 - 2 && newWanderPoint.Y > 0)
                {
                    if (this.Grid[newWanderPoint.X, newWanderPoint.Y] != 0)
                        TryFindNewPath(entityPosition, newWanderPoint);

                }

            }
        }

        private bool TryFindNewPath(Vector2 currentEntityPosition, Point endPoint)
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

        /// <summary>
        /// returns true if the next point is clear. False if not. Only reason this would happen would be if their path is changed
        /// after it is already found. Example: player placing a barrel in front of them.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        private bool MoveTowardsPointAndDecrementPath(GameTime gameTime, ref Vector2 position)
        {
            PathFinderNode node = this.CurrentPath[this.CurrentPath.Count - 1];
            if (Game1.CurrentStage.AllTiles.PathGrid.Weight[node.X, node.Y] == (int)GridStatus.Clear)
            {
                if (MoveTowardsVector(new Vector2(this.CurrentPath[this.CurrentPath.Count - 1].X * 16, this.CurrentPath[this.CurrentPath.Count - 1].Y * 16),ref position,  gameTime))
                {
                    HasReachedNextPoint = true;
                }
                return true;
            }
            else
                return false;
        }

        private Point GetNewWanderPoint(int currentTileX, int currentTileY)
        {
            int newX = Game1.Utility.RGenerator.Next(-10, 10) + currentTileX;
            int newY = Game1.Utility.RGenerator.Next(-10, 10) + currentTileY;

            return new Point(newX, newY);
        }

        public bool MoveTowardsVector(Vector2 goal, ref Vector2 position, GameTime gameTime)
        {

            // If we're already at the goal return immediatly
            this.IsMoving = true;
            if (position.X + 2 > goal.X && position.X - 2 < goal.X
                && position.Y + 2 > goal.Y && position.Y - 2 < goal.Y)
            {
                return true;
            }

            // Find direction from current position to goal
            Vector2 direction = Vector2.Normalize(goal - position);
            this.DirectionVector = direction;

            // If we moved PAST the goal, move it back to the goal
            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - position)) + 1) < 0.1f)
                position = goal;

            // Return whether we've reached the goal or not, leeway of 2 pixels 
            if (position.X + 2 > goal.X && position.Y - 2 < goal.X
               && position.Y + 2 > goal.Y && position.Y - 2 < goal.Y)
            {
                return true;
            }
            return false;
        }

        public void MoveAwayFromPoint(Vector2 positionToMoveAwayFrom,float speed,ref Vector2 position, GameTime gameTime)
        {

            this.IsMoving = true;


            Vector2 direction = Vector2.Normalize(positionToMoveAwayFrom - position);
            this.DirectionVector = new Vector2(direction.X * -1, direction.Y * -1);

            position -= direction * speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        }
    }
}

using Microsoft.Xna.Framework.Content;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.StageFolder.DungeonStuff
{
    public class ForestRoom : DungeonRoom
    {
        public ForestRoom(Dungeon dungeon, int x, int y) : base(dungeon,x,y)
        {
            this.Dungeon = dungeon;
            this.X = x;
            this.Y = y;


            this.DungeonPortals = new List<DungeonPortal>();

            this.Width = 128;

        }
        //for starting room
        public ForestRoom(ITileManager tileManager, Dungeon dungeon, int x, int y, ContentManager content) : base( tileManager,  dungeon,  x,  y,  content)
        {
            this.Dungeon = dungeon;
            this.X = x;
            this.Y = y;
            this.TileManager = tileManager;

            this.DungeonPortals = new List<DungeonPortal>();
            this.Width = 128;
        }

        protected override void GenerateSorroundingWalls(ref int gid, int i, int j)
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
    }
}

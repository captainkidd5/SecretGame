﻿using Microsoft.Xna.Framework;
using SecretProject.Class.NPCStuff;

namespace SecretProject.Class.TileStuff
{
    public enum MapLayer
    {
        BackGround = 0,
        MidGround = 1,
        Buildings = 2,
        ForeGround = 3
    }

    public class Tile : IEntity
    {

        private int gid;
        public int GID { get { return gid - 1; } set { gid = value; } }
        public int Y { get; set; }
        public int X { get; set; }
        public float LayerToDrawAt { get; set; } = 0f;
        public float LayerToDrawAtZOffSet { get; set; } = 0f;
        public float ColorMultiplier { get; set; } = 1f;

        public Rectangle SourceRectangle { get; set; }
        public Rectangle DestinationRectangle { get; set; }

        public string TileKey { get; set; }

        public GenerationType GenerationType { get; set; }


        public Tile(int x, int y, int gID)
        {

            this.X = x;
            this.Y = y;

            this.GID = gID;



        }

        public Vector2 GetPosition(IInformationContainer container)
        {
            return new Vector2(this.DestinationRectangle.X + container.X, this.DestinationRectangle.Y + container.Y);
        }

        public string GetTileKeyStringNew(int layer, IInformationContainer container)
        {
            return "" + this.X + "," + this.Y + "," + layer;
        }


        public void PlayerCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
        {
            this.ColorMultiplier = .5f;
        }

        public void MouseCollisionInteraction()
        {
            Chunk chunk = ChunkUtility.GetChunk(X / 16, Y / 16, Game1.OverWorld.AllTiles.ActiveChunks);
            if(chunk != null)
            {
                if (Game1.GetCurrentStage().AllTiles.MapName.Tilesets[Game1.OverWorld.AllTiles.TileSetNumber].Tiles[GID ].Properties.ContainsKey("action"))
                {
                    string action = Game1.GetCurrentStage().AllTiles.MapName.Tilesets[Game1.OverWorld.AllTiles.TileSetNumber].Tiles[GID ].Properties["action"];

                    TileUtility.ActionHelper((int)this.LayerToDrawAt, this.X, this.Y, action, Game1.myMouseManager, chunk);
                    //z, mouseI, mouseJ, this.MapName.Tilesets[this.TileSetNumber].Tiles[this.ChunkUnderMouse.AllTiles[LayerToDrawAt][X, Y].GID].Properties["action"], mouse, this.ChunkUnderMouse
                }
            }
            

        }

        public void Reset()
        {
            this.ColorMultiplier = 1f;
        }

    }

}
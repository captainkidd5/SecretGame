using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Physics.CollisionDetection;
using SecretProject.Class.TileStuff.SpawnStuff;
using SecretProject.Class.TileStuff.TileModifications;
using System.Collections.Generic;

namespace SecretProject.Class.TileStuff
{
    public enum MapLayer
    {
        BedRock = 0,
        BackGround = 1,
        MidGround = 2,
        Buildings = 3,
        ForeGround = 4,
        Front = 5
    }

    public class Tile : IEntity
    {
        public static int TileWidth = 16;

        private int gid;
        public int GID { get { return gid - 1; } set { gid = value; } }
        public int Y { get; set; }
        public int X { get; set; }

        public float Layer { get; set; }
        public float LayerToDrawAt { get; set; } 
        public float LayerToDrawAtZOffSet { get; set; } 
        public float ColorMultiplier { get; set; } = 1f; //this is reset every frame in the quad tree insertion

        public Rectangle SourceRectangle { get; set; }
        public Rectangle DestinationRectangle { get; set; }
        public Vector2 Position { get; set; }

        public string TileKey { get; set; }

        public GenerationType GenerationType { get; set; }

        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }

        public List<HullBody> Bodies { get; set; }
        public List<ITileAddon> Addons { get; private set; }


        public Tile(int x, int y, int gID)
        {

            this.X = x;
            this.Y = y;

            this.GID = gID;

            this.Origin = Game1.Utility.Origin;

            this.Bodies = new List<HullBody>();
            this.Addons = new List<ITileAddon>();

        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Addons.Count; i++)
            {
                Addons[i].Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch, Texture2D texture, float layer)
        {
            spriteBatch.Draw(texture,Position, SourceRectangle, Color.White * ColorMultiplier,
                                Rotation, Origin, 1f, SpriteEffects.None, layer + LayerToDrawAtZOffSet);

            for (int i = 0; i < Addons.Count; i++)
            {
                Addons[i].Draw(spriteBatch);
            }
        }

        public Vector2 GetPosition(TileManager TileManager)
        {
            return new Vector2(this.DestinationRectangle.X + TileManager.X, this.DestinationRectangle.Y + TileManager.Y);
        }

        public string GetTileKeyString(int layer, TileManager TileManager)
        {
            return "" + this.X + "," + this.Y + "," + layer;
        }


        public void DamageCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
        {
            this.ColorMultiplier = .25f;
        }

        public void MouseCollisionInteraction()
        {
          
        }

        public void Reset()
        {
            this.ColorMultiplier = 1f;
        }

    }

}
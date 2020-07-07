using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.TileStuff;

namespace SecretProject.Class.NPCStuff.CaptureCrateStuff
{
    public class CaptureCrate
    {
        public GraphicsDevice Graphics { get; set; }
        public TileManager TileManager { get; set; }
        public EnemyType ContainedAnimal { get; set; }

        public CaptureCrate(GraphicsDevice graphics, TileManager TileManager)
        {
            this.Graphics = graphics;
            this.TileManager = TileManager;
        }

        public static void Release(EnemyType enemyType, GraphicsDevice graphics, TileManager TileManager = null)
        {
            //if (TileManager != null)
            //{
            //    Game1.CurrentStage.Enemies.Add(Enemy.GetEnemyFromType(enemyType, null,  Game1.Player.position, graphics, TileManager, true));
            //}
            //else
            //{
            //    Game1.CurrentStage.Enemies.Add(Enemy.GetEnemyFromType(enemyType, null, Game1.Player.position, graphics, (TileManager)Game1.CurrentStage.AllTiles, false));
            //}
        }




    }
}

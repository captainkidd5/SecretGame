using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.TileStuff;

namespace SecretProject.Class.NPCStuff.CaptureCrateStuff
{
    public class CaptureCrate
    {
        public GraphicsDevice Graphics { get; set; }
        public IInformationContainer Container { get; set; }
        public EnemyType ContainedAnimal { get; set; }

        public CaptureCrate(GraphicsDevice graphics, IInformationContainer container)
        {
            this.Graphics = graphics;
            this.Container = container;
        }

        public static void Release(EnemyType enemyType, GraphicsDevice graphics, IInformationContainer container = null)
        {
            if (container != null)
            {
                Game1.GetCurrentStage().Enemies.Add(Enemy.GetEnemyFromType(enemyType, Game1.Player.position, graphics, container, true));
            }
            else
            {
                Game1.GetCurrentStage().Enemies.Add(Enemy.GetEnemyFromType(enemyType, Game1.Player.position, graphics, (IInformationContainer)Game1.GetCurrentStage().AllTiles, false));
            }
        }




    }
}

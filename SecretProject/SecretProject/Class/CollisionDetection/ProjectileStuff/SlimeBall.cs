using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.CollisionDetection.ProjectileStuff
{
    class SlimeBall : Projectile
    {
        public SlimeBall(GraphicsDevice graphics, IEntity entityFiredFrom, Dir directionFiredFrom, Vector2 startPosition, float rotation, float speed, Vector2 positionToMoveToward, List<Projectile> allProjectiles) : base(graphics, entityFiredFrom, directionFiredFrom, startPosition, rotation, speed, positionToMoveToward, allProjectiles)
        {
            this.SourceRectangle = Game1.ItemVault.GenerateNewItem(255, null).SourceTextureRectangle;
        }
    }
}

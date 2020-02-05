using Microsoft.Xna.Framework;

namespace SecretProject.Class.NPCStuff
{
    public interface IEntity
    {

        void DamageCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom);
        void MouseCollisionInteraction();
        void Reset();
    }
}

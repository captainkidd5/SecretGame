using Microsoft.Xna.Framework;

namespace SecretProject.Class.NPCStuff
{
    public interface IEntity
    {

        void PlayerCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom);
        void MouseCollisionInteraction();
        void Reset();
    }
}

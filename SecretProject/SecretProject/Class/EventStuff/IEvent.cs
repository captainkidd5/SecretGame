using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.StageFolder;
using System.Collections.Generic;

namespace SecretProject.Class.EventStuff
{
    public interface IEvent
    {
        GraphicsDevice Graphics { get; set; }
        List<Character> CharactersInvolved { get; set; }
        bool FreezePlayerControls { get; set; }
        int DayToTrigger { get; set; }
        TmxStageBase StageToTrigger { get; set; }
        bool IsCompleted { get; set; }
        bool IsActive { get; set; }
        int CurrentStep { get; set; }
        int TotalSteps { get; set; }
        void Start();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}

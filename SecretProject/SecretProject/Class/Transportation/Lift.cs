using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Transportation
{

    public class Lift
    {
        public string LiftKey { get; set; }
        public int CurrentStage { get; set; }
        public Vector2 LocalPosition { get; set; }
        public string FlavorText { get; set; }

        public Lift(string liftKey, int currentStage, Vector2 locationPosition,string flavorText)
        {
            this.LiftKey = liftKey;
            this.CurrentStage = currentStage;
            this.LocalPosition = locationPosition;
            this.FlavorText = flavorText;
        }

        public void Transport(Lift LiftToTransportTo)
        {
            if(CurrentStage ==  (int)Stages.World)
            {
                for(int i =0; i < Game1.GetCurrentStage().AllTiles.ActiveChunks.GetLength(0); i++)
                {
                    for (int j = 0; j < Game1.GetCurrentStage().AllTiles.ActiveChunks.GetLength(1); j++)
                    {
                        Game1.GetCurrentStage().AllTiles.ActiveChunks[i, j].Save();
                    }
                }
            }
            Game1.SwitchStage(CurrentStage, LiftToTransportTo.CurrentStage);
            Game1.Player.Position = new Vector2(LiftToTransportTo.LocalPosition.X, LiftToTransportTo.LocalPosition.Y);
            if(LiftToTransportTo.CurrentStage == (int)Stages.World)
            {
                Game1.GetCurrentStage().AllTiles.LoadInitialChunks();
            }
            Game1.Player.UserInterface.CurrentOpenInterfaceItem = UI.ExclusiveInterfaceItem.None;
        }
    }
}

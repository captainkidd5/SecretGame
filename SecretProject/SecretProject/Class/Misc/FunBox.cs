using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Misc;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Misc
{
    public enum FunBoxElement
    {
        none = 0,
        bird = 1,
        mouse = 2,
        scarab = 3,
        
    }
    public class FunBox
    {
        private readonly int MinimumIntervalBeforeNewAdd = 5;
        private readonly int MaximumIntervalBeforeNewAdd = 25;

        private int NextInterval { get; set; }

        public List<FunItems> FunItems { get; set; }
        private List<IWeightable> FunData { get; set; }
        private List<IWeightable> GrassCreatureData { get; set; }
        private GraphicsDevice Graphics { get; set; }

        private int TimeLastElementAdded { get; set; }
        

        public FunBox(GraphicsDevice graphics)
        {
            this.Graphics = graphics;
            this.FunItems = new List<FunItems>();
            this.FunData = new List<IWeightable>()
            {
                new FunItemData(FunBoxElement.bird, 25),
            };

            this.GrassCreatureData = new List<IWeightable>()
            {
                new FunItemData(FunBoxElement.mouse, 50),
                new FunItemData(FunBoxElement.scarab, 50),
            };
        }

        public void AddGrassCreature(GrassCreatureType grassCreatureType, Vector2 position)
        {
            GrassCreature grassCreature;
            switch(grassCreatureType)
            {
                case GrassCreatureType.mouse:
                    grassCreature = new GrassCreature(GrassCreatureType.mouse, this.Graphics, position, this.FunItems);
                    break;

                default:
                    grassCreature = new GrassCreature(GrassCreatureType.mouse, this.Graphics, position, this.FunItems);
                    break;
            }
            this.FunItems.Add(grassCreature);
        }

        public void AddRandomGrassCreature(Vector2 position)
        {
            FunItemData grassCreature = (FunItemData)WheelSelection.GetSelection(this.GrassCreatureData);
            GrassCreature creature = null;
            switch (grassCreature.FunBoxElement)
            {
                case FunBoxElement.mouse:
                    creature = new GrassCreature(GrassCreatureType.mouse, this.Graphics, position, this.FunItems);
                    break;

                case FunBoxElement.scarab:
                    creature = new GrassCreature(GrassCreatureType.scarab, this.Graphics, position, this.FunItems);
                    break;

                default:
                    creature = new GrassCreature(GrassCreatureType.mouse, this.Graphics, position, this.FunItems);
                    break;
            }
            this.FunItems.Add(creature);
        }

        public void AddRandomElement()
        {
            IWeightable data = WheelSelection.GetSelection(FunData);

            switch((data as FunItemData).FunBoxElement)
            {
                case FunBoxElement.bird:
                    if(Game1.CurrentStage.LocationType == StageFolder.LocationType.Exterior)
                    {
                        int amt = Game1.Utility.RNumber(1, 5);
                        for (int i = 0; i < amt; i++)
                        {
                            this.FunItems.Add(new SmallBird(this.Graphics, GetDirection(), this.FunItems));
                        }
                    }
                    break;

            }
        }

        

        private Dir GetDirection()
        {
            if (Game1.Utility.RGenerator.Next(0, 2) == 0)
                return Dir.Right;
            else
                return Dir.Left;


        }

        /// <summary>
        /// Checks to see whether enough time has passed for a new fun item to be added to the game. If yes, reset the next time interval to a random 
        /// value between the min and max readonly values.
        /// </summary>
        /// <returns></returns>
        private bool CheckIfAdd()
        {
            if (TimeLastElementAdded + NextInterval < Game1.GlobalClock.SecondsPassedToday)
            {
                
                TimeLastElementAdded = (int)Game1.GlobalClock.SecondsPassedToday;
                NextInterval = Game1.Utility.RGenerator.Next(MinimumIntervalBeforeNewAdd, MaximumIntervalBeforeNewAdd);
                return true;

            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            if(CheckIfAdd())
            {
                AddRandomElement();
            }

            for(int i = 0; i < this.FunItems.Count; i++)
            {
                this.FunItems[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.FunItems.Count; i++)
            {
                this.FunItems[i].Draw(spriteBatch);
            }
        }
        
    }

    public class FunItemData : IWeightable
    {
        public FunBoxElement FunBoxElement { get; set; }
        public int Chance { get; set; }

        public FunItemData(FunBoxElement funBoxElement, int chance)
        {
            this.FunBoxElement = funBoxElement;
            this.Chance = chance;
        }
    }
}

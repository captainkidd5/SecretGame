using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Universal
{
    public class TurnDial
    {
        
        public Stage CurrentLocation { get; set; }

        private List<Stage> AvailableLocations { get; set; }
        private int CurrentIndex { get; set; }

        public TurnDial()
        {
            this.CurrentIndex = 0;
            this.AvailableLocations = new List<Stage>()
            {
                StageManager.Town,
                StageManager.HomeStead,
                StageManager.SippiDesert

            };
            CurrentLocation = StageManager.Town;
        }

        public void CycleLocation(Dir direction, TileManager tileManager, int layer, int x, int y)
        {
            int tilePosition = 0;
            switch(direction)
            {
                case Dir.Right:
                    CurrentIndex++;
                    tilePosition = -1;
                    if (CurrentIndex >= AvailableLocations.Count)
                        CurrentIndex = 0;
                    break;
                case Dir.Left:
                    CurrentIndex--;
                    tilePosition = 1;
                    if (CurrentIndex < 0)
                        CurrentIndex = AvailableLocations.Count - 1;
                    break;
                default:
                    throw new Exception("Turn dial cannot be turned in direction " + direction.ToString());
            }

            TileUtility.ReplaceTile(layer, x + tilePosition, y, 1601 + 100 * CurrentIndex, tileManager);
            CurrentLocation = AvailableLocations[CurrentIndex];
        }
    }
}

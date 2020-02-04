using Microsoft.Xna.Framework;

namespace SecretProject.Class.PathFinding
{
    public enum GridStatus
    {
        Obstructed = 0,
        Clear = 1
    }


    public class ObstacleGrid
    {

        public Rectangle Size;

        public byte[,] Weight;



        public ObstacleGrid(int mapWidth, int mapHeight)
        {
            Size = new Rectangle(0, 0, mapWidth, mapHeight);
            Weight = new byte[mapWidth, mapHeight];
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    Weight[i, j] = (int)GridStatus.Clear;
                }
            }

        }

        //1 empty, 0 obstructed
        public void UpdateGrid(int indexI, int indexJ, GridStatus newValue)
        {
          //  if(indexI < 16 && indexI >= 0)
          //  {
            //    if(indexJ < 16 && indexJ >= 0)
             //   {
                    Weight[indexI, indexJ] = (byte)newValue;
                    return;
             //   }
                
           // }
            System.Console.WriteLine("Index Error");
            
        }



    }
}

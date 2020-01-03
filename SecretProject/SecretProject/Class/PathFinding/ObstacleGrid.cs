using Microsoft.Xna.Framework;

namespace SecretProject.Class.PathFinding
{
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
                    Weight[i, j] = 1;
                }
            }

        }

        //1 empty, 0 obstructed
        public void UpdateGrid(int indexI, int indexJ, int newValue)
        {
            Weight[indexI, indexJ] = (byte)newValue;
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.StageFolder;

namespace SecretProject.Class.TileStuff.SpawnStuff.CampStuff
{
    public class ArcaneCamp : Camp
    {


        public ArcaneCamp(int probability) : base(probability)
        {
            this.CampType = CampType.Arcane;

            this.SpawnElements = new List<SpawnElement>()
            {
                //new SpawnElement(3740, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 15){Unlocked = true }, //Purple Orb
                // new SpawnElement(3742, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 15){Unlocked = true }, //Magic stick
                //  new SpawnElement(3743, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 15){Unlocked = true }, //Arcane Crystal
            };
            this.FloorTileID = 3347;

        }

        public override void Spawn(IInformationContainer container, ILocation location)
        {
            base.Spawn(container, location);
            Vector2 position = new Vector2(container.X * 16 * 16 + 64, container.Y * 16 * 16 + 64);


            location.Enemies.Add(new InkWizard("InkWizard", null, position, container.GraphicsDevice, Game1.AllTextures.EnemySpriteSheet, container, CurrentBehaviour.Wander));

            TilingContainer floortiling = Game1.Procedural.GetTilingContainerFromGID(GenerationType.ArcaneFloor);

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if(Game1.Utility.RGenerator.Next(0, 10) < 6)
                    {
                        TileUtility.ReplaceTile(1, i, j, this.FloorTileID, container);
                        Vector2 tilePosition = container.AllTiles[1][i, j].GetPosition(container);
                        WangManager.GroupReassignForTiling((int)tilePosition.X, (int)tilePosition.Y, this.FloorTileID, floortiling.GeneratableTiles, floortiling.TilingDictionary, 1,  container.ITileManager);
                    }
                    
                   
                }
            }
        }
    }
}

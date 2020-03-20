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
    public class CalciteCamp : Camp
    {


        public CalciteCamp(int probability) : base(probability)
        {
            this.CampType = CampType.Calcite;

            this.SpawnElements = new List<SpawnElement>()
            {
                new SpawnElement(4139, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 15){Unlocked = true }, //Bone idol
                 new SpawnElement(3939, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 15){Unlocked = true }, //Drum
                  new SpawnElement(4141, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 15){Unlocked = true }, //Hide Rack
            };
            this.FloorTileID = 4143;

        }

        public override void Spawn(IInformationContainer container, ILocation location)
        {
            base.Spawn(container, location);
            Vector2 position = new Vector2(container.X * 16 * 16 + 64, container.Y * 16 * 16 + 64);


            location.Enemies.Add(new CalciteWarrior("Calcite", null, position, container.GraphicsDevice, Game1.AllTextures.EnemySpriteSheet, container, CurrentBehaviour.Wander));

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (Game1.Utility.RGenerator.Next(0, 10) < 6)
                    {
                        TileUtility.ReplaceTile(1, i, j, this.FloorTileID, container);
                    }

                }
            }
        }
    }
}

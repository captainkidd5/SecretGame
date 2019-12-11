using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.NPCStuff
{
    public enum NPCType
    {
        Boar = 1,
        Crab = 2,
        Rabbit = 3
    }

    public class NPCGenerator
    {
        public static List<NPCSpawnData> NPCInfo = new List<NPCSpawnData>()
        {   new NPCSpawnData(NPCType.Boar, GenerationType.Dirt, .1f, .75f),
            new NPCSpawnData(NPCType.Crab, GenerationType.Sand, .2f, .25f),
            new NPCSpawnData(NPCType.Rabbit, GenerationType.Dirt, .5f, .2f)


        };

        public IInformationContainer Container { get; set; }
        GraphicsDevice Graphics;

        public NPCGenerator(IInformationContainer container, GraphicsDevice graphics)
        {
            this.Container = container;
            this.Graphics = graphics;
        }

        public void DecrementPackValue(float packValue)
        {
            packValue -= .1f;
        }

        public List<Enemy> SpawnNpcPack(GenerationType tileType)
        {
            List<Enemy> NPCPack = new List<Enemy>();
            NPCSpawnData spawnData = new NPCSpawnData();
            for (int i = 0; i < NPCInfo.Count; i++)
            {
                if(NPCInfo[i].BiomeGenerationType == tileType)
                {
                    spawnData = NPCInfo[i];
                    break;
                }
            }

            if(spawnData == null)
            {
                return null;
            }
            else
            {
                if (DetermineWhetherAtLeastOneSpawns(spawnData.SpawnFrequency))
                {


                    int numberInPack = 0;
                    float spawnFrequency = spawnData.SpawnFrequency;
                    bool flag = true;
                    while(flag)
                    {
                        flag = DetermineNewNPCS(NPCPack, spawnData, spawnFrequency, numberInPack);
                    }
                }
                return NPCPack;
            }


        }

        public bool DetermineWhetherAtLeastOneSpawns(float chance)
        {
            if(Game1.Utility.RFloat(0, 100) < chance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DetermineNewNPCS(List<Enemy> enemyList, NPCSpawnData info,float packFrequency, int numberAlreadyInPack)
        {
            if(Game1.Utility.RFloat(0,100) < info.PackFrequency)
            {
                DecrementPackValue(packFrequency);
                enemyList.Add(info.GetNewEnemy(Graphics, new Vector2(Container.AllTiles[2][8,8].DestinationRectangle.X, Container.AllTiles[2][8, 8].DestinationRectangle.Y), Container));
                return true;
            }
            else
            {
                return false;
            }
        }


    }

    public class NPCSpawnData
    {
        public NPCType Type { get; set; }
        public GenerationType BiomeGenerationType { get; set; }
        public float SpawnFrequency { get; set; }
        public float PackFrequency { get; set; }

        public NPCSpawnData()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">NPC type, seen at the top of NPC Generation Type</param>
        /// <param name="biomeGenerationType">NPC will be allowed to spawn on this tile type</param>
        /// <param name="spawnFrequency">chance of spawning </param>
        /// <param name="packFrequency">chance that at least one more mob will spawn</param>
        public NPCSpawnData(NPCType type, GenerationType biomeGenerationType, float spawnFrequency, float packFrequency)
        {
            this.Type = type;
            this.BiomeGenerationType = biomeGenerationType;
            this.SpawnFrequency = spawnFrequency;
            this.PackFrequency = packFrequency;
        }

        public Enemy GetNewEnemy(GraphicsDevice graphics, Vector2 position, IInformationContainer container)
        {
            switch (Type)
            {
                case NPCType.Boar:
                    
                    return new Boar("boar", position, graphics, Game1.AllTextures.EnemySpriteSheet, container);
                   

                case NPCType.Crab:
                    return new Crab("Crab", position, graphics, Game1.AllTextures.EnemySpriteSheet, container);
  
                case NPCType.Rabbit:
                    return new Rabbit("Rabbit", position, graphics, Game1.AllTextures.EnemySpriteSheet, container);

                default:
                    return null;
            }

        }

    }
}

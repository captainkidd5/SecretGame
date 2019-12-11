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
        public static List<NPCSpawnData> DirtCreatures = new List<NPCSpawnData>()
        {
            new NPCSpawnData(NPCType.Boar, GenerationType.Dirt, 0f, .4f, .75f),
             new NPCSpawnData(NPCType.Rabbit, GenerationType.Dirt, .45f, 8f, .75f),
        };

        public static List<NPCSpawnData> SandCreatures = new List<NPCSpawnData>()
        {
            new NPCSpawnData(NPCType.Crab, GenerationType.Sand, .2f, .5f, .25f)
        };
        public static List<List<NPCSpawnData>> NPCInfo = new List<List<NPCSpawnData>>()
        {
            DirtCreatures,
            SandCreatures
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

        public List<Enemy> SpawnNpcPack(GenerationType tileType, Vector2 position)
        {
            List<Enemy> NPCPack = new List<Enemy>();
            NPCSpawnData spawnData = new NPCSpawnData();
            for (int i = 0; i < NPCInfo.Count; i++)
            {
                
                if(NPCInfo[i][0].BiomeGenerationType == tileType)
                {
                    float testFrequency = Game1.Utility.RFloat(0, 1f);
                    for(int j =0; j < NPCInfo[i].Count; j++)
                    {
                        if (testFrequency >= NPCInfo[i][j].LowerSpawnFrequency && testFrequency <= NPCInfo[i][j].UpperSpawnFrequency)
                        {
                            if (testFrequency > .5f)
                            {
                                Console.WriteLine();
                            }
                            spawnData = NPCInfo[i][j];
                            break;
                        }

                        
                    }
                   
                }
            }

            if(spawnData == null)
            {
                return null;
            }
            else
            {
                float spawnFrequency = spawnData.UpperSpawnFrequency;
                if (DetermineWhetherAtLeastOneSpawns(spawnData.UpperSpawnFrequency))
                {


                    int numberInPack = 0;
                    
                    bool flag = true;
                    while(flag)
                    {
                        flag = DetermineNewNPCS(NPCPack, spawnData, spawnFrequency, numberInPack, position);
                    }
                }
                return NPCPack;
            }


        }

        public bool DetermineWhetherAtLeastOneSpawns(float chance)
        {
            if(Game1.Utility.RFloat(0, 1) < chance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DetermineNewNPCS(List<Enemy> enemyList, NPCSpawnData info,float packFrequency, int numberAlreadyInPack, Vector2 positionToSpawn)
        {
            if(Game1.Utility.RFloat(0,1) < info.PackFrequency)
            {
                DecrementPackValue(packFrequency);
                enemyList.Add(info.GetNewEnemy(Graphics, positionToSpawn, Container));
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
        public float UpperSpawnFrequency { get; set; }
        public float LowerSpawnFrequency { get; set; }
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
        public NPCSpawnData(NPCType type, GenerationType biomeGenerationType, float lowerSpawnFrequency,float upperSpawnFrequency, float packFrequency)
        {
            this.Type = type;
            this.BiomeGenerationType = biomeGenerationType;
            this.UpperSpawnFrequency = upperSpawnFrequency;
            this.LowerSpawnFrequency = lowerSpawnFrequency;
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

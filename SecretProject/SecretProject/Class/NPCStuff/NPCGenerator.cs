using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;

namespace SecretProject.Class.NPCStuff
{
    public enum NPCType
    {
        Boar = 1,
        Crab = 2,
        Rabbit = 3,
        Butterfly = 4,
        WarChicken =5,
        CaveToad = 6,
        SporeShooter = 7
    }

    public class NPCGenerator
    {
        public static List<NPCSpawnData> DirtCreatures = new List<NPCSpawnData>()
        {
            new NPCSpawnData(NPCType.WarChicken, GenerationType.Dirt, 0f, .2f, .75f),
            new NPCSpawnData(NPCType.WarChicken,GenerationType.Dirt, .11f, .3f, .75f),
            new NPCSpawnData(NPCType.Butterfly, GenerationType.Dirt, .11f, .3f, .75f),
            
             new NPCSpawnData(NPCType.Rabbit, GenerationType.Dirt, .3f, .5f, .75f),
             


        };

        public static List<NPCSpawnData> SandCreatures = new List<NPCSpawnData>()
        {
            new NPCSpawnData(NPCType.Crab, GenerationType.Sand, .2f, .5f, .25f)
        };
        public static List<NPCSpawnData> CaveCreatures = new List<NPCSpawnData>()
        {
            new NPCSpawnData(NPCType.SporeShooter, GenerationType.CaveDirt,  0f, .7f, .75f),
            new NPCSpawnData(NPCType.SporeShooter, GenerationType.CaveDirt, .3f, .5f, .75f),
        };
        public static List<List<NPCSpawnData>> NPCInfo = new List<List<NPCSpawnData>>()
        {
            DirtCreatures,
            SandCreatures,
            CaveCreatures
        };



        public IInformationContainer Container { get; set; }
        GraphicsDevice Graphics;

        public NPCGenerator(IInformationContainer container, GraphicsDevice graphics)
        {
            this.Container = container;
            Graphics = graphics;
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

                if (NPCInfo[i][0].BiomeGenerationType == tileType)
                {
                    float testFrequency = Game1.Utility.RFloat(0, 1f);
                    for (int j = 0; j < NPCInfo[i].Count; j++)
                    {
                        if (testFrequency >= NPCInfo[i][j].LowerSpawnFrequency && testFrequency <= NPCInfo[i][j].UpperSpawnFrequency)
                        {
                            //if (testFrequency > .5f)
                            //{
                            //    Console.WriteLine();
                            //}
                            spawnData = NPCInfo[i][j];
                            break;
                        }


                    }

                }
            }

            if (spawnData == null)
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
                    while (flag)
                    {
                        flag = DetermineNewNPCS(NPCPack, spawnData, spawnFrequency, numberInPack, position);
                    }
                }
                return NPCPack;
            }


        }

        public bool DetermineWhetherAtLeastOneSpawns(float chance)
        {
            if (Game1.Utility.RFloat(0, 1) < chance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DetermineNewNPCS(List<Enemy> enemyList, NPCSpawnData info, float packFrequency, int numberAlreadyInPack, Vector2 positionToSpawn)
        {
            if (Game1.Utility.RFloat(0, 1) < info.PackFrequency)
            {
                DecrementPackValue(packFrequency);
                enemyList.Add(info.GetNewEnemy(Graphics, enemyList, positionToSpawn, this.Container));
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
        public NPCSpawnData(NPCType type, GenerationType biomeGenerationType, float lowerSpawnFrequency, float upperSpawnFrequency, float packFrequency)
        {
            this.Type = type;
            this.BiomeGenerationType = biomeGenerationType;
            this.UpperSpawnFrequency = upperSpawnFrequency;
            this.LowerSpawnFrequency = lowerSpawnFrequency;
            this.PackFrequency = packFrequency;
        }

        public Enemy GetNewEnemy(GraphicsDevice graphics, List<Enemy> pack, Vector2 position, IInformationContainer container)
        {
            switch (this.Type)
            {
                case NPCType.Boar:

                    return new Boar("boar", pack, position, graphics, Game1.AllTextures.EnemySpriteSheet, container, CurrentBehaviour.Chase) { HasPackAggression = true } ;


                case NPCType.Crab:
                    return new Crab("Crab", pack, position, graphics, Game1.AllTextures.EnemySpriteSheet, container, CurrentBehaviour.Chase) { HasPackAggression = true };

                case NPCType.Rabbit:
                    return new Rabbit("Rabbit", pack, position, graphics, Game1.AllTextures.EnemySpriteSheet, container, CurrentBehaviour.Flee);

                case NPCType.Butterfly:
                    return new Butterfly("Butterfly", pack, position, graphics, Game1.AllTextures.EnemySpriteSheet, container, CurrentBehaviour.Flee);
                case NPCType.WarChicken:
                    return new WarChicken("WarChicken", pack, position, graphics, Game1.AllTextures.EnemySpriteSheet, container, CurrentBehaviour.Flee) { HasPackAggression = true };
                case NPCType.CaveToad:
                    return new CaveToad("CaveToad", pack, position, graphics, Game1.AllTextures.EnemySpriteSheet, container, CurrentBehaviour.Chase) { HasPackAggression = true };
                case NPCType.SporeShooter:
                    return new SporeShooter("SporeShooter", pack, position, graphics, Game1.AllTextures.EnemySpriteSheet, container, CurrentBehaviour.Wander) { HasPackAggression = true };
                default:
                    return null;
            }

        }

    }
}

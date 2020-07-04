using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.TileStuff;
using SecretProject.Class.TileStuff.SpawnStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;

namespace SecretProject.Class.NPCStuff
{
    public enum NPCType
    {
        boar = 1,
        crab = 2,
        rabbit = 3,
        butterfly = 4,
        warchicken =5,
        cavetoad = 6,
        sporeshooter = 7,
        goat = 8,
        bee = 9
    }

    public class NPCGenerator
    {

        private static List<IWeightable> DirtCreatures = new List<IWeightable>()
        {
            new NPCSpawnData(NPCType.warchicken, GenerationType.Dirt, 20, .8f),
            new NPCSpawnData(NPCType.butterfly, GenerationType.Dirt, 40, .8f),

             new NPCSpawnData(NPCType.rabbit, GenerationType.Dirt, 40, .8f),
             new NPCSpawnData(NPCType.goat, GenerationType.Dirt, 40, .8f),
             new NPCSpawnData(NPCType.bee, GenerationType.Dirt,  40, .8f),


        };

        private static List<IWeightable> SandCreatures = new List<IWeightable>()
        {
            new NPCSpawnData(NPCType.crab, GenerationType.Sand, 30, .25f)
        };
        private static List<IWeightable> CaveCreatures = new List<IWeightable>()
        {
            new NPCSpawnData(NPCType.cavetoad, GenerationType.CaveDirt,  30,  .25f),
            new NPCSpawnData(NPCType.sporeshooter, GenerationType.CaveDirt, 30,  0f),
        };
        private static List<List<IWeightable>> NPCInfo = new List<List<IWeightable>>()
        {
            DirtCreatures,
            SandCreatures,
            CaveCreatures
        };



        private IInformationContainer container;

        private GraphicsDevice Graphics;

        public NPCGenerator(IInformationContainer container, GraphicsDevice graphics)
        {
            this.container = container;
            Graphics = graphics;
            
        }

        private void DecrementPackValue(float packValue)
        {
            packValue -= .1f;
        }

        public List<Enemy> SpawnTargetNPCPack(NPCType type, IInformationContainer container, int numberToSpawn, Vector2 position)
        {
            List<Enemy> NPCPack = new List<Enemy>();

            for(int i =0; i < numberToSpawn; i++)
            {
                NPCPack.Add(NPCSpawnData.GetNewEnemy(type, this.Graphics, NPCPack, position, container));
            }
            return NPCPack;
           
        }

        public List<Enemy> SpawnNpcPack(GenerationType tileType, Vector2 position, IInformationContainer container)
        {
            this.container = container;
            List<Enemy> NPCPack = new List<Enemy>();
            NPCSpawnData spawnData = null;
            for (int i = 0; i < NPCInfo.Count; i++)
            {
                NPCSpawnData data = (NPCInfo[i][0] as NPCSpawnData);
                if ((data.BiomeGenerationType == tileType))
                {
                    spawnData = (NPCSpawnData)WheelSelection.GetSelection(NPCInfo[i]); //get spawnData based on weights.
                    break;

                }
            }

            if (spawnData == null)
            {
                return new List<Enemy>();
            }
            else
            {

                    int numberInPack = 0;

                    bool flag = true;
                    while (flag)
                    {
                        flag = DetermineNewNPCS(NPCPack, spawnData, ref numberInPack, position,container);
                    }
                
                return NPCPack;
            }
        }

        private bool DetermineNewNPCS(List<Enemy> enemyList, NPCSpawnData info, ref int numberAlreadyInPack, Vector2 positionToSpawn, IInformationContainer container)
        {
            if(numberAlreadyInPack == 0)
            {
                enemyList.Add(NPCSpawnData.GetNewEnemy(info.Type, Graphics, enemyList, positionToSpawn, container));
                numberAlreadyInPack++;
                return true;
            }
            if (Game1.Utility.RFloat(0, 1) < info.PackFrequency)
            {
                DecrementPackValue(info.PackFrequency);
                enemyList.Add(NPCSpawnData.GetNewEnemy(info.Type, Graphics, enemyList, positionToSpawn, this.container));
                return true;
            }
            else
            {
                return false;
            }
        }


    }

    public class NPCSpawnData : IWeightable
    {
        public NPCType Type { get; set; }
        public GenerationType BiomeGenerationType { get; set; }
        public float PackFrequency { get; set; }
        public int Chance { get; set; }

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
        public NPCSpawnData(NPCType type, GenerationType biomeGenerationType, int chance, float packFrequency) 
        {
            this.Type = type;
            this.BiomeGenerationType = biomeGenerationType;
            this.Chance = chance;
            this.PackFrequency = packFrequency;
        }

        public static Enemy GetNewEnemy(NPCType type, GraphicsDevice graphics, List<Enemy> pack, Vector2 position, IInformationContainer container)
        {
            switch (type)
            {
                case NPCType.boar:

                    return new Boar( pack, position, graphics, container) { HasPackAggression = true } ;


                case NPCType.crab:
                    return new Crab( pack, position, graphics, container) { HasPackAggression = true };

                case NPCType.rabbit:
                    return new Rabbit( pack, position, graphics, container);

                case NPCType.butterfly:
                    return new Butterfly( pack, position, graphics, container);
                case NPCType.warchicken:
                    return new WarChicken( pack, position, graphics, container) { HasPackAggression = true };
                case NPCType.cavetoad:
                    return new CaveToad(pack, position, graphics, container) { HasPackAggression = true };
                case NPCType.sporeshooter:
                    return new SporeShooter( pack, position, graphics, container) { HasPackAggression = true };
                case NPCType.goat:
                    return new Goat( pack, position, graphics, container) { HasPackAggression = true };
                case NPCType.bee:
                    return new Bee( pack, position, graphics, container) { HasPackAggression = true };
                default:
                    return null;
            }

        }



    }
}

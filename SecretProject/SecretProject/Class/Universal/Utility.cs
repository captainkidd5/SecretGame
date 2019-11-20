using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.Playable;
using SecretProject.Class.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XMLData.DialogueStuff;

namespace SecretProject.Class.Universal
{
    public class Utility
    {
        public int CenterScreenX { get { return Game1.ScreenWidth / 2; } }
        public int CenterScreenY { get { return Game1.ScreenHeight / 2; } }
        public float GlobalButtonScale { get; set; } = 2f;
        public Vector2 centerScreen;
        public Random RGenerator;
        public FastNoise FastNoise;
        public Vector2 Origin { get; set; } = new Vector2(0, 0);
        public Vector2 DialogueTextLocation { get; set; } = new Vector2(Game1.ScreenWidth / 5, (float)(Game1.ScreenHeight - Game1.ScreenHeight / 2.5));
        public Rectangle ItemSourceNullRectangle { get; set; } = new Rectangle(320, 320, 16, 16);


        public float StandardButtonDepth { get; set; } = .7f;
        public float StandardTextDepth { get; set; } = .72f;

        //random tile information
        public List<int> DirtGeneratableTiles;
        public List<int> SandGeneratableTiles;
        public List<int> GrassGeneratableTiles;
        public List<int> WaterGeneratableTiles;
        public List<int> StoneGeneratableTiles { get; set; }
        public List<int> StandardGeneratableDirtTiles;
        public List<int> StandardGeneratableGrassTiles { get; set; }
        public List<int> FenceGeneratableTiles { get; set; }

        public Dictionary<int, int> FenceTiling { get; set; } = new Dictionary<int, int>()
        {
            {0, 456},{1,256}, {2, 455 },  {3, 255}, {4, 453}, {5, 253},{6,454},{7, 254}, {8, 456}, {9, 256}, {10, 455}, {11, 255}, {12,453}, {13,253}, {14,454}, {15, 254}
        };
        //public static Dictionary<int, int> StoneTiling = new Dictionary<int, int>()
        //{
        //    {0, 831},{1,932}, {2, 1031 },  {3, 1030}, {4, 931}, {5, 1028},{6,833},{7, 1029}, {8, 1032}, {9, 832}, {10, 830}, {11, 930}, {12,828}, {13,928}, {14,829}, {15, 929}
        //};


        // public static Color = new Color(100, 100, 100, 100);

        public Utility(int seed)
        {
            RGenerator = new Random(Seed: seed);
            centerScreen = new Vector2(CenterScreenX, CenterScreenY);
            DirtGeneratableTiles = new List<int>();
            SandGeneratableTiles = new List<int>();
            GrassGeneratableTiles = new List<int>();
            WaterGeneratableTiles = new List<int>();
            StoneGeneratableTiles = new List<int>();
            StandardGeneratableDirtTiles = new List<int>();
            StandardGeneratableGrassTiles = new List<int>();
            FenceGeneratableTiles = new List<int>();

            FastNoise = new FastNoise(45);
            FastNoise.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
            FastNoise.SetFractalOctaves(5);
            FastNoise.SetFractalLacunarity(3f);

            //Smaller the smooth the biomes
            FastNoise.SetFractalGain(.5f);

            //larger the smaller the biomes
            FastNoise.SetFrequency(.012f);
           // FastNoise.SetFractalType(FastNoise.FractalType.Billow)

        }



        public bool HasProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }



        public int RNumber(int min, int max)
        {
            return RGenerator.Next(min, max - 1);
        }

        public float RFloat(float min, float max)
        {
            return (float)RGenerator.NextDouble() * (max - min) + min;

        }

        #region TileUtility
        public int[] ParseSpawnsWithKey(string gidString)
        {
            //Regex keyParser = new Regex(@"d{4}\,");
            string[] gids = gidString.Split(',');
            int[] arrayToReturn = new int[gids.Length];
            for (int i = 0; i < gids.Length; i++)
            {
                arrayToReturn[i] = int.Parse(gids[i]);
            }

            return arrayToReturn;
        }

        //For use with the loot property of the tilesheet. Loot objects are separated by commas. The number before the colon is the item id and the number after is the probability of being dropped.
        public List<Loot> Parselootkey(string lootstring)
        {
            string[] commaPairs = lootstring.Split(',');
            int[] Ids = new int[commaPairs.Length];
            int[] probabilitys = new int[commaPairs.Length];
            int[] numberToSpawn = new int[commaPairs.Length];
            List<Loot> lootToReturn = new List<Loot>();
            for (int i = 0; i < commaPairs.Length; i++)
            {


                Ids[i] = int.Parse(commaPairs[i].Split(':')[0]);
                probabilitys[i] = int.Parse(commaPairs[i].Split(':')[1]);
                for(int j = 0; j < int.Parse(commaPairs[i].Split(':')[2]); j++)
                {
                    lootToReturn.Add(new Loot() { ID = Ids[i], Probability = probabilitys[i] });
                }
                
            }
            return lootToReturn;
        }

        public int DetermineLootDrop(Loot loot)
        {
            int amountToReturn = 0;
            int chance = RGenerator.Next(0, 100);
            if (chance <= loot.Probability)
            {
                amountToReturn++;
            }
            return amountToReturn;
        }

        public AnimationType GetRequiredTileTool(string info)
        {
            int toolToReturn = int.Parse(info.Split(',')[0]);
            switch(toolToReturn)
            {
                case -50:
                    return AnimationType.HandsPicking;
                case 0:
                    return AnimationType.HandsPicking;
                case 21:
                    return AnimationType.Chopping;
                case 22:
                    return AnimationType.Mining;

                default: return AnimationType.Mining;
            }
        }
        public int GetTileHitpoints(string info)
        {
            int pointsToReturn = int.Parse(info.Split(',')[1]);
            return pointsToReturn;
        }
        public int GetTileDestructionEffect(string info)
        {
            int effectToReturn = int.Parse(info.Split(',')[2]);
            return effectToReturn;
        }

        public Color GetTileEffectColor(string info)
        {
            switch (int.Parse(info.Split(',')[2]))
            {
                case 0:
                    return Color.White;
                case 1:
                    return Color.WhiteSmoke;
                case 2:
                    return Color.Red;
                case 3:
                    return Color.Green;
                case 4:
                    return Color.Blue;
                case 5:
                    return Color.SaddleBrown;
                default:
                    return Color.White;
            }

        }
        public int GetTileDestructionSound(string info)
        {
            int soundToReturn = int.Parse(info.Split(',')[3]);
            return soundToReturn;
        }

        

        public string[] GetActionHelperInfo(string info)
        {
            string[] stringSize = info.Split(',');
            string[] infoToReturn = new string[stringSize.Length];
            for(int i =0; i < stringSize.Length;i++)
            {
                infoToReturn[i] = info.Split(',')[i];
            }

            return infoToReturn;
        }
        #endregion

        #region LINEUTILITY
        public void DrawLine(Texture2D texture, SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            spriteBatch.Draw(texture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None, 1f);

        }

        public Vector2 RotateVector2(Vector2 pointToRotate,Vector2 origin, float angle)
        {
            Matrix rotationMatrix = Matrix.CreateRotationZ(angle);
            return Vector2.Transform(pointToRotate - origin, rotationMatrix);
        }
        #endregion
        #region SPEECHUTILITY
        public void PerformSpeechAction(string action, int speakerID, string name)
        {
            if(action.Contains('.'))
            {
                int newID = int.Parse(action.Split('.')[1]);
                Game1.Player.UserInterface.TextBuilder.Reset(unfreeze: false);
                DialogueSkeleton skeleton = Game1.DialogueLibrary.RetrieveDialogueNoTime(speakerID, newID);
                Game1.Player.UserInterface.TextBuilder.Skeleton = skeleton;
                Game1.Player.UserInterface.TextBuilder.SpeakerID = speakerID;
                Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, name + ": " + skeleton.TextToWrite, 2f, null, null);
                
             //   Game1.freeze = true;
                return;
            }
            switch(action)
            {
                case "OpenJulianShop":
                    Game1.freeze = true;
                    Game1.Player.UserInterface.IsShopMenu = true;
                    Game1.Player.UserInterface.ActivateShop(OpenShop.JulianShop);
                    Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.ShopMenu;
                    Game1.Player.UserInterface.TextBuilder.Reset();

                    break;
                case "OpenDobbinShop":
                    Game1.freeze = true;
                    Game1.Player.UserInterface.IsShopMenu = true;
                    Game1.Player.UserInterface.ActivateShop(OpenShop.DobbinShop);
                    Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.ShopMenu;
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;

                case "OpenElixirShop":
                    Game1.freeze = true;
                    Game1.Player.UserInterface.IsShopMenu = true;
                    Game1.Player.UserInterface.ActivateShop(OpenShop.ElixirShop);
                    Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.ShopMenu;
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;
                case "OpenKayaShop":
                    Game1.freeze = true;
                    Game1.Player.UserInterface.IsShopMenu = true;
                    Game1.Player.UserInterface.ActivateShop(OpenShop.KayaShop);
                    Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.ShopMenu;
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;

                case "ExitDialogue":
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;

            }
        }
        #endregion
    }
}

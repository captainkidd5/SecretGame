using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.Playable;
using SecretProject.Class.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using XMLData.DialogueStuff;

namespace SecretProject.Class.Universal
{
    public class Utility
    {
        public int CenterScreenX { get { return Game1.ScreenWidth / 2; } }
        public int CenterScreenY { get { return Game1.ScreenHeight / 2; } }
        public float GlobalButtonScale { get; set; } = 2f;
        public float ForeGroundMultiplier { get; set; } = .000001f;
        public Vector2 centerScreen;
        public Random RGenerator;

        public Vector2 Origin { get; set; } = new Vector2(0, 0);
        public Vector2 DialogueTextLocation { get; set; } = new Vector2(Game1.ScreenWidth / 5, (float)(Game1.ScreenHeight - Game1.ScreenHeight / 2.5));
        public Rectangle ItemSourceNullRectangle { get; set; } = new Rectangle(320, 320, 16, 16);


         public const float StandardButtonDepth = .7f;
        public float StandardTextDepth { get; set; } = .72f;

        public Utility(int seed)
        {
            RGenerator = new Random(Seed: seed);
            centerScreen = new Vector2(this.CenterScreenX, this.CenterScreenY);


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





        public AnimationType GetRequiredTileTool(string info)
        {
            int toolToReturn = int.Parse(info.Split(',')[0]);
            switch (toolToReturn)
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
                case 6:
                    return Color.Orange;
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
            for (int i = 0; i < stringSize.Length; i++)
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

        public Vector2 RotateVector2(Vector2 pointToRotate, Vector2 origin, float angle)
        {
            Matrix rotationMatrix = Matrix.CreateRotationZ(angle);
            return Vector2.Transform(pointToRotate - origin, rotationMatrix);
        }
        #endregion
        public Texture2D GetColoredRectangle(GraphicsDevice graphics, int width, int height, Color desiredColor)
        {
            Color[] dataColors = new Color[width * height];
            for (int i = 0; i < dataColors.Length; i++)
            {
                dataColors[i] = desiredColor;
            }
            Texture2D texture = new Texture2D(graphics, width, height);
            texture.SetData(0, new Rectangle(0, 0, width, height), dataColors, 0, width * height);
            return texture;
        }

        public Texture2D GetBorderOnlyRectangleTexture(GraphicsDevice graphicsDevice, int width, int height, Color desiredColor)
        {
            var Colors = new List<Color>();
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    if (x == 0 || //left side
                        y == 0 || //top side
                        x == width - 1 || //right side
                        y == height - 1) //bottom side
                    {
                        Colors.Add(desiredColor);
                    }
                    else
                    {
                        Colors.Add(new Color(0, 0, 0, 0));

                    }

                }
            }
            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            texture.SetData<Color>(Colors.ToArray());
            return texture;
        }
        #region SPEECHUTILITY
        public void PerformSpeechAction(string action, int speakerID, string name)
        {
            if (action.Contains('.'))
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
            switch (action)
            {
                case "OpenJulianShop":
                    Game1.freeze = true;
                    Game1.Player.UserInterface.ActivateShop(OpenShop.JulianShop);
                    Game1.Player.UserInterface.TextBuilder.Reset();

                    break;
                case "OpenDobbinShop":
                    Game1.Player.UserInterface.ActivateShop(OpenShop.DobbinShop);
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;

                case "OpenElixirShop":
                    Game1.Player.UserInterface.ActivateShop(OpenShop.ElixirShop);
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;
                case "OpenKayaShop":
                    Game1.Player.UserInterface.ActivateShop(OpenShop.KayaShop);
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;

                case "OpenBusinessSnailShop":
                    Game1.Player.UserInterface.ActivateShop(OpenShop.BusinessSnailShop);
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;

                case "CheckCurrentProject":

                case "ExitDialogue":
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;

            }
        }
        #endregion
    }
}

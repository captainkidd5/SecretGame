using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Physics;
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
        public GraphicsDevice Graphics { get; set; }
        public int CenterScreenX { get { return Game1.ScreenWidth / 2; } }
        public int CenterScreenY { get { return Game1.ScreenHeight / 2; } }
        public const float GlobalButtonScale = 2f;
        public const float ForeGroundMultiplier = .0001f;
        public const float LayerOffSetMultiplier = .00001f;
        public Vector2 centerScreen;
        public Random RGenerator;

        public readonly Vector2 Origin  = new Vector2(0, 0);
        public Vector2 DialogueTextLocation { get; set; } = new Vector2(Game1.ScreenWidth / 5, (float)(Game1.ScreenHeight - Game1.ScreenHeight / 2.5));
        public Rectangle ItemSourceNullRectangle { get; set; } = new Rectangle(320, 320, 16, 16);


         public const float StandardButtonDepth = .7f;
        public const float StandardTextDepth = .72f;
        public Vector2 ClockPosition { get; set; }




        public Utility(GraphicsDevice graphics, int seed)
        {
            this.Graphics = graphics;
            RGenerator = new Random(Seed: seed);
            centerScreen = new Vector2(this.CenterScreenX, this.CenterScreenY);
            ClockPosition = new Vector2(Game1.ScreenWidth * .9f, Game1.ScreenHeight * .1f);

        }

        public static int GetSquareTile(float num)
        {
            return (int)(num / 16);
        }


        public Vector2 CenterRectangleOnScreen(Rectangle sourceRectangle, float scale)
        {
            return new Vector2(centerScreen.X - sourceRectangle.Width * scale /2, centerScreen.Y - sourceRectangle.Height * scale /2);
        }

        public Vector2 CenterTextOnScreen(SpriteFont font, string text, float scale)
        {
            Vector2 fontLength = font.MeasureString(text);
            return new Vector2(centerScreen.X - fontLength.X * scale / 2, CenterScreenY - fontLength.Y * scale / 2);
        }

        public Vector2 CenterTextOnRectangle(SpriteFont font,Vector2 rectangleCenter, string text, float scale)
        {
            Vector2 fontLength = font.MeasureString(text);
            return new Vector2(rectangleCenter.X - fontLength.X * scale / 2, rectangleCenter.Y - fontLength.Y * scale / 2);
        }

        public Vector2 CenterOnTopRightCorner(Rectangle sourceRectangle,Rectangle rectangleToPlace, Vector2 position, float scale)
        {
            return new Vector2(position.X + sourceRectangle.Width * scale - rectangleToPlace.Width * scale, position.Y);
        }

        public Vector2 CenterRectangleInRectangle(Rectangle sourceRectangle, Rectangle rectangleToPlace, Vector2 sourcePosition, float scaleBig, float scaleSmall)
        {
            return new Vector2(sourcePosition.X + (sourceRectangle.Width * scaleBig / 2 - rectangleToPlace.Width / 2 * scaleSmall), sourcePosition.Y + (sourceRectangle.Height * scaleBig / 2 - rectangleToPlace.Height / 2 * scaleSmall));
        }

        public Vector2 CenterRectangleInRectangleLowerThird(Rectangle sourceRectangle, Rectangle rectangleToPlace, Vector2 sourcePosition, float scale)
        {
            return new Vector2(sourcePosition.X + (sourceRectangle.Width * scale / 2 - rectangleToPlace.Width / 2 * scale), sourcePosition.Y + (sourceRectangle.Height * scale - rectangleToPlace.Height  * scale));
        }

        public Vector2 GetCenterOfRectangle(Rectangle sourceRectangle, Vector2 rectanglePosition, float scale)
        {
            return new Vector2(rectanglePosition.X + sourceRectangle.Width / 2 * scale, rectanglePosition.Y + sourceRectangle.Height / 2 * scale);
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

        /// <summary>
        /// Returns 1 or -1 at a 50% chance
        /// </summary>
        /// <returns></returns>
        public int GetMultiplier()
        {
            return RGenerator.Next(0, 2) * 2 - 1;
        }

        public Vector2 GetDistanceBetweenTwoVectors(Vector2 entity1, Vector2 entity2)
        {
            return entity1 - entity2;
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
            return (AnimationType)Enum.Parse(typeof(AnimationType), info.Split(',')[0]);
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

        

        public enum ItemTier
        {
            Tier0 = 0,
            Tier1 = 1,
            Tier2 = 2,
            Tier3 = 3,
            Tier4 = 4,
            Tier5 = 5,
        }


        public bool GetTileTierRequired(string[] info)
        {
            if(info.Length > 4)
            {
                int tierRequired = (int)Enum.Parse(typeof(ItemTier), info[5]);
                if(Game1.Player.GetCurrentEquippedToolData().Tier >= tierRequired)
                {
                    return true;
                }
                else
                {
                    Game1.Player.UserInterface.AddAlert(AlertType.Normal,  Game1.Utility.centerScreen, "Higher level tool required!");
                    return false;
                }
            }
            return true;
        }

        public int GetTileDestructionSound(string[] info, bool isFinalHit = false)
        {
            int index = 3;
            if(isFinalHit)
            {
                if(info.Length > 4)
                {
                    index = 4;
                }
                
            }
            int soundToReturn = int.Parse(info[index]);
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

        /// <summary>
        /// Gives the angle between two vectors in degrees
        /// </summary>
        /// <param name="mainEntity">entity checking from</param>
        /// <param name="entity2">entity chaking to</param>
        /// <returns></returns>
        public float GetAngleBetweenTwoVectors(Vector2 mainEntity, Vector2 entity2)
        {
            Vector2 direction = mainEntity - entity2;


            // Vector2 direction = Game1.myMouseManager.WorldMousePosition - positionToCheck;
            float angle = MathHelper.ToDegrees((float)(Math.Atan2(direction.X, direction.Y)) * -1);
            if (angle < 0)
            {
                angle = 360 - Math.Abs(angle);
            }


            return angle;
        }
        #endregion
        public Texture2D GetColoredRectangle(int width, int height, Color desiredColor)
        {
            Color[] dataColors = new Color[width * height];
            for (int i = 0; i < dataColors.Length; i++)
            {
                dataColors[i] = desiredColor;
            }
            Texture2D texture = new Texture2D(this.Graphics, width, height);
            texture.SetData(0, new Rectangle(0, 0, width, height), dataColors, 0, width * height);
            return texture;
        }

        public Texture2D GetColoredCircle(float radius, Color desiredColor)
        {
            radius = radius / 2;
            int width = (int)radius * 2;
            int height = width;

            Vector2 center = new Vector2(radius, radius);

            Circle circle = new Circle(center, radius,false);

            Color[] dataColors = new Color[width * height];
            int row = -1;
            int column = 0;
            for (int i = 0; i < dataColors.Length; i++)
            {
                column++;
                if(i % width == 0)
                {
                    row++;
                    column = 0;
                }
                Vector2 point = new Vector2(row, column);
                if(circle.ContainsPoint(point))
                {
                    dataColors[i] = desiredColor;
                }
                else
                {
                    dataColors[i] = Color.Transparent;
                }
                
            }
            Texture2D texture = new Texture2D(this.Graphics, width, height);
            texture.SetData(0, new Rectangle(0, 0, width, height), dataColors, 0, width * height);
            return texture;
        }

        public Rectangle GetRectangleFromTexture(Texture2D texture)
        {
            return new Rectangle(texture.Width, texture.Height, texture.Width, texture.Height);
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
        

        /// <summary>
        /// If number is less than zero, make it zero and return false, meaning the number provided was not originally positive
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool EnsurePositive(ref int number)
        {
            if(number >= 0)
            {
                return true;
            }
            number = 0;
            return false;
        }

        /// <summary>
        /// If number is less than or equal to max return false, else return true and set the number equal to max,
        /// meaning that the number was indeed greater than the max.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public bool EnsureNoMoreThanMax(ref int number, int max)

        {
            if(number <= max)
            {
                return false;

            }
            number = max;          
            return true;
        }


    }
}

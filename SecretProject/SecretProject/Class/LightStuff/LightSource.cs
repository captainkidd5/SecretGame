using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SecretProject.Class.LightStuff
{
    public enum LightType
    {
        DayTime = 1,
        NightTime = 2
    }

    public enum LightColor
    {
        None = 0,
        White = 1,
        Yellow = 2,
        Orange = 3
    }

    public class LightSource
    {
        public LightType LightType { get; set; }
        public Texture2D LightTexture { get; set; }
        public Vector2 Position;
        public bool IsActive { get; set; } = false;

        public LightSource(string data, Vector2 position)
        {
            this.LightTexture = GetTextureFromID(int.Parse(data.Split(',')[0]));
            Position = new Vector2(position.X + 8 - this.LightTexture.Width / 2, position.Y - this.LightTexture.Height / 2);
            //this.LightType = GetLightType(data);
            
        }

        private Texture2D GetTextureFromID(int id)
        {
            switch (id)
            {
                case 1:
                    return Game1.AllTextures.lightMask;
                default:
                    return Game1.AllTextures.lightMask;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.LightTexture, Position, Color.White);
        }

        public static Color GetLightType(string data)
        {

           // return (LightColor)Enum.Parse(typeof(LightColor), data.Split(',')[3]);
            switch((LightColor)Enum.Parse(typeof(LightColor), data.Split(',')[3]))
            {
                case LightColor.Orange:
                    return Color.Orange;
                case LightColor.White:
                    return Color.White;
                case LightColor.Yellow:
                    return Color.Yellow;
                default:
                    return Color.White;
            }
        }

        public static Vector2 ParseLightData(string data)
        {
            int x = int.Parse(data.Split(',')[1]);
            int y = int.Parse(data.Split(',')[2]);
            return new Vector2(x, y);
        }
    }
}

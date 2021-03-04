using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretProject.Class.Universal
{
    public enum LayerDepths
    {
        Zero = 0,
        Background = 1,
        Sprites = 2,
    }
    public static class Globals
    {

        public static bool IsMouseVisible = false;
        #region SCREEN
        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;
        public static int GameScale = 1;

        public static float AspectRatio = (float)ScreenWidth / (float)ScreenHeight;
        public static float PreferredAspect = (float)1280 / (float)720;
        public static Rectangle ScreenRectangle;

        public static bool IsFadingOut = false;

        /// <summary>
        /// Will fill in black bars on the top and bottom of the screen (like movies do!) so that
        /// the aspect ratio is maintined.
        /// </summary>
        public static Rectangle GetScreenRectangle()
        {
            Rectangle screenRectangle;
            if (AspectRatio <= PreferredAspect)
            {
                // output is taller than it is wider, bars on top/bottom
                int presentHeight = (int)((ScreenWidth / PreferredAspect) + 0.5f);
                int barHeight = (ScreenHeight - presentHeight) / 2;
                screenRectangle = new Rectangle(0, barHeight, ScreenWidth, presentHeight);
            }
            else
            {
                // output is wider than it is tall, bars left/right
                int presentWidth = (int)((ScreenHeight * PreferredAspect) + 0.5f);
                int barWidth = (ScreenWidth - presentWidth) / 2;
                screenRectangle = new Rectangle(barWidth, 0, presentWidth, ScreenHeight);
            }
            return screenRectangle;
        }
        public static Vector2 CenterScreen()
        {
            return new Vector2(ScreenWidth / 2, ScreenHeight / 2);
        }

        #endregion


        public static Random Random = new Random(100);

        #region DEBUG
        public static bool EnableVelcroDraw;
        #endregion

        #region LAYERING
        public static float LayerMultiplier = .00001f;
        public static float defaultForeGroundLayer = .3f;
        public static float GetLayerDepth(LayerDepths layerDepths)
        {
            return (float)layerDepths * .1f;
        }

        public static float GetYAxisLayerDepth(Vector2 position, Rectangle sourceRectangle)
        {
            float depth = defaultForeGroundLayer + (position.Y + sourceRectangle.Height) * LayerMultiplier;

            return depth;
        }
        /// <summary>
        /// Returns a float value which is at least slightly larger than the given layerDepth, and at most .099 greater than the value.
        /// If a dictionary is provided, it will make sure that the value is not already contained within the dictionary.
        /// </summary>
        /// <param name="dictionary">optional dictionary to search through.</param>
        public static float GetSpriteVariedLayerDepth(LayerDepths layerDepths, Dictionary<string, float> dictionary = null)
        {
            float variedLayerDepth = GetLayerDepth(layerDepths) + Random.Next(1, 999) * LayerMultiplier;
            if (dictionary != null)
            {
                if (dictionary.ContainsValue(variedLayerDepth))
                {
                    return GetSpriteVariedLayerDepth(layerDepths, dictionary);
                }
                return variedLayerDepth;
            }
            return variedLayerDepth;
        }
        #endregion

        public static Vector2 GetScreenBottomRight()
        {
            return new Vector2(ScreenWidth, ScreenHeight);
        }

        #region MULTIPLAYER
        public static bool EnableMultiplayer;
        public static bool IsHost;
        public static byte ConnectedPlayers = 1;
        //how often server updates clients (seconds)
        public static float MFrequency = .01f;
        #endregion

        #region TILES
        public static bool ShowTileSelector = true;
        #endregion

    }
}

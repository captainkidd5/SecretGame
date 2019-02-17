using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TiledSharp;

namespace SecretProject.Class.TextureStuff
{
   public class TextureBook
    {
        public Texture2D JoeSprite;
        public Texture2D joeDown;
        public Texture2D joeUp;
        public Texture2D joeRight;
        public Texture2D joeLeft;

        public Texture2D MainCharacterSpriteStrip;



        //playeractions
        public Texture2D CutGrassDown;
        public Texture2D CutGrassRight;
        public Texture2D CutGrassLeft;
        public Texture2D CutGrassUp;


        //UiStuff
        public Texture2D ToolBarButtonSelector;
        public Texture2D CursorWhiteHand;

        //Items
        public Texture2D Secateurs;

        

        //Maps
        public TmxMap Iliad;

        public TmxMap LodgeInterior;

        //TileSets
        public Texture2D MasterTileSet;
        public Texture2D LodgeInteriorTileSet;

        public TextureBook(ContentManager content, SpriteBatch spriteBatch)
        {
            JoeSprite = content.Load<Texture2D>("Player/Joe/joe");

            joeDown = content.Load<Texture2D>("Player/Joe/JoeWalkForwardNew");
            joeUp = content.Load<Texture2D>("Player/Joe/JoeWalkBackNew");
            joeRight = content.Load<Texture2D>("Player/Joe/JoeWalkRightNew");
            joeLeft = content.Load<Texture2D>("Player/Joe/JoeWalkLefttNew");

            MainCharacterSpriteStrip = content.Load<Texture2D>("Player/MainPlayer/newPlayer");

            CutGrassDown = content.Load<Texture2D>("Player/MainPlayer/ClippingForward");
            CutGrassRight = content.Load<Texture2D>("Player/MainPlayer/ClippingRight");
            CutGrassLeft = content.Load<Texture2D>("Player/MainPlayer/ClippingLeft");
            CutGrassUp = content.Load<Texture2D>("Player/MainPlayer/ClippingUp");

            ToolBarButtonSelector = content.Load<Texture2D>("Button/ToolBarButtonSelector");
            CursorWhiteHand = content.Load<Texture2D>("Button/Cursor1");

            Secateurs = content.Load<Texture2D>("Item/secateurs");

            Iliad = new TmxMap("Content/Map/worldMap.tmx");

            LodgeInterior = new TmxMap("Content/Map/lodgeInterior.tmx");

            MasterTileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");
            LodgeInteriorTileSet = content.Load<Texture2D>("Map/InteriorSpriteSheet1");
           

        }

    }
}

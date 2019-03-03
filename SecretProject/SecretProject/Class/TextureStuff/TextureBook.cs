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
        public Texture2D TransparentTextBox;
        public Texture2D TileSelector;
        public Texture2D ClockBackground;

        //Fonts
        public SpriteFont MenuText;

        //Items
        public Texture2D Secateurs;
        public Texture2D lodgeKey;
        public Texture2D shovel;
        public Texture2D stone;

        public Texture2D pie;
        public Texture2D puzzleFish;
        public Texture2D grass;
        public Texture2D barrel;

        

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
            TransparentTextBox = content.Load<Texture2D>("Button/transparentTextBox");
            TileSelector = content.Load<Texture2D>("Button/tileSelector");
            ClockBackground = content.Load<Texture2D>("Button/clockBackground");

            MenuText = content.Load<SpriteFont>("SpriteFont/MenuText");


            Secateurs = content.Load<Texture2D>("Item/secateurs");
            lodgeKey = content.Load<Texture2D>("Item/HouseKey");
            shovel = content.Load<Texture2D>("Item/Shovel");
            stone = content.Load<Texture2D>("Item/stone");
            pie = content.Load<Texture2D>("Item/pie");
            puzzleFish = content.Load<Texture2D>("Item/puzzleFish");
            grass = content.Load<Texture2D>("Item/grass");
            barrel = content.Load<Texture2D>("Item/Barrel");

            Iliad = new TmxMap("Content/Map/worldMap.tmx");

            LodgeInterior = new TmxMap("Content/Map/lodgeInterior.tmx");

            MasterTileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");
            LodgeInteriorTileSet = content.Load<Texture2D>("Map/InteriorSpriteSheet1");
           

        }

    }
}

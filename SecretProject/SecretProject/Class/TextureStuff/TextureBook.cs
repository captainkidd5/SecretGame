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

        public Texture2D Elixer;
        public Texture2D ElixirSpriteSheet;



        //playeractions
        public Texture2D CutGrassDown;
        public Texture2D CutGrassRight;
        public Texture2D CutGrassLeft;
        public Texture2D CutGrassUp;

        public Texture2D MiningDown;
        public Texture2D MiningRight;
        public Texture2D MiningLeft;
        public Texture2D MiningUp;

        public Texture2D ChoppingDown;
        public Texture2D ChoppingRight;
        public Texture2D ChoppingLeft;
        public Texture2D ChoppingUp;

        //UiStuff
        public Texture2D ToolBarButtonSelector;
        public Texture2D CursorWhiteHand;
        public Texture2D CursorPlant;
        public Texture2D TransparentTextBox;
        public Texture2D TileSelector;
        public Texture2D ClockBackground;

        public Texture2D ShopMenu;
        public Texture2D ShopMenuItemButton;
        public Texture2D RedEsc;

        public Texture2D BasicButton;

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
        public Texture2D redOrb;
        public Texture2D blueOrb;

        public Texture2D Axe;

        

        //Maps
        public TmxMap Iliad;
        public TmxMap RoyalDocks;

        public TmxMap LodgeInterior;

        public TmxMap Sea;

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

            Elixer = content.Load<Texture2D>("NPC/ElixerTest");
            ElixirSpriteSheet = content.Load<Texture2D>("NPC/Elixir/ElixirSpriteSheet");

            MainCharacterSpriteStrip = content.Load<Texture2D>("Player/MainPlayer/newPlayer");

            CutGrassDown = content.Load<Texture2D>("Player/MainPlayer/ClippingForward");
            CutGrassRight = content.Load<Texture2D>("Player/MainPlayer/ClippingRight");
            CutGrassLeft = content.Load<Texture2D>("Player/MainPlayer/ClippingLeft");
            CutGrassUp = content.Load<Texture2D>("Player/MainPlayer/ClippingUp");

            MiningDown = content.Load<Texture2D>("Player/MainPlayer/MiningDown");
            MiningRight = content.Load<Texture2D>("Player/MainPlayer/MiningRight");
            MiningLeft = content.Load<Texture2D>("Player/MainPlayer/MiningLeft");
            MiningUp = content.Load<Texture2D>("Player/MainPlayer/MiningUp");

            ChoppingDown = content.Load<Texture2D>("Player/MainPlayer/ChoppingDown");
            ChoppingRight = content.Load<Texture2D>("Player/MainPlayer/ChoppingRight");
            ChoppingLeft = content.Load<Texture2D>("Player/MainPlayer/ChoppingLeft");
            ChoppingUp = content.Load<Texture2D>("Player/MainPlayer/ChoppingUp");

            ToolBarButtonSelector = content.Load<Texture2D>("Button/ToolBarButtonSelector");
            CursorWhiteHand = content.Load<Texture2D>("Button/Cursor1");
            CursorPlant = content.Load<Texture2D>("Button/PlantCursor");
            TransparentTextBox = content.Load<Texture2D>("Button/transparentTextBox");
            TileSelector = content.Load<Texture2D>("Button/tileSelector");
            ClockBackground = content.Load<Texture2D>("Button/clockBackground");

            ShopMenu = content.Load<Texture2D>("Button/shopMenu");
            ShopMenuItemButton = content.Load<Texture2D>("Button/shopMenuItemButton");
            RedEsc = content.Load<Texture2D>("Button/redEsc");

            BasicButton = content.Load<Texture2D>("Button/basicButton");

            MenuText = content.Load<SpriteFont>("SpriteFont/MenuText");


            Secateurs = content.Load<Texture2D>("Item/secateurs");
            lodgeKey = content.Load<Texture2D>("Item/HouseKey");
            shovel = content.Load<Texture2D>("Item/Shovel");
            stone = content.Load<Texture2D>("Item/stone");
            pie = content.Load<Texture2D>("Item/pie");
            puzzleFish = content.Load<Texture2D>("Item/puzzleFish");
            grass = content.Load<Texture2D>("Item/grass");
            barrel = content.Load<Texture2D>("Item/Barrel");
            redOrb = content.Load<Texture2D>("Item/redOrb");
            blueOrb = content.Load<Texture2D>("Item/blueOrb");
            Axe = content.Load<Texture2D>("Item/axe");
           // Iliad = new TmxMap("Content/Map/worldMap.tmx");

           // LodgeInterior = new TmxMap("Content/Map/lodgeInterior.tmx");
           // RoyalDocks = new TmxMap("Content/Map/royalDocks.tmx");

           // Sea = new TmxMap("Content/Map/sea.tmx");


            MasterTileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");
            LodgeInteriorTileSet = content.Load<Texture2D>("Map/InteriorSpriteSheet1");
           

        }

    }
}

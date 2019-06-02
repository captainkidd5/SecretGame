using Microsoft.Xna.Framework;
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
        public Texture2D MainCharacterSpriteStrip;
        public Texture2D PlayerSpriteSheet;

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

        //Maps
        public TmxMap Iliad;
        public TmxMap RoyalDocks;

        public TmxMap LodgeInterior;

        public TmxMap Sea;

        //TileSets
        public Texture2D MasterTileSet;
        public Texture2D LodgeInteriorTileSet;

        public Texture2D ItemSpriteSheet;

        public TextureBook(ContentManager content, SpriteBatch spriteBatch)
        {
            PlayerSpriteSheet = content.Load<Texture2D>("Player/MainPlayer/PlayerSpriteSheet");

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

           // Iliad = new TmxMap("Content/Map/worldMap.tmx");

           // LodgeInterior = new TmxMap("Content/Map/lodgeInterior.tmx");
           // RoyalDocks = new TmxMap("Content/Map/royalDocks.tmx");

           // Sea = new TmxMap("Content/Map/sea.tmx");


            MasterTileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");
            LodgeInteriorTileSet = content.Load<Texture2D>("Map/InteriorSpriteSheet1");

            ItemSpriteSheet = content.Load<Texture2D>("Item/ItemSpriteSheet");
           

        }

        public Rectangle GetItemTextureFromAtlas(int row, int column)
        {
            int width = 16;
            int height = 16;

            return new Rectangle((int)column * width, (int)row * height, width, height);
        }

    }
}

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


        //UiStuff
        public Texture2D ToolBarButtonSelector;
        public Texture2D CursorWhiteHand;
        public Texture2D CursorPlant;
        public Texture2D TransparentTextBox;
        public Texture2D TileSelector;
        public Texture2D ClockBackground;

        public Texture2D UserInterfaceTileSet;

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

        public Texture2D RockParticle;

        public Texture2D ShipSpriteSheet;



        //Effects
        public Effect testEffect;
        public Effect practiceLightMaskEffect;
        public Texture2D lightMask;

        public TextureBook(ContentManager content, SpriteBatch spriteBatch)
        {
            PlayerSpriteSheet = content.Load<Texture2D>("Player/MainPlayer/PlayerSpriteSheet");

            Elixer = content.Load<Texture2D>("NPC/ElixerTest");
            ElixirSpriteSheet = content.Load<Texture2D>("NPC/Elixir/ElixirSpriteSheet");

            MainCharacterSpriteStrip = content.Load<Texture2D>("Player/MainPlayer/newPlayer");


            ToolBarButtonSelector = content.Load<Texture2D>("Button/ToolBarButtonSelector");
            CursorWhiteHand = content.Load<Texture2D>("Button/Cursor1");
            CursorPlant = content.Load<Texture2D>("Button/PlantCursor");


            MenuText = content.Load<SpriteFont>("SpriteFont/MenuText");


            MasterTileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");
            LodgeInteriorTileSet = content.Load<Texture2D>("Map/InteriorSpriteSheet1");

            ItemSpriteSheet = content.Load<Texture2D>("Item/ItemSpriteSheet");

            RockParticle = content.Load<Texture2D>("Particles/rockparticle");

            UserInterfaceTileSet = content.Load<Texture2D>("Button/userinterfaceTileSet");
            ShipSpriteSheet = content.Load<Texture2D>("Player/Ship/ShipSpriteSheet");

            testEffect = content.Load<Effect>("Effects/lightSpriteEffect1");
            lightMask = content.Load<Texture2D>("Effects/lightmask");
            practiceLightMaskEffect = content.Load<Effect>("Effects/practiceLighting1");

        }

        public Rectangle GetItemTextureFromAtlas(int row, int column)
        {
            int width = 16;
            int height = 16;

            return new Rectangle((int)column * width, (int)row * height, width, height);
        }

    }
}

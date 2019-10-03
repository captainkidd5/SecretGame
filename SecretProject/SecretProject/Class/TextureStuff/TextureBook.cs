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
        //Player
        public Texture2D PlayerBase;
        public Texture2D PlayerHair;
        public Texture2D PlayerShirt;
        public Texture2D PlayerPants;
        public Texture2D PlayerShoes;
        //NPCS
        public Texture2D Elixer;
        public Texture2D ElixirSpriteSheet;
        public Texture2D DobbinSpriteSheet;
        public Texture2D EnemySpriteSheet;

        public Texture2D KayaSpriteSheet;

        public Texture2D SnawSpriteSheet;
        public Texture2D JulianSpriteSheet;

        //playeractions


        //UiStuff

        public Texture2D CursorWhiteHand;
        public Texture2D CursorPlant;


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
        public Texture2D InteriorTileSet1;
        public Texture2D OrchardTileSet;

        public Texture2D ItemSpriteSheet;

        public Texture2D RockParticle;



        //Effects

        public Effect practiceLightMaskEffect;
        public Effect nightTint;
        public Texture2D lightMask;

        //Debug
        public Texture2D redPixel;

        //Props
        public Texture2D Gondola;
        public Texture2D TallGrass;
        public Texture2D Gears;


        //BackDrops
        public Texture2D WildernessBackdrop;




        public TextureBook(ContentManager content, SpriteBatch spriteBatch)
        {
            //Player
            PlayerBase = content.Load<Texture2D>("Player/PlayerParts/Base/base");
          PlayerHair = content.Load<Texture2D>("Player/PlayerParts/Hair/blondeSpikyHair");
            PlayerShirt = content.Load<Texture2D>("Player/PlayerParts/Pants/bluePants");
            PlayerPants = content.Load<Texture2D>("Player/PlayerParts/Shirts/redShirt");
            PlayerShoes = content.Load<Texture2D>("Player/PlayerParts/Shoes/brownShoes");


            //NPC
            Elixer = content.Load<Texture2D>("NPC/ElixerTest");
            ElixirSpriteSheet = content.Load<Texture2D>("NPC/Elixir/ElixirSpriteSheet");


            DobbinSpriteSheet = content.Load<Texture2D>("NPC/Dobbin/DobbinSpriteSheet");

            SnawSpriteSheet = content.Load<Texture2D>("NPC/Snaw/Snaw");


            EnemySpriteSheet = content.Load<Texture2D>("NPC/Enemy/EnemySpriteSheet");

            KayaSpriteSheet = content.Load<Texture2D>("NPC/Kaya/KayaSpriteSheet");
            JulianSpriteSheet = content.Load<Texture2D>("NPC/Julian/Julian");


            CursorWhiteHand = content.Load<Texture2D>("Button/Cursor1");
            CursorPlant = content.Load<Texture2D>("Button/PlantCursor");


            MenuText = content.Load<SpriteFont>("SpriteFont/MenuText");


            //MasterTileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");
            InteriorTileSet1 = content.Load<Texture2D>("Map/InteriorSpriteSheet1");
            //OrchardTileSet = content.Load<Texture2D>("Map/orchard");

            ItemSpriteSheet = content.Load<Texture2D>("Item/ItemSpriteSheet");

            RockParticle = content.Load<Texture2D>("Particles/rockparticle");

            UserInterfaceTileSet = content.Load<Texture2D>("Button/userinterfaceTileSet");

            lightMask = content.Load<Texture2D>("Effects/lightmask");
            practiceLightMaskEffect = content.Load<Effect>("Effects/practiceLighting1");
            nightTint = content.Load<Effect>("Effects/nightTint");

            redPixel = content.Load<Texture2D>("Debug/solidRed");

            //MINING FOLDER

            

            //props
            Gondola = content.Load<Texture2D>("WorldProps/Gondola");
            TallGrass = content.Load<Texture2D>("WorldProps/tallGrass");
            Gears = content.Load<Texture2D>("WorldProps/Gears");


            //backdrops
            WildernessBackdrop = content.Load<Texture2D>("BackDrops/wilderness");
        }


        public Rectangle GetItemTextureFromAtlas(int row, int column)
        {
            int width = 16;
            int height = 16;

            return new Rectangle((int)column * width, (int)row * height, width, height);
        }

    }
}

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
        public Texture2D PlayerParts;
        public Texture2D PlayerPartsMining;

        //MINING FOLDER
        //ARMS MINING
        public Texture2D RightArmMining;

        //HANDS MINING
        public Texture2D HandsMining;

        //HEAD MINING
        public Texture2D HeadMining;

        //LEGS MINING
        public Texture2D BasicLegsMining;

        //SHOES MINING
        public Texture2D ShoesMining;

        //TOOL MINING
        public Texture2D BasicHammerMining;

        //TORSO MINING
        public Texture2D TorsoBlueMining;

        public Texture2D Elixer;
        public Texture2D ElixirSpriteSheet;
        public Texture2D DobbinSpriteSheet;
        public Texture2D EnemySpriteSheet;

        public Texture2D KayaSpriteSheet;

        public Texture2D SnawSpriteSheet;

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


        //BackDrops
        public Texture2D WildernessBackdrop;



        public TextureBook(ContentManager content, SpriteBatch spriteBatch)
        {
            PlayerSpriteSheet = content.Load<Texture2D>("Player/MainPlayer/PlayerSpriteSheet");
            PlayerParts = content.Load<Texture2D>("Player/MainPlayer/playerParts");
            PlayerPartsMining = content.Load<Texture2D>("Player/MainPlayer/playerPartsMining");

            Elixer = content.Load<Texture2D>("NPC/ElixerTest");
            ElixirSpriteSheet = content.Load<Texture2D>("NPC/Elixir/ElixirSpriteSheet");


            DobbinSpriteSheet = content.Load<Texture2D>("NPC/Dobbin/DobbinSpriteSheet");

            SnawSpriteSheet = content.Load<Texture2D>("NPC/Snaw/Snaw");

            MainCharacterSpriteStrip = content.Load<Texture2D>("Player/MainPlayer/newPlayer");

            EnemySpriteSheet = content.Load<Texture2D>("NPC/Enemy/EnemySpriteSheet");

            KayaSpriteSheet = content.Load<Texture2D>("NPC/Kaya/KayaSpriteSheet");


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

            RightArmMining = content.Load<Texture2D>("Player/MainPlayer/Mining/ArmsMining/rightArmMining");

            HandsMining = content.Load<Texture2D>("Player/MainPlayer/Mining/HandsMining/handsMining");

            //HEAD MINING
            HeadMining = content.Load<Texture2D>("Player/MainPlayer/Mining/HeadMining/HeadMining");

            //LEGS MINING
            BasicLegsMining = content.Load<Texture2D>("Player/MainPlayer/Mining/LegsMining/basicLegsMining");

            //SHOES MINING
            ShoesMining = content.Load<Texture2D>("Player/MainPlayer/Mining/ShoesMining/ShoesMining");

            //TOOL MINING
            BasicHammerMining = content.Load<Texture2D>("Player/MainPlayer/Mining/ToolMining/basicHammerMining");

            //TORSO MINING
            TorsoBlueMining = content.Load<Texture2D>("Player/MainPlayer/Mining/TorsoMining/torsoBlueMining");

            //props
            Gondola = content.Load<Texture2D>("WorldProps/Gondola");

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

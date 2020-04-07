using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TiledSharp;

namespace SecretProject.Class.TextureStuff
{
    public enum EmoticonType
    {
        ExclamationPoint = 1
    }

    public class TextureBook
    {
        //Player

        public Texture2D EyesAtlas;
        public Texture2D PlayerBaseAtlas;
        public Texture2D ShirtAtlas;
        public Texture2D ShoesAtlas;
        public Texture2D HairAtlas;
        public Texture2D PantsAtlas;
        public Texture2D ArmsAtlas;

        public Texture2D PlayerBase;
        public Texture2D PlayerHair;
        public Texture2D PlayerShirt;
        public Texture2D PlayerPants;
        public Texture2D PlayerShoes;

        //CHOPPING TOOLS
        public Texture2D ChoppingTestTool;
        public Texture2D IronAxeTool;
        public Texture2D ChoppingToolAtlas;

        public Texture2D ChoppingPlayerBase;
        public Texture2D ChoppingPlayerHair;
        public Texture2D ChoppingPlayerShirt;
        public Texture2D ChoppingPlayerPants;
        public Texture2D ChoppingPlayerShoes;

        public Texture2D SwipingPlayerBase;

        public Texture2D SwipingPlayerShirt;


        public Texture2D PickUpItemBase;
        public Texture2D PickUpItemBlondeHair;
        public Texture2D PickUpItemBluePants;
        public Texture2D PickUpItemRedShirt;
        public Texture2D PickUpItemBrownShoes;

        public Texture2D portalJumpBase;
        public Texture2D portalJumpHair;
        public Texture2D portalJumpPants;
        public Texture2D portalJumpShirt;
        public Texture2D portalJumpShoes;

        //NPCS
        public Texture2D Elixer;
        public Texture2D ElixirSpriteSheet;
        public Texture2D ElixirPortrait;


        public Texture2D DobbinSpriteSheet;
        public Texture2D DobbinPortrait;

        public Texture2D EnemySpriteSheet;
        //BOSSES
        public Texture2D Carotar;
        public Texture2D CarotarShadow;

        public Texture2D Nelja;


        public Texture2D KayaSpriteSheet;
        public Texture2D KayaPortrait;

        public Texture2D SnawSpriteSheet;
        public Texture2D SnawPortrait;

        public Texture2D JulianSpriteSheet;
        public Texture2D JulianPortrait;

        public Texture2D SarahSpriteSheet;
        public Texture2D SarahPortrait;

        public Texture2D BusinessSnail;
        public Texture2D BusinessSnailPortrait;

        public Texture2D Mippin;
        public Texture2D MippinPortrait;

        public Texture2D Ned;
        public Texture2D NedPortrait;

        public Texture2D Teal;
        public Texture2D TealPortrait;

        public Texture2D Marcus;
        public Texture2D MarcusPotrait;

        public Texture2D Caspar;
        public Texture2D CasparPortrait;


        //playeractions


        //UiStuff



        public Texture2D UserInterfaceTileSet;



        //Fonts
        public SpriteFont MenuText;
        public SpriteFont BitFont;
        public SpriteFont ArialFont;

        //Maps



        //TileSets
        public Texture2D MasterTileSet;
        public Texture2D InteriorTileSet1;


        public Texture2D ItemSpriteSheet;

        //PARTICLES
        public Texture2D RockParticle;
        public Texture2D RainDrop;
        public Texture2D SmokeParticle;

        //prop
        public Texture2D Fire;


        //Effects

        public Effect practiceLightMaskEffect;
        public Effect nightTint;
        public Effect Pulse;
        public Effect whirlPoolGlow;
        public Texture2D lightMask;

        //Debug
        public Texture2D redPixel;

        //Props

        public Texture2D TallGrass;
        public Texture2D Gears;
        public Texture2D PlayerSilouhette;
        public Texture2D ButterFlys { get; set; }


        //BackDrops





        public TextureBook(ContentManager content, SpriteBatch spriteBatch)
        {
            //Player
            EyesAtlas = content.Load<Texture2D>("Player/EyesAtlas");
            HairAtlas = content.Load<Texture2D>("Player/hairAtlas");
            PlayerBaseAtlas = content.Load<Texture2D>("Player/playerBaseAtlas");
            ShirtAtlas = content.Load<Texture2D>("Player/shirtAtlas");
            PantsAtlas = content.Load<Texture2D>("Player/pantsAtlas");
            ShoesAtlas = content.Load<Texture2D>("Player/shoesAtlas");
            ArmsAtlas = content.Load<Texture2D>("Player/armsAtlas");


            PlayerBase = content.Load<Texture2D>("Player/PlayerParts/Base/base");
            PlayerHair = content.Load<Texture2D>("Player/PlayerParts/Hair/playerHair");
            PlayerShirt = content.Load<Texture2D>("Player/PlayerParts/Shirts/shirts");
            PlayerPants = content.Load<Texture2D>("Player/PlayerParts/Pants/pants");
            PlayerShoes = content.Load<Texture2D>("Player/PlayerParts/Shoes/shoes");

            //ANIMATIONS
            //CHOPPING
            //TOOLS
            ChoppingTestTool = content.Load<Texture2D>("Player/PlayerParts/Chopping/Tools/ChoppingTestTool");
            IronAxeTool = content.Load<Texture2D>("Player/PlayerParts/Chopping/Tools/IronAxeTool");
            ChoppingToolAtlas = content.Load<Texture2D>("Player/PlayerParts/Chopping/Tools/ChoppingToolAtlas");

            ChoppingPlayerBase = content.Load<Texture2D>("Player/PlayerParts/Chopping/Base/ChoppingBase");
            ChoppingPlayerHair = content.Load<Texture2D>("Player/PlayerParts/Chopping/Hair/ChoppingBlondeHair");
            ChoppingPlayerPants = content.Load<Texture2D>("Player/PlayerParts/Chopping/Pants/ChoppingPants");
            ChoppingPlayerShirt = content.Load<Texture2D>("Player/PlayerParts/Chopping/Shirts/ChoppingRedShirt");
            ChoppingPlayerShoes = content.Load<Texture2D>("Player/PlayerParts/Chopping/Shoes/ChoppingBrownShoes");


            //SWIPING
            SwipingPlayerBase = content.Load<Texture2D>("Player/PlayerParts/Swiping/Base/swipingBase");

            SwipingPlayerShirt = content.Load<Texture2D>("Player/PlayerParts/Swiping/Shirts/swipingShirts");


            PickUpItemBase = content.Load<Texture2D>("Player/PlayerParts/PickUpItem/Base/PickUpItemBase");
            PickUpItemBlondeHair = content.Load<Texture2D>("Player/PlayerParts/PickUpItem/Hair/PickUpItemBlondeHair");
            PickUpItemBluePants = content.Load<Texture2D>("Player/PlayerParts/PickUpItem/Pants/PickUpItemBluePants");
            PickUpItemRedShirt = content.Load<Texture2D>("Player/PlayerParts/PickUpItem/Shirts/PickUpItemRedShirt");
            PickUpItemBrownShoes = content.Load<Texture2D>("Player/PlayerParts/PickUpItem/Shoes/PickUpItemBrownShoes");

            portalJumpBase = content.Load<Texture2D>("Player/PlayerParts/PortalJump/Base/portalJumpBase");
            portalJumpHair = content.Load<Texture2D>("Player/PlayerParts/PortalJump/Hair/portalJumpHair");
            portalJumpPants = content.Load<Texture2D>("Player/PlayerParts/PortalJump/Pants/portalJumpPants");
            portalJumpShirt = content.Load<Texture2D>("Player/PlayerParts/PortalJump/Shirts/portalJumpShirt");
            portalJumpShoes = content.Load<Texture2D>("Player/PlayerParts/PortalJump/Shoes/portalJumpShoes");
            //NPC
            Elixer = content.Load<Texture2D>("NPC/ElixerTest");
            ElixirSpriteSheet = content.Load<Texture2D>("NPC/Elixir/ElixirSpriteSheet");
            ElixirPortrait = content.Load<Texture2D>("NPC/Elixir/elixirPortrait");

            DobbinSpriteSheet = content.Load<Texture2D>("NPC/Dobbin/DobbinSpriteSheet");
            DobbinPortrait = content.Load<Texture2D>("NPC/Dobbin/dobbinPortrait");


            SnawSpriteSheet = content.Load<Texture2D>("NPC/Snaw/Snaw");
            SnawPortrait = content.Load<Texture2D>("NPC/Snaw/SnawPortrait");

            EnemySpriteSheet = content.Load<Texture2D>("NPC/Enemy/EnemySpriteSheet");
            Nelja = content.Load<Texture2D>("NPC/Enemy/nelga");

            //BOSSES
            Carotar = content.Load<Texture2D>("NPC/Enemy/Carotar/Carotar");
            CarotarShadow = content.Load<Texture2D>("NPC/Enemy/Carotar/CarotarShadow");

            KayaSpriteSheet = content.Load<Texture2D>("NPC/Kaya/KayaSpriteSheet");
            KayaPortrait = content.Load<Texture2D>("NPC/Kaya/KayaPortrait");

            JulianSpriteSheet = content.Load<Texture2D>("NPC/Julian/Julian");
            JulianPortrait = content.Load<Texture2D>("NPC/Julian/julianPortrait");

            SarahSpriteSheet = content.Load<Texture2D>("NPC/Sarah/Sarah");
            SarahPortrait = content.Load<Texture2D>("NPC/Sarah/SarahPortrait");

            BusinessSnail = content.Load<Texture2D>("NPC/BusinessSnail/BusinessSnail");
            BusinessSnailPortrait = content.Load<Texture2D>("NPC/BusinessSnail/BusinessSnailPortrait");

            Mippin = content.Load<Texture2D>("NPC/Mippin/mippin");
            MippinPortrait = content.Load<Texture2D>("NPC/Mippin/mippinPortrait");

            Ned = content.Load<Texture2D>("NPC/Ned/Ned");
            NedPortrait = content.Load<Texture2D>("NPC/Ned/NedPortrait");

            Teal = content.Load<Texture2D>("NPC/Teal/Teal");
            TealPortrait = content.Load<Texture2D>("NPC/Teal/TealPortrait");

            Marcus = content.Load<Texture2D>("NPC/Marcus/Marcus");
            MarcusPotrait = content.Load<Texture2D>("NPC/Marcus/MarcusPortrait");

            Caspar = content.Load<Texture2D>("NPC/Caspar/Caspar");
            CasparPortrait = content.Load<Texture2D>("NPC/Caspar/CasparPortrait");

            MenuText = content.Load<SpriteFont>("SpriteFont/MenuText");
            BitFont = content.Load<SpriteFont>("SpriteFont/grakFont");
            ArialFont = content.Load<SpriteFont>("SpriteFont/arial_22");

            MasterTileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");
            InteriorTileSet1 = content.Load<Texture2D>("Map/InteriorSpriteSheet1");
            //OrchardTileSet = content.Load<Texture2D>("Map/orchard");

            ItemSpriteSheet = content.Load<Texture2D>("Item/ItemSpriteSheet");

            //PARTICLES
            RockParticle = content.Load<Texture2D>("Particles/rockparticle");
            RainDrop = content.Load<Texture2D>("Particles/raindrop");
            SmokeParticle = content.Load<Texture2D>("Particles/smokeParticle");
            Fire = content.Load<Texture2D>("Particles/fire1");

            UserInterfaceTileSet = content.Load<Texture2D>("Button/userinterfaceTileSet");


            //EFFECTS
            lightMask = content.Load<Texture2D>("Effects/lightmask");
            practiceLightMaskEffect = content.Load<Effect>("Effects/practiceLighting1");
            nightTint = content.Load<Effect>("Effects/nightTint");
            Pulse = content.Load<Effect>("Effects/Pulse");
            whirlPoolGlow = content.Load<Effect>("Effects/whirlPoolGlow");

            redPixel = content.Load<Texture2D>("Debug/solidRed");

            //MINING FOLDER




            TallGrass = content.Load<Texture2D>("WorldProps/tallGrass");
            Gears = content.Load<Texture2D>("WorldProps/Gears");
            PlayerSilouhette = content.Load<Texture2D>("WorldProps/introplayersillohoutte");
            this.ButterFlys = content.Load<Texture2D>("WorldProps/Butterflys");


            //backdrops
        }


        public Rectangle GetItemTextureFromAtlas(int row, int column)
        {
            int width = 16;
            int height = 16;

            return new Rectangle((int)column * width, (int)row * height, width, height);
        }

        public Rectangle GetItemTexture(int id, int tileSetDimension)
        {
            int Row = id % tileSetDimension;
            int Column = (int)Math.Floor((double)id / (double)tileSetDimension);

            return new Rectangle(16 * Column, 16 * Row, 16, 16);
        }
        

    }
}

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

        public Texture2D Train;


        //BackDrops





        public TextureBook(ContentManager content, SpriteBatch spriteBatch)
        {
           
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
            this.Train = content.Load<Texture2D>("WorldProps/train");

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

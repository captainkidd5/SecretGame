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
        public Texture2D SwipingPlayerHair;
        public Texture2D SwipingPlayerPants;
        public Texture2D SwipingPlayerShirt;
        public Texture2D SwipingPlayerShoes;
        public Texture2D SwipingTestTool;

        //NPCS
        public Texture2D Elixer;
        public Texture2D ElixirSpriteSheet;
        public Texture2D ElixirPortrait;


        public Texture2D DobbinSpriteSheet;
        public Texture2D DobbinPortrait;

        public Texture2D EnemySpriteSheet;
        

        public Texture2D KayaSpriteSheet;

        public Texture2D SnawSpriteSheet;
        public Texture2D JulianSpriteSheet;
        public Texture2D JulianPortrait;

        public Texture2D SarahSpriteSheet;

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
        public Effect Pulse;
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
            SwipingPlayerHair = content.Load<Texture2D>("Player/PlayerParts/Swiping/Hair/swipingBlondeHair");
            SwipingPlayerPants = content.Load<Texture2D>("Player/PlayerParts/Swiping/Pants/swipingBluePants");
            SwipingPlayerShirt = content.Load<Texture2D>("Player/PlayerParts/Swiping/Shirts/swipingRedShirt");
            SwipingPlayerShoes = content.Load<Texture2D>("Player/PlayerParts/Swiping/Shoes/swipingBrownShoes");
            SwipingTestTool = content.Load<Texture2D>("Player/PlayerParts/Swiping/Tools/testSword");

            //NPC
            Elixer = content.Load<Texture2D>("NPC/ElixerTest");
            ElixirSpriteSheet = content.Load<Texture2D>("NPC/Elixir/ElixirSpriteSheet");
            ElixirPortrait = content.Load<Texture2D>("NPC/Elixir/elixirPortrait");

            DobbinSpriteSheet = content.Load<Texture2D>("NPC/Dobbin/DobbinSpriteSheet");
            DobbinPortrait = content.Load<Texture2D>("NPC/Dobbin/dobbinPortrait");


            SnawSpriteSheet = content.Load<Texture2D>("NPC/Snaw/Snaw");


            EnemySpriteSheet = content.Load<Texture2D>("NPC/Enemy/EnemySpriteSheet");

            KayaSpriteSheet = content.Load<Texture2D>("NPC/Kaya/KayaSpriteSheet");

            JulianSpriteSheet = content.Load<Texture2D>("NPC/Julian/Julian");
            JulianPortrait = content.Load<Texture2D>("NPC/Julian/julianPortrait");

            SarahSpriteSheet = content.Load<Texture2D>("NPC/Sarah/Sarah");
            CursorWhiteHand = content.Load<Texture2D>("Button/Cursor1");
            CursorPlant = content.Load<Texture2D>("Button/PlantCursor");


            MenuText = content.Load<SpriteFont>("SpriteFont/MenuText");


            MasterTileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");
            InteriorTileSet1 = content.Load<Texture2D>("Map/InteriorSpriteSheet1");
            //OrchardTileSet = content.Load<Texture2D>("Map/orchard");

            ItemSpriteSheet = content.Load<Texture2D>("Item/ItemSpriteSheet");

            RockParticle = content.Load<Texture2D>("Particles/rockparticle");

            UserInterfaceTileSet = content.Load<Texture2D>("Button/userinterfaceTileSet");


            //EFFECTS
            lightMask = content.Load<Texture2D>("Effects/lightmask");
            practiceLightMaskEffect = content.Load<Effect>("Effects/practiceLighting1");
            nightTint = content.Load<Effect>("Effects/nightTint");
            Pulse = content.Load<Effect>("Effects/Pulse");

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

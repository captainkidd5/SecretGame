using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Playable;
using SecretProject.Class.StageFolder.DungeonStuff;
using SecretProject.Class.StageFolder.DungeonStuff.Desert;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Text;
using TiledSharp;

namespace SecretProject.Class.StageFolder
{
    public enum StagesEnum
    {
        Town = 0,
        ElixirHouse = 1,
        JulianHouse = 2,

        DobbinHouse = 3,
        PlayerHouse = 4,
        GeneralStore = 5,
        KayaHouse = 6,
        Cafe = 7,

        DobbinHouseUpper = 8,
        MarcusHouse = 9,

        LightHouse = 10,

        CasparHouse = 11,
        MountainTop = 12,
        GisaardRanch = 13,
        HomeStead = 14,
        ForestDungeon = 15,
        DesertDungeon = 16,
        SippiDesert = 17,
        TrainStation = 18,
        RooltapCastle = 19,
        ThroneRoom = 20,
        MainMenu = 50,
        Exit = 55,


    }
    public class StageManager : Component
    {



        public static Stage CurrentStage { get; set; }

        public List<Stage> Stages { get; private set; }

        private Texture2D MasterSpriteSheet { get; set; }
        private Texture2D InteriorSpriteSheet { get; set; }
        private PlayerManager PlayerManager { get; }
        private CharacterManager CharacterManager { get; set; }

        private TurnDial TurnDial { get; set; }

        public MainMenu mainMenu;
        public static Stage Town;
        public static Stage ElixirHouse;


        public static Stage JulianHouse;
        public static Stage DobbinHouse;
        // public  World OverWorld;
        public static Stage PlayerHouse;
        public static Stage GeneralStore;
        public static Stage KayaHouse;
        public static Stage Cafe;
        // public  World UnderWorld;
        public static Stage DobbinHouseUpper;
        public static Stage MarcusHouse;
        public static Stage LightHouse;
        public static Stage CasparHouse;
        public static Stage MountainTop;
        public static Stage GisaardRanch;
        public static Stage HomeStead;
        public static Stage ForestDungeon;
        public static Stage DesertDungeon;
        public static Stage SippiDesert;
        public static Stage TrainStation;
        public static Stage RooltapCastle;
        public static Stage ThroneRoom;

        public List<Stage> AllStages;

        public RenderTarget2D MainTarget;
        public RenderTarget2D NightLightsTarget;
        public RenderTarget2D DayLightsTarget;

        public StageManager(Game1 game1, GraphicsDevice graphics, ContentManager content, PlayerManager playerManager, CharacterManager characterManager) : base(graphics, content)
        {
            PlayerManager = playerManager;
            CharacterManager = characterManager;
        }

        public override void Load()
        {
            PresentationParameters pp = graphicsDevice.PresentationParameters;
            MainTarget = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, DepthFormat.Depth24);
            NightLightsTarget = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, DepthFormat.Depth24);
            DayLightsTarget = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, DepthFormat.Depth24);
            MasterSpriteSheet = content.Load<Texture2D>("maps/MasterSpriteSheet");
            TmxMap townMap = new TmxMap("Content/Map/Town.tmx");
            Town = new Stage(this,"Town", LocationType.Exterior, graphicsDevice, content, MasterSpriteSheet, townMap, 1, 1, content.ServiceProvider, PlayerManager, CharacterManager)
            { StageIdentifier = (int)StagesEnum.Town };





            ElixirHouse = new Stage(this, "ElixirHouse", LocationType.Interior, graphicsDevice, content, InteriorSpriteSheet, new TmxMap("content/Map/elixirShop.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.ElixirHouse };
            JulianHouse = new Stage(this, "JulianHouse", LocationType.Interior, graphicsDevice, content, InteriorSpriteSheet, new TmxMap("content/Map/JulianShop.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.JulianHouse };
            DobbinHouse = new Stage(this, "DobbinHouse", LocationType.Interior, graphicsDevice, content, InteriorSpriteSheet, new TmxMap("content/Map/DobbinHouse.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.DobbinHouse };
            PlayerHouse = new Stage(this, "PlayerHouse", LocationType.Interior, graphicsDevice, content, InteriorSpriteSheet, new TmxMap("content/Map/PlayerHouseSmall.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.PlayerHouse };
            GeneralStore = new Stage(this, "GeneralStore", LocationType.Interior, graphicsDevice, content, InteriorSpriteSheet, new TmxMap("content/Map/GeneralStore.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.GeneralStore };
            KayaHouse = new Stage(this, "KayaHouse", LocationType.Interior, graphicsDevice, content, InteriorSpriteSheet, new TmxMap("content/Map/KayaHouse.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.KayaHouse };
            Cafe = new Stage(this, "Cafe", LocationType.Interior, graphicsDevice, content, InteriorSpriteSheet, new TmxMap("content/Map/Cafe.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.Cafe };
            DobbinHouseUpper = new Stage(this, "DobbinHouseUpper", LocationType.Interior, graphicsDevice, content, InteriorSpriteSheet, new TmxMap("content/Map/DobbinHouseUpper.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.DobbinHouseUpper };
            MarcusHouse = new Stage(this, "MarcusHouse", LocationType.Interior, graphicsDevice, content, InteriorSpriteSheet, new TmxMap("content/Map/MarcusHouse.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.MarcusHouse };
            LightHouse = new Stage(this, "LightHouse", LocationType.Interior, graphicsDevice, content, InteriorSpriteSheet, new TmxMap("content/Map/LightHouse.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.LightHouse };
            CasparHouse = new Stage(this, "CasparHouse", LocationType.Interior, graphicsDevice, content, InteriorSpriteSheet, new TmxMap("content/Map/CasparHouse.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.CasparHouse };
            MountainTop = new Stage(this, "MountainTop", LocationType.Exterior, graphicsDevice, content, MasterSpriteSheet, new TmxMap("content/Map/MountainTop.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.MountainTop };
            GisaardRanch = new Stage(this, "GisaardRanch", LocationType.Exterior, graphicsDevice, content, MasterSpriteSheet, new TmxMap("content/Map/GisaardRanch.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.GisaardRanch };
            HomeStead = new Stage(this, "HomeStead", LocationType.Exterior, graphicsDevice, content, MasterSpriteSheet, new TmxMap("content/Map/HomeStead.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.HomeStead };
            ForestDungeon = new ForestDungeon("Forest", LocationType.Exterior, graphicsDevice, content, MasterSpriteSheet, new TmxMap("content/Map/HomeStead.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.ForestDungeon };
            DesertDungeon = new DesertDungeon("Desert", LocationType.Exterior, graphicsDevice, content, MasterSpriteSheet, new TmxMap("content/Map/HomeStead.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.DesertDungeon };
            SippiDesert = new Stage(this, "SippiDesert", LocationType.Exterior, graphicsDevice, content, MasterSpriteSheet, new TmxMap("content/Map/SippiDesert.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.SippiDesert };
            TrainStation = new Stage(this, "TrainStation", LocationType.Exterior, graphicsDevice, content, MasterSpriteSheet, new TmxMap("content/Map/TrainStation.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.TrainStation };
            RooltapCastle = new Stage(this, "RuletapCastle", LocationType.Exterior, graphicsDevice, content, MasterSpriteSheet, new TmxMap("content/Map/RooltapCastle.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.RooltapCastle };
            ThroneRoom = new Stage(this, "ThroneRoom", LocationType.Interior, graphicsDevice, content, InteriorSpriteSheet, new TmxMap("content/Map/ThroneRoom.tmx"), content.ServiceProvider, PlayerManager, CharacterManager) { StageIdentifier = StagesEnum.ThroneRoom };
            AllStages = new List<Stage>() { Town, ElixirHouse, JulianHouse, DobbinHouse, PlayerHouse, GeneralStore, KayaHouse, Cafe, DobbinHouseUpper, MarcusHouse, LightHouse, CasparHouse, MountainTop, GisaardRanch, HomeStead, ForestDungeon, DesertDungeon, SippiDesert, TrainStation, RooltapCastle, ThroneRoom };
            TurnDial = new TurnDial();
        }

        /// <summary>
        /// remove character and player hulls.
        /// </summary>
        private void UnloadPenumbraEntities()
        {
            if (CurrentStage.Penumbra != null)
            {
                CurrentStage.Penumbra.Lights.Clear();
                CurrentStage.Penumbra.Hulls.Clear();
            }

        }

        public Stage GetStageFromEnum(StagesEnum stage)
        {
            return AllStages.Find(x => x.StageIdentifier == stage);
        }

        public override void Unload()
        {

        }

        public void SwitchStage(Stage newStage)
        {
            if (CurrentStage != null)
                CurrentStage.Unload();

            newStage.Load();
            CurrentStage = newStage;
        }

        public void SwitchStage(Stage stageToSwitchTo, Portal portal = null)
        {
            Game1.UserInterface.LoadingScreen.BeginBlackTransition(.05f, 2f);

            IsFadingOut = true;

            Game1.VelcroWorld.Clear();


            //VelcroWorld.ProcessChanges();

            CurrentStage.UnloadContent();
            UnloadPenumbraEntities();

            if (CurrentStage == PlayerHouse)
                stageToSwitchTo = TurnDial.CurrentLocation;

            Stage newLocation = stageToSwitchTo;


            Game1.Player.LockBounds = true;

            if (!newLocation.IsLoaded)
            {

                newLocation.LoadContent();

            }
            newLocation.TryLoadExistingStage();
            if (CurrentStage.DebuggableShapes != null)
            {
                CurrentStage.DebuggableShapes.Clear();
            }
            if (portal != null)
            {
                List<Portal> portalTest = stageToSwitchTo.AllPortals;
                Portal tempPortal = stageToSwitchTo.AllPortals.Find(z => z.From == portal.To && z.To == portal.From);
                if (tempPortal != null)
                {
                    float x = tempPortal.PortalStart.X;
                    float width = tempPortal.PortalStart.Width / 2;
                    float y = tempPortal.PortalStart.Y;
                    float safteyX = tempPortal.SafteyOffSetX;
                    float safteyY = tempPortal.SafteyOffSetY;
                    Player.CreateBody();
                    Player.SetPosition(new Vector2(x + width + safteyX, y + safteyY));
                    cam.pos = Player.Position;

                }
                else
                {
                    Player.CreateBody();
                }
            }
            else
            {
                Player.CreateBody();
            }

            Player.Wardrobe.UpdateForCreationMenu();



            Game1.MouseManager.AttachMouseBody();

            LoadCharacterBodies(CurrentStage, newLocation);
            Train.SwitchStage(CurrentStage.StageIdentifier, newLocation.StageIdentifier);
            CurrentStage = newLocation;
            CurrentStage.DebuggableShapes.Add(new RectangleDebugger(Player.LargeProximitySensor, CurrentStage.DebuggableShapes));
            CurrentStage.AllTiles.UpdateCropTile();

            Player.LoadPenumbra(CurrentStage);


            VelcroWorld.ProcessChanges();
            Game1.GlobalClock.ProcessNewDayChanges();

        }

        private void LoadStage()
        {

        }

        private void UnloadStage()
        {

        }

        public void Update(GameTime gameTime)
        {
            CurrentStage.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentStage.Draw(spriteBatch, MainTarget,NightLightsTarget,DayLightsTarget);
        }

        public Stage GetStage(string name)
        {
            return Stages.Find(x => x.Name == name);
        }


    }

}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Playable;
using SecretProject.Class.StageFolder.DungeonStuff;
using SecretProject.Class.StageFolder.DungeonStuff.Desert;
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
    public class StageHandler
    {
        private readonly GraphicsDevice graphics;
        private readonly ContentManager content;

        public Stage CurrentStage { get; set; }

        public List<Stage> Stages { get; private set; }

        private Texture2D MasterSpriteSheet { get; set; }
        private Texture2D InteriorSpriteSheet { get; set; }
        private PlayerManager PlayerManager { get; }
        private CharacterManager CharacterManager { get; set;}

        public MainMenu mainMenu;
        public Stage Town;
        public Stage ElixirHouse;


        public Stage JulianHouse;
        public Stage DobbinHouse;
        // public  World OverWorld;
        public Stage PlayerHouse;
        public Stage GeneralStore;
        public Stage KayaHouse;
        public Stage Cafe;
        // public  World UnderWorld;
        public Stage DobbinHouseUpper;
        public Stage MarcusHouse;
        public Stage LightHouse;
        public Stage CasparHouse;
        public Stage MountainTop;
        public Stage GisaardRanch;
        public Stage HomeStead;
        public Stage ForestDungeon;
        public Stage DesertDungeon;
        public Stage SippiDesert;
        public Stage TrainStation;
        public Stage RooltapCastle;
        public Stage ThroneRoom;

        public List<Stage> AllStages;



        public StageHandler(Game1 game1, GraphicsDevice graphics, ContentManager content, PlayerManager playerManager, CharacterManager characterManager)
        {
            this.graphics = graphics;
            this.content = content;
            PlayerManager = playerManager;
            CharacterManager = characterManager;
        }

        public void Load()
        {
            MasterSpriteSheet = content.Load<Texture2D>("maps/MasterSpriteSheet");
            TmxMap townMap = new TmxMap("Content/Map/Town.tmx");
            Town = new Stage("Town", LocationType.Exterior, graphics, content, MasterSpriteSheet, townMap, 1, 1, content.ServiceProvider,PlayerManager)
            { StageIdentifier = (int)StagesEnum.Town };





            ElixirHouse = new Stage("ElixirHouse", LocationType.Interior, graphics, content, InteriorSpriteSheet, new TmxMap("content/Map/elixirShop.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.ElixirHouse };
            JulianHouse = new Stage("JulianHouse", LocationType.Interior, graphics, content, InteriorSpriteSheet, new TmxMap("content/Map/JulianShop.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.JulianHouse };
            DobbinHouse = new Stage("DobbinHouse", LocationType.Interior, graphics, content, InteriorSpriteSheet, new TmxMap("content/Map/DobbinHouse.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.DobbinHouse };
            PlayerHouse = new Stage("PlayerHouse", LocationType.Interior, graphics, content, InteriorSpriteSheet, new TmxMap("content/Map/PlayerHouseSmall.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.PlayerHouse };
            GeneralStore = new Stage("GeneralStore", LocationType.Interior, graphics, content, InteriorSpriteSheet, new TmxMap("content/Map/GeneralStore.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.GeneralStore };
            KayaHouse = new Stage("KayaHouse", LocationType.Interior, graphics, content, InteriorSpriteSheet, new TmxMap("content/Map/KayaHouse.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.KayaHouse };
            Cafe = new Stage("Cafe", LocationType.Interior, graphics, content, InteriorSpriteSheet, new TmxMap("content/Map/Cafe.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.Cafe };
            DobbinHouseUpper = new Stage("DobbinHouseUpper", LocationType.Interior, graphics, content, InteriorSpriteSheet, new TmxMap("content/Map/DobbinHouseUpper.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.DobbinHouseUpper };
            MarcusHouse = new Stage("MarcusHouse", LocationType.Interior, graphics, content, InteriorSpriteSheet, new TmxMap("content/Map/MarcusHouse.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.MarcusHouse };
            LightHouse = new Stage("LightHouse", LocationType.Interior, graphics, content, InteriorSpriteSheet, new TmxMap("content/Map/LightHouse.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.LightHouse };
            CasparHouse = new Stage("CasparHouse", LocationType.Interior, graphics, content, InteriorSpriteSheet, new TmxMap("content/Map/CasparHouse.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.CasparHouse };
            MountainTop = new Stage("MountainTop", LocationType.Exterior, graphics, content, MasterSpriteSheet, new TmxMap("content/Map/MountainTop.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.MountainTop };
            GisaardRanch = new Stage("GisaardRanch", LocationType.Exterior, graphics, content, MasterSpriteSheet, new TmxMap("content/Map/GisaardRanch.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.GisaardRanch };
            HomeStead = new Stage("HomeStead", LocationType.Exterior, graphics, content, MasterSpriteSheet, new TmxMap("content/Map/HomeStead.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.HomeStead };
            ForestDungeon = new ForestDungeon("Forest", LocationType.Exterior, graphics, content, MasterSpriteSheet, new TmxMap("content/Map/HomeStead.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.ForestDungeon };
            DesertDungeon = new DesertDungeon("Desert", LocationType.Exterior, graphics, content, MasterSpriteSheet, new TmxMap("content/Map/HomeStead.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.DesertDungeon };
            SippiDesert = new Stage("SippiDesert", LocationType.Exterior, graphics, content, MasterSpriteSheet, new TmxMap("content/Map/SippiDesert.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.SippiDesert };
            TrainStation = new Stage("TrainStation", LocationType.Exterior, graphics, content, MasterSpriteSheet, new TmxMap("content/Map/TrainStation.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.TrainStation };
            RooltapCastle = new Stage("RuletapCastle", LocationType.Exterior, graphics, content, MasterSpriteSheet, new TmxMap("content/Map/RooltapCastle.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.RooltapCastle };
            ThroneRoom = new Stage("ThroneRoom", LocationType.Interior, graphics, content, InteriorSpriteSheet, new TmxMap("content/Map/ThroneRoom.tmx"), 1, 0, content.ServiceProvider,PlayerManager) { StageIdentifier = StagesEnum.ThroneRoom };
            AllStages = new List<Stage>() { Town, ElixirHouse, JulianHouse, DobbinHouse, PlayerHouse, GeneralStore, KayaHouse, Cafe, DobbinHouseUpper, MarcusHouse, LightHouse, CasparHouse, MountainTop, GisaardRanch, HomeStead, ForestDungeon, DesertDungeon, SippiDesert, TrainStation, RooltapCastle, ThroneRoom };

        }

        public Stage GetStageFromEnum(StagesEnum stage)
        {
            return AllStages.Find(x => x.StageIdentifier == stage);
        }

        public void Unload()
        {

        }

        public void SwitchStage(Stage newStage)
        {
            if (CurrentStage != null)
                CurrentStage.Unload();

            newStage.Load();
            CurrentStage = newStage;
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
            CurrentStage.Draw(spriteBatch);
        }

        public Stage GetStage(string name)
        {
            return Stages.Find(x => x.Name == name);
        }

    }

}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretProject.Class.StageFolder
{

    public class StageHandler
    {
        private readonly GraphicsDevice graphics;
        private readonly ContentManager content;

        public TmxStageBase CurrentStage { get; set; }

        public List<TmxStageBase> Stages { get; private set; }

        private Texture2D MasterSpriteSheet { get; set; }


        public static MainMenu mainMenu;
        public static TmxStageBase Town;
        public static TmxStageBase ElixirHouse;


        public static TmxStageBase JulianHouse;
        public static TmxStageBase DobbinHouse;
        // public static World OverWorld;
        public static TmxStageBase PlayerHouse;
        public static TmxStageBase GeneralStore;
        public static TmxStageBase KayaHouse;
        public static TmxStageBase Cafe;
        // public static World UnderWorld;
        public static TmxStageBase DobbinHouseUpper;
        public static TmxStageBase MarcusHouse;
        public static TmxStageBase LightHouse;
        public static TmxStageBase CasparHouse;
        public static TmxStageBase MountainTop;
        public static TmxStageBase GisaardRanch;
        public static TmxStageBase HomeStead;
        public static TmxStageBase ForestDungeon;
        public static TmxStageBase DesertDungeon;
        public static TmxStageBase SippiDesert;
        public static TmxStageBase TrainStation;
        public static TmxStageBase RooltapCastle;
        public static TmxStageBase ThroneRoom;

        public static List<TmxStageBase> AllStages;



        public StageHandler(Game1 game1, GraphicsDevice graphics, ContentManager content)
        {
            this.graphics = graphics;
            this.content = content;


        }

        public void Load()
        {
            MasterSpriteSheet = content.Load<Texture2D>("maps/MasterSpriteSheet");

        }

        public void Unload()
        {

        }

        public void SwitchStage(TmxStageBase newStage)
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

        public TmxStageBase GetStage(string name)
        {
            return Stages.Find(x => x.Name == name);
        }

    }

}

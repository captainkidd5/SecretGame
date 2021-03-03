
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.UI;
using SecretProject.Class.UI.AlertStuff;
using SecretProject.Class.UI.MainMenuStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using SecretProject.Class.TileStuff;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.ParticileStuff;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.CollisionDetection.ProjectileStuff;
using TiledSharp;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Playable;
using XMLData.RouteStuff;
using System.IO;
using SecretProject.Class.UI.ButtonStuff;
using SecretProject.Class.Misc;
using SecretProject.Class.StageFolder.DungeonStuff;
using Penumbra;

namespace SecretProject.Class.StageFolder
{
    public class MainMenu : Stage
    {

        public enum MenuState
        {
            Primary = 0,
            Play = 1,
            Settings = 2,
            DevPanel = 3

        }

        //--------------------------------------
        //buttons
        public MenuState CurrentMenuState = MenuState.Primary;
        readonly Button Play;
        readonly Button Settings;
        readonly Button DevPanel;
        readonly Button Exit;

        //PLAY BUTTONs
        public ChooseGameMenu ChooseGameMenu;

        //SETTINGS BUTTONS

        Button FullScreen;

        //Dev Buttons

        Button StartGameInTown;
        Button StartGameInWilderness;
        Button StartGameInUnderWorld;

        //Universal Buttons
        Button Back;


        Texture2D BackDrop;



        public List<Button> PrimaryButtons { get; set; }
        public List<Button> SettingsButtons { get; set; }
        public List<Button> DevPanelButtons { get; set; }


        //Random stuff


        ContentManager MainMenuContentManager;



        public List<Alert> AllAlerts { get; set; }
        public bool IsDrawn { get; set; }



        private Game1 game;
        public MainMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, IServiceProvider service) : base(game, graphicsDevice, content)
        {
 
            Graphics = graphicsDevice;
            this.MainMenuContentManager = content;

            Vector2 buttonStartPosition = Game1.Utility.CenterRectangleOnScreen(new Rectangle(1024, 64, 112, 48), 2f);

            //PRIMARY 
            Play = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1024, 64, 112, 48), graphicsDevice, buttonStartPosition, CursorType.Normal, 2f);
            Settings = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(736, 32, 32, 32), graphicsDevice, new Vector2(Game1.PresentationParameters.BackBufferWidth * .025f, Game1.PresentationParameters.BackBufferHeight * .025f), CursorType.Normal, 2f)
            { ItemSourceRectangleToDraw = new Rectangle(16, 128, 32, 32) };
            Exit = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1024, 64, 112, 48), graphicsDevice, new Vector2(buttonStartPosition.X, buttonStartPosition.Y + 48 * 2f), CursorType.Normal, 2f);
            DevPanel = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(736, 72, 32, 32), graphicsDevice, new Vector2(Game1.ScreenWidth *.1f, Game1.ScreenHeight * .1f), CursorType.Normal, 2f);

            PrimaryButtons = new List<Button>() { Play, Exit, DevPanel };
            

            //PLAY
            this.ChooseGameMenu = new ChooseGameMenu(this.Graphics, 3f);





            //SETTINGS
            FullScreen = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1024, 64, 112, 48), graphicsDevice, new Vector2(1100, 400), CursorType.Normal, 2f);

            SettingsButtons = new List<Button>()
            {
                FullScreen,
            };


            //DEVPANEL
          
            StartGameInTown = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1024, 64, 112, 48), graphicsDevice, new Vector2(1100, 100), CursorType.Normal, 2f);
            StartGameInWilderness = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1024, 64, 112, 48), graphicsDevice, new Vector2(1100, 200), CursorType.Normal, 2f);
           StartGameInUnderWorld = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1024, 64, 112, 48), graphicsDevice, new Vector2(1100, 400), CursorType.Normal, 2f);

            Back = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1024, 64, 112, 48), graphicsDevice, new Vector2(Game1.ScreenWidth * .8f, Game1.ScreenHeight * .8f), CursorType.Normal);

            DevPanelButtons = new List<Button>() { StartGameInTown, StartGameInWilderness, StartGameInUnderWorld };

 
            BackDrop = content.Load<Texture2D>("MainMenu/MainMenuBackDrop");

            this.AllAlerts = new List<Alert>();
            this.IsDrawn = true;

            this.game = game;
            Penumbra = (PenumbraComponent)service.GetService(typeof(PenumbraComponent));
        }

        public void LoadBackGround()
        {
            BackDrop = MainMenuContentManager.Load<Texture2D>("MainMenu/MainMenuBackDrop");


        }
        public override void UnloadContent()
        {
            MainMenuContentManager.Unload();

        }
      


       


        public override void Update(GameTime gameTime)
        {

            Game1.isMyMouseVisible = true;
            for (int i = 0; i < AllAlerts.Count; i++)
            {
                AllAlerts[i].Update(gameTime, AllAlerts);
            }
            if (!Game1.freeze)
            {
                Settings.Update();
                if (Settings.isClicked)
                {
                    this.CurrentMenuState = MenuState.Settings;
                }

                switch (CurrentMenuState)
                {
                    //Choose between Play, Dev Panel, and Settings.
                    case MenuState.Primary:

                        UpdateMainState(gameTime);

                        break;

                    //NewGame or Load Game
                    case MenuState.Play:
                        UpdatePlayState(gameTime);

                        break;

                    //developer options
                    case MenuState.DevPanel:
                        UpdateDevPanel(gameTime);


                        break;

                    //Go to settings menu
                    case MenuState.Settings:
                        UpdateSettings(gameTime);
                        break;


                }
            }
            Back.Update(Game1.MouseManager);
            if (Back.isClicked)
            {
                ChooseGameMenu.MenuChoice = ChooseGameState.SaveSlotSelection;
                CurrentMenuState = MenuState.Primary;
               
            }

        }

        public void UpdateMainState(GameTime gameTime)
        {
            foreach (Button button in PrimaryButtons)
            {
                button.Update(Game1.MouseManager);
            }


            if (Play.isClicked)
            {
                CurrentMenuState = MenuState.Play;
                return;
            }
            
            else if (Exit.isClicked)
            {
                game.Exit();
            }
            else if (DevPanel.isClicked)
            {
                this.CurrentMenuState = MenuState.DevPanel;
            }
        }

        public void UpdatePlayState(GameTime gameTime)
        {
            ChooseGameMenu.Update(gameTime);
        }

        public void StartNewGame()
        {
            UnloadContent();
            
            foreach (Stage location in StageManager.AllStages) //initialize then unload all stages, except one the player starts in.
            {
                if(location != StageManager.PlayerHouse && location != StageManager.ForestDungeon && location != StageManager.DesertDungeon)
                {
                    location.StartNew();
                    location.UnloadContent();
                }
                
            }
            StageManager.PlayerHouse.StartNew();
            Game1.ItemVault.LoadExteriorContent(StageManager.Town.AllTiles);
            Game1.ItemVault.LoadInteriorContent(StageManager.PlayerHouse.AllTiles);
            CurrentMenuState = MenuState.Primary;
            //Game1.SaveLoadManager.SaveGameState(SaveType.MenuSave);
            StageManager.SwitchStage(StageManager.PlayerHouse);
            Game1.Player.UserInterface.LoadingScreen.BeginBlackTransition(.005f);
            Game1.Player.position = new Vector2(460, 660);

         //   Game1.OverWorld.AllTiles.LoadInitialChunks(new Vector2(1022, 1022));
        }

        public void UpdateDevPanel(GameTime gameTime)
        {
            foreach (Button button in DevPanelButtons)
            {
                button.Update(Game1.MouseManager);
            }
            if (StartGameInTown.isClicked)
            {
                UnloadContent();
                foreach (Stage stage in StageManager.AllStages)
                {

                    stage.LoadPreliminaryContent();


                }
                Game1.ItemVault.LoadExteriorContent(StageManager.Town.AllTiles);
                Game1.ItemVault.LoadInteriorContent(StageManager.Town.AllTiles);
                CurrentMenuState = MenuState.Primary;
                StageManager.SwitchStage(StageManager.Town);
            }
            else if (StartGameInWilderness.isClicked)
            {
                //UnloadContent();
                //foreach (TmxStageBase stage in Game1.AllStages)
                //{

                //    stage.LoadPreliminaryContent();


                //}
                //Game1.ItemVault.LoadExteriorContent(Game1.Town.AllTiles);
                //Game1.ItemVault.LoadInteriorContent(Game1.OverWorld.AllTiles);
                //CurrentMenuState = MenuState.Primary;
                //Game1.SwitchStage(0, Stages.OverWorld);
            }
            else if (StartGameInUnderWorld.isClicked)
            {
                UnloadContent();
                //foreach (Stage stage in Game1.AllStages)
                //{

                //    stage.LoadPreliminaryContent();


                //}
                Game1.ItemVault.LoadExteriorContent(StageManager.Town.AllTiles);
                //Game1.ItemVault.LoadInteriorContent(Game1.OverWorld.AllTiles);
                CurrentMenuState = MenuState.Primary;
               // Game1.SwitchStage(0, Stages.UnderWorld);
            }

        }

        public void UpdateSettings(GameTime gameTime)
        {
            foreach (Button button in this.SettingsButtons)
            {
                button.Update(Game1.MouseManager);
            }
            if (FullScreen.isClicked)
            {
                Game1.FullScreenToggle();
            }
        }

        public void ReturnToDefaultState()
        {
            this.IsDrawn = true;
            Game1.freeze = false;
        }

        public void AddAlert(AlertType type, Vector2 position, string text, Action positiveAction = null, Action negativeAction = null)
        {
            switch (type)
            {
                case AlertType.Confirmation:
                    this.AllAlerts.Add(new ConfirmationAlert(positiveAction,negativeAction, this.Graphics, position, text));
                    break;
                default:
                    this.AllAlerts.Add(new Alert(this.Graphics, position, text));
                    break;
            }

            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.Alert1);
        }

        public void Draw( SpriteBatch spriteBatch)

        {

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null,SamplerState.PointClamp);
            Game1.MouseManager.Draw(spriteBatch, 1f);
            spriteBatch.Draw(BackDrop, new Vector2(0, 0), null, Color.White, 0f, Game1.Utility.Origin, .75f, SpriteEffects.None, .5f);


            for (int i = 0; i < AllAlerts.Count; i++)
            {
                AllAlerts[i].Draw(spriteBatch);
            }
            if (IsDrawn)
            {

                Settings.Draw(spriteBatch, this.Settings.ItemSourceRectangleToDraw, this.Settings.BackGroundSourceRectangle, Game1.AllTextures.MenuText,
                     "", this.Settings.Position, Settings.Color, 2f, 2f,Utility.StandardButtonDepth + .01f, true);
                switch (CurrentMenuState)
                {
                    case MenuState.Primary:

                        Play.Draw(spriteBatch, Game1.AllTextures.MenuText, "Play", Play.FontLocation, Play.Color,Utility.StandardButtonDepth, Utility.StandardTextDepth, 3f);
                        


                        Exit.Draw(spriteBatch, Game1.AllTextures.MenuText, "Exit", Exit.FontLocation, Exit.Color,Utility.StandardButtonDepth, Utility.StandardTextDepth, 3f);
                        DevPanel.Draw(spriteBatch, Game1.AllTextures.MenuText, "", DevPanel.FontLocation, DevPanel.Color,Utility.StandardButtonDepth, Utility.StandardTextDepth, 2f);

                        break;

                    case MenuState.Play:


                        ChooseGameMenu.Draw(spriteBatch);
                        break;
                    case MenuState.Settings:
                        FullScreen.Draw(spriteBatch, Game1.AllTextures.MenuText, "FullScreen", FullScreen.FontLocation, FullScreen.Color,Utility.StandardButtonDepth, Utility.StandardTextDepth, 2f);
                        break;
                    case MenuState.DevPanel:
                        StartGameInTown.Draw(spriteBatch, Game1.AllTextures.MenuText, "Go to town", StartGameInTown.FontLocation, StartGameInTown.Color,Utility.StandardButtonDepth, Utility.StandardTextDepth, 2f);
                        StartGameInWilderness.Draw(spriteBatch, Game1.AllTextures.MenuText, "Go to wilderness", StartGameInWilderness.FontLocation, StartGameInWilderness.Color,Utility.StandardButtonDepth, Utility.StandardTextDepth, 2f);
                        StartGameInUnderWorld.Draw(spriteBatch, Game1.AllTextures.MenuText, "Go to underworld", StartGameInUnderWorld.FontLocation, StartGameInUnderWorld.Color,Utility.StandardButtonDepth, Utility.StandardTextDepth, 2f);
                        break;

                }
            }

            Back.Draw(spriteBatch, Game1.AllTextures.MenuText, "Back", Back.FontLocation, Back.Color,Utility.StandardButtonDepth, Utility.StandardTextDepth);

            spriteBatch.End();
         
        }
    }
}

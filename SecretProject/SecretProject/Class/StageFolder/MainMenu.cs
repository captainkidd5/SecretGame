
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

namespace SecretProject.Class.StageFolder
{
    public class MainMenu : ILocation
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
        Button Play;
        Button Settings;
        Button DevPanel;
        Button Exit;

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


        GraphicsDevice Graphics;
        ContentManager MainMenuContentManager;

        public event EventHandler SceneChanged;

        public List<Alert> AllAlerts { get; set; }
        public bool IsDrawn { get; set; }
        public LocationType LocationType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public StageType StageType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int StageIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string StageName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TileWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TileHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TilesetTilesWide { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TilesetTilesHigh { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Texture2D TileSet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ITileManager AllTiles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Camera2D Cam { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TileSetType TileSetNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Sprite> AllSprites { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<LightSource> AllNightLights { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<LightSource> AllDayTimeLights { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<ActionTimer> AllActions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Portal> AllPortals { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public UserInterface MainUserInterface { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ContentManager StageContentManager { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        GraphicsDevice ILocation.Graphics { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Rectangle MapRectangle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDark { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ShowBorders { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ParticleEngine ParticleEngine { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TextBuilder TextBuilder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsLoaded { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Character> CharactersPresent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<StringWrapper> AllTextToWrite { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<INPC> OnScreenNPCS { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Enemy> Enemies { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Projectile> AllProjectiles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TmxMap Map { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public QuadTree QuadTree { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<RisingText> AllRisingText { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<ParticleEngine> ParticleEngines { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SavePath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private Game1 game;
        public MainMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse, UserInterface userInterface)
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

        }

        public void LoadBackGround()
        {
            BackDrop = MainMenuContentManager.Load<Texture2D>("MainMenu/MainMenuBackDrop");


        }
        public void UnloadContent()
        {
            MainMenuContentManager.Unload();

        }
      


       


        public void Update(GameTime gameTime, MouseManager mouse, Player player)
        {

            Game1.isMyMouseVisible = true;
            for (int i = 0; i < AllAlerts.Count; i++)
            {
                AllAlerts[i].Update(gameTime, AllAlerts);
            }
            if (!Game1.freeze)
            {
                Settings.Update(mouse);
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
            foreach(ILocation location in Game1.AllStages)
            {
                location.AllTiles.StartNew();
            }
            Game1.ItemVault.LoadExteriorContent(Game1.Town.AllTiles);
            Game1.ItemVault.LoadInteriorContent(Game1.PlayerHouse.AllTiles);
            CurrentMenuState = MenuState.Primary;
            //Game1.SaveLoadManager.SaveGameState(SaveType.MenuSave);
            Game1.SwitchStage(0, Stages.PlayerHouse);
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
                foreach (ILocation stage in Game1.AllStages)
                {

                    stage.LoadPreliminaryContent();


                }
                Game1.ItemVault.LoadExteriorContent(Game1.Town.AllTiles);
                Game1.ItemVault.LoadInteriorContent(Game1.Town.AllTiles);
                CurrentMenuState = MenuState.Primary;
                Game1.SwitchStage(0, Stages.Town);
            }
            else if (StartGameInWilderness.isClicked)
            {
                //UnloadContent();
                //foreach (ILocation stage in Game1.AllStages)
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
                foreach (ILocation stage in Game1.AllStages)
                {

                    stage.LoadPreliminaryContent();


                }
                Game1.ItemVault.LoadExteriorContent(Game1.Town.AllTiles);
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

        public void Draw(GraphicsDevice graphics, RenderTarget2D mainTarget, RenderTarget2D lightsTarget, RenderTarget2D dayLightsTarget, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player)

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
                     "", this.Settings.Position, Settings.Color, 2f, 2f, Game1.Utility.StandardButtonDepth + .01f, true);
                switch (CurrentMenuState)
                {
                    case MenuState.Primary:

                        Play.Draw(spriteBatch, Game1.AllTextures.MenuText, "Play", Play.FontLocation, Play.Color, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth, 3f);
                        


                        Exit.Draw(spriteBatch, Game1.AllTextures.MenuText, "Exit", Exit.FontLocation, Exit.Color, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth, 3f);
                        DevPanel.Draw(spriteBatch, Game1.AllTextures.MenuText, "", DevPanel.FontLocation, DevPanel.Color, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth, 2f);

                        break;

                    case MenuState.Play:


                        ChooseGameMenu.Draw(spriteBatch);
                        break;
                    case MenuState.Settings:
                        FullScreen.Draw(spriteBatch, Game1.AllTextures.MenuText, "FullScreen", FullScreen.FontLocation, FullScreen.Color, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth, 2f);
                        break;
                    case MenuState.DevPanel:
                        StartGameInTown.Draw(spriteBatch, Game1.AllTextures.MenuText, "Go to town", StartGameInTown.FontLocation, StartGameInTown.Color, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth, 2f);
                        StartGameInWilderness.Draw(spriteBatch, Game1.AllTextures.MenuText, "Go to wilderness", StartGameInWilderness.FontLocation, StartGameInWilderness.Color, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth, 2f);
                        StartGameInUnderWorld.Draw(spriteBatch, Game1.AllTextures.MenuText, "Go to underworld", StartGameInUnderWorld.FontLocation, StartGameInUnderWorld.Color, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth, 2f);
                        break;

                }
            }

            Back.Draw(spriteBatch, Game1.AllTextures.MenuText, "Back", Back.FontLocation, Back.Color, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);


            spriteBatch.End();
        }



        public void LoadPreliminaryContent()
        {
            throw new NotImplementedException();
        }

        public void LoadContent(Camera2D camera, List<RouteSchedule> routeSchedules)
        {
            throw new NotImplementedException();
        }


        public void AddTextToAllStrings(string message, Vector2 position, float endAtX, float endAtY, float rate, float duration)
        {
            throw new NotImplementedException();
        }

        public void ActivateNewRisingText(float yStart, float yEnd, string stringToWrite, float speed, Color color, bool fade, float scale)
        {
            throw new NotImplementedException();
        }

        public void SaveLocation()
        {
            throw new NotImplementedException();
        }

        public void TryLoadExistingStage()
        {
            throw new NotImplementedException();
        }

        public void AssignPath(string startPath)
        {
            throw new NotImplementedException();
        }

        public string GetDebugString()
        {
            throw new NotImplementedException();
        }

        public void Save(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void Load(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System.Collections.Generic;

namespace SecretProject.Class.StageFolder
{
    public class MainMenu
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
        Button NewGame;
        Button Load;

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
        public List<Button> PlayButtons { get; set; }
        public List<Button> SettingsButtons { get; set; }
        public List<Button> DevPanelButtons { get; set; }


        //Random stuff
        SaveLoadManager mySave;

        GraphicsDevice Graphics;
        ContentManager MainMenuContentManager;





        public MainMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse, UserInterface userInterface)
        {
 
            Graphics = graphicsDevice;
            this.MainMenuContentManager = content;


            //PRIMARY 
            Play = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(Game1.Utility.centerScreen.X - 200, Game1.Utility.CenterScreenY), CursorType.Normal);
            Settings = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(Game1.Utility.centerScreen.X, Game1.Utility.CenterScreenY), CursorType.Normal);
            Exit = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(Game1.Utility.centerScreen.X + 200, Game1.Utility.CenterScreenY), CursorType.Normal);
            DevPanel = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(Game1.Utility.centerScreen.X + 400, Game1.Utility.CenterScreenY), CursorType.Normal);

            PrimaryButtons = new List<Button>() { Play, Settings, Exit, DevPanel };


            //PLAY
            NewGame = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 400), CursorType.Normal);
            Load = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 200), CursorType.Normal);

            PlayButtons = new List<Button>()
            {
                NewGame, 
                Load
            };



            //SETTINGS
            FullScreen = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 400), CursorType.Normal);

            SettingsButtons = new List<Button>()
            {
                FullScreen,
            };


            //DEVPANEL
          
            StartGameInTown = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 100), CursorType.Normal);
            StartGameInWilderness = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 200), CursorType.Normal);
           StartGameInUnderWorld = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 400), CursorType.Normal);

            Back = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(600, 800), CursorType.Normal);

            DevPanelButtons = new List<Button>() { StartGameInTown, StartGameInWilderness, StartGameInUnderWorld };

            mySave = new SaveLoadManager();
            BackDrop = content.Load<Texture2D>("MainMenu/MainMenuBackDrop");



        }

        public void LoadBackGround()
        {
            BackDrop = MainMenuContentManager.Load<Texture2D>("MainMenu/MainMenuBackDrop");


        }
        public void UnloadContent()
        {
            MainMenuContentManager.Unload();

        }


        public void Update(GameTime gameTime, MouseManager mouse, Game1 game)
        {

            Game1.isMyMouseVisible = true;

            switch (CurrentMenuState)
            {
                case MenuState.Primary:
                    foreach (Button button in PrimaryButtons)
                    {
                        button.Update(mouse);
                    }


                    if (Play.isClicked)
                    {
                        CurrentMenuState = MenuState.Play;
                        return;
                    }
                    else if (Settings.isClicked)
                    {
                        this.CurrentMenuState = MenuState.Settings;
                    }
                    else if (Exit.isClicked)
                    {
                        game.Exit();
                    }
                    else if(DevPanel.isClicked)
                    {
                        this.CurrentMenuState = MenuState.DevPanel;
                    }

                    
                    break;


                case MenuState.Play:
                    foreach(Button button in this.PlayButtons)
                    {
                        button.Update(Game1.myMouseManager);
                    }

                    if(NewGame.isClicked)
                    {
                        UnloadContent();
                        foreach (ILocation stage in Game1.AllStages)
                        {

                            stage.LoadPreliminaryContent();


                        }
                        Game1.ItemVault.LoadExteriorContent(Game1.Town.AllTiles);
                        Game1.ItemVault.LoadInteriorContent(Game1.OverWorld.AllTiles);
                        CurrentMenuState = MenuState.Primary;
                        Game1.SwitchStage(0, Stages.Town);
                    }
                    else if(Load.isClicked)
                    {
                        System.Console.WriteLine("Load was clicked");
                    }

                    break;

                case MenuState.DevPanel:
                    foreach (Button button in DevPanelButtons)
                    {
                        button.Update(mouse);
                    }
                    if (StartGameInTown.isClicked)
                    {
                        UnloadContent();
                        foreach (ILocation stage in Game1.AllStages)
                        {
       
                                stage.LoadPreliminaryContent();
                            

                        }
                        Game1.ItemVault.LoadExteriorContent(Game1.Town.AllTiles);
                        Game1.ItemVault.LoadInteriorContent(Game1.OverWorld.AllTiles);
                        CurrentMenuState = MenuState.Primary;
                        Game1.SwitchStage(0, Stages.Town);
                    }
                    else if (StartGameInWilderness.isClicked)
                    {
                        UnloadContent();
                        foreach (ILocation stage in Game1.AllStages)
                        {
        
                                stage.LoadPreliminaryContent();
                            

                        }
                        Game1.ItemVault.LoadExteriorContent(Game1.Town.AllTiles);
                        Game1.ItemVault.LoadInteriorContent(Game1.OverWorld.AllTiles);
                        CurrentMenuState = MenuState.Primary;
                        Game1.SwitchStage(0, Stages.OverWorld);
                    }
                    else if (StartGameInUnderWorld.isClicked)
                    {
                        UnloadContent();
                        foreach (ILocation stage in Game1.AllStages)
                        {

                            stage.LoadPreliminaryContent();


                        }
                        Game1.ItemVault.LoadExteriorContent(Game1.Town.AllTiles);
                        Game1.ItemVault.LoadInteriorContent(Game1.OverWorld.AllTiles);
                        CurrentMenuState = MenuState.Primary;
                        Game1.SwitchStage(0, Stages.UnderWorld);
                    }


                    break;

                case MenuState.Settings:
                    foreach(Button button in this.SettingsButtons)
                    {
                        button.Update(Game1.myMouseManager);
                    }
                    if (FullScreen.isClicked)
                    {
                        Game1.FullScreenToggle();
                    }
                    break;

                    
            }

            Back.Update(Game1.myMouseManager);
            if (Back.isClicked)
            {
                CurrentMenuState = MenuState.Primary;
            }

        }

        public void Draw(GraphicsDevice graphics, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse)

        {

            //GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            Game1.myMouseManager.Draw(spriteBatch, 1f);
            spriteBatch.Draw(BackDrop, new Vector2(0, 0), null, Color.White, 0f, Game1.Utility.Origin, .75f, SpriteEffects.None, .5f);

            //Draw Buttons
            switch (CurrentMenuState)
            {
                case MenuState.Primary:

                    Play.Draw(spriteBatch, Game1.AllTextures.MenuText, "Play", Play.FontLocation, Play.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    Settings.Draw(spriteBatch, Game1.AllTextures.MenuText, "Settings", Settings.FontLocation, Settings.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);

                    Exit.Draw(spriteBatch, Game1.AllTextures.MenuText, "Exit", Exit.FontLocation, Exit.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    DevPanel.Draw(spriteBatch, Game1.AllTextures.MenuText, "Dev Panel", DevPanel.FontLocation, DevPanel.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);

                    break;

                case MenuState.Play:


                    NewGame.Draw(spriteBatch, Game1.AllTextures.MenuText, "New Game", NewGame.FontLocation, NewGame.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    Load.Draw(spriteBatch, Game1.AllTextures.MenuText, "Load Game (not working yet)", Load.FontLocation, Load.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);

                    break;
                case MenuState.Settings:
                    FullScreen.Draw(spriteBatch, Game1.AllTextures.MenuText, "FullScreen", FullScreen.FontLocation, FullScreen.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    break;
                case MenuState.DevPanel:
                    StartGameInTown.Draw(spriteBatch, Game1.AllTextures.MenuText, "Go to town", StartGameInTown.FontLocation, StartGameInTown.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    StartGameInWilderness.Draw(spriteBatch, Game1.AllTextures.MenuText, "Go to wilderness", StartGameInWilderness.FontLocation, StartGameInWilderness.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    StartGameInUnderWorld.Draw(spriteBatch, Game1.AllTextures.MenuText, "Go to underworld", StartGameInUnderWorld.FontLocation, StartGameInUnderWorld.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    break;

            }

            Back.Draw(spriteBatch, Game1.AllTextures.MenuText, "Back", Back.FontLocation, Back.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);


            spriteBatch.End();
        }

    }
}

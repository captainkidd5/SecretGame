
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
            primary = 0,
            play = 1

        }

        //--------------------------------------
        //buttons
        public MenuState menuState = MenuState.primary;
        Button play;
        Button Load;
        Button Exit;
        Button FullScreen;

        //ChooseWorldSize Buttons

        Button StartGameInTown;
        Button StartGameInWilderness;
        Button StartGameInUnderWorld;


        Texture2D BackDrop;
        Texture2D cloud1;


        List<Button> primaryButtons;
        List<Button> chooseWorldSizeButtons;

        //--------------------------------------
        //fonts
        private SpriteFont font;

        //--------------------------------------
        //button textures

        SaveLoadManager mySave;

        GraphicsDevice graphics;
        ContentManager content;

        List<Sprite> clouds;



        public MainMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse, UserInterface userInterface)
        {
            //--------------------------------------
            //Load button textures
            graphics = graphicsDevice;
            this.content = content;

            //--------------------------------------
            //Initialize Buttons
            play = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 100), CursorType.Normal);
            Load = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 200), CursorType.Normal);
            Exit = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 300), CursorType.Normal);
            FullScreen = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 400), CursorType.Normal);
            primaryButtons = new List<Button>() { play, Load, Exit, FullScreen };

            StartGameInTown = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 100), CursorType.Normal);
            StartGameInWilderness = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 200), CursorType.Normal);
           StartGameInUnderWorld = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 400), CursorType.Normal);

            chooseWorldSizeButtons = new List<Button>() { StartGameInTown, StartGameInWilderness, StartGameInUnderWorld };
            //--------------------------------------
            //Load spritefonts
            font = Game1.AllTextures.MenuText;

            mySave = new SaveLoadManager();
            BackDrop = content.Load<Texture2D>("MainMenu/MainMenuBackDrop");

            cloud1 = content.Load<Texture2D>("MainMenu/cloud1");
            clouds = new List<Sprite>();
            for (int i = 0; i < 5; i++)
            {
                clouds.Add(new Sprite(graphics, cloud1, new Rectangle(0, 0, cloud1.Width, cloud1.Height),
                    new Vector2(Game1.Utility.RGenerator.Next(0, 1000), Game1.Utility.RGenerator.Next(0, 300)), cloud1.Width, cloud1.Height)
                { Speed = (float)Game1.Utility.RFloat(5f, 50f) });
            }

        }

        public void LoadBackGround()
        {
            BackDrop = content.Load<Texture2D>("MainMenu/MainMenuBackDrop");

            cloud1 = content.Load<Texture2D>("MainMenu/cloud1");
            clouds = new List<Sprite>();
            for (int i = 0; i < 5; i++)
            {
                clouds.Add(new Sprite(graphics, cloud1, new Rectangle(0, 0, cloud1.Width, cloud1.Height),
                    new Vector2(Game1.Utility.RGenerator.Next(0, 1000), Game1.Utility.RGenerator.Next(0, 300)), cloud1.Width, cloud1.Height)
                { Speed = (float)Game1.Utility.RFloat(5f, 50f) });
            }
        }
        public void UnloadContent()
        {
            content.Unload();
            clouds = null;
        }


        public void Update(GameTime gameTime, MouseManager mouse, Game1 game)
        {
            // customMouse.Update();
            //--------------------------------------
            //Update Buttons
            //Game1.SoundManager.TitleInstance.Play();
            Game1.isMyMouseVisible = true;
            for (int i = 0; i < clouds.Count; i++)
            {
                if (clouds[i].Position.X > Game1.ScreenWidth)
                {
                    clouds[i].Position.X = -300;
                    clouds[i].Position.Y = Game1.Utility.RGenerator.Next(0, 300);
                }
                clouds[i].Position.X += (float)(clouds[i].Speed * gameTime.ElapsedGameTime.TotalSeconds);

            }
            switch (menuState)
            {
                case MenuState.primary:
                    foreach (Button button in primaryButtons)
                    {
                        button.Update(mouse);
                    }

                    //--------------------------------------
                    //Check Conditions
                    if (play.isClicked)
                    {
                        menuState = MenuState.play;
                        return;
                    }
                    if (Load.isClicked)
                    {
                        //foreach (ILocation stage in Game1.AllStages)
                        //{
                        //    if (stage == Game1.World)
                        //    {
                        //        Game1.World.LoadPreliminaryContent(1);
                        //    }
                        //    else
                        //    {
                        //        stage.LoadPreliminaryContent();
                        //    }

                        //}
                        //mySave.Load(graphics);
                        //this.menuState = MenuState.primary;
                        //Game1.SwitchStage(0, Stages.World);
                    }
                    if (Exit.isClicked)
                    {
                        game.Exit();
                    }

                    if (FullScreen.isClicked)
                    {
                        Game1.FullScreenToggle();
                    }
                    break;

                case MenuState.play:
                    foreach (Button button in chooseWorldSizeButtons)
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
                        menuState = MenuState.primary;
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
                        menuState = MenuState.primary;
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
                        menuState = MenuState.primary;
                        Game1.SwitchStage(0, Stages.UnderWorld);
                    }


                    break;
            }

        }

        public void Draw(GraphicsDevice graphics, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse)

        {

            //GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            Game1.myMouseManager.Draw(spriteBatch, 1f);
            spriteBatch.Draw(BackDrop, new Vector2(0, 0), null, Color.White, 0f, Game1.Utility.Origin, .75f, SpriteEffects.None, .5f);
            for (int i = 0; i < clouds.Count; i++)
            {
                clouds[i].Draw(spriteBatch, .7f);
            }
            //--------------------------------------
            //Draw Buttons
            switch (menuState)
            {
                case MenuState.primary:

                    play.Draw(spriteBatch, font, "Play", play.FontLocation, play.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);

                    Load.Draw(spriteBatch, font, "This does nothing", Load.FontLocation, Load.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    Exit.Draw(spriteBatch, font, "Exit", Exit.FontLocation, Exit.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    FullScreen.Draw(spriteBatch, font, "FullScreen", FullScreen.FontLocation, FullScreen.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    break;

                case MenuState.play:
                    StartGameInTown.Draw(spriteBatch, font, "Go to town", StartGameInTown.FontLocation, StartGameInTown.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    StartGameInWilderness.Draw(spriteBatch, font, "Go to wilderness", StartGameInWilderness.FontLocation, StartGameInWilderness.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    StartGameInUnderWorld.Draw(spriteBatch, font, "Go to underworld", StartGameInWilderness.FontLocation, StartGameInWilderness.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);

                    break;
            }


            spriteBatch.End();
        }

    }
}

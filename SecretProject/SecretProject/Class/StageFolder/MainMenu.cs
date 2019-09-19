
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.UI;

namespace SecretProject.Class.StageFolder
{
    public class MainMenu
    {

        public enum MenuState
        {
            primary = 0,
            chooseWorldSize = 1,

        }

        //--------------------------------------
        //buttons
        public MenuState menuState = MenuState.primary;
        Button reloadMap;
        Button newGame;
        Button Load;
        Button Exit;
        Button FullScreen;

        //ChooseWorldSize Buttons

        Button worldSizeSmall;
        Button worldSizeMedium;
        Button worldSizeLarge;

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
            this.graphics = graphicsDevice;
            this.content = content;

            //--------------------------------------
            //Initialize Buttons
            reloadMap = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 100));
            newGame = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 200));
            Load = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 300));
            Exit = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 400));
            FullScreen = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 500));
            primaryButtons = new List<Button>() { newGame, Load, Exit, reloadMap, FullScreen };

            worldSizeSmall = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 100));
            worldSizeMedium = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 200));
            worldSizeLarge = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 300));
            chooseWorldSizeButtons = new List<Button>() { worldSizeSmall, worldSizeMedium, worldSizeLarge };
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
           // Game1.SoundManager.TitleInstance.Play();
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
            switch(this.menuState)
            {
                case MenuState.primary:
                    foreach (Button button in primaryButtons)
                    {
                        button.Update(mouse);
                    }

                    //--------------------------------------
                    //Check Conditions
                    if (newGame.isClicked)
                    {
                        this.menuState = MenuState.chooseWorldSize;
                        return;
                    }
                    if (Load.isClicked)
                    {
                        foreach (ILocation stage in Game1.AllStages)
                        {
                            if (stage == Game1.World)
                            {
                                Game1.World.LoadPreliminaryContent(1);
                            }
                            else
                            {
                                stage.LoadPreliminaryContent();
                            }

                        }
                        mySave.Load(graphics);
                        this.menuState = MenuState.primary;
                        Game1.SwitchStage(0, (int)Stages.World, gameTime);
                    }
                    if (Exit.isClicked)
                    {
                        game.Exit();
                    }

                    if (reloadMap.isClicked)
                    {
                        Game1.gameStages = Stages.Town;
                    }
                    if (FullScreen.isClicked)
                    {
                        Game1.FullScreenToggle();
                    }
                    break;

                case MenuState.chooseWorldSize:
                    foreach (Button button in chooseWorldSizeButtons)
                    {
                        button.Update(mouse);
                    }
                        if(worldSizeSmall.isClicked)
                        {
                            UnloadContent();
                            foreach (ILocation stage in Game1.AllStages)
                            {
                                if (stage == Game1.World)
                                {
                                  //  Game1.World.LoadPreliminaryContent(1);
                                }
                                else
                                {
                                    stage.LoadPreliminaryContent();
                                }

                            }
                            this.menuState = MenuState.primary;
                            Game1.SwitchStage(0, (int)Stages.Town,gameTime);
                        }
                        else if(worldSizeMedium.isClicked)
                        {
                            UnloadContent();
                            foreach (ILocation stage in Game1.AllStages)
                            {
                                if (stage == Game1.World)
                                {
                                    Game1.World.LoadPreliminaryContent(1);
                                }
                                else
                                {
                                    stage.LoadPreliminaryContent();
                                }

                            }
                            this.menuState = MenuState.primary;
                            Game1.SwitchStage(0, (int)Stages.World,gameTime);
                        }
                        else if(worldSizeLarge.isClicked)
                        {
                            UnloadContent();
                            foreach (ILocation stage in Game1.AllStages)
                            {
                                if (stage == Game1.World)
                                {
                                    Game1.World.LoadPreliminaryContent(3);
                                }
                                else
                                {
                                    stage.LoadPreliminaryContent();
                                }

                            }
                            this.menuState = MenuState.primary;
                            Game1.SwitchStage(0, (int)Stages.Pass,gameTime);
                        }
                    
                    break;
            }
            
        }

        public void Draw(GraphicsDevice graphics, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse)

        {
            //GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            spriteBatch.Draw(BackDrop, new Vector2(0, 0), Color.White);
            for (int i = 0; i < clouds.Count; i++)
            {
                clouds[i].Draw(spriteBatch, .7f);
            }
            //--------------------------------------
            //Draw Buttons
            switch (this.menuState)
            {
                case MenuState.primary:
                    reloadMap.Draw(spriteBatch, font, "Reload Map", reloadMap.FontLocation, Color.CornflowerBlue, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    //reloadMap.Draw(spriteBatch, font, "Reload Map", new Vector2(515, 122), Color.CornflowerBlue);
                    newGame.Draw(spriteBatch, font, "New Game", newGame.FontLocation, Color.CornflowerBlue, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    //resumeGame.Draw(spriteBatch, font, "Resume Game", new Vector2(510, 222), Color.CornflowerBlue);
                    Load.Draw(spriteBatch, font, "Load Game", Load.FontLocation, Color.CornflowerBlue, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    Exit.Draw(spriteBatch, font, "Exit", Exit.FontLocation, Color.CornflowerBlue, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    FullScreen.Draw(spriteBatch, font, "FullScreen", FullScreen.FontLocation, Color.CornflowerBlue, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);

                    //Load.Draw(spriteBatch, font, "Load Game", new Vector2(520, 322), Color.CornflowerBlue); Exit.Draw(spriteBatch, font, "Exit", new Vector2(545, 422), Color.CornflowerBlue);
                    //Exit.Draw(spriteBatch, font, "Exit", new Vector2(545, 422), Color.CornflowerBlue);
                    break;

                case MenuState.chooseWorldSize:
                    worldSizeSmall.Draw(spriteBatch, font, "Small World", worldSizeSmall.FontLocation, Color.Green, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    worldSizeMedium.Draw(spriteBatch, font, "Medium World", worldSizeMedium.FontLocation, Color.Green, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    worldSizeLarge.Draw(spriteBatch, font, "Large World", worldSizeLarge.FontLocation, Color.Green, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
                    break;
            }


            spriteBatch.End();
        }

    }
}

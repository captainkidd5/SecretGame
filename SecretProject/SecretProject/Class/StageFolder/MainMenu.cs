
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
    public class MainMenu// : IStage
    {

        //--------------------------------------
        //buttons
        Button reloadMap;
        Button newGame;
        Button Load;
        Button Exit;
        Button FullScreen;

        Texture2D BackDrop;
        Texture2D cloud1;
        

        List<Button> allButtons;

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
            reloadMap = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48,176,128, 64),  graphicsDevice, new Vector2(1100, 100));
            newGame = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 200));
            Load = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 300));
            Exit = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 400));
            FullScreen = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(1100, 500));
            allButtons = new List<Button>() { newGame, Load, Exit, reloadMap, FullScreen };

            //--------------------------------------
            //Load spritefonts
            font = Game1.AllTextures.MenuText;

            mySave = new SaveLoadManager();
            BackDrop = content.Load<Texture2D>("MainMenu/MainMenuBackDrop");

            cloud1 = content.Load<Texture2D>("MainMenu/cloud1");
            clouds = new List<Sprite>();
            for(int i = 0; i < 5; i++)
            { 
                clouds.Add(new Sprite(graphics, cloud1, new Rectangle(0, 0, cloud1.Width, cloud1.Height),
                    new Vector2(Game1.Utility.RGenerator.Next(0, 1000), Game1.Utility.RGenerator.Next(0, 300)), cloud1.Width, cloud1.Height)
                { Speed = (float)Game1.Utility.RFloat(5f, 50f) });
            }
        }

        public void LoadContent()
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
            Game1.isMyMouseVisible = true;
            for (int i = 0; i < clouds.Count; i++)
            {
               if(clouds[i].Position.X > Game1.ScreenWidth)
                {
                    clouds[i].Position.X = -300;
                    clouds[i].Position.Y = Game1.Utility.RGenerator.Next(0, 300);
                }
                clouds[i].Position.X+= (float)(clouds[i].Speed * gameTime.ElapsedGameTime.TotalSeconds);

        }
            foreach (Button button in allButtons)
            {
                button.Update(mouse);
            }

            //--------------------------------------
            //Check Conditions
            if (newGame.isClicked)
            {
                UnloadContent();
                Game1.SwitchStage(0, (int)Stages.Wilderness);
                Game1.Player.UserInterface.IsEscMenu = false;
            }
            if(Load.isClicked)
            {
                mySave.Load(graphics);
                Game1.gameStages = Stages.Wilderness;
                Game1.Player.UserInterface.IsEscMenu = false;
            }
            if (Exit.isClicked)
            {
                game.Exit();
            }

            if(reloadMap.isClicked)
            {
                Game1.Player.UserInterface.IsEscMenu = false;
                Game1.gameStages = Stages.Wilderness;
            }
            if(FullScreen.isClicked)
            {
                Game1.FullScreenToggle();
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

            reloadMap.Draw(spriteBatch, font, "Reload Map", reloadMap.FontLocation, Color.CornflowerBlue, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
            //reloadMap.Draw(spriteBatch, font, "Reload Map", new Vector2(515, 122), Color.CornflowerBlue);
            newGame.Draw(spriteBatch,  font, "New Game", newGame.FontLocation, Color.CornflowerBlue, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
            //resumeGame.Draw(spriteBatch, font, "Resume Game", new Vector2(510, 222), Color.CornflowerBlue);
            Load.Draw(spriteBatch, font, "Load Game", Load.FontLocation, Color.CornflowerBlue, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
            Exit.Draw(spriteBatch, font, "Exit", Exit.FontLocation, Color.CornflowerBlue, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);
            FullScreen.Draw(spriteBatch, font, "FullScreen", FullScreen.FontLocation, Color.CornflowerBlue, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth);

            //Load.Draw(spriteBatch, font, "Load Game", new Vector2(520, 322), Color.CornflowerBlue); Exit.Draw(spriteBatch, font, "Exit", new Vector2(545, 422), Color.CornflowerBlue);
            //Exit.Draw(spriteBatch, font, "Exit", new Vector2(545, 422), Color.CornflowerBlue);

            
            spriteBatch.End();
        }

    }
}

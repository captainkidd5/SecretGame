
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.UI;

namespace SecretProject.Class.StageFolder
{
    public class MainMenu// : IStage
    {

        //--------------------------------------
        //buttons
        Button newGame;
        Button Join;
        Button Load;
        Button Exit;
        

        List<Button> allButtons;

        //--------------------------------------
        //fonts
        private SpriteFont font;

        //--------------------------------------
        //button textures

        SaveLoadManager mySave;

        GraphicsDevice graphics;
        ContentManager content;
        



        public MainMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse, UserInterface userInterface)
        {
            //--------------------------------------
            //Load button textures
            this.graphics = graphicsDevice;
            this.content = content;

            //--------------------------------------
            //Initialize Buttons
            newGame = new Button(Game1.AllTextures.BasicButton, graphicsDevice, new Vector2(500, 100));
            Join = new Button(Game1.AllTextures.BasicButton, graphicsDevice, new Vector2(500, 200));
            Load = new Button(Game1.AllTextures.BasicButton, graphicsDevice, new Vector2(500, 300));
            Exit = new Button(Game1.AllTextures.BasicButton, graphicsDevice, new Vector2(500, 400));

            allButtons = new List<Button>() { Join, Load, Exit, newGame };

            //--------------------------------------
            //Load spritefonts
            font = Game1.AllTextures.MenuText;

            mySave = new SaveLoadManager();

        }

        public void Update(GameTime gameTime, MouseManager mouse, Game1 game)
        {
            // customMouse.Update();
            //--------------------------------------
            //Update Buttons
            Game1.isMyMouseVisible = true;

            foreach(Button button in allButtons)
            {
                button.Update(mouse);
            }

            //--------------------------------------
            //Check Conditions
            if (Join.isClicked)
            {
                Game1.gameStages = Stages.Iliad;
                Game1.userInterface.IsEscMenu = false;
            }
            if(Load.isClicked)
            {
                mySave.Load(graphics);
                Game1.gameStages = Stages.Iliad;
                Game1.userInterface.IsEscMenu = false;
            }
            if (Exit.isClicked)
            {
                game.Exit();
            }

            if(newGame.isClicked)
            {
                Game1.ReloadHome(graphics, content);
                Game1.userInterface.IsEscMenu = false;
                Game1.gameStages = Stages.Iliad;
            }
        }

        public void Draw(GraphicsDevice graphics, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse)

        {
            //GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            //--------------------------------------
            //Draw Buttons

            newGame.Draw(spriteBatch, font, "Reload Map", new Vector2(515, 122), Color.CornflowerBlue);
            Join.Draw(spriteBatch, font, "Resume Game", new Vector2(510, 222), Color.CornflowerBlue);
            Load.Draw(spriteBatch, font, "Load Game", new Vector2(520, 322), Color.CornflowerBlue); Exit.Draw(spriteBatch, font, "Exit", new Vector2(545, 422), Color.CornflowerBlue);
            Exit.Draw(spriteBatch, font, "Exit", new Vector2(545, 422), Color.CornflowerBlue);


            spriteBatch.End();
        }

    }
}

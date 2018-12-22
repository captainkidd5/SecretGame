
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.UI;

namespace SecretProject.Class.Stage
{
    public class MainMenu : Component
    {

        //--------------------------------------
        //buttons
        Button Join;
        Button Save;
        Button Exit;
        

        List<Button> allButtons;

        //--------------------------------------
        //fonts
        private SpriteFont font;

        //--------------------------------------
        //button textures
        Texture2D join;
        Texture2D save;
        Texture2D exit;

        SaveFile mySave;



        public MainMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse, UserInterface userInterface) : base(game, graphicsDevice, content, mouse, userInterface)
        {
            //--------------------------------------
            //Load button textures
            join = content.Load<Texture2D>("Button/basicButton");
            save = content.Load<Texture2D>("Button/basicButton");
            exit = content.Load<Texture2D>("Button/basicButton");

            //--------------------------------------
            //Initialize Buttons
            Join = new Button(join, graphicsDevice, customMouse, new Vector2(500, 100));
            Save = new Button(save, graphicsDevice, customMouse, new Vector2(500, 200));
            Exit = new Button(exit, graphicsDevice, customMouse, new Vector2(500, 300));

            allButtons = new List<Button>() { Join, Save, Exit };

            //--------------------------------------
            //Load spritefonts
            font = content.Load<SpriteFont>("SpriteFont/MenuText");

            mySave = new SaveFile();

        }

        public override void Update(GameTime gameTime)
        {
            customMouse.Update();
            //--------------------------------------
            //Update Buttons

            foreach(Button button in allButtons)
            {
                button.Update();
            }

            //--------------------------------------
            //Check Conditions
            if (Join.isClicked)
            {
                game.gameStages = Stages.Iliad;
                UserInterface.IsEscMenu = false;
            }
            if(Save.isClicked)
            {
                mySave.Load();
                game.gameStages = Stages.Iliad;
                UserInterface.IsEscMenu = false;
            }
            if (Exit.isClicked)
            {
                game.Exit();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            //--------------------------------------
            //Draw Buttons
            
            Exit.Draw(spriteBatch, font, "Exit", new Vector2(545, 322), Color.CornflowerBlue);
            Join.Draw(spriteBatch, font, "New Game", new Vector2(525, 122), Color.CornflowerBlue);
            Save.Draw(spriteBatch, font, "Load Game", new Vector2(520, 222), Color.CornflowerBlue);


            spriteBatch.End();
        }

    }
}

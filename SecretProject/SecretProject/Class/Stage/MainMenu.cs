
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;

namespace SecretProject.Class.Stage
{
    class MainMenu : Component
    {

        //--------------------------------------
        //buttons
        Button Join;
        Button Exit;

        List<Button> allButtons;

        //--------------------------------------
        //fonts
        private SpriteFont font;

        //--------------------------------------
        //button textures
        Texture2D join;
        Texture2D exit;



        public MainMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse) : base(game, graphicsDevice, content, mouse)
        {
            //--------------------------------------
            //Load button textures
            join = content.Load<Texture2D>("Button/basicButton");
            exit = content.Load<Texture2D>("Button/basicButton");

            //--------------------------------------
            //Initialize Buttons
            Join = new Button(join, graphicsDevice, customMouse, new Vector2(500, 100));
            Exit = new Button(exit, graphicsDevice, customMouse, new Vector2(500, 300));

            allButtons = new List<Button>() { Join, Exit };

            //--------------------------------------
            //Load spritefonts
            font = content.Load<SpriteFont>("SpriteFont/MenuText");


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
            Join.Draw(spriteBatch, font, "Play", new Vector2(545, 120), Color.CornflowerBlue);



            spriteBatch.End();


        }

    }
}

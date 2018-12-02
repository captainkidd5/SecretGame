using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



using SecretProject.Class.MenuStuff;

namespace SecretProject.Class.Stage
{
    class MainMenu : Component
    {

        //--------------------------------------
        //buttons
        Button Join;
        Button Exit;

        //--------------------------------------
        //fonts
        private SpriteFont font;

        //--------------------------------------
        //button textures
        Texture2D join;
        Texture2D exit;



        public MainMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseState mouse) : base(game, graphicsDevice, content, mouse)
        {
            //--------------------------------------
            //Load button textures
            join = content.Load<Texture2D>("Button/basicButton");
            exit = content.Load<Texture2D>("Button/basicButton");

            //--------------------------------------
            //Initialize Buttons
            Join = new Button(join, graphicsDevice, new Vector2(500, 100));
            Exit = new Button(exit, graphicsDevice, new Vector2(500, 300));

            //--------------------------------------
            //Load spritefonts
            font = content.Load<SpriteFont>("SpriteFont/MenuText");


        }

        public override void Update(GameTime gameTime, MouseState mouse)
        {

            //--------------------------------------
            //Update Join (play)
            Join.Update(mouse);
            if (Join.isClicked == true)
                game.gameStages = Stages.Iliad;

            Join.isClicked = false;

            //--------------------------------------
            //Update Exit
            Exit.Update(mouse);
            if (Exit.isClicked == true) game.Exit();

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            //--------------------------------------
            //Draw Buttons
            Join.Draw(spriteBatch);
            Exit.Draw(spriteBatch);

            //--------------------------------------
            //Draw Text
            spriteBatch.DrawString(font, "Exit", new Vector2(530, 310), Color.CornflowerBlue);
            spriteBatch.DrawString(font, "Play", new Vector2(530, 110), Color.CornflowerBlue);

            spriteBatch.End();


        }





    }
}

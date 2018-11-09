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

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using SecretProject.Class.MenuStuff;

namespace SecretProject.Class.Stage
{
    class MainMenu : Component
    {


        Button Join;
        Button Exit;
        private SpriteFont font;


        public MainMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseState mouse) : base(game, graphicsDevice, content, mouse)
        {
            Join = new Button(content.Load<Texture2D>("Button/basicButton"), graphicsDevice, new Vector2(500, 100));
            Exit = new Button(content.Load<Texture2D>("Button/basicButton"), graphicsDevice, new Vector2(500, 300));

            font = content.Load<SpriteFont>("SpriteFont/MenuText");


        }

        public override void Update(GameTime gameTime, MouseState mouse)
        {


            Join.Update(mouse);
            if (Join.isClicked == true)
                game.gameStages = Stages.Iliad;

            Join.isClicked = false;





            Exit.Update(mouse);
            if (Exit.isClicked == true) game.Exit();



        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            Join.Draw(spriteBatch);
            Exit.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Exit", new Vector2(530, 310), Color.CornflowerBlue);
            spriteBatch.DrawString(font, "Play", new Vector2(530, 110), Color.CornflowerBlue);
            spriteBatch.End();


        }





    }
}

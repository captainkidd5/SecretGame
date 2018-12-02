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

namespace SecretProject.Class.UI
{
    class ToolBar : Component
    {
        //--------------------------------------
        //Textures
        public Texture2D Background { get; set; }
        private Texture2D _toolBarButton;

        //--------------------------------------
        //Fonts
        private SpriteFont _font;


        //--------------------------------------
        //Buttons
        private Button _openInventory;
        private Button _inGameMenu;

        //button List
        private List<Button> allButtons;



        public ToolBar(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseState mouse) : base(game, graphicsDevice, content, mouse)
        {
            //--------------------------------------
            //initialize SpriteFonts
            _font = content.Load<SpriteFont>("SpriteFont/MenuText");

            //--------------------------------------
            //Initialize Textures
            this._toolBarButton = content.Load<Texture2D>("Button/ToolBarButton");
            this.Background = content.Load<Texture2D>("Button/ToolBar");

            //--------------------------------------
            //Initialize Buttons
            _inGameMenu = new Button(_toolBarButton, graphicsDevice, new Vector2(367, 635));
            _openInventory = new Button(_toolBarButton, graphicsDevice, new Vector2(433, 635));

            //--------------------------------------
            //Button List Stuff
            allButtons = new List<Button>(20);
            allButtons.Add(_openInventory);
            allButtons.Add(_inGameMenu);
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //--------------------------------------
            //Draw Background
            spriteBatch.Draw(Background, new Vector2(320, 635));

            //--------------------------------------
            //Draw Buttons
            foreach (Button button in allButtons)
            {
                button.Draw(spriteBatch);
            }

            //--------------------------------------
            //Draw Text
            spriteBatch.DrawString(_font, "Menu", new Vector2(377, 660), Color.CornflowerBlue);
            spriteBatch.DrawString(_font, "Inv", new Vector2(450, 660), Color.CornflowerBlue);


            spriteBatch.End();
        }

        public override void Update(GameTime gameTime, MouseState mouse)
        {

            //--------------------------------------
            //Update Buttons
            foreach (Button button in allButtons)
            {
                button.Update(mouse);
                
            }

            //--------------------------------------
            //Switch GameStages on click
            if (_inGameMenu.isClicked == true)
                game.gameStages = Stages.MainMenu;
            _openInventory.isClicked = false;
            
        }
    }
}

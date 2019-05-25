using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.SpriteFolder;

namespace SecretProject.Class.NPCStuff
{
    public class Elixir : INPC
    {
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public AnimatedSprite[] NPCAnimatedSprite { get; set; }
        public Texture2D Texture { get; set; } = Game1.AllTextures.ElixirSpriteSheet;

        public Rectangle NPCRectangle { get {return new Rectangle((int) Position.X + 2, (int) Position.Y + 20, ((int) Texture.Width / FrameNumber) -2, (int) Texture.Height - 25);}}


    //0 = down, 1 = left, 2 =  right, 3 = up
    public int CurrentDirection { get; set; } = 0;

        

        public int FrameNumber { get; set; } = 25;

        public Elixir(string name, Vector2 position, GraphicsDevice graphics)
        {
            NPCAnimatedSprite = new AnimatedSprite[26];

            NPCAnimatedSprite[0] = new AnimatedSprite(graphics, Game1.AllTextures.ElixirSpriteSheet, 1, 25, 25,0,1,6);
            NPCAnimatedSprite[1] = new AnimatedSprite(graphics, Game1.AllTextures.ElixirSpriteSheet, 1, 25, 25, 7,1, 12);
            NPCAnimatedSprite[2] = new AnimatedSprite(graphics, Game1.AllTextures.ElixirSpriteSheet, 1, 25, 25, 13, 1, 19);
            NPCAnimatedSprite[3] = new AnimatedSprite(graphics, Game1.AllTextures.ElixirSpriteSheet, 1, 25, 25, 19, 1, 25);

            this.Name = "Elixir";
            this.Position = position;

        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {

            switch (CurrentDirection)
            {
                case 0:
                    NPCAnimatedSprite[0].Update(gameTime);
                    break;
                case 1:
                    NPCAnimatedSprite[1].Update(gameTime);
                    break;
                case 2:
                    NPCAnimatedSprite[2].Update(gameTime);
                    break;
                case 3:
                    NPCAnimatedSprite[3].Update(gameTime);
                    break;
            }
            if(mouse.IsHovering(this.NPCRectangle) && mouse.IsClicked)
             {
                  Game1.userInterface.IsShopMenu = true;
              }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (CurrentDirection)
            {
                case 0:
                    NPCAnimatedSprite[0].Draw(spriteBatch, Position, .4f);
                    break;
                case 1:
                    NPCAnimatedSprite[1].Draw(spriteBatch, Position, .4f);
                    break;
                case 2:
                    NPCAnimatedSprite[2].Draw(spriteBatch, Position, .4f);
                    break;
                case 3:
                    NPCAnimatedSprite[3].Draw(spriteBatch, Position, .4f);
                    break;
            }
        }
    }
}

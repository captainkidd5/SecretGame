using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class CraftingMenu
    {
        public bool Layer1Expanded { get; set; } = true;
        private Rectangle Layer1SourceRectangle;
        private Vector2 layer1DrawPosition;
        public Rectangle Layer1TriggerBox { get { return new Rectangle((int)layer1DrawPosition.X, (int)layer1DrawPosition.Y, Layer1SourceRectangle.Width, Layer1SourceRectangle.Height); } }

        private List<Button> layer1Buttons;
        GraphicsDevice graphics;
        public CraftingMenu(GraphicsDevice graphics)
        {
            this.graphics = graphics;
            Layer1SourceRectangle = new Rectangle(1168, 240, 64, 576);
            layer1DrawPosition = new Vector2(25, 25);

            layer1Buttons = new List<Button>()
            {
                new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1104, 240, 64, 64), graphics, new Vector2(layer1DrawPosition.X, layer1DrawPosition.Y)),
                new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1104, 304, 64, 64), graphics, new Vector2(layer1DrawPosition.X, layer1DrawPosition.Y + 64)),
                new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1104, 368, 64, 64), graphics, new Vector2(layer1DrawPosition.X, layer1DrawPosition.Y + 128)),
                new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1104, 240, 64, 64), graphics, new Vector2(layer1DrawPosition.X, layer1DrawPosition.Y + 192))

            };
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            if (mouse.IsHovering(Layer1TriggerBox))
            {
                Layer1Expanded = true;
            }
            else
            {
                Layer1Expanded = false;
            }
            if(Layer1Expanded)
            {
                for(int i=0; i< layer1Buttons.Count; i++)
                {
                    layer1Buttons[i].Update(mouse);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(Layer1Expanded)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet,  layer1DrawPosition, Layer1SourceRectangle, Color.White, 0f, Game1.Utility.Origin,1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                for (int i = 0; i < layer1Buttons.Count; i++)
                {
                    layer1Buttons[i].Draw(spriteBatch);
                }
            }
        }
    }
}

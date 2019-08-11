using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff;

namespace SecretProject.Class.UI
{
    public class CraftingMenu
    {
        //LAYER1
        public bool Layer1Expanded { get; set; }
        private Rectangle Layer1SourceRectangle;
        private Vector2 layer1DrawPosition;
        public Rectangle Layer1TriggerBox { get { return new Rectangle((int)layer1DrawPosition.X, (int)layer1DrawPosition.Y, Layer1SourceRectangle.Width, Layer1SourceRectangle.Height); } }
        public Rectangle Layer2TriggerBox { get { return new Rectangle((int)layer2DrawPosition.X, (int)layer2DrawPosition.Y, layer2SourceRectangle.Width, layer2SourceRectangle.Height); } }
        private List<Button> layer1Buttons;
        public List<CraftingSlot> CraftingSlots { get; set; }

        public bool Layer2ExpandedOld { get; set; }
        public bool Layer2ExpandedNew { get; set; }
        private Rectangle layer2SourceRectangle;
        private Vector2 layer2DrawPosition;

        public bool Layer3Expanded { get; set; }
        private Rectangle layer3SourceRectangle;
        private Vector2 layer3DrawPosition;

        GraphicsDevice graphics;

        public CraftingGuide CraftingGuide { get; set; }

        public CraftingMenu(GraphicsDevice graphics)
        {
            this.graphics = graphics;

            Layer1SourceRectangle = new Rectangle(1168, 240, 64, 576);
            layer1DrawPosition = new Vector2(25, 25);

            layer2SourceRectangle = new Rectangle(1264, 240, 240, 162);

            layer1Buttons = new List<Button>()
            {
                new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1104, 240, 64, 64), graphics, new Vector2(layer1DrawPosition.X, layer1DrawPosition.Y)),
                new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1104, 304, 64, 64), graphics, new Vector2(layer1DrawPosition.X, layer1DrawPosition.Y + 64)),
                new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1104, 368, 64, 64), graphics, new Vector2(layer1DrawPosition.X, layer1DrawPosition.Y + 128)),
                new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1104, 240, 64, 64), graphics, new Vector2(layer1DrawPosition.X, layer1DrawPosition.Y + 192)),
                new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1104, 368, 64, 64), graphics, new Vector2(layer1DrawPosition.X, layer1DrawPosition.Y + 256)),
                new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1104, 368, 64, 64), graphics, new Vector2(layer1DrawPosition.X, layer1DrawPosition.Y + 320)),
                new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1104, 368, 64, 64), graphics, new Vector2(layer1DrawPosition.X, layer1DrawPosition.Y + 384))
                //new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1104, 240, 64, 64), graphics, new Vector2(layer1DrawPosition.X, layer1DrawPosition.Y + 192))

            };

            CraftingSlots = new List<CraftingSlot>()
            {
                new CraftingSlot(143, layer2DrawPosition, graphics)
            };


            layer3SourceRectangle = new Rectangle(1152, 112, 164, 100);

            foreach(Button button in layer1Buttons)
            {
                
            }
        }

        public void LoadContent(ContentManager content)
        {
            CraftingGuide = content.Load<CraftingGuide>("Item/Crafting/CraftingGuide");
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            Layer2ExpandedNew = false;
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
                    if(layer1Buttons[i].IsHovered)
                    {
                        layer2DrawPosition = new Vector2(layer1Buttons[i].Position.X + 64, layer1Buttons[i].Position.Y);
                        Layer2ExpandedNew = true;
                        foreach(CraftingSlot slot in CraftingSlots)
                        {
                            slot.Button.Update(mouse);
                        }
                    }
                }


            }
            if(Layer2ExpandedOld && mouse.MouseRectangle.Intersects(Layer2TriggerBox))
            {
                Layer2ExpandedNew = true;
              //  Console.WriteLine(Layer2ExpandedNew);
            }
            Layer2ExpandedOld = Layer2ExpandedNew;

            if(Layer2ExpandedNew)
            {
                //length from top of texture to top of yellow arrow for y
                layer3DrawPosition = new Vector2(layer2DrawPosition.X + layer2SourceRectangle.Width, layer2DrawPosition.Y + 44);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Layer2ExpandedNew)
            {
                Layer1Expanded = true;
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, layer2DrawPosition, layer2SourceRectangle, Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                foreach (CraftingSlot slot in CraftingSlots)
                {
                    slot.Button.Draw(spriteBatch);
                }
                //spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, layer3DrawPosition, layer3SourceRectangle, Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            }
            if (Layer1Expanded)
            {
              //  spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet,  layer1DrawPosition, Layer1SourceRectangle, Color.White, 0f, Game1.Utility.Origin,1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                for (int i = 0; i < layer1Buttons.Count; i++)
                {
                    layer1Buttons[i].Draw(spriteBatch);
                }
                
                
            }
            
        }
    }

    public class CraftingSlot
    {
        public int ItemID { get; set; }
        public Button Button { get; set; }
        public CraftingSlot(int itemID, Vector2 drawPosition, GraphicsDevice graphics)
        {
            Item item = Game1.ItemVault.GenerateNewItem(itemID, drawPosition);
            Button = new Button(item.ItemSprite.AtlasTexture, item.SourceTextureRectangle, graphics, drawPosition);
        }
    }

}

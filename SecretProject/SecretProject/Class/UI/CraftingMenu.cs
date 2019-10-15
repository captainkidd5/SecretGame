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
    public class CraftingMenu : IExclusiveInterfaceComponent
    {
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GraphicsDevice GraphicsDevice { get; set; }
        public Rectangle BackDropSourceRectangle { get; set; }
        public Vector2 BackDropPosition { get; set; }

        public float BackDropScale { get; set; }

        public int ActiveTab { get; set; }
        public Tab[] Tabs { get; set; }
        public int ActiveRecipe { get; set; }

        public CraftingGuide CraftingGuide { get; set; }


       // public List< MyProperty { get; set; }


        public CraftingMenu(ContentManager content, GraphicsDevice graphics)
        {
            this.GraphicsDevice = graphics;
            BackDropSourceRectangle = new Rectangle(304, 352, 224, 224);
            BackDropPosition = new Vector2(Game1.ScreenWidth /4, (int)(Game1.ScreenHeight / 16));
            BackDropScale = 3f;
            CraftingGuide = content.Load<CraftingGuide>("Item/Crafting/CraftingGuide");
            Tabs = new Tab[6];
            for(int i = 0; i < Tabs.Length; i++)
            {
                Tabs[i] = new Tab(GraphicsDevice, new Vector2(BackDropPosition.X, BackDropPosition.Y + i * 64), new Rectangle(272,384, 32,32), BackDropScale);
            }
                
            
            ActiveTab = 0;
        }

        public void Update(GameTime gameTime)
        {
            for(int i =0; i < Tabs.Length;i++)
            {
                Tabs[i].Update(gameTime);
                if(Tabs[i].Button.isClicked)
                {
                    ActiveTab = i;
                    Tabs[i].IsActive = true;
                    Tabs[i].ButtonColorMultiplier = .5f;
                }
                else
                {
                    Tabs[i].IsActive = false;
                    Tabs[i].ButtonColorMultiplier = 1f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, BackDropPosition, BackDropSourceRectangle, Color.White, 0f, Game1.Utility.Origin, BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
        }
    }


    public class Tab
    {
        public int Index { get; set; }
        public bool IsActive { get; set; }
        public int ActivePage { get; set; }
        public List<ToolTipPage> Pages { get; set; }

        public Button Button { get; set; }

        public Vector2 PositionToDraw { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public float ButtonColorMultiplier { get; set; }
        public Tab(GraphicsDevice graphics, Vector2 positionToDraw, Rectangle sourceRectangle, float scale)
        {
            this.PositionToDraw = positionToDraw;
            this.SourceRectangle = sourceRectangle;
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, SourceRectangle, graphics, PositionToDraw, CursorType.Normal, scale);
            ButtonColorMultiplier = 1f;
        }

        public void Update(GameTime gameTime)
        {
            Button.Update(Game1.myMouseManager);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           // Button.Draw()
        }//
    }


    public class ToolTipPage
    {
        public bool IsActive { get; set; }
        public int Index { get; set; }
        public List<ToolTip> ToolTips { get; set; }
        public ToolTipPage()
        {

        }
    }

    public class ToolTip
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Rectangle BackGroundSourceRectangle { get; set; }
        public Vector2 Position { get; set; }
        public Button Button { get; set; }
        public Item Item { get; set; }
        public CraftingMenu CraftingMenu { get; set; }

        public ToolTip(int itemToCraftID, Vector2 position,GraphicsDevice graphics, CraftingMenu craftingMenu)
        {
            this.Position = position;
            this.GraphicsDevice = graphics;
            Item = Game1.ItemVault.GenerateNewItem(itemToCraftID, null);
            Button = new Button(Game1.AllTextures.ItemSpriteSheet, Item.SourceTextureRectangle, GraphicsDevice, this.Position, CursorType.Normal, 3f);
            CraftingMenu = craftingMenu;
        }

        public void Update(GameTime gameTime)
        {
            if(this.Button.isClicked)
            {
                CraftingMenu.ActiveRecipe = this.Item.ID;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Button.Draw(spriteBatch, Button.ItemSourceRectangleToDraw, BackGroundSourceRectangle, Game1.AllTextures.MenuText, "", Position,  Color.White, 3f,Game1.Utility.StandardButtonDepth + .01f);
        }
    }




}

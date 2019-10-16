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

namespace SecretProject.Class.UI
{
    public class ProgressBook : IExclusiveInterfaceComponent
    {
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public GraphicsDevice GraphicsDevice { get; set; }
        public Rectangle BackDropSourceRectangle { get; set; }
        public Vector2 BackDropPosition { get; set; }
        public float BackDropScale { get; set; }

        public List<CategoryTab> Tabs { get; set; }
        public int ActiveTab { get; set; }

        public ProgressBook(ContentManager content, GraphicsDevice graphics)
        {
            this.GraphicsDevice = graphics;
            this.BackDropSourceRectangle = new Rectangle(432, 80, 192, 240);
            this.BackDropPosition = new Vector2(Game1.ScreenWidth / 4, -0);
            this.BackDropScale = 3f;
            Tabs = new List<CategoryTab>()
            {
                new CategoryTab(this,this.GraphicsDevice, new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width * this.BackDropScale, this.BackDropPosition.Y + 144),
                new Rectangle(624, 101, 27,32), this.BackDropScale),

                 new CategoryTab(this,this.GraphicsDevice, new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width * this.BackDropScale, this.BackDropPosition.Y + 288),
                new Rectangle(624, 101, 27,32), this.BackDropScale),

                  new CategoryTab(this,this.GraphicsDevice, new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width * this.BackDropScale, this.BackDropPosition.Y + 432),
                new Rectangle(624, 101, 27,32), this.BackDropScale)
            };

            Tabs[0].Pages.Add(new ProgressPage());
            Tabs[1].Pages.Add(new ProgressPage());
            Tabs[2].Pages.Add(new ProgressPage());

        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].Button.Update(Game1.myMouseManager);
                if (Tabs[i].Button.isClicked)
                {
                    this.ActiveTab = i;
                }
            }

            Tabs[ActiveTab].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackDropPosition, this.BackDropSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].Button.DrawNormal(spriteBatch, Tabs[i].Button.Position, Tabs[i].Button.BackGroundSourceRectangle, Color.White * Tabs[i].ButtonColorMultiplier, 0f, Game1.Utility.Origin, Tabs[i].Button.HitBoxScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            }

            Tabs[ActiveTab].Draw(spriteBatch);
        }
    }

    public class CategoryTab
    {
        public ProgressBook ProgressBook { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }
        public int ActivePage { get; set; }
        public List<ProgressPage> Pages { get; set; }

        public Button Button { get; set; }

        public Vector2 PositionToDraw { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public float ButtonColorMultiplier { get; set; }
        public CategoryTab(ProgressBook book, GraphicsDevice graphics, Vector2 positionToDraw, Rectangle sourceRectangle, float scale)
        {
            this.ProgressBook = book;
            this.PositionToDraw = positionToDraw;
            this.SourceRectangle = sourceRectangle;
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, SourceRectangle, graphics, PositionToDraw, CursorType.Normal, scale);
            this.ButtonColorMultiplier = 1f;
            this.ActivePage = 0;
            this.Pages = new List<ProgressPage>();
        }

        public void Update(GameTime gameTime)
        {

            // Pages[ActivePage].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.DrawString(Game1.AllTextures.MenuText, ActivePage.ToString(), ProgressBook.BackDropPosition, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
            this.Pages[ActivePage].Draw(spriteBatch);


        }


    }

    public class ProgressPage
    {
        public List<ProgressPageRequirement> Requirements { get; set; }

        public ProgressPage()
        {
            Requirements = new List<ProgressPageRequirement>();

        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.Requirements.Count; i++)
            {
                Requirements[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.Requirements.Count; i++)
            {
                Requirements[i].Draw(spriteBatch);
            }
        }
    }

    public class ProgressPageRequirement
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Button Button { get; set; }
        public bool Satisfied { get; set; }
        public Item Item { get; set; }
        public int CurrentCount { get; set; }
        public int CountRequired { get; set; }
        public float ColorMultiplier { get; set; }

        public ProgressPageRequirement(GraphicsDevice graphics, Button button, Item item, int countRequired)
        {
            this.GraphicsDevice = graphics;
            this.Button = button;
            this.Satisfied = false;
            this.Item = item;
            this.CurrentCount = 0;
            this.CountRequired = countRequired;
            this.ColorMultiplier = 1f;
        }

        public void Update(GameTime gameTime)
        {
            Button.Update(Game1.myMouseManager);
            //this.CurrentCount = Game1.Player.Inventory.FindNumberOfItemInInventory(Item.ID);
            //if (this.CurrentCount >= CountRequired)
            //{
            //    this.Satisfied = true;
            //    ColorMultiplier = 1f;
            //}
            //else
            //{
            //    this.Satisfied = false;
            //    ColorMultiplier = .5f;
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Button.DrawNormal(spriteBatch, Button.Position, Button.BackGroundSourceRectangle, Color.White * ColorMultiplier, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, .95f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.CurrentCount.ToString() + "/" + this.CountRequired.ToString(),
                new Vector2(Button.Position.X, Button.Position.Y + 50), Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, .95f);
        }
    }
}

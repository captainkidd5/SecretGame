using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.SanctuaryStuff;

namespace SecretProject.Class.UI.SanctuaryStuff
{
    public class CompletionGuide
    {
        public GraphicsDevice Graphics { get; set; }


        //Background Information
        public Rectangle BackGroundSourceRectangle { get; set; }
        public Vector2 BackGroundPosition { get; set; }
        public float BackGroundScale { get; set; }

        public List<CompletionCategory> CategoryTabs { get; set; }
        public SanctuaryHolder SanctuaryHolder { get; set; }
        public CompletionCategory ActiveTab { get; set; }

        public CompletionGuide(GraphicsDevice graphicsDevice, SanctuaryHolder sanctuaryHolder)
        {
            this.Graphics = graphicsDevice;
            this.SanctuaryHolder = sanctuaryHolder;
            this.BackGroundSourceRectangle = new Rectangle(448, 112, 272, 160);
            this.BackGroundPosition = new Vector2(Game1.Utility.CenterScreenX, Game1.Utility.CenterScreenY);
            this.BackGroundScale = 2f;
            CategoryTabs = new List<CompletionCategory>()
            {
                new CompletionCategory("Flora", Graphics,BackGroundPosition,new Rectangle(464,80, 48, 32), BackGroundScale),
                new CompletionCategory("Minerals", Graphics,BackGroundPosition,new Rectangle(528,80, 48, 32), BackGroundScale),
                new CompletionCategory("Fauna", Graphics,BackGroundPosition,new Rectangle(592,80, 48, 32), BackGroundScale),
                new CompletionCategory("Special", Graphics,BackGroundPosition,new Rectangle(656,80, 48, 32), BackGroundScale),

            };
            for (int i = 0; i < CategoryTabs.Count; i++)
            {
                CategoryTabs[i].Pages.Add(new CompletionPage());
            }

            for (int i = 0; i < sanctuaryHolder.AllRequirements.Count; i++)
            {
                for (int j = 0; j < CategoryTabs.Count; j++)
                {
                    if (sanctuaryHolder.AllRequirements[i].Tab == j)
                    {

                        CategoryTabs[j].Pages[0].SanctuaryRequirements.Add(new CompletionRequirement(sanctuaryHolder.AllRequirements[i].ItemID, sanctuaryHolder.AllRequirements[i].GIDRequired, sanctuaryHolder.AllRequirements[i].NumberRequired,
                            sanctuaryHolder.AllRequirements[i].Rectangle));
                    }
                }
            }
            ActiveTab = CategoryTabs[0];


        }

        public Vector2 DrawPosition { get; set; }
        public void Update(GameTime gameTime)
        {
           DrawPosition = new Vector2(BackGroundPosition.X - (BackGroundSourceRectangle.Width * BackGroundScale) / 2,
               BackGroundPosition.Y - (BackGroundSourceRectangle.Height * BackGroundScale) / 2);
            for (int tab = 0; tab < CategoryTabs.Count; tab++)
            {
                CategoryTabs[tab].Button.Position = new Vector2(DrawPosition.X + 64 * tab * BackGroundScale, DrawPosition.Y - 32 * BackGroundScale);
                CategoryTabs[tab].Button.Update(Game1.myMouseManager);
                if (CategoryTabs[tab].Button.IsHovered)
                {
                    CategoryTabs[tab].ButtonColorMultiplier = .5f;
                }
                else
                {
                    CategoryTabs[tab].ButtonColorMultiplier = 1f;
                }
                if (CategoryTabs[tab].Button.isClicked)
                {
                    ActiveTab = CategoryTabs[tab];
                }
            }

            ActiveTab.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, DrawPosition, this.BackGroundSourceRectangle,
                   Color.White, 0f, Game1.Utility.Origin, BackGroundScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            ActiveTab.Draw(spriteBatch, new Vector2(DrawPosition.X + 16 * BackGroundScale, DrawPosition.Y + 16 * BackGroundScale), BackGroundScale);
            for (int tab = 0; tab < CategoryTabs.Count; tab++)
            {
                CategoryTabs[tab].Button.DrawNormal(spriteBatch, CategoryTabs[tab].Button.Position, CategoryTabs[tab].Button.BackGroundSourceRectangle, Color.White * CategoryTabs[tab].ButtonColorMultiplier, 0f, Game1.Utility.Origin, CategoryTabs[tab].Button.HitBoxScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            }
        }
    }
}

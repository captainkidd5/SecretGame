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
        public Vector2 BackGroundPosition{ get; set; }
        public float BackGroundScale { get; set; }

        public List<CompletionCategory> CategoryTabs { get; set; }
        public SanctuaryHolder SanctuaryHolder { get; set; }

        public CompletionGuide(GraphicsDevice graphicsDevice,SanctuaryHolder sanctuaryHolder)
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
            for(int i =0; i < CategoryTabs.Count; i++)
            {
                CategoryTabs[i].Pages.Add(new CompletionPage());
            }

            for(int i =0; i < sanctuaryHolder.AllRequirements.Count; i++)
            {
                for(int j =0; j < CategoryTabs.Count; j++)
                {
                    if(sanctuaryHolder.AllRequirements[i].Tab == j)
                    {

                        CategoryTabs[j].Pages[0].SanctuaryRequirements.Add(new CompletionRequirement(sanctuaryHolder.AllRequirements[i].ItemID,sanctuaryHolder.AllRequirements[i].GIDRequired, sanctuaryHolder.AllRequirements[i].NumberRequired,
                            sanctuaryHolder.AllRequirements[i].Rectangle));
                    }
                }
            }


        }

        public void Update(GameTime gameTime)
        {

            for(int page = 0; page < CategoryTabs.Count; page++)
            {
                CategoryTabs[page].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawPosition = new Vector2(BackGroundPosition.X - (BackGroundSourceRectangle.Width * BackGroundScale) / 2,
                BackGroundPosition.Y - (BackGroundSourceRectangle.Height * BackGroundScale) / 2);
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, drawPosition, this.BackGroundSourceRectangle,
                   Color.White, 0f, Game1.Utility.Origin, BackGroundScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            for (int page = 0; page < CategoryTabs.Count; page++)
            {
                CategoryTabs[page].Draw(spriteBatch, drawPosition, BackGroundScale);
            }
        }
    }
}

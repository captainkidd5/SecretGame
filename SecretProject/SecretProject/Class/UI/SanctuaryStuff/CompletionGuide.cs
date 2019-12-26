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

        public List<CategoryTab> CategoryTabs { get; set; }
        public SanctuaryHolder SanctuaryHolder { get; set; }

        public CompletionGuide(GraphicsDevice graphicsDevice,SanctuaryHolder sanctuaryHolder)
        {
            this.Graphics = graphicsDevice;
            this.SanctuaryHolder = sanctuaryHolder;
            this.BackGroundSourceRectangle = new Rectangle(448, 112, 272, 160);
            this.BackGroundPosition = new Vector2(Game1.Utility.CenterScreenX, Game1.Utility.CenterScreenY);
            CategoryTabs = new List<CategoryTab>()
            {
                new CategoryTab("Flora", Graphics,BackGroundPosition,new Rectangle(464,80, 48, 32), BackGroundScale),
                new CategoryTab("Minerals", Graphics,BackGroundPosition,new Rectangle(528,80, 48, 32), BackGroundScale),
                new CategoryTab("Fauna", Graphics,BackGroundPosition,new Rectangle(592,80, 48, 32), BackGroundScale),
                new CategoryTab("Special", Graphics,BackGroundPosition,new Rectangle(656,80, 48, 32), BackGroundScale),

            };

            for(int i =0; i < sanctuaryHolder.AllRequirements.Count; i++)
            {
                for(int j =0; j < CategoryTabs.Count; j++)
                {
                    if(sanctuaryHolder.AllRequirements[i].Tab == j)
                    {
                        
                        //CategoryTabs[j].Pages.Add(new CompletionPage)
                    }
                }
            }

            //for(int i =0; i < CategoryTabs.Count; i++)
            //{
            //    CompletionPage completionPage = new Comp
            //}


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
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackGroundPosition, this.BackGroundSourceRectangle,
                   Color.White, 0f, Game1.Utility.Origin, BackGroundScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            for (int page = 0; page < CategoryTabs.Count; page++)
            {
                CategoryTabs[page].Draw(spriteBatch);
            }
        }
    }
}

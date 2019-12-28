using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XMLData.SanctuaryStuff;

namespace SecretProject.Class.UI.SanctuaryStuff
{
    public class CompletionPage : IPage
    {
        public List<CompletionRequirement> SanctuaryRequirements { get; set; }



        public CompletionPage()
        {
            this.SanctuaryRequirements = new List<CompletionRequirement>();
        }
        public void Update(GameTime gameTime)
        {
           // throw new NotImplementedException();
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            for(int i =0; i < SanctuaryRequirements.Count; i++)
            {
                spriteBatch.DrawString(Game1.AllTextures.MenuText, SanctuaryRequirements[i].String, new Vector2(position.X, position.Y + (32 * i * scale)), Color.Black, 0f, Game1.Utility.Origin, scale - 1, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
                spriteBatch.Draw(Game1.AllTextures.MasterTileSet, new Rectangle((int)(position.X + SanctuaryRequirements[i].ImageLocation.X * (scale - 1)), (int)(position.Y + (32 * i * scale)), 48, 48),
                SanctuaryRequirements[i].SourceRectangle, Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, SanctuaryRequirements[i].CurrentCount.ToString() + "/" + SanctuaryRequirements[i].CountRequired.ToString(), new Vector2(position.X + SanctuaryRequirements[i].ImageLocation.X * (scale - 1) + 32 * scale, position.Y + (32 * i * scale)),
                    Color.Black, 0f, Game1.Utility.Origin, scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        
    }
}

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
                spriteBatch.DrawString(Game1.AllTextures.MenuText, SanctuaryRequirements[i].String, position, Color.White, 0f, Game1.Utility.Origin, scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
                spriteBatch.Draw(Game1.AllTextures.MasterTileSet, new Vector2(position.X + SanctuaryRequirements[i].ImageLocation.X * scale, position.Y),
                SanctuaryRequirements[i].SourceRectangle, Color.White, 0f, Game1.Utility.Origin, scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        
    }
}

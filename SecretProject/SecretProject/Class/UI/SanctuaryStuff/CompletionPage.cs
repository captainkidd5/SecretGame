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
        public List<SanctuaryRequirement> SanctuaryRequirements { get; set; }

        public CompletionPage(List<SanctuaryRequirement> sanctuaryRequirements)
        {
            this.SanctuaryRequirements = sanctuaryRequirements;
        }
        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        
    }
}

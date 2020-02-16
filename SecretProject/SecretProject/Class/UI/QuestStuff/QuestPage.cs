using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.QuestFolder;
using SecretProject.Class.Universal;

namespace SecretProject.Class.UI.QuestStuff
{
    public class QuestPage : IPage
    {
        public string Title { get; private set; }
        public string Description { get; private set; }

        public Vector2 TextPosition { get; private set; }



        public Button BackButton { get; private set; }

        public QuestPage(QuestHandler questHandler, Vector2 textPosition)
        {
            this.Title = questHandler.ActiveQuest.QuestName;
            this.Description = questHandler.ActiveQuest.UnlockDescription;
            this.TextPosition = textPosition;



        }

        public void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Description, this.TextPosition, Color.Black, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Utility.StandardButtonDepth + .06f);
        }

        
    }
}

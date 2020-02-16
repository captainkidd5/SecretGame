using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.QuestFolder;

namespace SecretProject.Class.UI.QuestStuff
{
    public class QuestPage : IPage
    {
        public string Title { get; private set; }
        public string Description { get; private set; }

        public Vector2 TextPosition { get; private set; }
        public Rectangle BackgroundSourceRectangle { get; private set; }

        public Button Button { get; set; }
        public Button BackButton { get; private set; }

        public QuestPage(QuestHandler questHandler, Vector2 textPosition)
        {
            this.Title = questHandler.ActiveQuest.QuestName;
            this.Description = questHandler.ActiveQuest.UnlockDescription;
            this.TextPosition = textPosition;
            this.BackgroundSourceRectangle = new Rectangle(864, 48, 112, 32);
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

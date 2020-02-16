using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.QuestFolder;
using SecretProject.Class.UI.ButtonStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.QuestStuff
{
    public class QuestLog : IPage
    {
        public List<IPage> Quests { get; set; }
        public RedEsc RedEsc { get; set; }

        public QuestLog(GraphicsDevice graphics)
        {

        }

        public void AddNewQuest(QuestHandler quest)
        {
            Quests.Add(new QuestPage())
        }

        public void Update(GameTime gameTime)
        {
            for(int i = 0; i < Quests.Count; i++)
            {
                Quests[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Quests.Count; i++)
            {
                Quests[i].Draw(spriteBatch);
            }
        }
    }
}

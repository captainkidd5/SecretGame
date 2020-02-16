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

        public Vector2 Position { get; set; }
        public Rectangle BackgroundSourceRectangle { get; set; }

        public QuestPage ActiveQuestPage { get; set; }
        public float Scale { get; set; }

        public QuestLog(GraphicsDevice graphics)
        {
            this.Position = Game1.Utility.centerScreen;
            this.BackgroundSourceRectangle = new Rectangle(624, 320, 160, 224);
            this.Scale = 1f;
            this.RedEsc = new RedEsc(Game1.Utility.CenterOnTopRightCorner(this.BackgroundSourceRectangle, RedEsc.RedEscRectangle, this.Position, this.Scale), graphics);
        }

        public void AddNewQuest(QuestHandler quest)
        {
            Quests.Add(new QuestPage(quest, new Vector2(this.Position.X, this.Position.Y + 64 * Quests.Count)));
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

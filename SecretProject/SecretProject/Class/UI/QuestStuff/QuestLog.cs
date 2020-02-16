﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.QuestFolder;
using SecretProject.Class.UI.ButtonStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.QuestStuff
{
    public class QuestLog : IPage
    {
        GraphicsDevice Graphics;
        public List<QuestPage> Quests { get; set; }
        public RedEsc RedEsc { get; set; }

        public Vector2 Position { get; set; }
        public Rectangle BackgroundSourceRectangle { get; set; }


        public QuestPage ActiveQuestPage { get; set; }
        public float Scale { get; set; }

        public List<Button> QuestButtons { get; set; }

        public Button BackButton { get; private set; }

        public QuestLog(GraphicsDevice graphics)
        {
            this.Graphics = graphics;
         
            this.BackgroundSourceRectangle = new Rectangle(624, 320, 160, 224);
            this.Scale = 3f;
            this.Position = Game1.Utility.CenterRectangleOnScreen(this.BackgroundSourceRectangle, this.Scale);
            this.RedEsc = new RedEsc(Game1.Utility.CenterOnTopRightCorner(this.BackgroundSourceRectangle, RedEsc.RedEscRectangle, this.Position, this.Scale), graphics);
            this.QuestButtons = new List<Button>();

            Quests = new List<QuestPage>();
            this.BackButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(304, 528, 32, 16),
                graphics, new Vector2(this.Position.X, this.Position.Y + this.BackgroundSourceRectangle.Height * this.Scale), CursorType.Normal, this.Scale);
        }

        public void AddNewQuest(QuestHandler quest)
        {
            Quests.Add(new QuestPage(quest, new Vector2(this.Position.X, this.Position.Y + 80 * Quests.Count)));
            QuestButtons.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(864, 48, 112, 32), this.Graphics, new Vector2(this.Position.X, this.Position.Y + 64 * Quests.Count), Controls.CursorType.Normal, this.Scale));
        }

        public void Update(GameTime gameTime)
        {
            RedEsc.Update(Game1.myMouseManager);
            if(RedEsc.isClicked)
            {
                Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.PageRuffleClose, true, .1f);
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                this.ActiveQuestPage = null;
            }
            for(int i = 0; i < QuestButtons.Count; i++)
            {
                QuestButtons[i].Update(Game1.myMouseManager);
                if(QuestButtons[i].isClicked)
                {
                    this.ActiveQuestPage = Quests[i];
                }
            }
            if(this.ActiveQuestPage != null)
            {
                this.ActiveQuestPage.Update(gameTime);
                this.BackButton.Update(Game1.myMouseManager);
                if(this.BackButton.isClicked)
                {
                    this.ActiveQuestPage = null;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackgroundSourceRectangle,
               Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardButtonDepth - .01f);
            RedEsc.Draw(spriteBatch);


            if(this.ActiveQuestPage == null)
            {
                for (int i = 0; i < QuestButtons.Count; i++)
                {
                    QuestButtons[i].Draw(spriteBatch, Game1.AllTextures.MenuText, Quests[i].Title, QuestButtons[i].Position, QuestButtons[i].Color, Utility.StandardButtonDepth + .01f, Game1.Utility.StandardTextDepth + .01f, this.Scale);
                }
            }
            else
            {
                this.ActiveQuestPage.Draw(spriteBatch);
                this.BackButton.Draw(spriteBatch);
            }
           

        }
    }
}

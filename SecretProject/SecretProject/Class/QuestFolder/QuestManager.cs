using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using XMLData.QuestStuff;

namespace SecretProject.Class.QuestFolder
{
    public class QuestManager : Component
    {
        public QuestHandler DobbinQuests { get; private set; }
        public QuestHandler ElixirQuests { get; private set; }
        public QuestHandler KayaQuests { get; private set; }
        public QuestHandler JulianQuests { get; private set; }
        public QuestHandler SarahQuests { get; private set; }
        public QuestHandler MippinQuests { get; private set; }
        public QuestHandler NedQuests { get; private set; }
        public QuestHandler TealQuests { get; private set; }
        public QuestHandler MarcusQuests { get; private set; }
        public QuestHandler SnawQuests { get; private set; }
        public QuestHandler BusinessSnailQuests { get; private set; }
        public QuestHandler CasparQuests { get; private set; }
        public QuestManager(GraphicsDevice graphics, ContentManager content) : base( graphics, content)
        {
        }

        public override void Load()
        {
            DobbinQuests = new QuestHandler(content.Load<QuestHolder>("QuestStuff/DobbinQuests"));
            ElixirQuests = new QuestHandler(content.Load<QuestHolder>("QuestStuff/ElixirQuests"));
            KayaQuests = new QuestHandler(content.Load<QuestHolder>("QuestStuff/KayaQuests"));
            JulianQuests = new QuestHandler(content.Load<QuestHolder>("QuestStuff/JulianQuests"));
            MippinQuests = new QuestHandler(content.Load<QuestHolder>("QuestStuff/MippinQuests"));
            TealQuests = new QuestHandler(content.Load<QuestHolder>("QuestStuff/TealQuests"));
            MarcusQuests = new QuestHandler(content.Load<QuestHolder>("QuestStuff/MarcusQuests"));
            NedQuests = new QuestHandler(content.Load<QuestHolder>("QuestStuff/NedQuests"));
            SnawQuests = new QuestHandler(content.Load<QuestHolder>("QuestStuff/SnawQuests"));
            BusinessSnailQuests = new QuestHandler(content.Load<QuestHolder>("QuestStuff/BusinessSnailQuests"));
            CasparQuests = new QuestHandler(content.Load<QuestHolder>("QuestStuff/CasparQuests"));
        }

        public override void Unload()
        {
            throw new NotImplementedException();
        }
    }
}

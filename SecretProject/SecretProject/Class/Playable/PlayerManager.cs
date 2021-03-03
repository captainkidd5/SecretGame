using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SecretProject.Class.Playable
{
    public class PlayerManager : Component, ISaveable
    {

        public Player LocalPlayer { get; set; }
        private SimpleTimer SimpleTimer { get; set; }

        public List<Player> Players { get; set; }



        public PlayerManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            Players = new List<Player>();
            SimpleTimer = new SimpleTimer(Globals.MFrequency);
        }
        public void Update(GameTime gameTime)
        {
            foreach (Player player in Players)
            {
                player.Update(gameTime);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Player player in Players)
            {
                player.Draw(spriteBatch);
            }
        }
        public void Load(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void Save(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
            throw new NotImplementedException();
        }

        public override void Unload()
        {
            throw new NotImplementedException();
        }
    }
}

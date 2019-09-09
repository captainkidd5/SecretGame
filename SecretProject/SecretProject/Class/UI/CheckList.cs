using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class CheckList
    {
        public Vector2 Position { get; set; }
        public Button RedEsc { get; set; }
        public bool IsActive { get; set; }
        public List<CheckListRequirement> AllRequirements { get; set; }
        public CheckList(GraphicsDevice graphics, Vector2 position, List<CheckListRequirement> allRequirements)
        {
            this.Position = position;
            RedEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), graphics, new Vector2(this.Position.X + 300, this.Position.Y));
            this.IsActive = false;
            this.AllRequirements = allRequirements;

        }

        public void TryFillRequirement(int gid)
        {
            if(AllRequirements.Any(x => x.GID == gid && !x.Completed))
            {
                AllRequirements.Find(x => x.GID == gid && !x.Completed).Completed = true;
            }
        }

        public void Update(GameTime gameTime,MouseManager mouse)
        {
            if(this.IsActive)
            {
                Game1.isMyMouseVisible = true;
                Game1.freeze = true;
                RedEsc.Update(mouse);
                if (RedEsc.isClicked)
                {
                    this.IsActive = false;
                }
            }
            

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(this.IsActive)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, Position, new Rectangle(80, 400, 1024, 672), Color.White);
                RedEsc.Draw(spriteBatch);
                for(int i =0; i < AllRequirements.Count; i++)
                {
                    switch(AllRequirements[i].Type)
                    {
                        case "plant":
                            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(50, 50 * i), new Rectangle(16, 256, 16, 16), Color.White);
                            break;
                    }
                }
            }
            
        }

        public class CheckListRequirement
        {
            public int GID { get; set; }
            public int Tier { get; set; }
            public string Type { get; set; }
            public bool Completed { get; set; }
            public CheckListRequirement(int gid, int tier, string type, bool completed)
            {
                this.GID = gid;
                this.Tier = tier;
                this.Type = type;
                this.Completed = completed;
            }
        }
    }
}

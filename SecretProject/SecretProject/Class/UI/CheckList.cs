using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using System.Collections.Generic;
using System.Linq;

namespace SecretProject.Class.UI
{
    public class CheckList : IExclusiveInterfaceComponent
    {
        public Vector2 Position { get; set; }
        public Button RedEsc { get; set; }
        public List<CheckListRequirement> AllRequirements { get; set; }
        public bool FreezesGame { get; set; }
        public bool IsActive { get; set; }

        public CheckList(GraphicsDevice graphics, Vector2 position, List<CheckListRequirement> allRequirements)
        {
            this.Position = position;
            this.RedEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), graphics, new Vector2(this.Position.X + 900, this.Position.Y), CursorType.Normal);
            this.IsActive = false;
            this.AllRequirements = allRequirements;
            this.FreezesGame = true;

        }

        public bool TryFillRequirement(int gid)
        {
            if (this.AllRequirements.Any(x => x.GID == gid && !x.Completed))
            {
                this.AllRequirements.Find(x => x.GID == gid && !x.Completed).Completed = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {

            Game1.isMyMouseVisible = true;
            Game1.freeze = true;
            this.RedEsc.Update(mouse);
            if (this.RedEsc.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            }



        }
        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, new Rectangle(80, 400, 1024, 672), Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            this.RedEsc.Draw(spriteBatch);
            for (int i = 0; i < this.AllRequirements.Count; i++)
            {
                switch (this.AllRequirements[i].Type)
                {
                    case "plant":
                        spriteBatch.DrawString(Game1.AllTextures.MenuText, this.AllRequirements[i].Name, new Vector2(this.Position.X + 50, this.Position.Y + 100 + 100 * i), Color.Black, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .0001f);
                        if (this.AllRequirements[i].Completed)
                        {
                            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(this.Position.X + 600, this.Position.Y + 100 + 100 * i), new Rectangle(208, 256, 32, 32), Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .0001f);
                        }
                        else
                        {
                            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(this.Position.X + 600, this.Position.Y + 100 + 100 * i), new Rectangle(16, 256, 32, 32), Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .0001f);
                        }

                        break;
                }
            }
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Reward: ", new Vector2(this.Position.X + 50, this.Position.Y + 100 + 100 * this.AllRequirements.Count), Color.Black, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .0001f);
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(this.Position.X + 200, this.Position.Y + 100 + 100 * this.AllRequirements.Count),
            new Rectangle(1328, 1472, 16, 32), Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .0001f);


        }

        public class CheckListRequirement
        {
            public string Name { get; set; }
            public int GID { get; set; }
            public int Tier { get; set; }
            public string Type { get; set; }
            public bool Completed { get; set; }
            public CheckListRequirement(string name, int gid, int tier, string type, bool completed)
            {
                this.Name = name;
                this.GID = gid;
                this.Tier = tier;
                this.Type = type;
                this.Completed = completed;
            }
        }
    }
}

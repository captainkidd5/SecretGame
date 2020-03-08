using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.QuestFolder;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI.ButtonStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.QuestStuff
{
    public class WorldQuestMenu
    {
        public GraphicsDevice Graphics { get; set; }
        public float Scale { get; private set; }
        public WorldQuest WorldQuest { get; private set; }
        public Rectangle BackSourceRectangle { get; private set; }
        public Vector2 Position { get; private set; }

        public RedEsc RedEsc { get; set; }

        public Tile Tile { get; set; }


        public WorldQuestMenu(GraphicsDevice graphics)
        {
            this.Graphics = graphics;
            this.Scale = 2f;
            this.BackSourceRectangle = new Rectangle(832, 496, 192, 192);
            this.Position = Game1.Utility.CenterRectangleOnScreen(this.BackSourceRectangle, this.Scale);
            this.RedEsc = new RedEsc(Game1.Utility.CenterOnTopRightCorner(this.BackSourceRectangle, RedEsc.RedEscRectangle, this.Position, this.Scale), graphics);
        }

        public void LoadQuest(WorldQuest worldQuest, Tile tile)
        {
            this.WorldQuest = worldQuest;
            this.Tile = tile;
        }

        public void AddSpriteToDictionary(Dictionary<string, Sprite> dictionary, IInformationContainer container, Tile tile)
        {
            dictionary.Add(tile.TileKey, new Sprite(this.Graphics, Game1.AllTextures.UserInterfaceTileSet,
                new Rectangle(16,48, 16, 32),new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y), 16,32));
        }

        public void Update(GameTime gameTime)
        {
            RedEsc.Update(Game1.MouseManager);
            if(RedEsc.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            }

            if(this.Tile == null)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.WorldQuest.Description, this.Position, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            this.RedEsc.Draw(spriteBatch);
        }
    }
}

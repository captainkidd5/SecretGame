
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System.Collections.Generic;


namespace SecretProject.Class.UI.MainMenuStuff
{
    public class ChooseGameMenu
    {
        public Vector2 Position { get; set; }
        public float  Scale { get; set; }
        public Rectangle BackGroundSourceRectangle { get; private set; }
        public Rectangle ButtonSourceRectangle { get; set; }
        public SaveSlot SaveSlot1 { get; private set; }
        public SaveSlot SaveSlot2 { get; private set; }
        public SaveSlot SaveSlot3 { get; private set; }

        public List<SaveSlot> AllSaveSlots { get; set; }

        public ChooseGameMenu(GraphicsDevice graphics, Vector2 position, float scale)
        {
            this.Position = position;
            this.Scale = scale;
            this.BackGroundSourceRectangle = new Rectangle(304, 365, 112, 163);
            this.ButtonSourceRectangle = new Rectangle(1024, 64, 112, 48);

            SaveSlot1 = new SaveSlot(new Button(Game1.AllTextures.UserInterfaceTileSet, this.ButtonSourceRectangle,
                graphics, this.Position, CursorType.Normal, this.Scale, null));
            SaveSlot2 = new SaveSlot(new Button(Game1.AllTextures.UserInterfaceTileSet, this.ButtonSourceRectangle,
                graphics, this.Position, CursorType.Normal, this.Scale, null));
            SaveSlot3 = new SaveSlot(new Button(Game1.AllTextures.UserInterfaceTileSet, this.ButtonSourceRectangle,
                graphics, this.Position, CursorType.Normal, this.Scale, null));
            this.AllSaveSlots = new List<SaveSlot>()
            {
                SaveSlot1,
                SaveSlot2,
                SaveSlot3
            };
        }

        public void Update(GameTime gameTime)
        {
            for(int i =0; i < this.AllSaveSlots.Count; i++)
            {
                AllSaveSlots[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.AllSaveSlots.Count; i++)
            {
                AllSaveSlots[i].Draw(spriteBatch);
            }
        }
    }
}

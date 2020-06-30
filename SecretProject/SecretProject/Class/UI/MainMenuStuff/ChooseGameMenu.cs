
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.UI;
using SecretProject.Class.UI.ButtonStuff;
using SecretProject.Class.Universal;
using System.Collections.Generic;
using System.IO;

namespace SecretProject.Class.UI.MainMenuStuff
{
    public enum ChooseGameState
    {
        SaveSlotSelection = 1,
        CreateNewCharacter = 2
    }
    public class ChooseGameMenu
    {
        public ChooseGameState MenuChoice { get; set; }
        public Vector2 Position { get; set; }
        public float  Scale { get; set; }
        public Rectangle BackGroundSourceRectangle { get; private set; }
        public Rectangle ButtonSourceRectangle { get; set; }


        public List<SaveSlot> AllSaveSlots { get; set; }

        public SaveSlot CurrentSaveSlot;

        public CharacterCreationMenu CharacterCreationMenu { get; set; }

        public ChooseGameMenu(GraphicsDevice graphics, float scale)
        {
            
            this.Scale = scale;
            this.BackGroundSourceRectangle = new Rectangle(304, 365, 112, 163);
            this.ButtonSourceRectangle = new Rectangle(1024, 64, 112, 48);
            this.Position = Game1.Utility.CenterRectangleInRectangle( Game1.ScreenRectangle, this.BackGroundSourceRectangle, Game1.Utility.Origin, 1f, this.Scale);


            string[] directories = System.IO.Directory.GetDirectories(@"Content/SaveFiles/GameSaves");
            int directoryCount = directories.Length;
            this.AllSaveSlots = new List<SaveSlot>() ;
            for (int i = 0; i < directoryCount; i++)
            {
                string directoryString = Directory.GetFiles(directories[i])[0];
                FileStream fileStream = File.OpenRead(directoryString); //first file in each save should always be the primary save data, no rearranging. 
                BinaryReader binaryReader = new BinaryReader(fileStream);
                string saveName = binaryReader.ReadString();
                this.AllSaveSlots.Add(new SaveSlot(graphics, i, new Button(Game1.AllTextures.UserInterfaceTileSet, this.ButtonSourceRectangle,
                graphics, new Vector2(this.Position.X, this.Position.Y + 100 * i), CursorType.Normal, this.Scale - 1, null), true, saveName));
            }
            SaveSlot EmptySaveSlot = new SaveSlot(graphics, directoryCount, new Button(Game1.AllTextures.UserInterfaceTileSet, this.ButtonSourceRectangle,
                graphics, new Vector2(this.Position.X, this.Position.Y + 100 * directoryCount), CursorType.Normal, this.Scale - 1, null),false);
            this.AllSaveSlots.Add(EmptySaveSlot);
            this.MenuChoice = ChooseGameState.SaveSlotSelection;
            this.CharacterCreationMenu = new CharacterCreationMenu(graphics, this.AllSaveSlots[this.AllSaveSlots.Count - 1]);
        }

        public void Update(GameTime gameTime)
        {
            switch(this.MenuChoice)
            {
                case ChooseGameState.SaveSlotSelection:
                    for (int i = 0; i < this.AllSaveSlots.Count; i++)
                    {
                        AllSaveSlots[i].Update(gameTime);
                    }
                    break;
                case ChooseGameState.CreateNewCharacter:
                    this.CharacterCreationMenu.Update(gameTime);
                    break;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (this.MenuChoice)
            {
                case ChooseGameState.SaveSlotSelection:
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, Utility.StandardTextDepth - .04f);
                    for (int i = 0; i < this.AllSaveSlots.Count; i++)
                    {
                        AllSaveSlots[i].Draw(spriteBatch);
                    }
                    break;
                case ChooseGameState.CreateNewCharacter:
                    this.CharacterCreationMenu.Draw(spriteBatch);
                    break;
            }
            
        }
    }
}

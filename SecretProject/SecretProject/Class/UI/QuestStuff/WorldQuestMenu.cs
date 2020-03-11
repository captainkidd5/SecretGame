using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
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


        public int TileLayer { get; set; }
        public int TileI { get; set; }
        public int TileJ { get; set; }
        public IInformationContainer Container { get; set; }

        public string Description { get; set; }

        public Button RepairButton { get; set; }

        public List<ItemButton> ItemRequirementButtons { get; set; }

        public Rectangle TileSourceRectangle { get; set; }
        public Vector2 TileDrawPosition { get; set; }


        public WorldQuestMenu(GraphicsDevice graphics)
        {
            this.Graphics = graphics;
            this.Scale = 3f;
            this.BackSourceRectangle = new Rectangle(832, 496, 192, 192);
            this.Position = Game1.Utility.CenterRectangleOnScreen(this.BackSourceRectangle, this.Scale);
            this.RedEsc = new RedEsc(Game1.Utility.CenterOnTopRightCorner(this.BackSourceRectangle, RedEsc.RedEscRectangle, this.Position, this.Scale), graphics);

            this.RepairButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(441, 496, 62, 16),
                graphics, Game1.Utility.CenterRectangleInRectangleLowerThird(this.BackSourceRectangle, new Rectangle(441, 496, 62, 16),
                this.Position, this.Scale), Controls.CursorType.Normal, this.Scale + 1);

            this.ItemRequirementButtons = new List<ItemButton>();
        }

        public void LoadQuest(WorldQuest worldQuest, int tileLayer, int tileI, int tileJ, IInformationContainer container)
        {
            this.WorldQuest = worldQuest;
            this.TileLayer = tileLayer;
            this.TileI = tileI;
            this.TileJ = tileJ;
     
            this.Description = worldQuest.Description;
            this.Container = container;
            this.ItemRequirementButtons = new List<ItemButton>();

            this.TileSourceRectangle = container.AllTiles[tileLayer][tileI, tileJ].SourceRectangle;
            this.TileDrawPosition = Game1.Utility.CenterRectangleInRectangle(this.BackSourceRectangle, this.TileSourceRectangle, this.Position, this.Scale);

            for(int i =0; i < worldQuest.ItemsRequired.Count; i++)
            {
                Item item = Game1.ItemVault.GenerateNewItem(worldQuest.ItemsRequired[i].ItemID, null);

                this.ItemRequirementButtons.Add(new ItemButton(Graphics, new Vector2(this.Position.X + 64 + i * 64, this.Position.Y + 112), worldQuest.ItemsRequired[i].Count, this.Scale, item));

            }
        }

        public void AddSpriteToDictionary(Dictionary<string, Sprite> dictionary, IInformationContainer container, Tile tile)
        {
            dictionary.Add(tile.TileKey, new Sprite(this.Graphics, Game1.AllTextures.UserInterfaceTileSet,
                new Rectangle(16,48, 16, 32),new Vector2(tile.DestinationRectangle.X + tile.SourceRectangle.Width / 4, tile.DestinationRectangle.Y), 16,32));
        }

        public void Update(GameTime gameTime)
        {
            RedEsc.Update(Game1.MouseManager);
            if(RedEsc.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            }
            this.RepairButton.Update(Game1.MouseManager);
            if(this.RepairButton.isClicked)
            {
                if(Game1.WorldQuestHolder.CheckIfRequirementsMet(this.WorldQuest))
                {
                    Game1.WorldQuestHolder.RemoveItemsFromPlayerInventory(this.WorldQuest);

                    TileUtility.ReplaceTile(this.TileLayer,this.TileI,this.TileJ, WorldQuest.ReplacementGID, Container);
                    this.WorldQuest.Completed = true;
                    Container.QuestIcons.Remove(Container.AllTiles[TileLayer][TileI, TileJ].GetTileKeyStringNew(TileLayer, Container));
                    Game1.Player.UserInterface.AddAlert(AlertType.Normal, AlertSize.Medium, Game1.Utility.centerScreen, "Repaired!");
                    Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                }
                else
                {
                    Game1.Player.UserInterface.AddAlert(AlertType.Normal, AlertSize.Medium, Game1.Utility.centerScreen, "Not enough resources");
                }
            }

            for(int i =0; i < this.ItemRequirementButtons.Count; i++)
            {
                ItemRequirementButtons[i].Update(Game1.MouseManager);
            }

            //if(this.Tile == null)
            //{
            //    Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            //}

            if(Game1.KeyboardManager.WasKeyPressed(Keys.Escape))
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Description, new Vector2(this.Position.X + 32, this.Position.Y + 32), Color.Black, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            spriteBatch.Draw(Game1.AllTextures.MasterTileSet, this.TileDrawPosition, this.TileSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
            this.RedEsc.Draw(spriteBatch);

            this.RepairButton.Draw(spriteBatch, Game1.AllTextures.MenuText, "Repair!", this.RepairButton.Position, Color.White, Game1.Utility.StandardButtonDepth + .02f, Game1.Utility.StandardTextDepth + .03f, 2f);

            for (int i = 0; i < this.ItemRequirementButtons.Count; i++)
            {
                ItemRequirementButtons[i].DrawItemButton(spriteBatch);
            }
        }
    }
}

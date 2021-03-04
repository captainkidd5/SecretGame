using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.QuestFolder;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI.ButtonStuff;
using SecretProject.Class.Universal;
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
        public TileManager TileManager { get; set; }

        public string Description { get; set; }
        public Vector2 DescriptionPosition { get; set; }

        public string RewardDescription { get; set; }
        public Vector2 RewardDescriptionPosition { get; set; }

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

        public void LoadQuest(WorldQuest worldQuest, int tileLayer, int tileI, int tileJ, TileManager TileManager)
        {
            this.WorldQuest = worldQuest;
            this.TileLayer = tileLayer;
            this.TileI = tileI;
            this.TileJ = tileJ;
     
            this.Description = worldQuest.Description;
            this.DescriptionPosition = new Vector2(Game1.Utility.CenterTextOnRectangle(Game1.AllTextures.MenuText, Game1.Utility.GetCenterOfRectangle(this.BackSourceRectangle, this.Position, this.Scale), this.Description, this.Scale).X, this.Position.Y + 32);

            this.RewardDescription = worldQuest.RewardDescription;
            this.RewardDescriptionPosition = new Vector2(Game1.Utility.CenterTextOnRectangle(Game1.AllTextures.MenuText, Game1.Utility.GetCenterOfRectangle(this.BackSourceRectangle, this.Position, this.Scale), this.Description, this.Scale).X, this.Position.Y + this.BackSourceRectangle.Height * Scale * .75f);
            this.TileManager = TileManager;
            this.ItemRequirementButtons = new List<ItemButton>();

            this.TileSourceRectangle = TileManager.AllTiles[tileLayer][tileI, tileJ].SourceRectangle;
            this.TileDrawPosition = Game1.Utility.CenterRectangleInRectangle(this.BackSourceRectangle, this.TileSourceRectangle, new Vector2(this.Position.X, this.Position.Y - 32), this.Scale, this.Scale);

            for(int i =0; i < worldQuest.ItemsRequired.Count; i++)
            {
                Item item = Game1.ItemVault.GenerateNewItem(worldQuest.ItemsRequired[i].ItemID, null);

                this.ItemRequirementButtons.Add(new ItemButton(Graphics, new Vector2(this.Position.X + 64 + i * 64, this.Position.Y + this.BackSourceRectangle.Height * Scale * .75f), worldQuest.ItemsRequired[i].Count, this.Scale, item));

            }
        }

        public void AddSpriteToDictionary(Dictionary<string, Sprite> dictionary, TileManager TileManager, Tile tile)
        {
            if(!dictionary.ContainsKey(tile.TileKey))
            {
                dictionary.Add(tile.TileKey, new Sprite(this.Graphics, Game1.AllTextures.UserInterfaceTileSet,
                new Rectangle(16, 48, 16, 32), new Vector2(tile.DestinationRectangle.X + tile.SourceRectangle.Width / 4, tile.DestinationRectangle.Y), 16, 32));
            }
            else
            {
                Console.WriteLine("dictionary already has key " + tile.TileKey);
            }
            
        }

        public void Update(GameTime gameTime)
        {
            RedEsc.Update();
            if(RedEsc.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            }
            this.RepairButton.Update();
            if(this.RepairButton.isClicked)
            {
                if(Game1.WorldQuestHolder.CheckIfRequirementsMet(this.WorldQuest))
                {
                    Game1.WorldQuestHolder.RemoveItemsFromPlayerInventory(this.WorldQuest);

                    TileUtility.ReplaceTile(this.TileLayer,this.TileI,this.TileJ, WorldQuest.ReplacementGID, TileManager);
                    this.WorldQuest.Completed = true;
                    TileManager.QuestIcons.Remove(TileManager.AllTiles[TileLayer][TileI, TileJ].GetTileKeyString(TileLayer, TileManager));
                    Game1.Player.UserInterface.AddAlert(AlertType.Normal, Game1.Utility.centerScreen, "Repaired!");
                    Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                }
                else
                {
                    Game1.Player.UserInterface.AddAlert(AlertType.Normal, Game1.Utility.centerScreen, "Not enough resources");
                }
            }

            for(int i =0; i < this.ItemRequirementButtons.Count; i++)
            {
                ItemRequirementButtons[i].Update();
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
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None,Utility.StandardButtonDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Description,this.DescriptionPosition, Color.Black, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardTextDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.RewardDescription, this.RewardDescriptionPosition, Color.Black, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Utility.StandardTextDepth);
            spriteBatch.Draw(Game1.AllTextures.MasterTileSet, this.TileDrawPosition, this.TileSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None,Utility.StandardButtonDepth + .01f);
            this.RedEsc.Draw(spriteBatch);

            this.RepairButton.Draw(spriteBatch, Game1.AllTextures.MenuText, "Repair!", this.RepairButton.Position, Color.White,Utility.StandardButtonDepth + .02f, Utility.StandardTextDepth + .03f, 2f);

            for (int i = 0; i < this.ItemRequirementButtons.Count; i++)
            {
                ItemRequirementButtons[i].DrawItemButton(spriteBatch);
            }
        }
    }
}

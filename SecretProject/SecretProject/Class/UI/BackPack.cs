using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class BackPack : IExclusiveInterfaceComponent
    {
        public bool IsActive { get; set; }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public GraphicsDevice Graphics { get; set; }
        public Inventory Inventory { get; set; }
        public int Capacity { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle BackGroundSourceRectangle { get; set; }
        public float Scale { get; set; }
        public bool IsAnySlotHovered { get; set; }
        public bool WasSlotJustReleased { get; set; }
        public Item ItemJustReleased { get; set; }
        public Sprite DragSprite { get; set; }
        public List<Button> AllSlots { get; set; }
        Button RedEsc;

        TextBuilder TextBuilder;

        public bool MouseIntersectsBackDrop { get; set; }

        public BackPack(GraphicsDevice graphics, Inventory Inventory)
        {
            this.Graphics = graphics;
            this.Inventory = Inventory;
            this.IsActive = false;
            this.Position = new Vector2(Game1.PresentationParameters.BackBufferWidth / 3, Game1.PresentationParameters.BackBufferHeight / 4);
            this.BackGroundSourceRectangle = new Rectangle(208, 560, 336, 128);
            this.Scale = 2f;

            AllSlots = new List<Button>();

            for (int i = 0; i < 10; i++)
            {
                AllSlots.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 64, 64), Graphics, new Vector2(Game1.PresentationParameters.BackBufferWidth * .35f + i * 65, Game1.PresentationParameters.BackBufferHeight * .9f), CursorType.Normal) { ItemCounter = 0, Index = i + 1 });
            }
            for (int i = 0; i < 10; i++)
            {
                AllSlots.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 64, 64), Graphics, new Vector2(Game1.PresentationParameters.BackBufferWidth * .35f + i * 65, Game1.PresentationParameters.BackBufferHeight * .9f + 32 * Scale), CursorType.Normal) { ItemCounter = 0, Index = i + 1 });
            }
            for (int i = 0; i < 10; i++)
            {
                AllSlots.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 64, 64), Graphics, new Vector2(Game1.PresentationParameters.BackBufferWidth * .35f + i * 65, Game1.PresentationParameters.BackBufferHeight * .9f + 64 * Scale), CursorType.Normal) { ItemCounter = 0, Index = i + 1 });
            }
            TextBuilder = new TextBuilder("", .01f, 5);

            RedEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), graphics, new Vector2(Position.X + BackGroundSourceRectangle.Width * Scale, Position.Y), CursorType.Normal);

        }

        public void Activate()
        {
            this.IsActive = true;

            int index = 0;
            for (int i = 0; i < 10; i++)
            {
                AllSlots[i].Position = new Vector2(BackGroundSourceRectangle.X * Scale + 32 * index * Scale + 32, BackGroundSourceRectangle.Y / Scale - 64);
                index++;
            }
            index = 0;
            for (int i = 10; i < 20; i++)
            {
                AllSlots[i].Position = new Vector2(BackGroundSourceRectangle.X * Scale + 32 * index * Scale + 32, BackGroundSourceRectangle.Y / Scale);
                index++;
            }
            index = 0;
            for (int i = 20; i < 30; i++)
            {
                AllSlots[i].Position = new Vector2(BackGroundSourceRectangle.X * Scale + 32 * index * Scale + 32, BackGroundSourceRectangle.Y / Scale + 64);
                index++;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {
                MouseIntersectsBackDrop = false;
                if (Game1.myMouseManager.MouseRectangle.Intersects(new Rectangle((int)Position.X, (int)Position.Y, (int)(BackGroundSourceRectangle.Width * Scale), (int)(BackGroundSourceRectangle.Height * Scale))))
                {
                    MouseIntersectsBackDrop = true;
                }
                RedEsc.Update(Game1.myMouseManager);
                if (RedEsc.isClicked)
                {
                    this.IsActive = false;
                    Game1.Player.UserInterface.BottomBar.ReturnToolBarButtonsToStandardPosition();
                    Game1.Player.UserInterface.BottomBar.IsActive = true;
                }
                TextBuilder.Update(gameTime);
                IsAnySlotHovered = false;
                for (int i = 0; i < AllSlots.Count; i++)
                {
                    UpdateInventorySlotTexture(Inventory, i);
                    AllSlots[i].Update(Game1.myMouseManager);
                    if (AllSlots[i].IsHovered)
                    {
                        IsAnySlotHovered = true;
                        if (AllSlots[i].ItemCounter > 0)
                        {
                            TextBuilder.Activate(false, TextBoxType.normal, false, Inventory.currentInventory[i].GetItem().Name, 1f,
                      new Vector2(AllSlots[i].Position.X, AllSlots[i].Position.Y - 32), 200f);
                            Game1.Player.UserInterface.InfoBox.IsActive = true;
                            switch (Game1.Player.UserInterface.CurrentOpenInterfaceItem)
                            {
                                case ExclusiveInterfaceItem.ShopMenu:
                                    Game1.Player.UserInterface.InfoBox.FitText(Inventory.currentInventory[i].GetItem().Name + ":  " + "Shop will buy for " + Inventory.currentInventory[i].GetItem().Price + ".", 1f);
                                    Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(AllSlots[i].Position.X - Game1.Player.UserInterface.InfoBox.SourceRectangle.Width + 50, AllSlots[i].Position.Y - 150);
                                    break;

                                default:
                                    Game1.Player.UserInterface.InfoBox.FitText(Inventory.currentInventory[i].GetItem().Name + ":  " + Inventory.currentInventory[i].GetItem().Description, 1f);
                                    Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(AllSlots[i].Position.X - Game1.Player.UserInterface.InfoBox.SourceRectangle.Width + 50, AllSlots[i].Position.Y - 150);
                                    break;
                            }
                        }

                    }
                    if (AllSlots[i].isClickedAndHeld && AllSlots[i].ItemCounter != 0)
                    {
                        Item tempItem = Inventory.currentInventory[i].GetItem();

                        Sprite tempSprite = new Sprite(Graphics, Game1.AllTextures.ItemSpriteSheet, Game1.Player.Inventory.currentInventory.ElementAt(i).SlotItems[0].SourceTextureRectangle,
                            Game1.myMouseManager.WorldMousePosition, 16, 16)
                        { IsBeingDragged = true, TextureScaleX = 5f, TextureScaleY = 5f };
                        DragSprite = tempSprite;
                        DragSprite.Update(gameTime, new Vector2(Game1.myMouseManager.Position.X - 16, Game1.myMouseManager.Position.Y - 16));
                        Game1.myMouseManager.ChangeMouseTexture(CursorType.Normal);


                    }
                }

                
                for (int i = 0; i < AllSlots.Count; i++)
                {
                    //INTERACTIONS WITH RELEASE ITEM 
                    if (AllSlots[i].wasJustReleased && AllSlots[i].ItemCounter > 0)
                    {
                        if (MouseIntersectsBackDrop)
                        {


                            this.WasSlotJustReleased = true;

                            Item tempItem = Inventory.currentInventory[i].GetItem();
                            this.ItemJustReleased = tempItem;

                            if (IsAnySlotHovered)
                            {
                                int index = 0;
                                for (int m = 0; m < AllSlots.Count; m++)
                                {
                                    index = m;
                                    if (AllSlots[m].IsHovered)
                                    {
                                        InventorySlot currentItems = Inventory.currentInventory[i];
                                        Inventory.currentInventory.RemoveAt(i);
                                        if (index > m)
                                        {
                                            index--;
                                        }
                                        Inventory.currentInventory.Insert(index, currentItems);
                                        //Inventory.currentInventory[i] = Inventory.currentInventory[m];
                                        //Inventory.currentInventory[m] = currentItems;
                                        return;
                                    }

                                }
                            }
                        }
                        else if (InteractWithChest(i))
                        {

                        }
                        else if (Game1.Player.UserInterface.CurrentOpenInterfaceItem == ExclusiveInterfaceItem.None)
                        {
                            if (Game1.Player.controls.pressedKeys.Contains(Keys.LeftShift))
                            {
                                Item tempItem = Inventory.currentInventory[i].GetItem();
                                int currentItemCount = AllSlots[i].ItemCounter;
                                for (int j = 0; j < currentItemCount; j++)
                                {
                                    Inventory.currentInventory[i].RemoveItemFromSlot();
                                    AllSlots[i].ItemCounter--;

                                    Item newWorldItem = Game1.ItemVault.GenerateNewItem(tempItem.ID, new Vector2(Game1.Player.Rectangle.X, Game1.Player.Rectangle.Y), true);
                                    newWorldItem.IsTossable = true;
                                    Game1.GetCurrentStage().AllItems.Add(newWorldItem);

                                }
                            }
                            else
                            {
                                Item tempItem = Inventory.currentInventory[i].GetItem();
                                Inventory.currentInventory[i].RemoveItemFromSlot();
                                AllSlots[i].ItemCounter--;

                                Item newWorldItem = Game1.ItemVault.GenerateNewItem(tempItem.ID, new Vector2(Game1.Player.Rectangle.X, Game1.Player.Rectangle.Y), true);
                                newWorldItem.IsTossable = true;
                                Game1.GetCurrentStage().AllItems.Add(newWorldItem);


                            }

                        }
                    }
                   






                    // DragSprite = null;



                }




            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsActive)
            {
                RedEsc.Draw(spriteBatch);
                TextBuilder.Draw(spriteBatch, .75f);
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth - .1f);
                for (int i = 0; i < AllSlots.Count; i++)
                {
                    if (AllSlots[i].IsHovered)
                    {
                        AllSlots[i].Draw(spriteBatch, AllSlots[i].ItemSourceRectangleToDraw, AllSlots[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, AllSlots[i].ItemCounter.ToString(), new Vector2(AllSlots[i].Position.X + 5, AllSlots[i].Position.Y + 5), Color.White * .5f, 2f, 2f, Game1.Utility.StandardButtonDepth);
                    }
                    else
                    {
                        AllSlots[i].Draw(spriteBatch, AllSlots[i].ItemSourceRectangleToDraw, AllSlots[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, AllSlots[i].ItemCounter.ToString(), new Vector2(AllSlots[i].Position.X + 5, AllSlots[i].Position.Y + 5), Color.White, 2f, 2f, Game1.Utility.StandardButtonDepth);
                    }

                }
            }

        }

        public void UpdateInventorySlotTexture(Inventory Inventory, int index)
        {
            if (Inventory.currentInventory.ElementAt(index) == null)
            {
                AllSlots[index].ItemCounter = 0;

            }
            else
            {
                AllSlots[index].ItemCounter = Inventory.currentInventory.ElementAt(index).SlotItems.Count;
            }

            if (AllSlots[index].ItemCounter > 0)
            {
                AllSlots[index].Texture = Inventory.currentInventory.ElementAt(index).SlotItems[0].ItemSprite.AtlasTexture;
                AllSlots[index].ItemSourceRectangleToDraw = Inventory.currentInventory.ElementAt(index).SlotItems[0].SourceTextureRectangle;
            }
            else
            {
                AllSlots[index].Texture = Game1.AllTextures.UserInterfaceTileSet;
                AllSlots[index].ItemSourceRectangleToDraw = new Rectangle(0, 80, 32, 32);
            }
        }

        public bool InteractWithChest(int index)
        {
            if (Game1.Player.UserInterface.IsAnyChestOpen)
            {
                if (Game1.GetCurrentStage().AllTiles.StoreableItems[Game1.Player.UserInterface.OpenChestKey].IsInventoryHovered)
                {
                    if (Game1.GetCurrentStage().AllTiles.StoreableItems[Game1.Player.UserInterface.OpenChestKey].Inventory.TryAddItem(Inventory.currentInventory[index].GetItem()))
                    {
                        Inventory.currentInventory[index].RemoveItemFromSlot();
                        AllSlots[index].ItemCounter--;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}

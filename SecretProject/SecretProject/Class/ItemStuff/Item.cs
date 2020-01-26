
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using XMLData.ItemStuff;

namespace SecretProject.Class.ItemStuff
{
    public class Item : IEntity
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public string Description { get; set; }

        public int Count { get; set; } = 0;
        public int InvMaximum { get; set; }
        public int WorldMaximum { get; set; }


        public bool IsFull { get; set; }
        public bool Ignored { get; set; }



        public Vector2 WorldPosition;


        public bool IsTossable { get; set; } = false;

        public bool IsDropped { get; set; }

        public Sprite ItemSprite { get; set; }

        public GraphicsDevice Graphics { get; set; }

        public ContentManager Content { get; set; }
        public bool IsPlantable { get; set; }


        public string id { get; set; }

        public Rectangle SourceTextureRectangle { get; set; }
        public Rectangle DestinationTextureRectangle { get; set; }

        public int Price { get; set; }


        public bool IsWorldItem { get; set; }


        int directionX = Game1.Utility.RGenerator.Next(-2, 2);

        int directionY = Game1.Utility.RGenerator.Next(-2, 2);

        public int SmeltedItem { get; set; }
        public int FuelValue { get; set; }
        public int Durability { get; set; }
        public int PlaceID { get; set; }
        public string TilingSet { get; set; }
        public int TilingLayer { get; set; }
        public int StaminaRestoredAmount { get; set; }
        public ItemType ItemType { get; set; }
        public int AnimationColumn { get; set; }
        public int CrateType { get; set; }

        public GenerationType GenerationType { get; set; }
        public bool Food { get; set; }
        public byte MeatValue { get; set; }
        public byte VegetableValue { get; set; }
        public byte FruitValue { get; set; }

        public List<Item> AllItems { get; set; }

        public Item(ItemData itemData, List<Item> allItems)
        {
            this.AllItems = allItems;
            this.Name = itemData.Name;
            this.Description = itemData.Description;
            this.InvMaximum = itemData.InvMaximum;
            this.ID = itemData.ID;
            this.Price = itemData.Price;

            this.SourceTextureRectangle = Game1.AllTextures.GetItemTexture(this.ID, 40);
            this.SmeltedItem = itemData.SmeltedItem;
            this.FuelValue = itemData.FuelValue;
            this.Durability = itemData.Durability;

            if (itemData.Plantable)
            {
                this.IsPlantable = itemData.Plantable;
            }

            this.PlaceID = itemData.PlaceID;

            this.StaminaRestoredAmount = itemData.StaminaRestoreAmount;
            this.ItemType = itemData.Type;
            this.AnimationColumn = itemData.AnimationColumn;

            this.TilingSet = itemData.TilingSet;
            if (this.TilingSet != null)
            {
                this.GenerationType = (GenerationType)Enum.Parse(typeof(GenerationType), this.TilingSet);


            }


            this.TilingLayer = itemData.TilingLayer;
            this.CrateType = itemData.CrateType;

            this.Food = itemData.Food;
            this.MeatValue = itemData.MeatValue;
            this.VegetableValue = itemData.VegetableValue;
            this.FruitValue = itemData.FruitValue;

        }
        public void Load()
        {
            if (this.IsWorldItem)
            {
                this.ItemSprite = new Sprite(this.Graphics, Game1.AllTextures.ItemSpriteSheet, this.SourceTextureRectangle,
                    WorldPosition, 16, 16)
                { IsBobbing = true, TextureScaleX = .75f, TextureScaleY = .75f, IsWorldItem = true, LayerDepth = .7f, ColliderType = ColliderType.Item, Entity = this };
                this.Ignored = true;
                AllItems.Add(this);
            }
            else
            {
                this.ItemSprite = new Sprite(this.Graphics, Game1.AllTextures.ItemSpriteSheet, this.SourceTextureRectangle, new Vector2(500, 635), 16, 16) { LayerDepth = .4f };
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsWorldItem)
            {
                this.ItemSprite.Update(gameTime);


                if (this.IsTossable == true)
                {
                    this.ItemSprite.Toss(gameTime, 1f, 1f);
                    if (this.ItemSprite.IsTossed)
                    {
                        this.Ignored = false;
                        this.IsTossable = false;
                        this.ItemSprite.IsTossed = false;
                    }
                }
                else
                {
                    this.Ignored = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsWorldItem)
            {
                this.ItemSprite.Draw(spriteBatch, .4f);
            }

        }

        public void PlayerCollisionInteraction()
        {
            if (!this.Ignored && Game1.Player.Inventory.IsPossibleToAddItem(this))
            {
                Magnetize(Game1.Player.position);
            }

        }

        public void Magnetize(Vector2 playerpos)
        {
            if (this.IsWorldItem)
            {

                if (Game1.Player.MainCollider.IsIntersecting(this.ItemSprite))
                {

                    Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.PickUpItem);
                    this.AllItems.Remove(this);
                   // Game1.GetCurrentStage().AllTiles.GetItems(this.WorldPosition).Remove(this);


                    Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(this.ID, null));
                    Game1.Player.UserInterface.BackPack.CheckGridItem();



                }
                Vector2 dir = new Vector2(Game1.Player.MainCollider.Rectangle.X, Game1.Player.MainCollider.Rectangle.Y) - this.ItemSprite.Position;
                dir.Normalize();
                this.ItemSprite.Position += dir;
                //ItemSprite.Position.X -= playerpos.X ;
                //ItemSprite.Position.Y -= playerpos.Y;

            }
        }


        public void AlterDurability(int amountToSubtract)
        {
            this.Durability -= amountToSubtract;
            if (this.Durability <= 0)
            {
                Game1.Player.Inventory.RemoveItem(this);
                Game1.SoundManager.ToolBreak.Play();
            }
        }

        public void KnockBack(Dir direction, int amount)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.ItemStuff.BuildingItems;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
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

        public PlaceableBuilding Building { get; set; }

        public string id { get; set; }

        public int TextureColumn { get; set; }
        public int TextureRow { get; set; }
        public Rectangle SourceTextureRectangle { get; set; }
        public Rectangle DestinationTextureRectangle { get; set; }

        public int Price { get; set; }


        public bool IsWorldItem { get; set; }


        int directionX = Game1.Utility.RGenerator.Next(-2, 2);

        int directionY = Game1.Utility.RGenerator.Next(-2, 2);

        public int SmeltedItem { get; set; }
        public int Durability { get; set; }
        public int PlaceID { get; set; }
        public string TilingSet { get; set; }
        public int StaminaRestored { get; set; }
        public int Type { get; set; }
        public int AnimationColumn { get; set; }

        public Dictionary<int, int> TilingDictionary { get; set; }




        public Item(ItemData itemData)
        {
            this.Name = itemData.Name;
            this.Description = itemData.Description;
            this.InvMaximum = itemData.InvMaximum;
            this.ID = itemData.ID;
            this.Price = itemData.Price;
            this.TextureColumn = itemData.TextureColumn;
            this.TextureRow = itemData.TextureRow;
            this.SourceTextureRectangle = Game1.AllTextures.GetItemTextureFromAtlas(TextureRow, TextureColumn);
            this.SmeltedItem = itemData.SmeltedItem;
            this.Durability = itemData.Durability;

            if (itemData.Plantable)
            {
                this.IsPlantable = itemData.Plantable;
            }

            this.PlaceID = itemData.PlaceID;
            
            this.StaminaRestored = itemData.StaminaRestored;
            this.Type = itemData.Type;
            this.AnimationColumn = itemData.AnimationColumn;

            this.TilingSet = itemData.TilingSet;
            if(this.TilingSet != null)
            {
                switch (TilingSet)
                {
                    case "FenceTiling":
                        this.TilingDictionary = Game1.Procedural.FenceTiling;
                        break;
                }
                
            }
        }
        public void Load()
        {
            if (IsWorldItem)
            {
                this.ItemSprite = new Sprite(Graphics, Game1.AllTextures.ItemSpriteSheet, SourceTextureRectangle,
                    this.WorldPosition, 16, 16)
                { IsBobbing = true, TextureScaleX = .75f, TextureScaleY = .75f, IsWorldItem = true, LayerDepth = .7f, ColliderType = ColliderType.Item, Entity = this };
                this.Ignored = true;
            }
            if (!IsWorldItem)
            {
                this.ItemSprite = new Sprite(Graphics, Game1.AllTextures.ItemSpriteSheet, SourceTextureRectangle, new Vector2(500, 635), 16, 16) { LayerDepth = .4f };
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsWorldItem)
            {
                ItemSprite.Update(gameTime);


                if (IsTossable == true)
                {
                    ItemSprite.Toss(gameTime, 1f, 1f);
                    if(ItemSprite.IsTossed)
                    {
                        this.Ignored = false;
                        IsTossable = false;
                        ItemSprite.IsTossed = false;
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
            if (IsWorldItem)
            {
                this.ItemSprite.Draw(spriteBatch, .4f);
            }

        }

        public void PlayerCollisionInteraction()
        {
            if(!this.Ignored && Game1.Player.Inventory.IsPossibleToAddItem(this))
            {
                Magnetize(Game1.Player.position);
            }
            
        }

        public void Magnetize(Vector2 playerpos)
        {
            if (IsWorldItem)
            {

                if (Game1.Player.MainCollider.IsIntersecting(ItemSprite))
                {

                    Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.PickUpItem);
                        Game1.GetCurrentStage().AllItems.Remove(this);

                    
                    Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(this.ID, null));
                    Game1.Player.UserInterface.BackPack.CheckGridItem();



                }
                Vector2 dir = new Vector2(Game1.Player.MainCollider.Rectangle.X, Game1.Player.MainCollider.Rectangle.Y) - ItemSprite.Position;
                dir.Normalize();
                ItemSprite.Position += dir;
                //ItemSprite.Position.X -= playerpos.X ;
                //ItemSprite.Position.Y -= playerpos.Y;

            }
        }


        public void AlterDurability(int amountToSubtract)
        {
            this.Durability -= amountToSubtract;
            if(this.Durability <= 0)
            {
                Game1.Player.Inventory.RemoveItem(this);
                Game1.SoundManager.ToolBreak.Play();
            }
        }

        public void KnockBack(Dir direction, int amount)
        {
            throw new NotImplementedException();
        }
    }
}
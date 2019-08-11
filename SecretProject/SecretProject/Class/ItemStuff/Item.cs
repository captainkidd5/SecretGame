﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff.BuildingItems;
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
    [Serializable, XmlRoot("Item")]
    public class Item
    {
        public string Name { get; set; }
        public int ID { get; set; }

        public int Count { get; set; } = 0;
        public int InvMaximum { get; set; }
        public int WorldMaximum { get; set; }

        [XmlIgnore]
        public bool IsFull { get; set; }
        public bool Ignored { get; set; }

        [XmlIgnore]
        public bool IsMagnetized { get; set; } = false;
        [XmlIgnore]
        public bool IsMagnetizable { get; set; } = false;

        [XmlIgnore]
        public Vector2 WorldPosition;

        [XmlIgnore]
        public bool IsTossable { get; set; } = false;

        public bool IsDropped { get; set; }
        [XmlIgnore]
        public Sprite ItemSprite { get; set; }
        [XmlIgnore]
        public GraphicsDevice Graphics { get; set; }
        [XmlIgnore]
        public ContentManager Content { get; set; }

        public bool IsPlaceable { get; set; } = false;
        public bool IsPlantable { get; set; }
        [XmlIgnore]
        public PlaceableBuilding Building { get; set; }

        public string id { get; set; }

        public int TextureColumn { get; set; }
        public int TextureRow { get; set; }
        public Rectangle SourceTextureRectangle { get; set; }
        public Rectangle DestinationTextureRectangle { get; set; }

        public int Price { get; set; }

        [XmlIgnore]
        public bool IsWorldItem { get; set; }

        [XmlIgnore]
        int directionX = Game1.Utility.RGenerator.Next(-2, 2);
        [XmlIgnore]
        int directionY = Game1.Utility.RGenerator.Next(-2, 2);

        //Need empty constructor for serialization!
        public Item()
        {

        }



        public Item(ItemData itemData)
        {
            this.Name = itemData.Name;
            this.InvMaximum = itemData.InvMaximum;
            this.ID = itemData.ID;
            this.Price = itemData.Price;
            this.TextureColumn = itemData.TextureColumn;
            this.TextureRow = itemData.TextureRow;
            this.SourceTextureRectangle = Game1.AllTextures.GetItemTextureFromAtlas(TextureRow, TextureColumn);


        }

        //only used on startup for raw items
     


        //Load to add proper texture to the itemsprite.
        public void Load()
        {
            if (IsWorldItem)
            {
                this.ItemSprite = new Sprite(Graphics, Game1.AllTextures.ItemSpriteSheet, SourceTextureRectangle,
                    this.WorldPosition, 16, 16)
                { IsBobbing = true, TextureScaleX = .75f, TextureScaleY = .75f, IsWorldItem = true, LayerDepth = .7f };
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

                if (!ItemSprite.PickedUp)
                {
                    this.ItemSprite.Bobber(gameTime);
                }
                //ItemSprite.UpdateDestinationRectangle();
                if (IsMagnetizable && Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(this.ID, null, false)))
                {
                    IsMagnetized = true;
                    IsDropped = false;
                    //ItemSprite.PickedUp = true;

                    IsMagnetizable = false;

                }

                if (IsTossable == true)
                {
                    ItemSprite.Toss(gameTime, 1f, 1f);
                    //IsTossable = false;
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

        public void Magnetize(Vector2 playerpos)
        {
            if (IsWorldItem)
            {


                if (ItemSprite.TextureScaleX <= 0f || ItemSprite.TextureScaleY <= 0f)
                {
                    if (!ItemSprite.PickedUp)
                    {
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.PickUpItemInstance, false, 0);
                        ItemSprite.PickedUp = true;
                        Ignored = true;
                        this.IsMagnetized = false;
                        Game1.GetCurrentStage().AllItems.Remove(this);
                    }

                    //ItemSprite.IsDrawn = false;
                }
                ItemSprite.Position.X -= playerpos.X - 2;
                ItemSprite.Position.Y -= playerpos.Y;
                ItemSprite.TextureScaleX -= .1f;
                ItemSprite.TextureScaleY -= .1f;
            }
        }
    }
}
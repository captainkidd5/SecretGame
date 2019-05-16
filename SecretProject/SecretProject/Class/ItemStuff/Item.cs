using Microsoft.Xna.Framework;
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

namespace SecretProject.Class.ItemStuff
{
    [Serializable, XmlRoot("Item")]
    public class Item
    {
        public string Name { get; set; }
        public int ID { get; set; }
        [XmlIgnore]
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

        [XmlIgnore]
        public Texture2D Texture { get; set; }
        public bool IsDropped { get; set; }
        [XmlIgnore]
        public Sprite ItemSprite { get; set; }
        [XmlIgnore]
        public GraphicsDevice Graphics { get; set; }
        [XmlIgnore]
        public ContentManager Content { get; set; }

        public bool IsPlaceable { get; set; } = false;
        [XmlIgnore]
        public PlaceableBuilding Building { get; set; }

        public string id { get; set; }

        public string TextureString { get; set; }

        [XmlIgnore]
        public bool IsWorldItem { get; set; }

        [XmlIgnore]
        int directionX = Game1.RGenerator.Next(-2, 2);
        [XmlIgnore]
        int directionY = Game1.RGenerator.Next(-2, 2);

        //Need empty constructor for serialization!
        public Item()
        {

        }

        //for every cloned item
        public Item(Item item)
        {
            //string ID = item.id.ToString();
            this.Name = item.Name;
            this.InvMaximum = item.InvMaximum;
            this.TextureString = item.TextureString;
            this.Texture = item.Texture;
            this.IsPlaceable = item.IsPlaceable;
            this.ID = item.ID;

            this.Graphics = item.Graphics;
            this.Content = item.Content;

            if(this.IsPlaceable)
            {

             this.Building = new PlaceableBuilding(this.ID);

            }

         }
        
        public Item(int id, GraphicsDevice graphics, ContentManager content)
        {
            string ID = id.ToString();
            this.Name = Game1.ItemVault.RawItems[ID].Name;
            this.InvMaximum = Game1.ItemVault.RawItems[ID].InvMaximum;
            this.TextureString = Game1.ItemVault.RawItems[ID].TextureString;
            this.Texture = content.Load<Texture2D>(TextureString);
            this.IsPlaceable = Game1.ItemVault.RawItems[ID].IsPlaceable;
            this.ID = id;

            this.Graphics = graphics;
            this.Content = content;                
        }

        //Load to add proper texture to the itemsprite.
        public void Load()
        {
            if (IsWorldItem)
            {
                this.ItemSprite = new Sprite(Graphics, Content, this.Texture, this.WorldPosition, false, .4f) { IsBobbing = true, ScaleX = .75f, ScaleY = .75f };
            }
            if (!IsWorldItem)
            {
                this.ItemSprite = new Sprite(Graphics, Content, this.Texture, new Vector2(500, 635), false, .4f);
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

                    if (IsMagnetizable && Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(this.ID, null, false)))
                    {
                        IsMagnetized = true;
                        IsDropped = false;
                        //ItemSprite.PickedUp = true;
                        
                        IsMagnetizable = false;

                    }
                
                if (IsTossable == true)
                {
                    ItemSprite.Toss(gameTime, directionX, directionY);
                    //IsTossable = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(IsWorldItem)
            {
                this.ItemSprite.Draw(spriteBatch, .4f);
            }
            
        }

        public void Magnetize(Vector2 playerpos)
        {
            if (IsWorldItem)
            {


                if (ItemSprite.ScaleX <= 0f || ItemSprite.ScaleY <= 0f)
                {
                    if (!ItemSprite.PickedUp)
                    {
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.PickUpItemInstance, false, 0);
                        ItemSprite.PickedUp = true;
                        Ignored = true;
                        this.IsMagnetized = false;
                        Game1.GetCurrentStage().allItems.Remove(this);
                    }

                    //ItemSprite.IsDrawn = false;
                }
                ItemSprite.Position.X -= playerpos.X;
                ItemSprite.Position.Y -= playerpos.Y;
                ItemSprite.ScaleX -= .1f;
                ItemSprite.ScaleY -= .1f;
            }
        }
    }
}


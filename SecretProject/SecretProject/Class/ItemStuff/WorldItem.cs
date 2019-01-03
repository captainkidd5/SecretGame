using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff.Items;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
    public class WorldItem
    {


        public string Name { get; set; }
        public int ID { get; set; }
        public int Count { get; set; }
        public int WorldMaximum { get; set; }
        public Texture2D Texture { get; set; }
        public bool IsDropped { get; set; }
        public Sprite ItemSprite { get; set; }
        public bool IsFull { get; set; }

        public bool Ignored { get; set; }

        public bool IsMagnetized { get; set; }
        public bool IsMagnetizable { get; set; }

        public GraphicsDevice Graphics { get; set; }
        public ContentManager Content { get; set; }


        public WorldItem(string Name, GraphicsDevice graphics, ContentManager content, Vector2 WorldPosition)
        {
            this.Content = content;
            this.Graphics = graphics;
            IsDropped = true;
            this.Name = Name;

            Ignored = false;
            switch (Name)
            {
                case "pie":
                    this.Texture = content.Load<Texture2D>("Item/pie");
                    this.WorldMaximum = 5;
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, WorldPosition, true, .4f);
                    break;

                case "shrimp":
                    this.Texture = content.Load<Texture2D>("Item/puzzleFish");
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, WorldPosition, true, .4f);
                    this.WorldMaximum = 10;

                    break;

                default:
                    throw new NotImplementedException();


            }


        }

        public void Update(GameTime gameTime)
        {
            if(!ItemSprite.PickedUp)
            {
                this.ItemSprite.Bobber(gameTime);
            }
            

            if(IsDropped)
            {
                if(IsMagnetizable && Game1.Player.Inventory.TryAddItem(new InventoryItem(this.Name, this.Graphics, this.Content)))
                {
                    IsMagnetized = true;
                    IsDropped = false;
                    ItemSprite.PickedUp = true;

                }
            }

            

        }

        public void Draw(SpriteBatch spriteBatch)
        {

          this.ItemSprite.Draw(spriteBatch, .4f);

            
        }

        public void Magnetize(Vector2 playerpos)
        {

                if (ItemSprite.ScaleX <= 0f || ItemSprite.ScaleY <= 0f)
                {
                    if (ItemSprite.IsDrawn)
                    {
                    ItemSprite.BubbleInstance.Play();
                    ItemSprite.PickedUp = true;
                    Ignored = true;
                    }
                
                    ItemSprite.IsDrawn = false;
                }
            ItemSprite.Position.X -= playerpos.X;
            ItemSprite.Position.Y -= playerpos.Y;
            ItemSprite.ScaleX -= .1f;
            ItemSprite.ScaleY -= .1f;

        }
    }
}

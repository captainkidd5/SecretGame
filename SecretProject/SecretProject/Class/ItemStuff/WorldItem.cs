﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff.BuildingItems;
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

        public bool IsTossable { get; set; } = false;

        public GraphicsDevice Graphics { get; set; }
        public ContentManager Content { get; set; }

        public Vector2 WorldPosition;

        //placeable part

        public bool IsPlaceable { get; set; } = false;
        public PlaceableBuilding Building { get; set; }

        int directionX = Game1.RGenerator.Next(-2, 2);
        int directionY = Game1.RGenerator.Next(-2, 2);


        public WorldItem(string Name, GraphicsDevice graphics, ContentManager content, Vector2 worldPosition)
        {
            this.Content = content;
            this.Graphics = graphics;
            IsDropped = true;
            this.Name = Name;
            this.WorldPosition = worldPosition;

            
            

            Ignored = false;
            switch (Name)
            {
                case "pie":
                    this.Texture = Game1.AllTextures.pie;
                    this.WorldMaximum = 5;
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, this.WorldPosition, true, .4f);
                    break;

                case "shrimp":
                    this.Texture = Game1.AllTextures.puzzleFish;
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, this.WorldPosition, true, .4f);
                    this.WorldMaximum = 10;
                    break;

                case "grass":
                    this.Texture = Game1.AllTextures.grass;
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, this.WorldPosition, true, .4f);
                    this.WorldMaximum = 5;
                    ItemSprite.ScaleX = .5f;
                    ItemSprite.ScaleY = .5f;
                    break;

                case "barrel":
                    this.Texture = Game1.AllTextures.barrel;
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, this.WorldPosition, true, .4f);
                    this.WorldMaximum = 3;
                    ItemSprite.ScaleX = .5f;
                    ItemSprite.ScaleY = .5f;
                    this.IsPlaceable = true;
                    Building = new PlaceableBuilding(Name);
                    break;

                case "secateur":
                    this.Texture = Game1.AllTextures.Secateurs;
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, this.WorldPosition, true, .4f);
                    this.WorldMaximum = 5;
                    ItemSprite.ScaleX = .5f;
                    ItemSprite.ScaleY = .5f;
                    break;

                case "lodgeKey":
                    this.Texture = Game1.AllTextures.lodgeKey;
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, this.WorldPosition, true, .4f);
                    this.WorldMaximum = 1;
                    ItemSprite.ScaleX = .5f;
                    ItemSprite.ScaleY = .5f;
                    break;

                case "shovel":
                    this.Texture = Game1.AllTextures.shovel;
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, this.WorldPosition, true, .4f);
                    this.WorldMaximum = 1;
                    ItemSprite.ScaleX = .5f;
                    ItemSprite.ScaleY = .5f;
                    break;

                case "stone":
                    this.Texture = Game1.AllTextures.stone;
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, this.WorldPosition, true, .4f);
                    this.WorldMaximum = 1;
                    ItemSprite.ScaleX = .5f;
                    ItemSprite.ScaleY = .5f;
                    break;
                /*
                                case "animatedGrass":
                                    this.Texture = content.Load<Texture2D>("Item/grass");
                                    this.ItemSprite = new Sprite(graphics, content, this.Texture, WorldPosition, true, .4f) { IsAnimated = true, Rows = 1, Columns =4 }  ;
                                    this.WorldMaximum = 5;
                                    ItemSprite.ScaleX = .5f;
                                    ItemSprite.ScaleY = .5f;
                                    break;
                                    */
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
                if(IsMagnetizable && Game1.Player.Inventory.TryAddItem(new InventoryItem(this.ID, this.Graphics, this.Content)))
                {
                    IsMagnetized = true;
                    IsDropped = false;
                    ItemSprite.PickedUp = true;

                }
            }

            if(IsTossable == true)
            {
                ItemSprite.Toss(gameTime, directionX, directionY);
                //IsTossable = false;
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
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.PickUpItemInstance, false, 0);
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

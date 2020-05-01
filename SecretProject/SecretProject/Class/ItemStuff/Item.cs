﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Physics;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.TileStuff.SpawnStuff;
using System;
using System.Collections.Generic;
using XMLData.ItemStuff;

namespace SecretProject.Class.ItemStuff
{
    public class Item : IEntity
    {

        public int ID { get; set; }


        public int Count { get; set; }

        public bool Ignored { get; set; }



        public Vector2 WorldPosition;


        public bool IsTossable { get; set; } = false;

        public bool IsDropped { get; set; }

        public Sprite ItemSprite { get; set; }

        public GraphicsDevice Graphics { get; set; }

        public ContentManager Content { get; set; }





        public Rectangle SourceTextureRectangle { get; set; }
        public Rectangle DestinationTextureRectangle { get; set; }
        public float DurabilityLineWidth { get; set; }




        public bool IsWorldItem { get; set; }


        int directionX = Game1.Utility.RGenerator.Next(-2, 2);

        int directionY = Game1.Utility.RGenerator.Next(-2, 2);


        public int Durability { get; set; }

        public string TilingSet { get; set; }
        public int TilingLayer { get; set; }

        public ItemType ItemType { get; set; }
        public int AnimationColumn { get; set; }
        public int CrateType { get; set; }

        public GenerationType GenerationType { get; set; }


        public List<Item> AllItems { get; set; }
        public Bouncer Bouncer { get; set; }

        public float LayerDepth { get; set; }

        public Collider Collider { get; set; }
        private Vector2 PrimaryVelocity;

        public Item(ItemData itemData, List<Item> allItems)
        {
            this.AllItems = allItems;

            this.ID = itemData.ID;


            this.SourceTextureRectangle = Game1.AllTextures.GetItemTexture(this.ID, 40);

            this.Durability = itemData.Durability;



            this.ItemType = itemData.Type;
            this.AnimationColumn = itemData.AnimationColumn;

            this.TilingSet = itemData.TilingSet;
            if (this.TilingSet != null)
            {
                this.GenerationType = (GenerationType)Enum.Parse(typeof(GenerationType), this.TilingSet);


            }


            this.TilingLayer = itemData.TilingLayer;
            this.CrateType = itemData.CrateType;


        }
        public void Load()
        {
            if (this.IsWorldItem)
            {
                this.ItemSprite = new Sprite(this.Graphics, Game1.AllTextures.ItemSpriteSheet, this.SourceTextureRectangle,
                    WorldPosition, 16, 16)
                {
                    TextureScaleX = .75f,
                    TextureScaleY = .75f,
                    IsWorldItem = true,
                    LayerDepth = .7f,
                    ColliderType = ColliderType.Item,
                    Entity = this
                };
                this.Ignored = true;
                this.Bouncer = new Bouncer(WorldPosition, Game1.Player.controls.Direction);


                this.LayerDepth = .5f + (this.WorldPosition.Y) * Game1.Utility.ForeGroundMultiplier;
                this.Collider = new Collider(this.Graphics, this.SourceTextureRectangle, this, ColliderType.Item);
                AllItems.Add(this);
            }
            else
            {
                this.ItemSprite = new Sprite(this.Graphics, Game1.AllTextures.ItemSpriteSheet, this.SourceTextureRectangle, new Vector2(500, 635), 16, 16) { LayerDepth = .4f };
                if (this.Durability > 0)
                {

                }
                this.DurabilityLineWidth = GetDurabilityLineLength();
            }
        }

        private float GetDurabilityLineLength()
        {
            return (float)this.Durability / (float)Game1.ItemVault.GetItem(this.ID).Durability;
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsWorldItem)
            {
                this.ItemSprite.Update(gameTime);
                this.Collider.Rectangle = this.ItemSprite.DestinationRectangle;

                if (this.Bouncer != null)
                {
                    if (CheckCollisions())
                    {
                        return;
                    }
                    this.PrimaryVelocity = Bouncer.Velocity;

                    this.ItemSprite.Position = Bouncer.Update(gameTime, ref this.PrimaryVelocity);
                    if (!this.Bouncer.IsActive)
                    {
                        this.Bouncer = null;
                    }
                    // CheckAndHandleCollisions();
                }
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

        private bool CheckCollisions()
        {
            if (this.Bouncer != null)
            {
                List<ICollidable> returnObjects = new List<ICollidable>();

                Game1.GetCurrentStage().QuadTree.Retrieve(returnObjects, this.Collider);

                for (int i = 0; i < returnObjects.Count; i++)
                {
                    if (returnObjects[i].ColliderType == ColliderType.inert)
                    {
                        if (this.Collider.HandleMove(this.WorldPosition, ref PrimaryVelocity, returnObjects[i]))
                        {
                            Console.WriteLine("Debug!");
                            this.Bouncer = null;
                            this.IsTossable = false;
                            return true;
                        }

                    }
                }

            }

            return false;
        }



        private bool DidCollide(Vector2 velocity, ICollidable objectBody)
        {
            Rectangle rect = this.ItemSprite.Rectangle;

            rect.X += (int)velocity.X;
            rect.Y += (int)velocity.Y;

            if (rect.Intersects(objectBody.Rectangle))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsWorldItem)
            {
                this.ItemSprite.Draw(spriteBatch, this.LayerDepth);

            }

        }

        public void DamageCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
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

                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.PickUpItem, true, .5f, 0f);
                    this.AllItems.Remove(this);
                    // Game1.GetCurrentStage().AllTiles.GetItems(this.WorldPosition).Remove(this);
                    this.IsWorldItem = false;
                    if (this.Durability > 0)
                    {
                        this.AlterDurability(0);
                    }
                    Game1.Player.Inventory.TryAddItem(this);
                    Game1.Player.UserInterface.BackPack.CheckGridItem();



                }
                Vector2 dir = new Vector2(Game1.Player.MainCollider.Rectangle.X, Game1.Player.MainCollider.Rectangle.Y) - this.ItemSprite.Position;
                dir.Normalize();
                this.ItemSprite.Position += dir;


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
            else
            {
                this.DurabilityLineWidth = GetDurabilityLineLength();
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

        public void MouseCollisionInteraction()
        {
            throw new NotImplementedException();
        }
    }
}
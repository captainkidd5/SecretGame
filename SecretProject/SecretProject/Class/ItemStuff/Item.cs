
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Physics;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.TileStuff.SpawnStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using XMLData.ItemStuff;

namespace SecretProject.Class.ItemStuff
{
    public class Item : IEntity
    {

        public int ID { get; set; }


        public int Count { get; set; }

        public bool Ignored { get; set; }



        public Vector2 WorldPosition;


        public bool IsTossable { get; set; }

        public bool IsDropped { get; set; }

        public Sprite ItemSprite { get; set; }

        private GraphicsDevice Graphics { get; set; }

        private ContentManager Content { get; set; }





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

        public float LayerDepth { get; set; }

        public Body ItemBody { get; set; }
        public Body ArtificialFloorBody { get; set; }

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
                    Entity = this
                };
                this.Ignored = true;

               
                float randomOffSet = Game1.Utility.RFloat(Utility.ForeGroundMultiplier, Utility.ForeGroundMultiplier * 10);
                this.LayerDepth = .5f + (this.WorldPosition.Y) * Utility.ForeGroundMultiplier + randomOffSet;

                AllItems.Add(this);

                ItemBody = BodyFactory.CreateCircle(Game1.VelcroWorld, 4f, 1f, new Vector2(WorldPosition.X + 8, WorldPosition.Y + 8), BodyType.Dynamic);
                ItemBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Item;
                ItemBody.IgnoreGravity = false;
                
               ItemBody.Mass = 1f;
                ItemBody.Friction = .8f;
                ItemBody.GravityScale = 1.5f;
                ItemBody.Restitution = .4f;
                ItemBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Player | VelcroPhysics.Collision.Filtering.Category.Solid | VelcroPhysics.Collision.Filtering.Category.ArtificialFloor;
                ItemBody.OnCollision += OnCollision;
                ItemBody.ApplyLinearImpulse(new Vector2(20,- 15));

                //Artificial floor ensures that objects dont just fall to the bottom of the map. Floor x follows item x.
                ArtificialFloorBody = BodyFactory.CreateRectangle(Game1.VelcroWorld, 20, 2, 1f);
                ArtificialFloorBody.Position = new Vector2(ItemBody.Position.X, ItemBody.Position.Y + 20);
                ArtificialFloorBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.ArtificialFloor;
                ArtificialFloorBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Item;
                ArtificialFloorBody.IgnoreGravity = true;
                ArtificialFloorBody.BodyType = BodyType.Static;


                
                //Body.Apl
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
        private void OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(fixtureB.CollisionCategories ==  VelcroPhysics.Collision.Filtering.Category.Player)
            {
                Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.PickUpItem, true, .5f, 0f);
                
                this.IsWorldItem = false;
                if (this.Durability > 0)
                {
                    this.AlterDurability(0);
                }
                Game1.Player.Inventory.TryAddItem(this);
                Game1.Player.UserInterface.BackPack.CheckGridItem();


                Game1.VelcroWorld.RemoveBody(this.ItemBody);
                Game1.VelcroWorld.RemoveBody(ArtificialFloorBody);

                this.AllItems.Remove(this);
                
            }
            //TryMagnetize();
           // 
        }

            private float GetDurabilityLineLength()
        {
            return (float)this.Durability / (float)Game1.ItemVault.GetItem(this.ID).Durability;
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsWorldItem)
            {
                if(ItemBody.Position.Y <= ArtificialFloorBody.Position.Y - 5)
                {
                    ItemBody.ApplyForce(new Vector2(0, 100));
                    ArtificialFloorBody.Position = new Vector2(ItemBody.Position.X, ArtificialFloorBody.Position.Y);
                }
                

                this.ItemSprite.Position = this.ItemBody.Position;
                this.ItemSprite.Update(gameTime);



            }
        }




        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsWorldItem)
            {
                this.ItemSprite.Draw(spriteBatch, this.LayerDepth);

            }

        }

        public void TryMagnetize()
        {
            if (!this.Ignored && Game1.Player.Inventory.IsPossibleToAddItem(this))
            {
                Magnetize(Game1.Player.position);
            }

        }

        public void Magnetize(Vector2 playerpos)
        {
            //if (this.IsWorldItem)
            //{



            //    if (Game1.Player.MainCollider.IsIntersecting(this.RectangleCollider))
            //    {

            //        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.PickUpItem, true, .5f, 0f);
            //        this.AllItems.Remove(this);
            //        // Game1.CurrentStage.AllTiles.GetItems(this.WorldPosition).Remove(this);
            //        this.IsWorldItem = false;
            //        if (this.Durability > 0)
            //        {
            //            this.AlterDurability(0);
            //        }
            //        Game1.Player.Inventory.TryAddItem(this);
            //        Game1.Player.UserInterface.BackPack.CheckGridItem();



            //    }
            //    Vector2 dir = new Vector2(Game1.Player.MainCollider.Rectangle.X, Game1.Player.MainCollider.Rectangle.Y) - this.ItemSprite.Position;
            //    dir.Normalize();
            //    this.ItemSprite.Position += dir;


            //}
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

        public void DamageCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
        {
            throw new NotImplementedException();
        }
    }
}
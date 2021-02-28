using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Collision.Handlers;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace SecretProject.Class.Physics.CollisionDetection
{
    /// <summary>
    /// Wrapper class for Velcro Physics Body! <see cref="TileObjectHelper"/> .
    /// </summary>
    public class HullBody
    {
        public List<HullBody> CollidableObjects { get; set; }

        public Body Body { get; set; }
        public Vector2 Position { get { return Body.Position; } set { Body.SetTransform(value, 0f); } }

        public HullBody(Vector2 position)
        {

        }

        /// <summary>
        /// Constructor for rectangles.
        /// </summary>
        public HullBody(List<HullBody> collidableObjects, BodyType bodyType, Vector2 position, int width, int height, Category category,
            List<Category> categoriesCollidesWith, OnCollisionHandler cDelegate, OnSeparationHandler sDelegate, float density = 1f, float rotation = 0f, bool isSensor = false)
        {
            this.CollidableObjects = collidableObjects;
            CreateRectangle(width, height, density, rotation, position, bodyType, category, categoriesCollidesWith, cDelegate, sDelegate, isSensor);
        }

        /// <summary>
        /// Constructor for circles.
        /// </summary>
        public HullBody(List<HullBody> collidableObjects, BodyType bodyType, Vector2 position, float radius, Category category, List<Category> categoriesCollidesWith,
             OnCollisionHandler cDelegate, OnSeparationHandler sDelegate, float density = 1f, bool isSensor = false)
        {
            this.CollidableObjects = collidableObjects;
            CreateCircle(radius, density, position, bodyType, category, categoriesCollidesWith, cDelegate, sDelegate, isSensor);
        }


        protected virtual void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            throw new NotImplementedException();
        }

        public virtual void CreateCircle(float radius, float density, Vector2 position, BodyType bodyType, Category category, List<Category> categoriesCollidesWith,
            OnCollisionHandler cDelegate, OnSeparationHandler sDelegate, bool isSensor = false)
        {
            Body = BodyFactory.CreateCircle(Game1.VelcroWorld, radius, density, position, bodyType);
            Body.CollisionCategories = category;
            foreach (Category c in categoriesCollidesWith)
            {
                Body.CollidesWith = c;
            }

            Body.OnCollision += cDelegate;
            Body.OnSeparation += sDelegate;
            Body.IsSensor = isSensor;

        }

        public virtual void CreateRectangle(float width, float height, float density, float rotation, Vector2 position, BodyType bodyType, Category category, List<Category> categoriesCollidesWith,
            OnCollisionHandler cDelegate, OnSeparationHandler sDelegate, bool isSensor = false)
        {
            Body = BodyFactory.CreateRectangle(Game1.VelcroWorld, width, height, density, position, rotation, bodyType);
            Body.CollisionCategories = category;
            Body.OnCollision += cDelegate;
            foreach (Category c in categoriesCollidesWith)
            {
                Body.CollidesWith = c;
            }

            Body.OnCollision += cDelegate;
            Body.OnSeparation += sDelegate;
            Body.IsSensor = isSensor;
        }

        public void Destroy()
        {
            Game1.VelcroWorld.RemoveBody(Body);
            CollidableObjects.Remove(this);
        }
    }
}

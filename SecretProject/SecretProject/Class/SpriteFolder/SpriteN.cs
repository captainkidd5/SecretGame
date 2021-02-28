using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretProject.Class.SpriteFolder
{

    /// <summary>
    /// Base class for any drawn entity.
    /// </summary>
    public class SpriteN : Component
    {
        protected Texture2D Texture { get; set; }

        protected Vector2 Position { get; set; }

        protected Vector2 Origin { get; set; }
        protected float Scale { get; set; }
        protected float LayerDepth { get; set; }
        protected float FixedLayerOffSet { get; set; }
        protected SpriteEffects SpriteEffectsAnchor { get; set; } //this is set when sprite is created and never changed
        public Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, SourceRectangle.Width, SourceRectangle.Height);
            }
        }
        public Rectangle SourceRectangle { get; private set; }
        public SpriteEffects SpriteEffects { get; set; }
        public Color PrimaryColor { get; set; }

        public float Rotation { get; set; }

        /// <summary>
        /// Constructor for world sprites.
        /// </summary>
        public SpriteN(GraphicsDevice graphics, ContentManager content, Texture2D texture, Rectangle sourceRectangle,
            Color? primaryColor, float? rotation, Vector2? origin, float? scale, LayerDepths layer, float layerOffset = 0f, bool randomizeLayers = false, bool flip = false, float customLayer = 0f) : base(graphics, content)
        {
            this.Texture = texture;
            this.SourceRectangle = sourceRectangle;
            this.PrimaryColor = primaryColor ?? Color.White;
            this.Rotation = rotation ?? 0f;
            this.Origin = origin ?? Vector2.Zero;
            this.Scale = scale ?? 1f;
            this.FixedLayerOffSet = layerOffset;

            if (randomizeLayers)
                this.LayerDepth = Globals.GetSpriteVariedLayerDepth(layer);
            else if (customLayer != 0f)
                this.LayerDepth = customLayer;
            else
                this.LayerDepth = Globals.GetLayerDepth(layer);

            if (flip)
                SpriteEffectsAnchor = SpriteEffects.FlipHorizontally;
            else
                SpriteEffectsAnchor = SpriteEffects.None;
        }


        public void Update(GameTime gameTime, Vector2 position)
        {
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRectangle, PrimaryColor, Rotation, Origin, Scale, SpriteEffects, GetYAxisLayerDepth() + FixedLayerOffSet);
        }

        public void Draw(SpriteBatch spriteBatch, float customLayerDepth)
        {
            spriteBatch.Draw(Texture, Position, SourceRectangle, PrimaryColor, Rotation, Origin, Scale, SpriteEffects, customLayerDepth + FixedLayerOffSet);
        }

        /// <summary>
        /// Returns a layer depth which is relative to how far up or down the sprite is on the map.
        /// Useful for allowing the sprite to move "in front" or "behind of" other objects.
        /// </summary>
        private float GetYAxisLayerDepth()
        {
            return Globals.GetYAxisLayerDepth(Position, SourceRectangle);
        }

        public void UpdateSourceRectangle(Rectangle rectangle)
        {
            this.SourceRectangle = rectangle;
        }

        public void SetEffectToDefault()
        {
            SpriteEffects = SpriteEffectsAnchor;
        }

        public override void Load()
        {
            throw new NotImplementedException();
        }

        public override void Unload()
        {
            throw new NotImplementedException();
        }

        public void UpdateColor(Color colorToUse)
        {
            PrimaryColor = colorToUse;
        }

    }

}

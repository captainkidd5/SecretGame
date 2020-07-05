using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.PathFinding;
using SecretProject.Class.PathFinding.PathFinder;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Universal;

namespace SecretProject.Class.Misc
{
    public enum GrassCreatureType
    {
        none = 0,
        mouse = 1
    }
    public class GrassCreature : FunItems
    {
        public GrassCreatureType GrassCreatureType { get; set; }
        public string EntityName { get; set; }
        public List<FunItems> FunItems { get; set; }

        private Texture2D Texture { get; set; }
        private Vector2 position;
        private Sprite[] AnimatedSprite { get; set; }

        private Vector2 DestinationPosition { get; set; }

        private float Speed { get; set; } = 1.7f;

        private Navigator Navigator { get; set; }
        private Dir CurrentDirection;
        private float SourceRectangleHeight { get; set; }

        private float ColorMultiplier { get; set; }
        private SimpleTimer SimpleTimer { get; set; }
        private bool IsMoving { get; set; }


        public GrassCreature(GrassCreatureType grassCreatureType, GraphicsDevice graphics, Vector2 position, List<FunItems> funItems)
        {
            this.position = position;
            this.FunItems = funItems;
            this.Texture = Game1.AllTextures.ButterFlys;

            this.AnimatedSprite = new Sprite[4];
            this.EntityName = GetName(funItems.Count);
            this.GrassCreatureType = grassCreatureType;
            switch (grassCreatureType)
            {
                case GrassCreatureType.mouse:
                    this.AnimatedSprite[0] = new Sprite(graphics, Texture, 0, 64, 16, 16, 3, .25f, this.position, changeFrames: false);
                    this.AnimatedSprite[1] = new Sprite(graphics, Texture, 16, 64, 16, 16, 3, .25f, this.position, changeFrames: false);
                    this.AnimatedSprite[2] = new Sprite(graphics, Texture, 32, 64, 16, 16, 3, .25f, this.position, changeFrames: false) { Flip = true };
                    this.AnimatedSprite[3] = new Sprite(graphics, Texture, 32, 64, 16, 16, 3, .25f, this.position, changeFrames: false);

                    this.SourceRectangleHeight = 16;
                    this.SimpleTimer = new SimpleTimer(3f);
                    this.IsMoving = true;
                    this.ColorMultiplier = 1f;
                    break;

            }

            this.Navigator = new Navigator(this.EntityName, Game1.CurrentStage.AllTiles.PathGrid.Weight);
        }

        private string GetName(int index)
        {
            return this.GrassCreatureType.ToString() + "_" + index.ToString();
        }

        private bool Fade(GameTime gameTime)
        {
            if (SimpleTimer.Run(gameTime))
                return true;
            if (this.ColorMultiplier >= 0)
            {
                this.ColorMultiplier -= (float)gameTime.ElapsedGameTime.TotalSeconds * 2f;
            }

            return false;
        }

        public void Update(GameTime gameTime)
        {
            switch (this.GrassCreatureType)
            {
                case GrassCreatureType.mouse:
                    if (IsMoving)
                    {

                    }
                    else
                    {
                        if (Fade(gameTime)) //make mouse more transparent if reached end of path, then delete.
                            FunItems.Remove(this);


                    }
                    Navigator.Wander(gameTime, ref this.position, ref CurrentDirection);
                    this.position += new Vector2(1f, 1f) * Navigator.DirectionVector;


                    if (Navigator.HasReachedNextPoint)
                    {
                        if (Navigator.CurrentPath.Count == 0)
                        {
                            IsMoving = false;
                            //FunItems.Remove(this);
                        }

                    }

                    break;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            this.AnimatedSprite[(int)this.CurrentDirection].ColorMultiplier = this.ColorMultiplier;
            this.AnimatedSprite[(int)this.CurrentDirection].DrawAnimation(spriteBatch, new Vector2(this.position.X - this.SourceRectangleHeight / 3.5f,
                this.position.Y - this.SourceRectangleHeight / 1.5f),
                    .5f + (position.Y + SourceRectangleHeight) * Utility.ForeGroundMultiplier);
        }


    }
}

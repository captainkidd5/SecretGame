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
        private Vector2 Position { get; set; }
        private Sprite[] AnimatedSprite { get; set; }

        private Vector2 DestinationPosition { get; set; }

        private float Speed { get; set; } = 1.7f;

        private Navigator Navigator { get; set; }


        public GrassCreature(GrassCreatureType grassCreatureType, GraphicsDevice graphics, Vector2 position, List<FunItems> funItems)
        {
            this.Position = position;
            this.FunItems = funItems;
            this.Texture = Game1.AllTextures.ButterFlys;
            
            this.AnimatedSprite = new Sprite[3];
            this.EntityName = GetName(funItems.Count);

            switch(grassCreatureType)
            {
                case GrassCreatureType.mouse:
                    this.AnimatedSprite[0] = new Sprite(graphics, Texture, 0, 64, 16, 16, 3, .25f, this.Position, changeFrames: false);
                    this.AnimatedSprite[1] = new Sprite(graphics, Texture, 16, 64, 16, 16, 3, .25f, this.Position, changeFrames: false);
                    this.AnimatedSprite[2] = new Sprite(graphics, Texture, 32, 64, 16, 16, 3, .25f, this.Position, changeFrames: false);
                    break;
            }
            
            this.Navigator = new Navigator(this.EntityName, Game1.CurrentStage.AllTiles.PathGrid.Weight);
        }

        private string GetName(int index)
        {
            return this.GrassCreatureType.ToString() + "_" + index.ToString();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        
    }
}

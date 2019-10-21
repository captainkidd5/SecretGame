using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



using SecretProject.Class.SpriteFolder;

namespace SecretProject.Class.CollisionDetection
{
    public class ObjectBody : ICollidable
    {

        protected Texture2D rectangleTexture;

        public bool ShowRectangle { get; set; }

        public Vector2 Position;
        public Vector2 Velocity;
        public Color Color = Color.White;

        public int Rows { get; set; }

        public int Columns { get; set; }


        public Rectangle DestinationRectangle { get; set; }

        float layerDepth { get; set; } = 1;

        public float Identifier { get; set; } = 1;

        //0 doesnt check for collisions with other objects, 1 does (player, npcs, moving stuff etc)
        public int CollisionType { get; set; }



        public ObjectBody(GraphicsDevice graphicsDevice, Rectangle rectangle, float Identifier)
        {
            this.DestinationRectangle = rectangle;


            SetRectangleTexture(graphicsDevice);

            ShowRectangle = true;
            this.Identifier = Identifier;
            this.CollisionType = 0;
        }


        private void SetRectangleTexture(GraphicsDevice graphicsDevice)
        {
            var Colors = new List<Color>();
            for (int y = 0; y < DestinationRectangle.Height; y++)
            {
                for (int x = 0; x < DestinationRectangle.Width; x++)
                {
                    if (x == 0 || //left side
                        y == 0 || //top side
                        x == DestinationRectangle.Width - 1 || //right side
                        y == DestinationRectangle.Height - 1) //bottom side
                    {
                        Colors.Add(new Color(255, 255, 255, 255));
                    }
                    else
                    {
                        Colors.Add(new Color(0, 0, 0, 0));

                    }

                }
            }
            rectangleTexture = new Texture2D(graphicsDevice, DestinationRectangle.Width, DestinationRectangle.Height);
            rectangleTexture.SetData<Color>(Colors.ToArray());
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(rectangleTexture, new Vector2(Position.X, Position.Y), Color.White);

            
         

        }
        public virtual void Draw(SpriteBatch spriteBatch, float layerDepth)
        {

                this.layerDepth = layerDepth;
                spriteBatch.Draw(rectangleTexture, new Vector2(DestinationRectangle.X, DestinationRectangle.Y), color: Color.White, layerDepth: layerDepth);
            
        }
    }
}

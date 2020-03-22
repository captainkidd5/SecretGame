using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable
{
    public enum ClothingType
    {
        Hair = 1,
        Shirt = 2,
        Pants = 3,
        Shoes = 4,
        Base = 5
    }

    public class WardrobeNew : ISaveable
    {
        public GraphicsDevice Graphics { get; set; }


        public int MaxSpriteIndex { get; set; }



        public int HairIndex { get; set; }
        public int ShirtIndex { get; set; }
        public int PantsIndex { get; set; }
        public int ShoesIndex { get; set; }

        public int[] AllIndexes { get; set; }


        public ClothingPiece Hair { get; set; }
        public ClothingPiece Shirt { get; set; }
        public ClothingPiece Pants { get; set; }
        public ClothingPiece Shoes { get; set; }

        public List<ClothingPiece> BasicClothing { get; set; }

        public float HairDepth { get; private set; }
        public float ShirtDepth { get; private set; }
        public float PantsDepth { get; private set; }
        public float ShoesDepth { get; private set; }

        public AnimationSet RunSet { get; set; }

        public AnimationSet CurrentAnimationSet { get; set; }


        public WardrobeNew(GraphicsDevice graphics, Vector2 playerPosition)
        {
            this.Graphics = graphics;


            this.MaxSpriteIndex = 64;

            this.AllIndexes = new int[4]
            {
                HairIndex,
                ShirtIndex,
                PantsIndex,
                ShoesIndex,
            };

            this.HairDepth = 00000011f;
            this.ShirtDepth = .00000010f;
            this.PantsDepth = .00000009f;
            this.ShoesDepth = .00000008f;

            this.HairIndex = 0;
            this.ShirtIndex = 0;
            this.PantsIndex = 0;
            this.ShoesIndex = 0;

            this.Hair = new ClothingPiece(graphics, Game1.AllTextures.PlayerHair, Color.White, this.HairIndex, this.HairDepth);
            this.Shirt = new ClothingPiece(graphics, Game1.AllTextures.PlayerShirt, Color.White, this.ShirtIndex, this.ShirtDepth);
            this.Pants = new ClothingPiece(graphics, Game1.AllTextures.PlayerPants, Color.White, this.PantsIndex, this.PantsDepth);
            this.Shoes = new ClothingPiece(graphics, Game1.AllTextures.PlayerShoes, Color.White, this.ShoesIndex, this.ShoesDepth);

            this.BasicClothing = new List<ClothingPiece>()
            { Hair, Shirt, Pants, Shoes};

            this.RunSet = new AnimationSet(graphics, this.BasicClothing);


        }



        public void UpdateMovementAnimations(GameTime gameTime, Vector2 position, int currentFrame, bool setFrameZero = false)
        {

            this.CurrentAnimationSet.Update(gameTime, position, currentFrame);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.CurrentAnimationSet.Draw(spriteBatch);
        }

        public void CycleClothing(ClothingLayer layer, Vector2 playerPosition, bool backwards = false)
        {
            int frameChangeAmt = 32;
            ///if (backwards)
          //  {
          //      frameChangeAmt = frameChangeAmt * -1;
          //  }
          ////  int newSpriteY = BasicMovementAnimations[0, (int)layer].FirstFrameY + frameChangeAmt;

          //  if (newSpriteY > this.MaxSpriteIndex)
          //  {
          //      newSpriteY = 0;
          //  }
          //  if (newSpriteY < 0)
          //  {
          //      newSpriteY = this.MaxSpriteIndex;
          //  }


          //  for (int i = 0; i < BasicMovementAnimations.GetLength(0); i++)
          //  {
          //     // BasicMovementAnimations[i, (int)layer].FirstFrameY = newSpriteY;


          //  }
          //  this.AllIndexes[(int)layer] = newSpriteY;
        }

        public void CycleSwipingClothing()
        {
            //for (int i = 0; i < this.AllIndexes.Length; i++)
            //{
            //    for (int j = 0; j < this.AllIndexes.Length; j++)
            //    {
            //        SwipingAnimations[i, j].FirstFrameY = this.AllIndexes[j];
            //        SwipingAnimations[i, j].UpdateSourceRectangle();
            //    }
            //}
        }

        public void CycleBasicAnimations()
        {
            for (int i = 0; i < this.AllIndexes.Length; i++)
            {

                for (int j = 0; j < this.AllIndexes.Length; j++)
                {
                  //  BasicMovementAnimations[i, j].FirstFrameY = this.AllIndexes[j];
                }
            }
        }

        public void Save(BinaryWriter writer)
        {
            for (int i = 0; i < this.AllIndexes.Length; i++)
            {
                writer.Write(this.AllIndexes[i]);
            }
        }

        public void Load(BinaryReader reader)
        {


            for (int z = 0; z < AllIndexes.Length; z++)
            {
                AllIndexes[z] = reader.ReadInt32();
            }
            CycleBasicAnimations();
            CycleSwipingClothing();

        }
    }
}

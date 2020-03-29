using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Playable.WardrobeStuff;
using SecretProject.Class.Playable.WardrobeStuff.AnimationSetStuff;
using SecretProject.Class.Playable.WardrobeStuff.Pieces;
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
    //public enum AnimationType
    //{
    //    Running = 1,
    //    Chopping = 2,
    //    PickUp = 3,

    //}

    public class WardrobeNew : ISaveable
    {

        public GraphicsDevice Graphics { get; set; }


        public int MaxSpriteIndex { get; set; }



        public int HairIndex { get; set; }
        public int ShirtIndex { get; set; }
        public int PantsIndex { get; set; }
        public int ShoesIndex { get; set; }

        public int[] AllIndexes { get; set; }


        public HairPiece Hair { get; set; }
        public ShirtPiece ShirtPiece { get; set; }
        public PantsPiece PantsPiece { get; set; }
        public HeadPiece BodyPiece { get; set; }
        public ShoesPiece ShoesPiece { get; set; }
        public ArmsPiece ArmsPiece { get; set; }


        public List<ClothingPiece> BasicClothing { get; set; }


        public AnimationSet RunSet { get; set; }

        public AnimationSet CurrentAnimationSet { get; set; }
        public Dir CurrentDirection { get; set; }

        public List<Color> SkinColors { get; set; }
        public int SkinIndex { get; set; }


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



            this.HairIndex = 0;
            this.ShirtIndex = 0;
            this.PantsIndex = 0;
            this.ShoesIndex = 0;

            Hair = new HairPiece();
            ShirtPiece = new ShirtPiece();
            PantsPiece = new PantsPiece();
            BodyPiece = new HeadPiece();
            ShoesPiece = new ShoesPiece();
            ArmsPiece = new ArmsPiece();
            this.BasicClothing = new List<ClothingPiece>()
            { Hair,ShirtPiece, PantsPiece,BodyPiece,ShoesPiece, ArmsPiece};

            this.RunSet = new AnimationSet(graphics, this.BasicClothing, 5);

            this.CurrentAnimationSet = this.RunSet;

            this.SkinColors = new List<Color>()
            {
                new Color(141, 85, 36),
                new Color(198, 134, 66),
                new Color(224, 172, 105),
                new Color(241, 194, 125),
                new Color(255, 219, 172),

            };

        }

        public void IncreaseSkinShade()
        {
            if(this.SkinIndex < this.SkinColors.Count - 1)
            {
                this.SkinIndex++;
            }
            else
            {
                this.SkinIndex = 0;
            }

            this.BodyPiece.Color = this.SkinColors[this.SkinIndex];

            this.ArmsPiece.Color = this.SkinColors[this.SkinIndex];
        }

        public void SetZero()
        {
            this.CurrentAnimationSet.CurrentFrame = 0;
        }

        public void UpdateCurrentDirection(Dir direction)
        {

        }

        public void UpdateMovementAnimations(GameTime gameTime, Vector2 position, Dir direction, bool isMoving)
        {

            this.CurrentAnimationSet.Update(gameTime, position,  direction, isMoving);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.CurrentAnimationSet.Draw(spriteBatch);
        }

        public void SwapAnimations(AnimationSet set)
        {
            switch (set)
            {
                //case AnimationSet.Running:
                //    this.CurrentAnimationSet = RunSet;
                //    break;
            }
            
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
           // CycleBasicAnimations();
            CycleSwipingClothing();

        }
    }
}

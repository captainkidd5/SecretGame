using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Playable.WardrobeStuff;
using SecretProject.Class.Playable.WardrobeStuff.AnimationSetStuff;
using SecretProject.Class.Playable.WardrobeStuff.Pieces;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.UI.MainMenuStuff;
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





        public HairPiece Hair { get; set; }
        public ShirtPiece ShirtPiece { get; set; }
        public PantsPiece PantsPiece { get; set; }
        public HeadPiece HeadPiece { get; set; }
        public ShoesPiece ShoesPiece { get; set; }
        public ArmsPiece ArmsPiece { get; set; }


        public List<ClothingPiece> BasicClothing { get; set; }


        public AnimationSet RunSet { get; set; }

        public AnimationSet CurrentAnimationSet { get; set; }
        public Dir CurrentDirection { get; set; }

        public List<Color> SkinColors { get; set; }
        public int SkinIndex { get; set; }

        public List<Color> HairColors { get; set; }
        public int HairIndex { get; set; }


        public WardrobeNew(GraphicsDevice graphics, Vector2 playerPosition)
        {
            this.Graphics = graphics;


            this.MaxSpriteIndex = 64;



            Hair = new HairPiece();
            ShirtPiece = new ShirtPiece();
            PantsPiece = new PantsPiece();
            HeadPiece = new HeadPiece();
            ShoesPiece = new ShoesPiece();
            ArmsPiece = new ArmsPiece();
            this.BasicClothing = new List<ClothingPiece>()
            { Hair,ShirtPiece, PantsPiece,HeadPiece,ShoesPiece, ArmsPiece};

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

            this.HairColors = new List<Color>()
            {
                new Color(139,69,19),//saddle brown
                new Color(218,165,32) //goldenrod
            };

        }

        public void ChangeHairColor(CycleDirection direction)
        {
            this.HairIndex += (int)direction;
            if (this.HairIndex < this.HairColors.Count)
            {
               // this.HairIndex++;
            }
            else
            {
                this.HairIndex = 0;
            }

            if (this.HairIndex < 0)
            {
                this.HairIndex = this.HairColors.Count - 1;
            }

            this.Hair.Color = this.HairColors[this.HairIndex];


        }

        public void ChangeSkin(CycleDirection direction)
        {
            this.SkinIndex += (int)direction;
            if(this.SkinIndex < this.SkinColors.Count)
            {
              //  this.SkinIndex++;
            }
            else
            {
                this.SkinIndex = 0;
            }

            if(this.SkinIndex < 0)
            {
                this.SkinIndex = this.SkinColors.Count - 1;
            }

            this.HeadPiece.Color = this.SkinColors[this.SkinIndex];

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

        public void UpdateForCreationMenu()
        {
            this.CurrentAnimationSet.UpdateSourceRectangles();
        }

        public void DrawForCreationMenu(SpriteBatch spriteBatch)
        {
            this.CurrentAnimationSet.DrawForCreationMenu(spriteBatch);
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

        public void SetScale(int scale)
        {
            this.CurrentAnimationSet.SetScale(scale);
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


            Hair.Save(writer);
            HeadPiece.Save(writer);
            ShirtPiece.Save(writer);
            PantsPiece.Save(writer);
            ShoesPiece.Save(writer);

        }

        public void Load(BinaryReader reader)
        {
            Hair.Load(reader);
            HeadPiece.Load(reader);
            ShirtPiece.Load(reader);
            PantsPiece.Load(reader);
            ShoesPiece.Load(reader);


            CycleSwipingClothing();

        }
    }
}

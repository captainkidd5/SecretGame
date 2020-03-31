﻿using Microsoft.Xna.Framework;
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
        public EyePiece EyePiece { get; set; }
        public HeadPiece HeadPiece { get; set; }
        public ShoesPiece ShoesPiece { get; set; }
        public ArmsPiece ArmsPiece { get; set; }


        public List<ClothingPiece> BasicClothing { get; set; }


        public AnimationSet RunSet { get; set; }

        public AnimationSet CurrentAnimationSet { get; set; }
        public Dir CurrentDirection { get; set; }

        public List<Color> SkinColors { get; set; }
        public int SkinColorIndex { get; set; }

        public List<Color> HairColors { get; set; }
        public int HairColorIndex { get; set; }




        public WardrobeNew(GraphicsDevice graphics, Vector2 playerPosition)
        {
            this.Graphics = graphics;


            this.MaxSpriteIndex = 64;
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




            Hair = new HairPiece(this.HairColors[0]);
            ShirtPiece = new ShirtPiece(Color.Red);
            PantsPiece = new PantsPiece(Color.Blue);
            EyePiece = new EyePiece(Color.White);
            HeadPiece = new HeadPiece(this.SkinColors[2]);
            ShoesPiece = new ShoesPiece(Color.Brown);
            ArmsPiece = new ArmsPiece(this.SkinColors[2]);
            this.BasicClothing = new List<ClothingPiece>()
            { Hair,ShirtPiece, PantsPiece,EyePiece,HeadPiece,ShoesPiece, ArmsPiece};

            this.RunSet = new AnimationSet(graphics, this.BasicClothing, 5);

            this.CurrentAnimationSet = this.RunSet;

           

        }

        public void ChangeHairColor(CycleDirection direction)
        {
            this.HairColorIndex += (int)direction;
            if (this.HairColorIndex < this.HairColors.Count)
            {
               // this.HairIndex++;
            }
            else
            {
                this.HairColorIndex = 0;
            }

            if (this.HairColorIndex < 0)
            {
                this.HairColorIndex = this.HairColors.Count - 1;
            }

            this.Hair.Color = this.HairColors[this.HairColorIndex];


        }

        public void ChangeSkin(CycleDirection direction)
        {
            this.SkinColorIndex += (int)direction;
            if(this.SkinColorIndex < this.SkinColors.Count)
            {
              //  this.SkinIndex++;
            }
            else
            {
                this.SkinColorIndex = 0;
            }

            if(this.SkinColorIndex < 0)
            {
                this.SkinColorIndex = this.SkinColors.Count - 1;
            }

            this.HeadPiece.Color = this.SkinColors[this.SkinColorIndex];

            this.ArmsPiece.Color = this.SkinColors[this.SkinColorIndex];
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

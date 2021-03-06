using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    public enum Brightness
    {
        Dark = 75,
        Normal = 100,
        Bright = 105

    }

    public class Wardrobe : Component, ISaveable
    {

        private GraphicsDevice Graphics { get; set; }


        private int MaxSpriteIndex { get; set; }





        public HairPiece Hair { get; set; }

        public ShirtPiece ShirtPiece { get; set; }
        public PantsPiece PantsPiece { get; set; }
        public EyePiece EyePiece { get; set; }
        public HeadPiece HeadPiece { get; set; }
        public ShoesPiece ShoesPiece { get; set; }
        public ArmsPiece ArmsPiece { get; set; }


        public List<ClothingPiece> BasicClothing { get; set; }


        public AnimationSet RunSet { get; set; }
        public AnimationSet ChopSet { get; set; }
        public AnimationSet SwipeSet { get; set; }
        public AnimationSet PickUpItemSet { get; set; }

        public AnimationSet CurrentAnimationSet { get; set; }
        public Dir CurrentDirection { get; set; }

        //SKIN
        public List<Color> SkinColors { get; set; }
        public int SkinColorIndex { get; set; }

        public Color DarkSkinColor { get; set; }
        public Color MediumSkinColor { get; set; }
        public Color LightSkinColor { get; set; }

        public List<Color> ReplacementSkinColors { get; set; }


        public List<Color> HairColors { get; set; }
        public int HairColorIndex { get; set; }

        public Dir CurrentFacedDirection { get; set; }

        public Texture2D EyesAtlas;
        public Texture2D PlayerBaseAtlas;
        public Texture2D ShirtAtlas;
        public Texture2D ShoesAtlas;
        public Texture2D HairAtlas;
        public Texture2D PantsAtlas;
        public Texture2D ArmsAtlas;

        public Texture2D ToolAtlas;

        public Texture2D PlayerBase;
        public Texture2D PlayerHair;
        public Texture2D PlayerShirt;
        public Texture2D PlayerPants;
        public Texture2D PlayerShoes;

        //CHOPPING TOOLS
        public Texture2D ChoppingTestTool;
        public Texture2D IronAxeTool;
        public Texture2D ChoppingToolAtlas;

        public Texture2D ChoppingPlayerBase;
        public Texture2D ChoppingPlayerHair;
        public Texture2D ChoppingPlayerShirt;
        public Texture2D ChoppingPlayerPants;
        public Texture2D ChoppingPlayerShoes;

        public Texture2D SwipingPlayerBase;

        public Texture2D SwipingPlayerShirt;


        public Texture2D PickUpItemBase;
        public Texture2D PickUpItemBlondeHair;
        public Texture2D PickUpItemBluePants;
        public Texture2D PickUpItemRedShirt;
        public Texture2D PickUpItemBrownShoes;

        public Texture2D portalJumpBase;
        public Texture2D portalJumpHair;
        public Texture2D portalJumpPants;
        public Texture2D portalJumpShirt;
        public Texture2D portalJumpShoes;


        public Wardrobe(GraphicsDevice graphics, ContentManager content, Vector2 playerPosition) : base(graphics, content)
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

            EyesAtlas = content.Load<Texture2D>("Player/EyesAtlas");
            HairAtlas = content.Load<Texture2D>("Player/hairAtlas");
            PlayerBaseAtlas = content.Load<Texture2D>("Player/playerBaseAtlas");
            ShirtAtlas = content.Load<Texture2D>("Player/shirtAtlas");
            PantsAtlas = content.Load<Texture2D>("Player/pantsAtlas");
            ShoesAtlas = content.Load<Texture2D>("Player/shoesAtlas");
            ArmsAtlas = content.Load<Texture2D>("Player/armsAtlas");

            ToolAtlas = content.Load<Texture2D>("Player/ToolAtlas");


            PlayerBase = content.Load<Texture2D>("Player/PlayerParts/Base/base");
            PlayerHair = content.Load<Texture2D>("Player/PlayerParts/Hair/playerHair");
            PlayerShirt = content.Load<Texture2D>("Player/PlayerParts/Shirts/shirts");
            PlayerPants = content.Load<Texture2D>("Player/PlayerParts/Pants/pants");
            PlayerShoes = content.Load<Texture2D>("Player/PlayerParts/Shoes/shoes");

            //ANIMATIONS
            //CHOPPING
            //TOOLS
            ChoppingTestTool = content.Load<Texture2D>("Player/PlayerParts/Chopping/Tools/ChoppingTestTool");
            IronAxeTool = content.Load<Texture2D>("Player/PlayerParts/Chopping/Tools/IronAxeTool");
            ChoppingToolAtlas = content.Load<Texture2D>("Player/PlayerParts/Chopping/Tools/ChoppingToolAtlas");

            ChoppingPlayerBase = content.Load<Texture2D>("Player/PlayerParts/Chopping/Base/ChoppingBase");
            ChoppingPlayerHair = content.Load<Texture2D>("Player/PlayerParts/Chopping/Hair/ChoppingBlondeHair");
            ChoppingPlayerPants = content.Load<Texture2D>("Player/PlayerParts/Chopping/Pants/ChoppingPants");
            ChoppingPlayerShirt = content.Load<Texture2D>("Player/PlayerParts/Chopping/Shirts/ChoppingRedShirt");
            ChoppingPlayerShoes = content.Load<Texture2D>("Player/PlayerParts/Chopping/Shoes/ChoppingBrownShoes");


            //SWIPING
            SwipingPlayerBase = content.Load<Texture2D>("Player/PlayerParts/Swiping/Base/swipingBase");

            SwipingPlayerShirt = content.Load<Texture2D>("Player/PlayerParts/Swiping/Shirts/swipingShirts");


            PickUpItemBase = content.Load<Texture2D>("Player/PlayerParts/PickUpItem/Base/PickUpItemBase");
            PickUpItemBlondeHair = content.Load<Texture2D>("Player/PlayerParts/PickUpItem/Hair/PickUpItemBlondeHair");
            PickUpItemBluePants = content.Load<Texture2D>("Player/PlayerParts/PickUpItem/Pants/PickUpItemBluePants");
            PickUpItemRedShirt = content.Load<Texture2D>("Player/PlayerParts/PickUpItem/Shirts/PickUpItemRedShirt");
            PickUpItemBrownShoes = content.Load<Texture2D>("Player/PlayerParts/PickUpItem/Shoes/PickUpItemBrownShoes");

            portalJumpBase = content.Load<Texture2D>("Player/PlayerParts/PortalJump/Base/portalJumpBase");
            portalJumpHair = content.Load<Texture2D>("Player/PlayerParts/PortalJump/Hair/portalJumpHair");
            portalJumpPants = content.Load<Texture2D>("Player/PlayerParts/PortalJump/Pants/portalJumpPants");
            portalJumpShirt = content.Load<Texture2D>("Player/PlayerParts/PortalJump/Shirts/portalJumpShirt");
            portalJumpShoes = content.Load<Texture2D>("Player/PlayerParts/PortalJump/Shoes/portalJumpShoes");


            Hair = new HairPiece(graphics,content, HairAtlas, this.HairColors[0]);
            ShirtPiece = new ShirtPiece(graphics, content, ShirtAtlas, Color.White);
            PantsPiece = new PantsPiece(graphics, content, PantsAtlas, Color.Blue);
            EyePiece = new EyePiece(graphics, content, EyesAtlas, Color.White);
            HeadPiece = new HeadPiece(graphics, content, PlayerBaseAtlas, this.SkinColors[2]);
            ShoesPiece = new ShoesPiece(graphics, content, ShoesAtlas, Color.Brown);
            ArmsPiece = new ArmsPiece(graphics, content, ArmsAtlas, this.SkinColors[2]);
            this.BasicClothing = new List<ClothingPiece>()
            { Hair,ShirtPiece, PantsPiece,EyePiece,HeadPiece,ShoesPiece, ArmsPiece};

            this.RunSet = new AnimationSet("basic",graphics, this.BasicClothing, 5, .115f);
            this.ChopSet = new ChoppingSet("Chopping", graphics, this.BasicClothing, 5, .1f);
            this.SwipeSet = new SwordSwipeSet("Swiping", graphics, this.BasicClothing, 5, .1f);
            this.PickUpItemSet = new PickUpItemSet("PickUpItem", graphics, this.BasicClothing, 3, .115f);

            this.CurrentAnimationSet = this.RunSet;

            this.DarkSkinColor = new Color(89, 86, 82);
            this.MediumSkinColor = new Color(220, 229, 246);
            this.LightSkinColor = new Color(255, 255, 255);

            this.ReplacementSkinColors = new List<Color>()
            {
                DarkSkinColor,
                MediumSkinColor,
                LightSkinColor
            };
            this.ArmsPiece.ChangePartOfTexture(this.SkinColors[2], ReplacementSkinColors);

            


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

            this.ArmsPiece.ChangePartOfTexture(this.SkinColors[this.SkinColorIndex], ReplacementSkinColors);

        }

        public void SetZero()
        {
            this.CurrentAnimationSet.CurrentFrame = 0;
        }

        public void UpdateCurrentDirection(Dir direction)
        {

        }

        public void UpdateAnimations(GameTime gameTime, Vector2 position, Dir direction, bool isMoving)
        {
            if(direction != Dir.None)
            {
                this.CurrentDirection = direction; //direction must be in one of the four directions. 
            }
            this.CurrentAnimationSet.Update(gameTime, position, CurrentDirection, isMoving);

        }

        public bool PlayAnimationOnce(GameTime gameTime, AnimationSet animationSet, Vector2 position, Dir direction)
        {
            if(animationSet.UpdateOnce(gameTime, position, direction))
            {
                Game1.Player.controls.Direction = direction;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float yLayerHeight)
        {
            this.CurrentAnimationSet.Draw(spriteBatch, yLayerHeight);
        }

        public void UpdateForCreationMenu()
        {
            this.CurrentAnimationSet.UpdateSourceRectangles();
        }

        public void DrawForCreationMenu(SpriteBatch spriteBatch)
        {
            this.CurrentAnimationSet.DrawForCreationMenu(spriteBatch);
        }

        public static Color ChangeColorLevel(Color color, int listIndex)
        {
            Brightness brightness = Brightness.Bright;
            switch (listIndex)
            {
                case 0:
                    brightness = Brightness.Dark;
                    break;
                case 1:
                    brightness = Brightness.Normal;
                    break;
                case 2:
                    brightness = Brightness.Bright;
                    break;
            }


            float brightNessValue = (float)brightness / 100;
            int newR = (int)(color.R * brightNessValue);
            int newG = (int)(color.G * brightNessValue);
            int newB = (int)(color.B * brightNessValue);
            return new Color(newR, newG, newB);
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

        public virtual void SaveColor(BinaryWriter writer, Color color)
        {
            writer.Write(color.R);
            writer.Write(color.G);
            writer.Write(color.B);
        }

        public void SaveColorList(BinaryWriter writer, List<Color> colorList)
        {
            writer.Write(colorList.Count);
            for (int i = 0; i < colorList.Count; i++)
            {
                SaveColor(writer, colorList[i]);
            }
        }

        public void LoadColorList(BinaryReader reader, List<Color> colorList)
        {
            int colorCount = reader.ReadInt32();
            colorList.Clear();
            for (int i = 0; i < colorCount; i++)
            {
                colorList.Add(new Color(reader.ReadByte(), reader.ReadByte(), reader.ReadByte()));
            }
        }


        public void Save(BinaryWriter writer)
        {


            Hair.Save(writer);
            HeadPiece.Save(writer);
            EyePiece.Save(writer);
            ShirtPiece.Save(writer);
            PantsPiece.Save(writer);
            ShoesPiece.Save(writer);

           
            writer.Write(this.SkinColorIndex);
            SaveColorList(writer, this.ReplacementSkinColors);
        }

        public void Load(BinaryReader reader)
        {
            Hair.Load(reader);
            HeadPiece.Load(reader);
            EyePiece.Load(reader);
            ShirtPiece.Load(reader);
            PantsPiece.Load(reader);
            ShoesPiece.Load(reader);

            this.SkinColorIndex = reader.ReadInt32();
            this.ArmsPiece.ChangePartOfTexture(this.HeadPiece.Color, ReplacementSkinColors);
            this.ShirtPiece.ChangeArmSleeves();
            LoadColorList(reader, this.ReplacementSkinColors);
            


            CycleSwipingClothing();

        }

        public override void Load()
        {
            throw new NotImplementedException();
        }

        public override void Unload()
        {
            throw new NotImplementedException();
        }
    }
}

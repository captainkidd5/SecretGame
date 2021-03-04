using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SecretProject.Class.Playable
{
    public class PlayerManager : Component, ISaveable
    {

        public Player Player { get; set; }
        private SimpleTimer SimpleTimer { get; set; }

        public List<Player> Players { get; set; }

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

        public PlayerManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            Players = new List<Player>();
            SimpleTimer = new SimpleTimer(Globals.MFrequency);
        }
        public void Update(GameTime gameTime)
        {
            foreach (Player player in Players)
            {
                player.Update(gameTime);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Player player in Players)
            {
                player.Draw(spriteBatch);
            }
        }
        public void Load(BinaryReader reader)
        {
            

            //Player
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

            Player = new Player("NAME", new Vector2(630, 600), PlayerBase, 5, content, graphicsDevice) { Activate = true, IsDrawn = true };
        }

        public void Save(BinaryWriter writer)
        {
            throw new NotImplementedException();
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.QuestFolder;
using SecretProject.Class.RouteStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretProject.Class.NPCStuff
{
    public class CharacterManager : Component
    {
        //NPCS
        public static Character Elixir;
        public static Character Dobbin;
        public static Character Kaya;

        public static Character Snaw;
        public static Character BusinessSnail;
        public static Character Julian;
        public static Character Sarah;
        public static Character Mippin;
        public static Character Ned;
        public static Character Teal;
        public static Character Marcus;
        public static Character Caspar;
        public static List<Character> AllCharacters;


        private DialogueManager DialogueManager { get; set; }

        private RouteManager RouteManager { get; set; }

        private QuestManager QuestManager { get; set; }
        public CharacterManager(GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
            this.DialogueManager = new DialogueManager(graphicsDevice, content);
            this.RouteManager = new RouteManager(graphicsDevice, content);
            this.QuestManager = new QuestManager(graphicsDevice, content);
        }
        private void LoadCharacters()
        {

            Vector2 elixirPosition = Character.GetWorldPosition(new Vector2(25, 12));
            Elixir = new Character("Elixir",graphicsDevice,content, new Vector2(19, 26), content.Load<Texture2D>("NPC/Elixir/ElixirSpriteSheet"), RouteManager.ElixirRouteSchedule, StageManager.ElixirHouse, false, QuestManager.ElixirQuests, content.Load<Texture2D>("NPC/Elixir/elixirPortrait"))
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphicsDevice, Game1.AllTextures.ElixirSpriteSheet, 48, 0, 16, 48, 6, .15f, elixirPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.ElixirSpriteSheet,  144, 0, 28, 48, 6, .15f, elixirPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.ElixirSpriteSheet,  240, 0, 16, 48, 6, .15f, elixirPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.ElixirSpriteSheet,336, 0, 16, 48, 6, .15f, elixirPosition)
                },

                NPCRectangleXOffSet = 8,
                NPCRectangleYOffSet = 34,
                NPCRectangleHeightOffSet = 8,
                NPCRectangleWidthOffSet = 8,
                SpeakerID = 1,


                DebugColor = Color.HotPink,
            };
            Elixir.LoadLaterStuff(graphicsDevice);






            Vector2 dobbinPosition = Character.GetWorldPosition(new Vector2(18, 8));
            Dobbin = new Character("Dobbin", graphicsDevice, content, new Vector2(5, 7), content.Load<Texture2D>("NPC/Dobbin/DobbinSpriteSheet"), RouteManager.DobbinRouteSchedule , StageManager.DobbinHouse, false, QuestManager.DobbinQuests, content.Load<Texture2D>("NPC/Dobbin/dobbinPortrait"))
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphicsDevice, Game1.AllTextures.DobbinSpriteSheet,0, 0, 28, 48, 6, .15f, dobbinPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.DobbinSpriteSheet,  167, 0, 28, 48, 6, .15f, dobbinPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.DobbinSpriteSheet, 335, 0, 28, 48, 6, .15f, dobbinPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.DobbinSpriteSheet,503, 0, 28, 48, 6, .15f, dobbinPosition)
                },

                NPCRectangleXOffSet = 15,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 2,

                DebugColor = Color.HotPink,
            };
            Dobbin.LoadLaterStuff(graphicsDevice);



            Vector2 kayaPosition = Character.GetWorldPosition(new Vector2(20, 19));
            Kaya = new Character("Kaya", graphicsDevice, content, new Vector2(51, 32), content.Load<Texture2D>("NPC/Kaya/KayaSpriteSheet"), RouteManager.KayaRouteSchedule, StageManager.Town, false, QuestManager.KayaQuests, content.Load<Texture2D>("NPC/Kaya/KayaPortrait"))
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphicsDevice, Game1.AllTextures.KayaSpriteSheet, 0, 0, 16, 34, 6, .15f, kayaPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.KayaSpriteSheet, 112, 0, 16, 34, 7, .15f, kayaPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.KayaSpriteSheet, 224, 0, 16, 34, 7, .15f, kayaPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.KayaSpriteSheet, 336, 0, 16, 34, 6, .15f, kayaPosition)
                },

                NPCRectangleXOffSet = 5,
                NPCRectangleYOffSet = 15,
                NPCRectangleHeightOffSet = 12,
                NPCRectangleWidthOffSet = 8,
                SpeakerID = 4,

                DebugColor = Color.HotPink,
            };
            Kaya.LoadLaterStuff(graphicsDevice);

            Snaw = new Character("Snaw", graphicsDevice, content, new Vector2(121, 67), content.Load<Texture2D>("NPC/Snaw/Snaw"),
                3, content.Load<Texture2D>("NPC/Snaw/SnawPortrait"), SnawQuests)
            {
                NPCAnimatedSprite = new Sprite[1] { new Sprite(graphicsDevice, Game1.AllTextures.SnawSpriteSheet,
                0, 0, 72, 96, 3, .3f, new Vector2(1280, 500)) { IsAnimated = true,  } },
                CurrentDirection = 0,
                SpeakerID = 3,
                CurrentStageLocation = Game1.Town,
                FrameToSet = 3,
                IsBasicNPC = true
            };

            Vector2 julianPosition = Character.GetWorldPosition(new Vector2(16, 15));
            Julian = new Character("Julian", graphicsDevice, content, new Vector2(16, 15), content.Load<Texture2D>("NPC/Julian/Julian"), RouteManager.JulianRouteSchedule, StageManager.JulianHouse, false, QuestManager.JulianQuests, content.Load<Texture2D>("NPC/Julian/julianPortrait"))
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphicsDevice, Game1.AllTextures.JulianSpriteSheet, 0, 0, 16, 34, 6, .15f, julianPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.JulianSpriteSheet, 96, 0, 16, 34, 7, .15f, julianPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.JulianSpriteSheet, 208, 0, 16, 34, 7, .15f, julianPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.JulianSpriteSheet, 320, 0, 16, 34, 6, .15f, julianPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 5,

                DebugColor = Color.HotPink,
            };
            Julian.LoadLaterStuff(graphicsDevice);


            Vector2 sarahPosition = Character.GetWorldPosition(new Vector2(56, 28));
            Sarah = new Character("Sarah", graphicsDevice, content, new Vector2(56, 28), content.Load<Texture2D>("NPC/Sarah/Sarah"), RouteManager.SarahRouteSchedule, StageManager.Town, false, QuestManager.SarahQuests, content.Load<Texture2D>("NPC/Sarah/SarahPortrait"))
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphicsDevice, Game1.AllTextures.SarahSpriteSheet, 0, 0, 16, 32, 6, .15f, sarahPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.SarahSpriteSheet, 96, 0, 16, 32, 7, .15f, sarahPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.SarahSpriteSheet, 96, 0, 16, 32, 7, .15f, sarahPosition) { Flip = true },
            new Sprite(graphicsDevice, Game1.AllTextures.SarahSpriteSheet, 208, 0, 16, 32, 6, .15f, sarahPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 6,

                DebugColor = Color.HotPink,
            };
            Sarah.LoadLaterStuff(graphicsDevice);


            BusinessSnail = new Character("Business Snail", graphicsDevice, content, new Vector2(34, 80), content.Load<Texture2D>("NPC/BusinessSnail/BusinessSnail"),
                1, content.Load<Texture2D>("NPC/Sarah/SarahPortrait"), QuestManager.BusinessSnailQuests)
            {
                NPCAnimatedSprite = new Sprite[1] { new Sprite(graphicsDevice, Game1.AllTextures.BusinessSnail,
                0, 0, 32, 32, 1, 1f, new Vector2(1280, 600)) { IsAnimated = true,  } },
                CurrentDirection = 0,
                SpeakerID = 7,
                CurrentStageLocation = StageManager.Town,
                FrameToSet = 0,
                IsBasicNPC = true
            };





            Vector2 mippinPosition = Character.GetWorldPosition(new Vector2(49, 33));
            Mippin = new Character("Mippin", graphicsDevice, content, new Vector2(49, 33), content.Load<Texture2D>("NPC/Mippin/mippin"), RouteManager.MippinRouteSchedule, StageManager.Town, false, QuestManager.MippinQuests, content.Load<Texture2D>("NPC/Mippin/mippinPortrait"))
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphicsDevice, Game1.AllTextures.Mippin, 0, 0, 16, 32, 6, .15f, mippinPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.Mippin, 96, 0, 16, 32, 7, .15f, mippinPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.Mippin, 96, 0, 16, 32, 7, .15f, mippinPosition) { Flip = true },
            new Sprite(graphicsDevice, Game1.AllTextures.Mippin, 208, 0, 16, 32, 6, .15f, mippinPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 8,

                DebugColor = Color.HotPink,
            };
            Mippin.LoadLaterStuff(graphicsDevice);


            Vector2 nedPosition = Character.GetWorldPosition(new Vector2(52, 15));
            Ned = new Character("Ned", graphicsDevice, content, new Vector2(52, 15), content.Load<Texture2D>("NPC/Ned/Ned"), RouteManager.NedRouteSchedule, StageManager.Town, false, QuestManager.NedQuests, content.Load<Texture2D>("NPC/Ned/NedPortrait"))
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphicsDevice, Game1.AllTextures.Ned, 0, 0, 16, 32, 6, .15f, nedPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.Ned, 96, 0, 16, 32, 7, .15f, nedPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.Ned, 96, 0, 16, 32, 7, .15f, nedPosition) { Flip = true },
            new Sprite(graphicsDevice, Game1.AllTextures.Ned, 208, 0, 16, 32, 6, .15f, nedPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 9,

                DebugColor = Color.HotPink,
            };
            Ned.LoadLaterStuff(graphicsDevice);



            Vector2 tealPosition = Character.GetWorldPosition(new Vector2(55, 17));
            Teal = new Character("Teal", graphicsDevice, content, new Vector2(55, 17), content.Load<Texture2D>("NPC/Teal/Teal"), RouteManager.TealRouteSchedule, StageManager.Town, false, QuestManager.TealQuests, content.Load<Texture2D>("NPC/Teal/TealPortrait"))
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphicsDevice, Game1.AllTextures.Teal, 0, 0, 16, 32, 6, .15f, tealPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.Teal, 96, 0, 16, 32, 7, .15f, tealPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.Teal, 96, 0, 16, 32, 7, .15f, tealPosition) { Flip = true },
            new Sprite(graphicsDevice, Game1.AllTextures.Teal, 208, 0, 16, 32, 6, .15f, tealPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 10,

                DebugColor = Color.HotPink,
            };
            Teal.LoadLaterStuff(graphicsDevice);


            Vector2 marcusPosition = Character.GetWorldPosition(new Vector2(16, 26));
            Marcus = new Character("Marcus", graphicsDevice, content, new Vector2(16, 26), content.Load<Texture2D>("NPC/Marcus/Marcus"), RouteManager.MarcusRouteSchedule, StageManager.MarcusHouse, false, QuestManager.MarcusQuests, content.Load<Texture2D>("NPC/Marcus/MarcusPortrait"))
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphicsDevice, Game1.AllTextures.Marcus, 0, 0, 16, 32, 6, .15f, marcusPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.Marcus, 96, 0, 16, 32, 7, .15f, marcusPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.Marcus, 96, 0, 16, 32, 7, .15f, marcusPosition) { Flip = true },
            new Sprite(graphicsDevice, Game1.AllTextures.Marcus, 208, 0, 16, 32, 6, .15f, marcusPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 11,
                // NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);

                DebugColor = Color.HotPink,
            };
            Marcus.LoadLaterStuff(graphicsDevice);


            Vector2 casparPosition = Character.GetWorldPosition(new Vector2(16, 14));
            Caspar = new Character("Caspar", graphicsDevice, content, new Vector2(16, 14), content.Load<Texture2D>("NPC/Caspar/Caspar"), RouteManager.CasparRouteSchedule, StageManager.CasparHouse, false, QuestManager.CasparQuests, content.Load<Texture2D>("NPC/Caspar/CasparPortrait"))
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphicsDevice, Game1.AllTextures.Caspar, 0, 0, 16, 32, 6, .15f, casparPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.Caspar, 96, 0, 16, 32, 7, .15f, casparPosition),
            new Sprite(graphicsDevice, Game1.AllTextures.Caspar, 96, 0, 16, 32, 7, .15f, casparPosition) { Flip = true },
            new Sprite(graphicsDevice, Game1.AllTextures.Caspar, 208, 0, 16, 32, 6, .15f, casparPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 12,
                // NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);

                DebugColor = Color.HotPink,
            };
            Caspar.LoadLaterStuff(graphicsDevice);

            AllCharacters = new List<Character>()
            {
                Elixir,
                Dobbin,
                Kaya,
                Snaw,
                Julian,
                Sarah,
                BusinessSnail,
                Mippin,
                Ned,
                Teal,
                Marcus,
                Caspar
            };

            foreach (Stage stage in StageManager.AllStages)
            {
                foreach (Character character in AllCharacters)
                {
                    if (character.CurrentStageLocation == CurrentStage)
                    {
                        stage.CharactersPresent.Add(character);
                    }
                }

            }


        }

        private static void LoadCharacterBodies(Stage oldStage, Stage newStage)
        {

            for (int i = 0; i < AllCharacters.Count; i++)
            {
                AllCharacters[i].ProcessPlayerChangeStage(oldStage, newStage);
                AllCharacters[i].InRangeOfPlayer = false;
            }

        }

        public void Update(GameTime gameTime)
        {
            if (Game1.Flags.UpdateCharacters)
            {
                foreach (Character character in AllCharacters)
                {
                    character.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Character character in AllCharacters)
            {
                character.Draw(spriteBatch);
            }
        }
        public override void Load()
        {
            DialogueManager.Load();
            RouteManager.Load();
            LoadCharacters();
           
        }

        public override void Unload()
        {
            throw new NotImplementedException();
        }
    }
}

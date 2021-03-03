using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.StageFolder;
using System.Collections.Generic;

namespace SecretProject.Class.SoundStuff
{


    public class SoundBoard
    {
        //Change this to enable sound
        public float GameVolume { get;  set; } = .25f;
        //Sound Effects are WAV
        //Songs are MP3
        public SoundEffect PickUpItem { get; private set; }

        public SoundEffect StoneStep { get; private set; }


        public SoundEffect GrassBreak {get;set;}

        public SoundEffect PlaceBarrel { get; private set; }

        public SoundEffect DoorOpen { get; private set; }

        public SoundEffect DigDirt { get; private set; }

        public SoundEffect StoneSmash { get; private set; }
        public SoundEffect MiningHit { get; private set; }


        public SoundEffect TreeFall { get; private set; }

        public SoundEffect WalkGrass { get; private set; }


        public SoundEffect WalkSand { get; private set; }


        public SoundEffect WalkWood { get; private set; }


        public SoundEffect WalkStone { get; private set; }
        public SoundEffect CrunchStep { get; private set; }

        public SoundEffect Chirp1 { get; private set; }

        public SoundEffect Chirp2 { get; private set; }

        public SoundEffect Chirp3 { get; private set; }

        public SoundEffect FoodBite { get; private set; }
        public SoundEffect GrassCut { get; private set; }

        //GADGETS
        public SoundEffect PotLidOpen { get; private set; }
        public SoundEffect PotLidClose { get; private set; }

        //AMBIENT
        public SoundEffect Crickets1 { get; private set; }



        public SoundEffect OwlHoot1 { get; private set; }
        public SoundEffect LightRain { get; private set; }
        public SoundEffectInstance LightRainInstance { get; private set; }

        public SoundEffect SunnySounds { get; private set; }
        public SoundEffectInstance SunnySoundsInstance { get; private set; }


        public SoundEffect PigGrunt { get; private set; }


        public SoundEffect PigGrunt2 { get; private set; }

        public SoundEffect ChickenCluck1 { get; private set; }

        public SoundEffect DogBark { get; private set; }
        public SoundEffect ToadCroak { get; private set; }
        public SoundEffect GoatBleat { get; private set; }

        public SoundEffect CropPluck { get; private set; }


        public SoundEffect TextNoise { get; private set; }
        public SoundEffect TextNoise2 { get; private set; }

        public SoundEffect CraftMetal { get; private set; }
        public SoundEffect ToolBreak { get; private set; }

        public SoundEffect Sell1 { get; private set; }

        public SoundEffect SanctuaryAdd { get; private set; }

        public SoundEffect GearSpin { get; private set; }

        public SoundEffect PlaceItem1 { get; private set; }


        public SoundEffect FurnaceLight { get; private set; }
        public SoundEffect FurnaceFire { get; private set; }
        public SoundEffect FurnaceDone { get; private set; }
        public SoundEffect UnlockItem { get; private set; }

        public SoundEffect ChestOpen { get; private set; }

        public SoundEffect PumpkinSmash { get; private set; }


        public SoundEffect CoinGet { get; private set; }
        public SoundEffect MiniReward { get; private set; }

        public SoundEffect BoneRattle1 { get; private set; }
        public SoundEffect BoneRattle2 { get; private set; }

        //COMBAT
        public SoundEffect Slash1 { get; private set; }
        public SoundEffect SwordSwing { get; private set; }
        public SoundEffect SwordImpact { get; private set; }
        public SoundEffect BushCut { get; private set; }

        //BOW AND ARROW
        public SoundEffect BowShoot { get; private set; }
        public SoundEffect ArrowMiss { get; private set; }

        public SoundEffect SporeShoot { get; private set; }
        public SoundEffect SlimeHit { get; private set; }

        public SoundEffect Thunder1 { get; private set; }

        //songs
        public SoundEffect DustStorm { get; private set; }
        public SoundEffect Lakescape { get; private set; }
        public SoundEffect Title { get; private set; }
        public SoundEffect MelodyOfTheSea { get; private set; }
        public SoundEffect DeeperAndDeeper { get; private set; }
        public SoundEffect ForestersTheme { get; private set; }
        public SoundEffect LighthouseTheme { get; private set; }

        //Event songs
        //Intro Scene
        public SoundEffect Downpour { get; private set; }

        public SoundEffect CurrentSong { get; private set; }
        public SoundEffectInstance CurrentSongInstance { get; private set; }

        //Emoticons
        public SoundEffect Exclamation { get; private set; }

        //UI
        public SoundEffect UIClick { get; private set; }
        public SoundEffect Alert1 { get; private set; }
        public SoundEffect PageRuffleOpen;
        public SoundEffect PageRuffleClose;

        public SongChooser TitleSongs { get; private set; }
        public SongChooser WorldSongs { get; private set; }
        public SongChooser UnRaiSongs { get; private set; }
        public SongChooser InteriorSongs { get; private set; }

        public Dictionary<int,SoundEffect>  SoundEffectDictionary { get; private set; }

        public SoundBoard(Game1 game, ContentManager content)
        {
            //this.PickUpItem = content.Load<SoundEffect>("SoundEffects/bubble");


            //GrassBreak = content.Load<SoundEffect>("SoundEffects/grassBreakWav");


            //PlaceBarrel = content.Load<SoundEffect>("SoundEffects/placeBarrel");


           


            DigDirt = content.Load<SoundEffect>("SoundEffects/diggingDirt");


            StoneSmash = content.Load<SoundEffect>("SoundEffects/stoneSmash");
            MiningHit = content.Load<SoundEffect>("SoundEffects/miningHit");

            TreeFall = content.Load<SoundEffect>("SoundEffects/treeFall");

            WalkGrass = content.Load<SoundEffect>("SoundEffects/walkGrass");


            WalkSand = content.Load<SoundEffect>("SoundEffects/sandStep");


            WalkWood = content.Load<SoundEffect>("SoundEffects/woodStep");


            WalkStone = content.Load<SoundEffect>("SoundEffects/stoneStep");
            CrunchStep = content.Load<SoundEffect>("SoundEffects/CrunchStep");


            Chirp1 = content.Load<SoundEffect>("SoundEffects/chirp1");


            Chirp2 = content.Load<SoundEffect>("SoundEffects/chirp2");


            Chirp3 = content.Load<SoundEffect>("SoundEffects/chirp3");

            FoodBite = content.Load<SoundEffect>("SoundEffects/foodBite");
            GrassCut = content.Load<SoundEffect>("SoundEffects/grasscut");

            //GADGETS
            PotLidClose = content.Load<SoundEffect>("SoundEffects/PotLidClose");
            PotLidOpen = content.Load<SoundEffect>("SoundEffects/PotLidOpen");

            Crickets1 = content.Load<SoundEffect>("SoundEffects/crickets1");
            LightRain = content.Load<SoundEffect>("SoundEffects/rainSound");
            LightRainInstance = LightRain.CreateInstance();
            SunnySounds = content.Load<SoundEffect>("SoundEffects/DayTimeAmbience");
            SunnySoundsInstance = SunnySounds.CreateInstance();


            OwlHoot1 = content.Load<SoundEffect>("SoundEffects/owlHoot1");


            PigGrunt = content.Load<SoundEffect>("SoundEffects/pigGrunt");


            PigGrunt2 = content.Load<SoundEffect>("SoundEffects/PigGrunt2");
            ChickenCluck1 = content.Load<SoundEffect>("SoundEffects/ChickenCluck");

            DogBark = content.Load<SoundEffect>("SoundEffects/dogbarking");
            ToadCroak = content.Load<SoundEffect>("SoundEffects/toadCroak");
            GoatBleat = content.Load<SoundEffect>("SoundEffects/goatBleat");

            CropPluck = content.Load<SoundEffect>("SoundEffects/ropePop");

            PumpkinSmash = content.Load<SoundEffect>("SoundEffects/pumpkinSquash");
            TextNoise = content.Load<SoundEffect>("SoundEffects/textNoise");
            TextNoise2 = content.Load<SoundEffect>("SoundEffects/textNoise2");

            CraftMetal = content.Load<SoundEffect>("SoundEffects/metalCraft");
            ToolBreak = content.Load<SoundEffect>("SoundEffects/toolBreak");
            Sell1 = content.Load<SoundEffect>("SoundEffects/sell1");
            SanctuaryAdd = content.Load<SoundEffect>("SoundEffects/sanctuaryAddEffect");

            GearSpin = content.Load<SoundEffect>("SoundEffects/gearspin");

            PlaceItem1 = content.Load<SoundEffect>("SoundEffects/placeItem1");


            FurnaceLight = content.Load<SoundEffect>("SoundEffects/FurnaceLight");
            FurnaceFire = content.Load<SoundEffect>("SoundEffects/furnaceFire");
            FurnaceDone = content.Load<SoundEffect>("SoundEffects/furnacedone");

            UnlockItem = content.Load<SoundEffect>("SoundEffects/unlockitem");
            ChestOpen = content.Load<SoundEffect>("SoundEffects/ChestOpen");
            Thunder1 = content.Load<SoundEffect>("SoundEffects/Thunder1");

            CoinGet = content.Load<SoundEffect>("SoundEffects/CoinGet");
            MiniReward = content.Load<SoundEffect>("SoundEffects/MiniReward");

            BoneRattle1 = content.Load<SoundEffect>("SoundEffects/boneRattle1");
            BoneRattle2 = content.Load<SoundEffect>("SoundEffects/boneRattle2");

            //COMBAT
            Slash1 = content.Load<SoundEffect>("SoundEffects/Slash1");
            SwordSwing = content.Load<SoundEffect>("SoundEffects/swordSwing");
            SwordImpact = content.Load<SoundEffect>("SoundEffects/swordImpact");
            BushCut = content.Load<SoundEffect>("SoundEffects/bushCut");


            //BOW AND ARROW
            BowShoot = content.Load<SoundEffect>("SoundEffects/bowDraw");
            ArrowMiss = content.Load<SoundEffect>("SoundEffects/arrowMiss");
            SlimeHit = content.Load<SoundEffect>("SoundEffects/SlimeHit");
            SporeShoot = content.Load<SoundEffect>("SoundEffects/SporeShoot");

            //Songs
            DustStorm = content.Load<SoundEffect>("Songs/DustStorm");

            Title = content.Load<SoundEffect>("Songs/Title");
            Lakescape = content.Load<SoundEffect>("Songs/Lakescape");

            Downpour = content.Load<SoundEffect>("Songs/Downpour");
            MelodyOfTheSea = content.Load<SoundEffect>("Songs/MelodyOfTheSea");
            DeeperAndDeeper = content.Load<SoundEffect>("Songs/DeeperAndDeeper");

            ForestersTheme = content.Load<SoundEffect>("Songs/ForestersTheme");
            LighthouseTheme = content.Load<SoundEffect>("Songs/LighthouseTheme");
            this.CurrentSong = Title;
            this.CurrentSongInstance = this.CurrentSong.CreateInstance();

            //Emoticons
            this.Exclamation = content.Load<SoundEffect>("SoundEffects/exclamation");

            //UI
            UIClick = content.Load<SoundEffect>("SoundEffects/UIClick");
            Alert1 = content.Load<SoundEffect>("SoundEffects/Alert1");
            PageRuffleOpen = content.Load<SoundEffect>("SoundEffects/paperRuffleOpen");
            PageRuffleClose = content.Load<SoundEffect>("SoundEffects/paperRuffleClose");
            DoorOpen = ChestOpen;
            this.SoundEffectDictionary = new Dictionary<int, SoundEffect>()
            {
                {1, WalkGrass },
                {2, WalkStone},
                {3,WalkWood },
                {4, CrunchStep},
                {5, PickUpItem},
                {6, GrassBreak},
                {7, DigDirt},
                {8, MiningHit},
                {9, Chirp1},
                {10,Chirp2 },
                {11,Chirp3 },
                {12, Crickets1},
                {13, OwlHoot1},
                {14, PigGrunt},
                {15, PigGrunt2},
                {16, CropPluck},
                {17, PumpkinSmash},
                {18, WalkSand},
                {19, TreeFall},
                {20, StoneSmash},
                {21, BoneRattle1},

            };



            this.TitleSongs = new SongChooser(new List<SoundEffect>()
            {
                ForestersTheme,
                Title

            },1);


            this.WorldSongs = new SongChooser(new List<SoundEffect>()
            {
               MelodyOfTheSea,
                ForestersTheme,
                Title,
                Lakescape,
                DeeperAndDeeper,
                LighthouseTheme

            }, 3);

            this.UnRaiSongs = new SongChooser(new List<SoundEffect>()
            {
                DeeperAndDeeper,
                

            },1);

            this.InteriorSongs = new SongChooser(new List<SoundEffect>()
            {
                MelodyOfTheSea, LighthouseTheme

            },1);
        }
        public void PlaySong()
        {
            if (this.CurrentSongInstance.State == SoundState.Stopped)
            {
                this.CurrentSongInstance = FetchNewSong().CreateInstance();
                this.CurrentSongInstance.Volume = this.GameVolume * .4f;
                this.CurrentSongInstance.Play();
            }

        }




        public SoundEffect FetchNewSong()
        {
            if(StageManager.CurrentStage == Game1.mainMenu)
            {
                return TitleSongs.FetchSong();
            }
            else
            {
                return InteriorSongs.FetchSong();
            }
        }

        public bool FadeSongOut(GameTime gameTime)
        {
            if(this.CurrentSongInstance.Volume > 0)
            {
                float currentVolume = this.CurrentSongInstance.Volume - (float)gameTime.ElapsedGameTime.TotalSeconds * .25f;
                if(currentVolume < 0)
                {
                    currentVolume = 0;
                }
                this.CurrentSongInstance.Volume = currentVolume;
                return true;
            }
            Game1.IsFadingIn = true;
            this.CurrentSongInstance.Stop();
            PlaySong();
            return false;
            
        }
        public bool FadeSongIn(GameTime gameTime)
        {
            if(this.CurrentSongInstance.Volume < this.GameVolume)
            {
                float currentVolume = this.CurrentSongInstance.Volume + (float)gameTime.ElapsedGameTime.TotalSeconds * .25f;
                if(currentVolume > 1)
                {
                    currentVolume = 1;
                }
                this.CurrentSongInstance.Volume = currentVolume;
                return true;
            }
            return false;
        }

        public void PlayOpenUI()
        {

            UIClick.Play(this.GameVolume, 1f, 1f);
        }
        public void PlayCloseUI()
        {

            UIClick.Play(this.GameVolume, .5f, 1f);
        }

        public void PlaySoundEffect(SoundEffect soundEffect, bool randomizePitch = false, float pitchHighCap = 1f, float pitchLowCap = 0f)
        {
            //SoundEffectInstance instance = soundEffect.CreateInstance();
            //instance.Volume = volume;
            //instance.Play();
            //instance.Dispose();
            float pitch = 0f;
            if (randomizePitch)
            {
                pitch = Game1.Utility.RFloat(0, pitchHighCap);
            }
           // soundEffect.Play(this.GameVolume, pitch, 1f);
        }

        public void PlaySoundWithRadius(Vector2 entityPosition, SoundEffect soundEffect, bool randomizePitch = false, float pitchCap = 1f)
        {

        }

        public void PlaySoundEffectOnce(SoundEffectInstance soundEffect, LocationType locationType)
        {
            if (StageManager.CurrentStage != null)
            {


                if (StageManager.CurrentStage.LocationType == locationType)
                {
                    if (soundEffect.State == SoundState.Stopped)
                    {
                        soundEffect.Volume = this.GameVolume;

                        soundEffect.Play();
                    }
                }
                else
                {
                    soundEffect.Stop();
                }
            }
        }

        public void PlayEmoticonSound(EmoticonType emoticonType)
        {
            switch (emoticonType)
            {
                case EmoticonType.Exclamation:
                    PlaySoundEffect(this.Exclamation, true);
                    break;
            }

        }

        

        public void PlaySoundEffectFromInt(int numberOfLoops, int soundKey)
        {
            if(soundKey != 0)
            {
                for (int i = 0; i < numberOfLoops; i++)
                {
                    PlaySoundEffect(this.SoundEffectDictionary[soundKey], true, .2f);

                }
            }
            

        }
    }
}

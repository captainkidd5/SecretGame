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
        public float GameVolume { get; set; } = 1f;
        //Sound Effects are WAV
        //Songs are MP3
        public SoundEffect PickUpItem { get; set; }

        public SoundEffect StoneStep { get; set; }


        public SoundEffect GrassBreak;

        public SoundEffect PlaceBarrel;

        public SoundEffect DoorOpen;

        public SoundEffect DigDirt;

        public SoundEffect StoneSmash;
        public SoundEffect MiningHit;


        public SoundEffect TreeFall;

        public SoundEffect WalkGrass;


        public SoundEffect WalkSand;


        public SoundEffect WalkWood;


        public SoundEffect WalkStone;
        public SoundEffect CrunchStep;

        public SoundEffect Chirp1;

        public SoundEffect Chirp2;

        public SoundEffect Chirp3;

        public SoundEffect FoodBite;
        public SoundEffect GrassCut;

        //GADGETS
        public SoundEffect PotLidOpen;
        public SoundEffect PotLidClose;

        //AMBIENT
        public SoundEffect Crickets1;



        public SoundEffect OwlHoot1;
        public SoundEffect LightRain;
        public SoundEffectInstance LightRainInstance;

        public SoundEffect SunnySounds;
        public SoundEffectInstance SunnySoundsInstance;


        public SoundEffect PigGrunt;


        public SoundEffect PigGrunt2;

        public SoundEffect ChickenCluck1;

        public SoundEffect RabbitWeet;
        public SoundEffect DogBark;
        public SoundEffect ToadCroak;

        public SoundEffect CropPluck;


        public SoundEffect TextNoise;
        public SoundEffect TextNoise2;

        public SoundEffect CraftMetal;
        public SoundEffect ToolBreak;

        public SoundEffect Sell1;

        public SoundEffect SanctuaryAdd;

        public SoundEffect GearSpin;

        public SoundEffect PlaceItem1;


        public SoundEffect FurnaceLight;
        public SoundEffect FurnaceFire;
        public SoundEffect FurnaceDone;
        public SoundEffect UnlockItem;

        public SoundEffect PumpkinSmash;


        public SoundEffect CoinGet;
        public SoundEffect MiniReward;

        //COMBAT
        public SoundEffect Slash1;
        public SoundEffect SwordSwing;
        public SoundEffect SwordImpact;
        public SoundEffect BushCut;

        //BOW AND ARROW
        public SoundEffect BowShoot;
        public SoundEffect ArrowMiss;

        public SoundEffect SporeShoot;
        public SoundEffect SlimeHit;
        
        public SoundEffect Thunder1;

        //songs
        public SoundEffect DustStorm;
        public SoundEffect Lakescape;
        public SoundEffect Title;
        public SoundEffect MelodyOfTheSea;
        public SoundEffect DeeperAndDeeper;
        public SoundEffect ForestersTheme;

        //Event songs
        //Intro Scene
        public SoundEffect Downpour;

        public SoundEffect CurrentSong { get; set; }
        public SoundEffectInstance CurrentSongInstance { get; set; }

        //Emoticons
        public SoundEffect Exclamation { get; set; }

        //UI
        public SoundEffect UIClick;
        public SoundEffect Alert1;
        public SoundEffect PageRuffleOpen;
        public SoundEffect PageRuffleClose;

        public SongChooser TitleSongs { get; set; }
        public SongChooser TownSongs { get; set; }
        public SongChooser WorldSongs { get; set; }
        public SongChooser UnRaiSongs { get; set; }
        public SongChooser InteriorSongs { get; set; }

        public SoundBoard(Game1 game, ContentManager content)
        {
            this.PickUpItem = content.Load<SoundEffect>("SoundEffects/bubble");


            GrassBreak = content.Load<SoundEffect>("SoundEffects/grassBreakWav");


            PlaceBarrel = content.Load<SoundEffect>("SoundEffects/placeBarrel");


            DoorOpen = content.Load<SoundEffect>("SoundEffects/doorOpen");


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
            RabbitWeet = content.Load<SoundEffect>("SoundEffects/RabbitWeet");
            DogBark = content.Load<SoundEffect>("SoundEffects/dogbarking");
            ToadCroak = content.Load<SoundEffect>("SoundEffects/toadCroak");

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
            Thunder1 = content.Load<SoundEffect>("SoundEffects/Thunder1");

            CoinGet = content.Load<SoundEffect>("SoundEffects/CoinGet");
            MiniReward = content.Load<SoundEffect>("SoundEffects/MiniReward");

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

            this.CurrentSong = Title;
            this.CurrentSongInstance = this.CurrentSong.CreateInstance();

            //Emoticons
            this.Exclamation = content.Load<SoundEffect>("SoundEffects/exclamation");

            //UI
            UIClick = content.Load<SoundEffect>("SoundEffects/UIClick");
            Alert1 = content.Load<SoundEffect>("SoundEffects/Alert1");
            PageRuffleOpen = content.Load<SoundEffect>("SoundEffects/paperRuffleOpen");
            PageRuffleClose = content.Load<SoundEffect>("SoundEffects/paperRuffleClose");

            this.TitleSongs = new SongChooser(new List<SoundEffect>()
            {
                MelodyOfTheSea,
                ForestersTheme,
                Title,
                Lakescape

            });

            this.TownSongs = new SongChooser(new List<SoundEffect>()
            {
                MelodyOfTheSea,
                ForestersTheme,
                Title,
                Lakescape

            });

            this.WorldSongs = new SongChooser(new List<SoundEffect>()
            {
                DustStorm,
                ForestersTheme,

            });

            this.UnRaiSongs = new SongChooser(new List<SoundEffect>()
            {
                DeeperAndDeeper,

            });

            this.InteriorSongs = new SongChooser(new List<SoundEffect>()
            {
                MelodyOfTheSea,

            });
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
            switch (Game1.gameStages)
            {
                case Stages.MainMenu:
                    return TitleSongs.FetchSong();
                case Stages.OverWorld:

                    return WorldSongs.FetchSong();
                case Stages.Town:
                    return TownSongs.FetchSong();
                case Stages.UnderWorld:
                    return UnRaiSongs.FetchSong();
                default:
                    return InteriorSongs.FetchSong();

            }

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
            soundEffect.Play(this.GameVolume, pitch, 1f);
        }

        public void PlaySoundWithRadius(Vector2 entityPosition, SoundEffect soundEffect, bool randomizePitch = false, float pitchCap = 1f)
        {

        }

        public void PlaySoundEffectOnce(SoundEffectInstance soundEffect, LocationType locationType)
        {
            if (Game1.GetCurrentStage() != null)
            {


                if (Game1.GetCurrentStage().LocationType == locationType)
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



            for (int i = 0; i < numberOfLoops; i++)
            {
                switch (soundKey)
                {


                    case 1:

                        PlaySoundEffect(WalkGrass, true, .2f);
                        break;
                    case 2:
                        PlaySoundEffect(WalkStone, true, .2f);
                        break;
                    case 3:

                        PlaySoundEffect(WalkWood, true, .2f);
                        break;
                    case 4:
                        PlaySoundEffect(CrunchStep, true, .2f);
                        break;
                    case 5:

                        PlaySoundEffect(this.PickUpItem, true, .2f);
                        break;
                    case 6:

                        PlaySoundEffect(GrassBreak, true, .2f);
                        break;
                    case 7:

                        PlaySoundEffect(DigDirt);
                        break;
                    case 8:

                        PlaySoundEffect(MiningHit, true, .2f);
                        break;
                    //day time sound effects
                    case 9:
                        PlaySoundEffect(Chirp1, true);
                        break;
                    case 10:
                        PlaySoundEffect(Chirp2, true);
                        break;
                    case 11:
                        PlaySoundEffect(Chirp3, true);
                        break;
                    case 12:
                        PlaySoundEffect(Crickets1);
                        break;
                    case 13:
                        PlaySoundEffect(OwlHoot1);
                        break;

                    case 14:
                        PlaySoundEffect(PigGrunt, true, .2f);
                        break;
                    case 15:
                        PlaySoundEffect(PigGrunt2);
                        break;
                    case 16:
                        PlaySoundEffect(CropPluck, true, .2f);
                        break;
                    case 17:
                        PlaySoundEffect(PumpkinSmash);
                        break;

                    case 18:
                        PlaySoundEffect(WalkSand);
                        break;

                    case 19:
                        PlaySoundEffect(TreeFall, true, .2f);
                        break;


                }
            }



        }
    }
}

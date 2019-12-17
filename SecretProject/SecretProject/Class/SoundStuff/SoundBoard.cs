using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.StageFolder;

namespace SecretProject.Class.SoundStuff
{
    public class SoundBoard
    {
        //Change this to enable sound
        public float GameVolume { get; set; } = 0f;
        //Sound Effects are WAV
        //Songs are MP3
        public SoundEffect PickUpItem { get; set; }

        public SoundEffect StoneStep { get; set; }


        public SoundEffect GrassBreak;

        public SoundEffect PlaceBarrel;

        public SoundEffect DoorOpen;

        public SoundEffect DigDirt;

        public SoundEffect StoneSmash;


        public SoundEffect WalkGrass;


        public SoundEffect WalkSand;


        public SoundEffect WalkWood;


        public SoundEffect WalkStone;


        public SoundEffect Chirp1;

        public SoundEffect Chirp2;

        public SoundEffect Chirp3;

        //AMBIENT
        public SoundEffect Crickets1;


        public SoundEffect OwlHoot1;
        public SoundEffect LightRain;
        public SoundEffectInstance LightRainInstance;

        public SoundEffect SunnySounds;
        public SoundEffectInstance SunnySoundsInstance;


        public SoundEffect PigGrunt;


        public SoundEffect PigGrunt2;


        public SoundEffect CropPluck;


        public SoundEffect TextNoise;
        public SoundEffect TextNoise2;

        public SoundEffect CraftMetal;
        public SoundEffect ToolBreak;

        public SoundEffect Sell1;

        public SoundEffect SanctuaryAdd;

        public SoundEffect GearSpin;

        public SoundEffect PlaceItem1;
        public SoundEffect PlaceItem2;

        public SoundEffect FurnaceLight;

        public SoundEffect UnlockItem;

        public SoundEffect PumpkinSmash;


        //COMBAT
        public SoundEffect Slash1;
        public SoundEffect BushCut;

        //songs
        public SoundEffect DustStorm;
        public SoundEffect Lakescape;
        public SoundEffect Title;

        public SoundEffect CurrentSong { get; set; }
        public SoundEffectInstance CurrentSongInstance { get; set; }


        public SoundBoard(Game1 game, ContentManager content)
        {
            PickUpItem = content.Load<SoundEffect>("SoundEffects/bubble");


            GrassBreak = content.Load<SoundEffect>("SoundEffects/grassBreakWav");


            PlaceBarrel = content.Load<SoundEffect>("SoundEffects/placeBarrel");


            DoorOpen = content.Load<SoundEffect>("SoundEffects/doorOpen");


            DigDirt = content.Load<SoundEffect>("SoundEffects/diggingDirt");


            StoneSmash = content.Load<SoundEffect>("SoundEffects/stoneSmash");


            WalkGrass = content.Load<SoundEffect>("SoundEffects/walkGrass");


            WalkSand = content.Load<SoundEffect>("SoundEffects/sandStep");


            WalkWood = content.Load<SoundEffect>("SoundEffects/woodStep");


            WalkStone = content.Load<SoundEffect>("SoundEffects/stoneStep");


            Chirp1 = content.Load<SoundEffect>("SoundEffects/chirp1");


            Chirp2 = content.Load<SoundEffect>("SoundEffects/chirp2");


            Chirp3 = content.Load<SoundEffect>("SoundEffects/chirp3");


            Crickets1 = content.Load<SoundEffect>("SoundEffects/crickets1");
            LightRain = content.Load<SoundEffect>("SoundEffects/rainSound");
            LightRainInstance = LightRain.CreateInstance();
            SunnySounds = content.Load<SoundEffect>("SoundEffects/DayTimeAmbience");
            SunnySoundsInstance = SunnySounds.CreateInstance();


            OwlHoot1 = content.Load<SoundEffect>("SoundEffects/owlHoot1");


            PigGrunt = content.Load<SoundEffect>("SoundEffects/pigGrunt");


            PigGrunt2 = content.Load<SoundEffect>("SoundEffects/PigGrunt2");


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
            PlaceItem2 = content.Load<SoundEffect>("SoundEffects/placeItem2");

            FurnaceLight = content.Load<SoundEffect>("SoundEffects/FurnaceLight");

            UnlockItem = content.Load<SoundEffect>("SoundEffects/unlockitem");

            //COMBAT
            Slash1 = content.Load<SoundEffect>("SoundEffects/Slash1");
            BushCut = content.Load<SoundEffect>("SoundEffects/bushCut");

            //Songs
            DustStorm = content.Load<SoundEffect>("Songs/DustStorm");

            Title = content.Load<SoundEffect>("Songs/Title");
            Lakescape = content.Load<SoundEffect>("Songs/Lakescape");

            this.CurrentSong = Title;
            this.CurrentSongInstance = CurrentSong.CreateInstance();

        }
        public void PlaySong()
        {
            if (this.CurrentSongInstance.State == SoundState.Stopped)
            {
                this.CurrentSongInstance = FetchNewSong().CreateInstance() ;
                this.CurrentSongInstance.Volume = GameVolume * .4f;
                this.CurrentSongInstance.Play();
            }

        }

        public SoundEffect FetchNewSong()
        {
            switch (Game1.gameStages)
            {
                case Stages.MainMenu:
                    return Title;
                case Stages.OverWorld:
                
                    return DustStorm;
                case Stages.Town:
                    return Lakescape;
                default:
                    return Title;

            }

        }


        public void PlaySoundEffectInstance(SoundEffect soundEffect)
        {
            //SoundEffectInstance instance = soundEffect.CreateInstance();
            //instance.Volume = volume;
            //instance.Play();
            //instance.Dispose();
            soundEffect.Play(GameVolume, 0f, 1f);
        }

        public void PlaySoundEffectOnce(SoundEffectInstance soundEffect, LocationType locationType)
        {
            if (Game1.GetCurrentStage() != null)
            {


                if (Game1.GetCurrentStage().LocationType == locationType)
                {
                    if (soundEffect.State == SoundState.Stopped)
                    {
                        soundEffect.Play();
                    }
                }
                else
                {
                    soundEffect.Stop();
                }
            }
        }

        public void PlaySoundEffectFromInt(int numberOfLoops, int soundKey)
        {



            for (int i = 0; i < numberOfLoops; i++)
            {
                switch (soundKey)
                {


                    case 1:

                        PlaySoundEffectInstance(WalkGrass);
                        break;
                    case 2:
                        PlaySoundEffectInstance(WalkStone);
                        break;
                    case 3:

                        PlaySoundEffectInstance(WalkWood);
                        break;
                    case 4:
                        PlaySoundEffectInstance(WalkSand);
                        break;
                    case 5:

                        PlaySoundEffectInstance(PickUpItem);
                        break;
                    case 6:
                       
                        PlaySoundEffectInstance(GrassBreak);
                        break;
                    case 7:
                       
                        PlaySoundEffectInstance(DigDirt);
                        break;
                    case 8:
                        
                        PlaySoundEffectInstance(StoneSmash);
                        break;
                    //day time sound effects
                    case 9:
                        PlaySoundEffectInstance(Chirp1);
                        break;
                    case 10:
                        PlaySoundEffectInstance(Chirp2);
                        break;
                    case 11:
                        PlaySoundEffectInstance(Chirp3);
                        break;
                    case 12:
                        PlaySoundEffectInstance(Crickets1);
                        break;
                    case 13:
                        PlaySoundEffectInstance(OwlHoot1);
                        break;

                    case 14:
                        PlaySoundEffectInstance(PigGrunt);
                        break;
                    case 15:
                        PlaySoundEffectInstance(PigGrunt2);
                        break;
                    case 16:
                        PlaySoundEffectInstance(CropPluck);
                        break;
                    case 17:
                        PlaySoundEffectInstance(PumpkinSmash);
                        break;


                }
            }



        }
    }
}

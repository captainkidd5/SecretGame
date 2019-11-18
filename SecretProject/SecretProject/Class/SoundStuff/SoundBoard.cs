using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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


        public SoundEffect Crickets1;


        public SoundEffect OwlHoot1;


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


            OwlHoot1 = content.Load<SoundEffect>("SoundEffects/owlHoot1");


            PigGrunt = content.Load<SoundEffect>("SoundEffects/pigGrunt");


            PigGrunt2 = content.Load<SoundEffect>("SoundEffects/PigGrunt2");


            CropPluck = content.Load<SoundEffect>("SoundEffects/ropePop");


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
                case Stages.World:
                
                    return DustStorm;
                case Stages.Town:
                    return Lakescape;
                default:
                    return Title;

            }

        }


        public void PlaySoundEffectInstance(SoundEffect soundEffect, float volume)
        {
            //SoundEffectInstance instance = soundEffect.CreateInstance();
            //instance.Volume = volume;
            //instance.Play();
            //instance.Dispose();
            soundEffect.Play(volume, 0f, 1f);
        }

        public void PlaySoundEffectFromInt(int numberOfLoops, int soundKey, float volume)
        {



            for (int i = 0; i < numberOfLoops; i++)
            {
                switch (soundKey)
                {


                    case 1:

                        PlaySoundEffectInstance(WalkGrass, volume);
                        break;
                    case 2:
                        PlaySoundEffectInstance(WalkStone, volume);
                        break;
                    case 3:

                        PlaySoundEffectInstance(WalkWood, volume);
                        break;
                    case 4:
                        PlaySoundEffectInstance(WalkSand, volume);
                        break;
                    case 5:

                        PlaySoundEffectInstance(PickUpItem, volume);
                        break;
                    case 6:
                        GrassBreak.Play();
                        break;
                    case 7:
                        DigDirt.Play();
                        break;
                    case 8:
                        StoneSmash.Play();
                        break;
                    //day time sound effects
                    case 9:
                        PlaySoundEffectInstance(Chirp1, volume);
                        break;
                    case 10:
                        PlaySoundEffectInstance(Chirp2, volume);
                        break;
                    case 11:
                        PlaySoundEffectInstance(Chirp3, volume);
                        break;
                    case 12:
                        PlaySoundEffectInstance(Crickets1, volume);
                        break;
                    case 13:
                        PlaySoundEffectInstance(OwlHoot1, volume);
                        break;

                    case 14:
                        PlaySoundEffectInstance(PigGrunt, volume);
                        break;
                    case 15:
                        PlaySoundEffectInstance(PigGrunt2, volume);
                        break;
                    case 16:
                        PlaySoundEffectInstance(CropPluck, volume);
                        break;


                }
            }



        }
    }
}

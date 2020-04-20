using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.SoundStuff
{
    public class SongChooser
    {
        public int NumberOfSongsBeforeRepeatAllowed;
        public List<SoundEffect> Songs { get; set; }
        public List<SoundEffect> LastPlayedSongs { get; set; }

        public SongChooser(List<SoundEffect> songs, int numberOfSongsBeforeRepeat)
        {
            this.NumberOfSongsBeforeRepeatAllowed = numberOfSongsBeforeRepeat;
            this.Songs = songs;
            this.LastPlayedSongs = new List<SoundEffect>();
        }

        //possible memory leak
        public SoundEffect FetchSong()
        {
            int songIndex = Game1.Utility.RNumber(0, Songs.Count + 1);

            if (LastPlayedSongs.Contains(Songs[songIndex]))
            {
                return FetchSong();
            }
            else
            {
                if(LastPlayedSongs.Count > 0)
                {
                    LastPlayedSongs.RemoveAt(0);
                }
                LastPlayedSongs.Add(Songs[songIndex]);
                
                return Songs[songIndex];
            }
        }

    }
}

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
        public static int NumberOfSongsBeforeRepeatAllowed = 3;
        public List<SoundEffect> Songs { get; set; }
        public List<SoundEffect> LastPlayedSongs { get; set; }

        public SongChooser(List<SoundEffect> songs)
        {
            this.Songs = songs;
            this.LastPlayedSongs = new List<SoundEffect>();
        }

        public SoundEffect FetchSong()
        {
            int songIndex = Game1.Utility.RNumber(0, Songs.Count);
            if (LastPlayedSongs.Count > NumberOfSongsBeforeRepeatAllowed || LastPlayedSongs.Count >= Songs.Count)
            {
                LastPlayedSongs.RemoveAt(LastPlayedSongs.Count - 1);
            }
            if (!LastPlayedSongs.Contains(Songs[songIndex]))
            {
                
                LastPlayedSongs.Add(Songs[songIndex]);
                return Songs[songIndex];
                
            }
            else
            {
                return FetchSong();
            }
        }

        
    }
}

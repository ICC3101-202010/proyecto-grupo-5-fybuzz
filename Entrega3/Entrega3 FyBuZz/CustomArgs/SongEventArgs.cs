using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega3_FyBuZz.CustomArgs
{
    public class SongEventArgs : EventArgs
    {
        public string NameText { get; set; }
        public string ArtistText { get; set; }
        public string AlbumText { get; set; }
        public string DiscographyText { get; set; }
        public string GenderText { get; set; }
        public DateTime DateText { get; set; }
        public string StudioText { get; set; }
        public double DurationText { get; set; }
        public string FormatText { get; set; }
        public string LyricsText { get; set; }
        public string FileNameText { get; set; }
        public string FileDestName { get; set; }
        public int RankingText { get; set; }
        public int SkipOrPrevious { get; set; }
        public PlayList playlistSong { get; set; }
        public string FileLyricsSource { get; set; }
        public List<string> OnQueueText { get; set; }
        public int NumberText { get; set; }
        public string PicFile { get; set; }
        public string PicSource {get; set; }
        public double PresentTimeText { get; set; }
    }
}

using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega3_FyBuZz.CustomArgs
{
    public class VideoEventArgs : EventArgs
    {
        public string NameText { get; set; }
        public string ActorsText { get; set; }
        public string DirectorsText { get; set; }
        public string ReleaseDateText { get; set; }
        public string DimensionText { get; set; }
        public string QualityText { get; set; }
        public string Categorytext { get; set; }
        public string DescriptionText { get; set; }
        public string VideoImageFile { get; set; } 
        public string VideoImageDest { get; set; }
        public string DurationText { get; set; }
        public string FormatText { get; set; }
        public string SubtitlesText { get; set; }
        public string FileDestText { get; set; }
        public string FileNameText { get; set; }
        public int RankingText { get; set; }
        public int previousOrSkip { get; set; }
        public PlayList playlistVideo { get; set; }
        public List<string> OnQueue { get; set; }
        public int NumberText { get; set; }
        public string VideoSubSource { get; set; }
        public double PresentTimeText { get; set; }
    }
}

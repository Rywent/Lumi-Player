using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LumiPlayer.Models
{
    public class PlaybackModel
    {
        public BitmapImage PathToShuffle { get; set; }
        public BitmapImage PathToRepeat { get; set; }

        public BitmapImage PathToPreviousTrack { get; set; }
        public BitmapImage PathToNextTrack { get; set; }
        public BitmapImage PathToPause { get; set; }

        public double SliderValue { get; set; }
        public double SliderMaximum { get; set; }
        
        public bool IsRepeat { get; set; }
        public bool IsShuffle { get; set; }
        public bool IsPlaying { get; set; }
    }
}

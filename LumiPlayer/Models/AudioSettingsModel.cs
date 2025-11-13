using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LumiPlayer.Models
{
    public class AudioSettingsModel
    {
        public string PathToFolder { get; set; }
        public BitmapImage VolumeButtonImg { get; set; }

        public double VolumeSliderValue { get; set; }
        public double VolumeSliderMaximum {  get; set; }
        public bool IsVolumeOn {  get; set; }
    }
}

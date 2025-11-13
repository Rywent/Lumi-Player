using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LumiPlayer.Models
{
    public class TrackInfoModel
    {
        public BitmapImage PathToImage { get; set; }
        public string TrackName { get; set; }
        public string ExecuteName { get; set; }
        public string Duration { get; set; }
        public string CurrentTime { get; set; }
    }
}

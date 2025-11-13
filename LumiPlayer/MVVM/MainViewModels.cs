using LumiPlayer.Models;
using LumiPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumiPlayer.MVVM
{
    public class MainViewModels
    {
        public TrackInfoViewModel TrackInfoVM { get; set; }
        public PlaybackControlViewModel PlaybackVM { get; set; }
        public AudioSettingsViewModel AudioSettingsVM { get; set; }
        public MainViewModels()
        {
            TrackInfoVM = new TrackInfoViewModel();
            PlaybackVM = new PlaybackControlViewModel(TrackInfoVM);
            AudioSettingsVM = new AudioSettingsViewModel(PlaybackVM);

            AudioSettingsVM.TrackUpdated += PlaybackVM.UpdateTracks;
        }
    }
}

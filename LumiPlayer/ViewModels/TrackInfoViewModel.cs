using LumiPlayer.Models;
using LumiPlayer.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using LumiPlayer.Scripts;

namespace LumiPlayer.ViewModels
{
    public class TrackInfoViewModel : DataUpdateViewModel
    {
        private TrackInfoModel _trackModel = new TrackInfoModel();

        public BitmapImage TrackImage
        {
            get => _trackModel.PathToImage;
            set
            {
                _trackModel.PathToImage = value;
                OnPropertyChanged("TrackImage");
            }
        }

        public string TrackName
        {
            get => _trackModel.TrackName;
            set
            {
                _trackModel.TrackName = value;
                OnPropertyChanged("TrackName");

            }
        }
        public string TrackExecuter
        {
            get => _trackModel.ExecuteName;
            set
            {
                _trackModel.ExecuteName = value;
                OnPropertyChanged("TrackExecuter");

            }
        }

        public string CurrentTime
        {
            get => _trackModel.CurrentTime;
            set
            {
                _trackModel.CurrentTime = value;
                OnPropertyChanged("CurrentTime");

            }
        }
        public string TrackDuration
        {
            get => _trackModel.Duration;
            set
            {
                _trackModel.Duration = value;
                OnPropertyChanged("TrackDuration");

            }
        }

        public TrackInfoViewModel()
        {
            TrackImage = SetImageFromPath("/Resources/img/SystemPhoto/photo-not-found.png");
            TrackName = "Track not selected";
            TrackExecuter = "...";
            TrackDuration = "00:00";
        }

        public void SetTrackData(string filePath)
        {
            var track = MusicMetadataReader.ReadMetadata(filePath);

            TrackName = track.TrackName ?? "No data";
            TrackExecuter = track.ExecuteName ?? "No data";
            TrackDuration = track.Duration ?? "No data";
            TrackImage = track.PathToImage;
        }

        private BitmapImage SetImageFromPath(string imagePath)
        {
            var defaultImage = new BitmapImage();
            defaultImage.BeginInit();
            defaultImage.UriSource = new Uri($"pack://application:,,,{imagePath}", UriKind.Absolute);
            defaultImage.CacheOption = BitmapCacheOption.OnLoad;
            defaultImage.EndInit();
            return defaultImage;
        }
    }
}

using LumiPlayer.Models;
using LumiPlayer.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace LumiPlayer.ViewModels
{
    public class AudioSettingsViewModel : DataUpdateViewModel
    {
        private PlaybackControlViewModel _playbackVM;
        private AudioSettingsModel _model;

        public Dictionary<int, string> Tracks = new Dictionary<int, string>();
        public event Action<Dictionary<int, string>> TrackUpdated;

        public RelayCommand OpenFolder => new RelayCommand(execute => SetFolderPath());
        public RelayCommand VolumeCommand => new RelayCommand(execute => OnOffVolume());


        public BitmapImage VolumeButton
        {
            get => _model.VolumeButtonImg;
            set
            {
                _model.VolumeButtonImg = value;
                OnPropertyChanged("VolumeButton");
            }
        }
        public string PathText
        {
            get => _model.PathToFolder;
            set
            {
                _model.PathToFolder = value;
                OnPropertyChanged("PathText");

            }
        }

        public double VolumeSliderValue
        {
            get => _model.VolumeSliderValue;
            set
            {
                if(_model.VolumeSliderValue != value)
                {
                    _model.VolumeSliderValue = value;
                    OnPropertyChanged("VolumeSliderValue");
                    _playbackVM._mediaPlayer.Volume = value;

                    if(value > 0)
                    {
                        VolumeButton = SetImageFromPath("/Resources/img/volume.png");

                    }
                    else
                    {
                        VolumeButton = SetImageFromPath("/Resources/img/volume-x.png");

                    }
                    OnPropertyChanged(nameof(VolumeButton));


                }
            }
        }

        public double VolumeSliderMaximum => _model.VolumeSliderMaximum;    

        public AudioSettingsViewModel(PlaybackControlViewModel playbackControlViewModel)
        {
            _playbackVM = playbackControlViewModel;
            _model = new AudioSettingsModel
            {
                VolumeSliderMaximum = 1.0,
                VolumeSliderValue = _playbackVM._mediaPlayer.Volume
            };
            VolumeButton = SetImageFromPath("/Resources/img/volume.png");

        }

        void SetFolderPath()
        {
            using(var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select music folder";
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;

                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    PathText = dialog.SelectedPath;
                    OnPropertyChanged("PathText");
                    GetAlltracks();

                }
            }
        }

        private void GetAlltracks()
        {
            if (string.IsNullOrEmpty(PathText))
                return;

            Tracks.Clear();
            string[] paths = Directory.GetFiles(PathText);
            for(int i = 0; i < paths.Length; i++)
            {
                Tracks.Add(i + 1, paths[i]);
            }
            TrackUpdated?.Invoke(Tracks);
        }

        public void OnOffVolume()
        {
            _model.IsVolumeOn = !_model.IsVolumeOn;
            if(_model.IsVolumeOn)
            {
                VolumeButton = SetImageFromPath("/Resources/img/volume.png");
                VolumeSliderValue = 0.2;
                OnPropertyChanged("VolumeButton");

            }
            else
            {
                VolumeButton = SetImageFromPath("/Resources/img/volume-x.png");
                VolumeSliderValue = 0;
                OnPropertyChanged("VolumeButton");
            }
        }


        static BitmapImage SetImageFromPath(string imagePath)
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

using LumiPlayer.Models;
using LumiPlayer.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace LumiPlayer.ViewModels
{
    public class PlaybackControlViewModel : DataUpdateViewModel
    {
        private TrackInfoViewModel _trackInfoViewModel;
        private PlaybackModel _model = new PlaybackModel();
        public MediaPlayer _mediaPlayer = new MediaPlayer();    

        Random random = new Random();
        private int _currentTrackId = 1;
        private Dictionary<int, string> _tracks;

        private DispatcherTimer _timer;
        private bool _isUpdatingSliderFromPlayer = false;


        public BitmapImage ShuffleButton
        {
            get => _model.PathToShuffle;
            set
            {
                _model.PathToShuffle = value;
                OnPropertyChanged("ShuffleButton");
            }
        }
        public BitmapImage RepeatButton
        {
            get => _model.PathToRepeat;
            set
            {
                _model.PathToRepeat = value;
                OnPropertyChanged("RepeatButton");

            }
        }
        public BitmapImage PauseButton
        {
            get => _model.PathToPause;
            set
            {
                _model.PathToPause = value;
                OnPropertyChanged("PauseButton");

            }
        }

        public BitmapImage PreviousTrackButton
        {
            get => _model.PathToPreviousTrack;
            set
            {
                _model.PathToPreviousTrack = value;
                OnPropertyChanged("PreviousTrackButton");

            }
        }
        public BitmapImage NextTrackButton
        {
            get => _model.PathToNextTrack;
            set
            {
                _model.PathToNextTrack = value;
                OnPropertyChanged("NextTrackButton");

            }
        }

        public double SliderValue
        {
            get => _model.SliderValue;  
            set
            {
                if(_model.SliderValue != value)
                {
                    _model.SliderValue = value;
                    OnPropertyChanged("SliderValue");

                    if(!_isUpdatingSliderFromPlayer && _mediaPlayer.NaturalDuration.HasTimeSpan)
                    {
                        var totalSeconds = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        _mediaPlayer.Position = TimeSpan.FromSeconds((value / 100) * totalSeconds);
                    }

                }
            }
        }
        public double SliderMaximum
        {
            get => _model.SliderMaximum;
            set
            {
                if(_model.SliderMaximum != value)
                {
                    _model.SliderMaximum = value;
                    OnPropertyChanged("SliderMaximum");

                }
            }
        }



        // commands
        public RelayCommand ShuffleCommand => new RelayCommand(execute => OnShuffle());
        public RelayCommand RepeatCommand => new RelayCommand(execute => OnRepeat());
        public RelayCommand PauseCommand => new RelayCommand(execute => OnPause());
        public RelayCommand NextTrackCommand => new RelayCommand(execute => OnNext());
        public RelayCommand PreviousTrackCommand => new RelayCommand(execute => OnPrevious());

        public PlaybackControlViewModel(TrackInfoViewModel trackInfoViewModel)
        {
            _model.IsRepeat = false;
            _model.IsShuffle = false;
            _model.IsPlaying = false;

            ShuffleButton = SetImageFromPath("/Resources/img/shuffle-off.png");
            RepeatButton = SetImageFromPath(@"/Resources/img/repeat-off.png");

            PauseButton = SetImageFromPath(@"/Resources/img/circle-pause.png");
            PreviousTrackButton = SetImageFromPath(@"/Resources/img/skip-back.png");
            NextTrackButton = SetImageFromPath(@"/Resources/img/skip-forward.png");

            _trackInfoViewModel = trackInfoViewModel;
        }

        public void UpdateTracks(Dictionary<int, string> tracks)
        {
            _tracks = tracks;
            SetTrack(_currentTrackId);
        }

        void SetTrack(int id)
        {
            if(_tracks.TryGetValue(id, out var track))
            {
                _mediaPlayer.Open(new Uri(track));
                _mediaPlayer.Play();
                _trackInfoViewModel.SetTrackData(track);

                SetTimer();
            }
        }

        void SetTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(10);
            _timer.Tick += (s, e) =>
            {
                if(_mediaPlayer.NaturalDuration.HasTimeSpan)
                {
                    if (SliderMaximum != 100)
                        SliderMaximum = 100;

                    var totalSeconds = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                    var currentSeconds = _mediaPlayer.Position.TotalSeconds;

                    if(totalSeconds > 0)
                    {
                        _isUpdatingSliderFromPlayer = true;
                        SliderValue = (currentSeconds / totalSeconds) * 100;
                        _isUpdatingSliderFromPlayer = false;

                        _trackInfoViewModel.CurrentTime = _mediaPlayer.Position.ToString(@"mm\:ss");

                        if (currentSeconds >= totalSeconds - 0.5)
                            OnTrackEnded();
                    }
                }
            };
            _timer.Start();
        }

        void OnShuffle()
        {
            _model.IsShuffle = !_model.IsShuffle;
            if(_model.IsShuffle)
                ShuffleButton = SetImageFromPath(@"/Resources/img/shuffle-on.png");
            else
                ShuffleButton = SetImageFromPath(@"/Resources/img/shuffle-off.png");
            OnPropertyChanged("IsShuffle");
        }

        void OnRepeat()
        {
            _model.IsRepeat = !_model.IsRepeat;
            if(_model.IsRepeat)
                RepeatButton = SetImageFromPath(@"/Resources/img/repeat-on.png");
            else
                RepeatButton = SetImageFromPath(@"/Resources/img/repeat-off.png");
            OnPropertyChanged("IsRepeat");

        }

        void OnPause()
        {
            _model.IsPlaying = !_model.IsPlaying;
            if(_model.IsPlaying)
            {
                PauseButton = SetImageFromPath(@"/Resources/img/circle-pause.png");
                _mediaPlayer.Play();
            }
            else
            {
                PauseButton = SetImageFromPath(@"/Resources/img/circle-play.png");
                _mediaPlayer.Pause();
            }
            OnPropertyChanged("IsPlaying");

        }
        void OnNext()
        {
            if(_currentTrackId + 1 <= _tracks.Count)
            {
                _currentTrackId++;
                SetTrack(_currentTrackId);
                PauseButton = SetImageFromPath(@"/Resources/img/circle-pause.png");

            }
        }
        void OnPrevious()
        {
            if(_currentTrackId - 1 >= 1)
            {
                _currentTrackId--;
                SetTrack(_currentTrackId);
                PauseButton = SetImageFromPath(@"/Resources/img/circle-pause.png");
            }
        }

        void OnTrackEnded()
        {
            if (_model.IsRepeat)
                SetTrack(_currentTrackId);
            else if (_model.IsShuffle)
                SetTrack(random.Next(1, _tracks.Count + 1));
            else
                OnNext();
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

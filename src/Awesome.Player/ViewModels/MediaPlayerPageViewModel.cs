using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Awesome.Player.Core.ExtentionMethods;
using Awesome.Player.Core.Infrastructure;
using Awesome.Player.Events;
using Awesome.Player.Models;
using MediaManager;
using MediaManager.Library;
using MediaManager.Player;
using MediaManager.Playback;
using MediaManager.Queue;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace Awesome.Player.ViewModels
{
	public class MediaPlayerPageViewModel : BaseProvider
	{
		private IEventAggregator _eventAggregator;
		public IMediaManager MediaManager { get; }
		
		private IMediaItem _source;
		public IMediaItem Source
		{
			get { return _source; }
			set { SetProperty(ref _source, value); }
		}
		
		private double _videoHeight = 200;
		public double VideoHeight
		{
			get { return _videoHeight; }
			set { SetProperty(ref _videoHeight, value); }
		}
		
		private double _videoWidth = 0;
		public double VideoWidth
		{
			get { return _videoWidth; }
			set { SetProperty(ref _videoWidth, value); } 
		}

		private MediaModel _media;
		public MediaModel Media
		{
			get { return _media; }
			set { SetProperty(ref _media, value); }
		}
		
		private MediaItem _mediaItem;
		public MediaItem MediaItem
		{
			get { return _mediaItem; }
			set { SetProperty(ref _mediaItem, value); }
		}

		private double _position = 0;
		public double Position
		{
			get => _position;
			set
			{
				SetProperty(ref _position, value);
			}
		}
		
		private double _duration = 100;
		public double Duration
		{
			get => _duration;
			set => SetProperty(ref _duration, value);
		}

		private double _progress;
		public double Progress
		{
			get => _progress;
			set => SetProperty(ref _progress, value);
		}

		private bool _dragStarted = false;
		public bool DragStarted
		{
			get => _dragStarted;
			set => SetProperty(ref _dragStarted, value);
		}

		private TimeSpan _timeSpanPosition = TimeSpan.Zero;
		public TimeSpan TimeSpanPosition
		{
			get => _timeSpanPosition;
			set
			{
				SetProperty(ref _timeSpanPosition, value);
			}
		}

		private TimeSpan _timeSpanDuration = TimeSpan.Zero;
		public TimeSpan TimeSpanDuration
		{
			get => _timeSpanDuration;
			set => SetProperty(ref _timeSpanDuration, value);
		}

		private double _timeTextPosition;
		public double TimeTextPosition
		{
			get { return _timeTextPosition; }
			set { SetProperty(ref _timeTextPosition, value); }
		}

		private double _sliderWidth;
		public double SliderWidth
		{
			get { return _sliderWidth; }
			set { SetProperty(ref _sliderWidth, value); }
		}

		private string _note;
		public string Note
		{
			get { return _note; }
			set { SetProperty(ref _note, value); }
		}

		private DelegateCommand _dragStartedCommand;
		public DelegateCommand DragStartedCommand =>
			_dragStartedCommand ?? (_dragStartedCommand = new DelegateCommand(() => DragStarted = true));

		private DelegateCommand _dragCompletedCommand;
		public DelegateCommand DragCompletedCommand =>
			_dragCompletedCommand ?? (_dragCompletedCommand = new DelegateCommand(ExecuteSliderDragCompletedCommand));

		async void ExecuteSliderDragCompletedCommand()
		{
			DragStarted = false;
			await MediaManager.SeekTo(TimeSpan.FromSeconds(Position));
		}
		
		private DelegateCommand<object> _addNoteCommand;
		public DelegateCommand<object> AddNoteCommand =>
			_addNoteCommand ?? (_addNoteCommand = new DelegateCommand<object>(ExecuteAddNoteCommand));

		void ExecuteAddNoteCommand(object obj)
		{
			var slider = (Slider)obj;
			var position = slider.Value;
			
			Debug.WriteLine($"************************************************** Slider X: {slider.X} ***********************************\n");
			Debug.WriteLine($"************************************************** Slider Y: {slider.Y} ***********************************\n");
			Debug.WriteLine($"************************************************** Clicked Position: {position} ***********************************\n");
			Debug.WriteLine($"************************************************** Slider Minimum: {slider.Minimum} ***********************************\n");
			Debug.WriteLine($"************************************************** Slider Maximum: {slider.Maximum} ***********************************\n");
			
		}

		private DelegateCommand _onBackToMainPageCommand;
		public DelegateCommand OnBackToMainPageCommand =>
			_onBackToMainPageCommand ?? (_onBackToMainPageCommand = new DelegateCommand(ExecuteOnBackToMainPageCommand));

		void ExecuteOnBackToMainPageCommand()
		{
			NavigationService.NavigateAsync("/MediaHomeView");
		}
		
		public MediaPlayerPageViewModel(
			INavigationService navigationService,
			IPageDialogService pageDialogService,
			IDeviceService deviceService, 
			IEventAggregator eventAggregator,
			IMediaManager mediaManager) : base(navigationService, pageDialogService, deviceService)
		{
			_eventAggregator = eventAggregator;
			MediaManager = mediaManager ?? throw new ArgumentNullException(nameof(mediaManager));

			MediaItem = new MediaItem();

			MediaManager.MediaPlayer.PropertyChanged += MediaPlayer_PropertyChanged;
			mediaManager.MediaItemChanged += MediaManager_MediaItemChanged;
			MediaManager.PositionChanged += MediaManager_PositionChanged;
		}

		private void MediaManager_PositionChanged(object sender, MediaManager.Playback.PositionChangedEventArgs e)
		{
			if (!DragStarted)
			{
				TimeSpanPosition = e.Position;
				Position = e.Position.TotalSeconds;
			}

			TimeSpanDuration = MediaManager.Duration;
			Duration = MediaManager.Duration.TotalSeconds;
			
			TimeTextPosition = ((SliderWidth - 25) * Position) / Duration;
			// note text in the position
			var n = Media.Notes.FirstOrDefault(x => Math.Floor(x.Position) == Math.Floor(Position));

			if (!string.IsNullOrEmpty(n?.Text))
			{
				Device.StartTimer(TimeSpan.FromSeconds(3), () =>
				{
					// Do something - change the note for 3 seconds
					Note = n.Text;

					return false; // True = Repeat again, False = Stop the timer
				});
			}
			else
			{
				Note = "";
			}

		}
		
		private void MediaManager_MediaItemChanged(object sender, MediaManager.Media.MediaItemEventArgs e)
		{
			Source = e.MediaItem;
		}

		private void MediaPlayer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(MediaManager.MediaPlayer.VideoHeight) && VideoWidth > 0)
			{
				VideoHeight = VideoWidth / MediaManager.MediaPlayer.VideoAspectRatio;
			}
		}

		public override async void OnNavigatedTo(INavigationParameters parameters)
		{
			if (parameters.ContainsKey("mediaFile"))
				Media = (MediaModel)parameters["mediaFile"];

			TimeSpanPosition = MediaManager.Position;
			Position = MediaManager.Position.TotalSeconds;
			TimeSpanDuration = MediaManager.Duration;
			Duration = MediaManager.Duration.TotalSeconds;

			if (Media == null) return;

			if (MediaManager.State == MediaPlayerState.Paused && CrossMediaManager.Current.Queue.Current.MediaUri == Media.Link) return;
			
			if (!MediaManager.IsPlaying() || CrossMediaManager.Current.Queue.Current.MediaUri != Media.Link)
			{
				if (string.IsNullOrEmpty(Media.Link)) return;
				await MediaManager.Stop();
				await CrossMediaManager.Current.Play(Media.Link);
			}

			DebugLogExtention.DebugModeGrandiosity($"{Media.Caption}\n{Media.Title}");

			MediaItem = (MediaItem) await CrossMediaManager.Current.Extractor.CreateMediaItem(Media.Link);
			Media.Extension = MediaItem.FileExtension.Trim('.');

			Debug.WriteLine($"************************************************** MIDIA DIRATION {MediaItem.Duration} ***********************************\n");
			Debug.WriteLine($"************************************************** MIDIA MediaUri {MediaItem.MediaUri} ***********************************\n");
			Debug.WriteLine($"************************************************** MIDIA Title {MediaItem.Title} ***********************************\n");
			Debug.WriteLine($"************************************************** MIDIA Display Title {MediaItem.DisplayTitle} ***********************************\n");
			Debug.WriteLine($"************************************************** MIDIA Link: {Media.Link} ***********************************\n");

			if (Media.Notes != null)
				_eventAggregator.GetEvent<NotesChangedEvent>().Publish(Media.Notes);

		}
	}
}

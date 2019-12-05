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

		private MediaPlayerState _playingState;
		public MediaPlayerState PlayingState
		{
			get { return _playingState; }
			set { SetProperty(ref _playingState, value); }
		}

		//private TimeSpan _nowDuration;
		//public TimeSpan NowDuration
		//{
		//	get { return _nowDuration; }
		//	set { SetProperty(ref _nowDuration, value); }
		//}

		private double _nowDurationNum;
		public double NowDurationNum
		{
			get { return _nowDurationNum; }
			set { SetProperty(ref _nowDurationNum, value); }
		}

		private TimeSpan _musicDuration;
		public TimeSpan MusicDuration
		{
			get { return _musicDuration; }
			set { SetProperty(ref _musicDuration, value); }
		}

		private MediaItem _mediaItem;
		public MediaItem MediaItem
		{
			get { return _mediaItem; }
			set { SetProperty(ref _mediaItem, value); }
		}

		private DelegateCommand _sliderDragCompletedCommand;
		public DelegateCommand SliderDragCompletedCommand =>
			_sliderDragCompletedCommand ?? (_sliderDragCompletedCommand = new DelegateCommand(ExecuteSliderDragCompletedCommand));

		async void ExecuteSliderDragCompletedCommand()
		{
			await CrossMediaManager.Current.SeekTo(TimeSpan.FromMilliseconds(NowDurationNum));
		}

		private DelegateCommand<MediaModel> _goBackToHomeCommand;
		public DelegateCommand<MediaModel> GoBackToHomeCommand =>
			_goBackToHomeCommand ?? (_goBackToHomeCommand = new DelegateCommand<MediaModel>(ExecuteGoBackToHomeCommand));

		void ExecuteGoBackToHomeCommand(MediaModel media)
		{
			//if (PlayingState==MediaPlayerState.Playing)
			//{
				var navigationParameters = new NavigationParameters();
				navigationParameters.Add("mediaFile", media);

				NavigationService.GoBackAsync(navigationParameters);

				//PlayingState = MediaPlayerState.Stopped;
			//}

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

			//NowDuration = TimeSpan.FromMilliseconds(200);
			MusicDuration = TimeSpan.FromMilliseconds(1000);
			
			//CrossMediaManager.Current.PositionChanged += Current_PositionChanged;
			MediaManager.MediaPlayer.PropertyChanged += MediaPlayer_PropertyChanged;
			mediaManager.MediaItemChanged += MediaManager_MediaItemChanged;
		}

		private void Current_PositionChanged(object sender, MediaManager.Playback.PositionChangedEventArgs e)
		{
			//NowDuration = e.Position;
			NowDurationNum = e.Position.TotalMilliseconds;
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

			DebugLogExtention.DebugModeGrandiosity($"{Media.Caption}\n{Media.Title}");

			await CrossMediaManager.Current.Play(Media.Link);

			MediaItem = (MediaItem) await CrossMediaManager.Current.Extractor.CreateMediaItem(Media.Link);
			MusicDuration = MediaItem.Duration;
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

using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Awesome.Player.Core.ExtentionMethods;
using Awesome.Player.Core.Infrastructure;
using Awesome.Player.Models;
using Awesome.Player.Resources;
using MediaManager;
using MediaManager.Library;
using MediaManager.Media;
using MediaManager.Player;
using MvvmHelpers;
using Plugin.FilePicker;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Realms;
using Xamarin.Forms;

namespace Awesome.Player.ViewModels
{
	public class MediaHomeViewViewModel : BaseProvider
	{
		#region properties

		private readonly IDialogService _dialogService;
		public IMediaManager MediaManager { get; }

		private ObservableRangeCollection<MediaItem> _medias ;
		public ObservableRangeCollection<MediaItem> Medias
		{
			get { return _medias; }
			set { SetProperty(ref _medias, value); }
		}

		//public IEnumerable<MediaItem> Medias { get; }

		private IMediaItem _source;
		public IMediaItem Source
		{
			get { return _source; }
			set { SetProperty(ref _source, value); }
		}

		private bool _isVisible;
		public bool IsVisible
		{
			get
			{
				_isVisible = MediaManager.State != MediaPlayerState.Stopped;
				return _isVisible;
			}
			set { SetProperty(ref _isVisible, value); }
		}

		private string _playPauseImage;
		public string PlayPauseImage
		{
			get
			{
				_playPauseImage = MediaManager.IsPlaying() ? AppIcons.PauseFont : AppIcons.PlayFont;
				return _playPauseImage;
			}
			set => SetProperty(ref _playPauseImage, value);
		}

		private string _likeFontFamily = Device.RuntimePlatform == Device.Android ? "FontAwesome5Pro-Regular-400.otf#Font Awesome 5 Pro Regular" : null; // set only for Android
		public string LikeFontFamily
		{
			get => _likeFontFamily;
			set => SetProperty(ref _likeFontFamily, value);
		}

		private double _progress;
		public double Progress
		{
			get => _progress;
			set => SetProperty(ref _progress, value);
		}

		public IMediaItem CurrentMediaItem => MediaManager.Queue.Current;

		private FormattedString _currentMediaItemText;
		public FormattedString CurrentMediaItemText
		{
			get
			{
				var currentMediaItemText = new FormattedString();
				if (MediaManager.Queue.Current != null)
				{
					currentMediaItemText.Spans.Add(new Span { Text = MediaManager.Queue.Current.DisplayTitle, FontAttributes = FontAttributes.Bold, FontSize = 12 });
					currentMediaItemText.Spans.Add(new Span { Text = " • " });
					currentMediaItemText.Spans.Add(new Span { Text = MediaManager.Queue.Current.DisplaySubtitle, FontSize = 12 });
				}
				else
					currentMediaItemText.Spans.Add(new Span { Text = "CHAMELEON" });

				_currentMediaItemText = currentMediaItemText;
				return currentMediaItemText;
			}
			set => SetProperty(ref _currentMediaItemText, value);
		}

		private bool _isLiked;
		public bool IsLiked
		{
			get { return _isLiked; }
			set { SetProperty(ref _isLiked, value); }
		}

		private bool _videoVisible;
		public bool VideoVisible
		{
			get
			{
				_videoVisible = CrossMediaManager.Current.Queue.HasCurrent && CrossMediaManager.Current.Queue.Current.MediaType == MediaType.Video;
				VideoGridHeight = _videoVisible ? 70 : 0;
				return _videoVisible;
			}
			set { SetProperty(ref _videoVisible, value); }
		}

		private int _videoGridHeight = 70;
		public int VideoGridHeight
		{
			get { return _videoGridHeight; }
			set { SetProperty(ref _videoGridHeight, value); }
		}

		private MediaItem _pickedMediaItem;
		public MediaItem PickedMediaItem
		{
			get => _pickedMediaItem;
			set => SetProperty(ref _pickedMediaItem, value);
		}

		private MediaItem _selectedMedia;
		public MediaItem SelectedMedia
		{
			get { return _selectedMedia; }
			set { SetProperty(ref _selectedMedia, value); }
		}

		#endregion properties

		#region Commands

		private DelegateCommand<MediaItem> _itemTappedCommand;

		public DelegateCommand<MediaItem> ItemTappedCommand =>
			_itemTappedCommand ??
			(_itemTappedCommand = new DelegateCommand<MediaItem>(ExecuteMediaItemTappedCommand));
			
		private DelegateCommand _playPauseCommand;
		public DelegateCommand PlayPauseCommand =>
			_playPauseCommand ?? (_playPauseCommand = new DelegateCommand(PlayPause));
			
		private DelegateCommand _likeCommand;
		public DelegateCommand LikeCommand =>
			_likeCommand ?? (_likeCommand = new DelegateCommand(LikeMedia));

		private DelegateCommand _openPickerDialog;
		public DelegateCommand OpenPickerDialog =>
			_openPickerDialog ?? (_openPickerDialog = new DelegateCommand(ExecuteOpenPickerDialog));

		private DelegateCommand _videoTappedCommand;
		public DelegateCommand VideoTappedCommand =>
			_videoTappedCommand ?? (_videoTappedCommand = new DelegateCommand(ExecuteVideoTappedCommand));
			
		void ExecuteOpenPickerDialog()
		{
			try
			{
				_dialogService.ShowDialog("ResourceAddView", CloseDialogCallback);
			}
			catch (Exception ex)
			{
				ex.DebugModeExceptionLog("ExecuteOpenPickerDialog Error:");
			}
		}

		private async void CloseDialogCallback(IDialogResult dialogResult)
		{
			if (dialogResult.Parameters.ContainsKey("pickedMediaItem"))
			{
				//var selectedMediaItem = (MediaItem)dialogResult.Parameters["selectedMediaItem"];
				PickedMediaItem = dialogResult.Parameters.GetValue<MediaItem>("pickedMediaItem");
			}

			SaveMediaToPlaylist(PickedMediaItem);

			await GetMediaItems();
		}

		private void SaveMediaToPlaylist(MediaItem pickedMediaItem)
		{
			var realm = Realm.GetInstance();

			var mediaModel = (MediaModel)pickedMediaItem;
			//var notes = new Note()
			//{
			//	MediaUri = mediaModel.MediaUri
			//};

			realm.Write(() =>
			{
				realm.Add(mediaModel);
				//realm.Add(notes);
			});
			
			var songs = realm.All<MediaModel>();
			var songNotes = realm.All<Note>()
				.Where(x=> x.MediaUri == mediaModel.MediaUri);
		}

		private async void ExecuteMediaItemTappedCommand(MediaItem media)
		{
			if (media == null) return;

			try
			{
				//var mediaItem = (MediaItem) await CrossMediaManager.Current.Extractor.CreateMediaItem(media.FileName);
				
				var navigationParameters = new NavigationParameters();
				navigationParameters.Add("mediaFile", media);
				await NavigationService.NavigateAsync("MediaPlayerPage", navigationParameters);
			}
			catch (Exception ex) {ex.DebugModeExceptionLog("Navigate from MainPage");}
		}

		private async void PlayPause()
		{
			if (MediaManager.IsPlaying())
				PlayPauseImage = AppIcons.PlayFont;
			else
				PlayPauseImage = AppIcons.PauseFont;

			await MediaManager.PlayPause();
		}

		private void LikeMedia()
		{
			if (IsLiked)
				LikeFontFamily = Device.RuntimePlatform == Device.Android ? "FontAwesome5Pro-Regular-400.otf#Font Awesome 5 Pro Regular" : null; // set only for Android
			else
				LikeFontFamily = Device.RuntimePlatform == Device.Android ? "FontAwesome5Pro-Solid-900.otf#Font Awesome 5 Pro Solid" : null; // set only for Android

			IsLiked = !IsLiked;
		}

		void ExecuteVideoTappedCommand()
		{
			var currentItem = CrossMediaManager.Current.Queue.Current;

			var parameters = new NavigationParameters();
			parameters.Add("mediaFile", currentItem);
			NavigationService.NavigateAsync("MediaPlayerPage", parameters);
		}

		#endregion Commands

		public MediaHomeViewViewModel(
			INavigationService navigationService,
			IPageDialogService pageDialogService,
			IDeviceService deviceService,
			IMediaManager mediaManager,
			IDialogService dialogService) : base(navigationService, pageDialogService, deviceService)
		{
			_dialogService = dialogService;
			MediaManager = mediaManager ?? throw new ArgumentNullException(nameof(mediaManager));
			
			Medias = new ObservableRangeCollection<MediaItem>();

			MediaManager.StateChanged += MediaManager_StateChanged;
			MediaManager.PositionChanged += MediaManager_PositionChanged;
			//mediaManager.MediaItemChanged += MediaManager_MediaItemChanged;
			MediaManager.MediaItemChanged += MediaManager_MediaItemChanged;
		}

		private async Task GetMediaItems()
		{
			var realm = Realm.GetInstance();
			var models = realm.All<MediaModel>();

			var tempCollection = new List<MediaItem>();

			foreach (var model in models)
			{
				var item = (MediaItem) await CrossMediaManager.Current.Extractor.CreateMediaItem(model.MediaUri);
				tempCollection.Add(item);
			}
			
			Medias.ReplaceRange(tempCollection);
		}


		private void MediaManager_StateChanged(object sender, MediaManager.Playback.StateChangedEventArgs e)
		{
			if (MediaManager.IsPlaying())
				PlayPauseImage = AppIcons.PauseFont;
			else
				PlayPauseImage = AppIcons.PlayFont;

			RaisePropertyChanged(nameof(CurrentMediaItemText));

			Progress = MediaManager.Position.TotalSeconds / MediaManager.Duration.TotalSeconds;

			RaisePropertyChanged(nameof(IsVisible));
			RaisePropertyChanged(nameof(CurrentMediaItem));
			RaisePropertyChanged(nameof(VideoVisible));
		}

		private void MediaManager_PositionChanged(object sender, MediaManager.Playback.PositionChangedEventArgs e)
		{
			Progress = e.Position.TotalSeconds / MediaManager.Duration.TotalSeconds;
		}
		
		private void MediaManager_MediaItemChanged(object sender, MediaManager.Media.MediaItemEventArgs e)
		{
			Source = e.MediaItem;
		}

		public async void NavigateFromViewCommand(MediaModel media)
		{
			if (media == null) return;

			try
			{
				var navigationParameters = new NavigationParameters();
				navigationParameters.Add("mediaFile", media);
				await NavigationService.NavigateAsync("MediaPlayerPage", navigationParameters);
			}
			catch (Exception ex) {}
		}

		public override async void OnNavigatedTo(INavigationParameters parameters)
		{
			await GetMediaItems();

		}

		public override void Destroy()
		{
			MediaManager.StateChanged -= MediaManager_StateChanged;
			MediaManager.PositionChanged -= MediaManager_PositionChanged;
			MediaManager.MediaItemChanged -= MediaManager_MediaItemChanged;
			GC.Collect();
		}
	}
}

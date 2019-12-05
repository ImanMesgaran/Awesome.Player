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
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace Awesome.Player.ViewModels
{
	public class MediaHomeViewViewModel : BaseProvider
	{
		public IMediaManager MediaManager { get; }

		/// <summary>
		///     The ListView item tapped command
		/// </summary>
		private DelegateCommand<MediaModel> _itemTappedCommand;

		/// <summary>
		///     Gets the ListView item tapped command.
		/// </summary>
		/// <value>The item tapped command.</value>
		public DelegateCommand<MediaModel> ItemTappedCommand =>
			_itemTappedCommand ??
			(_itemTappedCommand = new DelegateCommand<MediaModel>(ExecuteMediaItemTappedCommand));

		private async void ExecuteMediaItemTappedCommand(MediaModel media)
		{
			if (media == null) return;

			//var currentMedia = CrossMediaManager.Current.IsPlaying();

			//if (CrossMediaManager.Current.IsPlaying() && CrossMediaManager.Current.Queue.Current.Title != media.Title)
			//{
			//	await CrossMediaManager.Current.Stop();
			//}

			//await CrossMediaManager.Current.Stop();


			//var mediaItem = (MediaItem)await CrossMediaManager.Current.Extractor.CreateMediaItem(media.Link);
			//Debug.WriteLine($"************************************************** HOMEPAGE ONSELECT MEDIA LINK: {media.Link} ***********************************\n");
			
			//await CrossMediaManager.Current.Play(media.Link);
			//await CrossMediaManager.Current.Play("http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4");
			try
			{
				
				var navigationParameters = new NavigationParameters();
				navigationParameters.Add("mediaModel", media);
				await NavigationService.NavigateAsync("MediaPlayerPage", navigationParameters);
				
			}
			catch (Exception ex) {ex.DebugModeExceptionLog("Navigate from MainPage");}
		}

		private DelegateCommand _playPauseCommand;
		public DelegateCommand PlayPauseCommand =>
			_playPauseCommand ?? (_playPauseCommand = new DelegateCommand(PlayPause));

		private async void PlayPause()
		{
			if (MediaManager.IsPlaying())
				PlayPauseImage = AppIcons.PlayFont;
			else
				PlayPauseImage = AppIcons.PauseFont;

			await MediaManager.PlayPause();
		}

		private DelegateCommand _likeCommand;
		public DelegateCommand LikeCommand =>
			_likeCommand ?? (_likeCommand = new DelegateCommand(LikeMedia));

		private void LikeMedia()
		{
			if (IsLiked)
				LikeFontFamily = Device.RuntimePlatform == Device.Android ? "FontAwesome5Pro-Regular-400.otf#Font Awesome 5 Pro Regular" : null; // set only for Android
			else
				LikeFontFamily = Device.RuntimePlatform == Device.Android ? "FontAwesome5Pro-Solid-900.otf#Font Awesome 5 Pro Solid" : null; // set only for Android

			IsLiked = !IsLiked;
		}

		private MediaModel _mediaFile;
		public MediaModel MediaFile
		{
			get { return _mediaFile; }
			set { SetProperty(ref _mediaFile, value); }
		}

		private ObservableCollection<MediaModel> _medias;
		public ObservableCollection<MediaModel> Medias
		{
			get { return _medias; }
			set { SetProperty(ref _medias, value); }
		}

		//private TimeSpan _musicDuration;
		//public TimeSpan MusicDuration
		//{
		//	get { return _musicDuration; }
		//	set { SetProperty(ref _musicDuration, value); }
		//}

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
		
		private string _playPauseImage =  AppIcons.PauseFont;
		public string PlayPauseImage
		{
			get => _playPauseImage;
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

		public MediaHomeViewViewModel(
			INavigationService navigationService,
			IPageDialogService pageDialogService,
			IDeviceService deviceService,
			IMediaManager mediaManager) : base(navigationService, pageDialogService, deviceService)
		{
			MediaManager = mediaManager ?? throw new ArgumentNullException(nameof(mediaManager));
			
			MediaManager.StateChanged += MediaManager_StateChanged;
			MediaManager.PositionChanged += MediaManager_PositionChanged;
			mediaManager.MediaItemChanged += MediaManager_MediaItemChanged;
			
			#region hard-coded files

			Medias = new ObservableCollection<MediaModel>()
			{
				new MediaModel()
				{
					Image = "https://m.media-amazon.com/images/I/51JhWaf36qL._AA256_.jpg",
					Title = "Vivaldi Four Season - Winter",
					Caption = "Best of Classical Masterpieces",
					Duration = "7:32",
					Views = "3.2M",
					Link = "https://dl.just-music.ir/music/BestOf/Vivaldi/The Very Best Of/CD1/01 - Violin Concerto, for violin, strings & continuo in E major ('La Primavera,' The.mp3",
					Notes = new List<Note>()
					{
						new Note()
						{
							Text = "sample 1",
							Position = 1682
						},
						new Note()
						{
							Text = "sample 2",
							Position = 51529
						},
						new Note()
						{
							Text = "sample 3",
							Position = 123247
						},
						new Note()
						{
							Text = "sample 4",
							Position = 214488
						}
					}
				},
				new MediaModel()
				{
					Image = "https://static-s.aa-cdn.net/img/ios/537630791/8bf08ea73d7eec85c76e87408dfddbdc?v=1",
					Title = "Beethoven Symphony No. 5, c",
					Caption = "Beethoven Works - Ludwig Van Beethoven Songs",
					Duration = "11:00",
					Views = "1.8M",
					Link = "https://dl.just-music.ir/music/BestOf/Vivaldi/The Very Best Of/CD1/01 - Violin Concerto, for violin, strings & continuo in E major ('La Primavera,' The.mp3"
				},
				new MediaModel()
				{
					Image = "https://m.media-amazon.com/images/I/41D7UYio+zL._AA256_.jpg",
					Title = "Beethoven Symphony No. 8",
					Caption = "Beethoven Works - Ludwig Van Beethoven Songs",
					Duration = "8:16",
					Views = "10.5M",
					Link = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4"
				},
				new MediaModel()
				{
					Image = "https://i.guim.co.uk/img/static/sys-images/Guardian/Pix/arts/2006/01/26/moz3128.jpg?width=300&quality=85&auto=format&fit=max&s=17a0dec768c8dcf4a224fe1416be9f48",
					Title = "Mozart – Eine kleine Nachtmusik",
					Caption = "Best of Classical Masterpieces",
					Duration = "7:32",
					Views = "900.3K",
					Link = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4"
				},
				new MediaModel()
				{
					Image = "https://m.media-amazon.com/images/I/51JhWaf36qL._AA256_.jpg",
					Title = "Beethoven – Für Elise",
					Caption = "Best of Classical Masterpieces",
					Duration = "7:32",
					Views = "2.3M",
					Link = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4"
				},
				new MediaModel()
				{
					Image = "https://m.media-amazon.com/images/I/51JhWaf36qL._AA256_.jpg",
					Title = "Vivaldi Four Season - Winter",
					Caption = "Best of Classical Masterpieces",
					Duration = "7:32",
					Views = "4.1M",
					Link = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4"
				},new MediaModel()
				{
					Image = "https://m.media-amazon.com/images/I/51JhWaf36qL._AA256_.jpg",
					Title = "Vivaldi Four Season - Winter",
					Caption = "Best of Classical Masterpieces",
					Duration = "7:32",
					Views = "302K",
					Link = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4"
				}
			};

			#endregion hard-coded files

			//MusicDuration = TimeSpan.FromMilliseconds(1000);
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
			catch (Exception ex)
			{
				
			}
		}

		public override void OnNavigatedTo(INavigationParameters parameters)
		{
			if (parameters.ContainsKey("mediaFile"))
			{
				MediaFile = (MediaModel)parameters["mediaFile"];

				DebugLogExtention.DebugModeGrandiosity("*************");
			}
		}
	}
}

using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Awesome.Player.Core.ExtentionMethods;
using Awesome.Player.Core.Infrastructure;
using Awesome.Player.Models;
using Prism.Navigation;
using Prism.Services;

namespace Awesome.Player.ViewModels
{
	public class MediaHomeViewViewModel : BaseProvider
	{
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

			try
			{
				var navigationParameters = new NavigationParameters();
				navigationParameters.Add("mediaModel", media);
				await NavigationService.NavigateAsync("MediaPlayerPage", navigationParameters);
			}
			catch (Exception ex) {}
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

		public MediaHomeViewViewModel(
			INavigationService navigationService,
			IPageDialogService pageDialogService,
			IDeviceService deviceService) : base(navigationService, pageDialogService, deviceService)
		{
			Medias = new ObservableCollection<MediaModel>()
			{
				new MediaModel()
				{
					Image = "https://m.media-amazon.com/images/I/51JhWaf36qL._AA256_.jpg",
					Title = "Vivaldi Four Season - Winter",
					Caption = "Best of Classical Masterpieces",
					Duration = "7:32",
					Views = "3.2M",
					Link = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4"
				},
				new MediaModel()
				{
					Image = "https://static-s.aa-cdn.net/img/ios/537630791/8bf08ea73d7eec85c76e87408dfddbdc?v=1",
					Title = "Beethoven Symphony No. 5, c",
					Caption = "Beethoven Works - Ludwig Van Beethoven Songs",
					Duration = "11:00",
					Views = "1.8M",
					Link = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4"
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
				//DebugLogExtention.DebugModeGrandiosity($"{MediaFile.Position}\n");
			}
		}
	}
}

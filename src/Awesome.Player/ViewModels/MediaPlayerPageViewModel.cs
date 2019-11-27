using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Awesome.Player.Core.ExtentionMethods;
using Awesome.Player.Core.Infrastructure;
using Awesome.Player.Models;
using MediaManager.Player;
using Prism.Navigation;
using Prism.Services;

namespace Awesome.Player.ViewModels
{
	public class MediaPlayerPageViewModel : BaseProvider
	{
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

		public MediaPlayerPageViewModel(
			INavigationService navigationService,
			IPageDialogService pageDialogService,
			IDeviceService deviceService) : base(navigationService, pageDialogService, deviceService)
		{

		}
		
		public override void OnNavigatedTo(INavigationParameters parameters)
		{
			if (parameters.ContainsKey("mediaFile"))
				Media = (MediaModel)parameters["mediaFile"];

			DebugLogExtention.DebugModeGrandiosity($"{Media.Caption}\n{Media.Title}");
		}
	}
}

using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using Awesome.Player.Core.ExtentionMethods;
using Awesome.Player.Core.Infrastructure;
using Awesome.Player.Events;
using MediaManager;
using MediaManager.Library;
using Plugin.FilePicker;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;

namespace Awesome.Player.ViewModels
{
	public class ResourceAddViewViewModel : BaseProvider, IDialogAware
	{
		private readonly IEventAggregator _eventAggregator;
		private readonly IUserDialogs _userDialogs;

		private MediaItem _selectedItem = new MediaItem();
		public MediaItem SelectedItem
		{
			get { return _selectedItem; }
			set { SetProperty(ref _selectedItem, value); }
		}

		private DelegateCommand _openFilePickerCommand;
		public DelegateCommand OpenFilePickerCommand =>
			_openFilePickerCommand ?? (_openFilePickerCommand = new DelegateCommand(OpenFilePicker));

		private DelegateCommand<IDialogParameters> _closeCommand;
		public DelegateCommand<IDialogParameters> CloseCommand =>
			_closeCommand ?? (_closeCommand = new DelegateCommand<IDialogParameters>(ExecuteCloseCommand));

		private void ExecuteCloseCommand(IDialogParameters parameters = null)
		{
			if (RequestClose != null) RequestClose(parameters);
		}
		
		private DelegateCommand _openVideoPickerCommand;
		public DelegateCommand OpenVideoPickerCommand =>
			_openVideoPickerCommand ?? (_openVideoPickerCommand = new DelegateCommand(OpenVideoPicker));

		private DelegateCommand _openUrlCommand;
		public DelegateCommand OpenUrlCommand =>
			_openUrlCommand ?? (_openUrlCommand = new DelegateCommand(OpenUrl));

		private async void OpenFilePicker()
		{
			try
			{
				//var status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
				//if (status != PermissionStatus.Granted)
				//{
				//	await _userDialogs.AlertAsync("EnablePermissions");
				//	return;
				//}

				var fileData = await CrossFilePicker.Current.PickFile();
				if (fileData == null)
					return; // user canceled file picking

				/*
				await NavigationService.NavigateAsync("MediaPlayerPage");
				await CrossMediaManager.Current.Play(fileData.FilePath);
				*/

				//var mediaItem = (MediaItem) await CrossMediaManager.Current.Extractor.CreateMediaItem(fileData.FilePath);
				//var parameters = new NavigationParameters();
				//parameters.Add("selectedMediaItem", mediaItem);
				//await NavigationService.GoBackAsync(parameters);
				
				// setting the selected video file

				SelectedItem = (MediaItem)await CrossMediaManager.Current.Extractor.CreateMediaItem(fileData.FilePath);

				var parameters = new DialogParameters();
				parameters.Add("pickedMediaItem", SelectedItem);

				//if (RequestClose != null) RequestClose(parameters);
				ExecuteCloseCommand(parameters);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception choosing file: " + ex.ToString());
			}
		}
		private async void OpenVideoPicker()
		{
			if (await CrossMedia.Current.Initialize() && CrossMedia.Current.IsPickVideoSupported)
			{
				string path = null;
				try
				{
					var videoMediaFile = await CrossMedia.Current.PickVideoAsync();
					if (videoMediaFile != null)
					{
						path = videoMediaFile.Path;
					}
				}
				catch (MediaPermissionException)
				{
					await _userDialogs.AlertAsync("EnablePermissions");
				}
				if (!string.IsNullOrEmpty(path))
				{
					//await NavigationService.NavigateAsync("MediaPlayerPage");
					//await CrossMediaManager.Current.Play(path);

					SelectedItem = (MediaItem)await CrossMediaManager.Current.Extractor.CreateMediaItem(path);

					var parameters = new DialogParameters();

					if (SelectedItem.MediaType == MediaType.Audio || SelectedItem.MediaType == MediaType.Video)
					{
						parameters.Add("pickedMediaItem", SelectedItem);
					}

					ExecuteCloseCommand(parameters);
				}
			}
			else await _userDialogs.AlertAsync("EnablePermissions");
		}
		private async void OpenUrl()
		{
			try
			{
				var result = await _userDialogs.PromptAsync("EnterUrl", inputType: InputType.Url);

				if (!result.Ok) return;

				//TODO: Check if the url is valid
				if (!string.IsNullOrWhiteSpace(result.Value))
				{
					//await NavigationService.NavigateAsync("MediaPlayerPage");
					//await CrossMediaManager.Current.Play(result.Value);

					bool isUri = Uri.IsWellFormedUriString(result.Value, UriKind.RelativeOrAbsolute);

					if (isUri)
					{
						SelectedItem =
							(MediaItem) await CrossMediaManager.Current.Extractor.CreateMediaItem(result.Value);
					}

					var parameters = new DialogParameters();

					if (SelectedItem.MediaType == MediaType.Audio || SelectedItem.MediaType == MediaType.Video)
					{
						parameters.Add("pickedMediaItem", SelectedItem);
					}

					ExecuteCloseCommand(parameters);
				}
			}
			catch (Exception ex)
			{
				ex.DebugModeExceptionLog("Select Media Uri Error:");
			}
		}

		public ResourceAddViewViewModel(
			INavigationService navigationService, 
			IPageDialogService pageDialogService,
			IDeviceService deviceService,
			IUserDialogs userDialogs,
			IEventAggregator eventAggregator) : base(navigationService, pageDialogService, deviceService)
		{
			_eventAggregator = eventAggregator;
			_userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));

			//CloseCommand = new DelegateCommand(() =>
			//{
			//	if (RequestClose != null) RequestClose(null);
			//});
		}

		public bool CanCloseDialog() => true;

		public void OnDialogClosed()
		{
			_eventAggregator
				.GetEvent<CloseDialogEvent>()
				.Unsubscribe(() => ExecuteCloseCommand());
		}

		public void OnDialogOpened(IDialogParameters parameters)
		{
			_eventAggregator
				.GetEvent<CloseDialogEvent>()
				.Subscribe(() => ExecuteCloseCommand());

		}

		public event Action<IDialogParameters> RequestClose;
	}
}

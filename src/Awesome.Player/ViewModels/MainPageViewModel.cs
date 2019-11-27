using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Awesome.Player.Core.Infrastructure;
using Prism.Services;

namespace Awesome.Player.ViewModels
{
	public class MainPageViewModel : BaseProvider
	{
		public MainPageViewModel(
			INavigationService navigationService,
			IPageDialogService pageDialogService,
			IDeviceService deviceService) : base(navigationService, pageDialogService, deviceService)
		{
			Title = "Main Page";
		}
	}
}

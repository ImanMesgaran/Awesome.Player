﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Awesome.Player.Core.Infrastructure;
using Prism.Navigation;
using Prism.Services;

namespace Awesome.Player.ViewModels
{
	public class MediaSearchViewViewModel : BaseProvider
	{
		public MediaSearchViewViewModel(
			INavigationService navigationService,
			IPageDialogService pageDialogService,
			IDeviceService deviceService) : base(navigationService, pageDialogService, deviceService)
		{

		}
	}
}

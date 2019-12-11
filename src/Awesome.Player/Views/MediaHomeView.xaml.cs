using System;
using System.Linq;
using Awesome.Player.Events;
using Awesome.Player.Models;
using Awesome.Player.ViewModels;
using MediaManager;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Xamarin.Forms;

namespace Awesome.Player.Views
{
	public partial class MediaHomeView : ContentPage , IDestructible
	{
		private readonly IEventAggregator _eventAggregator;

		public IMediaManager MediaManager { get; }

		//public MediaHomeView()
		//{

		//}

		public MediaHomeView(
			IEventAggregator eventAggregator)
		{
			InitializeComponent();

			_eventAggregator = eventAggregator;

			MediaManager = ((MediaHomeViewViewModel) this.BindingContext).MediaManager;
			
		}

		public void Destroy()
		{
			MediaCollectionView.Behaviors?.Clear();
			videoView.Dispose();
			GC.Collect();
		}

		protected override bool OnBackButtonPressed()
		{
			var mediaViewmodel = this.BindingContext as MediaHomeViewViewModel;
			
			var dialogInitialized = mediaViewmodel != null && mediaViewmodel.IsDialogInitialized;

			if (dialogInitialized)
			{
				_eventAggregator.GetEvent<CloseDialogEvent>().Publish();

				return true;
			}

			return false;
		}
	}
}

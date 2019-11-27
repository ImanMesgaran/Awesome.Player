using System;
using System.Linq;
using Awesome.Player.Models;
using Awesome.Player.ViewModels;
using Prism.Navigation;
using Xamarin.Forms;

namespace Awesome.Player.Views
{
	public partial class MediaHomeView : ContentPage,IDestructible
	{
		public MediaHomeView()
		{
			InitializeComponent();
		}

		public void Destroy()
		{
			MediaCollectionView.Behaviors?.Clear();
		}

		private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection.FirstOrDefault() is MediaModel item)
			{
				var viewmodel = this.BindingContext as MediaHomeViewViewModel;
				viewmodel?.NavigateFromViewCommand((MediaModel)e.CurrentSelection.FirstOrDefault());
				MediaCollectionView.SelectedItem = null;
			}
		}
	}
}

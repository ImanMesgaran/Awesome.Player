using Prism.Navigation;
using Xamarin.Forms;

namespace Awesome.Player.Views
{
	public partial class MediaPlayerPage : ContentPage, IDestructible
	{
		public MediaPlayerPage()
		{
			InitializeComponent();
		}

		public void Destroy()
		{
			
		}
	}
}

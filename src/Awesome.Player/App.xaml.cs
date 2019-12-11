using Acr.UserDialogs;
using Prism;
using Prism.Ioc;
using Awesome.Player.ViewModels;
using Awesome.Player.Views;
using MediaManager;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Awesome.Player
{
	public partial class App
	{
		/* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
		public App() : this(null) { }

		public App(IPlatformInitializer initializer) : base(initializer) { }

		protected override async void OnInitialized()
		{
			InitializeComponent();

			await NavigationService.NavigateAsync("MyTabbedPage");
			//await NavigationService.NavigateAsync("MyNavigationPage/MainPage");
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<NavigationPage>();
			containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
			containerRegistry.RegisterForNavigation<MyTabbedPage, MyTabbedPageViewModel>();
			containerRegistry.RegisterForNavigation<MyNavigationPage, MyNavigationPageViewModel>();
			containerRegistry.RegisterForNavigation<MediaHomeView, MediaHomeViewViewModel>();
			containerRegistry.RegisterForNavigation<MediaNewsView, MediaNewsViewViewModel>();
			containerRegistry.RegisterForNavigation<MediaSearchView, MediaSearchViewViewModel>();
			containerRegistry.RegisterForNavigation<MediaLibraryView, MediaLibraryViewViewModel>();
			containerRegistry.RegisterForNavigation<MediaPlayerPage, MediaPlayerPageViewModel>();

			//var mediaManager = CrossMediaManager.Current;
			containerRegistry.RegisterInstance<IMediaManager>(CrossMediaManager.Current);
			containerRegistry.RegisterInstance<IUserDialogs>(UserDialogs.Instance);
			containerRegistry.RegisterDialog<ResourceAddView, ResourceAddViewViewModel>();
			containerRegistry.RegisterDialog<NoteAddView, NoteAddViewViewModel>();
			containerRegistry.RegisterForNavigation<EmptyView, EmptyViewViewModel>();
		}
	}
}

using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MediaManager;
using Plugin.CurrentActivity;
using Prism;
using Prism.Ioc;

namespace Awesome.Player.Droid
{
    [Activity(Label = "Awesome.Player", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.Orientation ,ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            CrossMediaManager.Current.Init(this);
            CrossCurrentActivity.Current.Init(this, bundle);
            UserDialogs.Init(this);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

			LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
	        Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

	        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}


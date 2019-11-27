using Prism.Navigation;
using Prism.Services;

namespace Awesome.Player.Core.Infrastructure
{
    public class BaseProvider : NavigationViewModelBase
    {
        #region Default Constructor

        public BaseProvider(INavigationService navigationService , IPageDialogService pageDialogService ,
            IDeviceService deviceService) : base(navigationService , pageDialogService , deviceService) { }

        public override void OnNavigatedFrom(INavigationParameters parameters) { }

        public override void OnNavigatedTo(INavigationParameters parameters) { }

        public override void OnNavigatingTo(INavigationParameters parameters) { }

        #endregion Default Constructor

        protected static string _baseUrl;
    }
}
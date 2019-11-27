using System;
using System.Threading.Tasks;
using Prism;
using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace Awesome.Player.Core.Infrastructure
{
    /// <summary>
    /// Class ViewModelBase
    /// </summary>
    /// <seealso cref="BindableBase" />
    /// <seealso cref="Prism.Navigation.INavigationAware" />
    /// <seealso cref="Prism.Navigation.IConfirmNavigation" />
    /// <seealso cref="Prism.Navigation.IConfirmNavigationAsync"/>
    /// <seealso cref="Prism.AppModel.IApplicationLifecycleAware"/>
    /// <seealso cref="Prism.AppModel.IPageLifecycleAware"/>
    /// <seealso cref="Prism.IActiveAware"/>
    /// <seealso cref="Prism.Navigation.IDestructible" /> 
    public abstract class ViewModelBase : BindableBase, INavigationAware, IConfirmNavigation, IConfirmNavigationAsync, IApplicationLifecycleAware, IPageLifecycleAware, IActiveAware, IDestructible
    {
        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        /// <value>The navigation service.</value>
        protected INavigationService NavigationService { get; }

        /// <summary>
        /// Gets the page dialog service.
        /// </summary>
        /// <value>The page dialog service.</value>
        protected IPageDialogService PageDialogService { get; }

        /// <summary>
        /// Gets the device service.
        /// </summary>
        /// <value>The device service.</value>
        protected IDeviceService DeviceService { get; }

        protected ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService,
                             IDeviceService deviceService)
        {
            if (navigationService == null)
            {
                throw new ArgumentNullException(nameof(navigationService));
            }
            if (pageDialogService == null)
            {
                throw new ArgumentNullException(nameof(pageDialogService));
            }
            if (deviceService == null)
            {
                throw new ArgumentNullException(nameof(deviceService));
            }
            PageDialogService = pageDialogService;
            DeviceService = deviceService;
            NavigationService = navigationService;
        }

        #region INavigationAware

        /// <summary>
        /// Invoked by Prism after navigating away from viewmodel's page.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public abstract void OnNavigatedFrom(INavigationParameters parameters);

        /// <summary>
        /// Invoked by Prism after navigating to the viewmodel's page.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public abstract void OnNavigatedTo(INavigationParameters parameters);

        /// <summary>
        /// Invoked by Prism before navigating to the viewmodel's page. Deriving classes can use this method to invoke async loading of data instead of waiting for the OnNavigatedTo method to be invoked.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public abstract void OnNavigatingTo(INavigationParameters parameters);

        #endregion INavigationAware

        #region IConfirmNavigation / IConfirmNavigationAsync

        public virtual bool CanNavigate(INavigationParameters parameters) => true;

        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters) =>
            Task.FromResult(CanNavigate(parameters));

        #endregion IConfirmNavigation / IConfirmNavigationAsync

        #region IApplicationLifecycleAware

        public virtual void OnResume() { }

        public virtual void OnSleep() { }

        #endregion IApplicationLifecycleAware
        
        #region IPageLifecycleAware

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }

        #endregion IPageLifecycleAware

        #region IActiveAware

        protected bool HasInitialized { get; set; }
		
        // NOTE: Prism.Forms only sets IsActive, and does not do anything with the event.
        public event EventHandler IsActiveChanged
        {
            add { }
            remove { }
        }

        bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, OnIsActiveChanged); }
        }
        
        protected virtual void OnIsActiveChanged()
        {
            // NOTE: You must either subscribe to the event or handle the logic here.
            // TODO: uncomment IsActiveChanged?.Invoke to be able to add IsActiveChanged += Active/Deactive to ViewModel Constructure
            //IsActiveChanged?.Invoke(this, EventArgs.Empty);

            if (IsActive)
            {
                HandleActivation();
            }
            else
            {
                HandleDisactivation();
            }
        }

        protected virtual void HandleActivation() { }

        protected virtual void HandleDisactivation() { }

        #endregion IActiveAware

        #region IDestructible

        public virtual void Destroy() { }

        #endregion IDestructible
    }
}
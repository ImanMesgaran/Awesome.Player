using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace Awesome.Player.Core.Infrastructure
{
    /// <summary>
    /// Class NavigationAwareViewModelBase.
    /// </summary>
    /// <seealso cref="ViewModelBase"/>
    public abstract class NavigationViewModelBase : ViewModelBase
    {
        DelegateCommand<string> _navigateAbsoluteCommand;
        DelegateCommand<string> _navigateCommand;
        DelegateCommand<string> _navigateModalCommand;
        DelegateCommand<string> _navigateNonModalCommand;

        const string ButtonTextOK = "OK";
        const string CaptionError = "Error";
        const string ParameterKey = "Key";
        const string RootUriPrependText = "/";

        #region NavigationStack Commands

        /// <summary>
        /// Gets the NavigateAbsoluteCommand.
        /// </summary>
        public DelegateCommand<string> NavigateAbsoluteCommand => _navigateAbsoluteCommand ?? (_navigateAbsoluteCommand = new DelegateCommand<string>(async param => await NavigateAbsoluteCommandExecute(param), CanNavigateAbsoluteCommandExecute));

        /// <summary>
        /// Gets the navigate command.
        /// </summary>
        /// <value>The navigate command.</value>
        public DelegateCommand<string> NavigateCommand => _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(async param => await NavigateCommandExecute(param), CanNavigateCommandExecute));

        /// <summary>
        /// Gets the navigate modal command.
        /// </summary>
        /// <value>The navigate modal command.</value>
        public DelegateCommand<string> NavigateModalCommand => _navigateModalCommand ?? (_navigateModalCommand = new DelegateCommand<string>(async param => await NavigateModalCommandExecute(param), CanNavigateModalCommandExecute));

        /// <summary>
        /// Gets the NavigateNonModalCommand.
        /// </summary>
        public DelegateCommand<string> NavigateNonModalCommand => _navigateNonModalCommand ?? (_navigateNonModalCommand = new DelegateCommand<string>(async param => await NavigateNonModalCommandExecute(param), CanNavigateNonModalCommandExecute));
        
        #region  Navigation Methods & Overloads

        /// <summary>
        /// Determines whether this instance can execute the NavigateAbsoluteCommand.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns><c>true</c> if this instance can execute NavigateAbsoluteCommand; otherwise, <c>false</c>.</returns>
        protected virtual bool CanNavigateAbsoluteCommandExecute(string uri)
        {
            return !string.IsNullOrEmpty(uri);
        }

        /// <summary>
        /// Determines whether this instance can execute the NavigateAbsoluteCommand.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns><c>true</c> if this instance can execute NavigateAbsoluteCommand; otherwise, <c>false</c>.</returns>
        protected virtual bool CanNavigateCommandExecute(string uri)
        {
            return !string.IsNullOrEmpty(uri);
        }

        /// <summary>
        /// Determines whether this instance can execute the NavigateModalCommand.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns><c>true</c> if this instance can execute NavigateModalCommand; otherwise, <c>false</c>.</returns>
        protected virtual bool CanNavigateModalCommandExecute(string uri)
        {
            return !string.IsNullOrEmpty(uri);
        }

        /// <summary>
        /// Determines whether this instance can execute the NavigateNonModalCommand.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns><c>true</c> if this instance can execute NavigateNonModalCommand; otherwise, <c>false</c>.</returns>
        protected virtual bool CanNavigateNonModalCommandExecute(string uri)
        {
            return !string.IsNullOrEmpty(uri);
        }

        /// <summary>
        /// Navigates back by invoking NavigationService.GoBackAsync.
        /// </summary>
        /// <returns>Task.</returns>
        protected virtual async Task GoBack()
        {
            try
            {
                await this.NavigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the exception by clearing the IsBusy flag, then displaying an alert with the base exception message.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>Task.</returns>
        protected virtual Task HandleException(Exception ex)
        {
            ClearIsBusy();
            var baseException = ex.GetBaseException();

            return this.PageDialogService.DisplayAlertAsync(CaptionError, baseException.Message, ButtonTextOK);
        }
        
        /// <summary>
        /// Navigates to the uri after creating a new navigation root. (Effectively replacing the Application MainPage.)
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <returns>Task.</returns>
        protected virtual async Task NavigateAbsoluteCommandExecute(string uri)
        {
            try
            {
                if (CanNavigateAbsoluteCommandExecute(uri))
                {
                    SetIsBusy();
                    await NavigateToNewRootUri(uri);
                }
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
            finally
            {
                ClearIsBusy();
            }
        }

        /// <summary>
        /// Navigates to the uri.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <returns>Task.</returns>
        protected virtual async Task NavigateCommandExecute(string uri)
        {
            try
            {
                if (CanNavigateCommandExecute(uri))
                {
                    SetIsBusy();
                    await NavigateToUri(uri);
                }
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
            finally
            {
                ClearIsBusy();
            }
        }

        /// <summary>
        /// Navigates to the uri using a Modal navigation.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <returns>Task.</returns>
        protected virtual async Task NavigateModalCommandExecute(string uri)
        {
            try
            {
                if (CanNavigateCommandExecute(uri))
                {
                    SetIsBusy();
                    await NavigateModalToUri(uri);
                }
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
            finally
            {
                ClearIsBusy();
            }
        }

        /// <summary>
        /// Navigates to the uri using a Modal navigation. The parameter is wrapped in NavigationParameters and the parameter Key is set to Key.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ArgumentException">The uri cannot be null or white space.</exception>
        /// <exception cref="System.ArgumentNullException">The parameter was null.</exception>
        protected async Task NavigateModalToUri(string uri, object parameter)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(uri));
            }

            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            try
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add(ParameterKey, parameter);
                await NavigateModalToUri(uri, navigationParameters);
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
        }

        /// <summary>
        /// Navigates to the uri using a Modal navigation.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <param name="navigationParameters">The navigation parameters.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ArgumentException">The uri cannot be null or white space.</exception>
        /// <exception cref="System.ArgumentNullException">The navigationParameters were null.</exception>
        protected async Task NavigateModalToUri(string uri, INavigationParameters navigationParameters)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(uri));
            }

            if (navigationParameters == null)
            {
                throw new ArgumentNullException(nameof(navigationParameters));
            }

            try
            {
                await this.NavigationService.NavigateAsync(uri, navigationParameters/*, true*/);
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
        }

        /// <summary>
        /// Navigates to the uri using a Modal navigation.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ArgumentException">The uri cannot be null or white space.</exception>
        protected async Task NavigateModalToUri(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(uri));
            }

            try
            {
                await this.NavigationService.NavigateAsync(uri, useModalNavigation: true);
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
        }

        /// <summary>
        /// Navigates to the uri using Non-Modal navigation.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <returns>Task.</returns>
        protected virtual async Task NavigateNonModalCommandExecute(string uri)
        {
            try
            {
                if (CanNavigateCommandExecute(uri))
                {
                    SetIsBusy();
                    await this.NavigationService.NavigateAsync(uri, null/*, false*/);
                }
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
            finally
            {
                ClearIsBusy();
            }
        }

        /// <summary>
        /// Navigates to new root uri.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ArgumentException">The uri cannot be null or white space.</exception>
        protected async Task NavigateToNewRootUri(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(uri));
            }

            await NavigateToNewRootUriInternal(uri);
        }

        /// <summary>
        /// Navigates to new root uri. The parameter is wrapped in NavigationParameters and the parameter Key is set to Key.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ArgumentException">The uri cannot be null or white space.</exception>
        /// <exception cref="System.ArgumentNullException">The parameter was null.</exception>
        protected async Task NavigateToNewRootUri(string uri, object parameter)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(uri));
            }

            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var navigationParameters = new NavigationParameters();
            navigationParameters.Add(ParameterKey, parameter);
            await NavigateToNewRootUriInternal(uri, navigationParameters);
        }

        /// <summary>
        /// Navigates to the uri after creating a new navigation root. (Effectively replacing the Application MainPage.)
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <param name="navigationParameters">The navigation parameters.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ArgumentException">The uri cannot be null or white space.</exception>
        /// <exception cref="System.ArgumentNullException">The navigationParameters were null.</exception>
        protected async Task NavigateToNewRootUri(string uri, INavigationParameters navigationParameters)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(uri));
            }

            if (navigationParameters == null)
            {
                throw new ArgumentNullException(nameof(navigationParameters));
            }

            await NavigateToNewRootUriInternal(uri, navigationParameters);
        }

        async Task NavigateToNewRootUriInternal(string uri, INavigationParameters navigationParameters = null)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(uri));
            }

            try
            {
                if (!uri.StartsWith(RootUriPrependText))
                {
                    uri = string.Concat(RootUriPrependText, uri);
                }

                await this.NavigationService.NavigateAsync(uri, navigationParameters/*, false*/);
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
        }

        /// <summary>
        /// Navigates to uri.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ArgumentException">The uri cannot be null or white space.</exception>
        protected async Task NavigateToUri(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(uri));
            }

            try
            {
                await this.NavigationService.NavigateAsync(uri, useModalNavigation: false);
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
        }

        /// <summary>
        /// Navigates to uri. The parameter is wrapped in NavigationParameters and the parameter Key is set to Key.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ArgumentException">The uri cannot be null or white space.</exception>
        /// <exception cref="System.ArgumentNullException">The parameter was null.</exception>
        protected async Task NavigateToUri(string uri, object parameter)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(uri));
            }

            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            try
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add(ParameterKey, parameter);
                await NavigateToUri(uri, navigationParameters);
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
        }

        /// <summary>
        /// Navigates to uri.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <param name="navigationParameters">The navigation parameters.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ArgumentException">The uri cannot be null or white space.</exception>
        /// <exception cref="System.ArgumentNullException">The navigationParameters are null.</exception>
        protected async Task NavigateToUri(string uri, INavigationParameters navigationParameters)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(uri));
            }

            if (navigationParameters == null)
            {
                throw new ArgumentNullException(nameof(navigationParameters));
            }

            try
            {
                await NavigationService.NavigateAsync(uri, navigationParameters/*, false*/);
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
        }

        #endregion  Navigation Methods & Overloads

        #endregion NavigationStack Commands

        #region Display Dialog

        /// <summary>
        /// Displays an alert dialog.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="buttonText">The button text.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ArgumentException">The title or message or buttonText was null or white space.</exception>
        protected async Task DisplayDialog(string title, string message, string buttonText = ButtonTextOK)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(message));
            }

            if (string.IsNullOrWhiteSpace(buttonText))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(buttonText));
            }

            try
            {
                await this.PageDialogService.DisplayAlertAsync(title, message, buttonText);
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
        }

        /// <summary>
        /// Displays an alert dialog.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="acceptButtonText">The accept button text.</param>
        /// <param name="cancellationButtonText">The cancellation button text.</param>
        /// <returns>Task&lt;bool&gt;.</returns>
        /// <exception cref="System.ArgumentException">The title or message or acceptButtonText or cancellationButtonText was null or white space.</exception>
        protected async Task<bool> DisplayDialog(string title, string message, string acceptButtonText, string cancellationButtonText)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(title));
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(message));
            }
            if (string.IsNullOrWhiteSpace(acceptButtonText))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(acceptButtonText));
            }
            if (string.IsNullOrWhiteSpace(cancellationButtonText))
            {
                throw new ArgumentException("Value cannot be null or white space.", nameof(cancellationButtonText));
            }
            try
            {
                return await this.PageDialogService.DisplayAlertAsync(title, message, acceptButtonText, cancellationButtonText);
            }
            catch (Exception ex)
            {
                await HandleException(ex);
                return false;
            }
        }

        #endregion Display Dialog
        
        #region Page-Level Properties

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Icon { get; set; }

        public bool CanLoadMore { get; set; }

        public string Header { get; set; }

        public string Footer { get; set; }

        public static string OrganizationZ { get; set; }

        public static string NameZ { get; set; }
        
        #endregion Page-Level Properties
        
        #region IsBusy Flag implementation

        private bool _isBusy;
        /// <summary>
        /// Gets or sets the is busy.
        /// </summary>
        /// <value>The is busy.</value>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                SetProperty(ref _isBusy, value);
                OnIsBusyChanged();
            }
        }

        /// <summary>
        /// Invoked when IsBusy changes.
        /// </summary>
        protected virtual void OnIsBusyChanged()
        {
        }
        
        /// <summary>
        /// Clears the is busy flag on the device main thread.
        /// </summary>
        protected void ClearIsBusy()
        {
            this.DeviceService.BeginInvokeOnMainThread(() => this.IsBusy = false);
        }

        /// <summary>
        /// Sets the is busy flag on the device main thread.
        /// </summary>
        protected void SetIsBusy()
        {
            this.DeviceService.BeginInvokeOnMainThread(() => this.IsBusy = true);
        }

        #endregion IsBusy Flag implementation

        protected NavigationViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService, IDeviceService deviceService) 
            : base(navigationService, pageDialogService, deviceService) { }
    }
}

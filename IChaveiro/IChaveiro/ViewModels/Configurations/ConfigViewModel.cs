using System;
using System.Threading.Tasks;
using IChaveiro.ModelRealm;
using IChaveiro.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace IChaveiro.ViewModels.Configurations
{
    public class ConfigViewModel : ViewModelTabbedBase
    {
        protected ConfigViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region Fields
        private bool _status;
        public bool Status
        {
            get => _status;
            set
            {
                SetProperty(ref _status, value);
                if (HasInitialized)
                {
                    ChangeLockSmithStatus();
                }
            }
        }
        #endregion

        public override void Initialize(INavigationParameters parameters)
        {
            GetLockSmithStatus();
        }

        private DelegateCommand aboutCommand;
        public DelegateCommand AboutCommand => aboutCommand ??= new DelegateCommand(async () => await Navigation("AboutView"), () => !IsBusy);

        private DelegateCommand profileCommand;
        public DelegateCommand ProfileCommand => profileCommand ??= new DelegateCommand(async () => await Navigation("ProfileView"), () => !IsBusy);

        private DelegateCommand addressCommand;
        public DelegateCommand AddressCommand => addressCommand ??= new DelegateCommand(async () => await Navigation("AddressView"), () => !IsBusy);

        private DelegateCommand logoutCommand;
        public DelegateCommand LogoutCommand => logoutCommand ??= new DelegateCommand(async () => await LogoutAsync(), () => !IsBusy);

        private async Task LogoutAsync()
        {
            try
            {
                IsBusy = true;
                new ReloadRealm().Logout();
                await NavigationService.NavigateAsync("/NavigationPage/LoginView");
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void GetLockSmithStatus()
        {
            try
            {
                IsBusy = true;
                Status = await Utils.GetApi().GetStatusLockSmith();
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            finally
            {
                HasInitialized = true;
                IsBusy = false;
            }
        }

        private async void ChangeLockSmithStatus()
        {
            try
            {
                IsBusy = true;
                bool value = await Utils.GetApi().ChangeStatusLockSmith();
                if (value != Status)
                {
                    Status = value;
                }
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
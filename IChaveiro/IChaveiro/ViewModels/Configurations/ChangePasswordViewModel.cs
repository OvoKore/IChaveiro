using System;
using System.Threading.Tasks;
using IChaveiro.DTO;
using IChaveiro.Validators;
using IChaveiro.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace IChaveiro.ViewModels.Configurations
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        protected ChangePasswordViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region Fields
        private string _currentPassword;
        public string CurrentPassword
        {
            get => _currentPassword;
            set => SetProperty(ref _currentPassword, value);
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }
        #endregion

        private DelegateCommand changePasswordCommand;
        public DelegateCommand ChangePasswordCommand => changePasswordCommand ??= new DelegateCommand(async () => await ChangePasswordAsync(), () => !IsBusy);

        private async Task ChangePasswordAsync()
        {
            try
            {
                IsBusy = true;
                LocksmithPassword validator = new LocksmithPassword()
                {
                    current_password = CurrentPassword,
                    new_password = NewPassword,
                    confirm_password = ConfirmPassword
                };
                var resultValidation = new LocksmithPasswordValidator().Validate(validator);
                if (resultValidation.IsValid)
                {
                    ChangePasswordDTO changePasswordDTO = new ChangePasswordDTO()
                    {
                        old_password = Utils.Crypt(CurrentPassword),
                        new_password = Utils.Crypt(NewPassword)
                    };
                    MsgDTO apiResposta = await Utils.GetApi().ChangePassword(changePasswordDTO);
                    if (apiResposta.msg == MsgDTO.SUCESS)
                    {
                        await PageDialogService.DisplayAlertAsync("Update", "Password changed", "OK");
                        await NavigationService.GoBackAsync();
                    }
                    else
                    {
                        await PageDialogService.DisplayAlertAsync("Erro", apiResposta.msg, "OK");
                    }
                }
                else
                {
                    await PageDialogService.DisplayAlertAsync("Required fields", string.Join("\n", resultValidation.Errors), "OK");
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
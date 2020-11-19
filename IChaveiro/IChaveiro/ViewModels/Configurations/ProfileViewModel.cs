using System;
using System.Threading.Tasks;
using IChaveiro.Models;
using IChaveiro.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using IChaveiro.Validators;
using IChaveiro.DTO;

namespace IChaveiro.ViewModels.Configurations
{
    public class ProfileViewModel : ViewModelBase
    {
        protected ProfileViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region Fields
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _cell_phone;
        public string CellPhone
        {
            get => _cell_phone;
            set => SetProperty(ref _cell_phone, value);
        }

        private string _cnpj;
        public string Cnpj
        {
            get => _cnpj;
            set => SetProperty(ref _cnpj, value);
        }

        private string _company_name;
        public string CompanyName
        {
            get => _company_name;
            set => SetProperty(ref _company_name, value);
        }

        private string _state_registration;
        public string StateRegistration
        {
            get => _state_registration;
            set => SetProperty(ref _state_registration, value);
        }
        #endregion

        public override void Initialize(INavigationParameters parameters)
        {
            GetLockSmith();
            HasInitialized = true;
        }

        public async void GetLockSmith()
        {
            try
            {
                IsBusy = true;
                var apiRetorno = await Utils.GetApi().GetLockSmith();
                CompanyName = apiRetorno.company_name;
                Email = apiRetorno.email;
                CellPhone = apiRetorno.cell_phone;
                Cnpj = apiRetorno.cnpj;
                StateRegistration = apiRetorno.state_registration ?? string.Empty;
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

        private DelegateCommand changePasswordCommand;
        public DelegateCommand ChangePasswordCommand => changePasswordCommand ??= new DelegateCommand(async () => await Navigation("ChangePasswordView"), () => !IsBusy);

        private DelegateCommand updateCommand;
        public DelegateCommand UpdateCommand => updateCommand ??= new DelegateCommand(async () => await UpdateAsync(), () => !IsBusy);

        private async Task UpdateAsync()
        {
            try
            {
                IsBusy = true;
                SmallLocksmithVal _locksmith = new SmallLocksmithVal()
                {
                    company_name = CompanyName,
                    cell_phone = CellPhone,
                    email = Email,
                    cnpj = Cnpj,
                    state_registration = StateRegistration
                };
                var resultValidation = new SmallLocksmithValidator().Validate(_locksmith);
                if (resultValidation.IsValid)
                {
                    Locksmith locksmith = new Locksmith() {
                        company_name = CompanyName,
                        cell_phone = CellPhone.Replace("(","").Replace(")","").Replace("-",""),
                        email = Email.ToLower(),
                        cnpj = Cnpj.Replace(".", "").Replace("-", "").Replace("/", ""),
                        state_registration = StateRegistration
                    };
                    MsgDTO apiRetorno = await Utils.GetApi().UpdateLocksmith(locksmith);
                    if (apiRetorno.msg == MsgDTO.SUCESS)
                    {
                        await PageDialogService.DisplayAlertAsync("Update", "User updated!", "OK");
                        await NavigationService.GoBackAsync();
                    }
                    else
                    {
                        await PageDialogService.DisplayAlertAsync("Erro", apiRetorno.msg, "OK");
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
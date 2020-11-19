using System;
using System.Threading.Tasks;
using IChaveiro.DTO;
using IChaveiro.ModelRealm;
using IChaveiro.Models;
using IChaveiro.Services;
using IChaveiro.Validators;
using IChaveiro.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using Refit;

namespace IChaveiro.ViewModels.LoginSignUp
{
    public class SignUpViewModel : ViewModelBase
    {
        protected SignUpViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region Fields
        private string _companyName;
        public string CompanyName
        {
            get => _companyName;
            set => SetProperty(ref _companyName, value);
        }

        private string _cnpj;
        public string Cnpj
        {
            get => _cnpj;
            set => SetProperty(ref _cnpj, value);
        }

        private string _cellphone;
        public string Cellphone
        {
            get => _cellphone;
            set => SetProperty(ref _cellphone, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _confirmEmail;
        public string ConfirmEmail
        {
            get => _confirmEmail;
            set => SetProperty(ref _confirmEmail, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private string _stateRegistration;
        public string StateRegistration
        {
            get => _stateRegistration;
            set => SetProperty(ref _stateRegistration, value);
        }
        #endregion

        private DelegateCommand registerCommand;
        public DelegateCommand RegisterCommand => registerCommand ??= new DelegateCommand(async () => await RegisterAsync(), () => !IsBusy);

        private async Task RegisterAsync()
        {
            try
            {
                IsBusy = true;
                LocksmithVal _locksmith = new LocksmithVal()
                {
                    company_name = CompanyName,
                    password = Password,
                    confirm_password = ConfirmPassword,
                    cell_phone = Cellphone,
                    email = Email,
                    confirm_email = ConfirmEmail,
                    cnpj = Cnpj,
                    state_registration = StateRegistration
                };
                var resultValidation = new LocksmithValidator().Validate(_locksmith);
                if (resultValidation.IsValid)
                {
                    var usuarioAPI = RestService.For<IRestApi>(EndPoints.BaseUrl);
                    Locksmith locksmith = new Locksmith(_locksmith);
                    try
                    {
                        var usuariosRetorno = await usuarioAPI.Create(locksmith);
                        if (usuariosRetorno.msg == MsgDTO.SUCESS)
                        {
                            var loginRetorno = await usuarioAPI.Login(locksmith);
                            if (loginRetorno.msg == MsgDTO.SUCESS)
                            {
                                var realm = Realm.GetInstance();
                                realm.Write(() =>
                                {
                                    realm.RemoveAll<TokenRealm>();
                                    realm.Add(new TokenRealm(loginRetorno));
                                });
                                await PageDialogService.DisplayAlertAsync(string.Empty, "Registration completed", "OK");
                                await NavigationService.NavigateAsync($"/NavigationPage/MenuView");
                            }
                        }
                    }
                    catch (ApiException ex)
                    {
                        await PageDialogService.DisplayAlertAsync(ex.StatusCode.ToString(), ex.GetContentAsAsync<MsgDTO>().Result.msg, "OK");
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
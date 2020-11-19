using System;
using System.Linq;
using System.Threading.Tasks;
using IChaveiro.DTO;
using IChaveiro.ModelRealm;
using IChaveiro.Models;
using IChaveiro.Services;
using IChaveiro.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using Refit;

namespace IChaveiro.ViewModels.LoginSignUp
{
    public class LoginViewModel : ViewModelBase
    {
        protected LoginViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region Fields
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        #endregion

        public async override void Initialize(INavigationParameters parameters)
        {
            Realm realm = Realm.GetInstance();
            if (realm.All<TokenRealm>().Count() > 0)
            {
                TokenRealm token = realm.All<TokenRealm>().First();
                int valid_refresh_token = DateTimeOffset.Compare(token.ExpiresRefresh, DateTime.Now);
                if (valid_refresh_token >= 0)
                {
                    TokenDTO apiRetorno = await Utils.GetApi(type_token: "RefreshToken").RefreshToken();
                    if (apiRetorno.msg == MsgDTO.SUCESS)
                    {
                        realm.Write(() =>
                        {
                            realm.RemoveAll<TokenRealm>();
                            realm.Add(new TokenRealm(apiRetorno));
                        });
                        await NavigationService.NavigateAsync("/NavigationPage/MenuView");
                    }
                    else
                    {
                        await NavigationService.NavigateAsync("/NavigationPage/LoginView");
                    }
                }
                else
                {
                    await NavigationService.NavigateAsync("/NavigationPage/LoginView");
                }
            }
        }

        private DelegateCommand loginCommand;
        public DelegateCommand LoginCommand => loginCommand ??= new DelegateCommand(async () => await LoginAsync(), () => !IsBusy);

        private async Task LoginAsync()
        {
            try
            {
                IsBusy = true;
                if (Utils.EmailValidator(Email) && Utils.PasswordValidator(Password))
                {
                    Locksmith locksmith = new Locksmith
                    {
                        email = Email.ToLower(),
                        password = Utils.Crypt(Password)
                    };
                    var usuarioAPI = RestService.For<IRestApi>(EndPoints.BaseUrl);
                    try
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
                            await NavigationService.NavigateAsync("/MenuView");
                        }
                    }
                    catch (ApiException ex)
                    {
                        await PageDialogService.DisplayAlertAsync(ex.StatusCode.ToString(), ex.GetContentAsAsync<MsgDTO>().Result.msg, "OK");
                    }
                }
                else
                {
                    await PageDialogService.DisplayAlertAsync("Erro", "Invalid Email or Password.", "OK");
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

        private DelegateCommand signUpCommand;
        public DelegateCommand SignUpCommand => signUpCommand ??= new DelegateCommand(async () => await Navigation("SignUpView"), () => !IsBusy);
    }
}
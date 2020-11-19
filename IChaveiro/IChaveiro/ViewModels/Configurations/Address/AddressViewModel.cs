using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using IChaveiro.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using IChaveiro.ModelRealm;
using IChaveiro.DTO;

namespace IChaveiro.ViewModels.Configurations.Address
{
    using Address = Models.Address;

    public class AddressViewModel : ViewModelBase
    {
        protected AddressViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region Fields
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private bool _main;
        public bool Main
        {
            get => _main;
            set => SetProperty(ref _main, value);
        }

        private string _cep;
        public string Cep
        {
            get => _cep;
            set => SetProperty(ref _cep, value);
        }

        private string _uf;
        public string Uf
        {
            get => _uf;
            set => SetProperty(ref _uf, value);
        }

        private string _cidade;
        public string Cidade
        {
            get => _cidade;
            set => SetProperty(ref _cidade, value);
        }

        private string _bairro;
        public string Bairro
        {
            get => _bairro;
            set => SetProperty(ref _bairro, value);
        }

        private string _logradouro;
        public string Logradouro
        {
            get => _logradouro;
            set => SetProperty(ref _logradouro, value);
        }

        private string _numero;
        public string Numero
        {
            get => _numero;
            set => SetProperty(ref _numero, value);
        }

        private string _complemento;
        public string Complemento
        {
            get => _complemento;
            set => SetProperty(ref _complemento, value);
        }
        #endregion

        public ObservableCollection<Address> ListAddress { get; private set; } = new ObservableCollection<Address>();

        private DelegateCommand newAddressCommand;
        public DelegateCommand NewAddressCommand => newAddressCommand ??= new DelegateCommand(async () => await Navigation("NewAddressView"), () => !IsBusy);

        public override void Initialize(INavigationParameters parameters)
        {
            GetAddressList();
            HasInitialized = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (HasInitialized)
            {
                var reload = new ReloadRealm("GetAddressList");
                if (reload.Get().Count() != 0)
                {
                    GetAddressList();
                    reload.Remove();
                }
            }
        }

        public override void RefreshAsync()
        {
            GetAddressList();
            IsRefreshing = false;
        }

        public async void GetAddressList()
        {
            try
            {
                IsBusy = true;
                var apiRetorno = await Utils.GetApi().GetAddressList();
                ListAddress.Clear();
                foreach (Address a in apiRetorno)
                {
                    if (a.main)
                    {
                        ListAddress.Insert(0, a);
                    }
                    else
                    {
                        ListAddress.Add(a);
                    }
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

        private DelegateCommand<Address> editAddressCommand;
        public DelegateCommand<Address> EditAddressCommand => editAddressCommand ??= new DelegateCommand<Address>(async (address) => await EditAddressAsync(address), (address) => !IsBusy);

        private async Task EditAddressAsync(Address address)
        {
            try
            {
                var navigationParams = new NavigationParameters
                {
                    { "Id", address.id },
                    { "Name", address.name },
                    { "Main", address.main },
                    { "Cep", address.cep },
                    { "Uf", address.uf },
                    { "Cidade", address.cidade },
                    { "Bairro", address.bairro },
                    { "Logradouro", address.logradouro },
                    { "Numero", address.numero },
                    { "Complemento", address.complemento }
                };
                await NavigationWithParameters("NewAddressView", navigationParams);
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                GetAddressList();
            }
        }

        private DelegateCommand<Address> deleteAddressCommand;
        public DelegateCommand<Address> DeleteAddressCommand => deleteAddressCommand ??= new DelegateCommand<Address>(async (address) => await DeleteAddressAsync(address), (address) => !IsBusy);

        private async Task DeleteAddressAsync(Address address)
        {
            try
            {
                var result = await PageDialogService.DisplayAlertAsync("CONFIRM", "Do you really want to delete?", "Yes", "No");
                if (result)
                {
                    MsgDTO apiRetorno = await Utils.GetApi().DeleteAddress(address);
                    if (apiRetorno.msg == MsgDTO.SUCESS)
                    {
                        await PageDialogService.DisplayAlertAsync("DELETE", "Successfully deleted!", "OK");
                    }
                    else
                    {
                        await PageDialogService.DisplayAlertAsync("FAILED", "Failed to delete!", "OK");
                    }
                    GetAddressList();
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
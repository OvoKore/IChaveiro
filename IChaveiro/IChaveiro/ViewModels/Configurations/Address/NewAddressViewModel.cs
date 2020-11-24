using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IChaveiro.DTO;
using IChaveiro.ModelRealm;
using IChaveiro.Services;
using IChaveiro.Validators;
using IChaveiro.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Refit;

namespace IChaveiro.ViewModels.Configurations.Address
{
    using Address = Models.Address;

    public class NewAddressViewModel : ViewModelBase
    {
        protected NewAddressViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region Fields
        private bool _back = false;
        public bool Back
        {
            get => _back;
            set => SetProperty(ref _back, value);
        }

        private List<StateDTO> _allEstados = Utils.GetStates();
        public List<StateDTO> AllEstados
        {
            get => _allEstados;
            set => SetProperty(ref _allEstados, value);
        }
        private List<CityDTO> _allCidades = Utils.GetCities();
        public List<CityDTO> AllCidades
        {
            get => _allCidades;
            set => SetProperty(ref _allCidades, value);
        }

        private List<CityDTO> _listCidade = new List<CityDTO>();
        public List<CityDTO> ListCidade
        {
            get => _listCidade;
            set => SetProperty(ref _listCidade, value);
        }

        private StateDTO _state = null;
        public StateDTO State
        {
            get => _state;
            set
            {
                SetProperty(ref _state, value);
                if (!Back)
                {
                    ListCidade = AllCidades.Where(c => c.State == value.ID).ToList<CityDTO>();
                }
                City = null;
            }
        }
        private CityDTO _city = null;
        public CityDTO City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

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

        private bool _enderecoHabilitado = true;
        public bool EnderecoHabilitado
        {
            get => _enderecoHabilitado;
            set => SetProperty(ref _enderecoHabilitado, value);
        }
        #endregion

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.Count() != 0)
            {
                Id = parameters.GetValue<int>("Id");
                Name = parameters.GetValue<string>("Name");
                Main = parameters.GetValue<bool>("Main");
                Cep = parameters.GetValue<string>("Cep");
                Uf = parameters.GetValue<string>("Uf");
                Cidade = parameters.GetValue<string>("Cidade");
                Bairro = parameters.GetValue<string>("Bairro");
                Logradouro = parameters.GetValue<string>("Logradouro");
                Numero = parameters.GetValue<string>("Numero");
                Complemento = parameters.GetValue<string>("Complemento");
                State = AllEstados.Where(x => x.StateAbbreviation == Uf).First();
                City = AllCidades.Where(x => x.State == State.ID && x.Name == Cidade).First();
                EnderecoHabilitado = false;
            }
            else
            {
                Main = true;
            }
            HasInitialized = true;
        }

        public override void Destroy()
        {
            Back = true;
        }

        private DelegateCommand consultarCepCommand;
        public DelegateCommand ConsultarCepCommand => consultarCepCommand ??= new DelegateCommand(async () => await ConsultarCepAsync(), () => !IsBusy);

        private async Task ConsultarCepAsync()
        {
            try
            {
                IsBusy = true;
                CepDTO cepResposta = await RestService.For<IRestApi>(EndPoints.CepUrl).Cep(Cep);
                if (cepResposta != null)
                {
                    State = AllEstados.Where(e => e.StateAbbreviation == cepResposta.uf).First();
                    ListCidade = AllCidades.Where(c => c.State == State.ID).ToList();
                    City = ListCidade.Where(c => c.Name == cepResposta.localidade).First();
                    Bairro = cepResposta.bairro;
                    Logradouro = cepResposta.logradouro;
                    EnderecoHabilitado = false;
                }
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
                EnderecoHabilitado = true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private DelegateCommand registerCommand;
        public DelegateCommand RegisterCommand => registerCommand ??= new DelegateCommand(async () => await RegisterAsync(), () => !IsBusy);

        private async Task RegisterAsync()
        {
            try
            {
                IsBusy = true;
                AddressVal address = new AddressVal()
                {
                    name = Name,
                    cep = Cep,
                    uf = State.StateAbbreviation,
                    cidade = City.Name,
                    bairro = Bairro,
                    logradouro = Logradouro,
                    numero = Numero,
                    complemento = Complemento
                };
                var resultValidation = new AddressValidator().Validate(address);
                if (resultValidation.IsValid)
                {
                    var api = RestService.For<IRestApi>(EndPoints.BaseUrl, Utils.GetRefitSettings());
                    Address _address = new Address(address)
                    {
                        main = Main
                    };
                    try
                    {
                        if (Id == 0)
                        {
                            var apiRetorno = await api.AddAddress(_address);
                            if (apiRetorno.msg == MsgDTO.SUCESS)
                            {
                                await PageDialogService.DisplayAlertAsync("ADD", "Successfully added!", "OK");
                            }
                        }
                        else
                        {
                            _address.id = Id;
                            var apiRetorno = await api.UpdateAddress(_address);
                            if (apiRetorno.msg == MsgDTO.SUCESS)
                            {
                                await PageDialogService.DisplayAlertAsync("UPDATE", "Updated successfully!", "OK");
                            }
                        }
                        new ReloadRealm("GetAddressList").Add();
                        await NavigationService.GoBackAsync();
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
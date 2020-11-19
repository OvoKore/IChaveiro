using System;
using System.Linq;
using System.Threading.Tasks;
using IChaveiro.DTO;
using IChaveiro.ModelRealm;
using IChaveiro.Models;
using IChaveiro.Validators;
using IChaveiro.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Refit;

namespace IChaveiro.ViewModels.Services
{
    public class NewServicesViewModel : ViewModelTabbedBase
    {
        protected NewServicesViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
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

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private double _lowPrice;
        public double LowPrice
        {
            get => _lowPrice;
            set => SetProperty(ref _lowPrice, value);
        }

        private double _highPrice;
        public double HighPrice
        {
            get => _highPrice;
            set => SetProperty(ref _highPrice, value);
        }
        #endregion

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.Count() != 0)
            {
                Id = parameters.GetValue<int>("Id");
                Name = parameters.GetValue<string>("Name");
                Description = parameters.GetValue<string>("Description");
                LowPrice = parameters.GetValue<double>("LowPrice");
                HighPrice = parameters.GetValue<double>("HighPrice");
            }
        }

        private DelegateCommand registerCommand;
        public DelegateCommand RegisterCommand => registerCommand ??= new DelegateCommand(async () => await RegisterAsync(), () => !IsBusy);

        private async Task RegisterAsync()
        {
            try
            {
                IsBusy = true;
                ServiceVal service = new ServiceVal()
                {
                    name = Name,
                    description = Description,
                    low_price = LowPrice,
                    high_price = HighPrice
                };
                var resultValidation = new ServiceValidator().Validate(service);
                if (resultValidation.IsValid)
                {
                    var api = Utils.GetApi();
                    Service _service = new Service(service);
                    try
                    {
                        if (Id == 0)
                        {
                            var apiRetorno = await api.AddService(_service);
                            if (apiRetorno.msg == MsgDTO.SUCESS)
                            {
                                await PageDialogService.DisplayAlertAsync("ADD", "Successfully added!", "OK");
                            }
                        }
                        else
                        {
                            _service.id = Id;
                            var apiRetorno = await api.UpdateService(_service);
                            if (apiRetorno.msg == MsgDTO.SUCESS)
                            {
                                await PageDialogService.DisplayAlertAsync("UPDATE", "Updated successfully!", "OK");
                            }
                        }

                        new ReloadRealm("GetServiceList").Add();
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
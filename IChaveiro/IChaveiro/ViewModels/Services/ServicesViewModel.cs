using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using IChaveiro.DTO;
using IChaveiro.ModelRealm;
using IChaveiro.Models;
using IChaveiro.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace IChaveiro.ViewModels.Services
{
    public class ServicesViewModel : ViewModelTabbedBase
    {
        protected ServicesViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        public ObservableCollection<Service> Services { get; private set; } = new ObservableCollection<Service>();

        private DelegateCommand newServiceCommand;
        public DelegateCommand NewServiceCommand => newServiceCommand ??= new DelegateCommand(async () => await Navigation("NewServiceView"), () => !IsBusy);

        public override void Initialize(INavigationParameters parameters)
        {
            GetServiceList();
            HasInitialized = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var reload = new ReloadRealm("GetServiceList");
            if (reload.Get().Count() != 0)
            {
                GetServiceList();
                reload.Remove();
            }
        }

        public async void GetServiceList()
        {
            try
            {
                IsBusy = true;
                var apiRetorno = await Utils.GetApi().GetServiceList();
                Services.Clear();
                foreach (Service s in apiRetorno)
                {
                    Services.Add(s);
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

        private DelegateCommand<Service> editServiceCommand;
        public DelegateCommand<Service> EditServiceCommand => editServiceCommand ??= new DelegateCommand<Service>(async (service) => await EditServiceAsync(service), (service) => !IsBusy);

        private async Task EditServiceAsync(Service service)
        {
            try
            {
                var navigationParams = new NavigationParameters
                {
                    { "Id", service.id },
                    { "Name", service.name },
                    { "Description", service.description },
                    { "LowPrice", service.low_price },
                    { "HighPrice", service.high_price }
                };
                await NavigationWithParameters("NewServiceView", navigationParams);
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                GetServiceList();
            }
        }
        
        private DelegateCommand<Service> deleteServiceCommand;
        public DelegateCommand<Service> DeleteServiceCommand => deleteServiceCommand ??= new DelegateCommand<Service>(async (service) => await DeleteServiceAsync(service), (service) => !IsBusy);

        private async Task DeleteServiceAsync(Service service)
        {
            try
            {
                var result = await PageDialogService.DisplayAlertAsync("CONFIRM", "Do you really want to delete?", "Yes", "No");
                if (result)
                {
                    MsgDTO apiRetorno = await Utils.GetApi().DeleteService(service);
                    if (apiRetorno.msg == MsgDTO.SUCESS)
                    {
                        await PageDialogService.DisplayAlertAsync("DELETE", "Successfully deleted!", "OK");
                    }
                    else
                    {
                        await PageDialogService.DisplayAlertAsync("FAILED", "Failed to delete!", "OK");
                    }
                    GetServiceList();
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
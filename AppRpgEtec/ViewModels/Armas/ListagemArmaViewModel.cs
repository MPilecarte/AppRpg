using AppRpgEtec.Models;
using AppRpgEtec.Services.Armas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Armas
{
    public class ListagemArmaViewModel : BaseViewModel
    {
        private ArmaService armaService;    

        public ObservableCollection<Models.Arma> Armas { get; set; }

        public ICommand NovaArma { get; }
        public ListagemArmaViewModel() 
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            armaService = new ArmaService(token);
            Armas = new ObservableCollection<Models.Arma>();

            _ = ObterArmas();

            NovaArma = new Command(async () => { await ExibirCadastroArma(); }) ;
        }

      

        public async Task ObterArmas()
        {
            try
            {
                Armas = await armaService.GetArmasAsync();
                OnPropertyChanged(nameof(Armas));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ExibirCadastroArma()
        {
            try
            {
                await Shell.Current.GoToAsync("cadArmaView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                  .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task RemoverArma(Arma a)
        {
            try
            {
                if (await Application.Current.MainPage.
                    DisplayAlert("Confirmação", $"Você deseja mesmo excluir {a.Nome}?", "Sim", "Não"))
                {
                    await armaService.DeleteArmaAsync(a.Id);

                    await Application.Current.MainPage.DisplayAlert("Mensagem",
                        "Arma removida com sucesso!", "Ok");

                    _ = ObterArmas();
                }
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage
                 .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}

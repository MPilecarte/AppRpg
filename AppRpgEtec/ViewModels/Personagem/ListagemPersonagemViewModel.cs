﻿using AppRpgEtec.Models;
using AppRpgEtec.Services.Personagens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels
{
    public class ListagemPersonagemViewModel : BaseViewModel
    {
        private PersonagemService pService;
        public ObservableCollection<AppRpgEtec.Models.Personagem> Personagens { get; set; }   

        public ICommand NovoPersonagem { get; }
        public ICommand RemoverPersonagemCommand { get; }


        public ListagemPersonagemViewModel() 
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);
            Personagens = new ObservableCollection<AppRpgEtec.Models.Personagem>();

            _ = ObterPersonagens();

            NovoPersonagem = new Command(async () => { await ExibirCadastroPersonagem(); });
            RemoverPersonagemCommand = new Command<Models.Personagem>(async (Models.Personagem p) => { await RemoverPersonagem(p);});
        }

        private AppRpgEtec.Models.Personagem personagemSelecionado;

        public AppRpgEtec.Models.Personagem PersonagemSelecionado
        {
            get { return PersonagemSelecionado;  }
            set
            {
                if (value != null)
                {
                    personagemSelecionado = value;

                    Shell.Current
                        .GoToAsync($"cadPersonagemView?pId={personagemSelecionado.Id}");
                }
            }
        }


        public async Task ObterPersonagens()
        {
            try
            {
                Personagens = await pService.GetPersonagensAsync();
                OnPropertyChanged(nameof(Personagens));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ExibirCadastroPersonagem()
        {
            try
            {
                await Shell.Current.GoToAsync("cadPersonagemView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                  .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task RemoverPersonagem(Models.Personagem p)
        {
            try
            {
                if (await Application.Current.MainPage
                    .DisplayAlert("Confirmação", $"Confirma a remoção de {p.Nome}?", "Sim", "Não"))
                {
                    await pService.DeletePersonagemAsync(p.Id);

                    await Application.Current.MainPage.DisplayAlert("Mensagem",
                        "Personagem removido com sucesso!", "Ok");

                    _ = ObterPersonagens();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                  .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }
        
    }
}

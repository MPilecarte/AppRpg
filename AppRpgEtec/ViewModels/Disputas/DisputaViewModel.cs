using AppRpgEtec.Models;
using AppRpgEtec.Services.Personagens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Disputas
{
    public class DisputaViewModel : BaseViewModel
    {
        private PersonagemService pService;
        public ObservableCollection<Models.Personagem> PersonagensEncontrados { get; set; }    
        public Models.Personagem Atacante { get; set; }
        public Models.Personagem Oponete { get; set; }

       
        public DisputaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);

            Atacante = new Models.Personagem();
            Oponete = new Models.Personagem();

            PesquisarPersonagensCommand = new Command<string>(async(string pesquisa) => { await PesquisarPersonagens(pesquisa); }); 
        }

        public ICommand PesquisarPersonagensCommand { get; set; }

        #region Atributos e Propriedades

        public string DescricaoPersonagemAtacante
        {
            get => Atacante.Nome;
        }

        public string DescricaoPersonagemOponente
        {
            get => Oponete.Nome;
        }

        private Models.Personagem personagemSelecionado;

        public Models.Personagem PersonagemSelecionado
        {
            set
            {
                if(value != null)
                {
                    personagemSelecionado = value;
                    SelecionarPersonagem(personagemSelecionado);
                    OnPropertyChanged();
                    PersonagensEncontrados.Clear(); 
                }
            }
        }

        #endregion

        #region Metodos

        public async Task PesquisarPersonagens(string textoPesquisarPersonagem)
        {
            try
            {
                PersonagensEncontrados = await pService.GetPersonagensByNomeAsync(textoPesquisarPersonagem);
                OnPropertyChanged(nameof(PersonagensEncontrados));  
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                 .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }


        public async void SelecionarPersonagem(Models.Personagem p)
        {
            try
            {
                string tipoCombatente = await Application.Current.MainPage
                    .DisplayActionSheet("Atacante ou Oponete?", "Cancelar", "", "Atacante", "Oponete");

                if(tipoCombatente == "Atacante")
                {
                    Atacante = p;
                    OnPropertyChanged(nameof(DescricaoPersonagemAtacante));
                }
                else if (tipoCombatente == "Oponete")
                {
                    Oponete = p;
                    OnPropertyChanged(nameof(DescricaoPersonagemOponente));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                 .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        #endregion
    }
}

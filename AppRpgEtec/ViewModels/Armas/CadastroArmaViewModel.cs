﻿using AppRpgEtec.Models;
using AppRpgEtec.Services.Armas;
using AppRpgEtec.Services.Personagens;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Armas
{    
    public class CadastroArmaViewModel : BaseViewModel
    {
        private ArmaService aService;
        private PersonagemService pService;

        public CadastroArmaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ArmaService(token);
            pService = new PersonagemService(token);

            ObterPersonagens();

            //SalvarCommand = new Command(async () => await SalvarArma());
            SalvarCommand = new Command(SalvarArma);
        }

        public ICommand SalvarCommand { get; set; }



        #region Atributos_Propriedades

        private int id;
        private string nome;
        private int dano;
        private int personagemId;

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public string Nome
        {
            get => nome;
            set
            {
                nome = value;
                OnPropertyChanged();
            }
        }
        public int Dano
        {
            get => dano;
            set
            {
                dano = value;
                OnPropertyChanged();

            }
        }
        public int PersonagemId
        {
            get => personagemId;
            set
            {
                personagemId = value;
                OnPropertyChanged();
            }
        }

        private Models.Personagem personagemSelecionado;
        public Models.Personagem PersonagemSelecionado
        {
            get { return personagemSelecionado; }
            set
            {
                if (value != null)
                {
                    personagemSelecionado = value;
                    OnPropertyChanged();
                }
            }
        }
        

        public ObservableCollection<Models.Personagem> Personagens { get; set; }

        #endregion

        #region Metodos



        public async void ObterPersonagens()
        {
            try
            {
                Personagens = await pService.GetPersonagensAsync();
                OnPropertyChanged(nameof(Personagens));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        public async void SalvarArma()
        {
            try
            {
                Arma model = new Arma()
                {
                    Id = this.id,
                    Nome = this.nome,
                    Dano = this.dano,
                    PersonagemId = this.personagemSelecionado.Id
                };

                if (model.Id == 0)
                    await aService.PostArmaAsync(model);
                

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados salvo com sucesso", "Ok");

                await Shell.Current.GoToAsync("..");
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops!", ex.Message, "Ok");
            }
        }

        

        #endregion



    }
}

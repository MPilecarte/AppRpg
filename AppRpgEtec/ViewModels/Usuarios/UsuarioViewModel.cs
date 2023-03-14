﻿using AppRpgEtec.Models;
using AppRpgEtec.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class UsuarioViewModel : BaseViewModel
    {
        private UsuarioServices uService;

        public ICommand RegistrarCommad { get; set; }
        public ICommand AutenticarCommand { get; set; }


        public UsuarioViewModel()
        {
            uService = new UsuarioServices();
            InicializarCommands();
        
        }

        private void InicializarCommands()
        {
            RegistrarCommad = new Command(async () => await RegistrarUsuario());
            AutenticarCommand = new Command(async () => await AutenticarUsuario());
        }

        #region AtributosPropriedades

        private string login = string.Empty;
        public string Login
        { 
            get { return login; } 
            
            set 
            {
                login= value;
                OnPropertyChanged();
            } 
        }

        private string senha = string.Empty;
        public string Senha
        {
            get { return senha; }

            set
            {
                senha= value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Metodos
        public async Task RegistrarUsuario()
        {
            try
            {
                Usuario u = new Usuario();
                u.UserName = Login;
                u.PasswordString = Senha;

                Usuario uRegistrado = await uService.PostRegistrarUsuarioAsync(u);

                if (uRegistrado.Id != 0) 
                { 
                    string mensagem = $"Usuário Id {uRegistrado.Id} registrado com sucesso!";
                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "Ok");

                    await Application.Current.MainPage
                        .Navigation.PopAsync(); 
                }
            }
            catch (Exception ex) 
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }


        public async Task AutenticarUsuario()
        {
            try 
            {
                Usuario u = new Usuario();
                u.UserName = Login;
                u.PasswordString = Senha;

                Usuario uAutenticado = await uService.PostAutenticarUsuarioAsync(u);

                if (!string.IsNullOrEmpty(uAutenticado.Token))
                {
                    string mensagem = $"Bem-vindo {uAutenticado.UserName}";

                    Preferences.Set("UsuarioId", uAutenticado.Id);
                    Preferences.Set("UsuarioUsername", uAutenticado.UserName);
                    Preferences.Set("UsuarioPerfil", uAutenticado.Perfil);
                    Preferences.Set("UsuarioToken", uAutenticado.Token);

                    await Application.Current.MainPage
                        .DisplayAlert("Informação", mensagem, "Ok");

                    Application.Current.MainPage = new MainPage();  
                }
                else 
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Informação", "Dados incorretos :(", "Ok");
                }
            }
            catch(Exception ex) 
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informção", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }


        #endregion
    }
}

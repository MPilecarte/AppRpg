using AppRpgEtec.Models;
using AppRpgEtec.ViewModels.Usuarios;

namespace AppRpgEtec.Views.Usuarios;

public partial class LoginView : ContentPage
{
	UsuarioViewModel viewModel;
	public LoginView()
	{
		InitializeComponent();

		viewModel = new UsuarioViewModel();
		BindingContext = viewModel;
	}
}
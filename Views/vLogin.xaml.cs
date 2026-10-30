
namespace fushcasinaExamem.Views;

public partial class vLogin : ContentPage
{
  
    string[,] usuarios =
    {
        { "recepcion",  "gym2026"  },
        { "entrenador", "fit_01"   },
        { "uisrael",    "2026"     },
        { "gerente",    "admin123" }
    };

    public vLogin()
    {
        InitializeComponent();
    }

  
    protected override void OnAppearing()
    {
        base.OnAppearing();
        txtUsuario.Text = "";
        txtContrasenia.Text = "";
    }

    private async void btnIniciarSesion_Clicked(object sender, EventArgs e)
    {
        try
        {
            string usuarioIngresado = txtUsuario.Text ?? "";
            string contraseniaIngresada = txtContrasenia.Text ?? "";

            // Validar campos vacíos
            if (string.IsNullOrWhiteSpace(usuarioIngresado) || string.IsNullOrWhiteSpace(contraseniaIngresada))
            {
                await DisplayAlertAsync("ERROR", "Ingrese el usuario y la contraseña", "Cerrar");
                return;
            }

            // Recorrer la matriz buscando el usuario y la contraseña
            bool accesoCorrecto = false;

            for (int i = 0; i < usuarios.GetLength(0); i++)
            {
                if (usuarios[i, 0] == usuarioIngresado.Trim() && usuarios[i, 1] == contraseniaIngresada)
                {
                    accesoCorrecto = true;
                    break;
                }
            }

            if (accesoCorrecto)
            {
                //  ventana Registro se va el nombre del usuario que ingreso correctamente
                await Navigation.PushAsync(new vRegistro(usuarioIngresado.Trim()));
            }
            else
            {
                await DisplayAlertAsync("ERROR", "Usuario o contraseña incorrectos", "Cerrar");
                txtUsuario.Text = "";
                txtContrasenia.Text = "";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("ERROR", ex.Message, "Cerrar");
        }
    }

    private async void btnAcercaDe_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new vAcercaDe());
    }
}
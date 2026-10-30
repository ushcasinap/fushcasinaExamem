namespace fushcasinaExamem.Views;


public partial class vResumen : ContentPage
{
    string usuario = "";
    string nombre = "";
    string apellido = "";
    int edad = 0;
    string fechaInicio = "";
    string plan = "";
    string casillero = "";
    double montoInicial = 0;
    double pagoMensual = 0;
    double pagoTotal = 0;

    // Recibe los datos desde vRegistro
    public vResumen(string usuarioR, string nombreR, string apellidoR, int edadR,
                    string fechaInicioR, string planR, bool casilleroR,
                    double montoInicialR, double pagoMensualR, double pagoTotalR)
    {
        InitializeComponent();

        usuario = usuarioR;
        nombre = nombreR;
        apellido = apellidoR;
        edad = edadR;
        fechaInicio = fechaInicioR;
        plan = planR;
        casillero = casilleroR ? "Sí" : "No";
        montoInicial = montoInicialR;
        pagoMensual = pagoMensualR;
        pagoTotal = pagoTotalR;

        lblUsuario.Text = "Usuario conectado: " + usuario;
        lblNombre.Text = nombre;
        lblApellido.Text = apellido;
        lblEdad.Text = edad.ToString();
        lblFechaInicio.Text = fechaInicio;
        lblPlan.Text = plan;
        lblCasillero.Text = casillero;
        lblMontoInicial.Text = "$ " + montoInicial.ToString("0.00");
        lblPagoMensual.Text = "$ " + pagoMensual.ToString("0.00");
        lblPagoTotal.Text = "$ " + pagoTotal.ToString("0.00");
    }

    private async void btnExportar_Clicked(object sender, EventArgs e)
    {
        try
        {
            string texto = "===== RESUMEN DE SOCIO =====\n" +
                           "Usuario conectado: " + usuario + "\n" +
                           "Nombre: " + nombre + "\n" +
                           "Apellido: " + apellido + "\n" +
                           "Edad: " + edad + "\n" +
                           "Fecha de inicio: " + fechaInicio + "\n" +
                           "Plan: " + plan + "\n" +
                           "Casillero: " + casillero + "\n" +
                           "Monto Inicial: $ " + montoInicial.ToString("0.00") + "\n" +
                           "Pago mensual: $ " + pagoMensual.ToString("0.00") + "\n" +
                           "Pago Total: $ " + pagoTotal.ToString("0.00") + "\n\n";

            // Ruta del Escritorio del usuario
            string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // En Android no existe Escritorio, se usa la carpeta de la app
            if (string.IsNullOrEmpty(escritorio))
            {
                escritorio = FileSystem.AppDataDirectory;
            }

            string nombreArchivo = "socios.txt";
            string ruta = Path.Combine(escritorio, nombreArchivo);
            File.AppendAllText(ruta, texto);

            await DisplayAlertAsync("ÉXITO", "Archivo: " + nombreArchivo + "\nRuta: " + ruta, "Cerrar");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("ERROR", ex.Message, "Cerrar");
        }
    }

    private async void btnCerrarSesion_Clicked(object sender, EventArgs e)
    {
        // Regresa al Login (primera ventana) y elimina Registro y Resumen de la pila
        await Navigation.PopToRootAsync();
    }
}
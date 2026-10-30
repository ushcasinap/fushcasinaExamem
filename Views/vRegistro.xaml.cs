
namespace fushcasinaExamem.Views;

public partial class vRegistro : ContentPage
{
    // Datos quemados de la membresía
    const double costoMembresia = 1200;
    const int numeroCuotas = 5;

    string usuario = "";

    // Recibe el usuario desde el Login
    public vRegistro(string usuarioR)
    {
        InitializeComponent();

        usuario = usuarioR;
        lblUsuario.Text = "Usuario conectado: " + usuario;

        // Opciones del Picker
        pkPlan.Items.Add("Musculación");
        pkPlan.Items.Add("Cardio");
        pkPlan.Items.Add("Crossfit");
        pkPlan.Items.Add("Full");

        // controla fechas
        dpFechaInicio.MinimumDate = DateTime.Today;
        dpFechaInicio.Date = DateTime.Today;
    }

    private async void btnCalcular_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Validar monto inicial
            double montoInicial;
            if (!double.TryParse(txtMontoInicial.Text, out montoInicial) || montoInicial < 120 || montoInicial > 600)
            {
                await DisplayAlertAsync("ERROR", "El monto inicial debe ser un número entre 120 y 600", "Cerrar");
                txtMontoInicial.Text = "";
                txtPagoMensual.Text = "";
                return;
            }

            double pagoMensual = CalcularPagoMensual(montoInicial, swCasillero.IsToggled);
            txtPagoMensual.Text = pagoMensual.ToString("0.00");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("ERROR", ex.Message, "Cerrar");
        }
    }

    private async void btnVerResumen_Clicked(object sender, EventArgs e)
    {
        try
        {
            string nombre = (txtNombre.Text ?? "").Trim();
            string apellido = (txtApellido.Text ?? "").Trim();
            string edadTexto = (txtEdad.Text ?? "").Trim();
            DateTime fechaInicio = Convert.ToDateTime(dpFechaInicio.Date);

            // Validar campos vacíos
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || string.IsNullOrEmpty(edadTexto))
            {
                await DisplayAlertAsync("ERROR", "Nombre, apellido y edad son obligatorios", "Cerrar");
                return;
            }

            // Validar nombre y apellido: solo letras
            if (!SoloLetras(nombre))
            {
                await DisplayAlertAsync("ERROR", "El nombre solo debe tener letras", "Cerrar");
                txtNombre.Text = "";
                return;
            }

            if (!SoloLetras(apellido))
            {
                await DisplayAlertAsync("ERROR", "El apellido solo debe tener letras", "Cerrar");
                txtApellido.Text = "";
                return;
            }

            // Validar edad: solo números y mayor o igual a 16
            int edad;
            if (!SoloNumeros(edadTexto) || !int.TryParse(edadTexto, out edad))
            {
                await DisplayAlertAsync("ERROR", "La edad solo debe tener números", "Cerrar");
                txtEdad.Text = "";
                return;
            }

            if (edad < 16 || edad > 100)
            {
                await DisplayAlertAsync("ERROR", "La edad debe ser mayor o igual a 16 años", "Cerrar");
                txtEdad.Text = "";
                return;
            }

            // Validar fecha: no se permiten fechas pasadas
            if (fechaInicio.Date < DateTime.Today)
            {
                await DisplayAlertAsync("ERROR", "La fecha de inicio no puede ser una fecha pasada", "Cerrar");
                dpFechaInicio.Date = DateTime.Today;
                return;
            }

            // Validar plan
            if (pkPlan.SelectedIndex == -1)
            {
                await DisplayAlertAsync("ERROR", "Seleccione un plan", "Cerrar");
                return;
            }

            string plan = pkPlan.Items[pkPlan.SelectedIndex];

            // Validar monto inicial
            double montoInicial;
            if (!double.TryParse(txtMontoInicial.Text, out montoInicial) || montoInicial < 120 || montoInicial > 600)
            {
                await DisplayAlertAsync("ERROR", "El monto inicial debe ser un número entre 120 y 600", "Cerrar");
                txtMontoInicial.Text = "";
                txtPagoMensual.Text = "";
                return;
            }

            // Validar que se haya calculado el pago mensual
            if (string.IsNullOrEmpty(txtPagoMensual.Text))
            {
                await DisplayAlertAsync("ERROR", "Primero presione Calcular Pago mensual", "Cerrar");
                return;
            }

            // Se recalcula por si cambió el monto o el casillero después de calcular
            bool casillero = swCasillero.IsToggled;
            double pagoMensual = CalcularPagoMensual(montoInicial, casillero);
            double pagoTotal = montoInicial + (pagoMensual * numeroCuotas);
            txtPagoMensual.Text = pagoMensual.ToString("0.00");

            // Envía los datos a la ventana Resumen
            await Navigation.PushAsync(new vResumen(usuario, nombre, apellido, edad,
                                                    fechaInicio.ToString("dd/MM/yyyy"), plan, casillero,
                                                    montoInicial, pagoMensual, pagoTotal));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("ERROR", ex.Message, "Cerrar");
        }
    }

    // Cuota = (costo - monto inicial) / 5 + 2% del costo (+ 10 si tiene casillero)
    private double CalcularPagoMensual(double montoInicial, bool casillero)
    {
        double saldo = costoMembresia - montoInicial;
        double cuota = (saldo / numeroCuotas) + (costoMembresia * 0.02);

        if (casillero)
        {
            cuota = cuota + 10;
        }

        return cuota;
    }

    // Devuelve true si el texto tiene solo números
    private bool SoloNumeros(string texto)
    {
        foreach (char c in texto)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }
        return true;
    }

    // Devuelve true si el texto tiene solo letras (y espacios)
    private bool SoloLetras(string texto)
    {
        foreach (char c in texto)
        {
            if (!char.IsLetter(c) && c != ' ')
            {
                return false;
            }
        }
        return true;
    }
}
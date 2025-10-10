using DataSerializers;
using System.Data;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace FORO_Programacion_Avanzada
{
    public partial class Form1 : Form
    {
        private List<Estudiante> ListaEstudiantes = new List<Estudiante>();
        string csvPath = Path.Combine(Application.StartupPath, "Archivo", "Estudiantes.csv");
        public Form1()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {

            try
            {
                // Cargar desde CSV
                ListaEstudiantes = CsvSerializer.Deserialize(csvPath);

                // Vincular DataGridView
                dtgvEstudiantes.AutoGenerateColumns = true; // pon false si ya definiste columnas en el diseñador
                dtgvEstudiantes.DataSource = null;
                dtgvEstudiantes.DataSource = ListaEstudiantes;

                // Vincular ListBox de nombres
                lstEstudiantes.DataSource = null;
                lstEstudiantes.DataSource = ListaEstudiantes;
                lstEstudiantes.DisplayMember = "Nombre";

                // Seleccionar el primer registro para que se ejecute SelectionChanged y se actualicen
                if (dtgvEstudiantes.Rows.Count > 0)
                {
                    dtgvEstudiantes.ClearSelection();
                    dtgvEstudiantes.Rows[0].Selected = true;
                    dtgvEstudiantes.CurrentCell = dtgvEstudiantes.Rows[0].Cells[0];
                    // Llamada explícita por si el evento no se dispara automáticamente
                    dtgvEstudiantes_SelectionChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
        }


        private bool ValidarDatos()
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txbNombre.Text) ||
                string.IsNullOrWhiteSpace(txbEdad.Text) ||
                string.IsNullOrWhiteSpace(txbNota1.Text) ||
                string.IsNullOrWhiteSpace(txbNota2.Text) ||
                string.IsNullOrWhiteSpace(txbNota3.Text))
            {
                MessageBox.Show("Faltan campos obligatorios.");
                return false;
            }

            // Verifica género
            string genero = rbMasculino.Checked ? "Masculino" : (rbFemenino.Checked ? "Femenino" : "");
            if (string.IsNullOrEmpty(genero))
            {
                MessageBox.Show("Debes seleccionar el género.");
                return false;
            }
            return true;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos() == true)
            {
                Estudiante nuevo = new Estudiante
                {
                    Nombre = txbNombre.Text,
                    Edad = int.Parse(txbEdad.Text),
                    Nota1 = double.Parse(txbNota1.Text),
                    Nota2 = double.Parse(txbNota2.Text),
                    Nota3 = double.Parse(txbNota3.Text),
                    Genero = rbMasculino.Checked ? "Masculino" : "Femenino"
                };

                if (cbDeportes.Checked) nuevo.Actividades.Add("Deportes");
                if (cbMusica.Checked) nuevo.Actividades.Add("Música");
                if (cbArte.Checked) nuevo.Actividades.Add("Arte");

                //configurando el datagrid para que muestre los datos
                ListaEstudiantes.Add(nuevo);
                dtgvEstudiantes.AutoGenerateColumns = true;
                dtgvEstudiantes.DataSource = null;
                dtgvEstudiantes.DataSource = ListaEstudiantes;

                //configurando el listbox para que solo muestre los nombres de los estudiantes
                lstEstudiantes.DataSource = null;
                lstEstudiantes.DataSource = ListaEstudiantes;
                lstEstudiantes.DisplayMember = "Nombre";

                //metodo que guarda los datos en el csv
                GuardarDatos();
                LimpiarFormulario();
            }

        }

        private void GuardarDatos()
        {
            try
            {
                CsvSerializer.Serialize(ListaEstudiantes, csvPath);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}");
            }
        }

        private void LimpiarFormulario()
        {
            txbNombre.Clear();
            txbEdad.Clear();
            txbNota1.Clear();
            txbNota2.Clear();
            txbNota3.Clear();
            rbMasculino.Checked = false;
            rbFemenino.Checked = false;
            cbDeportes.Checked = false;
            cbMusica.Checked = false;
            cbArte.Checked = false;
        }

        private void PromedioEstado(Estudiante estudiante)
        {

            lbPromedio.Text = estudiante.Promedio().ToString();
            lbEstado.Text = estudiante.Estado(estudiante.Promedio());

            if (estudiante.Promedio() >= 3.0)
            {
                pbImagen.Image = Image.FromFile(@"Imagenes\Aprobado.png");
                pbImagen.SizeMode = PictureBoxSizeMode.Zoom;
            }

            else
            {
                pbImagen.Image = Image.FromFile(@"Imagenes\Reprobado.png");
                pbImagen.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }



        private void dtgvEstudiantes_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgvEstudiantes.CurrentRow != null && dtgvEstudiantes.CurrentRow.DataBoundItem is Estudiante seleccionado)
            {
                // TextBox
                txbNombre.Text = seleccionado.Nombre;
                txbEdad.Text = seleccionado.Edad.ToString();
                txbNota1.Text = seleccionado.Nota1.ToString();
                txbNota2.Text = seleccionado.Nota2.ToString();
                txbNota3.Text = seleccionado.Nota3.ToString();

                // RadioButton
                if (seleccionado.Genero == "Masculino")
                    rbMasculino.Checked = true;
                else if (seleccionado.Genero == "Femenino")
                    rbFemenino.Checked = true;

                // Actividades
                cbDeportes.Checked = seleccionado.Actividades.Contains("Deportes");
                cbMusica.Checked = seleccionado.Actividades.Contains("Música");
                cbArte.Checked = seleccionado.Actividades.Contains("Arte");

                // Estado y PictureBox
                PromedioEstado(seleccionado);
            }
        }

       

        private void cORREOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dtgvEstudiantes.CurrentRow != null)
            {
                // Obtener el objeto Estudiante de la fila seleccionada
                Estudiante estudianteSeleccionado = dtgvEstudiantes.CurrentRow.DataBoundItem as Estudiante;

                if (estudianteSeleccionado != null)
                {
                    // Abrir el formulario de correo pasando el estudiante
                    Correo formCorreo = new Correo(estudianteSeleccionado);
                    formCorreo.Show();
                }
            }
            else
            {
                MessageBox.Show("Debes seleccionar un estudiante primero.");
            }
        }
    }
}


using System.Data;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using FORO_Programacion_Avanzada.Modelos;
using FORO_Programacion_Avanzada.Data;
using MySqlX.XDevAPI;
using ZstdSharp.Unsafe;

namespace FORO_Programacion_Avanzada
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CargarDatos();
        }

        EstudianteRepository estudianteRepository = new EstudianteRepository();
        ActividadExtraRepository actividadRepository = new ActividadExtraRepository();

        private void CargarDatos()
        {
            var ListaEstudiantes = estudianteRepository.ObtenerEstudiantes();

            var listaAdaptada = ListaEstudiantes.Select(e => new
            {
                e.IdEstudiante,
                e.Nombre,
                e.Genero,
                e.Edad,
                e.Nota1,
                e.Nota2,
                e.Nota3,
                Actividades = string.Join(", ", e.Actividades.Select(a => a.NombreActividad))
            }).ToList();

            // Asignamos al DataGridView
            dtgvEstudiantes.DataSource = listaAdaptada;

            var ListaEstudiantesActividades = actividadRepository.ObtenerInfoEstudianteActividad();
            dtgvEstudianteActividad.DataSource = ListaEstudiantesActividades;

            var listaActividades = actividadRepository.ObtenerActividades();

            chlActividades.Items.Clear(); // Limpia por si ya tenía datos

            foreach (var actividad in listaActividades)
            {
                chlActividades.Items.Add(actividad);
            }

        }
        private void LimpiarFormulario()
        {
            txbCedula.Clear();
            txbNombre.Clear();
            txbEdad.Clear();
            txbNota1.Clear();
            txbNota2.Clear();
            txbNota3.Clear();
            txbNombreFiltro.Clear();
            cmbFiltro.SelectedIndex = -1;
            rbMasculino.Checked = false;
            rbFemenino.Checked = false;
        }

        private bool ValidarDatos()
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txbCedula.Text) ||
                string.IsNullOrWhiteSpace(txbNombre.Text) ||
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
            try
            {
                if (ValidarDatos())
                {
                    Estudiante nuevo = new Estudiante
                    {
                        IdEstudiante = int.Parse(txbCedula.Text),
                        Nombre = txbNombre.Text,
                        Edad = int.Parse(txbEdad.Text),
                        Nota1 = double.Parse(txbNota1.Text),
                        Nota2 = double.Parse(txbNota2.Text),
                        Nota3 = double.Parse(txbNota3.Text),
                        Genero = rbMasculino.Checked ? "Masculino" : "Femenino"
                    };

                    // Construir cadena "1;2;3"
                    string actividades = string.Join(";", chlActividades.CheckedItems
                        .Cast<Actividades_Extracurriculares>()
                        .Select(a => a.IdActividad));

                    estudianteRepository.RegistrarEstudianteConActividades(nuevo, actividades);
                    PromedioEstado(nuevo);

                    MessageBox.Show("Registro completo", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }
     

        private void PromedioEstado(Estudiante estudiante)
        {
            double promedio = (estudiante.Nota1 + estudiante.Nota2 + estudiante.Nota3) / 3;

            if (promedio >= 3)
            {
                lbEstado.Text = "Aprobado";
                lbPromedio.Text = promedio.ToString();
                pbImagen.Image = Image.FromFile(@"Imagenes\Aprobado.png");
                pbImagen.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                lbEstado.Text = "Reprobado";
                lbPromedio.Text = promedio.ToString();
                pbImagen.Image = Image.FromFile(@"Imagenes\Reprobado.png");
                pbImagen.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }



        private void dtgvEstudiantes_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgvEstudiantes.CurrentRow == null)
                return;

            // Si el DataSource sigue siendo una lista de Estudiante
            if (dtgvEstudiantes.CurrentRow.DataBoundItem is Estudiante seleccionado)
            {
                txbCedula.Text = seleccionado.IdEstudiante.ToString();
                txbNombre.Text = seleccionado.Nombre;
                txbEdad.Text = seleccionado.Edad.ToString();
                txbNota1.Text = seleccionado.Nota1.ToString();
                txbNota2.Text = seleccionado.Nota2.ToString();
                txbNota3.Text = seleccionado.Nota3.ToString();

                if (seleccionado.Genero == "Masculino")
                    rbMasculino.Checked = true;
                else if (seleccionado.Genero == "Femenino")
                    rbFemenino.Checked = true;

                PromedioEstado(seleccionado);
            }
            else
            {
                // Si no es un objeto Estudiante (por ejemplo, si usaste un .Select)
                try
                {
                    txbCedula.Text = dtgvEstudiantes.CurrentRow.Cells["IdEstudiante"].Value?.ToString();
                    txbNombre.Text = dtgvEstudiantes.CurrentRow.Cells["Nombre"].Value?.ToString();
                    txbEdad.Text = dtgvEstudiantes.CurrentRow.Cells["Edad"].Value?.ToString();
                    txbNota1.Text = dtgvEstudiantes.CurrentRow.Cells["Nota1"].Value?.ToString();
                    txbNota2.Text = dtgvEstudiantes.CurrentRow.Cells["Nota2"].Value?.ToString();
                    txbNota3.Text = dtgvEstudiantes.CurrentRow.Cells["Nota3"].Value?.ToString();

                    string genero = dtgvEstudiantes.CurrentRow.Cells["Genero"].Value?.ToString();
                    rbMasculino.Checked = genero == "Masculino";
                    rbFemenino.Checked = genero == "Femenino";

                    // Si PromedioEstado solo necesita el promedio, puedes calcularlo así:
                    float n1 = float.Parse(txbNota1.Text);
                    float n2 = float.Parse(txbNota2.Text);
                    float n3 = float.Parse(txbNota3.Text);
                    float promedio = (n1 + n2 + n3) / 3;
                    //PromedioEstado(seleccionado);
                }
                catch
                {
                    // Si alguna columna no existe, evita que se rompa el programa
                }
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarDatos())
                {
                    Estudiante estudianteEditado = new Estudiante
                    {
                        IdEstudiante = int.Parse(txbCedula.Text),
                        Nombre = txbNombre.Text,
                        Edad = int.Parse(txbEdad.Text),
                        Nota1 = double.Parse(txbNota1.Text),
                        Nota2 = double.Parse(txbNota2.Text),
                        Nota3 = double.Parse(txbNota3.Text),
                        Genero = rbMasculino.Checked ? "Masculino" : "Femenino"
                    };

                    estudianteRepository.editarEstudiante(estudianteEditado, estudianteEditado.IdEstudiante);

                    actividadRepository.EliminarEstudianteActividad(estudianteEditado.IdEstudiante);

                    foreach (Actividades_Extracurriculares actividad in chlActividades.CheckedItems)
                    {
                        int idActividad = actividad.IdActividad;

                        actividadRepository.RegistrarEstudianteActividad(estudianteEditado.IdEstudiante, idActividad);
                    }


                    MessageBox.Show("Edicion completa: ", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarDatos();
                    LimpiarFormulario();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void dtgvEstudiantes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtener el Id del estudiante
                int IdEstudiante = Convert.ToInt32(dtgvEstudiantes.Rows[e.RowIndex].Cells["IdEstudiante"].Value);

                // Confirmación antes de eliminar
                DialogResult resultado = MessageBox.Show(
                    "¿Estás seguro de eliminar a este estudiante?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    // Llamar al repositorio
                    estudianteRepository.EliminarEstudiante(IdEstudiante);

                    CargarDatos();
                    LimpiarFormulario();

                }
            }
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            estudianteRepository.GenerarReporte();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            string Genero = cmbFiltro.SelectedItem?.ToString() == "Todos" ? null : cmbFiltro.SelectedItem?.ToString();
            string NombreFiltrado = txbNombreFiltro.Text;


            var ListaFiltrada = estudianteRepository.FiltrarEstudiantes(NombreFiltrado, Genero);

            var listaAdaptada = ListaFiltrada.Select(e => new
            {
                e.IdEstudiante,
                e.Nombre,
                e.Genero,
                e.Edad,
                e.Nota1,
                e.Nota2,
                e.Nota3,
                Promedio = (e.Nota1 + e.Nota2 + e.Nota3) / 3,
                Actividades = string.Join(", ", e.Actividades.Select(a => a.NombreActividad))
            }).ToList();

            dtgvEstudiantes.DataSource = listaAdaptada;

            LimpiarFormulario();
        }

        private void btnRestablecer_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }
    }
}

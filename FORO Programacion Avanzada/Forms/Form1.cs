using DataSerializers;
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
                if (ValidarDatos() == true)
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

                    estudianteRepository.RegistrarEstudiante(nuevo);



                    foreach (Actividades_Extracurriculares actividad in chlActividades.CheckedItems)
                    {
                        int idActividad = actividad.IdActividad;

                        actividadRepository.RegistrarEstudianteActividad(nuevo.IdEstudiante, idActividad);
                    }

                    MessageBox.Show("Registro completo: ", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarDatos();
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
                txbCedula.Text = seleccionado.IdEstudiante.ToString();
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
    }
}

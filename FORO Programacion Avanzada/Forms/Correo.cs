using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using FORO_Programacion_Avanzada.Modelos;

namespace FORO_Programacion_Avanzada
{
    public partial class Correo : Form
    {
        Estudiante estudianteSeleccionado;
        public Correo(Estudiante estudiante)
        {
            InitializeComponent();
            estudianteSeleccionado = estudiante;

        }

        private bool ValidarCorreo(string email)
        {
            // Validar formato
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return false;
            }

            // Validar dominio permitido
            string dominiosConfig = System.Configuration.ConfigurationManager.AppSettings["dominiosPermitidos"];
            string[] dominiosPermitidos = dominiosConfig.Split(',');
            string dominio = email.Split('@')[1];
            return dominiosPermitidos.Contains(dominio);
        }
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            string destinatario = txbEmail.Text.Trim();

            // Validar correo
            if (!ValidarCorreo(destinatario))
            {
                MessageBox.Show("Correo inválido o dominio no permitido.");
                return;
            }

            // Verificar que haya un estudiante seleccionado
            if (estudianteSeleccionado == null)
            {
                MessageBox.Show("Debes seleccionar un estudiante.");
                return;
            }

            try
            {
                // Leer configuración SMTP desde app.config
                string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
                int smtpPort = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);
                string smtpUser = ConfigurationManager.AppSettings["smtpUser"];
                string smtpPass = ConfigurationManager.AppSettings["smtpPass"];

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(smtpUser);
                mail.To.Add(destinatario);
                mail.Subject = $"Información del estudiante {estudianteSeleccionado.Nombre}";
                mail.Body = $"Nombre: {estudianteSeleccionado.Nombre}\n" +
                            $"Edad: {estudianteSeleccionado.Edad}\n" +
                            $"Nota1: {estudianteSeleccionado.Nota1}\n" +
                            $"Nota2: {estudianteSeleccionado.Nota2}\n" +
                            $"Nota3: {estudianteSeleccionado.Nota3}\n" +
                            $"Género: {estudianteSeleccionado.Genero}\n";


                // Configurar y enviar correo
                SmtpClient smtp = new SmtpClient(smtpServer, smtpPort);
                smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtp.EnableSsl = false; // Para smtp4dev; si usas Gmail/Outlook, cambia a true

                smtp.Send(mail);

                MessageBox.Show("Correo enviado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar correo: " + ex.Message);
            }
        }

        private void Correo_Load(object sender, EventArgs e)
        {

        }
    }
}

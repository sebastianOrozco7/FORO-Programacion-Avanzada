using FORO_Programacion_Avanzada.Data;

namespace FORO_Programacion_Avanzada
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //string connectionString = "server=tu_servidor;database=gestionestudiantes;user=usuario;password=contraseña;";
            //ConexionProtegida.SaveEncryptedConnectionString(connectionString);
            //MessageBox.Show("✅ Cadena encriptada correctamente. Revisa el archivo conn.enc en bin/Debug.");




            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
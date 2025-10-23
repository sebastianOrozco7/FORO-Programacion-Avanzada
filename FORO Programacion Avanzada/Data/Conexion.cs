using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using Microsoft.VisualBasic.Logging;
using FORO_Programacion_Avanzada.Modelos;

namespace FORO_Programacion_Avanzada.Data
{
    internal class Conexion
    {
        private static string GetConnectionString()
        {
            string ambiente = ConfigurationManager.AppSettings["Ambiente"];
            string nombreConexion = ambiente == "Remoto" ? "RemoteConnection" : "LocalConnection";
            return ConfigurationManager.ConnectionStrings[nombreConexion].ConnectionString;
        }

        public static MySqlConnection GetConnectionWithRetry(int maxRetries = 3)
        {
            string connString = GetConnectionString();
            int retry = 0;

            while (retry < maxRetries)
            {
                try
                {
                    MySqlConnection conn = new MySqlConnection(connString);
                    conn.Open();
                    Logger.Log("Conexión establecida correctamente al entorno: " + ConfigurationManager.AppSettings["Ambiente"]);
                    return conn;
                }
                catch (MySqlException ex)
                {
                    Logger.LogError($"Intento {retry + 1} fallido: {ex.Message}");


                    retry++;
                    Thread.Sleep(2000); // Espera 2 segundos antes de reintentar
                }
            }

            throw new Exception("Conexión fallida después de varios intentos.");
        }
        
    }
}

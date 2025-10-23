using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace FORO_Programacion_Avanzada.Modelos
{
    public static class Logger
    {
        private static readonly string logPath = @"..\..\..\Archivos\log.txt";

        public static void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logPath, true))
                {
                    writer.WriteLine($"[{DateTime.Now}] ✅  CORRECTO: {message}");
                }
            }
            catch
            {
                // Evita que falle el programa si hay error al escribir el log.
            }
        }
        public static void LogError(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logPath, true))
                {
                    writer.WriteLine($"[{DateTime.Now}] ❌  ERROR: {message}");
                }
            }
            catch
            {
                // Evita que falle el programa si hay error al escribir el log.
            }
        }
    }
}

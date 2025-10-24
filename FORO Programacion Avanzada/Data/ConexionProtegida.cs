using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FORO_Programacion_Avanzada.Data
{
    public static class ConexionProtegida
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "conn.enc");

        // Ejecutar una vez para crear el archivo conn.enc
        public static void SaveEncryptedConnectionString(string connectionString)
        {
            byte[] plain = Encoding.UTF8.GetBytes(connectionString);
            byte[] encrypted = ProtectedData.Protect(plain, null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(FilePath, encrypted);
        }

        // Leer en runtime
        public static string ReadDecryptedConnectionString()
        {
            if (!File.Exists(FilePath)) throw new FileNotFoundException("No se encontró conn.enc");
            byte[] encrypted = File.ReadAllBytes(FilePath);
            byte[] plain = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(plain);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace FORO_Programacion_Avanzada.Data
{
    internal class Conexion
    {
        private readonly string CadenaDeConexion = "Server = localhost; Database =  GestionEstudiantes; Uid=root; Pwd=admin;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(CadenaDeConexion);
        }
        
    }
}

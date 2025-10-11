using FORO_Programacion_Avanzada.Modelos;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FORO_Programacion_Avanzada.Data
{
    internal class EstudianteRepository
    {
        public void RegistrarEstudiante(Estudiante estudiante)
        {
            string Query = "INSERT INTO Estudiante(IdEstudiante,Nombre,Genero,Edad,Nota1,Nota2,Nota3)" +
                            "VALUES (@IdEstudiante,@Nombre,@Genero,@Edad,@Nota1,@Nota2,@Nota3)";
            bool Registrado = false;

            
                using(MySqlConnection conexion = new Conexion().GetConnection())
                {
                    conexion.Open();

                    using(MySqlCommand cmd = new MySqlCommand(Query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEstudiante",estudiante.IdEstudiante);
                        cmd.Parameters.AddWithValue("@Nombre",estudiante.Nombre);
                        cmd.Parameters.AddWithValue("@Genero", estudiante.Genero);
                        cmd.Parameters.AddWithValue("@Edad", estudiante.Edad);
                        cmd.Parameters.AddWithValue("@Nota1", estudiante.Nota1);
                        cmd.Parameters.AddWithValue("@Nota2", estudiante.Nota2);
                        cmd.Parameters.AddWithValue("@Nota3", estudiante.Nota3);

                        cmd.ExecuteNonQuery();
                        
                    }
                }
        }

        public List<Estudiante> ObtenerEstudiantes()
        {

            List<Estudiante> lista = new List<Estudiante>();
            string Query = "SELECT IdEstudiante,Nombre,Genero,Edad,Nota1,Nota2,Nota3 FROM Estudiante";

            using (MySqlConnection conexion = new Conexion().GetConnection())
            {
                conexion.Open();
                using (MySqlCommand cmd = new MySqlCommand(Query, conexion))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Estudiante estudiante = new Estudiante
                            {
                                IdEstudiante = reader.GetInt32("IdEstudiante"),
                                Nombre = reader.GetString("Nombre"),
                                Genero = reader.GetString("Genero"),
                                Edad = reader.GetInt32("Edad"),
                                Nota1 = reader.GetFloat("Nota1"),
                                Nota2 = reader.GetFloat("Nota2"),
                                Nota3 = reader.GetFloat("Nota3"),
                            };
                            lista.Add(estudiante);
                        }
                    }
                }
            }

            return lista;
        }
    }
}

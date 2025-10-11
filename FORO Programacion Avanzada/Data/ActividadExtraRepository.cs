using FORO_Programacion_Avanzada.Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORO_Programacion_Avanzada.Data
{
    internal class ActividadExtraRepository
    {

        public List<Actividades_Extracurriculares> ObtenerActividades()
        {
            List<Actividades_Extracurriculares> actividadesExtra = new List<Actividades_Extracurriculares>();

            string Query = "Select IdActividad, Nombre from ActividadExtra";

            using (MySqlConnection conexion = new Conexion().GetConnection())
            {
                conexion.Open();
                using(MySqlCommand cmd = new MySqlCommand(Query, conexion))
                {
                    using(MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Actividades_Extracurriculares actividad = new Actividades_Extracurriculares
                            {
                                IdActividad = reader.GetInt32("IdActividad"),
                                NombreActividad = reader.GetString("Nombre")
                            };
                            actividadesExtra.Add(actividad);
                        }
                    }
                }
            }

            return actividadesExtra;

        }

        public void RegistrarEstudianteActividad(int IdEstudiante, int IdActividad)
        {
       
            string Query = "INSERT INTO EstudianteActividad(IdEstudiante,IdActividad)" +
                            "VALUES(@IdEstudiante,@IdActividad)";

                using (MySqlConnection conexion = new Conexion().GetConnection())
                {
                    conexion.Open();

                    using (MySqlCommand cmd = new MySqlCommand(Query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEstudiante", IdEstudiante);
                        cmd.Parameters.AddWithValue("@IdActividad", IdActividad);

                        cmd.ExecuteNonQuery();
                    }
                }
        }

        public DataTable ObtenerInfoEstudianteActividad()
        {
            string Query = @"
                            SELECT 
                            e.Nombre AS NombreEstudiante,
                            a.Nombre AS NombreActividad
                            FROM EstudianteActividad ea
                            INNER JOIN Estudiante e ON ea.IdEstudiante = e.IdEstudiante
                            INNER JOIN ActividadExtra a ON ea.IdActividad = a.IdActividad;";

            using(MySqlConnection conexion = new Conexion().GetConnection())
            {
                conexion.Open();
                using (MySqlCommand cmd = new MySqlCommand(Query, conexion))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        return tabla;
                    }
                }
            }
        }

        public void EliminarEstudianteActividad(int IdEstudiante)
        {
            string QueryEliminar = "DELETE FROM EstudianteActividad Where IdEstudiante = @IdEstudiante";

            using (MySqlConnection conexion = new Conexion().GetConnection())
            {
                conexion.Open();
                using(MySqlCommand cmdEliminar = new MySqlCommand(QueryEliminar, conexion))
                {
                    cmdEliminar.Parameters.AddWithValue("@IdEstudiante",IdEstudiante);
                    cmdEliminar.ExecuteNonQuery();
                }
            }
        }
    }
}

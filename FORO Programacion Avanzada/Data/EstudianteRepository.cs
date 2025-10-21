using FORO_Programacion_Avanzada.Modelos;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FORO_Programacion_Avanzada.Data
{
    internal class EstudianteRepository
    {
        public void RegistrarEstudiante(Estudiante estudiante)
        {
            string Query = "INSERT INTO Estudiante(IdEstudiante,Nombre,Genero,Edad,Nota1,Nota2,Nota3)" +
                            "VALUES (@IdEstudiante,@Nombre,@Genero,@Edad,@Nota1,@Nota2,@Nota3)";

            try
            {
                using (MySqlConnection conexion = new Conexion().GetConnection())
                {
                    conexion.Open();

                    using (MySqlCommand cmd = new MySqlCommand(Query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEstudiante", estudiante.IdEstudiante);
                        cmd.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
                        cmd.Parameters.AddWithValue("@Genero", estudiante.Genero);
                        cmd.Parameters.AddWithValue("@Edad", estudiante.Edad);
                        cmd.Parameters.AddWithValue("@Nota1", estudiante.Nota1);
                        cmd.Parameters.AddWithValue("@Nota2", estudiante.Nota2);
                        cmd.Parameters.AddWithValue("@Nota3", estudiante.Nota3);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Guardamos el error en el log
                Logger.LogError($"Error al registrar estudiante {estudiante.Nombre}: {ex.Message}");
                // Lanza la excepción nuevamente si quieres mostrar mensaje al usuario
                throw;
            }

        }

        public List<Estudiante> ObtenerEstudiantes()
        {
            // Diccionario para evitar duplicados
            Dictionary<int, Estudiante> estudiantesDict = new Dictionary<int, Estudiante>();

            string Query = @"
        SELECT 
            e.IdEstudiante,
            e.Nombre,
            e.Genero,
            e.Edad,
            e.Nota1,
            e.Nota2,
            e.Nota3,
            a.IdActividad,
            a.Nombre AS NombreActividad
        FROM Estudiante e
        LEFT JOIN EstudianteActividad ea ON e.IdEstudiante = ea.IdEstudiante
        LEFT JOIN ActividadExtra a ON ea.IdActividad = a.IdActividad;";

            using (MySqlConnection conexion = new Conexion().GetConnection())
            {
                conexion.Open();
                using (MySqlCommand cmd = new MySqlCommand(Query, conexion))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idEstudiante = reader.GetInt32("IdEstudiante");

                            // Si el estudiante no existe aún en el diccionario, lo creamos
                            if (!estudiantesDict.ContainsKey(idEstudiante))
                            {
                                var estudiante = new Estudiante
                                {
                                    IdEstudiante = idEstudiante,
                                    Nombre = reader.GetString("Nombre"),
                                    Genero = reader.GetString("Genero"),
                                    Edad = reader.GetInt32("Edad"),
                                    Nota1 = reader.GetFloat("Nota1"),
                                    Nota2 = reader.GetFloat("Nota2"),
                                    Nota3 = reader.GetFloat("Nota3"),
                                    Actividades = new List<Actividades_Extracurriculares>() // importante
                                };

                                estudiantesDict.Add(idEstudiante, estudiante);
                            }

                            // Si el estudiante tiene una actividad asociada, la agregamos
                            if (!reader.IsDBNull(reader.GetOrdinal("IdActividad")))
                            {
                                var actividad = new Actividades_Extracurriculares
                                {
                                    IdActividad = reader.GetInt32("IdActividad"),
                                    NombreActividad = reader.GetString("NombreActividad")
                                };

                                estudiantesDict[idEstudiante].Actividades.Add(actividad);
                            }
                        }
                    }
                }
            }

            // Devolvemos la lista final de estudiantes
            return estudiantesDict.Values.ToList();
        }


        public void editarEstudiante(Estudiante estudiante, int Id)
        {
            string Query = "UPDATE Estudiante SET Nombre = @Nombre, Genero = @Genero, Edad = @Edad," +
                            "Nota1 = @Nota1, Nota2 = @Nota2, Nota3 = @Nota3 WHERE IdEstudiante = @IdEstudiante";

            try
            {
                using (MySqlConnection conexion = new Conexion().GetConnection())
                {
                    conexion.Open();

                    using (MySqlCommand cmd = new MySqlCommand(Query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
                        cmd.Parameters.AddWithValue("@Genero", estudiante.Genero);
                        cmd.Parameters.AddWithValue("@Edad", estudiante.Edad);
                        cmd.Parameters.AddWithValue("@Nota1", estudiante.Nota1);
                        cmd.Parameters.AddWithValue("@Nota2", estudiante.Nota2);
                        cmd.Parameters.AddWithValue("@Nota3", estudiante.Nota3);
                        cmd.Parameters.AddWithValue("@IdEstudiante", estudiante.IdEstudiante);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Guardamos el error en el log
                Logger.LogError($"Error al actualizar estudiante {estudiante.Nombre}: {ex.Message}");
                // Lanza la excepción nuevamente si quieres mostrar mensaje al usuario
                throw;
            }
        }

        public void EliminarEstudiante(int IdEstudiante)
        {
            string Query = "DELETE FROM Estudiante WHERE IdEstudiante = @IdEstudiante";

            try
            {

                //primero necesitamos eliminar la relacion porque si no nos dara error asi que llamamos el metodo que borra EstudianteActividad
                ActividadExtraRepository actividadExtra = new ActividadExtraRepository();
                actividadExtra.EliminarEstudianteActividad(IdEstudiante);

                using (MySqlConnection conexion = new Conexion().GetConnection())
                {
                    conexion.Open();

                    using (MySqlCommand cmd = new MySqlCommand(Query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEstudiante", IdEstudiante);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Guardamos el error en el log
                Logger.LogError($"Error al eliminar estudiante con el id {IdEstudiante}: {ex.Message}");
                // Lanza la excepción nuevamente si quieres mostrar mensaje al usuario
                throw;
            }
        }

        public void GenerarReporte()
        {
            try
            {
                string Query = "SELECT " +
                               "Nombre, " +
                               "(Nota1 + Nota2 + Nota3) / 3.0 AS Promedio, " +
                               "CASE " +
                               "WHEN (Nota1 + Nota2 + Nota3) / 3.0 >= 3.0 THEN 'Aprobado' " +
                               "ELSE 'Reprobado' " +
                               "END AS Estado " +
                               "FROM Estudiante;";

                using (MySqlConnection conexion = new Conexion().GetConnection())
                {
                    conexion.Open();
                    using(MySqlCommand cmd = new MySqlCommand(Query, conexion))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            string Reporte = "REPORTE DE ESTUDIANTES:\n ";

                            while(reader.Read())
                            {
                                Reporte += $"Nombre: {reader["Nombre"]}, Promedio: {reader["Promedio"]}, Estado: {reader["Estado"]}\n";

                            }
                            MessageBox.Show(Reporte);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"ERROR EN REPORTE: {ex.Message}");
            }
        }

        public List<Estudiante> FiltrarEstudiantes(string Nombre, string Genero)
        {
            var estudiantes = new List<Estudiante>();
            var estudianteDict = new Dictionary<int, Estudiante>();

            string query = @"
                        SELECT e.IdEstudiante, e.Nombre, e.Genero, e.Edad, e.Nota1, e.Nota2, e.Nota3,
                               a.Nombre AS Actividad
                        FROM Estudiante e
                        LEFT JOIN EstudianteActividad ea ON e.IdEstudiante = ea.IdEstudiante
                        LEFT JOIN ActividadExtra a ON ea.IdActividad = a.IdActividad
                        WHERE (@Genero IS NULL OR e.Genero = @Genero)
                          AND (@Nombre IS NULL OR e.Nombre LIKE CONCAT('%', @Nombre, '%'));
                    ";

            try
            {
                using (MySqlConnection conn = new Conexion().GetConnection())
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Genero", string.IsNullOrEmpty(Genero) ? DBNull.Value : Genero);
                        cmd.Parameters.AddWithValue("@Nombre", string.IsNullOrEmpty(Nombre) ? DBNull.Value : Nombre);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("IdEstudiante");

                                if (!estudianteDict.ContainsKey(id))
                                {
                                    var est = new Estudiante
                                    {
                                        IdEstudiante = id,
                                        Nombre = reader.GetString("Nombre"),
                                        Genero = reader.GetString("Genero"),
                                        Edad = reader.GetInt32("Edad"),
                                        Nota1 = reader.GetFloat("Nota1"),
                                        Nota2 = reader.GetFloat("Nota2"),
                                        Nota3 = reader.GetFloat("Nota3"),
                                        Actividades = new List<Actividades_Extracurriculares>()
                                    };

                                    estudianteDict.Add(id, est);
                                    estudiantes.Add(est);
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("Actividad")))
                                {
                                    estudianteDict[id].Actividades.Add(new Actividades_Extracurriculares
                                    {
                                        NombreActividad = reader.GetString("Actividad")
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Registrar el error en el log
                Logger.LogError($"Error al filtrar estudiantes (Nombre: {Nombre}, Género: {Genero}): {ex.Message}");
                throw;
            }

            return estudiantes;
        }
    }
}

using FORO_Programacion_Avanzada.Modelos;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FORO_Programacion_Avanzada.Data
{
    internal class EstudianteRepository
    {
        //metodo para registrar estudiantes y sus actividades mediante un procedimiento almacenado
        public void RegistrarEstudianteConActividades(Estudiante estudiante, string actividades)
        {
            using (MySqlConnection conexion = Conexion.GetConnectionWithRetry())
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_InsertEstudiante", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_IdEstudiante", estudiante.IdEstudiante);
                    cmd.Parameters.AddWithValue("@p_Nombre", estudiante.Nombre);
                    cmd.Parameters.AddWithValue("@p_Genero", estudiante.Genero);
                    cmd.Parameters.AddWithValue("@p_Edad", estudiante.Edad);
                    cmd.Parameters.AddWithValue("@p_Nota1", estudiante.Nota1);
                    cmd.Parameters.AddWithValue("@p_Nota2", estudiante.Nota2);
                    cmd.Parameters.AddWithValue("@p_Nota3", estudiante.Nota3);
                    cmd.Parameters.AddWithValue("@p_Actividades", actividades); 

                    cmd.ExecuteNonQuery();
                }
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

            using (MySqlConnection conexion = Conexion.GetConnectionWithRetry())
            {
                
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

        //metodo para editar estudiantes y sus actividades mediante un procedimiento almacenado
        public void editarEstudiante(Estudiante estudiante, int Id)
        {
            try
            {
                using (MySqlConnection conexion = Conexion.GetConnectionWithRetry())
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_UpdateEstudiante", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("p_IdEstudiante", estudiante.IdEstudiante);
                        cmd.Parameters.AddWithValue("p_Nombre", estudiante.Nombre);
                        cmd.Parameters.AddWithValue("p_Genero", estudiante.Genero);
                        cmd.Parameters.AddWithValue("p_Edad", estudiante.Edad);
                        cmd.Parameters.AddWithValue("p_Nota1", estudiante.Nota1);
                        cmd.Parameters.AddWithValue("p_Nota2", estudiante.Nota2);
                        cmd.Parameters.AddWithValue("p_Nota3", estudiante.Nota3);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error al actualizar estudiante {estudiante.Nombre}: {ex.Message}");
                throw;
            }
        }

        //metodo para eliminar estudiantes y sus actividades mediante un procedimiento almacenado
        public void EliminarEstudiante(int IdEstudiante)
        {
            try
            {
                using (MySqlConnection conexion = Conexion.GetConnectionWithRetry())
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_DeleteEstudiante", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_IdEstudiante", IdEstudiante);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error al eliminar estudiante con el id {IdEstudiante}: {ex.Message}");
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

                using (MySqlConnection conexion = Conexion.GetConnectionWithRetry())
                {
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
                using (MySqlConnection conn = Conexion.GetConnectionWithRetry())
                {

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

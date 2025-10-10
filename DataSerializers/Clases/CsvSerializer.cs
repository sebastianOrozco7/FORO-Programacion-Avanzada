namespace DataSerializers
{
    public class CsvSerializer
    {
        public static void Serialize(List<Estudiante> estudiantes, string path)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine("Nombre,Edad,Nota1,Nota2,Nota3,Genero,Actividades");
                    foreach (var est in estudiantes)
                    {
                        string actividades = string.Join(";", est.Actividades);

                        writer.WriteLine($"{est.Nombre},{est.Edad},{est.Nota1},{est.Nota2},{est.Nota3},{est.Genero},{actividades}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar: {ex.Message}");
            }
        }

        public static List<Estudiante> Deserialize(string path)
        {
            var estudiantes = new List<Estudiante>();
            if (!File.Exists(path))
                return estudiantes;
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    // Saltar encabezado
                    reader.ReadLine();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] datos = line.Split(',');
                        if (datos.Length >= 7)
                        {
                            Estudiante est = new Estudiante
                            {
                                Nombre = datos[0],
                                Edad = int.Parse(datos[1]),
                                Nota1 = double.Parse(datos[2]),
                                Nota2 = double.Parse(datos[3]),
                                Nota3 = double.Parse(datos[4]),
                                Genero = datos[5]
                            };
                            string[] acts = datos[6].Split(';');
                            est.Actividades.AddRange(acts);
                            estudiantes.Add(est);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cargar: {ex.Message}");
            }
            return estudiantes;
        }
    }
}

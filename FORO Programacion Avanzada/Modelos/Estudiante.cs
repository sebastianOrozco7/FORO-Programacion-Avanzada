using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORO_Programacion_Avanzada.Modelos
{
    public class Estudiante
    {
        public int IdEstudiante {  get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public double Nota1 { get; set; }
        public double Nota2 { get; set; }
        public double Nota3 { get; set; }
        public string Genero { get; set; }

        public double Promedio()
        {
            double promedio = (Nota1 + Nota2 + Nota3) / 3.0;
            return promedio;
        }
        public string Estado(double promedio)
        {
            if (promedio >= 3.0)
                return "Aprobado";

            else
                return "Reprobado";
        }
    }
}

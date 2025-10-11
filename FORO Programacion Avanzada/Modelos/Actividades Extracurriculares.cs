using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORO_Programacion_Avanzada.Modelos
{
    internal class Actividades_Extracurriculares
    {
        public int IdActividad {  get; set; }
        public string NombreActividad { get; set; }

        public override string ToString()
        {
            return NombreActividad;
        }
    }
}

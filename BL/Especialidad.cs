using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Especialidad
    {
        public static Dictionary<string, object> GetAll()
        {
            ML.Especialidad esp = new ML.Especialidad();
            string excepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Especialidad", esp }, { "Excepcion", excepcion }, { "Resultado", false } };

            try
            {
                using (DL.CoteroNuevoHospitalContext context = new DL.CoteroNuevoHospitalContext())
                {
                    var registros =
                        //LINQ

                        (from Especialidad in context.Especialidads
                         select new
                         {
                             IdEspecialidad = Especialidad.IdEspecialidad,
                             Nombre = Especialidad.Nombre,

                         }).ToList();
                    if (registros != null)
                    {
                        esp.Especialidades = new List<object>();
                        foreach (var registro in registros)
                        {
                            //Instanciar el Hospital
                            ML.Especialidad especialidad = new ML.Especialidad();

                            especialidad.IdEspecialidad = registro.IdEspecialidad;
                            especialidad.Nombre = registro.Nombre;
                           
                            esp.Especialidades.Add(especialidad);

                        }
                        diccionario["Resultado"] = true;
                        diccionario["Especialidad"] = esp;
                    }
                }
            }
            catch (Exception ex)
            {
                diccionario["Resultado"] = false;
                diccionario["Especialidad"] = ex.Message;
            }
            return diccionario;
        }

    }
}

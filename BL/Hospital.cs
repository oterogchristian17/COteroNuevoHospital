using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Hospital
    {
        public static Dictionary<string, object> GetAll()
        {
            ML.Hospital hosp = new ML.Hospital();
            string excepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Hospital", hosp }, { "Excepcion", excepcion }, { "Resultado", false } };

            try
            {
                using (DL.CoteroNuevoHospitalContext context = new DL.CoteroNuevoHospitalContext()) 
                {
                    var registros =
                        //LINQ

                        (from Hospital in context.Hospitals
                         select new
                         {
                             IdHospital = Hospital.IdHospital,
                             Nombre = Hospital.Nombre,
                             Direccion = Hospital.Direccion,
                             AñoConstruccion = Hospital.AñoConstruccion,
                             Capacidad = Hospital.Capacidad,
                             IdEspecialidad = Hospital.IdEspecialidad

                         }).ToList();
                    if ( registros != null) 
                    {
                        hosp.Hospitales = new List<object>();
                        foreach (var registro in registros) 
                        {
                            //Instanciar el Hospital
                            ML.Hospital hospital = new ML.Hospital();

                            hospital.IdHospital = registro.IdHospital;
                            hospital.Nombre = registro.Nombre;
                            hospital.Direccion = registro.Direccion;
                            hospital.AñoConstruccion = (DateTime)registro.AñoConstruccion;
                            hospital.Capacidad = (int)registro.Capacidad;

                            //Propiedad de Navegacion

                            hospital.Especialidad = new ML.Especialidad();
                            hospital.Especialidad.IdEspecialidad = (int)registro.IdEspecialidad;

                            hosp.Hospitales.Add(hospital);  

                        }
                        diccionario["Resultado"] = true;
                        diccionario["Usuario"] = hosp;
                    }    
                }
            }
            catch (Exception ex) 
            {
                diccionario["Resultado"] = false;
                diccionario["Hospital"] = ex.Message;
            }
            return diccionario;
        }


        public static Dictionary<string, object> Delete(int IdHospital)
        {
            string exepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Hospital", IdHospital }, { "Exepcion", exepcion }, { "Resultado", false } };

            try
            {

                using (DL.CoteroNuevoHospitalContext context = new DL.CoteroNuevoHospitalContext())
                {

                    var filasAfectadas = context.Database.ExecuteSqlRaw($"HospitalDelete'{IdHospital}'");

                    if (filasAfectadas > 0)

                    {
                        diccionario["Resultado"] = true;
                    }
                    else
                    {
                        diccionario["Resultado"] = false;
                    }
                }

            }
            catch (Exception ex)
            {
                diccionario["Resultado"] = false;
                diccionario["Exepcion"] = ex.Message;
            }
            return diccionario;
        }

        public static Dictionary<string, object> Add(ML.Hospital hospital)
        {
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Excepcion", "" }, { "Resultado", false } };

            try
            {
                //AQUI CAMBIA EL USING A DL
                using (DL.CoteroNuevoHospitalContext context = new DL.CoteroNuevoHospitalContext())
                {

                    //AQUI CAMBIA LA SENTENCIA PARA LLAMAR AL STORE PROCEDURE
                    var filasAfectadas = context.Database.ExecuteSqlRaw($"HospitalAdd '{hospital.Nombre}', '{hospital.Direccion}','{hospital.AñoConstruccion}', '{hospital.Capacidad}','{hospital.Especialidad.IdEspecialidad}'");

                    //Validar si las filas fueron afectadas
                    if (filasAfectadas > 0)
                    {
                        diccionario["Resultado"] = true;
                    }
                    else
                    {
                        diccionario["Resultado"] = false;
                    }
                }
            }
            catch (Exception ex) //SI FALLÓ ALGO
            {
                diccionario["Resultado"] = false;
                diccionario["Excepcion"] = ex.Message;

            }
            return diccionario;
        }




    }
}

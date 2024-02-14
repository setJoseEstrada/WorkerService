using Nucleo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_Service
{
    public class AppSettingsManager
    {
        private readonly IConfiguration _configuration;
        public readonly SYS_DTO_Conexion _oConexiones;
        public AppSettingsManager(IConfiguration configuration) 
        {
            _configuration = configuration;
            _oConexiones = new SYS_DTO_Conexion();
            CargarConexiones();
        }

        public void CargarConexiones() 
        {
            var sConectionStringSection = _configuration.GetSection("ConnectionStrings");
            

            _oConexiones.sRutaPendiente = sConectionStringSection["Ruta_Pendientes"];
            _oConexiones.sRutaProcesados = sConectionStringSection["Ruta_Procesados"];
        }
    }
}

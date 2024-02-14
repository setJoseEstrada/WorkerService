using Nucleo.DTO;
using Nucleo.Interface;

namespace GA_Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly SYS_DTO_Conexion oConexiones;

        private AppSettingsManager _settingsManager;

        private ISegundoPLano _iSegundoPLano;
        

        public Worker(ILogger<Worker> logger, AppSettingsManager appSettingsManager, ISegundoPLano segundoPLano)
        {
            _logger = logger;
            _settingsManager = appSettingsManager;
            _iSegundoPLano = segundoPLano;
            oConexiones = new SYS_DTO_Conexion();
            oConexiones  = _settingsManager._oConexiones;

        }

      
       




        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
               
                await _iSegundoPLano.Leer(oConexiones.sRutaPendiente, oConexiones.sRutaProcesados);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); 

            }
        }
    }
}
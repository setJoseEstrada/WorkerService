using Nucleo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleo.Interface
{
    public interface ISegundoPLano
    {
        public Task<List<SYS_DTO_Archivos>>Leer(string sPath, string sPathDestino);
    }
}

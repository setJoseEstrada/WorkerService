using Nucleo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using Nucleo.DTO;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Nucleo
{
    public class SegundoPlano:ISegundoPLano
    {
        public async Task<List<SYS_DTO_Archivos>> Leer(string sPath, string sPathDestino)
        {
            List<SYS_DTO_Archivos> lArchivos = new List<SYS_DTO_Archivos>();

            try
			{
                if (Directory.Exists(sPath))
                {
                    
                    string[] sNombresArchivos = Directory.GetFiles(sPath);

                    Task<List<SYS_DTO_Archivos>> Archivos =  AgregarValores(sNombresArchivos);

                    List<SYS_DTO_Archivos> lista = await Archivos;


                    if(!await APIREST(lista, sPathDestino, sPath))
                    {

                    }



                }
                else
                {
                    Console.WriteLine("La carpeta especificada no existe.");
                }

            }
            catch (Exception ex)
			{

				throw;
			}

            return lArchivos;
        }




        public async Task<List<SYS_DTO_Archivos>> AgregarValores(string[] sArvhivos)
        {
            List<SYS_DTO_Archivos> larchivos = new List<SYS_DTO_Archivos>();

            try
            {

                foreach (var item in sArvhivos)
                {
                    string scontenido =  await File.ReadAllTextAsync(item);

                    SYS_DTO_Archivos oArchivos = new SYS_DTO_Archivos();

                    string[] divisiones = scontenido.Split('|');
                    var id_tienda = new string( divisiones[0].Where(char.IsDigit).ToArray());
                    var sNombre = item.Split('\\');
                    oArchivos.sNombreArchivo = sNombre[3];
                    oArchivos.sTienda = id_tienda;
                    oArchivos.sRegistradora = divisiones[1];
                    oArchivos.sFecha = divisiones[2];
                    oArchivos.sHora = divisiones[3];
                    oArchivos.sTicket = divisiones[4];
                    oArchivos.dImporteImpuesto = Convert.ToDouble( divisiones[5]);
                    oArchivos.dTotal =Convert.ToDouble(divisiones[6]);


                    if(!larchivos.Any(x=> x.sTicket == oArchivos.sTicket))
                    {

                    larchivos.Add(oArchivos);
                    }
                    

                }

            }
            catch (Exception)
            {

                throw;
            }

            return larchivos;
        }


        public async Task<bool>APIREST(List<SYS_DTO_Archivos> sArchivos, string sDestino, string sOrigen)
        {
            Boolean result = false;

            try
            {
                int iLote = 500;
                int iTotalLote = sArchivos.Count();

                for (int i = 0; i < iTotalLote; i+= iLote)
                {
                    List<SYS_DTO_Archivos> lLote = sArchivos.Skip(i).Take(iLote).ToList();

                    string sJson = JsonConvert.SerializeObject(lLote);
                    string urlEnpoint = "https://localhost:44314/Tienda/InsertarDatos";

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                        var contenido = new StringContent(sJson, Encoding.UTF8, "application/json");
                        var respuesta = await httpClient.PostAsync(urlEnpoint, contenido);
                        if (respuesta.IsSuccessStatusCode)
                        {
                            if(!Directory.Exists(sDestino))
                            {

                                Directory.CreateDirectory(sDestino);

                            }

                            string sJsonRespuesta = await respuesta.Content.ReadAsStringAsync();

                            var lListaRespuesta = JsonConvert.DeserializeObject<List<string>>(sJsonRespuesta);

                            foreach(var archivo in lListaRespuesta)
                            {
                                string sNombreArchivo = archivo;
                                string sRutaOrigen = Path.Combine(sOrigen, sNombreArchivo);
                                string sRutaDestino = Path.Combine(sDestino, sNombreArchivo);

                                File.Move(sRutaOrigen, sRutaDestino);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error: {respuesta.StatusCode}");
                        }
                    }

                }


               
               

                
            }
            catch (Exception ex)
            {

                throw;
            }

            return result;
        }
    }
}

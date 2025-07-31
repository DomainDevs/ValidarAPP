using Domain.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ConsultaPoliza2018.Models.Entities
{
    public class Poliza
    {

        public List<string> json { get; set; } = new List<string>();
        public string json2 { get; set; }
        //private 

        public async Task GetResultAsync()
        {
            var ds = new DataSet();
            var parametros = new Dictionary<string, object>();

            try
            {

                //var conexion = "HolaMundo";
                var storeProcedure = "COMM.GET_PENDIENT_OPERATIONS";
                var parametro = 259; // 146;//82;// 115;

                //IDataAccess dataAccess = new Domain.DataAccess.Sybase();                
                //ds = dataAccess.consultarSP(conexion, storeProcedure, parametro);

                var dataAccess = new DataAccessClass("cobosyb1", "SISE3G_PRV", "sa", "123456", BaseDeDatos.Sybase).MethodFactory();
                ds = dataAccess.consultarSP(storeProcedure, parametro);


                var iterator = ds.Tables[0].Rows.GetEnumerator();
                while (iterator.MoveNext())
                {
                    var actual = (DataRow)iterator.Current;

                    
                    json.Add(formatearJson(actual["Operation"].ToString()));
                }


                this.json2 = string.Join(@"\n", json.ToArray());

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e);
            }
        }
        private string formatearJson (string json)
        {
            System.Diagnostics.Debug.WriteLine(json);
            var objeto = JsonConvert.DeserializeObject(json); // (objeto, Formatting.Indented);
            return JsonConvert.SerializeObject(objeto, Formatting.Indented);

        }
    }
}
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sistran.Co.Previsora.Application.FullServicesProvider.Experian.Models;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Co.Previsora.Application.FullServicesProvider.Experian.Builders
{
    public static class ResponseSearchBuilder
    {
        public static ResponseSearch AddExperianLogData(this ResponseSearch rs, ResquestSearch search,CIFIN_LOG log)
        {

            rs.Rol = "Externo";
            rs.NombreRS = log.Nombre;
            rs.CodTipoDoc = Convert.ToInt32(log.cod_tipo_doc);
            rs.Documento = Convert.ToString(log.number_doc);
            rs.TipoDoc = log.CodigoTipoIndentificacion;
            rs.Apellido1 = log.Apellido1;
            rs.Apellido2 = log.Apellido2;
            rs.Nombre = log.Nombre;
            rs.CodigoCiiu = log.CodigoCiiu?.ToInt() ?? 0;
            rs.CodSucursal = search.cod_Rol.ToInt();
            rs.Message = string.Format(Resources.DocumentStateFromExperian, log.Estado);
            if(search.tipo_doc == 1)
            {
                rs.NombreRS = null;
            }
            else
            {
                rs.Apellido1 = null;
                rs.Apellido2 = null;
                rs.Nombre = null;
            }
            return rs;
        }
    }
}

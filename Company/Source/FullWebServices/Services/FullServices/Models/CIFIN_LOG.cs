using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    public class CIFIN_LOG
	{
		#region Properties

        public string cod_tipo_doc
        { get; set; }

        public string number_doc
        { get; set; }

        public string request_date
        { get; set; }

        public string user_log
        { get; set; }

        public string IdentificadorLinea
        { get; set; }

        public string CodigoTipoIndentificacion
        { get; set; }

        public string NumeroIdentificacion
        { get; set; }

        public string NombreTitular
        { get; set; }

		public string  txt_lugar_expedicion
        { get; set; }

        public string CodigoCiiu
        { get; set; }

        public string NombreCiiu
        { get; set; }

        public string Estado
        { get; set; }

        public string Fecha
        { get; set; }

        public string Hora
	    {    get; set; }

        public string Entidad
        { get; set; }

        public string RespuestaConsulta
        { get; set; }

        public string Nombre
        { get; set; }

        public string Apellido1
        { get; set; }

        public string Apellido2
        { get; set; }

        public string CodigoEstado
        { get; set; }

        public int Identity
        { get; set; }

		#endregion

	}
}

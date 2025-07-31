using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    public class ERROR_CIFIN
	{
		#region Properties

        public int CodigoError
        { get; set; }

        public string MensajeError
        { get; set; }

        public string CodigoInformacion
        { get; set; }

        public string MotivoConsulta
        { get; set; }

        public string NumeroIdentificacion
        { get; set; }

        public string TipoIdentificacion
        { get; set; }

        public string Operacion
        { get; set; }

        public int Identity
        { get; set; }

		#endregion

	}
}

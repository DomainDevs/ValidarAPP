using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    public class CIFIN_STATEDOCUMENT
	{
		#region Properties

        public int cod_doc
        { get; set; }

        public string txt_desc_redu
        { get; set; }

        public string txt_desc
        { get; set; }

        public int Identity
        { get; set; }

		#endregion

	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	
    public class CIFIN_DOCUMENTTYPE
	{  

		#region Properties
               
        public double cod_tipo_doc_cifin
        {    get; set; }

        public string txt_desc_redu
        { get; set; }

        public string txt_desc
        { get; set; }

        public double cod_tipo_doc_sise
        { get; set; }

        public string txt_desc_sise
        { get; set; }


		#endregion

	}
}

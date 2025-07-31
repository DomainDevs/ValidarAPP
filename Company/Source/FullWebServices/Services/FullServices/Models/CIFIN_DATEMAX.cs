using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    public class CIFIN_DATEMAX
	{
		#region Properties

        public int Id
        { get; set; }

        public string DateMonthMax
        { get; set; }

        public int LeftOver
        { get; set; }

        public int ContMax
        { get; set; }

        public int SubCon
        { get; set; }

        public int CertDate
        { get; set; }

		#endregion

	}
}

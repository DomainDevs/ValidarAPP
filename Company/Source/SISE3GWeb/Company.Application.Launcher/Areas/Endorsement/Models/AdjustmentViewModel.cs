using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    public class AdjustmentViewModel: EndorsementViewModel
    {
        /// <summary>
        /// fecha de vigencia desde. del endoso
        /// </summary>
        public DateTime CurrentFrom { get; set; }
        /// <summary>
        /// fecha de vigencia hasta. del endoso
        /// </summary>
        public DateTime CurrentTo { get; set; }
        /// <summary>
        ///numero de dias se calcula entre la vigencia hasta menos la vigencia desde
        /// </summary>
        public int Days { get; set; }
        /// <summary>
        /// texto
        /// </summary>
        public String Text { get; set; }
        /// <summary>
        /// Observacion
        /// </summary>
        public String Observation { get; set; }
        /// <summary>
        /// id del riesgo
        /// </summary>
        public int RiskId { get; set; }
        /// <summary>
        /// descripcion del riesgo
        /// </summary>
        public int Risk { get; set; }
        /// <summary>
        /// id del objeto del seguro
        /// </summary>
        public int InsuranceObjectId { get; set; }
        /// <summary>
        /// descripcion  del objeto del seguro
        /// </summary>
        public int InsuranceObject { get; set; }

        /// <summary>
        /// Titulo
        /// </summary>
        public string Title { get; set; }

    }
}
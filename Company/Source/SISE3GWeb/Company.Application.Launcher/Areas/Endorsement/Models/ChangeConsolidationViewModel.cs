using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    public class ChangeConsolidationViewModel : EndorsementViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Nombre Agencia
        /// </summary>
        public string ChangeConsolidationName { get; set; }
        /// <summary>
        /// Participación
        /// </summary>             
        public decimal ChangeConsolidationAddress { get; set; }
        /// <summary>
        /// Porcentaje
        /// </summary>   
        public decimal ChangeConsolidationPhone { get; set; }
        /// <summary>
        /// Porcentaje Adicional
        /// </summary>   
        public decimal? ChangeConsolidationEmail { get; set; }
        /// <summary>
        /// Es Principal?
        /// </summary>
        public bool IsPrincipal { get; set; }
        /// <summary>
        /// Fecha cambio de tomador?
        /// </summary>
        public DateTime ChangeConsolidationFrom { get; set; }
        /// <summary>
        /// Contract
        /// </summary>
        public CompanyIssuanceInsured companyContract { get; set; }
    }
}
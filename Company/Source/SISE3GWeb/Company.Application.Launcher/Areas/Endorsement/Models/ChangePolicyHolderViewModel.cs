using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    public class ChangePolicyHolderViewModel : EndorsementViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Nombre Agencia
        /// </summary>
        public string ChangePolicyHolderName { get; set; }
        /// <summary>
        /// Participación
        /// </summary>             
        public decimal ChangePolicyHolderAddress { get; set; }
        /// <summary>
        /// Porcentaje
        /// </summary>   
        public decimal ChangePolicyHolderPhone { get; set; }
        /// <summary>
        /// Porcentaje Adicional
        /// </summary>   
        public decimal? ChangePolicyHolderEmail { get; set; }
        /// <summary>
        /// Es Principal?
        /// </summary>
        public bool IsPrincipal { get; set; }
        /// <summary>
        /// Fecha cambio de tomador?
        /// </summary>
        public string ChangePolicyHolderFrom { get; set; }
        /// <summary>
        /// Contract
        /// </summary>
        public CompanyIssuanceInsured companyContract { get; set; }
        /// <summary>
        /// Holder
        /// </summary>
        public Holder holder { get; set; }
    }
}
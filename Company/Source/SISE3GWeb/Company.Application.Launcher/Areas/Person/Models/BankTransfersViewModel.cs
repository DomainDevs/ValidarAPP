using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    public class BankTransfersViewModel
    {
        /// <summary>
        /// IndividualId
        /// </summary>
        public int IndividualId { get; set; }

        /// <summary>
        /// Tipo de pago
        /// </summary>
        public int PaymentType { get; set; }

        /// <summary>
        /// Banco
        /// </summary>
        public string BankId { get; set; }

        /// <summary>
        /// Sucursal bancaria
        /// </summary>
        public string BankBranchesId { get; set; }

        /// <summary>
        /// Plaza del banco
        /// </summary>
        public string BankSquare { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Ciudad
        /// </summary>
        public string CityId { get; set; }

        /// <summary>
        /// País
        /// </summary>
        public string CountryId { get; set; }

        /// <summary>
        /// Tipo de cuenta
        /// </summary>
        public string AccountTypeId { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Beneficiario del pago
        /// </summary>
        public string PaymentBeneficiary { get; set; }

        /// <summary>
        /// Beneficiario
        /// </summary>
        public string Beneficiary { get; set; }

        /// <summary>
        /// Número de cuenta
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Cuenta activa?
        /// </summary>
        public string ActiveAccount { get; set; }

        /// <summary>
        /// Cuenta por defecto?
        /// </summary>
        public string DefaultAccount { get; set; }

        /// <summary>
        /// Banco intermediario
        /// </summary>
        public string IntermediaryBank { get; set; }

        /// <summary>
        /// Descripcion bancaria
        /// </summary>
        public string Description { get; set; }
    }
}
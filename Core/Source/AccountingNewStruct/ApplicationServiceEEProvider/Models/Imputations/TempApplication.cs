using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// Modelo de aplicación de pagos temporal
    /// </summary>
    public class TempApplication
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador de Módulo
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// Identificador del recurso origen de aplicación
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// Fecha de registro
        /// </summary>
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// Fecha contable
        /// </summary>
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Identificador de la persona a quien se aplica el pago
        /// </summary>
        public int IndividualId { get; set; }

        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Application premiums
        /// </summary>
        public List<TempApplicationPremium> TempApplicationPremiums { get; set; }

        /// <summary>
        /// Application accounting
        /// </summary>
        public List<ApplicationAccounting> TempApplicationAccountings { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.DTOs
{
    public class SarlaftDTO
    {
        /// <summary>
        /// Obtiene o Setea El identificador
        /// </summary>
        /// <value>
        ///  sarlaft identificador.
        /// </value>
        [DataMember]
        public int SarlaftId { get; set; }

        /// <summary>
        /// Obtiene o Setea Ingresos
        /// </summary>
        /// <value>
        /// Valor Ingresos
        /// </value>
        [DataMember]
        public decimal IncomeAmount { get; set; }

        /// <summary>
        /// Obtiene o Setea Gastos
        /// </summary>
        /// <value>
        /// valor Gastos
        /// </value>
        [DataMember]
        public decimal ExpenseAmount { get; set; }

        /// <summary>
        /// Obtiene o Setea Ingresos Adicionales
        /// </summary>
        /// <value>
        /// valor Ingresos Adicionales
        /// </value>
        [DataMember]
        public decimal ExtraIncomeAmount { get; set; }

        /// <summary>
        /// Obtiene o Setea Bienes
        /// </summary>
        /// <value>
        /// Valor Bienes
        /// </value>
        [DataMember]
        public decimal AssetsAmount { get; set; }

        /// <summary>
        ///  Obtiene o Setea Pasivos
        /// </summary>
        /// <value>
        /// Valor Pasivo
        /// </value>
        [DataMember]
        public decimal LiabilitiesAmount { get; set; }

        /// <summary>
        /// Obtiene o Setea Descripcion
        /// </summary>
        /// <value>
        ///  Descripcion.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o Setea Transaccion en el Extranjero
        /// </summary>
        /// <value>
        /// <c>true</c> Si tiene Transacciones en el Extranjero; si no tiene, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsForeignTransaction { get; set; }

        /// <summary>
        /// Obtiene o Setea Monto Transaccion en el Extranjero
        /// </summary>
        /// <value>
        /// Monto Transaccion en el Extranjero
        /// </value>
        [DataMember]
        public decimal? ForeignTransactionAmount { get; set; }
    }
}


// -----------------------------------------------------------------------
// <copyright file="CptUniqueUserSalePointAlliance.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
using System.Runtime.Serialization;
namespace Sistran.Company.Application.UniqueUserParamService.Models
{
    /// <summary>
    /// Modelo de negocio del punto de venta del usuario.
    /// </summary>
    [DataContract]
    public class CptUniqueUserSalePointAlliance
    {
        /// <summary>
        /// Obtiene o establece el identificador del usuario.
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del aliado.
        /// </summary>
        [DataMember]
        public int AllianceId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la sucursal del aliado.
        /// </summary>
        [DataMember]
        public int BranchAllianceId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del punto de venta del aliado.
        /// </summary>
        [DataMember]
        public int SalePointAllianceId { get; set; }

        /// <summary>
        /// Obtiene o establece el individual id del agente.
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del agente.
        /// </summary>
        [DataMember]
        public int AgentAgencyId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si esta o no asignado el punto de venta.
        /// </summary>
        [DataMember]
        public bool IsAssign { get; set; }        
    }
}

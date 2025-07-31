// -----------------------------------------------------------------------
// <copyright file="ParamCompanyListCombo.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camilo jimenéz</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationParamBusinessService.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// ParamCompanyListCombo.Objeto con Lista de Elementos asociados.
    /// </summary>
    [DataContract]
    public class ParamCompanyListCombo
    {
        /// <summary>
        /// Gets or sets. Nombre del campo de llave primaria de la tabla(Value Member).
        /// </summary>
        [DataMember]
        public string PkColumn { get; set; }

        /// <summary>
        /// Gets or sets. Nombre del campo que contiene la Descripción(Display Member)
        /// </summary>
        [DataMember]
        public string DescriptionColumn { get; set; }

        /// <summary>
        /// Gets or sets. Nombre de la tabla asociada.
        /// </summary>
        [DataMember]
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets. Filtro asociado a la tabla (opcional).
        /// </summary>
        [DataMember]
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets. Orden asociada  a la consulta.
        /// </summary>
        [DataMember]
        public string Order { get; set; }

        /// <summary>
        /// Gets or sets. Elementos asociados al cuadro combinado.
        /// </summary>
        [DataMember]
        public List<ParamCompanyElement> CompanyElements { get; set; }
    }
}

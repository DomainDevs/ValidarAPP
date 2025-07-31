// -----------------------------------------------------------------------
// <copyright file="ParamCoverageView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;

    /// <summary>
    /// View de cobertura con sus relaciones
    /// </summary>
    [Serializable]
    public class ParamCoverageView : BusinessView
    {
        /// <summary>
        /// Obtiene la coleccion de coberturas
        /// </summary>
        public BusinessCollection Coverage
        {
            get
            {
                return this["Coverage"];
            }
        }

        /// <summary>
        /// Obtiene coleccion de CoCoverages
        /// </summary>
        public BusinessCollection CoCoverages
        {
            get
            {
                return this["CoCoverage"];
            }
        }

        /// <summary>
        /// Obtiene la coleccion de ramos tecnicos
        /// </summary>
        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        /// <summary>
        /// Obtiene la coleccion de subramos tecnicos
        /// </summary>
        public BusinessCollection SubLineBusiness
        {
            get
            {
                return this["SubLineBusiness"];
            }
        }

        /// <summary>
        /// Obtiene la coleccion de amparos
        /// </summary>
        public BusinessCollection Perils
        {
            get
            {
                return this["Peril"];
            }
        }

        /// <summary>
        /// Obtiene coleccion de objetos del seguro
        /// </summary>
        public BusinessCollection InsuredObjects
        {
            get
            {
                return this["InsuredObject"];
            }
        }

        /// <summary>
        /// Obtiene coleccion de clauseslevels
        /// </summary>
        public BusinessCollection ClauseLevels
        {
            get
            {
                return this["ClauseLevel"];
            }
        }

        /// <summary>
        /// Obtiene coleccion de deducibles relacionados con la cobertura
        /// </summary>
        public BusinessCollection CoverageDeductibles
        {
            get
            {
                return this["CoverageDeductible"];
            }
        }

        /// <summary>
        /// Obtiene coleccion de tipos de detalle relacionados con la cobertura
        /// </summary>
        public BusinessCollection CoverDetailTypes
        {
            get
            {
                return this["CoverDetailType"];
            }
        }

        /// <summary>
        /// Obtiene coleccion de homologacion de coverturas 2G
        /// </summary>
        public BusinessCollection CoEquivalenceCoverage
        {
            get
            {
                return this["CoEquivalenceCoverage"];
            }
        }
    }
}

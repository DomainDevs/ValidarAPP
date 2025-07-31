// -----------------------------------------------------------------------
// <copyright file="ParamTechnicalPlan.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Alberto Sánchez Lesmes</author>
// -----------------------------------------------------------------------
using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Error;
using System.Collections.Generic;

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    public class BaseParamTechnicalPlan: Extension
    {
        /// <summary>
        /// Identificador del Plan Técnico.
        /// </summary>
        private readonly int _Id;
        /// <summary>
        /// Descripción del Plan Técnico.
        /// </summary>
        private readonly string _Description;
        /// <summary>
        /// Descripción Corta del Plan Técnico.
        /// </summary>
        private readonly string _SmallDescription;
        
        
        /// <summary>
        /// Fecha de Creación
        /// </summary>
        private readonly System.DateTime _CurrentFrom;
        /// <summary>
        /// Fecha de Finalización
        /// </summary>
        private readonly System.DateTime? _CurrentTo;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamTechnicalPlan"/>.
        /// </summary>
        /// <param name="id">Identificador del Plan Técnico.</param>
        /// <param name="description">Descripción del Plan Técnico.</param>
        /// <param name="smallDescription">Descripción Corta del Plan Técnico.</param>
        /// <param name="coveredRiskType">Tipo de Riesgo Cubierto relacionado con el Plan Técnico.</param>
        /// <param name="currentFrom">Fecha de Creación del Plan Técnico.</param>
        /// <param name="currentTo">Fecha de Finalización del Plan Técnico.</param>
        protected BaseParamTechnicalPlan(int id, string description, string smallDescription,  System.DateTime currentFrom, System.DateTime? currentTo)
        {
            _Id = id;
            _Description = description;
            _SmallDescription = smallDescription;
            _CurrentFrom = currentFrom;
            _CurrentTo = currentTo;
        }

        /// <summary>
        /// Obtiene el Id del Plan Técnico.
        /// </summary>
        public int Id
        {
            get
            {
                return _Id;
            }
        }

        /// <summary>
        /// Obtiene la Descripción del Plan Técnico.
        /// </summary>
        public string Description
        {
            get
            {
                return _Description;
            }
        }

        /// <summary>
        /// Obtiene la Descripción Corta del Plan Técnico.
        /// </summary>
        public string SmallDescription
        {
            get
            {
                return _SmallDescription;
            }
        }

       

        /// <summary>
        /// Obtiene la Fecha de Creación del Plan Técnico.
        /// </summary>
        public System.DateTime CurrentFrom
        {
            get
            {
                return _CurrentFrom;
            }
        }

        /// <summary>
        /// Obtiene la Fecha de Finalización del Plan Técnico.
        /// </summary>
        public System.DateTime? CurrentTo
        {
            get
            {
                return _CurrentTo;
            }
        }

        
    }
}

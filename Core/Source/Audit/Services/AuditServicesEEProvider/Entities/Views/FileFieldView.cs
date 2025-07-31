// -----------------------------------------------------------------------
// <copyright file="FileFieldView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuditService.EEProvider.Entities.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;

    /// <summary>
    /// Campos del archivo
    /// </summary>
    [Serializable]
    public class FileFieldView : BusinessView
    {
        /// <summary>
        /// Obtiene configutación archivos
        /// </summary>
        public BusinessCollection Files
        {
            get
            {
                return this["File"];
            }
        }

        /// <summary>
        /// Obtiene plantillas del archivo
        /// </summary>
        public BusinessCollection FileTemplates
        {
            get
            {
                return this["FileTemplate"];
            }
        }

        /// <summary>
        /// Obtiene campos de las plantillas
        /// </summary>
        public BusinessCollection FileTemplateFields
        {
            get
            {
                return this["FileTemplateField"];
            }
        }

        /// <summary>
        /// Obtiene listado de campos
        /// </summary>
        public BusinessCollection Fields
        {
            get
            {
                return this["Field"];
            }
        }

        /// <summary>
        /// Obtiene tipos del campo
        /// </summary>
        public BusinessCollection FieldTypes
        {
            get
            {
                return this["FieldType"];
            }
        }

        /// <summary>
        /// Obtiene listado de plantillas
        /// </summary>
        public BusinessCollection Template
        {
            get
            {
                return this["Template"];
            }
        }
    }
}
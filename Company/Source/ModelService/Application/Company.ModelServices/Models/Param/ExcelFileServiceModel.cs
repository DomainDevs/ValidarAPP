// -----------------------------------------------------------------------
// <copyright file="ErrorServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ModelServices.Models.Param
{
    /// <summary>
    /// Clase publica para archivo de excel
    /// </summary>
    public class ExcelFileServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Datos del archivo de excel
        /// </summary>
        public string FileData { get; set; }
    }
}

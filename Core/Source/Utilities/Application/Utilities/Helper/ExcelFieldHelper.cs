// -----------------------------------------------------------------------
// <copyright file="IUnderwritingParamService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
// -----------------------------------------------------------------------
namespace System
{
    /// <summary>
    /// Clase de ayuda para los textos en los archivos de excel
    /// </summary>
    public static class ExcelFieldHelper
    {
        /// <summary>
        /// Convierte el valor boleano en texto
        /// </summary>
        /// <param name="value">valor boleano</param>
        /// <returns>Texto del boleano</returns>
        public static string ToStringFieldExcel(this bool value)
        {
            if (value)
                return "SI";
            else
                return "NO";
        }
    }
}

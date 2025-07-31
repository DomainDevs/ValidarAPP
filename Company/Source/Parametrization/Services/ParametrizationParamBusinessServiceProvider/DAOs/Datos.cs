// -----------------------------------------------------------------------
// <copyright file="Datos.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>camilo jimenéz</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs
{
    using System;
    using System.Data;
    using Sistran.Co.Application.Data;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Model;

    /// <summary>
    /// Datos.clase con metodos DAOS asociados.
    /// </summary>
    public class Datos
    {
        /// <summary>
        /// GetCombo. Obtiene Datatable con valores asociados.
        /// </summary>
        /// <param name="listCombocompany">Modelo Company con información asociada a la tabla y campos requeridos</param>
        /// <returns>DataTable.tabla con PK y descripción </returns>
        public DataTable GetCombo(ParamCompanyListCombo listCombocompany)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                NameValue[] parameters = new NameValue[5];
                parameters[0] = new NameValue("@SQLPK", listCombocompany.PkColumn??"");
                parameters[1] = new NameValue("@SQLDESCRIP", listCombocompany.DescriptionColumn??"");
                parameters[2] = new NameValue("@SQLTABLE", listCombocompany.Table??"");
                parameters[3] = new NameValue("@SQLWHERE", listCombocompany.Filter??"");
                parameters[4] = new NameValue("@SQLORDER", listCombocompany.Order??"");
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                  ds= pdb.ExecuteSPDataSet("UP.SP_COMBO", parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
    }
}

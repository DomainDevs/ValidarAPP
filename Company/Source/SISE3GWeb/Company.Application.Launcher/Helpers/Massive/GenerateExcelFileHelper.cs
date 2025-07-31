using System;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Configuration;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.CollectiveServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public static class GenerateExcelFile
    {
       
        /// <summary>
        /// obtiene el la posicion exacta de la celda 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        static string GetColumnName(string prefix, uint column) =>
            column < 26 ? $"{prefix}{(char)(65 + column)}" :
            GetColumnName(GetColumnName(prefix, (column - column % 26) / 26 - 1), column % 26);
       public static string GetCellReference(uint row, uint column) => $"{GetColumnName("", column)}{row}";

        public static Cell CreateCell(string value, CellValues dataType, int row, int col, int styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                CellReference = row != 0 ? GetCellReference(Convert.ToUInt32(row), Convert.ToUInt32(col)) : null,
                StyleIndex = Convert.ToUInt32(styleIndex)
            };
        }

        public static MergeCell MergeCell(int row1, int col1, int row2, int col2)
        {
            string cell1 = GetCellReference(Convert.ToUInt32(row1), Convert.ToUInt32(col1));
            string cell2 = GetCellReference(Convert.ToUInt32(row2), Convert.ToUInt32(col2));
            return new MergeCell()
            {
                Reference = new StringValue(cell1 + ":" + cell2)
            };
        }
    }
}
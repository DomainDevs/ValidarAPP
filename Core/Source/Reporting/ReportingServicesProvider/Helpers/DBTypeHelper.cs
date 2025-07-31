using Sistran.Core.Application.ReportingServices.Models;
using System;
using System.Data;

namespace Sistran.Core.Application.ReportingServices.Provider.Helpers
{
    public class DBTypeHelper
    {
        internal static DbType GetDbType(Parameter parameter)
        {
            if (parameter.DbType == typeof(Int32))
            {
                return System.Data.DbType.Int32;
            }
            else if (parameter.DbType == typeof(decimal))
            {
                return System.Data.DbType.Decimal;
            }
            else if (parameter.DbType == typeof(bool))
            {
                return System.Data.DbType.Boolean;
            }
            else
            {
                return System.Data.DbType.String;
            }
        }
    }
}

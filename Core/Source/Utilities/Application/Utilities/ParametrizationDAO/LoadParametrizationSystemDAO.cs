using System.Data;
using Sistran.Core.Application.Utilities.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Co.Application.Data;

namespace Sistran.Core.Application.Utilities.DAOs
{
    using System.Collections.Generic;
    using System.Linq;

    public class LoadParametrizationSystemDAO
    {

        public void LoadEnumParameterValuesFromDB()
        {
            DataTable dataTable;
            using (DynamicDataAccess dataAccess = new DynamicDataAccess())
            {
                dataTable = dataAccess.ExecuteSPDataTable("PARAM.LOAD_SYSTEM_PARAMS");
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                EnumHelper.enumParameterCache.Clear();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    EnumHelper.enumParameterCache.TryAdd(dataRow[0].ToString(), new EnumParameter
                    {
                        Key = dataRow[0].ToString(),
                        Value = dataRow[1],
                        SourceColum = dataRow[2].ToString(),
                        SourceTable = dataRow[3].ToString(),
                        Filter = dataRow[4].ToString(),
                        Description = dataRow[5].ToString()
                    });                   
                }
            }            
        }

        public IEnumerable<int> LoadEnumParameterValuesFromDB(IEnumerable<string> keys)
        {
            DataTable dataTable;
            using (DynamicDataAccess dataAccess = new DynamicDataAccess())
            {
                dataTable = dataAccess.ExecuteSPDataTable("PARAM.LOAD_SYSTEM_PARAMS");
            }

            if (dataTable == null || dataTable.Rows.Count <= 0)
            {
                yield break;
            }

            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (keys.Contains(dataRow[0].ToString()))
                {
                    yield return int.Parse(dataRow[1].ToString());
                }
            }
        }
    }
}

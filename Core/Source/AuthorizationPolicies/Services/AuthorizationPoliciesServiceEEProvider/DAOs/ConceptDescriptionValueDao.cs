using Sistran.Co.Application.Data;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine.StoredProcedure;
using System.Collections;
using System.Data;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    public class ConceptDescriptionValueDao
    {

        public string GetValueConceptDescription(Models.ConceptDescriptionValue descriptionValue, params string[] parameters)
        {
            var filter = string.Format(descriptionValue.Filter, parameters);

            //var pars = new Param[3];
            //pars[0] = new Param("TABLES", descriptionValue.TableName);
            //pars[1] = new Param("FIELDS", descriptionValue.Fields);
            //pars[2] = new Param("FILTER", filter);

            //ArrayList datas = DataFacadeManager.Instance.GetDataFacade().ExecuteSPReader("SCR.GET_DATA_FROM_FILTER", pars);

            NameValue[] pars = new NameValue[3];
            pars[0] = new NameValue("TABLES", descriptionValue.TableName);
            pars[1] = new NameValue("FIELDS", descriptionValue.Fields);
            pars[2] = new NameValue("FILTER", filter);
            DataTable dataTable;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("SCR.GET_DATA_FROM_FILTER", pars);
            }
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return string.Empty;
            }
            return dataTable.Rows[0][0].ToString();
        }
    }
}

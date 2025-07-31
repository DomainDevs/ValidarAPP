using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    class GuaranteeRequiredDocumentBusiness
    {

        /// <summary>
        /// Obtener la documentacion recibida
        /// </summary>
        /// <param name="individualId">individualId Asegurado</param>
        /// <returns></returns>
        public List<Models.GuaranteeRequiredDocument> GetDocumentationReceivedByGuaranteeId(int guaranteeId)
        {
            
            List<Models.GuaranteeRequiredDocument> guaranteeRequiredDocuments = new List<Models.GuaranteeRequiredDocument>();

            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(GuaranteeRequiredDocument.Properties.Description, "grd"), "Description"));
            select.AddSelectValue(new SelectValue(new Column(GuaranteeRequiredDocument.Properties.DocumentCode, "grd"), "DocumentCode"));
            select.AddSelectValue(new SelectValue(new Column(GuaranteeRequiredDocument.Properties.GuaranteeCode, "grd"), "GuaranteeCode"));

            Join join = new Join(new ClassNameTable(typeof(Guarantee), "g"), new ClassNameTable(typeof(GuaranteeRequiredDocument), "grd"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(Guarantee.Properties.GuaranteeCode, "g")
                .Equal()
                .Property(GuaranteeRequiredDocument.Properties.GuaranteeCode, "grd")
                .GetPredicate());

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Guarantee.Properties.GuaranteeCode, "g");
            filter.Equal();
            filter.Constant(guaranteeId);

            select.Table = join;
            select.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                Models.GuaranteeRequiredDocument guaranteeRequiredDocument = null;
                while (reader.Read())
                {
                    guaranteeRequiredDocument = new Models.GuaranteeRequiredDocument
                    {
                        DocumentCode = (int)reader["DocumentCode"],
                        Description = (string)reader["Description"],
                        GuaranteeCode = (int)reader["GuaranteeCode"]
                    };
                    guaranteeRequiredDocuments.Add(guaranteeRequiredDocument);
                }
            }
            return guaranteeRequiredDocuments;
        }
    }
}

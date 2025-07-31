using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using UWEN = Sistran.Company.Application.Issuance.Entities;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class FirstPayComponentDAO
    {
        /// <summary>
        /// Obtener la lista de primer componente de pago según el Id del plan financiero
        /// </summary>
        /// <param name="financialPlanId"></param>
        /// <returns></returns>
        public List<FirstPayComponent> GetListFirstPayComponentByFinancialPlanId(int financialPlanId)
        {
            List<FirstPayComponent> listFirstPayComponent = new List<FirstPayComponent>();
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.Table = new ClassNameTable(typeof(UWEN.FirstPayComponent));
            ObjectCriteriaBuilder objectCriteriaBuilder = new ObjectCriteriaBuilder();
            objectCriteriaBuilder.Property(UWEN.FirstPayComponent.Properties.FinancialPlanId);
            objectCriteriaBuilder.Equal();
            objectCriteriaBuilder.Constant(financialPlanId);

            BusinessCollection businessCollection = null;
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UWEN.FirstPayComponent), objectCriteriaBuilder.GetPredicate()));
            }
            if (businessCollection != null & businessCollection.Any())
            {
                listFirstPayComponent = ModelAssembler.CreateFirstPayComponents(businessCollection);
            }
            return listFirstPayComponent;
        }
    }
}
using Sistran.Company.Application.DeclarationBusinessService;
using Sistran.Company.Application.DeclarationBusinessServiceEEProvider.Business;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Linq;
using Sistran.Company.Application.DeclarationBusinessServiceEEProvider.Resources;
using Sistran.Core.Framework.Queries;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using System.Data;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Collections.Generic;

namespace Sistran.Company.Application.DeclarationBusinessServiceEEProvider
{
    public class DeclarationBusinessEEProvider : IDeclarationBusinessService
    {
        /// <summary>
        /// Crea un Endorsement
        /// </summary>
        public CompanyPolicy CreateEndorsementDeclaration(CompanyPolicy companyPolicy, Dictionary<string, object> formValues)
        {
            try
            {
                DeclarationPropertyBusiness declarationPropertyBusiness = new DeclarationPropertyBusiness();
                return declarationPropertyBusiness.CreateEndorsementDeclaration(companyPolicy, formValues);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Error.ErrorCreateDeclarationEndorsement), ex);
            }
        }

        public bool ValidateDeclarativeInsuredObjects(decimal policyId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, "er");
                filter.Equal();
                filter.Constant(policyId);
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, "er");
                filter.Equal();
                filter.Constant(1);
                filter.And();
                filter.Property(QUOEN.InsuredObject.Properties.IsDeclarative, "io");
                filter.Equal();
                filter.Constant(1);
                //columnas que devuelve
                SelectQuery SelectQuery = new SelectQuery();
                SelectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.InsuredObject.Properties.IsDeclarative, "io"), "IsDeclarative"));

                Join join = new Join(new ClassNameTable(typeof(ISSEN.EndorsementRisk), "er"), new ClassNameTable(typeof(ISSEN.RiskInsuredObject), "rio"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(ISSEN.EndorsementRisk.Properties.RiskId, "er")
                    .Equal()
                    .Property(ISSEN.RiskInsuredObject.Properties.RiskId, "rio")
                    .GetPredicate());
                join = new Join(join, new ClassNameTable(typeof(QUOEN.InsuredObject), "io"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(ISSEN.RiskInsuredObject.Properties.InsuredObjectId, "rio")
                    .Equal()
                    .Property(QUOEN.InsuredObject.Properties.InsuredObjectId, "io")
                    .GetPredicate());
                SelectQuery.Table = join;
                SelectQuery.Where = filter.GetPredicate();
                SelectQuery.Distinct = true;
                //SelectQuery.GetFirstSelect();

                bool result = false;
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(SelectQuery))
                {
                    while (reader.Read())
                    {
                        result = (bool)reader["IsDeclarative"];
                        break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

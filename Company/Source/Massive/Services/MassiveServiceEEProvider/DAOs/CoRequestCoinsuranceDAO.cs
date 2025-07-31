using Sistran.Core.Application.CommonService.Models;
using Sistran.Company.Application.MassiveServices.EEProvider.Assemblers;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.MassiveServices.EEProvider.Entities.View;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Company.Application.Request.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Request.Entities;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.MassiveServices.EEProvider.DAOs
{
    /// <summary>
    /// Tipo de Negocio
    /// </summary>
    public class CoRequestCoinsuranceDAO
    {

        public List<CompanyIssuanceCoInsuranceCompany> GetCoRequestCoinsuranceByRequedIdByRequestEndorsementIdType(int requestId, int requestEndorsementId, BusinessType businessType)
        {
            CoRequestCoinsuranceDAO coRequestCoinsuranceDAO = new CoRequestCoinsuranceDAO();
            List<CompanyIssuanceCoInsuranceCompany> coInsurancesCompany = new List<CompanyIssuanceCoInsuranceCompany>();
            switch (businessType)
            {
                case BusinessType.Accepted:
                    CompanyIssuanceCoInsuranceCompany coInsuranceCompany = coRequestCoinsuranceDAO.GetCoRequestCoinsuranceAcceptedByRequedIdByRequestEndorsementId(requestId, requestEndorsementId);
                    if (coInsurancesCompany != null)
                        coInsurancesCompany.Add(coInsuranceCompany);
                    break;
                    
                case BusinessType.Assigned:
                    coInsurancesCompany.AddRange(coRequestCoinsuranceDAO.GetCoRequestCoinsuranceAssignedByRequedIdByRequestEndorsementId(requestId, requestEndorsementId));
                    break;
            }
            return coInsurancesCompany;
        }

        /// <summary>
        /// Crear tipo de Negocio Coaseguro Cedido en la Solicitud
        /// </summary>
        /// <param name="InsuranceCompanies">The insurance companies.</param>
        /// <param name="requestId">The request identifier.</param>
        /// <param name="requestEndorsementId">The request endorsement identifier.</param>
        public void CreateCoRequestCoinsuranceAssigned(List<IssuanceCoInsuranceCompany> InsuranceCompanies, int requestId, int requestEndorsementId)
        {
            for (int i = 0; i < InsuranceCompanies.Count; i++)
            {
                CoRequestCoinsuranceAssigned coRequestEntity = EntityAssembler.CreateCoRequestCoinsuranceAssigned(InsuranceCompanies[i], requestId, requestEndorsementId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(coRequestEntity);
            }
        }

        /// <summary>
        /// Crear tipo de Negocio Coaseguro Aceptado en la Solicitud
        /// </summary>
        /// <param name="InsuranceCompanies">The insurance companies.</param>
        /// <param name="requestId">The request identifier.</param>
        /// <param name="requestEndorsementId">The request endorsement identifier.</param>
        public void CreateCoRequestCoinsuranceAccepted(IssuanceCoInsuranceCompany insuranceCompanies, int requestId, int requestEndorsementId)
        {
            CoRequestCoinsuranceAccepted coRequestEntity = EntityAssembler.CreateCoRequestCoinsuranceAccepted(insuranceCompanies, requestId, requestEndorsementId);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(coRequestEntity);
        }

        /// <summary>
        /// Obtener tipo de Negocio Coaseguro Aceptado en la Solicitud
        /// </summary>
        /// <param name="requestId">Numero Solicitud Agrupadora</param>
        /// <param name="requestEndorsementId">Numero Endoso Solicitud Agrupadora.</param>
        public CompanyIssuanceCoInsuranceCompany GetCoRequestCoinsuranceAcceptedByRequedIdByRequestEndorsementId(int requestId, int requestEndorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoRequestCoinsuranceAccepted.Properties.RequestId, typeof(CoRequestCoinsuranceAccepted).Name);
            filter.Equal();
            filter.Constant(requestId);
            filter.And();
            filter.Property(CoRequestCoinsuranceAccepted.Properties.RequestEndorsementId, typeof(CoRequestCoinsuranceAccepted).Name);
            filter.Equal();
            filter.Constant(requestEndorsementId);
            CoRequestCoinsuranceAccepted coRequestEntity = (CoRequestCoinsuranceAccepted)DataFacadeManager.Instance.GetDataFacade().List(typeof(CoRequestCoinsuranceAccepted), filter.GetPredicate()).FirstOrDefault();
            if (coRequestEntity != null)
            {
                CompanyIssuanceCoInsuranceCompany InsuranceCompanies = ModelAssembler.CreateCoRequestCoinsuranceAccepted(coRequestEntity);
                InsuranceCompanies.Description = DelegateService.underwritingService.GetCoInsuranceCompanyByCoinsuranceId((int)InsuranceCompanies.Id).Description;
                return InsuranceCompanies;
            }
            return null;
        }

        /// <summary>
        /// Obtener tipo de Negocio Coaseguro Cedido en la Solicitud
        /// </summary>
        /// <param name="requestId">Numero Solicitud Agrupadora</param>
        /// <param name="requestEndorsementId">Numero Endoso Solicitud Agrupadora.</param>
        public List<CompanyIssuanceCoInsuranceCompany> GetCoRequestCoinsuranceAssignedByRequedIdByRequestEndorsementId(int requestId, int requestEndorsementId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoRequestCoinsuranceAssigned.Properties.RequestId, typeof(CoRequestCoinsuranceAssigned).Name);
            filter.Equal();
            filter.Constant(requestId);
            filter.And();
            filter.Property(CoRequestCoinsuranceAssigned.Properties.RequestEndorsementId, typeof(CoRequestCoinsuranceAssigned).Name);
            filter.Equal();
            filter.Constant(requestEndorsementId);
            CoRequestCoinsuranceAssignedView view = new CoRequestCoinsuranceAssignedView();
            ViewBuilder builder = new ViewBuilder("CoRequestCoinsuranceAssignedView");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            return ModelAssembler.CreateCoRequestCoinsuranceAssigned(view);
        }
    }
}

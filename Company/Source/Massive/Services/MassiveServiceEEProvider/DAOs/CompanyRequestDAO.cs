using System;
using System.Linq;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.MassiveServices.Models;
using REQEN = Sistran.Company.Application.Request.Entities;
using Sistran.Company.Application.MassiveServices.EEProvider.Assemblers;
using System.Collections.Generic;
using Sistran.Core.Framework.Queries;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Company.Application.MassiveServices.EEProvider.Entities.View;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.MassiveServices.EEProvider.DAOs
{
    public class CompanyRequestDAO
    {
        /// <summary>
        /// Crear Una Solicitud Agrupadora
        /// </summary>
        /// <param name="companyRequest">Solicitud Agrupadora</param>
        /// <returns>Solicitud Agrupadora</returns>
        public CompanyRequest CreateCompanyRequest(CompanyRequest companyRequest)
        {
            REQEN.CoRequest entityCoRequest = EntityAssembler.CreateCoRequest(companyRequest);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCoRequest);
            companyRequest.Id = entityCoRequest.RequestId;

            REQEN.CoRequestEndorsement entityCoRequestEndorsement = EntityAssembler.CreateCoRequestEndorsement(companyRequest.CompanyRequestEndorsements.First());
            entityCoRequestEndorsement.RequestId = companyRequest.Id;
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCoRequestEndorsement);
            companyRequest.CompanyRequestEndorsements.First().Id = entityCoRequestEndorsement.RequestEndorsementId;

            foreach (IssuanceAgency agency in companyRequest.CompanyRequestEndorsements.First().Agencies)
            {
                REQEN.CoRequestAgent entityCoRequestAgent = EntityAssembler.CreateCoRequest(agency);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCoRequestAgent);

                REQEN.CoRequestEndorsementAgent entityCoRequestEndorsementAgent = new REQEN.CoRequestEndorsementAgent(entityCoRequestEndorsement.RequestEndorsementId, entityCoRequestAgent.RequestAgentId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCoRequestEndorsementAgent);
            }

            return companyRequest;
        }

        /// <summary>
        /// Guarda en base de datos Renovacion solicitud agrupadora
        /// </summary>
        /// <param name="request"> Modelo de solicitud agrupadora </param>
        /// <param name="userId"> Identificador del usuario </param>
        /// <returns> Modelo de la solicitud agrupadora </returns>
        public Models.CompanyRequest SaveRenewalRequest(Models.CompanyRequest request, int userId)
        {

            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            //using (Context.Current)
            //{
            //    using (Transaction transaction = new Transaction())
            //    {
            //        try
            //        {
            //            PrimaryKey key = CoRequest.CreatePrimaryKey(request.Id);
            //            CoRequest coRequestEntity = (CoRequest)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            //            if (coRequestEntity != null)
            //            {
            //                coRequestEntity.BusinessTypeCode = (int)request.BusinessType;
            //                DataFacadeManager.Instance.GetDataFacade().UpdateObject(coRequestEntity);
            //            }
            //            Models.CoRequestEndorsement requestEndorsement = request.CoRequestEndorsement.FirstOrDefault();
            //            int count = requestEndorsement.DocumentNum + 1;
            //            requestEndorsement.DocumentNum = count;
            //            CoRequestEndorsementDAO requestEndorsementDAO = new CoRequestEndorsementDAO();
            //            requestEndorsement = requestEndorsementDAO.CreateCoRequestEndorsement(requestEndorsement, userId, request.Id);
            //            CoRequestAgentDAO requestAgentDAO = new CoRequestAgentDAO();
            //            requestAgentDAO.CreateAgents(requestEndorsement.CoRequestAgent, requestEndorsement.Id);
            //            CoRequestCoinsuranceDAO requestCoinsuranceDAO = new CoRequestCoinsuranceDAO();
            //            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //            switch (request.BusinessType)
            //            {
            //                case BusinessType.Accepted:
            //                    filter = new ObjectCriteriaBuilder();
            //                    filter.PropertyEquals(CoRequestCoinsuranceAccepted.Properties.RequestId, request.Id);
            //                    filter.And();
            //                    filter.PropertyEquals(CoRequestCoinsuranceAccepted.Properties.RequestEndorsementId, requestEndorsement.Id);
            //                    DataFacadeManager.Instance.GetDataFacade().Delete<CoRequestCoinsuranceAccepted>(filter.GetPredicate());
            //                    requestCoinsuranceDAO.CreateCoRequestCoinsuranceAccepted(request.InsuranceCompanies.FirstOrDefault(), request.Id, requestEndorsement.Id);
            //                    break;
            //                case BusinessType.Assigned:
            //                    filter = new ObjectCriteriaBuilder();
            //                    filter.PropertyEquals(CoRequestCoinsuranceAssigned.Properties.RequestId, request.Id);
            //                    filter.And();
            //                    filter.PropertyEquals(CoRequestCoinsuranceAssigned.Properties.RequestEndorsementId, requestEndorsement.Id);
            //                    DataFacadeManager.Instance.GetDataFacade().Delete<CoRequestCoinsuranceAssigned>(filter.GetPredicate());
            //                    requestCoinsuranceDAO.CreateCoRequestCoinsuranceAssigned(request.InsuranceCompanies, request.Id, requestEndorsement.Id);
            //                    break;
            //            }
            //            transaction.Complete();
            //            stopWatch.Stop();
            //            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.MassiveServices.EEProvider.DAOs.SaveCoRequest");
            //            request.CoRequestEndorsement[0] = requestEndorsement;
            //            return request;
            //        }

            //        catch (Exception exception)
            //        {
            //            throw new BusinessException("SaveCoRequest", exception);
            //        }

            //    }
            //}
            return null;
        }

        /// <summary>
        /// Buscar Solicitudes por código
        /// </summary>
        /// <param name="requestId">Codigo de solicitud</param>        
        /// <returns></returns>    
        public Models.CompanyRequest GetCoRequestByRequestId(int requestId)
        {
            List<CompanyRequest> listCompanyRequests = new List<CompanyRequest>();
            CompanyRequest companyRequests = new CompanyRequest();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (requestId > 0)
            {
                filter.Property(REQEN.CoRequest.Properties.RequestId, typeof(REQEN.CoRequestEndorsement).Name);
                filter.Equal();
                filter.Constant(requestId);
            }
            CoRequestView view = new CoRequestView();
            ViewBuilder builder = new ViewBuilder("CoRequestView");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.CoRequest.Count > 0)
            {
                listCompanyRequests = ModelAssembler.CreateCoRequests(view.CoRequest);

                foreach (Models.CompanyRequest request in listCompanyRequests)
                {
                    if (request != null)
                    {
                        companyRequests = request;
                        break;
                    }
                }
            }
            return companyRequests;

            #region Old
            //CoRequestView view = new CoRequestView();
            //ViewBuilder builder = new ViewBuilder("CoRequestView");

            ////Se consulta el max documentnum de la solicitud
            //SelectQuery select = new SelectQuery();
            //Function function = new Function(FunctionType.Max);
            //function.AddParameter(new Column(CoRequestEndorsement.Properties.DocumentNum, typeof(CoRequestEndorsement).Name));
            //select.AddSelectValue(new SelectValue(function, "DocumentNum"));
            //ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //filter.Property(CoRequestEndorsement.Properties.RequestId, typeof(CoRequestEndorsement).Name);
            //filter.Equal();
            //filter.Constant(requestId);
            //select.Table = new ClassNameTable(typeof(CoRequestEndorsement));
            //select.Where = filter.GetPredicate();
            //int maxDocumentNum = 0;
            //using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            //{
            //    while (reader.Read())
            //    {
            //        maxDocumentNum = reader.GetInt32(0);
            //    }
            //}

            //filter.Clear();
            //filter.Property(CoRequest.Properties.RequestId, typeof(CoRequest).Name);
            //filter.Equal();
            //filter.Constant(requestId);
            //filter.And();
            //filter.Property(CoRequestEndorsement.Properties.DocumentNum, typeof(CoRequestEndorsement).Name);
            //filter.Equal();
            //filter.Constant(maxDocumentNum);


            //builder.Filter = filter.GetPredicate();
            //DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            //stopWatch.Stop();
            //Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.MassiveServices.EEProvider.DAOs.GetCoRequestByRequestId");

            //Models.CompanyRequest request = ModelAssembler.CreateCoRequests(view.CoRequest, view.CoRequestEndorsement, view.CoRequestEndorsementAgent, view.CoRequestAgent, view.Agent, view.BillingGroup).FirstOrDefault();
            //if (request != null && request.CoRequestEndorsement != null)
            //{
            //    CoRequestCoinsuranceDAO coRequestCoinsuranceDAO = new CoRequestCoinsuranceDAO();
            //    switch (request.BusinessType)
            //    {
            //        case BusinessType.Accepted:
            //            request.InsuranceCompanies = new List<CoInsuranceCompany>();
            //            request.InsuranceCompanies.Add(coRequestCoinsuranceDAO.GetCoRequestCoinsuranceAcceptedByRequedIdByRequestEndorsementId(request.Id, request.CoRequestEndorsement.FirstOrDefault().Id));
            //            break;
            //        case BusinessType.Assigned:
            //            request.InsuranceCompanies = coRequestCoinsuranceDAO.GetCoRequestCoinsuranceAssignedByRequedIdByRequestEndorsementId(request.Id, request.CoRequestEndorsement.FirstOrDefault().Id);
            //            break;
            //    }
            //    request.CoRequestEndorsement.FirstOrDefault().Corequest = null;
            //}
            //return request;
            //return null;
            #endregion old
        }

        /// <summary>
        /// Obtener Solicitudes Agrupadoras Por Grupo Facturación, Id O Descripción Solicitud
        /// </summary>
        /// <param name="billingGroup">Id Grupo Facturación</param>
        /// <param name="description">Id O Descripción Solicitud</param>
        /// <returns>Solicitudes Agrupadoras</returns>
        public List<CompanyRequest> GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber(int billingGroupId, string description, int? requestNumber)
        {
            List<CompanyRequest> companyRequests = new List<CompanyRequest>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int32 requestId = 0;
            Int32.TryParse(description, out requestId);

            if (requestId > 0)
            {
                filter.Property(REQEN.CoRequest.Properties.RequestId, typeof(REQEN.CoRequestEndorsement).Name);
                filter.Equal();
                filter.Constant(requestId);
            }
            //else
            //{
            //    filter.Property(REQEN.CoRequest.Properties.Description, typeof(REQEN.CoRequest).Name);
            //    filter.Like();
            //    filter.Constant("%" + description + "%");
            //}

            if (billingGroupId > 0)
            {
                filter.And();
                filter.Property(ISSEN.BillingGroup.Properties.BillingGroupCode, typeof(ISSEN.BillingGroup).Name);
                filter.Equal();
                filter.Constant(billingGroupId);
            }

            if (requestNumber.HasValue)
            {
                filter.And();
                filter.Property(REQEN.CoRequestEndorsement.Properties.DocumentNum, typeof(REQEN.CoRequestEndorsement).Name);
                filter.Equal();
                filter.Constant(requestNumber.Value);
            }

            CoRequestView view = new CoRequestView();
            ViewBuilder builder = new ViewBuilder("CoRequestView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.CoRequest.Count > 0)
            {
                companyRequests = ModelAssembler.CreateCoRequests(view.CoRequest, view.CoRequestEndorsement, view.CoRequestEndorsementAgent, view.CoRequestAgent, view.Agent, view.BillingGroup);

                if (companyRequests.Count == 1)
                {
                    foreach (IssuanceAgency item in companyRequests[0].CompanyRequestEndorsements.Last().Agencies)
                    {
                        IssuanceAgency agency = DelegateService.underwritingService.GetAgencyByAgentIdAgentAgencyId(item.Agent.IndividualId, item.Id);
                        item.Code = agency.Code;
                        item.FullName = agency.FullName;
                        item.Branch = agency.Branch;
                    }
                }

                foreach (Models.CompanyRequest request in companyRequests)
                {
                    if (request != null && request.CompanyRequestEndorsements != null)
                    {
                        CoRequestCoinsuranceDAO coRequestCoinsuranceDAO = new CoRequestCoinsuranceDAO();
                        switch (request.BusinessType)
                        {
                            case BusinessType.Accepted:
                                request.InsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>();
                                request.InsuranceCompanies.Add(coRequestCoinsuranceDAO.GetCoRequestCoinsuranceAcceptedByRequedIdByRequestEndorsementId(request.Id, request.CompanyRequestEndorsements.FirstOrDefault().Id));
                                break;
                            case BusinessType.Assigned:
                                request.InsuranceCompanies = coRequestCoinsuranceDAO.GetCoRequestCoinsuranceAssignedByRequedIdByRequestEndorsementId(request.Id, request.CompanyRequestEndorsements.FirstOrDefault().Id);
                                break;
                        }
                    }
                }
            }

            return companyRequests;
        }

        /// <summary>
        /// Buscar Solicitudes Por Id o Descripción
        /// </summary>
        /// <param name="description">Id o Descripción</param>
        /// <returns>Lista de Solicitudes</returns>
        public List<Models.CompanyRequest> GetCoRequestByDescription(string description)
        {
            //List<Models.CompanyRequest> coRequests = new List<Models.CompanyRequest>();
            //ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //int requestId = 0;
            //int.TryParse(description, out requestId);

            //if (requestId > 0)
            //{
            //    filter.Property(CoRequest.Properties.RequestId, typeof(CoRequest).Name).Equal().Constant(requestId);
            //}
            //else
            //{
            //    filter.Property(CoRequest.Properties.Description, typeof(CoRequest).Name).Like().Constant("%" + description + "%");
            //}

            //BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoRequest), filter.GetPredicate()));

            //if (businessCollection.Count > 0)
            //{
            //    coRequests = ModelAssembler.CreateCoRequests(businessCollection);
            //}

            //return coRequests;
            return null;
        }


        public CompanyRequestEndorsement GetCompanyRequestEndorsmentPolicyWithRequest(DateTime PolicyFrom, CompanyRequest companyRequest)
        {
            foreach (var item in companyRequest.CompanyRequestEndorsements)
            {
                if (PolicyFrom  >= item.CurrentFrom  && PolicyFrom < item.CurrentTo)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
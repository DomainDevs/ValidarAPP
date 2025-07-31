using AutoMapper;
using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyPartnerBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyPartnerBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        #region Partner V1
        public CompanyPartner GetCompanyPartnerByDocumentIdDocumentTypeIndividualId( String documentId, int documentType, int individualId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPartner();
                var result = coreProvider.GetPartnerByDocumentIdDocumentTypeIndividualId( documentId, documentType, individualId);
                return imapper.Map<Partner, CompanyPartner>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }


        }

        public List<CompanyPartner> GetCompanyPartnerByIndividualId(int individualId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPartner();
                var result = coreProvider.GetPartnerByIndividualId(individualId);
                return imapper.Map<List<Partner>, List<CompanyPartner>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPartner CreateCompanyPartner(CompanyPartner partner)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPartner();
                var partnerCore = imapper.Map<CompanyPartner, Partner>(partner);
                var result = coreProvider.CreatePartner(partnerCore);
                return imapper.Map<Partner, CompanyPartner>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyPartner CreateCompanyPartner(PartnerDTO partner)
        {
            try
            {
                return new CompanyPartner()
                {
                    Active = partner.Active,
                    IdentificationDocument = new CompanyIdentificationDocument()
                    {
                        DocumentType = new CompanyDocumentType() { Id = Convert.ToInt16(partner.DocumentTypeId) },
                        Number = partner.IdentificationDocumentNumber
                    },
                    IndividualId = partner.IndividualId,
                    PartnerId = partner.PartnerId,
                    TradeName = partner.TradeName
                };
            
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyPartner UpdateCompanyPartner(CompanyPartner partner)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPartner();
                var partnerCore = imapper.Map<CompanyPartner, Partner>(partner);
                var result = coreProvider.UpdatePartner(partnerCore);
                return imapper.Map<Partner, CompanyPartner>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion Partner V1
    }
}

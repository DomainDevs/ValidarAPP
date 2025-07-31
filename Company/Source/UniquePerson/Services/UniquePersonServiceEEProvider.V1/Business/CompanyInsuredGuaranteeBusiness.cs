using System.Collections.Generic;
using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyInsuredGuaranteeBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyInsuredGuaranteeBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyGuaranteeInsuredGuarantee> GetCompanyInsuredGuaranteesByIndividualId(int individualId)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var result = coreProvider.GetInsuredGuaranteeByIndividualId(individualId);
            return imapper.Map<List<GuaranteeInsuredGuarantee>, List<CompanyGuaranteeInsuredGuarantee>>(result);
        }

        #region PromissoryNote

        internal CompanyInsuredGuaranteePromissoryNote GetCompanyInsuredGuaranteePromissoryNoteByIdByIndividualId(int individualId, int id)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var result = coreProvider.GetInsuredGuaranteePromissoryNoteByIndividualIdById(individualId, id);
            return imapper.Map<InsuredGuaranteePromissoryNote, CompanyInsuredGuaranteePromissoryNote>(result);
        }

        internal CompanyInsuredGuaranteePromissoryNote CreateCompanyInsuredGuaranteePromissoryNote(CompanyInsuredGuaranteePromissoryNote companyInsuredGuaranteePromissoryNote)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var insuredGuaranteePromissoryNoteCore = imapper.Map<CompanyInsuredGuaranteePromissoryNote, InsuredGuaranteePromissoryNote>(companyInsuredGuaranteePromissoryNote);
            var result = coreProvider.CreateInsuredGuaranteePromissoryNote(insuredGuaranteePromissoryNoteCore);
            return imapper.Map<InsuredGuaranteePromissoryNote, CompanyInsuredGuaranteePromissoryNote>(result);
        }

        internal CompanyInsuredGuaranteePromissoryNote UpdateCompanyInsuredGuaranteePromissoryNote(CompanyInsuredGuaranteePromissoryNote companyInsuredGuaranteePromissoryNote)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var insuredGuaranteePromissoryNoteCore = imapper.Map<CompanyInsuredGuaranteePromissoryNote, InsuredGuaranteePromissoryNote>(companyInsuredGuaranteePromissoryNote);
            var result = coreProvider.UpdateInsuredGuaranteePromissoryNote(insuredGuaranteePromissoryNoteCore);
            return imapper.Map<InsuredGuaranteePromissoryNote, CompanyInsuredGuaranteePromissoryNote>(result);
        }


        #endregion

        #region Pledge
        internal CompanyInsuredGuaranteePledge GetCompanyInsuredGuaranteePledgeByIdByIndividualId(int individualId, int id)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var result = coreProvider.GetInsuredGuaranteePledgeByIndividualIdById(individualId, id);
            return imapper.Map<InsuredGuaranteePledge, CompanyInsuredGuaranteePledge>(result);
        }

        internal CompanyInsuredGuaranteePledge CreateCompanyInsuredGuaranteePledge(CompanyInsuredGuaranteePledge companyInsuredGuaranteePledge)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var insuredGuaranteePledgeCore = imapper.Map<CompanyInsuredGuaranteePledge, InsuredGuaranteePledge>(companyInsuredGuaranteePledge);
            var result = coreProvider.CreateInsuredGuaranteePledge(insuredGuaranteePledgeCore);
            return imapper.Map<InsuredGuaranteePledge, CompanyInsuredGuaranteePledge>(result);
        }

        internal CompanyInsuredGuaranteePledge UpdateCompanyInsuredGuaranteePledge(CompanyInsuredGuaranteePledge companyInsuredGuaranteePledge)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var insuredGuaranteePledgeCore = imapper.Map<CompanyInsuredGuaranteePledge, InsuredGuaranteePledge>(companyInsuredGuaranteePledge);
            var result = coreProvider.UpdateInsuredGuaranteePledge(insuredGuaranteePledgeCore);
            return imapper.Map<InsuredGuaranteePledge, CompanyInsuredGuaranteePledge>(result);
        }

        #endregion

        #region Mortgage

        internal CompanyInsuredGuaranteeMortgage GetCompanyInsuredGuaranteeMortgageByIdByIndividualId(int individualId, int id)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var result = coreProvider.GetInsuredGuaranteeMortgageByIndividualIdById(individualId, id);
            return imapper.Map<InsuredGuaranteeMortgage, CompanyInsuredGuaranteeMortgage>(result);
        }

        internal CompanyInsuredGuaranteeMortgage CreateCompanyCompanyInsuredGuaranteeMortgage(CompanyInsuredGuaranteeMortgage companyInsuredGuaranteeMortgage)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var insuredGuaranteeMortgageCore = imapper.Map<CompanyInsuredGuaranteeMortgage, InsuredGuaranteeMortgage>(companyInsuredGuaranteeMortgage);
            var result = coreProvider.CreateInsuredGuaranteeMortgage(insuredGuaranteeMortgageCore);
            return imapper.Map<InsuredGuaranteeMortgage, CompanyInsuredGuaranteeMortgage>(result);
        }

        internal CompanyInsuredGuaranteeMortgage UpdateCompanyCompanyInsuredGuaranteeMortgage(CompanyInsuredGuaranteeMortgage companyInsuredGuaranteeMortgage)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var insuredGuaranteeMortgageCore = imapper.Map<CompanyInsuredGuaranteeMortgage, InsuredGuaranteeMortgage>(companyInsuredGuaranteeMortgage);
            var result = coreProvider.UpdateInsuredGuaranteeMortgage(insuredGuaranteeMortgageCore);
            return imapper.Map<InsuredGuaranteeMortgage, CompanyInsuredGuaranteeMortgage>(result);
        }

        #endregion

        #region FixedTermDeposit

        internal CompanyInsuredGuaranteeFixedTermDeposit GetCompanyInsuredGuaranteeFixedTermDepositByIdByIndividualId(int individualId, int id)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var result = coreProvider.GetInsuredGuaranteeFixedTermDepositByIndividualIdById(individualId, id);
            return imapper.Map<InsuredGuaranteeFixedTermDeposit, CompanyInsuredGuaranteeFixedTermDeposit>(result);
        }

        internal CompanyInsuredGuaranteeFixedTermDeposit CreateCompanyInsuredGuaranteeFixedTermDeposit(CompanyInsuredGuaranteeFixedTermDeposit guaranteeFixedTermDeposit)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var insuredGuaranteeFixedTermDeposit = imapper.Map<CompanyInsuredGuaranteeFixedTermDeposit, InsuredGuaranteeFixedTermDeposit>(guaranteeFixedTermDeposit);
            var result = coreProvider.CreateInsuredGuaranteeFixedTermDeposit(insuredGuaranteeFixedTermDeposit);
            return imapper.Map<InsuredGuaranteeFixedTermDeposit, CompanyInsuredGuaranteeFixedTermDeposit>(result);
        }

        internal CompanyInsuredGuaranteeFixedTermDeposit UpdateCompanyInsuredGuaranteeFixedTermDeposit(CompanyInsuredGuaranteeFixedTermDeposit guaranteeFixedTermDeposit)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var insuredGuaranteeFixedTermDeposit = imapper.Map<CompanyInsuredGuaranteeFixedTermDeposit, InsuredGuaranteeFixedTermDeposit>(guaranteeFixedTermDeposit);
            var result = coreProvider.UpdateInsuredGuaranteeFixedTermDeposit(insuredGuaranteeFixedTermDeposit);
            return imapper.Map<InsuredGuaranteeFixedTermDeposit, CompanyInsuredGuaranteeFixedTermDeposit>(result);
        }

        #endregion

        #region Others

        internal CompanyInsuredGuaranteeOthers GetCompanyInsuredGuaranteeOthersByIndividualIdById(int individualId, int id)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var result = coreProvider.GetInsuredGuaranteeOthersByIndividualIdById(individualId, id);
            return imapper.Map<InsuredGuaranteeOthers, CompanyInsuredGuaranteeOthers>(result);
        }

        internal CompanyInsuredGuaranteeOthers CreateCompanyInsuredGuaranteeOthers(CompanyInsuredGuaranteeOthers insuredGuaranteeOthers)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var CoreinsuredGuaranteeOthers = imapper.Map<CompanyInsuredGuaranteeOthers, InsuredGuaranteeOthers>(insuredGuaranteeOthers);
            var result = coreProvider.CreateInsuredGuaranteeOthers(CoreinsuredGuaranteeOthers);
            return imapper.Map<InsuredGuaranteeOthers, CompanyInsuredGuaranteeOthers>(result);
        }

        internal CompanyInsuredGuaranteeOthers UpdateCompanyInsuredGuaranteeOthers(CompanyInsuredGuaranteeOthers insuredGuaranteeOthers)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var CoreinsuredGuaranteeOthers = imapper.Map<CompanyInsuredGuaranteeOthers, InsuredGuaranteeOthers>(insuredGuaranteeOthers);
            var result = coreProvider.UpdateInsuredGuaranteeOthers(CoreinsuredGuaranteeOthers);
            return imapper.Map<InsuredGuaranteeOthers, CompanyInsuredGuaranteeOthers>(result);
        }

        #endregion

        internal CompanyInsuredGuaranteePrefix CreateCompanyCompanyInsuredGuaranteePrefix(CompanyInsuredGuaranteePrefix companyInsuredGuaranteeMortgage)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var insuredGuaranteeMortgageCore = imapper.Map<CompanyInsuredGuaranteePrefix, InsuredGuaranteePrefix>(companyInsuredGuaranteeMortgage);
            var result = coreProvider.CreateInsuredGuaranteePrefix(insuredGuaranteeMortgageCore);
            return imapper.Map<InsuredGuaranteePrefix, CompanyInsuredGuaranteePrefix>(result);
        }


    }
}

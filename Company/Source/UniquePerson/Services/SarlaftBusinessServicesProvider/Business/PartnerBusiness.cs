using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.Assemblers;
using Sistran.Company.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UPCEN = Sistran.Company.Application.UniquePerson.Entities;
using UCCEN = Sistran.Core.Application.UniquePerson.Entities;

namespace Sistran.Company.Application.SarlaftBusinessServicesProvider.Business
{
    public class PartnerBusiness
    {
        public CompanyIndividualPartner CreatePartner(CompanyIndividualPartner partner)
        {
            UPCEN.IndividualPartner entityPartner = EntityAssembler.CreatePartner(partner);


            SelectQuery selectQuery = new SelectQuery();
            Function function = new Function(FunctionType.Max);

            function.AddParameter(new Column(UPCEN.IndividualPartner.Properties.PartnerId));

            selectQuery.Table = new ClassNameTable(typeof(UPCEN.IndividualPartner));
            selectQuery.AddSelectValue(new SelectValue(function));

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    entityPartner.PartnerId = (Convert.ToInt32(reader[0]) + 1);
                }
            }
            DataFacadeManager.Insert(entityPartner);
            return ModelAssembler.CreateIndividualPartner(entityPartner);
        }

        public CompanyIndividualPartner UpdatePartner(CompanyIndividualPartner partner)
        {
            UPCEN.IndividualPartner entityPartner = EntityAssembler.CreatePartner(partner);
            DataFacadeManager.Update(entityPartner);
            return ModelAssembler.CreateIndividualPartner(entityPartner);
        }

        public void DeletePartner(CompanyIndividualPartner partner)
        {
            DeleteCoBeneficiaryPartners(partner);
            PrimaryKey partnerPrimaryKey = UPCEN.IndividualPartner.CreatePrimaryKey(partner.Id, partner.IdCardNumero, partner.IndividualId, partner.DocumentTypeId, partner.SarlaftId);
            DataFacadeManager.Delete(partnerPrimaryKey);
        }

        public void DeleteCoBeneficiaryPartners(CompanyIndividualPartner partner)
        {
            List<CompanyIndividualPartner> companyIndividualPartners = new List<CompanyIndividualPartner>();
            companyIndividualPartners.Add(partner);
            companyIndividualPartners = GetCoBeneficiaryPartners(companyIndividualPartners);
            if (companyIndividualPartners.FirstOrDefault().FinalBeneficiary != null
                && companyIndividualPartners.FirstOrDefault().FinalBeneficiary.Count > 0)
            {
                foreach (var beneficiary in companyIndividualPartners.FirstOrDefault().FinalBeneficiary)
                {
                    PrimaryKey partnerPrimaryKey = UPCEN.CoBeneficiaryPartner.CreatePrimaryKey(beneficiary.Id, beneficiary.IdCardNumero, beneficiary.IndividualId, beneficiary.DocumentTypeId, beneficiary.SarlaftId);
                    DataFacadeManager.Delete(partnerPrimaryKey);
                }
            }
        }

        public List<CompanyIndividualPartner> GetPartnersByIndividualId(int individualId, int sarlaftId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPCEN.IndividualPartner.Properties.IndividualId, typeof(UPCEN.IndividualPartner).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(UPCEN.IndividualPartner.Properties.SarlaftId, typeof(UPCEN.IndividualPartner).Name);
            filter.Equal();
            filter.Constant(sarlaftId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(IndividualPartner), filter.GetPredicate());
            List<CompanyIndividualPartner> partners = ModelAssembler.CreateIndividualPartners(businessCollection);

            return GetCoBeneficiaryPartners(partners);

        }

        public List<CompanyIndividualPartner> GetCoBeneficiaryPartners(List<CompanyIndividualPartner> ListPartners)
        {
            foreach (CompanyIndividualPartner item in ListPartners)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UPCEN.CoBeneficiaryPartner.Properties.IndividualId, typeof(UPCEN.CoBeneficiaryPartner).Name);
                filter.Equal();
                filter.Constant(item.IndividualId);
                filter.And();
                filter.Property(UPCEN.CoBeneficiaryPartner.Properties.PartnerId, typeof(UPCEN.CoBeneficiaryPartner).Name);
                filter.Equal();
                filter.Constant(item.Id);
                filter.And();
                filter.Property(UPCEN.CoBeneficiaryPartner.Properties.SarlaftId, typeof(UPCEN.CoBeneficiaryPartner).Name);
                filter.Equal();
                filter.Constant(item.SarlaftId);

                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(CoBeneficiaryPartner), filter.GetPredicate());
                if (businessCollection.Count > 0)
                {
                    item.FinalBeneficiary = ModelAssembler.CreateBeneficiaryPartner(businessCollection);
                }
            }
            return ListPartners;

        }



        public List<CompanyIndividualPartner> GetCoPartnersByIndividualId(List<CompanyIndividualPartner> ListPartners)
        {
            foreach (CompanyIndividualPartner item in ListPartners)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UPCEN.CoIndividualPartner.Properties.IndividualId, typeof(UPCEN.CoIndividualPartner).Name);
                filter.Equal();
                filter.Constant(item.IndividualId);
                filter.And();
                filter.Property(UPCEN.CoIndividualPartner.Properties.PartnerId, typeof(UPCEN.CoIndividualPartner).Name);
                filter.Equal();
                filter.Constant(item.Id);

                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(CoIndividualPartner), filter.GetPredicate());
                if (businessCollection.Count > 0)
                {
                    item.CoIndividualPartner = ModelAssembler.CreateCoIndividualPartner((CoIndividualPartner)businessCollection[0]);
                }
            }
            return ListPartners;

        }

        #region CoPartner
        public CompanyFinalBeneficiary CreateBeneficiaryPartner(CompanyFinalBeneficiary beneficiaryPartner)
        {
            UPCEN.CoBeneficiaryPartner entityBeneficiaryPartner = EntityAssembler.CreateBeneficiaryPartner(beneficiaryPartner);

            DataFacadeManager.Insert(entityBeneficiaryPartner);

            return ModelAssembler.CreateBeneficiaryPartner(entityBeneficiaryPartner);
        }

        public List<CompanyFinalBeneficiary> CreateBeneficiaryPartner(List<CompanyFinalBeneficiary> finalBeneficiaryList, int Id, int individualid)
        {
            List<CompanyFinalBeneficiary> finalBeneficiaryListnew = new List<CompanyFinalBeneficiary>();
            foreach (CompanyFinalBeneficiary item in finalBeneficiaryList)
            {
                if ((item.Id == 0 && item.IndividualId == 0) || (item.Id > 0 && item.Id != Id))
                {
                    item.Id = Id;
                    item.IndividualId = individualid;
                }
                finalBeneficiaryListnew.Add(this.CreateBeneficiaryPartner(item));
            }


            return finalBeneficiaryListnew;
        }




        public CompanyIndividualPartner CreateCoPartner(CompanyIndividualPartner partner)
        {
            partner.CoIndividualPartner.Id = partner.Id;
            partner.CoIndividualPartner.IndividualId = partner.IndividualId;

            UPCEN.CoIndividualPartner entityPartner = EntityAssembler.CreateCoPartner(partner.CoIndividualPartner);


            DataFacadeManager.Insert(entityPartner);
            partner.CoIndividualPartner = ModelAssembler.CreateCoIndividualPartner(entityPartner);
            return partner;
        }

        public List<CompanyRole> GetRoles()
        {
            List<CompanyRole> roles;
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(UCCEN.Role));
            if (businessCollection.Count > 0)
            {
                roles = ModelAssembler.CreateRoles(businessCollection);
            }
            else
            {
                roles = new List<CompanyRole>();
            }

            return roles;

        }


        #endregion
    }
}

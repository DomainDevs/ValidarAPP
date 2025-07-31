using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPCEN = Sistran.Company.Application.UniquePerson.Entities;

namespace Sistran.Company.Application.SarlaftBusinessServicesProvider.Business
{
    public class LegalRepresentativeBusiness
    {
       
        public CompanyLegalRepresentative CreateRepresentLegal(CompanyLegalRepresentative legalRepresent)
        {
            UPCEN.IndividualLegalRepresent legalRepresentEntity = EntityAssembler.CreateLegalRepresentative(legalRepresent);

            if(this.VerifyExistence(this.GetLegalRepresentByIndividualId(legalRepresent.IndividualId, legalRepresent.SarlaftId), legalRepresentEntity)) 
               DataFacadeManager.Insert(legalRepresentEntity);
            else
                DataFacadeManager.Update(legalRepresentEntity);

            return ModelAssembler.CreateLegalRepresent(legalRepresentEntity);
        }

        public CompanyLegalRepresentative UpdateRepresentLegal(CompanyLegalRepresentative legalRepresent)
        {
            UPCEN.IndividualLegalRepresent legalRepresentEntity = EntityAssembler.CreateLegalRepresentative(legalRepresent);
            DataFacadeManager.Update(legalRepresentEntity);
            return ModelAssembler.CreateLegalRepresent(legalRepresentEntity);
        }

        public List<CompanyLegalRepresentative> GetLegalRepresentByIndividualId(int individualId, int sarlaftId)
        {
            List<CompanyLegalRepresentative> companyLegalRepresentatives = new List<CompanyLegalRepresentative>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPCEN.IndividualLegalRepresent.Properties.IndividualId, typeof(UPCEN.IndividualLegalRepresent).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(UPCEN.IndividualLegalRepresent.Properties.SarlaftId, typeof(UPCEN.IndividualLegalRepresent).Name);
            filter.Equal();
            filter.Constant(sarlaftId);

            var businessCollection = DataFacadeManager.GetObjects(typeof(UPCEN.IndividualLegalRepresent), filter.GetPredicate());
            if (businessCollection != null)
            {
                companyLegalRepresentatives = ModelAssembler.CreateLegalRepresents(businessCollection);
                //filter = new ObjectCriteriaBuilder();
                //filter.Property(UPCEN.IndividualSubstituteLegalRepresent.Properties.IndividualId, typeof(UPCEN.IndividualSubstituteLegalRepresent).Name);
                //filter.Equal();
                //filter.Constant(individualId);
                //var businessCollection2 = DataFacadeManager.GetObjects(typeof(UPCEN.IndividualSubstituteLegalRepresent), filter.GetPredicate());
                //if(businessCollection2 != null)
                //{
                //    var result = ModelAssembler.CreateSubstituteLegalRepresents(businessCollection2);
                //    foreach (var item in result)
                //    {
                //        companyLegalRepresentatives.Add(item);
                //    }
                //}
                return companyLegalRepresentatives;
            }
            else
            {
                return null;
            }
        }

        public CompanyLegalRepresentative CreateSubstituteRepresentLegal(CompanyLegalRepresentative legalRepresent)
        {
            UPCEN.IndividualSubstituteLegalRepresent individualSubstituteLegalRepresentEntity = EntityAssembler.CreateSubstituteLegalRepresentative(legalRepresent);
            DataFacadeManager.Insert(individualSubstituteLegalRepresentEntity);
            return ModelAssembler.CreateLegalRepresent(individualSubstituteLegalRepresentEntity);
        }

        public CompanyLegalRepresentative UpdateSubstituteRepresentLegal(CompanyLegalRepresentative legalRepresent)
        {
            UPCEN.IndividualSubstituteLegalRepresent individualSubstituteLegalRepresentEntity = EntityAssembler.CreateSubstituteLegalRepresentative(legalRepresent);
            DataFacadeManager.Update(individualSubstituteLegalRepresentEntity);
            return ModelAssembler.CreateLegalRepresent(individualSubstituteLegalRepresentEntity);
        }

        public Boolean VerifyExistence(List<CompanyLegalRepresentative> LegalRepresents, UPCEN.IndividualLegalRepresent Entity)
        {
            Boolean band = true;
            if(LegalRepresents != null && LegalRepresents.Count != 0)
            {
                foreach(CompanyLegalRepresentative nodo in LegalRepresents )
                {
                    if (nodo.LegalRepresentType == Entity.LegalRepresentTypeCode)
                    {
                        band = false;
                        break;
                    }

                };

            }
            
            return band;
        }
    }
}

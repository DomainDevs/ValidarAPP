using Sistran.Company.Application.SarlaftBusinessServices.Models;
using UPCEN = Sistran.Company.Application.UniquePerson.Entities;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.SarlaftBusinessServicesProvider.Business
{
    public class IndividualLinkBusiness
    {
        public List<CompanyRelationShip> GetRelationship()
        {
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(UPCEN.RelationshipSarlaft));

            return ModelAssembler.CreateRelationShips(businessCollection);
        }

        public List<CompanyIndvidualLink> GetIndividualLinksByIndividualId(int individualId, int sarlaftId)
        {
            List<CompanyIndvidualLink> companyIndvidualLinks = new List<CompanyIndvidualLink>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(UPCEN.IndividualLink.Properties.IndividualId, typeof(UPCEN.IndividualLink).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(UPCEN.IndividualLink.Properties.SarlaftId, typeof(UPCEN.IndividualLink).Name);
            filter.Equal();
            filter.Constant(sarlaftId);

            IndividualLinkView individualLinkView = new IndividualLinkView();
            ViewBuilder viewBuilder = new ViewBuilder("IndividualLinkView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, individualLinkView);

            if (individualLinkView.IndividualLinks.Count > 0)
            {
                companyIndvidualLinks = ModelAssembler.CreateIndividualLinks(individualLinkView.IndividualLinks);

                foreach (CompanyIndvidualLink companyIndvidualLink in companyIndvidualLinks)
                {
                    companyIndvidualLink.LinkType.Description = individualLinkView.LinkTypes.Cast<UPCEN.LinkType>().First(x => x.LinkTypeCode == companyIndvidualLink.LinkType.Id).Description;
                    companyIndvidualLink.RelationshipSarlaft.Description = individualLinkView.RelationShipSarlafts.Cast<UPCEN.RelationshipSarlaft>().First(x => x.RelationshipSarlaftCode == companyIndvidualLink.RelationshipSarlaft.Id).Description;
                }

                return companyIndvidualLinks;
            }
            else
            {
                return companyIndvidualLinks;
            }
        }

        public CompanyIndvidualLink CreateIndividualLink(CompanyIndvidualLink companyIndvidualLink)
        {
            UPCEN.IndividualLink entityIndividualLink = EntityAssembler.CreateIndvidualLink(companyIndvidualLink);
            DataFacadeManager.Insert(entityIndividualLink);
            return ModelAssembler.CreateIndividualLink(entityIndividualLink);
        }

        public CompanyIndvidualLink UpdateIndividualLink(CompanyIndvidualLink companyIndvidualLink)
        {
            CompanyIndvidualLink companyIndLink = new CompanyIndvidualLink();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPCEN.IndividualLink.Properties.IndividualId);
            filter.Equal();
            filter.Constant(companyIndvidualLink.IndividualId);
            filter.And();
            filter.Property(UPCEN.IndividualLink.Properties.RelationshipSarlaftCode);
            filter.Equal();
            filter.Constant(companyIndvidualLink.RelationshipSarlaft.Id);
            filter.And();
            filter.Property(UPCEN.IndividualLink.Properties.SarlaftId);
            filter.Equal();
            filter.Constant(companyIndvidualLink.SarlaftId);

            if (DataFacadeManager.GetObjects(typeof(UPCEN.IndividualLink), filter.GetPredicate()).Count > 0)
            {
                var entityIndLink = DataFacadeManager.GetObjects(typeof(UPCEN.IndividualLink), filter.GetPredicate()).Cast<UPCEN.IndividualLink>().First();

                PrimaryKey IndLinkPrimaryKey = UPCEN.IndividualLink.CreatePrimaryKey(entityIndLink.IndividualId, entityIndLink.LinkTypeCode, entityIndLink.RelationshipSarlaftCode, entityIndLink.SarlaftId);
                DataFacadeManager.Delete(IndLinkPrimaryKey);

                UPCEN.IndividualLink entityIndividualLink = EntityAssembler.CreateIndvidualLink(companyIndvidualLink);
                DataFacadeManager.Insert(entityIndividualLink);
                companyIndLink = ModelAssembler.CreateIndividualLink(entityIndividualLink);
            }
            return companyIndLink;
        }
    }
}

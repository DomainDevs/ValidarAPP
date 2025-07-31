using Sistran.Core.Application.UniquePersonV1.Entities;
using MOUP = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Framework.BAF;
using System.Data;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class PartnerBusiness
    {

        

        /// <summary>
        /// Gets the partner by document identifier document type individual identifier.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <returns></returns>
        public MOUP.Partner GetPartnerByDocumentIdDocumentTypeIndividualId(String documentId, int documentType, int IndividualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filterDocument = new ObjectCriteriaBuilder();
            filterDocument.Property(IndividualPartner.Properties.IdCardNo, typeof(IndividualPartner).Name);
            filterDocument.Equal();
            filterDocument.Constant(documentId);
            filterDocument.And();
            filterDocument.Property(IndividualPartner.Properties.IdCardTypeCode, typeof(IndividualPartner).Name);
            filterDocument.Equal();
            filterDocument.Constant(documentType);
            filterDocument.And();
            filterDocument.Property(IndividualPartner.Properties.IndividualId, typeof(IndividualPartner).Name);
            filterDocument.Equal();
            filterDocument.Constant(IndividualId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(IndividualPartner), filterDocument.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPartnerByDocumentIdDocumentTypeIndividualId");
            return ModelAssembler.CreateIndividualPartNer(businessCollection);
        }

        /// <summary>
        /// Gets the partner by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public List<MOUP.Partner> GetPartnerByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filterIndividualId = new ObjectCriteriaBuilder();
            filterIndividualId.Property(IndividualPartner.Properties.IndividualId, typeof(IndividualPartner).Name);
            filterIndividualId.Equal();
            filterIndividualId.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(IndividualPartner), filterIndividualId.GetPredicate()));
            List<MOUP.Partner> partners = ModelAssembler.GetIndividualPartNer(businessCollection);

            DocumentTypeBusiness documentTypeBusiness = new DocumentTypeBusiness();
            List<MOUP.DocumentType> pts = new List<MOUP.DocumentType>();
            foreach (MOUP.Partner item in partners)
            {

                pts = documentTypeBusiness.GetDocumentTypes(item.IdentificationDocument.DocumentType.Id);
                if (pts.LongCount() != 0)
                {
                    item.IdentificationDocument.DocumentType.Description = pts.First(x => x.Id == item.IdentificationDocument.DocumentType.Id).SmallDescription;

                }

            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPartnerByIndividualId");
            return partners;
        }

        /// <summary>
        /// Creates the partner.
        /// </summary>
        /// <param name="partNer">The part ner.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public MOUP.Partner CreatePartner(MOUP.Partner partner)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            IndividualPartner partnerEntity = EntityAssembler.IndividualPartnerFields(partner);
            SelectQuery selectQuery = new SelectQuery();
            Function function = new Function(FunctionType.Max);

            function.AddParameter(new Column(IndividualPartner.Properties.PartnerId));
            selectQuery.Table = new ClassNameTable(typeof(IndividualPartner), "IndividualPartner");
            selectQuery.AddSelectValue(new SelectValue(function));

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    partnerEntity.PartnerId = (Convert.ToInt32(reader[0]) + 1);
                }
            }
            DataFacadeManager.Insert(partnerEntity);
            MOUP.Partner partnerModel = ModelAssembler.CreateIndividualPartNer(partnerEntity);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreatePartner");
            return partnerModel;
        }

        /// <summary>
        /// Updates the partner.
        /// </summary>
        /// <param name="partner">The partner.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">El Accionista ya Existe</exception>
        public MOUP.Partner UpdatePartner(MOUP.Partner partner)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PrimaryKey key = IndividualPartner.CreatePrimaryKey(partner.PartnerId, partner.IdentificationDocument.Number,partner.IndividualId,partner.IdentificationDocument.DocumentType.Id);
            var parnerEntity = (IndividualPartner)DataFacadeManager.GetObject(key);
            parnerEntity.TradeName = partner.TradeName;
            parnerEntity.Active = partner.Active;

            DataFacadeManager.Update(parnerEntity);
            MOUP.Partner partnerModel = ModelAssembler.CreateIndividualPartNer(parnerEntity);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdatePartner");
            return partnerModel;
        }
    }
}

using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{

    /// <summary>
    /// Accionistas Asociados
    /// </summary>
    public class PartnerDAO
    {

        /// <summary>
        /// Gets the partner by document identifier document type individual identifier.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.Partner GetPartnerByDocumentIdDocumentTypeIndividualId(string documentId, int documentType, int IndividualId)
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
        public List<Models.Partner> GetPartnerByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filterIndividualId = new ObjectCriteriaBuilder();
            filterIndividualId.Property(IndividualPartner.Properties.IndividualId, typeof(IndividualPartner).Name);
            filterIndividualId.Equal();
            filterIndividualId.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(IndividualPartner), filterIndividualId.GetPredicate()));
            List<Models.Partner> partners = ModelAssembler.GetIndividualPartNer(businessCollection);

            DocumentTypeDAO documentTypeDAO = new DocumentTypeDAO();
            List<Models.DocumentType> pts = new List<Models.DocumentType>();
            foreach (Models.Partner item in partners)
            {

                pts = documentTypeDAO.GetDocumentTypes(item.IdentificationDocument.DocumentType.Id);
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
        public Models.Partner CreatePartner(Partner partNer, int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            partNer.IndividualId = individualId;
            IndividualPartner individualPartnerEntity = EntityAssembler.IndividualPartnerFields(partNer);
            individualPartnerEntity.IndividualId = individualId;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.InsertObject(individualPartnerEntity);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreatePartner");
            return ModelAssembler.CreateIndividualPartNer(individualPartnerEntity);
        }

        /// <summary>
        /// Updates the partner.
        /// </summary>
        /// <param name="partner">The partner.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">El Accionista ya Existe</exception>
        public Models.Partner UpdatePartner(Partner partner, int individualId)
        {

            try
            {
                //validar que no exista
                ObjectCriteriaBuilder filterDocument = new ObjectCriteriaBuilder();
                filterDocument.Property(IndividualPartner.Properties.IndividualId, typeof(IndividualPartner).Name);
                filterDocument.Equal();
                filterDocument.Constant(individualId);
                filterDocument.And();
                filterDocument.Property(IndividualPartner.Properties.IdCardNo, typeof(IndividualPartner).Name);
                filterDocument.Equal();
                filterDocument.Constant(partner.IdentificationDocument.Number);



                IndividualPartner individualPartnerEntityUpdate = (IndividualPartner)DataFacadeManager.Instance.GetDataFacade().List(typeof(IndividualPartner), filterDocument.GetPredicate()).FirstOrDefault();
                if (individualPartnerEntityUpdate != null)
                {
                    individualPartnerEntityUpdate.TradeName = partner.TradeName;
                    individualPartnerEntityUpdate.Active = partner.Active;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade()) {
                        daf.UpdateObject(individualPartnerEntityUpdate);
                    }
                    return ModelAssembler.CreateIndividualPartNer(individualPartnerEntityUpdate);
                }

                return CreatePartner(partner, individualId);

            }
            catch (DuplicatedObjectException)
            {
                throw new BusinessException("El Accionista ya Existe");
            }
        }
    }
}

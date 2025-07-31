using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;
using models = Sistran.Core.Application.UniquePersonService.Models;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Tipos de Documento
    /// </summary>
    public class DocumentTypeDAO
    {
        /// <summary>
        /// Obtener los tipos de documentos
        /// </summary>
        /// <param name="typeDocument">tipo de documento
        /// 1. persona natural
        /// 2. persona juridica
        /// 3. todos</param>
        /// <returns></returns>
        public List<Models.DocumentType> GetDocumentTypes(int typeDocument)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Models.DocumentType> documentTypes = new List<models.DocumentType>();

            switch (typeDocument)
            {
                case 1:
                    BusinessCollection businessCollection1 = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(IdentityCardType)));
                    documentTypes = ModelAssembler.CreateIdentityCardTypes(businessCollection1);
                    break;
                case 2:
                    BusinessCollection businessCollection2 = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TributaryIdentityType)));
                    documentTypes = ModelAssembler.CreateTributaryIdentityTypes(businessCollection2);
                    break;
                case 3:
                    BusinessCollection businessCollection3 = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(DocumentType)));
                    documentTypes = ModelAssembler.CreateDocumentTypes(businessCollection3);
                    break;
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetDocumentTypes");
            return documentTypes;
        }

        internal Models.DocumentType GetDocumentTypeById(int documentTypeId)
        {
            PrimaryKey primaryKey = DocumentType.CreatePrimaryKey(documentTypeId);
            DocumentType entityDocumentsType = (DocumentType)DataFacadeManager.GetObject(primaryKey);

            return ModelAssembler.CreateDocumentType(entityDocumentsType);
        }
    }
}

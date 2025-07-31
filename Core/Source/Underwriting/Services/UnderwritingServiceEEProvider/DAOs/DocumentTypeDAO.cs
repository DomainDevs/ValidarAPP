using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
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
        public List<IssuanceDocumentType> GetDocumentTypes(int typeDocument)
        {
            List<IssuanceDocumentType> documentTypes = new List<IssuanceDocumentType>();

            switch (typeDocument)
            {
                case 1:
                    BusinessCollection businessCollection1 = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.IdentityCardType)));
                    documentTypes = ModelAssembler.CreateIdentityCardTypes(businessCollection1);
                    break;
                case 2:
                    BusinessCollection businessCollection2 = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.TributaryIdentityType)));
                    documentTypes = ModelAssembler.CreateTributaryIdentityTypes(businessCollection2);
                    break;
                case 3:
                    BusinessCollection businessCollection3 = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.DocumentType)));
                    documentTypes = ModelAssembler.CreateDocumentTypes(businessCollection3);
                    break;
            }

            return documentTypes;
        }
    }
}
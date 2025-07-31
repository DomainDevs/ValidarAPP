using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs
{
    public class CompanyDocumentTypeDAO
    {
        internal CompanyDocumentType GetDocumentTypeById(int documentTypeId)
        {
            PrimaryKey primaryKey = DocumentType.CreatePrimaryKey(documentTypeId);
            DocumentType entityDocumentsType = (DocumentType)DataFacadeManager.GetObject(primaryKey);

            return ModelAssembler.CreateDocumentType(entityDocumentsType);
        }
    }
}

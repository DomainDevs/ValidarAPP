using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs
{
    public class DocumentTypeRangeDAO
    {
        internal Models.DocumentTypeRange UpdateDocumentTypeRange(Models.DocumentTypeRange documentTypeRange)
        {
            DocumentsTypeRange entityDocumentTypeRange = EntityAssembler.CreateDocumentTypeRange(documentTypeRange);
            DataFacadeManager.Update(entityDocumentTypeRange);

            return ModelAssembler.CreateDocumentTypeRange(entityDocumentTypeRange);
        }


    }
}

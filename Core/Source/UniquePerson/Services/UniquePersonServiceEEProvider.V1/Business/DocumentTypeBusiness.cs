using Sistran.Core.Application.UniquePersonService.V1.Enums;
using MOUP= Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class DocumentTypeBusiness
    {
        /// <summary>
        /// Obtener los tipos de documentos
        /// </summary>
        /// <param name="typeDocument">tipo de documento
        /// 1. persona natural
        /// 2. persona juridica
        /// 3. todos</param>
        /// <returns></returns>
        public List<MOUP.DocumentType> GetDocumentTypes(int typeDocument)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<MOUP.DocumentType> documentTypes = new List<MOUP.DocumentType>();

            switch (typeDocument)
            {
                case 1:
                    documentTypes = ModelAssembler.CreateIdentityCardTypes(DataFacadeManager.GetObjects(typeof(IdentityCardType)));
                    break;
                case 2:
                    documentTypes = ModelAssembler.CreateTributaryIdentityTypes(DataFacadeManager.GetObjects(typeof(TributaryIdentityType)));
                    break;
                case 3:
                    documentTypes = ModelAssembler.CreateDocumentTypes(DataFacadeManager.GetObjects(typeof(DocumentType)));
                    break;
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetDocumentTypes");
            return documentTypes;
        }
        
    }
}
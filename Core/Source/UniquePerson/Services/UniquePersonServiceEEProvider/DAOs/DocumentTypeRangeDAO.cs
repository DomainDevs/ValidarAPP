using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    public class DocumentTypeRangeDAO
    {
        internal Models.DocumentTypeRange CreateDocumentTypeRange(Models.DocumentTypeRange documentTypeRange)
        {
            DocumentsTypeRange entityDocumentTypeRange = EntityAssembler.CreateDocumentTypeRange(documentTypeRange);

            SelectQuery selectQuery = new SelectQuery();
            Function funtion = new Function(FunctionType.Max);

            funtion.AddParameter(new Column(DocumentsTypeRange.Properties.DocumentsTypeRangeCode, typeof(DocumentsTypeRange).Name));

            selectQuery.Table = new ClassNameTable(typeof(DocumentsTypeRange), typeof(DocumentsTypeRange).Name);
            selectQuery.AddSelectValue(new SelectValue(funtion, typeof(DocumentsTypeRange).Name));

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    entityDocumentTypeRange.DocumentsTypeRangeCode = (Convert.ToInt32(reader[0]) + 1);
                }
            }

            DataFacadeManager.Insert(entityDocumentTypeRange);

            return ModelAssembler.CreateDocumentTypeRange(entityDocumentTypeRange);
        }

        internal Models.DocumentTypeRange UpdateDocumentTypeRange(Models.DocumentTypeRange documentTypeRange)
        {
            DocumentsTypeRange entityDocumentTypeRange = EntityAssembler.CreateDocumentTypeRange(documentTypeRange);
            DataFacadeManager.Update(entityDocumentTypeRange);

            return ModelAssembler.CreateDocumentTypeRange(entityDocumentTypeRange);
        }

        internal void DeleteDocumentTypeRange(int documentTypeRangeId)
        {
            PrimaryKey primaryKey = DocumentsTypeRange.CreatePrimaryKey(documentTypeRangeId);
            DataFacadeManager.Delete(primaryKey);
        }

        internal List<Models.DocumentTypeRange> GetDocumentsTypeRangeId(int documentTypeRangeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(DocumentsTypeRange.Properties.DocumentsTypeRangeCode, documentTypeRangeId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(DocumentsTypeRange), filter.GetPredicate());

            return ModelAssembler.CreateDocumentsTypeRange(businessCollection);
        }

        internal List<Models.DocumentTypeRange> GetDocumentTypeRange()
        {
            return ModelAssembler.CreateDocumentsTypeRange(DataFacadeManager.GetObjects(typeof(DocumentsTypeRange)));
        }
 
    }
}

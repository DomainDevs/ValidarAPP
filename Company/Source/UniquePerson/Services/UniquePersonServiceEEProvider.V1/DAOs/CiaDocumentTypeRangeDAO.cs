using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entities = Sistran.Core.Application.UniquePerson.Entities;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs
{
    public class CiaDocumentTypeRangeDAO
    {

        internal Models.CiaDocumentTypeRange GetCiaDocumentTypeRangeId(int documentTypeRangeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (documentTypeRangeId != 0)
            {
                filter.Property(entities.CiaDocumentsTypeRange.Properties.DocumentsTypeRangeCode);
                filter.Equal();
                filter.Constant(documentTypeRangeId);
            }
            BusinessCollection<entities.CiaDocumentsTypeRange> businessCollection = new BusinessCollection<entities.CiaDocumentsTypeRange>();
            businessCollection = DataFacadeManager.Instance.GetDataFacade().List<entities.CiaDocumentsTypeRange>(filter.GetPredicate());

            if (businessCollection.Count > 0)
            {
                return ModelAssembler.CreateCiaDocumentTypeRange(businessCollection[0]);
            }
            else
            {
                return null;
            }
        }

        internal Models.CiaDocumentTypeRange CreateCiaDocumentTypeRange(Models.CiaDocumentTypeRange ciaDocumentTypeRange)
        {
            CiaDocumentsTypeRange entityCiaDocumentTypeRange = EntityAssembler.CreateCiaDocumentTypeRange(ciaDocumentTypeRange);

            SelectQuery selectQuery = new SelectQuery();
            Function funtion = new Function(FunctionType.Max);

            funtion.AddParameter(new Column(DocumentsTypeRange.Properties.DocumentsTypeRangeCode, typeof(CiaDocumentsTypeRange).Name));

            selectQuery.Table = new ClassNameTable(typeof(DocumentsTypeRange), typeof(CiaDocumentsTypeRange).Name);
            selectQuery.AddSelectValue(new SelectValue(funtion, typeof(CiaDocumentsTypeRange).Name));

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    entityCiaDocumentTypeRange.DocumentsTypeRangeCode = (Convert.ToInt32(reader[0]));
                }
            }

            DataFacadeManager.Insert(entityCiaDocumentTypeRange);

            return ModelAssembler.CreateCiaDocumentTypeRange(entityCiaDocumentTypeRange);
        }
        internal Models.CiaDocumentTypeRange UpdateCiaDocumentTypeRange(Models.CiaDocumentTypeRange ciaDocumentTypeRange)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CiaDocumentsTypeRange.Properties.DocumentsTypeRangeCode, typeof(CiaDocumentsTypeRange).Name);
            filter.Equal();
            filter.Constant(ciaDocumentTypeRange.DocumentTypeRange);

            CiaDocumentsTypeRange CiaDocumentsTypeRangeEntity = (CiaDocumentsTypeRange)DataFacadeManager.Instance.GetDataFacade().List(typeof(CiaDocumentsTypeRange), filter.GetPredicate()).FirstOrDefault();

            if (CiaDocumentsTypeRangeEntity != null)
            {
                CiaDocumentsTypeRangeEntity.IndividualTypeCode = ciaDocumentTypeRange.IndividualTypeId;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(CiaDocumentsTypeRangeEntity);
                return ModelAssembler.CreateCiaDocumentTypeRange(CiaDocumentsTypeRangeEntity);
            }

            return CreateCiaDocumentTypeRange(ciaDocumentTypeRange);
        }
    }
}

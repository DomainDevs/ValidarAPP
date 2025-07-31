using System;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Collections.Generic;
using Sistran.Core.Framework.Queries;
using System.Data;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class ProspectNaturalBusiness
    {

        public List<ProspectNatural> GetProspectNaturalByDocument(string documentNumber)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.Prospect.Properties.IdCardNo, typeof(UniquePersonV1.Entities.Prospect).Name);
            filter.Equal();
            filter.Constant(documentNumber);
            var prospectCollection = DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.Prospect), filter.GetPredicate());
            return ModelAssembler.CreateNaturalProspects(prospectCollection);
        }


        internal ProspectNatural UpdateProspectNatural(ProspectNatural prospectNatural)
        {
            UniquePersonV1.Entities.Prospect ProspectEntity = EntityAssembler.CreateProspectNatural(prospectNatural);
            DataFacadeManager.Update(ProspectEntity);
            return ModelAssembler.CreateModelProspect(ProspectEntity);
        }

        internal ProspectNatural CreateProspectNatural(ProspectNatural prospectNatural)
        {
            UniquePersonV1.Entities.Prospect ProspectEntity = EntityAssembler.CreateProspectNatural(prospectNatural);

            SelectQuery selectQuery = new SelectQuery();
            Function funtion = new Function(FunctionType.Max);
            funtion.AddParameter(new Column(UniquePersonV1.Entities.Prospect.Properties.ProspectId));
            selectQuery.Table = new ClassNameTable(typeof(UniquePersonV1.Entities.Prospect), "ProspectId");
            selectQuery.AddSelectValue(new SelectValue(funtion));

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    ProspectEntity.ProspectId = (Convert.ToInt32(reader[0]) + 1);
                }
            }

            DataFacadeManager.Insert(ProspectEntity);
            CreateCoProspect(ProspectEntity.ProspectId);
            return ModelAssembler.CreateModelProspect(ProspectEntity);
        }
        internal void CreateCoProspect(int ProspectId)
        {
           CoProspect CoprospectNaturalentity = EntityAssembler.CreateCoProspect(ProspectId);
           DataFacadeManager.Instance.GetDataFacade().InsertObject(CoprospectNaturalentity);
        }
        

    }
}

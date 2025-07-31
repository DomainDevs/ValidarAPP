using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.CommonService.Assemblers;
using Sistran.Core.Framework.Queries;

namespace Sistran.Company.Application.CommonServices.EEProvider.DAOs
{
    public class NomenclatureDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomenclature"></param>
        /// <returns></returns>
        public List<Nomenclature> GetNomenclatures()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoNomenclatures)));
            return ModelAssembler.CreateNomenclatures(businessCollection);
        }

        public List<Nomenclature> GetNomenclaturesTask(int id, string Nomenclatures, string Abreviature, bool getAllData)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoNomenclatures.Properties.Nomenclatura, typeof(CoNomenclatures).Name);
            filter.Like();
            filter.Constant("%" + Nomenclatures + "%");
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoNomenclatures), filter.GetPredicate()));
            return ModelAssembler.CreateNomenclatures(businessCollection);
        }

        public List<Nomenclature> GetNomenclatures(string Nomenclatures)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoNomenclatures.Properties.Nomenclatura, typeof(CoNomenclatures).Name);
            filter.Equal();
            filter.Constant(Nomenclatures);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoNomenclatures), filter.GetPredicate()));
            return ModelAssembler.CreateNomenclatures(businessCollection);
        }
    }
}

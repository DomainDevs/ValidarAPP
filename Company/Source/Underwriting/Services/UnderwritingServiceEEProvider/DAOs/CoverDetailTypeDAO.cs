using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using QUOEN = Sistran.Core.Application.Quotation.Entities;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class CoverDetailTypeDAO
    {
        private IDataFacade _dataFacade;
        public CoverDetailTypeDAO(IDataFacade dataFacade)
        {
            _dataFacade = dataFacade;
        }

        /// <summary>
        /// Busca la informacion de los tipos de detalle asociados a la cobertura
        /// </summary>
        /// <param name="coverageId">Id cobertura</param>
        /// <returns>Detalles asociados a la cobertura</returns>
        public List<CoverDetailType> GetCoverDetailTypesByCoverageId(int coverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(QUOEN.CoverDetailType.Properties.CoverageId, typeof(QUOEN.CoverDetailType).Name);
            filter.Equal();
            filter.Constant(coverageId);

            CoverageDetailTypeView view = new CoverageDetailTypeView();
            ViewBuilder builder = new ViewBuilder("CoverageDetailTypeView");
            builder.Filter = filter.GetPredicate();
            
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            List<CoverDetailType> coverDetailTypes = ModelAssembler.CreateCoverDetailTypes(view.CoverDetailTypes);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs.GetDetailTypeByCoverageId");
            return coverDetailTypes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTMO = Sistran.Core.Application.Utilities.Error;
using Sistran.Core.Application.UnderwritingParamService.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using INTEN = Sistran.Core.Application.Integration.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingParamService.EEProvider.Resources;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    public class Coverage2GDAO
    {
        public UTMO.Result<List<ParamCoCoverage2G>, UTMO.ErrorModel> GetCoCoverages2GByInsuredObject(int idInsuredObject)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(INTEN.CoCoverage2g.Properties.InsuredObject, typeof(INTEN.CoCoverage2g).Name);
                filter.Equal();
                filter.Constant(idInsuredObject);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(INTEN.CoCoverage2g), filter.GetPredicate()));
                return new UTMO.ResultValue<List<ParamCoCoverage2G>, UTMO.ErrorModel>(ModelAssembler.CreateCoCoverage2G(businessCollection));
            }
            catch (Exception ex)
            {
                return new UTMO.ResultError<List<ParamCoCoverage2G>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorGetCoverages2G }, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
        }

        public UTMO.Result<bool, UTMO.ErrorModel> CreateHomologationCoverage(ParamCoverage paramCoverage, int coverageId3g)
        {
            try
            {
                INTEN.CiaEquivalenceCoverage ciaEquivalenceCoverage = EntityAssembler.CreateCiaEquivalenceCoverage(paramCoverage, coverageId3g);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(ciaEquivalenceCoverage);
                return new UTMO.ResultValue<bool, UTMO.ErrorModel>(true);
            }
            catch (Exception ex)
            {
                return new UTMO.ResultError<bool, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorCreateCoverageHomologation2G }, Utilities.Enums.ErrorType.TechnicalFault, ex));

            }
        }

        public UTMO.Result<bool, UTMO.ErrorModel> UpdateHomologationCoverage(ParamCoverage paramCoverage)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(INTEN.CiaEquivalenceCoverage.Properties.CoverageId3g, typeof(INTEN.CiaEquivalenceCoverage).Name);
                filter.Equal();
                filter.Constant(paramCoverage.Id);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(INTEN.CiaEquivalenceCoverage), filter.GetPredicate()));
                if(businessCollection.Count > 0)
                {
                    INTEN.CiaEquivalenceCoverage ciaEquivalenceCoverage = businessCollection.Cast<INTEN.CiaEquivalenceCoverage>().FirstOrDefault();
                    ciaEquivalenceCoverage.CoverageId2g = paramCoverage.Homologation2G.Id;
                    ciaEquivalenceCoverage.InsuredObject2g = paramCoverage.Homologation2G.InsuredObjectId;
                    ciaEquivalenceCoverage.LineBusinessCode = paramCoverage.Homologation2G.LineBusinessId;
                    ciaEquivalenceCoverage.SubLineBusinessCode = paramCoverage.Homologation2G.SubLineBusinessId;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(ciaEquivalenceCoverage);
                }
                return new UTMO.ResultValue<bool, UTMO.ErrorModel>(true);
            }
            catch (Exception ex)
            {
                return new UTMO.ResultError<bool, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorUpdateCoverageHomologate2G }, Utilities.Enums.ErrorType.TechnicalFault, ex));

            }
        }

        public UTMO.Result<bool, UTMO.ErrorModel> DeleteHomologationCoverage(ParamCoverage paramCoverage)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(INTEN.CiaEquivalenceCoverage.Properties.CoverageId3g, typeof(INTEN.CiaEquivalenceCoverage).Name);
                filter.Equal();
                filter.Constant(paramCoverage.Id);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(INTEN.CiaEquivalenceCoverage), filter.GetPredicate()));
                if (businessCollection.Count > 0)
                {
                    INTEN.CiaEquivalenceCoverage ciaEquivalenceCoverage = businessCollection.Cast<INTEN.CiaEquivalenceCoverage>().FirstOrDefault();
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(ciaEquivalenceCoverage);
                }
                return new UTMO.ResultValue<bool, UTMO.ErrorModel>(true);
            }
            catch (Exception ex)
            {
                return new UTMO.ResultError<bool, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorDeleteCoverageHomologation2G }, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
        }

    }
}

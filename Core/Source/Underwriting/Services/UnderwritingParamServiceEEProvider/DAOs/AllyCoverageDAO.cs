using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingParamService.Models;
using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Error;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    public class AllyCoverageDAO
    {
        public ParamQueryCoverage CreateAllyCoverage(ParamQueryCoverage paramQueryAllyCoverage)
        {
            try
            {
                AllyCoverage entity = EntityAssembler.CreateParamAllyCoverage(paramQueryAllyCoverage);//CreateParamCoCoverageValue(paramQueryAllyCoverage);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.InsertObject(entity);
                }

                return paramQueryAllyCoverage;
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message, e);
            }

            //try
            //{
            //    //int maxId = FindCity(companyParamCity.Country.Id, companyParamCity.State.Id);
            //    //City entity = EntityAssembler.CreateParamCity(companyParamCity, maxId);
            //    //using (var daf = DataFacadeManager.Instance.GetDataFacade())
            //    //{
            //    //    daf.InsertObject(entity);
            //    //}

            //    //if (entity.CityCode != 0)
            //    //{
            //    //    companyParamCity.Id = entity.CityCode;
            //    //    return companyParamCity;
            //    //}
            //    //else
            //    //{
            //    //    throw new BusinessException("Error en GetProductsByScriptId ");
            //    //}

            //}
            //catch (Exception ex)
            //{
            //    throw new BusinessException(ex.Message, ex);
            //}

        }

        public ParamQueryCoverage DeleteAllyCoverage(ParamQueryCoverage paramQueryAllyCoverage)
        {
            try
            {
                //AllyCoverage entity = EntityAssembler.CreateParamAllyCoverage(paramQueryAllyCoverage);//CreateParamCoCoverageValue(paramQueryAllyCoverage);

                PrimaryKey primaryKey = AllyCoverage.CreatePrimaryKey(paramQueryAllyCoverage.AllyCoverage.Id, paramQueryAllyCoverage.CoveragePrincipal.Id);
                var ally_coverage_entity = (AllyCoverage)DataFacadeManager.GetObject(primaryKey);

                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.DeleteObject(ally_coverage_entity);//InsertObject(entity);
                }
                return paramQueryAllyCoverage;
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message, e);
            }

        }

        /// <summary>
        /// retorna el consecutivo correspondiente a la combinacion de 
        /// country y state, el id generado se usa para insertar la cobertura
        /// </summary>
        /// <param name="coverageId"></param>
        /// <param name="coverageAllyId"></param>
        /// <returns></returns>
        public int FindAllyCoverage(int coverageId, int coverageAllyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(AllyCoverage.Properties.AllyCoverageId, coverageAllyId);
            filter.And();
            filter.PropertyEquals(AllyCoverage.Properties.CoverageId, coverageId);
            System.Collections.IList alliesList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AllyCoverage), filter.GetPredicate(), null));
            List<ParamQueryCoverage> allies = ModelAssembler.ConvertToModelAllyCoverage(alliesList);//ModelAssembler.ConvertToModelCities(cityList);
            return allies.Max(x => x.Id) + 1;

        }

        public ParamQueryCoverage UpdateAllyCoverage(ParamQueryCoverage allyCoverage, ParamQueryCoverage allyCoverageOld)
        {
            try
            {
                PrimaryKey primaryKey = AllyCoverage.CreatePrimaryKey(allyCoverageOld.AllyCoverage.Id, allyCoverageOld.CoveragePrincipal.Id);
                var ally_coverage_entity = (AllyCoverage)DataFacadeManager.GetObject(primaryKey);

                ally_coverage_entity.CoveragePercentage = allyCoverage.CoveragePercentage;
                DataFacadeManager.Update(ally_coverage_entity);
                return allyCoverage;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            
        }
        
        ParamQueryCoverage GetByDescriptionAllyCoverage(ParamQueryCoverage allyCoverage)
        {
            throw new Exception();
        }

        string DeleteAllyCoverage(int pos)
        {
            throw new Exception();
        }

        public List<ParamQueryCoverage>GetAllAllyCoverage()
        {
            List<string> errorListDescription = new List<string>();
            try
            {
                List<ParamQueryCoverage> listCompanyParamCity = new List<ParamQueryCoverage>();
                //CoCoverageValueView view = new CoCoverageValueView();
                //ViewBuilder builder = new ViewBuilder("CoCoverageValueView");

                //DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                //foreach (CoCoverageValue coCoverageValue in view.CoCoverageValue)
                //{
                //    QUOTATIONENTITIES.Coverage coverage = view.Coverage.Cast<QUOTATIONENTITIES.Coverage>().First(x => x.CoverageId == coCoverageValue.CoverageId);
                //    COMMON.Prefix prefix = view.Prefix.Cast<COMMON.Prefix>().First(x => x.PrefixCode == coCoverageValue.PrefixCode);

                //    ParamCoCoverageValue resultCity = ModelAssembler.CreateCoCoverageValue(coverage, prefix, coCoverageValue);
                //    listCompanyParamCity.Add(resultCity);
                //}

                return listCompanyParamCity;
            }

            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        List<ParamQueryCoverage>GetAdvAllyCoverage(ParamQueryCoverage allyCoverage)
        {
            throw new Exception();
        }

        public string GenerateExcelAllyCoverage(string fileName)
        {
            var CoCoverageValues = GetAllAllyCoverage();
            //FileDAO fileDAOs = new FileDAO();
            //return fileDAOs.GenerateFileToCoCoverageValue(CoCoverageValues, fileName);
            return "";
        }
        

    }
}

using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Diagnostics;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using QUOTATIONENTITIES= Sistran.Core.Application.Quotation.Entities;

using System.Data;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using COMMON=Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class CoCoverageValueDAO
    {
        /// <summary>
        /// CreateCoCoverageValue: metodo que inserta una cobertura- table QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        public ParamCoCoverageValue CreateCoCoverageValue (ParamCoCoverageValue paramCoCoverageValue)
        {
            try
            {
                CoCoverageValue entity = EntityAssembler.CreateParamCoCoverageValue(paramCoCoverageValue);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.InsertObject(entity);
                }
               
                return paramCoCoverageValue;
            }
            catch(Exception e)
            {
                throw new BusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// UpdateCoCoverageValue metodo que actualiza la informaaicon de una cobertura - table:QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        public ParamCoCoverageValue UpdateCoCoverageValue (ParamCoCoverageValue paramCoCoverageValue)
        {
            try
            {
                PrimaryKey primaryKey = CoCoverageValue.CreatePrimaryKey(paramCoCoverageValue.Prefix.Id, paramCoCoverageValue.Coverage.Id);
                var coCoverageEntity = (CoCoverageValue)DataFacadeManager.GetObject(primaryKey);
                coCoverageEntity.ValuePje = paramCoCoverageValue.Percentage;
                DataFacadeManager.Update(coCoverageEntity);
                return paramCoCoverageValue;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetByDescriptionCoCoverageValue: metodo que consulta el listado de coberturas a partir de la descripcion ingresada en la busqueda simple- table:QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        public List<ParamCoCoverageValue> GetCoCoverageValueByPrefixId(int prefixId)
        {            
            List<string> errorListDescription = new List<string>();
            try
            {
                List<ParamCoCoverageValue> listCompanyParamCity = new List<ParamCoCoverageValue>();
                CoCoverageValueView view = new CoCoverageValueView();
                ViewBuilder builder = new ViewBuilder("CoCoverageValueView");

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(CoCoverageValue.Properties.PrefixCode, typeof(CoCoverageValue).Name);
                filter.Equal();
                filter.Constant(prefixId);

                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                foreach (CoCoverageValue coCoverageValue in view.CoCoverageValue)
                {
                    QUOTATIONENTITIES.Coverage coverage = view.Coverage.Cast<QUOTATIONENTITIES.Coverage>().First(x => x.CoverageId == coCoverageValue.CoverageId);
                    COMMON.Prefix prefix = view.Prefix.Cast<COMMON.Prefix>().First(x => x.PrefixCode == coCoverageValue.PrefixCode);

                    ParamCoCoverageValue resultCity = ModelAssembler.CreateCoCoverageValue(coverage, prefix, coCoverageValue);
                    listCompanyParamCity.Add(resultCity);
                }
              
                return listCompanyParamCity;
            }

            catch (System.Exception ex)
            {                
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// DeleteCoCoverageValue: metodo que elimina un valor de cobertura - table:QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        public bool DeleteCoCoverageValue(ParamCoCoverageValue paramCoCoverageValue)
        {
            try
            {
                PrimaryKey primaryKey = CoCoverageValue.CreatePrimaryKey(paramCoCoverageValue.Prefix.Id,paramCoCoverageValue.Coverage.Id);
                bool result = DataFacadeManager.Delete(primaryKey);               
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetAllCoCoverageValue: metodo que consulta el listado completo de valores de coberturas - table:QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <returns></returns>
        public List<ParamCoCoverageValue> GetAllCoCoverageValue()
        {
            List<string> errorListDescription = new List<string>();
            try
            {
                List<ParamCoCoverageValue> listCompanyParamCity = new List<ParamCoCoverageValue>();
                CoCoverageValueView view = new CoCoverageValueView();
                ViewBuilder builder = new ViewBuilder("CoCoverageValueView");                              

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                foreach (CoCoverageValue coCoverageValue in view.CoCoverageValue)
                {
                    QUOTATIONENTITIES.Coverage coverage = view.Coverage.Cast<QUOTATIONENTITIES.Coverage>().First(x => x.CoverageId == coCoverageValue.CoverageId);
                    COMMON.Prefix prefix = view.Prefix.Cast<COMMON.Prefix>().First(x => x.PrefixCode == coCoverageValue.PrefixCode);

                    ParamCoCoverageValue resultCity = ModelAssembler.CreateCoCoverageValue(coverage, prefix, coCoverageValue);
                    listCompanyParamCity.Add(resultCity);
                }

                return listCompanyParamCity;
            }

            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetAdvCoCoverageValue metodo que obtiene el listado de coberturas a partir de los filtros ingresados en la busqueda avanzada- table:QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        public List<ParamCoCoverageValue> GetAdvCoCoverageValue(ParamCoCoverageValue paramCoCoverageValue)
        {
            List<string> errorListDescription = new List<string>();
            try
            {
                List<ParamCoCoverageValue> listCompanyParamCity = new List<ParamCoCoverageValue>();
                CoCoverageValueView view = new CoCoverageValueView();
                ViewBuilder builder = new ViewBuilder("CoCoverageValueView");

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(CoCoverageValue.Properties.PrefixCode, typeof(CoCoverageValue).Name);
                filter.Equal();
                filter.Constant(paramCoCoverageValue.Prefix.Id);
                if(paramCoCoverageValue.Coverage.Id>0)
                {
                    filter.And();
                    filter.Property(CoCoverageValue.Properties.CoverageId, typeof(CoCoverageValue).Name);
                    filter.Equal();
                    filter.Constant(paramCoCoverageValue.Coverage.Id);
                }               

                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                foreach (CoCoverageValue coCoverageValue in view.CoCoverageValue)
                {
                    QUOTATIONENTITIES.Coverage coverage = view.Coverage.Cast<QUOTATIONENTITIES.Coverage>().First(x => x.CoverageId == coCoverageValue.CoverageId);
                    COMMON.Prefix prefix = view.Prefix.Cast<COMMON.Prefix>().First(x => x.PrefixCode == coCoverageValue.PrefixCode);

                    ParamCoCoverageValue resultCity = ModelAssembler.CreateCoCoverageValue(coverage, prefix, coCoverageValue);
                    listCompanyParamCity.Add(resultCity);
                }
                return listCompanyParamCity;
            }

            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GenerateExcelCoCoverageValue: metodo que genera el archivo excel del listado de coberturas - table:QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <returns></returns>
        public string GenerateExcelCoCoverageValue(string fileName)
        {
            var CoCoverageValues = GetAllCoCoverageValue();
            FileDAO fileDAOs = new FileDAO();
            return fileDAOs.GenerateFileToCoCoverageValue(CoCoverageValues, fileName);
        }

        /// <summary>
        /// GetCoverageByPrefixId: metodo que consulta el listado de coberturas por prefix y linebusiness
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        public List<BaseParamCoverage> GetCoverageByPrefixId(int prefixId)
        {
            try
            {                
                List<BaseParamCoverage> Listcoverages = new List<BaseParamCoverage>();
                CoCoverageValueView view = new CoCoverageValueView();              

                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(QUOTATIONENTITIES.Coverage.Properties.CoverageId, "c")));
                select.AddSelectValue(new SelectValue(new Column(QUOTATIONENTITIES.Coverage.Properties.PrintDescription, "c")));
               
                Join join = new Join(new ClassNameTable(typeof(COMMON.Prefix), "a"), new ClassNameTable(typeof(COMMON.PrefixLineBusiness), "b"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(COMMON.Prefix.Properties.PrefixCode, "a")
                    .Equal()
                    .Property(COMMON.PrefixLineBusiness.Properties.PrefixCode, "b")
                    .GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(QUOTATIONENTITIES.Coverage), "c"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(COMMON.PrefixLineBusiness.Properties.LineBusinessCode, "b")
                    .Equal()
                    .Property(QUOTATIONENTITIES.Coverage.Properties.LineBusinessCode, "c")
                    .GetPredicate());

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMON.Prefix.Properties.PrefixCode, "a");
                filter.Equal();
                filter.Constant(prefixId);

                select.Table = join;
                select.Where = filter.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        Listcoverages.Add(new BaseParamCoverage()
                        {
                           Id= Convert.ToInt32(reader["CoverageId"]),
                           Description= Convert.ToString(reader["PrintDescription"]),
                        });
                    }
                }
                return Listcoverages;
            }

            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}

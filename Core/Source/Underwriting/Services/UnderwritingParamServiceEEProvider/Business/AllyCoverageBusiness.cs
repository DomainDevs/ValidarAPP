using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingParamService.Models;
using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Business
{
    public class AllyCoverageBusiness
    {
        public ParamQueryCoverage UpdateBusinessAllyCoverage(ParamQueryCoverage allyCoverage, ParamQueryCoverage allyCoverageOld)
        {
            try
            {
                AllyCoverageDAO ally_coverage_DAO = new AllyCoverageDAO();
                return ally_coverage_DAO.UpdateAllyCoverage(allyCoverage, allyCoverageOld);
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message);
            }
        }

        public ParamQueryCoverage CreateBusinessAllyCoverage(ParamQueryCoverage allyCoverage)
        {
            try
            {
                AllyCoverageDAO ally_coverage_DAO = new AllyCoverageDAO();
                return ally_coverage_DAO.CreateAllyCoverage(allyCoverage);//UpdateAllyCoverage(allyCoverage);
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message);
            }
        }

        public ParamQueryCoverage DeleteBusinessAllyCoverage(ParamQueryCoverage allyCoverage)
        {
            try
            {
                AllyCoverageDAO ally_coverage_DAO = new AllyCoverageDAO();
                return ally_coverage_DAO.DeleteAllyCoverage(allyCoverage);//CreateAllyCoverage(allyCoverage);//UpdateAllyCoverage(allyCoverage);
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message);
            }
        }

        public string GenerateFileToAllyCoverage(string fileName)
        {
            AllyCoverageDAO allyCoverageDao = new AllyCoverageDAO();
            return allyCoverageDao.GenerateExcelAllyCoverage(fileName);//GenerateExcelCoCoverageValue(fileName);
        }
        //List<ParamQueryCoverage>
        public string GenerateFileToAllyCoverage(List<ParamQueryCoverage> li_paramquery, string fileName)
        {
            FileDAO fileDao = new FileDAO();
            return fileDao.GenerateFileToAllyCoverage(li_paramquery, fileName);//GenerateExcelAllyCoverage(fileName);
        }

    }
}

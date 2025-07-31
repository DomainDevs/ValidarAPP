using Sistran.Company.Application.PrintingServices.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Clases
{
    public interface IRisk
    {
        DataSet RiskData { get; set; }

        void printRisk(DataSet _risk);
        void printRisk(DataSet _risk, ArrayList _file, Policy _policyData, int _actRiskNum, int _coveredRiskType, out int _pageLinesCount, out int _limitPageLines, out int _pageAnexNum);
    }
}

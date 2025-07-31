using Sistran.Core.Services.UtilitiesServices.Models;
using Utilities.Excel.Models;

namespace Utilities.Excel
{
    public class ExcelService
    {
        public void CreateListRiskReport(File file, string path)
        {
            ExcelBuilder excelBuilder = new ExcelBuilder();
            excelBuilder.CreateExcelFile(file, path);
        }

    }
}

using Sistran.Company.Application.ModelServices.Models.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ModelServices.Models.Reports
{
    public class ScoreCreditsServiceModel : ErrorServiceModel
    {
        public List<ScoreCreditServiceModel> scoreCredits { get; set; }
        public ScoreCreditValidServiceModel scoreCreditValid { get; set; }
    }
}

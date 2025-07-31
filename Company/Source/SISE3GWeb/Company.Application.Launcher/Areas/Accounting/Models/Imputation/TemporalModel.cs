using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation
{
    [KnownType("TemporalModel")]
    public class TemporalModel
    {
        public int TempImputationId { get; set; }
        public int ImputationTypeId { get; set; }
        public int SourceId { get; set; }
    }

    [KnownType("ItemsToDeleteModel")]
    public class ItemsToDeleteModel
    {
        public List<TemporalModel> Temporals { get; set; }
    }
}
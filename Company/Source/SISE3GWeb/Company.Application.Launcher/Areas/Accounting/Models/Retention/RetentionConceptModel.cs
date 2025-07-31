
namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Retention
{
    public class RetentionConceptModel
    {

        public int Id { get; set; }
        public string Description { get; set; }
        public int RetentionBaseId { get; set; }
        public string RetentionBaseDescription { get; set; }
        public int StatusId { get; set; }
        public string StatusDescription { get; set; }
        public string Separator { get; set; }
        public int RetentionId { get; set; }
    }
}
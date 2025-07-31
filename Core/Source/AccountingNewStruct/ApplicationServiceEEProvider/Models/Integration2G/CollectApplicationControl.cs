namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Integration2G
{
    public class CollectApplicationControl
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Application id or collect id
        /// </summary>
        public int CollectApplicationId { get; set; }

        /// <summary>
        /// Origin screen, 1 default
        /// </summary>
        public int Origin { get; set; }

        /// <summary>
        /// Idenfier of application or collect
        /// </summary>
        public int AppSource { get; }

        /// <summary>
        /// Indicates wheter update (U) or insert (I)
        /// </summary>
        public string Action { get; set; }

        public CollectApplicationControl()
        {
            AppSource = 1;
        }
    }
}

namespace Sistran.Core.Application.UniquePersonListRiskBusinessServiceProvider
{
    public static class Initializator
    {
        static Initializator()
        {

        }

        public static void LoadOnMemoryListRisks()
        {
            UniquePersonListRiskBusinessServiceEEProvider provider = new UniquePersonListRiskBusinessServiceEEProvider();
            provider.LoadOnMemoryListRisks("SISE3G_SYSTEM");
        }
    }
}

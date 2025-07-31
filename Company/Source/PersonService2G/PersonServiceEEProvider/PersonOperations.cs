using PersonService;
using PersonServiceEEProvider.Business;

namespace PersonServiceEEProvider
{
    public static class PersonOperations
    {       

        public static IPersonService GetPersonOperations()
        {
            return new PersonBusiness();
        }
    }
}

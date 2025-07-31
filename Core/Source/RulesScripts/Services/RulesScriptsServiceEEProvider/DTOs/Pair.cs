namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    // Summary:
    //     Provides a basic utility class that is used to store two related objects.

    public  class Pair
    {
        // Summary:
        //     Gets or sets the first object of the object pair.
        public object First {get;set;}
        //
        // Summary:
        //     Gets or sets the second object of the object pair.
        public object Second {get; set;}

        //
        // Summary:
        //     Initializes a new instance of the System.Web.UI.Pair class, using the specified
        //     object pair.
        //
        // Parameters:
        //   x:
        //     An object.
        //
        //   y:
        //     An object.
        public Pair(object x, object y)
        {
            First = x;
            Second = y;
        }
    }
}

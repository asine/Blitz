using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Agatha.Common.WCF;

namespace Blitz.Common.Agatha
{
    public static class AgathaKnownTypeRegistration
    {
        public static void RegisterWCFAgathaTypes(Assembly assembly)
        {
            // Taken from - http://www.xavierdecoster.com/post/2010/05/07/Automate-WCF-KnownTypes-in-a-3-Tier-Silverlight-application.aspx

            // get all public types
            var publicTypes = assembly.GetExportedTypes();

            // get all data contracts
            var dataContracts = publicTypes.Where(type => type.GetCustomAttributes(typeof(DataContractAttribute), true).Length > 0);

            // register all data contracts to WCF (using Agatha)
            // notice that we have to provide a base type to Agatha's KnownTypeProvider (it does not support interfaces...), so what the heck, let's use object :-)
            KnownTypeProvider.RegisterDerivedTypesOf<object>(dataContracts);
        }
    }
}
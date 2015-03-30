using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EPS.IoC.ServiceLocation
{
    public abstract class CommonBootStrapper
    {
        public static IServiceLocator Locator;

        protected CommonBootStrapper()
        {
            Locator = CreateServiceLocator();
        }

        protected abstract IServiceLocator CreateServiceLocator();
    }

    public class BootStrapperManager
    {
        private static CommonBootStrapper _bootStrapper;


        public static void Initialize(CommonBootStrapper bootStrapper)
        {
            _bootStrapper = bootStrapper;
        }

        public static CommonBootStrapper BootStrapper
        {
            get { return _bootStrapper; }
        }
    }
}

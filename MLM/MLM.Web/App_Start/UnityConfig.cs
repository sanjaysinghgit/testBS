using Microsoft.Practices.Unity;
using MLM.DB;
using MLM.Models;
using System.Web.Http;
using Unity.WebApi;

namespace MLM.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
    //        container.RegisterType(typeof(IRepository<>), typeof(RepositoryBase<>)
    //, new PerResolveLifetimeManager()
    //, new InjectionConstructor(new ResolvedParameter<DatabaseFactory>()
    //    , new ResolvedParameter<ConnectionRetriever>()));

    //        container.RegisterType(typeof(RepositoryBase<MLMBaseEntity>), typeof(MLMSqlRepository<MLMBaseEntity>)
    //            , new PerResolveLifetimeManager()
    //            , new InjectionConstructor(
    //                new ResolvedParameter<DatabaseFactory>(),
    //                new ResolvedParameter<ConnectionRetriever>()));

    //        container.RegisterType(typeof(AgentRepository<Agent>),
    //            new InjectionProperty("EntityRepository", new ResolvedParameter(typeof(IRepository<Agent>))));

    //        container.RegisterType(typeof(SPHandler<MLMBaseEntity>),
    //            new InjectionProperty("SQLRepository", new ResolvedParameter(typeof(MLMSqlRepository<MLMBaseEntity>))));
            

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
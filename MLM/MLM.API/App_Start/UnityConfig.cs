using Microsoft.Practices.Unity;
using MLM.API.DB;
using MLM.API.Models;
using System.Web.Http;
using Unity.WebApi;

namespace MLM.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            //container.RegisterType<IDatabaseFactory, DatabaseFactory>("MLMDatabaseFactory", new PerResolveLifetimeManager());

            //container.RegisterType(typeof(IRepository<>), typeof(RepositoryBase<>)
            //    , new PerResolveLifetimeManager()
            //    , new InjectionConstructor(new ResolvedParameter<IDatabaseFactory>("MLMDatabaseFactory")
            //        , new ResolvedParameter<IConnectionRetriever>()));

            //container.RegisterType(typeof(RepositoryBase<MLMBaseEntity>), typeof(MLMSqlRepository<MLMBaseEntity>)
            //    , new PerResolveLifetimeManager()
            //    , new InjectionConstructor(
            //        new ResolvedParameter<IDatabaseFactory>("MLMDatabaseFactory"),
            //        new ResolvedParameter<IConnectionRetriever>()));


            //container.RegisterType(typeof(AgentHandler<MLMBaseEntity>),
            //    new InjectionProperty("SQLRepository", new ResolvedParameter(typeof(MLMSqlRepository<MLMBaseEntity>), "MLMSqlRepository")));



            container.RegisterType(typeof(IRepository<>), typeof(RepositoryBase<>)
                , new PerResolveLifetimeManager()
                , new InjectionConstructor(new ResolvedParameter<DatabaseFactory>()
                    , new ResolvedParameter<ConnectionRetriever>()));

            container.RegisterType(typeof(RepositoryBase<MLMBaseEntity>), typeof(MLMSqlRepository<MLMBaseEntity>)
                , new PerResolveLifetimeManager()
                , new InjectionConstructor(
                    new ResolvedParameter<DatabaseFactory>(),
                    new ResolvedParameter<ConnectionRetriever>()));

            container.RegisterType(typeof(AgentHandler<Agent>),
                new InjectionProperty("EntityRepository", new ResolvedParameter(typeof(IRepository<Agent>))));

            container.RegisterType(typeof(SPHandler<MLMBaseEntity>),
                new InjectionProperty("SQLRepository", new ResolvedParameter(typeof(MLMSqlRepository<MLMBaseEntity>))));
            

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
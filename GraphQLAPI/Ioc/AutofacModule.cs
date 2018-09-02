using Autofac;
using GraphQLDemo.Services;
using Microsoft.Extensions.Logging;
using System.Reflection;
using GraphQL.Types;
using GraphQLDemo.GraphQL;
using System.Linq;
using GraphQLDemo.GraphQL.Types;
using GraphQL;
using GraphQLDemo.Types;

namespace GraphQLDemo.Ioc
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // The generic ILogger<TCategoryName> service was added to the ServiceCollection by ASP.NET Core.
            // It was then registered with Autofac using the Populate method in ConfigureServices.
            builder.Register(c => new ValuesService(c.Resolve<ILogger<ValuesService>>()))
                .As<IValuesService>()
                .InstancePerLifetimeScope();
            RegisterGraphQLTypes(builder);

            RegisterServices(builder);
            RegisterGraphQLSchema(builder);
            RegisterGraphQLQueries(builder);
            RegisterGraphQLResolvers(builder);
        }


        private void RegisterServices(ContainerBuilder builder)
        {
            var serviceAssembly = typeof(AutofacModule).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(serviceAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        private void RegisterGraphQLQueries(ContainerBuilder builder)
        {
            builder.RegisterType<DocumentExecuter>().As<IDocumentExecuter>();
            builder.RegisterType<GraphQLQuery>().AsSelf();
            builder.RegisterType<GraphQLMutation>().AsSelf();

        }

        private void RegisterGraphQLTypes(ContainerBuilder builder)
        {
            var serviceAssembly = typeof(AutofacModule).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(serviceAssembly)
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsAssignableFrom(typeof(IGraphQLType))))
                .AsSelf();

            builder.RegisterGeneric(typeof(ResponseGraphType<>)).AsSelf();
            builder.RegisterGeneric(typeof(ResponseListGraphType<>)).AsSelf();
        }

        private void RegisterGraphQLSchema(ContainerBuilder builder)
        {
            var serviceAssembly = typeof(AutofacModule).GetTypeInfo().Assembly;

            builder.RegisterType<GraphQLSchema>().As<ISchema>();
            builder.Register(c => {
                var context = c.Resolve<IComponentContext>();
                return new FuncDependencyResolver(type => (GraphType)context.Resolve(type));
            }).As<IDependencyResolver>();


        }

        private void RegisterGraphQLResolvers(ContainerBuilder builder)
        {
            var serviceAssembly = typeof(AutofacModule).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(serviceAssembly)
                .Where(t => t.Name.EndsWith("Query") && t.Name != "Query")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(serviceAssembly)
                .Where(t => t.Name.EndsWith("Mutation") && t.Name != "Mutation")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}

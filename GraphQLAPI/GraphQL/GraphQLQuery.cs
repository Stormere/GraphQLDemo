using GraphQL.Types;
using GraphQLDemo.GraphQL.Queries;
using System;
using System.Linq;

namespace GraphQLDemo.GraphQL
{
    public class GraphQLQuery : ObjectGraphType<object>
    {
        public GraphQLQuery(IServiceProvider serviceProvider)
        {
            var type = typeof(IQuery);
            var resolversTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            foreach (var resolverType in resolversTypes)
            {
                var resolverTypeInterface = resolverType.GetInterfaces().Where(x => x != type).FirstOrDefault();
                if (resolverTypeInterface != null)
                {
                    var resolver = serviceProvider.GetService(resolverTypeInterface) as IQuery;
                    resolver.Resolve(this);
                }
            }
        }
    }

}

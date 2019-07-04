using GraphQLDemo.GraphQL.Mutations;
using GraphQL.Types;
using System;
using System.Linq;

namespace GraphQLDemo.GraphQL
{
    public class GraphQLMutation : ObjectGraphType<object>
    {

        public GraphQLMutation(IServiceProvider serviceProvider)
        {
            var type = typeof(IMutation);
            var resolversTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            foreach (var resolverType in resolversTypes)
            {
                var resolverTypeInterface = resolverType.GetInterfaces().Where(x => x != type).FirstOrDefault();
                if (resolverTypeInterface != null)
                {
                    var resolver = serviceProvider.GetService(resolverTypeInterface) as IMutation;
                    resolver.Resolve(this);
                }
            }
        }
    }
}

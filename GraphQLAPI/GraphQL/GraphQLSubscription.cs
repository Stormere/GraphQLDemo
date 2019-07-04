using System;
using System.Linq;
using System.Reactive;
using GraphQL.Types;
using GraphQLDemo.GraphQL.Subscriptions;

namespace GraphQLDemo.GraphQL
{
    public class GraphQLSubscription: ObjectGraphType<object>
    {
        public GraphQLSubscription()
        {


        }

        public GraphQLSubscription(IServiceProvider serviceProvider)
        {
            var type = typeof(ISubscription);
            var resolversTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            foreach (var resolverType in resolversTypes)
            {
                var resolverTypeInterface = resolverType.GetInterfaces().Where(x => x != type).FirstOrDefault();
                if (resolverTypeInterface != null)
                {
                    var resolver = serviceProvider.GetService(resolverTypeInterface) as ISubscription;
                    resolver.Resolve(this);
                }
            }
        }
    }
}
